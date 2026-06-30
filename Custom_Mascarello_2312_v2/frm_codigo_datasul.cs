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
    public partial class frm_codigo_datasul : Form
    {
        public frm_codigo_datasul(string desc)
        {
            InitializeComponent();
            txt_desc.Text = desc;
        }
        public String Codigo
        {
            get { return txt_cod.Text; }
            set { txt_cod.Text = value; }
        }
       
        private void frm_codigo_datasul_Load(object sender, EventArgs e)
        {         
           txt_cod.Focus();
        }
        private void txt_cod_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && txt_cod.Text !="")
            {
                Registrar_no_LOG(txt_cod.Text);
                this.Close();
            }
            if (e.KeyChar == 27 && txt_cod.Text != "")
            {
                Registrar_no_LOG(txt_cod.Text);
                this.Close();
            }
            
        }
        public static void Registrar_no_LOG(string Mensagem)
        {
            string gLog_FileName = @"c:\temp\" + "retorno.txt";
            System.IO.StreamWriter vWriter = new System.IO.StreamWriter(gLog_FileName, true, System.Text.Encoding.Unicode);
            vWriter.WriteLine(DateTime.Now.ToString() + " " + Mensagem);
            vWriter.Close();
        }

        private void txt_cod_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
