namespace DunaGridFormTest
{
    partial class Form1
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
            this.dunaGridHeaderRow1 = new DunaGrid.components.DunaGridHeaderRow();
            this.dunaGrid1 = new DunaGrid.DunaGrid();
            this.SuspendLayout();
            // 
            // dunaGridHeaderRow1
            // 
            this.dunaGridHeaderRow1.Columns = null;
            this.dunaGridHeaderRow1.Dock = System.Windows.Forms.DockStyle.Top;
            this.dunaGridHeaderRow1.Location = new System.Drawing.Point(12, 79);
            this.dunaGridHeaderRow1.Name = "dunaGridHeaderRow1";
            this.dunaGridHeaderRow1.RowSelectorWidth = 0;
            this.dunaGridHeaderRow1.Size = new System.Drawing.Size(452, 28);
            this.dunaGridHeaderRow1.TabIndex = 1;
            // 
            // dunaGrid1
            // 
            this.dunaGrid1.AutoColumnGenerator = true;
            this.dunaGrid1.BackColor = System.Drawing.Color.DarkGray;
            this.dunaGrid1.CellPadding = new System.Windows.Forms.Padding(3);
            this.dunaGrid1.DataSource = null;
            this.dunaGrid1.DefaultRowHeight = 20;
            this.dunaGrid1.LineColor = System.Drawing.Color.Black;
            this.dunaGrid1.Location = new System.Drawing.Point(102, 271);
            this.dunaGrid1.Name = "dunaGrid1";
            this.dunaGrid1.RowSelectorWidth = 30;
            this.dunaGrid1.Size = new System.Drawing.Size(302, 223);
            this.dunaGrid1.TabIndex = 0;
            this.dunaGrid1.Click += new System.EventHandler(this.dunaGrid1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 530);
            this.Controls.Add(this.dunaGridHeaderRow1);
            this.Controls.Add(this.dunaGrid1);
            this.Name = "Form1";
            this.Text = "DunaGrid Test Form";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DunaGrid.DunaGrid dunaGrid1;
        private DunaGrid.components.DunaGridHeaderRow dunaGridHeaderRow1;

    }
}

