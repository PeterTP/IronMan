using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IronMan
{
    public partial class SecondaryForm : Form
    {
        public MainForm parent;
        public DataTable configDataTable;
        public string[] processLine;
        public string processName;

        public SecondaryForm()
        {
            InitializeComponent();
        }

        //public void SetProcess(string a)
        //{
        //    Text = a;
        //    ProcessLabel.Text = a;
        //}

        private DataGridView CreateDataGrid(DataTabPage tabPage, string subGroup)
        {
            // Get subgroup table
            HashSet<string> subGroupSet = new HashSet<string>();
            var groupDataTable = new DataTable();
            string groupSql = "select public.ironman_subgroup.subgroup, public.ironman_subgroup.field_names, public.ironman_subgroup.field_units from public.ironman_subgroup where ";
            string groupTemplateSql = "public.ironman_subgroup.subgroup = '{0}'";

            foreach (DataRow configRow in configDataTable.Rows)
                subGroupSet.Add(string.Format(groupTemplateSql, configRow["subgroup"].ToString()));

            groupSql += string.Join(" or ", subGroupSet.ToArray());
            groupDataTable.Load(parent.ExecuteSql(groupSql));

            // Create datagrid and headers
            DataGridView dataGrid = new DataGridView
            {
                Name = tabPage.Text + "DataGrid",
                Size = tabPage.Size,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false
            };

            dataGrid.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn{HeaderText="Field"},
                new DataGridViewTextBoxColumn{HeaderText="Entry"},
                new DataGridViewTextBoxColumn{HeaderText="Units", ReadOnly=true},
                new DataGridViewTextBoxColumn{HeaderText="Judgement"},
                new DataGridViewTextBoxColumn{HeaderText="ActionSheetNo"}
            });

            // Create Fields
            DataRow groupRow = groupDataTable.Select(
                string.Format("subgroup = '{0}'", subGroup))[0];
            string[] groupFields = groupRow["field_names"].ToString().Split(';');
            string[] groupUnits = groupRow["field_units"].ToString().Split(';');

            for (int i = 0; i < groupFields.Length; i++)
                dataGrid.Rows.Add(new string[] { groupFields[i].Trim(' '), "-1", groupUnits[i].Trim(' '), "-", "" });

            dataGrid.CellEndEdit += new DataGridViewCellEventHandler(DataGridView_CellEndEdit);
            return dataGrid;
        }

        private void InititalizeTabs()
        {
            TabControl.Controls.Clear();

            // Delete irrelevant and duplicate tab names
            foreach (DataRow row in configDataTable.Rows)
            {
                string[] itemArr = row["sublevel"].ToString().Split('_');
                bool isValid = false;
                if (itemArr.Length-1 == processLine.Length)
                {
                    for (int i = 0; i < itemArr.Length - 1; i++)
                    {
                        if (itemArr[i] == processLine[i]) isValid = true;
                        break;
                    }
                }

                if (!isValid) row.Delete();
            };
            configDataTable.AcceptChanges();

            // Create tabs and datagrids
            if (configDataTable.Rows.Count == 1 && configDataTable.Rows[0]["sublevel"].ToString() == processLine[0])
            {
                string tabName = processLine[0];
                DataTabPage tabPage = new DataTabPage
                {
                    Text = tabName,
                    Name = tabName + "TabPage",
                    UseVisualStyleBackColor = true
                };

                DataGridView dataGrid = CreateDataGrid(tabPage, configDataTable.Rows[0]["subgroup"].ToString());
                tabPage.dataGrid = dataGrid;
                tabPage.Controls.Add(dataGrid);
                //Must set padding after adding the dataGrid so it can resize the datagrid
                tabPage.Padding = new Padding(4);
                TabControl.Controls.Add(tabPage);
            }
            else
            {
                foreach (DataRow row in configDataTable.Rows)
                {
                    string[] levels = row["sublevel"].ToString().Split('_');
                    string tabName = levels[levels.Length-1];
                    DataTabPage tabPage = new DataTabPage
                    {
                        Text = tabName,
                        Name = tabName + "TabPage",
                        UseVisualStyleBackColor = true
                    };

                    DataGridView dataGrid = CreateDataGrid(tabPage, row["subgroup"].ToString());
                    tabPage.dataGrid = dataGrid;
                    tabPage.Controls.Add(dataGrid);
                    //Must set padding after adding the dataGrid so it can resize the datagrid
                    tabPage.Padding = new Padding(4);
                    TabControl.Controls.Add(tabPage);
                }
            }
        }

        private void SecondaryForm_Load(object sender, EventArgs e)
        {
            Text = processName;
            ProcessLabel.Text = processName;
            ProcessLineComboBox.Text = processLine[0];
            if (configDataTable.Rows[0]["subgroup"].ToString() != "")
                InititalizeTabs();
            else
                TabControl.Dispose();
        }

        private void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string header = ((DataGridView)sender).Columns[e.ColumnIndex].HeaderText;
            if (header == "Entry" && (string)((DataGridView)sender)[e.ColumnIndex, e.RowIndex].Value != "")
            // ^^^ Fun fact: The 2nd comparison here is because empty fields get converted to 0
            {
                double value = Convert.ToDouble(((DataGridView)sender)[e.ColumnIndex, e.RowIndex].Value);
                string field = ((DataGridView)sender)[0, e.RowIndex].Value.ToString();
                string sub_level = string.Join("_", processLine);

                DataTable specDataTable = new DataTable();
                string specSql = string.Format("select * from public.ironman_specs where public.ironman_specs.sub_level = '{0}'", sub_level);
                specDataTable.Load(parent.ExecuteSql(specSql));
                if (specDataTable.Rows.Count > 0)
                {
                    DataRow specDataRow = specDataTable.Rows[0];
                    double min, max;

                    if (field.StartsWith("Sample Weight"))
                    {
                        min = (double)specDataRow["metal_weight_min"];
                        max = (double)specDataRow["metal_weight_max"];
                    }
                    else if (field.StartsWith("Metal Weight"))
                    {
                        min = (double)specDataRow["metal_weight_min"];
                        max = (double)specDataRow["metal_weight_max"];
                    }
                    else if (field.StartsWith("Metal Size"))
                    {
                        min = (double)specDataRow["metal_weight_min"];
                        max = (double)specDataRow["metal_weight_max"];
                    }
                    else
                    {
                        ((DataGridView)sender)[3, e.RowIndex].Value = "-";
                        return;
                    }
                    if (value >= min && value <= max)
                        ((DataGridView)sender)[3, e.RowIndex].Value = "ACCEPT";
                    else
                        ((DataGridView)sender)[3, e.RowIndex].Value = "REJECT";
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            //string insertSqlTemplate2 = "insert into public.ironman_data values (" +
            //    "'@ironman_process', " +
            //    "'@machine_id', " +
            //    "'@entry_date', " +
            //    "'@product_name', " +
            //    "'@lot_nos', " +
            //    "'@emp_id', " +
            //    "'@field', " +
            //    "'@vl', " +
            //    "'@unit', " +
            //    "'@judgement', " +
            //    "'@action_sheet')";

            //OdbcDataReader nowReader = parent.ExecuteSql("select now()::timestamp");
            //byte[] now = new byte[] {0};
            //while (nowReader.Read()) now[0] = nowReader.GetByte(0);
           
            string insertSqlTemplate = "insert into public.ironman_data values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')";

            Dictionary<string, string> dataResults = new Dictionary<string, string>
            {
                {"ironman_process", processName},
                {"machine_id", processLine[0]}, // TODO WARN Machine id?
                {"entry_date", DateTime.Now.ToString("s")},
                {"product_name", ProductNameComboBox.Text.ToString()},
                {"lot_nos", LotNoComboBox.Text.ToString()},
                {"emp_id", ""},
                {"field", ""},
                {"vl", ""},
                {"unit", ""},
                {"judgement", ""},
                {"action_sheet", ""}
            };

            try
            {
                foreach (DataTabPage page in TabControl.TabPages)
                {
                    DataGridView dataGrid = page.dataGrid;

                    foreach (DataGridViewRow row in dataGrid.Rows)
                    {
                        dataResults["field"] = row.Cells[0].Value.ToString();
                        dataResults["vl"] = row.Cells[1].Value.ToString();
                        dataResults["unit"] = row.Cells[2].Value.ToString();
                        dataResults["judgement"] = row.Cells[3].Value.ToString();
                        dataResults["action_sheet"] = row.Cells[4].Value.ToString();

                        string insertSql = String.Format(
                            insertSqlTemplate,
                            dataResults["ironman_process"],
                            dataResults["machine_id"],
                            dataResults["entry_date"],
                            dataResults["product_name"],
                            dataResults["lot_nos"],
                            dataResults["emp_id"],
                            dataResults["field"],
                            dataResults["vl"],
                            dataResults["unit"],
                            dataResults["judgement"],
                            dataResults["action_sheet"]
                        );

                        parent.ExecuteSql(insertSql);
                    }

                    //OdbcCommand cmd = new OdbcCommand(insertSqlTemplate, parent.postgreSQLConn);
                    //
                    //foreach (var data in dataResults)
                    //{
                    //    if (data.Key == "entry_date")
                    //    {
                    //        cmd.Parameters.Add('@' + data.Key, OdbcType.Timestamp);
                    //        cmd.Parameters['@' + data.Key].Value = now;
                    //    }
                    //    else
                    //    {
                    //        cmd.Parameters.Add('@' + data.Key, OdbcType.VarChar);
                    //        cmd.Parameters['@' + data.Key].Value = data.Value;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Saving Failed!" + ex);
            }
            finally
            {
                MessageBox.Show("Saved Successfully!");
            }
        }
    }

    public class DataTabPage : TabPage
    {
        public DataGridView dataGrid;
    }
}
