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
    public partial class frm_selecionar_mp : Form
    {
        public List<string> lista_mp { get; set; }

        string _mp = "";
        public String MP_returnada
        {
            get { return _mp; }
            set { _mp = value; }
        }
        public frm_selecionar_mp()
        {
            InitializeComponent();
        }
       
        private void frm_selecionar_mp_Load(object sender, EventArgs e)
        {
            int pos_y = 30;
            for (int i = 0; i <= lista_mp.Count-1; i++)
            {
                RadioButton mp_disponivel = new RadioButton();
                mp_disponivel.Text = lista_mp[i];
                mp_disponivel.AutoSize = true;
                mp_disponivel.Location = new Point(5, pos_y);
                groupBox1.Controls.Add(mp_disponivel);
                pos_y = pos_y + 30;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (RadioButton item in groupBox1.Controls)
            {
                if (item.Checked == true)
                {
                    _mp = item.Text;
                }

            }
            if(_mp !="")
            {
                Close();
            }
            else
            {
                MessageBox.Show("A seleção de uma matéria-prima Obrigátoria para essa Familia", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
