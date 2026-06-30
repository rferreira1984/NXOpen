namespace Custom_Mascarello
{
    partial class frm_Configurador_Teto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Configurador_Teto));
            this.dtg_config_teto = new System.Windows.Forms.DataGridView();
            this.pos_cav = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tipo = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lbl_resto = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_qtd = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Preview = new System.Windows.Forms.Button();
            this.btn_apply = new System.Windows.Forms.Button();
            this.btn_reset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_config_teto)).BeginInit();
            this.SuspendLayout();
            // 
            // dtg_config_teto
            // 
            this.dtg_config_teto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtg_config_teto.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pos_cav,
            this.tipo});
            this.dtg_config_teto.Location = new System.Drawing.Point(34, 86);
            this.dtg_config_teto.Name = "dtg_config_teto";
            this.dtg_config_teto.RowHeadersVisible = false;
            this.dtg_config_teto.Size = new System.Drawing.Size(288, 405);
            this.dtg_config_teto.TabIndex = 0;
            this.dtg_config_teto.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtg_config_teto_CellEndEdit);
            this.dtg_config_teto.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dtg_config_teto_RowsAdded);
            // 
            // pos_cav
            // 
            this.pos_cav.HeaderText = "POS CAV";
            this.pos_cav.Name = "pos_cav";
            this.pos_cav.ToolTipText = "1";
            this.pos_cav.Width = 80;
            // 
            // tipo
            // 
            this.tipo.HeaderText = "TIPO CAV";
            this.tipo.Items.AddRange(new object[] {
            "NORMAL",
            "ALCAPAO",
            "AR_CONDICIONADO"});
            this.tipo.Name = "tipo";
            this.tipo.Width = 200;
            // 
            // lbl_resto
            // 
            this.lbl_resto.AutoSize = true;
            this.lbl_resto.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_resto.Location = new System.Drawing.Point(157, 40);
            this.lbl_resto.Name = "lbl_resto";
            this.lbl_resto.Size = new System.Drawing.Size(27, 30);
            this.lbl_resto.TabIndex = 21;
            this.lbl_resto.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(36, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "ULTIMA CAVERNA:";
            // 
            // lbl_qtd
            // 
            this.lbl_qtd.AutoSize = true;
            this.lbl_qtd.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_qtd.Location = new System.Drawing.Point(295, 13);
            this.lbl_qtd.Name = "lbl_qtd";
            this.lbl_qtd.Size = new System.Drawing.Size(27, 30);
            this.lbl_qtd.TabIndex = 22;
            this.lbl_qtd.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(36, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(253, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "QTD CAV DISPONIVEIS PARA SELEÇÃO : ";
            // 
            // btn_Preview
            // 
            this.btn_Preview.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_Preview.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btn_Preview.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_Preview.Location = new System.Drawing.Point(140, 497);
            this.btn_Preview.Name = "btn_Preview";
            this.btn_Preview.Size = new System.Drawing.Size(83, 30);
            this.btn_Preview.TabIndex = 23;
            this.btn_Preview.Text = "Visualizar";
            this.btn_Preview.UseVisualStyleBackColor = false;
            this.btn_Preview.Click += new System.EventHandler(this.btn_Preview_Click);
            // 
            // btn_apply
            // 
            this.btn_apply.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_apply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btn_apply.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_apply.Location = new System.Drawing.Point(245, 497);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(77, 30);
            this.btn_apply.TabIndex = 23;
            this.btn_apply.Text = "Aplicar";
            this.btn_apply.UseVisualStyleBackColor = false;
            this.btn_apply.Click += new System.EventHandler(this.btn_aplicar_Click);
            // 
            // btn_reset
            // 
            this.btn_reset.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_reset.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btn_reset.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_reset.Location = new System.Drawing.Point(34, 497);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(83, 30);
            this.btn_reset.TabIndex = 23;
            this.btn_reset.Text = "Reset";
            this.btn_reset.UseVisualStyleBackColor = false;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // frm_Configurador_Teto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(174)))));
            this.ClientSize = new System.Drawing.Size(342, 543);
            this.Controls.Add(this.btn_apply);
            this.Controls.Add(this.btn_reset);
            this.Controls.Add(this.btn_Preview);
            this.Controls.Add(this.lbl_resto);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbl_qtd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtg_config_teto);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_Configurador_Teto";
            this.Text = "Configurador Teto";
            this.Load += new System.EventHandler(this.frm_Configurador_Teto_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtg_config_teto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dtg_config_teto;
        private System.Windows.Forms.Label lbl_resto;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_qtd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Preview;
        private System.Windows.Forms.DataGridViewTextBoxColumn pos_cav;
        private System.Windows.Forms.DataGridViewComboBoxColumn tipo;
        private System.Windows.Forms.Button btn_apply;
        private System.Windows.Forms.Button btn_reset;
    }
}