using NXOpen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Custom_Mascarello
{
    public partial class frm_edit_pos_compcs : MetroFramework.Forms.MetroForm
    {
        private string exp_name;
        private double exp_valor;
        private Part workpart;

        public frm_edit_pos_compcs(string exp_name, double exp_valor, Part workpart)
        {
            InitializeComponent();

            this.exp_name = exp_name;
            this.exp_valor = exp_valor;
            this.workpart = workpart;
        }
        private void frm_edit_pos_compcs_Load(object sender, EventArgs e)
        {
            txtbox_atual.Text = exp_valor.ToString();
            txtbox_nova.Text = exp_valor.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            txtbox_nova.Text = (Convert.ToDouble(txtbox_nova.Text) + Convert.ToDouble(txtbox_incremente.Text)).ToString();
            frm_PortaPacotes.preenche_expression(exp_name, txtbox_nova.Text, workpart);
        }

        private void txtbox_nova_TextChanged(object sender, EventArgs e)
        {
           
        }
        private void button2_Click(object sender, EventArgs e)
        {
            txtbox_nova.Text = (Convert.ToDouble(txtbox_nova.Text) - Convert.ToDouble(txtbox_incremente.Text)).ToString();
            frm_PortaPacotes.preenche_expression(exp_name, txtbox_nova.Text, workpart);
        }
        private void txtbox_nova_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            {
                frm_PortaPacotes.preenche_expression(exp_name, txtbox_nova.Text, workpart);
            }
        }
    }
}
