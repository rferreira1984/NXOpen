namespace Custom_Mascarello
{
    partial class frm_contraventamentos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_contraventamentos));
            this.btn_criar_add = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_criar_add
            // 
            this.btn_criar_add.BackColor = System.Drawing.SystemColors.Control;
            this.btn_criar_add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_criar_add.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_criar_add.Location = new System.Drawing.Point(17, 12);
            this.btn_criar_add.Name = "btn_criar_add";
            this.btn_criar_add.Size = new System.Drawing.Size(279, 30);
            this.btn_criar_add.TabIndex = 2;
            this.btn_criar_add.Text = "GERAR LISTA DE CORTE";
            this.btn_criar_add.UseVisualStyleBackColor = false;
            this.btn_criar_add.Click += new System.EventHandler(this.btn_criar_add_Click);
            // 
            // frm_contraventamentos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(312, 55);
            this.Controls.Add(this.btn_criar_add);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_contraventamentos";
            this.Text = "Contraventamentos / Lista de Corte ";
            this.Load += new System.EventHandler(this.frm_contraventamentos_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_criar_add;
    }
}