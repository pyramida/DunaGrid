namespace DunaGrid.components
{
    partial class DunaHScrollBar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.navigatePanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(84, 0);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(422, 64);
            this.hScrollBar1.TabIndex = 0;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // navigatePanel
            // 
            this.navigatePanel.Location = new System.Drawing.Point(3, 3);
            this.navigatePanel.Name = "navigatePanel";
            this.navigatePanel.Size = new System.Drawing.Size(78, 61);
            this.navigatePanel.TabIndex = 1;
            // 
            // DunaHScrollBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.navigatePanel);
            this.Controls.Add(this.hScrollBar1);
            this.Name = "DunaHScrollBar";
            this.Size = new System.Drawing.Size(540, 67);
            this.Load += new System.EventHandler(this.DunaHScrollBar_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Panel navigatePanel;
    }
}
