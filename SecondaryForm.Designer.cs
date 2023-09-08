namespace IronMan
{
    partial class SecondaryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ProcessLineComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ProductNameComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LotNoComboBox = new System.Windows.Forms.ComboBox();
            this.ProcessLabel = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.Judgement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Entry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Field = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExampleDataGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExampleTabPage = new System.Windows.Forms.TabPage();
            this.TabControl = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.ExampleDataGrid)).BeginInit();
            this.ExampleTabPage.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProcessLineComboBox
            // 
            this.ProcessLineComboBox.Enabled = false;
            this.ProcessLineComboBox.FormattingEnabled = true;
            this.ProcessLineComboBox.Location = new System.Drawing.Point(214, 164);
            this.ProcessLineComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.ProcessLineComboBox.Name = "ProcessLineComboBox";
            this.ProcessLineComboBox.Size = new System.Drawing.Size(264, 24);
            this.ProcessLineComboBox.TabIndex = 16;
            this.ProcessLineComboBox.Text = "F2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(81, 166);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "Process Line";
            // 
            // ProductNameComboBox
            // 
            this.ProductNameComboBox.Enabled = false;
            this.ProductNameComboBox.FormattingEnabled = true;
            this.ProductNameComboBox.Location = new System.Drawing.Point(214, 112);
            this.ProductNameComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.ProductNameComboBox.Name = "ProductNameComboBox";
            this.ProductNameComboBox.Size = new System.Drawing.Size(264, 24);
            this.ProductNameComboBox.TabIndex = 14;
            this.ProductNameComboBox.Text = "CEL-RP2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(81, 114);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "Product Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(81, 63);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Lot No";
            // 
            // LotNoComboBox
            // 
            this.LotNoComboBox.FormattingEnabled = true;
            this.LotNoComboBox.Location = new System.Drawing.Point(214, 63);
            this.LotNoComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.LotNoComboBox.Name = "LotNoComboBox";
            this.LotNoComboBox.Size = new System.Drawing.Size(264, 24);
            this.LotNoComboBox.TabIndex = 10;
            this.LotNoComboBox.Text = "38001";
            // 
            // ProcessLabel
            // 
            this.ProcessLabel.AutoSize = true;
            this.ProcessLabel.Location = new System.Drawing.Point(81, 32);
            this.ProcessLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ProcessLabel.Name = "ProcessLabel";
            this.ProcessLabel.Size = new System.Drawing.Size(110, 16);
            this.ProcessLabel.TabIndex = 11;
            this.ProcessLabel.Text = "Secondary Label";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(572, 166);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(112, 27);
            this.SaveButton.TabIndex = 17;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // Judgement
            // 
            this.Judgement.HeaderText = "Judgement";
            this.Judgement.MinimumWidth = 8;
            this.Judgement.Name = "Judgement";
            this.Judgement.Width = 150;
            // 
            // Entry
            // 
            this.Entry.HeaderText = "Entry";
            this.Entry.MinimumWidth = 8;
            this.Entry.Name = "Entry";
            this.Entry.Width = 150;
            // 
            // Field
            // 
            this.Field.HeaderText = "Field";
            this.Field.MinimumWidth = 8;
            this.Field.Name = "Field";
            this.Field.Width = 150;
            // 
            // ExampleDataGrid
            // 
            this.ExampleDataGrid.AllowUserToAddRows = false;
            this.ExampleDataGrid.AllowUserToDeleteRows = false;
            this.ExampleDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExampleDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ExampleDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ExampleDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.ExampleDataGrid.Location = new System.Drawing.Point(13, 12);
            this.ExampleDataGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ExampleDataGrid.Name = "ExampleDataGrid";
            this.ExampleDataGrid.RowHeadersWidth = 62;
            this.ExampleDataGrid.RowTemplate.Height = 28;
            this.ExampleDataGrid.Size = new System.Drawing.Size(656, 257);
            this.ExampleDataGrid.TabIndex = 7;
            this.ExampleDataGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellEndEdit);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Field";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Entry";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Judgement";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "ActionSheetNo";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            // 
            // ExampleTabPage
            // 
            this.ExampleTabPage.Controls.Add(this.ExampleDataGrid);
            this.ExampleTabPage.Location = new System.Drawing.Point(4, 25);
            this.ExampleTabPage.Name = "ExampleTabPage";
            this.ExampleTabPage.Padding = new System.Windows.Forms.Padding(10);
            this.ExampleTabPage.Size = new System.Drawing.Size(682, 281);
            this.ExampleTabPage.TabIndex = 1;
            this.ExampleTabPage.Text = "ExampleTabPage";
            this.ExampleTabPage.UseVisualStyleBackColor = true;
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.ExampleTabPage);
            this.TabControl.Location = new System.Drawing.Point(84, 225);
            this.TabControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(690, 310);
            this.TabControl.TabIndex = 0;
            // 
            // SecondaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 574);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ProcessLineComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ProductNameComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LotNoComboBox);
            this.Controls.Add(this.ProcessLabel);
            this.Controls.Add(this.TabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SecondaryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Secondary Form";
            this.Load += new System.EventHandler(this.SecondaryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ExampleDataGrid)).EndInit();
            this.ExampleTabPage.ResumeLayout(false);
            this.TabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox ProcessLineComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ProductNameComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox LotNoComboBox;
        private System.Windows.Forms.Label ProcessLabel;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Judgement;
        private System.Windows.Forms.DataGridViewTextBoxColumn Entry;
        private System.Windows.Forms.DataGridViewTextBoxColumn Field;
        private System.Windows.Forms.DataGridView ExampleDataGrid;
        private System.Windows.Forms.TabPage ExampleTabPage;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
    }
}