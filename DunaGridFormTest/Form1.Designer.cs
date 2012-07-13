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
            this.dunaGrid1 = new DunaGrid.DunaGridView();
            this.SuspendLayout();
            // 
            // dunaGrid1
            // 
            this.dunaGrid1.AutoColumnGenerator = true;
            this.dunaGrid1.BackColor = System.Drawing.Color.DarkGray;
            this.dunaGrid1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dunaGrid1.CellPadding = new System.Windows.Forms.Padding(3);
            this.dunaGrid1.DataSource = null;
            this.dunaGrid1.DefaultRowHeight = 20;
            this.dunaGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dunaGrid1.LineColor = System.Drawing.Color.Black;
            this.dunaGrid1.Location = new System.Drawing.Point(0, 0);
            this.dunaGrid1.Name = "dunaGrid1";
            this.dunaGrid1.RowSelectorWidth = 30;
            this.dunaGrid1.Size = new System.Drawing.Size(675, 530);
            this.dunaGrid1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 530);
            this.Controls.Add(this.dunaGrid1);
            this.Name = "Form1";
            this.Text = "DunaGrid Test Form";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DunaGrid.DunaGridView dunaGrid1;



    }
}

