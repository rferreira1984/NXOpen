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
    public partial class frm_result_search : Form
    {
        public static List<string> Resultado_Listar = new List<string>();
        public frm_result_search()
        {
            InitializeComponent();
            
        }

        private void frm_result_search_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Resultado_Listar.Count; i++)
            {
                string[] quebra = Resultado_Listar[i].Split(';');

                dtg_lista_itens.Rows.Add(quebra[0],quebra[1],quebra[2]);
                
            }
        }
        public void Resultado_search<T>(List<T> lista)
        {
            foreach (var t in lista)
            {
            }
        }

        private void dtg_lista_itens_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
