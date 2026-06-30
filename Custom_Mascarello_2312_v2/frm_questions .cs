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
    public partial class frm_questions : MetroFramework.Forms.MetroForm
    {
        private string exp_name;
        private double exp_valor;
        private Part workpart;

        public frm_questions(string exp_name, double exp_valor, Part workpart)
        {
            InitializeComponent();

            this.exp_name = exp_name;
            this.exp_valor = exp_valor;
            this.workpart = workpart;
        }
        private void frm_questions_Load(object sender, EventArgs e)
        {
            
        }
        
    }
}
