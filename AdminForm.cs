using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace IronMan
{
    public partial class AdminForm : Form
    {
        public OdbcConnection postgreSQLConn;
        public Dictionary<string, DataGridView[]> dataGrids; // Used to reference datagrid using page name
        public Dictionary<DataGridView, string> tableNames; // Used to reference tablename using datagrid

        public AdminForm()
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

        private void CreateReferences()
        {
            dataGrids = new Dictionary<string, DataGridView[]>
            {
                {"Lots", new DataGridView[] { LotsDataGrid } },
                {"Products", new DataGridView[] { ProductsDataGrid } },
                {"Data", new DataGridView[] { DataDataGrid } },
                {"Config", new DataGridView[] { MenuDataGrid, SubgroupDataGrid } },
                {"Specs", new DataGridView[] { SpecsDataGrid } }
            };

            tableNames = new Dictionary<DataGridView, string>
            {
                {LotsDataGrid, "ironman_newlots" },
                {ProductsDataGrid, "ironman_typelog" }, //TODO which one is products?
                {DataDataGrid, "ironman_data" },
                {MenuDataGrid, "ironman_menuconfig" },
                {SubgroupDataGrid, "ironman_subgroup" },
                {SpecsDataGrid, "ironman_specs" }

            };
        }
        private void FillDataGrid(string tabName)
        {
            string dataSqlTemplate = "select * from public.{0}";

            try //TODO better way to check if key exists
            {
                foreach (DataGridView dataGrid in dataGrids[tabName])
                {
                    DataTable dataGridDataTable = new DataTable();
                    dataGridDataTable.Load(ExecuteSql(String.Format(dataSqlTemplate, tableNames[dataGrid])));
                    dataGrid.DataSource = dataGridDataTable;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }


        private void AdminForm_Load(object sender, EventArgs e)
        {
            CreateConn();
            CreateReferences();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string loginSql = String.Format("select public.g_users.password from public.g_users where public.g_users.employee_id = '{0}'", IDTextBox.Text);
            string password = "";
            OdbcDataReader loginReader = ExecuteSql(loginSql);

            try
            {
                while (loginReader.Read()) { password = loginReader.GetString(0); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Input Error. Please try again");
                return;
            }

            if (PasswordTextBox.Text == password)
                MessageBox.Show("Login Successful");
            else
                MessageBox.Show("Login Failed");
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDataGrid(TabControl.SelectedTab.Text);
        }
    }
}
