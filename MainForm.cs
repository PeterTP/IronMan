using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace IronMan
{
    public partial class MainForm : Form
    {
        public OdbcConnection postgreSQLConn, oracleConn;
        private DataTable currentDataTable;
        private string currentMainLevel;

        //private enum FormNames
        //{
        //    Main,
        //    RawMaterial,
        //    FormulationLine,
        //    KneadingLine,
        //    TablettingLine,
        //    SideProcess
        //};

        //public IEnumerable<Control> GetSelfAndChildrenRecursive(Control parent)
        //{
        //    List<Control> controls = new List<Control>();
        //    foreach (Control child in parent.Controls)
        //    {
        //        controls.AddRange(GetSelfAndChildrenRecursive(child));
        //    }
        //    controls.Add(parent);
        //    return controls;
        //}

        //protected override void WndProc(ref Message m)
        //{
        //    base.WndProc(ref m);
        //
        //    // WM_SYSCOMMAND
        //    if (m.Msg == 0x0112)
        //    {
        //        if (m.WParam == new IntPtr(0xF030) // Maximize event - SC_MAXIMIZE from Winuser.h
        //            || m.WParam == new IntPtr(0xF120)) // Restore event - SC_RESTORE from Winuser.h
        //        {
        //            MainForm_Resize(this, new EventArgs());
        //        }
        //    }
        //}

        public MainForm()
        {
            InitializeComponent();
        }

        private void CreateConn()
        //Make sure to download the odbc
        {
            if (!File.Exists("C:\\SDMM Automation Programs\\General.py"))
            {
                MessageBox.Show("Config File: General.py not found! Quitting.");
                Application.Exit();
            }

            string[] config = File.ReadAllLines("C:\\SDMM Automation Programs\\General.py");
            Dictionary<string, string> d = new Dictionary<string, string>
            {
                {"PG_HOST=", ""},
                {"PG_DATABASE=", ""},
                {"PG_USER=", ""},
                {"PG_PASSWORD=", ""},
                {"PG_PORT=", ""},
            };

            try
            {
                foreach (string line in config)
                {
                    foreach (string key in d.Keys)
                    {
                        if (line.StartsWith(key))
                        {
                            d[key] = line.Split('"')[1];
                            if (d[key] == "")
                            {
                                MessageBox.Show("Config Parameter Missing in General.py! Quitting.");
                                Application.Exit();
                            }
                            break;
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Config File Error in General.py! Quitting.");
                Application.Exit();
            }

            string connString = string.Format
            (
                "Server={0};Port={1};Uid={2};Pwd={3};Database={4}",
                d["PG_HOST="], d["PG_PORT="], d["PG_USER="], d["PG_PASSWORD="], d["PG_DATABASE="]
            );
            postgreSQLConn = new OdbcConnection("Driver={PostgreSQL Unicode};" + connString);
        }

        public OdbcDataReader ExecuteSql(string SQLText)
        {
            OdbcCommand cmd = new OdbcCommand(SQLText, postgreSQLConn);

            try
            {
                if (postgreSQLConn.State != ConnectionState.Open)
                    postgreSQLConn.Open();
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error, Cannot Connect to Server " + ex);
                return null;
            }
        }

        //public OdbcDataReader ExecuteSql(OdbcCommand cmd)
        //{
        //    try
        //    {
        //        if (postgreSQLConn.State != ConnectionState.Open)
        //            postgreSQLConn.Open();
        //        cmd.ExecuteNonQuery();
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error, Cannot Connect to Server " + ex);
        //        return null;
        //    }
        //}

        private void InstantiateMenu(Button button, string mainKey, string subKey)
        {
            string sql = string.Format("select public.ironman_menuconfig.sublevel, public.ironman_menuconfig.subgroup from public.ironman_menuconfig where public.ironman_menuconfig.mainlevel = '{0}'", mainKey);
            DataTable dt = new DataTable();
            dt.Load(ExecuteSql(sql));
            HashSet<string> itemSet = new HashSet<string>();

            currentMainLevel = mainKey;
            currentDataTable = dt;

            ButtonMenu.Items.Clear();
            if (subKey != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string[] levels = row["sublevel"].ToString().Split('_');
                    string line = levels[0];
                    if (line.StartsWith(subKey) && char.IsDigit(line[subKey.Length]))
                    {
                        ProcessMenuItem currentItem = null;

                        foreach (ProcessMenuItem item in ButtonMenu.Items)
                            if (item.Text == line) { currentItem = item; break; }

                        if (currentItem == null)
                        {
                            ProcessMenuItem lineItem = new ProcessMenuItem
                            {
                                Text = line,
                                processPath = line
                            };
                            ButtonMenu.Items.Add(lineItem);
                            currentItem = lineItem;
                        }

                        for (int i = 0; i < levels.Length - 2; i++) // -2 beacause we ignore last value as well
                        {
                            bool containsItem = false;

                            foreach (ProcessMenuItem item in currentItem.DropDownItems)
                                if (item.Text == levels[i + 1])
                                {
                                    containsItem = true;
                                    currentItem = item;
                                    break;
                                }

                            if (!containsItem)
                            {
                                ProcessMenuItem item = new ProcessMenuItem
                                {
                                    Text = levels[i + 1],
                                    processPath = currentItem.processPath + '_' + levels[i + 1]
                                };
                                if (i == levels.Length - 3) item.Click += CurrentItem_Click;
                                currentItem.DropDownItems.Add(item);
                                currentItem = item;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (DataRow row in dt.Rows)
                    itemSet.Add(row["sublevel"].ToString().Split('_')[0]);
            }
            ButtonMenu.Show(button, new Point(0, button.Height));
        }

        private void ResetColors()
        {
            Color bc = Color.WhiteSmoke;
            Color fc = Color.Black;

            foreach (Button button in ProcessTableLayout.Controls)
            {
                button.BackColor = bc;
                button.ForeColor = fc;
            }

            //foreach (Control cont in this.Controls)
            //{
            //    if (cont is GroupBox)
            //    {
            //        foreach (Control butt in cont.Controls)
            //        {
            //            if (butt is Button button)
            //            {
            //                button.BackColor = bc;
            //                button.ForeColor = fc;
            //            }
            //        }
            //    }
            //}
            //FormulationLinesGroupBox.Visible = false;
            //KneadingLinesGroupBox.Visible = false;
            //SideProcessGroupBox.Visible = false;
        }

        private void SetButtonProperties(Button button, GroupBox groupBox = null)
        {
            ResetColors();
            button.BackColor = Color.DarkCyan;
            button.ForeColor = Color.White;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ResetColors();
            CreateConn();
        }

        private void CurrentItem_Click(object sender, EventArgs e)
        {
            CreateForm(((ProcessMenuItem)sender).processPath);
        }

        private void ButtonMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ProcessMenuItem item = (ProcessMenuItem)e.ClickedItem;
            CreateForm(item.processPath);
        }

        private void CreateForm(string processPath)
        {
            Point formLocation = this.Location;
            formLocation.Offset(this.Size.Width, 0);

            SecondaryForm form = new SecondaryForm
            {
                Location = formLocation,
                parent = this,
                configDataTable = currentDataTable,
                processLine = processPath.Split('_'),
            };

            switch (currentMainLevel)
            {
                case "raw_material":
                    form.processName = "Raw Material";
                    break;
                case "formulation":
                    form.processName = "Formulation Line";
                    break;
                case "kneading":
                    form.processName = "Kneading Line";
                    break;
                case "tabletting":
                    form.processName = "Tabletting Line";
                    break;
                case "side_process":
                    form.processName = "Side Process";
                    break;
            }

            form.ShowDialog();
        }

        private void RawMaterialButton_Click(object sender, EventArgs e)
        {
            InstantiateMenu(RawMaterialButton, "raw_material", null);
            SetButtonProperties(RawMaterialButton);
        }

        private void FormulationLinesButton_Click(object sender, EventArgs e)
        {
            InstantiateMenu(FormulationLinesButton, "formulation", "F");
        }

        private void KneadingLinesButton_Click(object sender, EventArgs e)
        {
            InstantiateMenu(KneadingLinesButton, "kneading", "K");
        }

        private void TablettingLinesButton_Click(object sender, EventArgs e)
        {
            InstantiateMenu(KneadingLinesButton, "tabletting", null);
            SetButtonProperties(TablettingLinesButton);
        }

        private void SideProcessButton_Click(object sender, EventArgs e)
        {
            InstantiateMenu(SideProcessButton, "side_process", null);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

    public class ProcessMenuItem : ToolStripMenuItem
    {
        public string processPath = "";
    }
}
