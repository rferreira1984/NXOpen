namespace Custom_Mascarello
{
    partial class frm_informacao_itens
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
            this.dtg_inf = new System.Windows.Forms.DataGridView();
            this.col_item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_inf)).BeginInit();
            this.SuspendLayout();
            // 
            // dtg_inf
            // 
            this.dtg_inf.AllowUserToAddRows = false;
            this.dtg_inf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtg_inf.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_item,
            this.col_codigo});
            this.dtg_inf.Location = new System.Drawing.Point(12, 26);
            this.dtg_inf.Name = "dtg_inf";
            this.dtg_inf.RowHeadersVisible = false;
            this.dtg_inf.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dtg_inf.Size = new System.Drawing.Size(375, 354);
            this.dtg_inf.TabIndex = 0;
            // 
            // col_item
            // 
            this.col_item.HeaderText = "DESCRIÇÃO ITEM";
            this.col_item.Name = "col_item";
            this.col_item.Width = 250;
            // 
            // col_codigo
            // 
            this.col_codigo.HeaderText = "CODIGO";
            this.col_codigo.Name = "col_codigo";
            this.col_codigo.Width = 120;
            // 
            // frm_informacao_itens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 410);
            this.Controls.Add(this.dtg_inf);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_informacao_itens";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Relação de Itens ";
            this.Load += new System.EventHandler(this.frm_informacao_itens_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtg_inf)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dtg_inf;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_item;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_codigo;
    }
}