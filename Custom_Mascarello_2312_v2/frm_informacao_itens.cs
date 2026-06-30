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
    public partial class frm_informacao_itens : Form
    {
        public frm_informacao_itens()
        {
            InitializeComponent();
        }
        public List<string> item_master { get; set; }
        public List<string> item_filho { get; set; } 


        private void frm_informacao_itens_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < item_master.Count; i++)
            {
                string[] quebra = item_master[i].Split(';');
              
                string[] rowi = { quebra[0], quebra[1] };

                dtg_inf.Rows.Add(rowi);
            }

            for (int j = 0; j < item_filho.Count; j++)
            {
                string[] quebra = item_filho[j].Split(';');
                string[] rowi = { quebra[0], quebra[1] };
                dtg_inf.Rows.Add(rowi); 
            }
        }
    }
}
