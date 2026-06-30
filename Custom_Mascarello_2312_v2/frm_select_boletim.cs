using AppCOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheckBox = System.Windows.Forms.CheckBox;

namespace Custom_Mascarello
{
    public partial class frm_select_boletim : Form
    {
        private string codigo;
        private string revisao;
        private int existe;
        private bool editar;
        public bool boletins = false;

        public frm_select_boletim(string codigo, string revisao, int existe, bool editar)
        {
            InitializeComponent();
            this.codigo = codigo;
            this.revisao = revisao;
            this.existe = existe;
            this.editar = editar;
        }
        private void DataGrid_Boletins_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (DataGrid_Boletins.IsCurrentCellDirty)
            {
                DataGrid_Boletins.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DataGrid_Boletins_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && DataGrid_Boletins.Columns[e.ColumnIndex].Name == "Insert")
            {
                bool isChecked = Convert.ToBoolean(DataGrid_Boletins.Rows[e.RowIndex].Cells["Insert"].Value);
                string boletimValue = GetBoletimValue(e.RowIndex);
                conexao_bd db = new conexao_bd();

                if (isChecked)
                {
                    db.insert_boletim(codigo, revisao, boletimValue, null);
                    AtualizarColunaNotaPintura(e.RowIndex, true, boletimValue);
                }
                else
                {
                    db.delete_boletim(codigo, revisao, boletimValue);
                    AtualizarColunaNotaPintura(e.RowIndex, false, boletimValue);
                }
            }
        }

        private void AtualizarColunaNotaPintura(int rowIndex, bool isChecked, string boletimValue)
        {
            if (isChecked)
            {
                var comboBoxCell = new DataGridViewComboBoxCell();
                conexao_bd db = new conexao_bd();

                DataSet dataSet = db.ColorSearch(boletimValue);

                comboBoxCell.Items.Add("");
                string validação = "";
                foreach (DataRow optionRow in dataSet.Tables[0].Rows)
                {
                    comboBoxCell.Items.Add(optionRow["info_color"].ToString());
                    validação = optionRow["info_color"].ToString();
                }
                if (validação != "")
                {
                    DataGrid_Boletins.Rows[rowIndex].Cells["NotaPintura"] = comboBoxCell;
                }
            }
            else
            {
                var textBoxCell = new DataGridViewTextBoxCell();
                textBoxCell.Value = "";
                DataGrid_Boletins.Rows[rowIndex].Cells["NotaPintura"] = textBoxCell;
            }
        }

        private void DataGrid_Boletins_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox comboBox && DataGrid_Boletins.CurrentCell.ColumnIndex == DataGrid_Boletins.Columns["NotaPintura"].Index)
            {
                comboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
                comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox && DataGrid_Boletins.CurrentCell != null)
            {
                int rowIndex = DataGrid_Boletins.CurrentCell.RowIndex;
                string colorValue = comboBox.SelectedItem?.ToString();
                string boletimValue = GetBoletimValue(rowIndex);

                if (!string.IsNullOrWhiteSpace(colorValue))
                {
                    string IDColor = conexao_bd.ReturnIDColor(colorValue);
                    if (!string.IsNullOrEmpty(IDColor))
                    {
                        conexao_bd.UpdateColor(codigo, revisao, boletimValue, IDColor);
                    }
                    else
                    {
                        MessageBox.Show("Nota de pintura inválida.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private string GetBoletimValue(int rowIndex)
        {
            object cellValue = DataGrid_Boletins.Rows[rowIndex].Cells["pos"].Value;
            return cellValue != null ? cellValue.ToString() : null;
        }

        private void frm_select_boletim_Load(object sender, EventArgs e)
        {
            conexao_bd busca_itens = new conexao_bd();
            DataSet _busca_itens = busca_itens.busca_boletim();

            foreach (DataRow row in _busca_itens.Tables[0].Rows)
            {
                int RowIndex = DataGrid_Boletins.Rows.Add();
                DataGrid_Boletins.Rows[RowIndex].Cells["boletim_"].Value = row["boletim"].ToString();
                DataGrid_Boletins.Rows[RowIndex].Cells["Description"].Value = row["descricao"].ToString();
                DataGrid_Boletins.Rows[RowIndex].Cells["pos"].Value = row["id"].ToString();
                DataGrid_Boletins.Rows[RowIndex].Cells["Insert"].Value = false;
                DataGrid_Boletins.Rows[RowIndex].Cells["NotaPintura"].Value = "";
            }

            if (existe == 1)
            {
                CarregarBoletinsSelecionados();
                existe = 0;
            }

            if (editar)
            {
                DataGrid_Boletins.CellValueChanged += DataGrid_Boletins_CellValueChanged;
                DataGrid_Boletins.CurrentCellDirtyStateChanged += DataGrid_Boletins_CurrentCellDirtyStateChanged;
                DataGrid_Boletins.EditingControlShowing += DataGrid_Boletins_EditingControlShowing;
            }
            else
            {
                DataGrid_Boletins.Enabled = false;
            }
        }

        private void CarregarBoletinsSelecionados()
        {
            conexao_bd db = new conexao_bd();
            DataSet _busca_itens_select = db.busca_boletim_selecionado(codigo, revisao);

            if (_busca_itens_select != null && _busca_itens_select.Tables.Count > 0)
            {
                foreach (DataRow row1 in _busca_itens_select.Tables[0].Rows)
                {
                    foreach (DataGridViewRow row in DataGrid_Boletins.Rows)
                    {
                        if (row.Cells["pos"].Value != null &&
                            row.Cells["pos"].Value.ToString() == row1["id_boletim"].ToString())
                        {
                            row.Cells["Insert"].Value = true;

                            var comboBoxCell = new DataGridViewComboBoxCell();
                            comboBoxCell.Items.Add("");

                            DataSet dataSet = db.ColorSearch(row1["id_boletim"].ToString());
                            string Validação = "";
                            foreach (DataRow optionRow in dataSet.Tables[0].Rows)
                            {
                                comboBoxCell.Items.Add(optionRow["info_color"].ToString());
                                Validação = optionRow["info_color"].ToString();
                            }
                            if (Validação != "")
                            {
                                row.Cells["NotaPintura"] = comboBoxCell;
                                comboBoxCell.Value = row1["info_color"].ToString();
                            }
                        }
                    }
                }
            }
        }

        private void frm_select_boletim_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (DataGridViewRow row in DataGrid_Boletins.Rows)
            {
                if (row.Cells["Insert"] != null && row.Cells["Insert"].Value != null && Convert.ToBoolean(row.Cells["Insert"].Value))
                {
                    boletins = true;
                }
            }
        }
    }
}
