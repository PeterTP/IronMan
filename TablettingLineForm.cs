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
    public partial class TablettingLineForm : Form
    {
        public TablettingLineForm()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.Text = "Formulation Line";
            comboBox3.Text = "F1";
            comboBox3.Enabled= false;
        }
    }
}
