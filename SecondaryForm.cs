using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace IronMan
{
    public partial class SecondaryForm : Form
    {
        public MainForm parent;
        public DataTable configDataTable;
        public string processLine;
        public string process;

        public SecondaryForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parent.ExecuteSql("");
        }

        //public void SetProcess(string a)
        //{
        //    Text = a;
        //    ProcessLabel.Text = a;
        //}

        private DataGridView CreateDataGrid(TabPage tabPage, string subGroup)
        {
            // Get subgroup table
            HashSet<string> subGroupSet = new HashSet<string>();
            var groupDataTable = new DataTable();
            string groupSql = "select public.ironman_subgroup.subgroup, public.ironman_subgroup.field_names from public.ironman_subgroup where ";
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
                new DataGridViewTextBoxColumn{HeaderText="Judgement"},
                new DataGridViewTextBoxColumn{HeaderText="ActionSheetNo"}
            });

            // Create Fields
            DataRow groupRow = groupDataTable.Select(
                string.Format("subgroup = '{0}'", subGroup))[0];
            string[] groupFields = groupRow["field_names"].ToString().Split(';');
            // TODO Confirm that spaces should be removed
            foreach (string fieldName in groupFields)
                dataGrid.Rows.Add(new string[] { fieldName.Trim(' '), "-1", "-", "" });

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
                if (!itemArr[0].StartsWith(processLine))
                    row.Delete();
            };
            configDataTable.AcceptChanges();

            // Create tabs and datagrids
            if (configDataTable.Rows.Count == 1 && configDataTable.Rows[0]["sublevel"].ToString() == processLine)
            {
                string tabName = processLine;
                TabPage tabPage = new TabPage
                {
                    Text = tabName,
                    Name = tabName + "TabPage",
                    UseVisualStyleBackColor = true
                };

                tabPage.Controls.Add(CreateDataGrid(tabPage, configDataTable.Rows[0]["subgroup"].ToString()));
                //Must set padding after adding the dataGrid so it can resize the datagrid
                tabPage.Padding = new Padding(4);
                TabControl.Controls.Add(tabPage);
            }
            else
            {
                foreach (DataRow row in configDataTable.Rows)
                {
                    string tabName = row["sublevel"].ToString().Split('_')[1];
                    TabPage tabPage = new TabPage
                    {
                        Text = tabName,
                        Name = tabName + "TabPage",
                        UseVisualStyleBackColor = true
                    };

                    tabPage.Controls.Add(CreateDataGrid(tabPage, row["subgroup"].ToString()));
                    //Must set padding after adding the dataGrid so it can resize the datagrid
                    tabPage.Padding = new Padding(4);
                    TabControl.Controls.Add(tabPage);
                }
            }
        }

        private void SecondaryForm_Load(object sender, EventArgs e)
        {
            Text = process;
            ProcessLabel.Text = process;
            ProcessLineComboBox.Text = processLine;
            InititalizeTabs();
        }

        private void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string header = ((DataGridView)sender).Columns[e.ColumnIndex].HeaderText;
            if (header == "Entry")
            {
                double value = Convert.ToDouble(((DataGridView)sender)[e.ColumnIndex, e.RowIndex].Value);
                string field = ((DataGridView)sender)[0, e.RowIndex].Value.ToString();
                string sub_level = processLine + '_' + TabControl.SelectedTab.Text;

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
                        ((DataGridView)sender)[2, e.RowIndex].Value = "-";
                        return;
                    }
                    if (value >= min && value <= max)
                        ((DataGridView)sender)[2, e.RowIndex].Value = "ACCEPT";
                    else
                        ((DataGridView)sender)[2, e.RowIndex].Value = "REJECT";
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            //TODO
            return;
        }
    }
}
