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
    public partial class frm_pos_variaveis : MetroFramework.Forms.MetroForm
    {
        private string [] Vars;
        private string[] _result;
        public String [] _Result
        {
            get { return _result; }
            set { _result = value; }
        }
        public frm_pos_variaveis(string[] vars)
        {
            InitializeComponent();

            this.Vars = vars;
           
        }
        private void frm_edit_pos_compcs_Load(object sender, EventArgs e)
        {

            foreach (string var in Vars)
            {
               Label label = new Label();
                label.Text = var;
                label.Location = new System.Drawing.Point(10, 10);
                label.AutoSize = true;
                this.Controls.Add(label);

                TextBox textBox = new TextBox();
                textBox.Name = "txtbox_" + var;
                textBox.Location = new System.Drawing.Point(10, label.Location.X + label.Width);
                textBox.Width = 100;
                textBox.KeyPress += new KeyPressEventHandler(textBox1_KeyPress);
                this.Controls.Add(textBox);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void txtbox_nova_TextChanged(object sender, EventArgs e)
        {
           
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
        }
        private void txtbox_nova_KeyPress(object sender, KeyPressEventArgs e)
        {

           
        }

        private void frm_pos_variaveis_FormClosed(object sender, FormClosedEventArgs e)
        {
            string[] _var = new string[Vars.Length];
            foreach (Control ctl in this.Controls)
            {
                if (ctl is TextBox)
                {
                    string nome = ctl.Name.Replace("txtbox_", "");
                    int pos = Array.IndexOf(Vars, nome);
                    if (pos >= 0)
                    {
                        _var[pos] = ctl.Text;
                    }
               
                }
            }
            _result = _var;

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.Close();
            }
        }
    }
}
