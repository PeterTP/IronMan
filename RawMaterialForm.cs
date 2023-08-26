using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IronMan
{
    public partial class RawMaterialForm : Form
    {
        public string data;

        public RawMaterialForm()
        {
            InitializeComponent();
        }

        private void SetData()
        {
            switch (Text)
            {
                case "F1":
                    data = "Filler_Inlet,Resin_Inlet,Mixer_Outlet";
                    break;
                case "F2":
                    data = "Filler_Inlet,Resin_Inlet,Additive_Inlet";
                    break;
                case "K1":
                    data = "Kneader_Inlet,Bucket_Conveyor_1,Bucket_Conveyor_2,EMS,AMS";
                    break;
                case "K2":
                    data = "Kneader_Inlet,Bucket_Conveyor_1,Bucket_Conveyor_2,Crusher_Hopper,EMS,AMS";
                    break;
                case "K3":
                    data = "Kneader_Inlet,Bucket_Conveyor,Crusher_Hopper,EMS,AMS";
                    break;
                case "Tabletting Lines":
                    data = "Iron_Separator";
                    break;
                case "Product Recrush":
                    data = "Product_Recrush";
                    break;
                case "Off-Line Magnet":
                    data = "Offline_Magnet";
                    break;
                default:
                    data = "No Match";
                    break;
            }
        }

        private void RawMaterialForm_Load(object sender, EventArgs e)
        {
            SetData();
            TableLayoutPanel dynamicTableLayoutPanel = new TableLayoutPanel
            {
                Location = new System.Drawing.Point(26, 72),
                Name = "TableLayoutPanel1",
                Size = new System.Drawing.Size(this.ClientSize.Width-20, this.ClientSize.Height-10),
                BackColor = Color.LightBlue,
                ColumnCount = 6,
                RowCount = 10
            };
            dynamicTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 55));
            dynamicTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 55));
            dynamicTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 55));
            dynamicTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 55));

            string[] fileDataField = new string[] { "SampleWeight(g)", "10", "ACCEPT"};
            string[] fileDataField2 = new string[] { "MetalWeight(mg)", "0.2", "ACCEPT" };
            string[] fileDataField3 = new string[] { "MetalSize(mm)", "0.1", "REJECT" };
            dataGridView1.Rows.Add(fileDataField);
            dataGridView1.Rows.Add(fileDataField2);
            dataGridView1.Rows.Add(fileDataField3);
        }
    }
}
