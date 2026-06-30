using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using NXOpen;
using NXOpen.UF;
using NXOpen.UIStyler;
using System.Windows.Forms;

namespace Custom_Mascarello
{
    public partial class frm_Mascarello : Form
    {
        public frm_Mascarello()
        {
            InitializeComponent();
        }

        private void frm_Mascarello_Load(object sender, EventArgs e)
        {
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Session theSession = Session.GetSession();
            //Part workPart = theSession.Parts.Work;
            //Part displayPart = theSession.Parts.Display;
            //// ----------------------------------------------
            ////   Menu: Tools->Expressions...
            //// ----------------------------------------------
            //theSession.Preferences.Modeling.UpdatePending = false;
            //int bt = (Convert.ToInt32(textBox1.Text) - Convert.ToInt32(textBox2.Text) - Convert.ToInt32(textBox3.Text));
            //double calc_bt = bt / Convert.ToInt32(textBox3.Text);

            //MessageBox.Show(Convert.ToString(calc_bt));
            //if (calc_bt <  0.71)
            //{
            //    NXOpen.Session.UndoMarkId markId1;
            //    markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Expression");
            //    string comp = textBox1.Text;
            //    string bd = textBox2.Text;
            //    string ee = textBox3.Text;

            //    Expression expression1 = (Expression)workPart.Expressions.FindObject("COMPRIMENTO");
            //    Expression expression2 = (Expression)workPart.Expressions.FindObject("BD");
            //    Expression expression3 = (Expression)workPart.Expressions.FindObject("EE");
            //    Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            //    workPart.Expressions.EditWithUnits(expression1, unit1, comp);
            //    workPart.Expressions.EditWithUnits(expression2, unit1, bd);
            //    workPart.Expressions.EditWithUnits(expression3, unit1, ee);

            //    theSession.Preferences.Modeling.UpdatePending = false;

            //    NXOpen.Session.UndoMarkId markId2;
            //    markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Expression Data");

            //    int nErrs1;
            //    nErrs1 = theSession.UpdateManager.DoUpdate(markId2);

            //    theSession.DeleteUndoMark(markId2, "Update Expression Data");

            //    theSession.Preferences.Modeling.UpdatePending = false;

            //    NXOpen.Session.UndoMarkId markId3;
            //    markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Expression Data");

            //    int nErrs2;
            //    nErrs2 = theSession.UpdateManager.DoUpdate(markId3);

            //    theSession.DeleteUndoMark(markId3, "Update Expression Data");
            //}
            //else
            //{
            //    MessageBox.Show("Balanço Traseiro fora da NORMA");
            //}
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_Fam_Click(object sender, EventArgs e)
        {
            //frm_Familias abrir = new frm_Familias();
            //abrir.Show();
        }

        private void btn_criar_cjx_Click(object sender, EventArgs e)
        {
            frm_Criar_Cj abrir = new frm_Criar_Cj();
            abrir.Show();
        }


        private void btn_config_Click(object sender, EventArgs e)
        {
            frm_Configurador_Projeto_M4 abrir = new frm_Configurador_Projeto_M4();
            abrir.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frm_Tubo abrir = new frm_Tubo();
            abrir.Show();
        }

 
    }
}
