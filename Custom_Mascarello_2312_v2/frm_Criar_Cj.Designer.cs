namespace Custom_Mascarello
{
    partial class frm_Criar_Cj
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
            this.label1 = new System.Windows.Forms.Label();
            this.btn_criar_cjx = new System.Windows.Forms.Button();
            this.txt_vao_cj = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_conjunto = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(34, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "VAO";
            // 
            // btn_criar_cjx
            // 
            this.btn_criar_cjx.Location = new System.Drawing.Point(221, 79);
            this.btn_criar_cjx.Name = "btn_criar_cjx";
            this.btn_criar_cjx.Size = new System.Drawing.Size(75, 23);
            this.btn_criar_cjx.TabIndex = 1;
            this.btn_criar_cjx.Text = "Criar";
            this.btn_criar_cjx.UseVisualStyleBackColor = true;
            this.btn_criar_cjx.Click += new System.EventHandler(this.btn_criar_cjx_Click);
            // 
            // txt_vao_cj
            // 
            this.txt_vao_cj.Location = new System.Drawing.Point(80, 82);
            this.txt_vao_cj.Name = "txt_vao_cj";
            this.txt_vao_cj.Size = new System.Drawing.Size(100, 20);
            this.txt_vao_cj.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "CODIGO CRIADO";
            // 
            // txt_conjunto
            // 
            this.txt_conjunto.Location = new System.Drawing.Point(147, 147);
            this.txt_conjunto.Name = "txt_conjunto";
            this.txt_conjunto.Size = new System.Drawing.Size(100, 20);
            this.txt_conjunto.TabIndex = 2;
            // 
            // frm_Criar_Cj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 279);
            this.Controls.Add(this.txt_conjunto);
            this.Controls.Add(this.txt_vao_cj);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_criar_cjx);
            this.Controls.Add(this.label1);
            this.Name = "frm_Criar_Cj";
            this.Text = "frm_Criar_Cj";
            this.Load += new System.EventHandler(this.frm_Criar_Cj_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_criar_cjx;
        private System.Windows.Forms.TextBox txt_vao_cj;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_conjunto;
    }
}