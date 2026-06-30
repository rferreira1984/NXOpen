namespace Custom_Mascarello

{
    partial class frm_select_boletim
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
            this.DataGrid_Boletins = new System.Windows.Forms.DataGridView();
            this.pos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Insert = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.boletim_ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NotaPintura = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_Boletins)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGrid_Boletins
            // 
            this.DataGrid_Boletins = new System.Windows.Forms.DataGridView();
            this.pos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Insert = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.boletim_ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NotaPintura = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_Boletins)).BeginInit();
            this.DataGrid_Boletins.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.DataGrid_Boletins.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.DataGrid_Boletins.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.DataGrid_Boletins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid_Boletins.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pos,
            this.Insert,
            this.boletim_,
            this.Description,
            this.NotaPintura});
            this.DataGrid_Boletins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGrid_Boletins.Location = new System.Drawing.Point(0, 0);
            this.DataGrid_Boletins.Name = "DataGrid_Boletins";
            this.DataGrid_Boletins.RowHeadersVisible = false;
            this.DataGrid_Boletins.Size = new System.Drawing.Size(571, 562);
            this.DataGrid_Boletins.TabIndex = 1;
            // pos
            // 
            this.pos.HeaderText = "POS";
            this.pos.Name = "pos";
            this.pos.ReadOnly = true;
            this.pos.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.pos.Visible = false;
            this.pos.Width = 30;
            // 
            // Insert
            // 
            this.Insert.HeaderText = "Inserir";
            this.Insert.Name = "Insert";
            this.Insert.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Insert.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Insert.Width = 50;
            // boletim_
            // 
            this.boletim_.HeaderText = "Boletim";
            this.boletim_.Name = "boletim_";
            this.boletim_.ReadOnly = true;
            this.boletim_.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Description
            // 
            this.Description.HeaderText = "Descrição";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Description.Width = 200;
            // 
            // NotaPintura
            // 
            this.NotaPintura.HeaderText = "Nota Pintura";
            this.NotaPintura.Name = "NotaPintura";
            this.NotaPintura.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.NotaPintura.Width = 200;
            // 
            // frm_select_boletim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 562);
            this.Controls.Add(this.DataGrid_Boletins);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_select_boletim";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Boletim Técnico";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_select_boletim_FormClosing);
            this.Load += new System.EventHandler(this.frm_select_boletim_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_Boletins)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView DataGrid_Boletins;
        private System.Windows.Forms.DataGridViewTextBoxColumn pos;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Insert;
        private System.Windows.Forms.DataGridViewTextBoxColumn boletim_;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn NotaPintura;
    }
}