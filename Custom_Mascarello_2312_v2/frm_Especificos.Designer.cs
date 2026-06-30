namespace Custom_Mascarello
{
    partial class frm_Especificos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Especificos));
            this.label1 = new System.Windows.Forms.Label();
            this.btn_aplicar_configuracoes = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightCyan;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Selecione as opções abaixo:";
            // 
            // btn_aplicar_configuracoes
            // 
            this.btn_aplicar_configuracoes.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_aplicar_configuracoes.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_aplicar_configuracoes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btn_aplicar_configuracoes.Font = new System.Drawing.Font("SansSerif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btn_aplicar_configuracoes.Location = new System.Drawing.Point(600, 8);
            this.btn_aplicar_configuracoes.Margin = new System.Windows.Forms.Padding(4);
            this.btn_aplicar_configuracoes.Name = "btn_aplicar_configuracoes";
            this.btn_aplicar_configuracoes.Size = new System.Drawing.Size(121, 31);
            this.btn_aplicar_configuracoes.TabIndex = 24;
            this.btn_aplicar_configuracoes.Text = "Aplicar";
            this.btn_aplicar_configuracoes.UseVisualStyleBackColor = false;
            this.btn_aplicar_configuracoes.Click += new System.EventHandler(this.btn_aplicar_configuracoes_Click);
            // 
            // frm_Especificos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(174)))));
            this.ClientSize = new System.Drawing.Size(734, 612);
            this.Controls.Add(this.btn_aplicar_configuracoes);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_Especificos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuração Específica";
            this.Load += new System.EventHandler(this.frm_Especificos_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_aplicar_configuracoes;
    }
}