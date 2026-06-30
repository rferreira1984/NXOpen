namespace Custom_Mascarello
{
    partial class frm_Mascarello
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
            this.button1 = new System.Windows.Forms.Button();
            this.btn_Fam = new System.Windows.Forms.Button();
            this.btn_criar_cjx = new System.Windows.Forms.Button();
            this.btn_config = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(108, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "GERAR";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_Fam
            // 
            this.btn_Fam.Location = new System.Drawing.Point(28, 220);
            this.btn_Fam.Name = "btn_Fam";
            this.btn_Fam.Size = new System.Drawing.Size(75, 23);
            this.btn_Fam.TabIndex = 3;
            this.btn_Fam.Text = "Familias";
            this.btn_Fam.UseVisualStyleBackColor = true;
            this.btn_Fam.Click += new System.EventHandler(this.btn_Fam_Click);
            // 
            // btn_criar_cjx
            // 
            this.btn_criar_cjx.Location = new System.Drawing.Point(28, 249);
            this.btn_criar_cjx.Name = "btn_criar_cjx";
            this.btn_criar_cjx.Size = new System.Drawing.Size(75, 23);
            this.btn_criar_cjx.TabIndex = 3;
            this.btn_criar_cjx.Text = "Familias Cj";
            this.btn_criar_cjx.UseVisualStyleBackColor = true;
            this.btn_criar_cjx.Click += new System.EventHandler(this.btn_criar_cjx_Click);
            // 
            // btn_config
            // 
            this.btn_config.Location = new System.Drawing.Point(28, 107);
            this.btn_config.Name = "btn_config";
            this.btn_config.Size = new System.Drawing.Size(105, 23);
            this.btn_config.TabIndex = 4;
            this.btn_config.Text = "Configurador Projeto";
            this.btn_config.UseVisualStyleBackColor = true;
            this.btn_config.Click += new System.EventHandler(this.btn_config_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(28, 136);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Teste Tubo";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frm_Mascarello
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 287);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_config);
            this.Controls.Add(this.btn_criar_cjx);
            this.Controls.Add(this.btn_Fam);
            this.Controls.Add(this.button1);
            this.Name = "frm_Mascarello";
            this.Text = "frm_Mascarello";
            this.Load += new System.EventHandler(this.frm_Mascarello_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_Fam;
        private System.Windows.Forms.Button btn_criar_cjx;
        private System.Windows.Forms.Button btn_config;
        private System.Windows.Forms.Button button2;
    }
}