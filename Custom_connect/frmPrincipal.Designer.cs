namespace Custom_Mascarello
{
    partial class frmPrincipal
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lista_mp = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tab_itens = new Syncfusion.Windows.Forms.Tools.TabControlAdv();
            this.tabprincipal = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button13 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.menu_acoes = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.upadateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.criarRevisãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lista_boletins = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aaaaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sssssToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tab_itens)).BeginInit();
            this.tab_itens.SuspendLayout();
            this.tabprincipal.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.menu_acoes.SuspendLayout();
            this.lista_boletins.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "213320.JPG");
            // 
            // lista_mp
            // 
            this.lista_mp.Name = "lista_mp";
            this.lista_mp.Size = new System.Drawing.Size(61, 4);
            // 
            // tab_itens
            // 
            this.tab_itens.BeforeTouchSize = new System.Drawing.Size(413, 148);
            this.tab_itens.Controls.Add(this.tabprincipal);
            this.tab_itens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_itens.FocusOnTabClick = false;
            this.tab_itens.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tab_itens.Location = new System.Drawing.Point(2, 32);
            this.tab_itens.Name = "tab_itens";
            this.tab_itens.Size = new System.Drawing.Size(413, 148);
            this.tab_itens.TabIndex = 1;
            this.tab_itens.TabStyle = typeof(Syncfusion.Windows.Forms.Tools.TabRendererMetro);
            this.tab_itens.ThemeName = "TabRendererMetro";
            this.tab_itens.ThemeStyle.PrimitiveButtonStyle.DisabledNextPageImage = null;
            // 
            // tabprincipal
            // 
            this.tabprincipal.Controls.Add(this.panel1);
            this.tabprincipal.Image = null;
            this.tabprincipal.ImageSize = new System.Drawing.Size(16, 16);
            this.tabprincipal.Location = new System.Drawing.Point(1, 25);
            this.tabprincipal.Name = "tabprincipal";
            this.tabprincipal.ShowCloseButton = true;
            this.tabprincipal.Size = new System.Drawing.Size(410, 121);
            this.tabprincipal.TabIndex = 1;
            this.tabprincipal.Text = "Principal";
            this.tabprincipal.ThemesEnabled = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button13);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 121);
            this.panel1.TabIndex = 2;
            // 
            // button13
            // 
            this.button13.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button13.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button13.Location = new System.Drawing.Point(90, 31);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(206, 40);
            this.button13.TabIndex = 16;
            this.button13.Text = "start";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click_1);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(413, 30);
            this.panel2.TabIndex = 2;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Custom_Mascarello.Properties.Resources.LOGOMARCA_MASCARELLO_2019;
            this.pictureBox2.Location = new System.Drawing.Point(870, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(125, 44);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // menu_acoes
            // 
            this.menu_acoes.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this.menu_acoes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.upadateToolStripMenuItem,
            this.criarRevisãoToolStripMenuItem});
            this.menu_acoes.Name = "menu_acoes";
            this.menu_acoes.Size = new System.Drawing.Size(143, 48);
            // 
            // upadateToolStripMenuItem
            // 
            this.upadateToolStripMenuItem.Image = global::Custom_Mascarello.Properties.Resources.update_24;
            this.upadateToolStripMenuItem.Name = "upadateToolStripMenuItem";
            this.upadateToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.upadateToolStripMenuItem.Text = "Update";
            // 
            // criarRevisãoToolStripMenuItem
            // 
            this.criarRevisãoToolStripMenuItem.Name = "criarRevisãoToolStripMenuItem";
            this.criarRevisãoToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.criarRevisãoToolStripMenuItem.Text = "Criar Revisão";
            this.criarRevisãoToolStripMenuItem.Visible = false;
            // 
            // lista_boletins
            // 
            this.lista_boletins.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aaaaToolStripMenuItem});
            this.lista_boletins.Name = "lista_mp";
            this.lista_boletins.Size = new System.Drawing.Size(99, 26);
            // 
            // aaaaToolStripMenuItem
            // 
            this.aaaaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sssssToolStripMenuItem});
            this.aaaaToolStripMenuItem.Name = "aaaaToolStripMenuItem";
            this.aaaaToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.aaaaToolStripMenuItem.Text = "aaaa";
            // 
            // sssssToolStripMenuItem
            // 
            this.sssssToolStripMenuItem.Name = "sssssToolStripMenuItem";
            this.sssssToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.sssssToolStripMenuItem.Text = "sssss";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(282, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 17;
            this.label1.Text = "...";
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 182);
            this.Controls.Add(this.tab_itens);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Text = "Mascarello - NX Custom V2312";
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tab_itens)).EndInit();
            this.tab_itens.ResumeLayout(false);
            this.tabprincipal.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.menu_acoes.ResumeLayout(false);
            this.lista_boletins.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ContextMenuStrip lista_mp;
        private Syncfusion.Windows.Forms.Tools.TabControlAdv tab_itens;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabprincipal;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ContextMenuStrip menu_acoes;
        private System.Windows.Forms.ToolStripMenuItem upadateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem criarRevisãoToolStripMenuItem;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.ContextMenuStrip lista_boletins;
        private System.Windows.Forms.ToolStripMenuItem aaaaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sssssToolStripMenuItem;
        private System.Windows.Forms.Label label1;
    }
}