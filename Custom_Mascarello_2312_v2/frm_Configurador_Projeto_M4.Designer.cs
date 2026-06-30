namespace Custom_Mascarello
{
    partial class frm_Configurador_Projeto_M4
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
            this.btn_aplicar_itens = new System.Windows.Forms.Button();
            this.btn_codificar = new System.Windows.Forms.Button();
            this.btn_add_port = new System.Windows.Forms.Button();
            this.dtg_view_dna = new System.Windows.Forms.DataGridView();
            this.Item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_import_dna = new System.Windows.Forms.Button();
            this.btn_rditar_dna = new System.Windows.Forms.Button();
            this.txt_fm_dna = new System.Windows.Forms.TextBox();
            this.lbl_criacao = new System.Windows.Forms.Label();
            this.lbl_alteracao = new System.Windows.Forms.Label();
            this.btn_aplicar_configuracoes = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbox_vao_4_le = new System.Windows.Forms.ComboBox();
            this.cmbox_vao_4_ld = new System.Windows.Forms.ComboBox();
            this.cmbox_vao_3_le = new System.Windows.Forms.ComboBox();
            this.cmbox_vao_3_ld = new System.Windows.Forms.ComboBox();
            this.cmbox_vao_2_le = new System.Windows.Forms.ComboBox();
            this.cmbox_vao_2_ld = new System.Windows.Forms.ComboBox();
            this.cmbox_vao_1_le = new System.Windows.Forms.ComboBox();
            this.cmbox_vao_1_ld = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbox_vao_bag_tras = new System.Windows.Forms.ComboBox();
            this.lbl_proccess = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_configurar_bagageiros = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_view_dna)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_aplicar_itens
            // 
            this.btn_aplicar_itens.Location = new System.Drawing.Point(760, 585);
            this.btn_aplicar_itens.Name = "btn_aplicar_itens";
            this.btn_aplicar_itens.Size = new System.Drawing.Size(97, 33);
            this.btn_aplicar_itens.TabIndex = 3;
            this.btn_aplicar_itens.Text = "Aplicar";
            this.btn_aplicar_itens.UseVisualStyleBackColor = true;
            this.btn_aplicar_itens.Click += new System.EventHandler(this.btn_aplicar_itens_Click);
            // 
            // btn_codificar
            // 
            this.btn_codificar.Location = new System.Drawing.Point(647, 585);
            this.btn_codificar.Name = "btn_codificar";
            this.btn_codificar.Size = new System.Drawing.Size(98, 33);
            this.btn_codificar.TabIndex = 4;
            this.btn_codificar.Text = "Codificar";
            this.btn_codificar.UseVisualStyleBackColor = true;
            this.btn_codificar.Click += new System.EventHandler(this.btn_codificar_Click);
            // 
            // btn_add_port
            // 
            this.btn_add_port.Location = new System.Drawing.Point(760, 524);
            this.btn_add_port.Name = "btn_add_port";
            this.btn_add_port.Size = new System.Drawing.Size(97, 33);
            this.btn_add_port.TabIndex = 3;
            this.btn_add_port.Text = "Add Itens";
            this.btn_add_port.UseVisualStyleBackColor = true;
            this.btn_add_port.Click += new System.EventHandler(this.button1_Click);
            // 
            // dtg_view_dna
            // 
            this.dtg_view_dna.AllowUserToAddRows = false;
            this.dtg_view_dna.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtg_view_dna.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Item,
            this.valor});
            this.dtg_view_dna.Location = new System.Drawing.Point(22, 56);
            this.dtg_view_dna.Name = "dtg_view_dna";
            this.dtg_view_dna.RowHeadersVisible = false;
            this.dtg_view_dna.Size = new System.Drawing.Size(599, 628);
            this.dtg_view_dna.TabIndex = 9;
            // 
            // Item
            // 
            this.Item.HeaderText = "Item";
            this.Item.Name = "Item";
            this.Item.ReadOnly = true;
            this.Item.Width = 160;
            // 
            // valor
            // 
            this.valor.HeaderText = "Opção";
            this.valor.Name = "valor";
            this.valor.ReadOnly = true;
            this.valor.Width = 300;
            // 
            // btn_import_dna
            // 
            this.btn_import_dna.Location = new System.Drawing.Point(644, 92);
            this.btn_import_dna.Name = "btn_import_dna";
            this.btn_import_dna.Size = new System.Drawing.Size(90, 23);
            this.btn_import_dna.TabIndex = 10;
            this.btn_import_dna.Text = "Importar DNA";
            this.btn_import_dna.UseVisualStyleBackColor = true;
            this.btn_import_dna.Click += new System.EventHandler(this.btn_import_dna_Click);
            // 
            // btn_rditar_dna
            // 
            this.btn_rditar_dna.Location = new System.Drawing.Point(644, 121);
            this.btn_rditar_dna.Name = "btn_rditar_dna";
            this.btn_rditar_dna.Size = new System.Drawing.Size(90, 23);
            this.btn_rditar_dna.TabIndex = 10;
            this.btn_rditar_dna.Text = "Editar DNA";
            this.btn_rditar_dna.UseVisualStyleBackColor = true;
            this.btn_rditar_dna.Click += new System.EventHandler(this.btn_rditar_dna_Click);
            // 
            // txt_fm_dna
            // 
            this.txt_fm_dna.Location = new System.Drawing.Point(645, 56);
            this.txt_fm_dna.Name = "txt_fm_dna";
            this.txt_fm_dna.Size = new System.Drawing.Size(89, 20);
            this.txt_fm_dna.TabIndex = 11;
            this.txt_fm_dna.TextChanged += new System.EventHandler(this.txt_fm_dna_TextChanged);
            // 
            // lbl_criacao
            // 
            this.lbl_criacao.AutoSize = true;
            this.lbl_criacao.Location = new System.Drawing.Point(22, 691);
            this.lbl_criacao.Name = "lbl_criacao";
            this.lbl_criacao.Size = new System.Drawing.Size(0, 13);
            this.lbl_criacao.TabIndex = 12;
            // 
            // lbl_alteracao
            // 
            this.lbl_alteracao.AutoSize = true;
            this.lbl_alteracao.Location = new System.Drawing.Point(22, 718);
            this.lbl_alteracao.Name = "lbl_alteracao";
            this.lbl_alteracao.Size = new System.Drawing.Size(0, 13);
            this.lbl_alteracao.TabIndex = 12;
            // 
            // btn_aplicar_configuracoes
            // 
            this.btn_aplicar_configuracoes.Location = new System.Drawing.Point(537, 691);
            this.btn_aplicar_configuracoes.Name = "btn_aplicar_configuracoes";
            this.btn_aplicar_configuracoes.Size = new System.Drawing.Size(84, 24);
            this.btn_aplicar_configuracoes.TabIndex = 13;
            this.btn_aplicar_configuracoes.Text = "Aplicar";
            this.btn_aplicar_configuracoes.UseVisualStyleBackColor = true;
            this.btn_aplicar_configuracoes.Click += new System.EventHandler(this.btn_aplicar_configuracoes_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cmbox_vao_4_le);
            this.panel2.Controls.Add(this.cmbox_vao_4_ld);
            this.panel2.Controls.Add(this.cmbox_vao_3_le);
            this.panel2.Controls.Add(this.cmbox_vao_3_ld);
            this.panel2.Controls.Add(this.cmbox_vao_2_le);
            this.panel2.Controls.Add(this.cmbox_vao_2_ld);
            this.panel2.Controls.Add(this.cmbox_vao_1_le);
            this.panel2.Controls.Add(this.cmbox_vao_1_ld);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(9, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(335, 119);
            this.panel2.TabIndex = 2;
            // 
            // cmbox_vao_4_le
            // 
            this.cmbox_vao_4_le.FormattingEnabled = true;
            this.cmbox_vao_4_le.Items.AddRange(new object[] {
            "NÃO",
            "1000",
            "1100",
            "1200",
            "1300",
            "1400",
            "1500",
            "1600",
            "1700",
            "1860"});
            this.cmbox_vao_4_le.Location = new System.Drawing.Point(254, 86);
            this.cmbox_vao_4_le.Name = "cmbox_vao_4_le";
            this.cmbox_vao_4_le.Size = new System.Drawing.Size(66, 21);
            this.cmbox_vao_4_le.TabIndex = 1;
            // 
            // cmbox_vao_4_ld
            // 
            this.cmbox_vao_4_ld.FormattingEnabled = true;
            this.cmbox_vao_4_ld.Items.AddRange(new object[] {
            "NÃO",
            "1000",
            "1100",
            "1200",
            "1300",
            "1400",
            "1500",
            "1600",
            "1700",
            "1860"});
            this.cmbox_vao_4_ld.Location = new System.Drawing.Point(254, 25);
            this.cmbox_vao_4_ld.Name = "cmbox_vao_4_ld";
            this.cmbox_vao_4_ld.Size = new System.Drawing.Size(66, 21);
            this.cmbox_vao_4_ld.TabIndex = 1;
            // 
            // cmbox_vao_3_le
            // 
            this.cmbox_vao_3_le.FormattingEnabled = true;
            this.cmbox_vao_3_le.Items.AddRange(new object[] {
            "1000",
            "1100",
            "1200",
            "1300",
            "1400",
            "1500",
            "1600",
            "1700",
            "1860"});
            this.cmbox_vao_3_le.Location = new System.Drawing.Point(170, 86);
            this.cmbox_vao_3_le.Name = "cmbox_vao_3_le";
            this.cmbox_vao_3_le.Size = new System.Drawing.Size(66, 21);
            this.cmbox_vao_3_le.TabIndex = 1;
            // 
            // cmbox_vao_3_ld
            // 
            this.cmbox_vao_3_ld.FormattingEnabled = true;
            this.cmbox_vao_3_ld.Items.AddRange(new object[] {
            "1000",
            "1100",
            "1200",
            "1300",
            "1400",
            "1500",
            "1600",
            "1700",
            "1860"});
            this.cmbox_vao_3_ld.Location = new System.Drawing.Point(170, 25);
            this.cmbox_vao_3_ld.Name = "cmbox_vao_3_ld";
            this.cmbox_vao_3_ld.Size = new System.Drawing.Size(66, 21);
            this.cmbox_vao_3_ld.TabIndex = 1;
            // 
            // cmbox_vao_2_le
            // 
            this.cmbox_vao_2_le.FormattingEnabled = true;
            this.cmbox_vao_2_le.Items.AddRange(new object[] {
            "TQ_ARLA",
            "BATERIA",
            "1000",
            "1100",
            "1200",
            "1300",
            "1400",
            "1500",
            "1600",
            "1700",
            "1860"});
            this.cmbox_vao_2_le.Location = new System.Drawing.Point(89, 86);
            this.cmbox_vao_2_le.Name = "cmbox_vao_2_le";
            this.cmbox_vao_2_le.Size = new System.Drawing.Size(66, 21);
            this.cmbox_vao_2_le.TabIndex = 1;
            // 
            // cmbox_vao_2_ld
            // 
            this.cmbox_vao_2_ld.FormattingEnabled = true;
            this.cmbox_vao_2_ld.Items.AddRange(new object[] {
            "TQ_ARLA",
            "BATERIA",
            "1000",
            "1100",
            "1200",
            "1300",
            "1400",
            "1500",
            "1600",
            "1700",
            "1860"});
            this.cmbox_vao_2_ld.Location = new System.Drawing.Point(89, 25);
            this.cmbox_vao_2_ld.Name = "cmbox_vao_2_ld";
            this.cmbox_vao_2_ld.Size = new System.Drawing.Size(66, 21);
            this.cmbox_vao_2_ld.TabIndex = 1;
            // 
            // cmbox_vao_1_le
            // 
            this.cmbox_vao_1_le.FormattingEnabled = true;
            this.cmbox_vao_1_le.Items.AddRange(new object[] {
            "TQ_ARLA",
            "BATERIA",
            "1000",
            "1100",
            "1200",
            "1300",
            "1400",
            "1500",
            "1600",
            "1700",
            "1860"});
            this.cmbox_vao_1_le.Location = new System.Drawing.Point(10, 86);
            this.cmbox_vao_1_le.Name = "cmbox_vao_1_le";
            this.cmbox_vao_1_le.Size = new System.Drawing.Size(66, 21);
            this.cmbox_vao_1_le.TabIndex = 1;
            // 
            // cmbox_vao_1_ld
            // 
            this.cmbox_vao_1_ld.FormattingEnabled = true;
            this.cmbox_vao_1_ld.Items.AddRange(new object[] {
            "TQ_ARLA",
            "BATERIA",
            "1000",
            "1100",
            "1200",
            "1300",
            "1400",
            "1500",
            "1600",
            "1700",
            "1860"});
            this.cmbox_vao_1_ld.Location = new System.Drawing.Point(10, 25);
            this.cmbox_vao_1_ld.Name = "cmbox_vao_1_ld";
            this.cmbox_vao_1_ld.Size = new System.Drawing.Size(66, 21);
            this.cmbox_vao_1_ld.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(260, 70);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "VÃO 4 LE\r\n";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(260, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "VÃO 4 LD";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(176, 70);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "VÃO 3 LE";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(176, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "VÃO 3 LD";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(95, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "VÃO 2 LE";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(95, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "VÃO 2 LD";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "VÃO 1 LE";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "VÃO 1 LD";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(137, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 16);
            this.label11.TabIndex = 3;
            this.label11.Text = "Central";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(380, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 16);
            this.label12.TabIndex = 3;
            this.label12.Text = "Traseiro";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.cmbox_vao_bag_tras);
            this.panel3.Location = new System.Drawing.Point(350, 39);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(142, 118);
            this.panel3.TabIndex = 4;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(30, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "VÃO BAG TRAS";
            // 
            // cmbox_vao_bag_tras
            // 
            this.cmbox_vao_bag_tras.FormattingEnabled = true;
            this.cmbox_vao_bag_tras.Items.AddRange(new object[] {
            "1000",
            "1100",
            "1200",
            "1300",
            "1400",
            "1500",
            "1600"});
            this.cmbox_vao_bag_tras.Location = new System.Drawing.Point(40, 41);
            this.cmbox_vao_bag_tras.Name = "cmbox_vao_bag_tras";
            this.cmbox_vao_bag_tras.Size = new System.Drawing.Size(66, 21);
            this.cmbox_vao_bag_tras.TabIndex = 1;
            // 
            // lbl_proccess
            // 
            this.lbl_proccess.AutoSize = true;
            this.lbl_proccess.Location = new System.Drawing.Point(3, 16);
            this.lbl_proccess.Name = "lbl_proccess";
            this.lbl_proccess.Size = new System.Drawing.Size(0, 13);
            this.lbl_proccess.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbl_proccess);
            this.panel1.Location = new System.Drawing.Point(28, 727);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 44);
            this.panel1.TabIndex = 7;
            // 
            // btn_configurar_bagageiros
            // 
            this.btn_configurar_bagageiros.Location = new System.Drawing.Point(627, 660);
            this.btn_configurar_bagageiros.Name = "btn_configurar_bagageiros";
            this.btn_configurar_bagageiros.Size = new System.Drawing.Size(136, 24);
            this.btn_configurar_bagageiros.TabIndex = 13;
            this.btn_configurar_bagageiros.Text = "Configurar Bagageiros";
            this.btn_configurar_bagageiros.UseVisualStyleBackColor = true;
            this.btn_configurar_bagageiros.Click += new System.EventHandler(this.btn_configurar_bagageiros_Click);
            // 
            // frm_Configurador_Projeto_M4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(881, 778);
            this.Controls.Add(this.btn_configurar_bagageiros);
            this.Controls.Add(this.btn_aplicar_configuracoes);
            this.Controls.Add(this.lbl_alteracao);
            this.Controls.Add(this.lbl_criacao);
            this.Controls.Add(this.txt_fm_dna);
            this.Controls.Add(this.btn_rditar_dna);
            this.Controls.Add(this.btn_import_dna);
            this.Controls.Add(this.dtg_view_dna);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_codificar);
            this.Controls.Add(this.btn_add_port);
            this.Controls.Add(this.btn_aplicar_itens);
            this.Name = "frm_Configurador_Projeto_M4";
            this.Text = "Configurador_Projeto";
            this.Load += new System.EventHandler(this.Configurador_Projeto_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtg_view_dna)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_aplicar_itens;
        private System.Windows.Forms.Button btn_codificar;
        private System.Windows.Forms.Button btn_add_port;
        private System.Windows.Forms.DataGridView dtg_view_dna;
        private System.Windows.Forms.Button btn_import_dna;
        private System.Windows.Forms.Button btn_rditar_dna;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item;
        private System.Windows.Forms.DataGridViewTextBoxColumn valor;
        private System.Windows.Forms.TextBox txt_fm_dna;
        private System.Windows.Forms.Label lbl_criacao;
        private System.Windows.Forms.Label lbl_alteracao;
        private System.Windows.Forms.Button btn_aplicar_configuracoes;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cmbox_vao_4_le;
        private System.Windows.Forms.ComboBox cmbox_vao_4_ld;
        private System.Windows.Forms.ComboBox cmbox_vao_3_le;
        private System.Windows.Forms.ComboBox cmbox_vao_3_ld;
        private System.Windows.Forms.ComboBox cmbox_vao_2_le;
        private System.Windows.Forms.ComboBox cmbox_vao_2_ld;
        private System.Windows.Forms.ComboBox cmbox_vao_1_le;
        private System.Windows.Forms.ComboBox cmbox_vao_1_ld;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbox_vao_bag_tras;
        private System.Windows.Forms.Label lbl_proccess;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_configurar_bagageiros;
    }
}