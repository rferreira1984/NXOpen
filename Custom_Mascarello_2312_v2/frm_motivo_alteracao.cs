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
    public partial class frm_motivo_alteracao : Form
    {
        string tipo = "";
        string desc ="";
        string decisao = "";
        public String Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
        public String Desc
        {
            get { return desc; }
            set { desc = value; }
        }
        public String Decisao
        {
            get { return decisao; }
            set { decisao = value; }
        }
        public frm_motivo_alteracao()
        {
            InitializeComponent();
        }

        private void frm_motivo_alteracao_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void btn_proximo_Click(object sender, EventArgs e)
        {
            tipo = comboBox1.Text;
            desc = textBox1.Text;
            decisao = "1";
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            tipo = "";
            desc = "";
            decisao = "0";

            this.Close();
        }
    }
}
