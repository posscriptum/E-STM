namespace E_STM
{
    partial class ShurtCircuit
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
            this.Attention = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Attention
            // 
            this.Attention.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Attention.AutoSize = true;
            this.Attention.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Attention.ForeColor = System.Drawing.Color.Red;
            this.Attention.Location = new System.Drawing.Point(13, 13);
            this.Attention.Name = "Attention";
            this.Attention.Size = new System.Drawing.Size(210, 74);
            this.Attention.TabIndex = 0;
            this.Attention.Text = "Attention!\r\nShort circuit!\r\n";
            this.Attention.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ShurtCircuit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 99);
            this.Controls.Add(this.Attention);
            this.Name = "ShurtCircuit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shurt Circuit!";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShurtCircuit_FormClosing);
            this.Load += new System.EventHandler(this.ShurtCircuit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Attention;
    }
}