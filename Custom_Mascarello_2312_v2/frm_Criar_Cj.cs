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
    public partial class frm_Criar_Cj : Form
    {
        public frm_Criar_Cj()
        {
            InitializeComponent();
        }

        private void btn_criar_cjx_Click(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();
            BasePart basePart1;
            PartLoadStatus partLoadStatus1;
            basePart1 = theSession.Parts.OpenBaseDisplay("@DB/000068/A", out partLoadStatus1);

            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            partLoadStatus1.Dispose();
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Enter Gateway");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Enter Drafting");
            workPart.Views.WorkView.UpdateCustomSymbols();

            workPart.Drafting.SetTemplateInstantiationIsComplete(true);
            
            Expression expression1 = (Expression)workPart.Expressions.FindObject("VAO_TOTAL");
            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            workPart.Expressions.EditWithUnits(expression1, unit1, txt_vao_cj.Text);


            Expression busca_COMP_PC_1 = (Expression)workPart.Expressions.FindObject("COMP_PC_1");
            Expression busca_COMP_PC_2 = (Expression)workPart.Expressions.FindObject("COMP_PC_2");
            Expression busca_COMP_PC_3 = (Expression)workPart.Expressions.FindObject("COMP_PC_3");
            theSession.Preferences.Modeling.UpdatePending = false;

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Expression Data");

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId5);

            theSession.DeleteUndoMark(markId5, "Update Expression Data");

            theSession.Preferences.Modeling.UpdatePending = false;

            NXOpen.Session.UndoMarkId markId6;
            markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Expression Data");

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId6);

            theSession.DeleteUndoMark(markId6, "Update Expression Data");

            Expression busca_ANG_A_PC_1 = (Expression)workPart.Expressions.FindObject("ANG_A_PC_1");
            Expression busca_ANG_B_PC_1 = (Expression)workPart.Expressions.FindObject("ANG_B_PC_1");
            Expression busca_ANG_A_PC_2 = (Expression)workPart.Expressions.FindObject("ANG_A_PC_2");
            Expression busca_ANG_B_PC_2 = (Expression)workPart.Expressions.FindObject("ANG_B_PC_2");
            Expression busca_ANG_A_PC_3 = (Expression)workPart.Expressions.FindObject("ANG_A_PC_3");
            Expression busca_ANG_B_PC_3 = (Expression)workPart.Expressions.FindObject("ANG_B_PC_3");


            string FAM = "000162";

            string[] expressao = {"COMP", "ANG_AB", "ANG_BA"};

            double[] valor = {busca_COMP_PC_1.Value, busca_ANG_A_PC_1.Value, busca_ANG_B_PC_1.Value};
            double[] tolerancia = {1,0.1,0.1,0.1,0.1,0.1};
            string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");

            string[] expressao_pc_2 = {"COMP", "ANG_AB", "ANG_BA"};

            double[] valor_pc_2 = { busca_COMP_PC_2.Value, busca_ANG_A_PC_2.Value, busca_ANG_B_PC_2.Value };
            double[] tolerancia_pc_2 = { 1, 1,0.1,0.1,0.1,0.1};
            string arquivo_pc_2 = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao_pc_2, valor_pc_2, 0, tolerancia_pc_2, "", "TESTE");

            string FAM_CJ = "000068";

            string[] expressao_cj = {"VAO_TOTAL"};

            double[] valor_cj = { Convert.ToDouble(txt_vao_cj.Text)};
            double[] tolerancia_cj = { 1, 1};
            string arquivo_cj = Custom_Mascarello.Create.CreateMember(FAM_CJ, "CONJUNTO_X_", expressao_cj, valor_cj, 0, tolerancia_cj, "", "TESTE");
            this.txt_conjunto.Text = arquivo_cj;
        }

        private void frm_Criar_Cj_Load(object sender, EventArgs e)
        {

        }
        
    }
}
