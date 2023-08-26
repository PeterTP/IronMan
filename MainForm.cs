using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace IronMan
{
    public partial class MainForm : Form
    {
        private OdbcConnection postgreSQLConn, oracleConn;
        private OdbcDataReader currentReader;
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

        private void InstantiateMenu(Button button, string mainKey, string subKey)
        {
            string sql = string.Format("select public.ironman_menuconfig.sublevel, public.ironman_menuconfig.subgroup from public.ironman_menuconfig where public.ironman_menuconfig.mainlevel = '{0}'", mainKey);
            currentReader = ExecuteSql(sql);
            DataTable dt = new DataTable();
            dt.Load(currentReader);
            HashSet<string> itemSet = new HashSet<string>();

            currentMainLevel = mainKey;
            currentDataTable = dt;

            foreach (DataRow row in dt.Rows)
            {
                string item = row["sublevel"].ToString().Split('_')[0];
                if (item.StartsWith(subKey) && char.IsDigit(item[subKey.Length]))
                    itemSet.Add(item);
            }

            ButtonMenu.Items.Clear();
            foreach (string item in itemSet)
                ButtonMenu.Items.Add(item);
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
            FormulationLinesGroupBox.Visible = false;
            KneadingLinesGroupBox.Visible = false;
            ResetColors();
            CreateConn();
        }

        private void ButtonMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var formLocation = this.Location;
            formLocation.Offset(this.Size.Width, 0);

            switch (currentMainLevel)
            {
                case "formulation":
                    FormulationLineForm fl_form = new FormulationLineForm
                    {
                        Location = formLocation,
                        parent = this,
                        configDataTable = currentDataTable,
                        processLine = e.ClickedItem.Text
                    };
                    fl_form.ShowDialog();
                    break;
                case "kneading":
                    KneadingLineForm kl_form = new KneadingLineForm
                    {
                        Location = formLocation,
                        parent = this,
                        configDataTable = currentDataTable,
                        processLine = e.ClickedItem.Text
                    };
                    kl_form.ShowDialog();
                    break;
            }
        }

        private void RawMaterialButton_Click(object sender, EventArgs e)
        {
            //ExecuteSQL("select * from public.ironman_menuconfig");
            SetButtonProperties(RawMaterialButton);
            RawMaterialForm form = new RawMaterialForm();
            form.ShowDialog();
        }

        private void FormulationLinesButton_Click(object sender, EventArgs e)
        {
            InstantiateMenu(FormulationLinesButton, "formulation", "F");
            SetButtonProperties(FormulationLinesButton, FormulationLinesGroupBox);
        }

        private void KneadingLinesButton_Click(object sender, EventArgs e)
        {
            InstantiateMenu(KneadingLinesButton, "kneading", "K");
            SetButtonProperties(KneadingLinesButton, KneadingLinesGroupBox);
        }

        private void TablettingLinesButton_Click(object sender, EventArgs e)
        {
            SetButtonProperties(TablettingLinesButton);
        }

        private void SideProcessButton_Click(object sender, EventArgs e)
        {
            SetButtonProperties(SideProcessButton, SideProcessGroupBox);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void F1Button_Click(object sender, EventArgs e)
        {
            SetButtonProperties(F1Button);
            FormulationLineForm form = new FormulationLineForm();
            form.ShowDialog();
        }

        private void F2Button_Click(object sender, EventArgs e)
        {
            SetButtonProperties(F2Button);
            FormulationLineForm form = new FormulationLineForm();
            form.ShowDialog();
        }

        private void K1Button_Click(object sender, EventArgs e)
        {
            SetButtonProperties(K1Button);
            KneadingLineForm form = new KneadingLineForm();
            form.ShowDialog();
        }

        private void K2Button_Click(object sender, EventArgs e)
        {
            SetButtonProperties(K1Button);
            KneadingLineForm form = new KneadingLineForm();
            form.ShowDialog();
        }

        private void K3Button_Click(object sender, EventArgs e)
        {
            SetButtonProperties(K3Button);
        }

        private void ProductRecrushButton_Click(object sender, EventArgs e)
        {
            SetButtonProperties(ProductRecrushButton);
        }

        private void OffLineMagnetButton_Click(object sender, EventArgs e)
        {
            SetButtonProperties(OffLineMagnetButton);
        }
    }
}
