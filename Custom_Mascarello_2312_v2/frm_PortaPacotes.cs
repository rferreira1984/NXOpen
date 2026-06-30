using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using NXOpen;
using NXOpen.UF;
using System.IO;
using AppCOM;
using static NXOpen.UF.UFSfLegend;
using Newtonsoft.Json.Linq;
using NXOpen.BlockStyler;
using Syncfusion.Windows.Forms.Tools;
using Label = System.Windows.Forms.Label;
using TabControl = System.Windows.Forms.TabControl;
using NXOpen.VectorArithmetic;
using Point = NXOpen.Point;
using View = NXOpen.View;
using PLMComponents.Parasolid.PK_.Unsafe;
using static NXOpen.CAE.Post;
using NXOpen.PDM;
using NXOpen.Annotations;
using NXOpen.Positioning;

namespace Custom_Mascarello
{
    public partial class frm_PortaPacotes : Syncfusion.WinForms.Controls.SfForm
    {
        private static Session theSession;
        private static UI theUI;
        private static UFSession theUfSession;
       // public static List<string> alto_falantes_le = new List<string>();
        public static List<int> af_le = new List<int>();
        public static List<int> af_ld = new List<int>();
        string alto_falante_le = "";
        string alto_falante_ld = "";
        public static List<string> itens_nov_entre_foco = new List<string>();
        public static List<string> itens_nov_243121 = new List<string>();
        public static List<string> itens_nov_243175 = new List<string>();

        public static List<string> itens_nov_entre_foco_2 = new List<string>();
        public static List<string> itens_nov_madeira = new List<string>();
        public static List<string> itens_subst = new List<string>();

        string luz_eme_ld_1 = "";
        string luz_eme_ld_2 = "";
        string luz_eme_ld_3 = "";
        string luz_eme_le_1 = "";
        string luz_eme_le_2 = "";
        string luz_eme_le_3 = "";

        double tol_entre_foco = 2.0;

        static Session theSession1 = Session.GetSession();
        static UFSession theUFSession = UFSession.GetUFSession();
        static BasePart workPart = theSession1.Parts.BaseWork;

        public frm_PortaPacotes()
        {
            InitializeComponent();
        }
        private void frm_PortaPacotes_Load(object sender, EventArgs e)
        {
            this.tabControl1.TabPages.Remove(tabPage2);
            this.tabControl1.TabPages.Remove(tabPage1);
            af_le.Clear();
            af_ld.Clear();

            itens_nov_entre_foco.Clear();
            itens_nov_243121.Clear();
            itens_nov_243175.Clear();
            itens_subst.Clear();

            itens_nov_entre_foco_2.Clear();

            btn_codificar.Enabled = false;
            btn_cod_conj.Enabled = false;
        }

      
        public void carregar_dados()
        { 
        
            ////string path = @"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\APLICATIVOS\CONFIGURADOR_PORTA_PACOTES\xml\BD\" + txt_codigo.Text + ".xml";

            //XmlDocument doc = new XmlDocument();
            //doc.Load(path);
            //XElement xml = XElement.Load(path);
            //XmlNodeList Lista_dados = default(XmlNodeList);
            //string node = "/C"+txt_codigo.Text;
            //Lista_dados = doc.SelectNodes(node);

            //foreach (XmlNode dado in Lista_dados)
            //{
            //    for (int h = 0; h < dado.ChildNodes.Count; h++)
            //    {
            //        if (dado.ChildNodes.Item(h).Name.Contains("ckdbox_af_le"))
            //        {                      
            //            af_le.Add(Convert.ToInt32(dado.ChildNodes.Item(h).InnerText));
            //        }
            //        if (dado.ChildNodes.Item(h).Name.Contains("ckdbox_af_ld"))
            //        {
            //            af_ld.Add(Convert.ToInt32(dado.ChildNodes.Item(h).InnerText));
            //        }
            //    }
            //}
            //af_le.Sort();
            //af_ld.Sort();
            //for (int i = 0; i < af_le.Count ; i++)
            //{
            //    if (i + 1 == af_le.Count)
            //    {
            //        alto_falante_le += af_le[i];
            //    }
            //    else
            //    {
            //        alto_falante_le += af_le[i] + ",";
            //    }
            //}
            //for (int i = 0; i < af_ld.Count; i++)
            //{
            //    if (i + 1 == af_ld.Count)
            //    {
            //        alto_falante_ld += af_ld[i];
            //    }
            //    else
            //    {
            //        alto_falante_ld += af_ld[i] + ",";
            //    }
            //}
            //foreach (XmlNode dado in Lista_dados)
            //{
            //    for (int i = 0; i < dado.ChildNodes.Count; i++)
            //    {

            //        foreach (Control Tab in tableLayoutPanel1.Controls)
            //        {
            //            //if (Tab is TabControl)
            //            //{
            //            foreach (Control Tabpage in Tab.Controls)
            //            {
            //                foreach (Control txt_inf in Tabpage.Controls)
            //                {
            //                    if (txt_inf is TextBox && txt_inf.Name == dado.ChildNodes.Item(i).Name)
            //                    {
            //                        try
            //                        {
            //                            txt_inf.Text = dado.ChildNodes.Item(i).InnerText;
            //                        }
            //                        catch
            //                        {
            //                        }
            //                    }
            //                }
            //            }
            //            //}
            //        }
            //    }
            //}
            //int qtd_focos_le = Convert.ToInt32(txt_qtd_foco_le.Text);

            //int pos_y_le = 455;
            //for (int j = 2; j <= qtd_focos_le; j++)
            //{
            //    foreach (XmlNode dado in Lista_dados)
            //    {
            //        for (int h = 0; h < dado.ChildNodes.Count; h++)
            //        {
            //            if (dado.ChildNodes.Item(h).Name == "ID_passo_le_" + j)
            //            {
            //                TextBox txt_box_le = new TextBox();
            //                txt_box_le.Name = "ID_passo_le_" + j;
            //                txt_box_le.Size = new Size(50, 22);
            //                txt_box_le.Font = new Font("Microsoft Sans Serif", 9);
            //                txt_box_le.Text = dado.ChildNodes.Item(h).InnerText;
            //                txt_box_le.TextAlign = HorizontalAlignment.Center;
            //                txt_box_le.Location = new System.Drawing.Point(276, pos_y_le);
            //                txt_box_le.Visible = true;

            //                this.tabPage1.Controls.Add(txt_box_le);
            //                txt_box_le.BringToFront();

            //                Label lbl_le = new Label();
            //                lbl_le.Name = "lbl_ID_passo_le_" + j;
            //                lbl_le.Size = new Size(80, 22);
            //                lbl_le.Font = new Font("SansSerif", 11, FontStyle.Bold);
            //                lbl_le.Text = "Foco " + j.ToString() + ":";
            //                lbl_le.Location = new System.Drawing.Point(205, pos_y_le);
            //                lbl_le.Visible = true;

            //                this.tabPage1.Controls.Add(lbl_le);
            //                lbl_le.SendToBack();


            //                Label lbl_seta = new Label();
            //                lbl_seta.Name = "lbl_seta_le_" + j;
            //                lbl_seta.Size = new Size(24, 25);
            //                lbl_seta.Font = new Font("Microsoft Sans Serif", 16, FontStyle.Bold);
            //                lbl_seta.Text = "+";
            //                lbl_seta.Location = new System.Drawing.Point(287, pos_y_le - 27);
            //                lbl_seta.Visible = true;

            //                this.tabPage1.Controls.Add(lbl_seta);
            //                lbl_seta.SendToBack();
            //            }
            //        }
            //    }

            //    pos_y_le += 45;

            //}
            //int qtd_eme_le = Convert.ToInt32(cmb_luz_eme_le.Text);
            //int pos_y_le_eme = 206+22+5;
            //for (int k = 1; k <= qtd_eme_le; k++)
            //{
            //    foreach (XmlNode dado in Lista_dados)
            //    {
            //        for (int h = 0; h < dado.ChildNodes.Count; h++)
            //        {
            //            if (dado.ChildNodes.Item(h).Name == "txt_eme_le_" + k)
            //            {
            //                TextBox txt_box_le_eme = new TextBox();
            //                txt_box_le_eme.Name = "txt_eme_le_" + k;
            //                txt_box_le_eme.Size = new Size(50, 22);
            //                txt_box_le_eme.Font = new Font("Microsoft Sans Serif", 9);
            //                txt_box_le_eme.Text = dado.ChildNodes.Item(h).InnerText;
            //                txt_box_le_eme.TextAlign = HorizontalAlignment.Center;
            //                txt_box_le_eme.Location = new System.Drawing.Point(500, pos_y_le_eme);
            //                txt_box_le_eme.Visible = true;

            //                this.tabPage1.Controls.Add(txt_box_le_eme);
            //                txt_box_le_eme.BringToFront();

            //                Label lbl_le_eme = new Label();
            //                lbl_le_eme.Name = "lbl_ID_passo_le_" + k;
            //                lbl_le_eme.Size = new Size(90, 22);
            //                lbl_le_eme.Font = new Font("SansSerif", 11, FontStyle.Bold);
            //                lbl_le_eme.Text = "Luz Eme " + k.ToString() + ":";
            //                lbl_le_eme.Location = new System.Drawing.Point(397, pos_y_le_eme);
            //                lbl_le_eme.Visible = true;

            //                this.tabPage1.Controls.Add(lbl_le_eme);
            //                lbl_le_eme.SendToBack();



            //            }
            //        }
            //    }

            //    pos_y_le_eme += 25;

            //}

            //int qtd_focos_ld = Convert.ToInt32(txt_qtd_foco_ld.Text);

            //int pos_y_ld = 453;
            //for (int j = 2; j <= qtd_focos_ld; j++)
            //{

            //    foreach (XmlNode dado in Lista_dados)
            //    {
            //        for (int h = 0; h < dado.ChildNodes.Count; h++)
            //        {
            //            if (dado.ChildNodes.Item(h).Name == "ID_passo_ld_" + j)
            //            {
            //                TextBox txt_box_ld = new TextBox();
            //                txt_box_ld.Name = "ID_passo_ld_" + j;
            //                txt_box_ld.Size = new Size(50, 22);
            //                txt_box_ld.Font = new Font("Microsoft Sans Serif", 9);
            //                txt_box_ld.Text = dado.ChildNodes.Item(h).InnerText;
            //                txt_box_ld.TextAlign = HorizontalAlignment.Center;
            //                txt_box_ld.Location = new System.Drawing.Point(653, pos_y_ld);
            //                txt_box_ld.Visible = true;

            //                this.tabPage1.Controls.Add(txt_box_ld);
            //                txt_box_ld.BringToFront();

            //                Label lbl_ld = new Label();
            //                lbl_ld.Name = "lbl_ID_passo_ld_" + j;
            //                lbl_ld.Size = new Size(80, 22);
            //                lbl_ld.Font = new Font("SansSerif", 11, FontStyle.Bold);
            //                lbl_ld.Text = "Foco " + j.ToString() + ":";
            //                lbl_ld.Location = new System.Drawing.Point(585, pos_y_ld);
            //                lbl_ld.Visible = true;

            //                this.tabPage1.Controls.Add(lbl_ld);
            //                lbl_ld.SendToBack();

            //                Label lbl_seta = new Label();
            //                lbl_seta.Name = "lbl_seta_ld_" + j;
            //                lbl_seta.Size = new Size(24, 25);
            //                lbl_seta.Font = new Font("Microsoft Sans Serif", 16, FontStyle.Bold);
            //                lbl_seta.Text = "+";
            //                lbl_seta.Location = new System.Drawing.Point(665, pos_y_ld - 27);
            //                lbl_seta.Visible = true;

            //                this.tabPage1.Controls.Add(lbl_seta);
            //                lbl_seta.SendToBack();
            //            }
            //        }
            //    }

            //    pos_y_ld += 45;

            //}
            //int qtd_eme_ld = Convert.ToInt32(cmb_luz_eme_ld.Text);
            //int pos_y_ld_eme = 206 + 22 + 5; ;
            //for (int k = 1; k <= qtd_eme_ld; k++)
            //{
               
            //    foreach (XmlNode dado in Lista_dados)
            //    {
            //        for (int h = 0; h < dado.ChildNodes.Count; h++)
            //        {
            //            if (dado.ChildNodes.Item(h).Name == "txt_eme_ld_" + k)
            //            {
            //                TextBox txt_box_ld_eme = new TextBox();
            //                txt_box_ld_eme.Name = "txt_eme_ld_" + k;
            //                txt_box_ld_eme.Size = new Size(50, 22);
            //                txt_box_ld_eme.Font = new Font("Microsoft Sans Serif", 9);
            //                txt_box_ld_eme.Text = dado.ChildNodes.Item(h).InnerText;
            //                txt_box_ld_eme.TextAlign = HorizontalAlignment.Center;
            //                txt_box_ld_eme.Location = new System.Drawing.Point(738, pos_y_ld_eme);
            //                txt_box_ld_eme.Visible = true;

            //                this.tabPage1.Controls.Add(txt_box_ld_eme);
            //                txt_box_ld_eme.BringToFront();

            //                Label lbl_ld_eme = new Label();
            //                lbl_ld_eme.Name = "lbl_ID_passo_ld_" + k;
            //                lbl_ld_eme.Size = new Size(100, 22);
            //                lbl_ld_eme.Font = new Font("SansSerif", 11, FontStyle.Bold);
            //                lbl_ld_eme.Text = "Luz Eme " + k.ToString() + ":";
            //                lbl_ld_eme.Location = new System.Drawing.Point(635, pos_y_ld_eme);
            //                lbl_ld_eme.Visible = true;

            //                this.tabPage1.Controls.Add(lbl_ld_eme);
            //                lbl_ld_eme.SendToBack();
            //            }
            //        }
            //    }

            //    pos_y_ld_eme += 25;

            
        }
       
        public void aplicar_configuracao_new(double pos_ac)
        {

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            theSession.Preferences.Modeling.UpdatePending = false;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Session");

            Expression expression1 = (Expression)workPart.Expressions.FindObject("v_pos_ac");
            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            workPart.Expressions.EditWithUnits(expression1, unit1, Convert.ToString(pos_ac).Replace(",","."));
           

         
            //workPart.Expressions.EditWithUnits(expression16, unit1, txt_dist_C.Text);

            theSession.Preferences.Modeling.UpdatePending = true;

            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Session");
            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

            
            //  MessageBox.Show(c);
        }
        public void aplicar_configuracao()
        {
            
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
          
            theSession.Preferences.Modeling.UpdatePending = false;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Session");

            Expression expression1 = (Expression)workPart.Expressions.FindObject("COMP");
            Expression expression2 = (Expression)workPart.Expressions.FindObject("ENTRADA_USB");
            Expression expression3 = (Expression)workPart.Expressions.FindObject("LUZ_PENUMBRA");
            Expression expression4 = (Expression)workPart.Expressions.FindObject("AC");
            Expression expression5 = (Expression)workPart.Expressions.FindObject("POS_AC");
            Expression expression6 = (Expression)workPart.Expressions.FindObject("RECORTE_MAD");
            Expression expression7 = (Expression)workPart.Expressions.FindObject("POS_PRIMEIRO_FOCO_LD");
            Expression expression8 = (Expression)workPart.Expressions.FindObject("POS_PRIMEIRO_FOCO_LE");
            Expression expression9 = (Expression)workPart.Expressions.FindObject("WC");
            Expression expression10 = (Expression)workPart.Expressions.FindObject("TIPO_TECLA");
            Expression expression11 = (Expression)workPart.Expressions.FindObject("POS_FORC_LD");
            Expression expression12 = (Expression)workPart.Expressions.FindObject("POS_FORC_LE");
            Expression expression13 = (Expression)workPart.Expressions.FindObject("MED_D");
            Expression expression14 = (Expression)workPart.Expressions.FindObject("MED_A");
            Expression expression15 = (Expression)workPart.Expressions.FindObject("MED_B");
            Expression expression16 = (Expression)workPart.Expressions.FindObject("MED_C");
            Expression expression17 = (Expression)workPart.Expressions.FindObject("FOCO_AF_LD");
            Expression expression18 = (Expression)workPart.Expressions.FindObject("FOCO_AF_LE");
            Expression expression19 = (Expression)workPart.Expressions.FindObject("QTD_PES_LD");
            Expression expression20 = (Expression)workPart.Expressions.FindObject("QTD_PES_LE");
            try
            {
                Expression expression21 = (Expression)workPart.Expressions.FindObject("CARROCERIA");
                string valor_carroceria = "\"" + cmb_carroceria.Text + "\"";
                workPart.Expressions.EditWithUnits(expression21, null, valor_carroceria);
            }
            catch
            {
                
                
            }
            try
            {
                Expression expression22 = (Expression)workPart.Expressions.FindObject("CALEFACAO");
                Unit unit12 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
               // double valor = 0 ;
                if (cmb_calefacao.Text == "SIM")
                {
                    workPart.Expressions.EditWithUnits(expression22, unit12, "1");
                }
                else
                {
                    workPart.Expressions.EditWithUnits(expression22, unit12, "0");
                }
            }

            catch
            {


            }
            try
            {
                Expression expression23 = (Expression)workPart.Expressions.FindObject("MICROFONE");
                Unit unit12 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
                // double valor = 0 ;
                if (cmb_microfone.Text == "SIM") 
                {
                    workPart.Expressions.EditWithUnits(expression23, unit12, "1");
                }
                else
                {
                    workPart.Expressions.EditWithUnits(expression23, unit12, "0");
                }
            }
            catch
            {


            }

            try
            {
                Expression expression24 = (Expression)workPart.Expressions.FindObject("QTD_LUZ_EME_LE");
                Unit unit12 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
              
                {
                    workPart.Expressions.EditWithUnits(expression24, unit12,  cmb_luz_eme_le.Text); 
                }
                
            }
            catch
            {


            }
            try
            {
                Expression expression25 = (Expression)workPart.Expressions.FindObject("QTD_LUZ_EME_LD");
                Unit unit12 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

                {
                    workPart.Expressions.EditWithUnits(expression25, unit12, cmb_luz_eme_ld.Text);
                }

            }
            catch
            {


            }
            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

            workPart.Expressions.EditWithUnits(expression1, unit1, txt_CT.Text);

            if (cmb_usb.Text == "SIM")
            {
                workPart.Expressions.EditWithUnits(expression2, unit1, "1");
            }
            else
            {
                workPart.Expressions.EditWithUnits(expression2, unit1, "0");
            }
            if (cmb_penumbra.Text == "SIM") 
            {
                workPart.Expressions.EditWithUnits(expression3, unit1, "1");
            }
            else
            {
                workPart.Expressions.EditWithUnits(expression3, unit1, "0");
            }
            if (cmb_ac.Text == "SIM") 
            {
                workPart.Expressions.EditWithUnits(expression4, unit1, "1");
            }
            else
            {
                workPart.Expressions.EditWithUnits(expression4, unit1, "0");
            }
            workPart.Expressions.EditWithUnits(expression5, unit1, txt_ac.Text);
            workPart.Expressions.EditWithUnits(expression6, unit1, txt_vao_mad.Text);
            workPart.Expressions.EditWithUnits(expression7, unit1, ID_txt_foco_1_ld.Text); 
            workPart.Expressions.EditWithUnits(expression8, unit1, ID_txt_foco_1_le.Text);
            if (cmb_wc.Text == "SIM") 
            {
                workPart.Expressions.EditWithUnits(expression9, unit1, "1");
            }
            else
            {
                workPart.Expressions.EditWithUnits(expression9, unit1, "0");
            }

            string valor_tecla = "\"" + cmb_tecla.Text + "\"";
            workPart.Expressions.EditWithUnits(expression10, null, valor_tecla);

            workPart.Expressions.EditWithUnits(expression11, unit1, txt_pos_forc_ar_ld.Text);
            workPart.Expressions.EditWithUnits(expression12, unit1, txt_pos_forc_ar_le.Text);
            workPart.Expressions.EditWithUnits(expression13, unit1, txt_dist_inicio_ld.Text);
            workPart.Expressions.EditWithUnits(expression14, unit1, txt_dist_inicio_le.Text);
            workPart.Expressions.EditWithUnits(expression15, unit1, txt_dist_B.Text);
            workPart.Expressions.EditWithUnits(expression16, unit1, txt_dist_C.Text);

            string aplica_af_ld = "\"" + alto_falante_ld + "\"";
            string aplica_af_le = "\"" + alto_falante_le + "\"";
            
            Unit nullUnit = null;

            workPart.Expressions.EditWithUnits(expression17, nullUnit, aplica_af_ld);
            workPart.Expressions.EditWithUnits(expression18, nullUnit, aplica_af_le);
            //workPart.Expressions.EditWithUnits(expression16, unit1, txt_dist_C.Text);

            theSession.Preferences.Modeling.UpdatePending = true;
            
            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Session");
            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

            verificar_itens_le();
            verificar_itens_ld();

            string a = "";
            for (int i = 0; i <= itens_nov_entre_foco.Count - 1; i++)
            {
                a += itens_nov_entre_foco[i] + "\n";

            }
            //  MessageBox.Show(a);

            string b = "";
            for (int i = 0; i <= itens_nov_243121.Count - 1; i++)
            {
                b += itens_nov_243121[i] + "\n";

            }
            //    MessageBox.Show(b);

            string c = "";
            for (int i = 0; i <= itens_nov_243175.Count - 1; i++)
            {
                c += itens_nov_243175[i] + "\n";

            }
            //  MessageBox.Show(c);
    }
        public void aplicar_configuracao_2_PP_WAVE()
        {

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            Part part1 = (Part)theSession.Parts.FindObject("@DB/TPL_PORTA_PAC_WAVE/000");
            PartLoadStatus partLoadStatus1;
            NXOpen.PartCollection.SdpsStatus status1;
            status1 = theSession.Parts.SetDisplay(part1, false, true, out partLoadStatus1);

            workPart = theSession.Parts.Work;
            displayPart = theSession.Parts.Display;
            partLoadStatus1.Dispose();
            theSession.Preferences.Modeling.UpdatePending = false;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Session");

            Expression expression1 = (Expression)workPart.Expressions.FindObject("COMP");
            Expression expression2 = (Expression)workPart.Expressions.FindObject("POS_AC");
            Expression expression3 = (Expression)workPart.Expressions.FindObject("RECORTE_MAD");
            Expression expression4 = (Expression)workPart.Expressions.FindObject("POS_PRIMEIRO_FOCO_LD");
            Expression expression5 = (Expression)workPart.Expressions.FindObject("POS_PRIMEIRO_FOCO_LE");
            Expression expression6 = (Expression)workPart.Expressions.FindObject("WC");
            Expression expression7 = (Expression)workPart.Expressions.FindObject("POS_AR_FORCADO_LD");
            Expression expression8 = (Expression)workPart.Expressions.FindObject("POS_AR_FORCADO_LE");
            Expression expression9 = (Expression)workPart.Expressions.FindObject("INICIO_LD");
            Expression expression10 = (Expression)workPart.Expressions.FindObject("INICIO_LE");
            Expression expression11 = (Expression)workPart.Expressions.FindObject("AC");


            try
            {
                
                string mod_ac = "\"" + _txt_mod_ac.Text + "\"";
                workPart.Expressions.EditWithUnits(expression11, null, mod_ac);
            }
            catch
            {


            }
            try
            {
                Expression expression13 = (Expression)workPart.Expressions.FindObject("CARROCERIA");
                string valor_carroceria = "\"" + _txt_carroceria.Text + "\"";
                workPart.Expressions.EditWithUnits(expression13, null, valor_carroceria);
            }
            catch
            {


            }
          
            try
            {
                Expression expression14 = (Expression)workPart.Expressions.FindObject("QTD_LUZ_EME_LE");
                Unit unit12 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

                {
                    workPart.Expressions.EditWithUnits(expression14, unit12, _cmb_luz_eme_le.Text);
                }

            }
            catch
            {


            }
            try
            {
                Expression expression15 = (Expression)workPart.Expressions.FindObject("QTD_LUZ_EME_LD");
                Unit unit12 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

                {
                    workPart.Expressions.EditWithUnits(expression15, unit12, _cmb_luz_eme_ld.Text);
                }

            }
            catch
            {


            }
            Expression expression16 = (Expression)workPart.Expressions.FindObject("PRIMEIRA_POLT_IND");
            Expression expression17 = (Expression)workPart.Expressions.FindObject("QTD_PORTA_FOCO_LD");
            Expression expression18 = (Expression)workPart.Expressions.FindObject("QTD_PORTA_FOCO_LE");

            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

            workPart.Expressions.EditWithUnits(expression1, unit1, _txt_ct.Text);
            workPart.Expressions.EditWithUnits(expression2, unit1, _txt_pos_ac.Text);
            workPart.Expressions.EditWithUnits(expression3, unit1, _txt_vao_mad.Text);
            workPart.Expressions.EditWithUnits(expression4, unit1, _ID_txt_foco_1_ld.Text);
            workPart.Expressions.EditWithUnits(expression5, unit1, _ID_txt_foco_1_le.Text);
            if (_txt_wc.Text != "5.1.1 - Não")
            {
                workPart.Expressions.EditWithUnits(expression6, unit1, "1");
            }
            else
            {
                workPart.Expressions.EditWithUnits(expression6, unit1, "0");
            }

            workPart.Expressions.EditWithUnits(expression7, unit1, _txt_pos_forc_ar_ld.Text);
            workPart.Expressions.EditWithUnits(expression8, unit1, _txt_pos_forc_ar_le.Text);
            workPart.Expressions.EditWithUnits(expression9, unit1, _txt_dist_inicio_ld.Text);
            workPart.Expressions.EditWithUnits(expression10, unit1, _txt_dist_inicio_le.Text);

            workPart.Expressions.EditWithUnits(expression17, unit1, _txt_qtd_foco_ld.Text);
            workPart.Expressions.EditWithUnits(expression18, unit1, _txt_qtd_foco_le.Text);

            if (_cmb_polt_ind.Text == "SIM")
            {
                workPart.Expressions.EditWithUnits(expression16, unit1, "1");
            }
            else
            {
                workPart.Expressions.EditWithUnits(expression16, unit1, "0");
            }

          
                   
           Expression expression19 = (Expression)workPart.Expressions.FindObject("M_LUZ_EME_LD_1");
           workPart.Expressions.EditWithUnits(expression19, unit1, luz_eme_ld_1);
            Expression expression20 = (Expression)workPart.Expressions.FindObject("M_LUZ_EME_LD_2");
            workPart.Expressions.EditWithUnits(expression20, unit1, luz_eme_ld_2);
            Expression expression21 = (Expression)workPart.Expressions.FindObject("M_LUZ_EME_LD_2");
            workPart.Expressions.EditWithUnits(expression20, unit1, luz_eme_ld_3);

            Expression expression22 = (Expression)workPart.Expressions.FindObject("M_LUZ_EME_LE_1");
            workPart.Expressions.EditWithUnits(expression22, unit1, luz_eme_le_1);
            Expression expression23 = (Expression)workPart.Expressions.FindObject("M_LUZ_EME_LE_2");
            workPart.Expressions.EditWithUnits(expression23, unit1, luz_eme_le_2);
            Expression expression24 = (Expression)workPart.Expressions.FindObject("M_LUZ_EME_LE_3");
            workPart.Expressions.EditWithUnits(expression24, unit1, luz_eme_le_3);




            Unit nullUnit = null;

            int total_polt_ld = Convert.ToInt32(_txt_qtd_foco_ld.Text);
            int total_polt_le = Convert.ToInt32(_txt_qtd_foco_le.Text);
            if (_cmb_polt_ind.Text == "SIM")
            {
                total_polt_ld = total_polt_ld - 1;
            }

            int n_polt_dupla_rest = (total_polt_le - total_polt_ld);
            int n_pares_polt_dup = (total_polt_le - n_polt_dupla_rest) * 2;

            int n_aux = 1;

            int cont_le = 1;
            int cont_ld = 1;
            for (int i = 1; i <= n_pares_polt_dup; i++)
            {

                if (i % 2 != 0)
                {
                    string exp_1_le = "N_LE_" + cont_le.ToString();
                    string exp_2_le = "N_LE_" + (cont_le+1).ToString();
                    Expression expression_le_1 = (Expression)workPart.Expressions.FindObject(exp_1_le);
                    Expression expression_le_2 = (Expression)workPart.Expressions.FindObject(exp_2_le);

                    string v_1 = "\"" + (n_aux).ToString().PadLeft(2, '0') + "\"";
                    string v_2 = "\"" + (n_aux+1).ToString().PadLeft(2, '0') + "\"";
                    workPart.Expressions.EditWithUnits(expression_le_1, null, v_1);
                    workPart.Expressions.EditWithUnits(expression_le_2, null, v_2);
                    cont_le += 2;
                    n_aux += 2;
                }
                else
                {
                    string exp_1_ld = "N_LD_" + cont_ld.ToString();
                    string exp_2_ld = "N_LD_" + (cont_ld + 1).ToString();
                    Expression expression_ld_1 = (Expression)workPart.Expressions.FindObject(exp_1_ld);
                    Expression expression_ld_2 = (Expression)workPart.Expressions.FindObject(exp_2_ld);

                    string v_1 = "\"" + (n_aux).ToString().PadLeft(2, '0') + "\"";
                    string v_2 = "\"" + (n_aux + 1).ToString().PadLeft(2, '0') + "\"";

                    workPart.Expressions.EditWithUnits(expression_ld_1, null, v_1);
                    workPart.Expressions.EditWithUnits(expression_ld_2, null, v_2);

                    cont_ld += 2;
                    n_aux += 2;
                }

            }
            for (int i = 1; i <= n_polt_dupla_rest; i++)
            {
                string exp_1_le = "N_LE_" + cont_le.ToString();
                string exp_2_le = "N_LE_" + (cont_le + 1).ToString();
                Expression expression_le_1 = (Expression)workPart.Expressions.FindObject(exp_1_le);
                Expression expression_le_2 = (Expression)workPart.Expressions.FindObject(exp_2_le);

                string v_1 = "\"" + (n_aux).ToString().PadLeft(2, '0') + "\"";
                string v_2 = "\"" + (n_aux + 1).ToString().PadLeft(2, '0') + "\"";

                workPart.Expressions.EditWithUnits(expression_le_1, null, v_1);
                workPart.Expressions.EditWithUnits(expression_le_2, null, v_2);

                cont_le += 2;
                n_aux += 2;
            }
            if(_cmb_polt_central.Text =="SIM")
            {
                Expression expression_central = (Expression)workPart.Expressions.FindObject("N_CENTRAL");
               
                string v_1 = "\"" + (n_aux).ToString().PadLeft(2, '0') + "\"";
                workPart.Expressions.EditWithUnits(expression_central, null, v_1);
                n_aux++;
            }
            if (_cmb_polt_ind.Text == "SIM")
            {
                Expression expression_central = (Expression)workPart.Expressions.FindObject("N_INDIVIDUAL");

                string v_1 = "\"" + (n_aux).ToString().PadLeft(2, '0') + "\"";
                workPart.Expressions.EditWithUnits(expression_central, null, v_1);

              
            }


            int n_polt_le = Convert.ToInt32(_txt_qtd_foco_le.Text);
            double pos_incial = Convert.ToDouble(_ID_txt_foco_1_le.Text);
            for (int i = 1; i <= n_polt_le; i++)
            {
                if (i == 1)
                {
                    Expression expression25 = (Expression)workPart.Expressions.FindObject("P_" + i.ToString() + "_LE");
                    workPart.Expressions.EditWithUnits(expression25, unit1, _ID_txt_foco_1_le.Text);
                }
                else
                {
                    string name_exp = "P_" + i.ToString() + "_LE";
                    string name_posicao = "_ID_passo_le_" + i.ToString();

                    foreach (Control Tab in tableLayoutPanel1.Controls)
                    {
                        foreach (Control Tabpage in Tab.Controls)
                        {
                            if (Tabpage.Name == "tabPage2")
                            {

                                foreach (Control txt_inf in Tabpage.Controls)
                                {
                                    if (txt_inf is TextBox && txt_inf.Name == name_posicao)
                                    {
                                        try
                                        {
                                            pos_incial += Convert.ToDouble(txt_inf.Text);
                                            Expression expression25 = (Expression)workPart.Expressions.FindObject(name_exp);
                                            workPart.Expressions.EditWithUnits(expression25, unit1, pos_incial.ToString());
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (_cmb_polt_ind.Text != "SIM")
            {
                int n_polt_ld = Convert.ToInt32(_txt_qtd_foco_ld.Text);
                pos_incial = Convert.ToDouble(_ID_txt_foco_1_ld.Text);
                for (int i = 1; i <= n_polt_ld; i++)
                {


                    if (i == 1)
                    {
                        Expression expression25 = (Expression)workPart.Expressions.FindObject("P_" + i.ToString() + "_LD");
                        workPart.Expressions.EditWithUnits(expression25, unit1, _ID_txt_foco_1_ld.Text);
                    }
                    else
                    {
                        string name_exp = "P_" + i.ToString() + "_LD";
                        string name_posicao = "_ID_passo_ld_" + i.ToString();

                        // MessageBox.Show(name_posicao);
                        foreach (Control Tab in tableLayoutPanel1.Controls)
                        {
                            foreach (Control Tabpage in Tab.Controls)
                            {
                                if (Tabpage.Name == "tabPage2")
                                {

                                    foreach (Control txt_inf in Tabpage.Controls)
                                    {
                                        if (txt_inf is TextBox && txt_inf.Name == name_posicao)
                                        {
                                            try
                                            {
                                                pos_incial += Convert.ToDouble(txt_inf.Text);
                                                Expression expression25 = (Expression)workPart.Expressions.FindObject(name_exp);
                                                workPart.Expressions.EditWithUnits(expression25, unit1, pos_incial.ToString());
                                            }
                                            catch
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                int n_polt_ld = Convert.ToInt32(_txt_qtd_foco_ld.Text);
                double pos_incial_ld = Convert.ToDouble(50);

                for (int i = 1; i <= n_polt_ld; i++)
                {


                    if (i == 1)
                    {
                        Expression expression25 = (Expression)workPart.Expressions.FindObject("P_1_INDIVIDUAL");
                        workPart.Expressions.EditWithUnits(expression25, unit1, "50");
                    }
                    if (i != 1)
                    {
                        int valor_aux = i;
                        int valor_aux_2 = i - 1;

                        string name_exp = "P_" + valor_aux_2.ToString() + "_LD";
                        string name_posicao = "_ID_passo_ld_" + valor_aux.ToString();
                        foreach (Control Tab in tableLayoutPanel1.Controls)
                        {
                            foreach (Control Tabpage in Tab.Controls)
                            {
                                if (Tabpage.Name == "tabPage2")
                                {
                                    foreach (Control txt_inf in Tabpage.Controls)
                                    {
                                        if (txt_inf is TextBox && txt_inf.Name == name_posicao)
                                        {

                                            try
                                            {
                                                pos_incial_ld += Convert.ToDouble(txt_inf.Text.Replace('.', ','));
                                                Expression expression25 = (Expression)workPart.Expressions.FindObject(name_exp);
                                                workPart.Expressions.EditWithUnits(expression25, unit1, pos_incial_ld.ToString());
                                            }
                                            catch
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }



            theSession.Preferences.Modeling.UpdatePending = true;

            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Session");
            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

            Part part2 = (Part)theSession.Parts.FindObject("@DB/TPL_EM_PORTA_PAC_WAVE/000");
            PartLoadStatus partLoadStatus2;
            NXOpen.PartCollection.SdpsStatus status2;
            status2 = theSession.Parts.SetDisplay(part2, false, true, out partLoadStatus2);

            workPart = theSession.Parts.Work;
            displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();

        }
        public void aplicar_configuracao_2_EM_PP_WAVE()
        {

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            theSession.Preferences.Modeling.UpdatePending = false;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Session");

            Expression expression1 = (Expression)workPart.Expressions.FindObject("COMP");
            Expression expression2 = (Expression)workPart.Expressions.FindObject("POS_AC");
            Expression expression3 = (Expression)workPart.Expressions.FindObject("RECORTE_MAD");
            Expression expression4 = (Expression)workPart.Expressions.FindObject("POS_PRIMEIRO_FOCO_LD");
            Expression expression5 = (Expression)workPart.Expressions.FindObject("POS_PRIMEIRO_FOCO_LE");
            Expression expression6 = (Expression)workPart.Expressions.FindObject("WC");
            Expression expression7 = (Expression)workPart.Expressions.FindObject("POS_AR_FORCADO_LD");
            Expression expression8 = (Expression)workPart.Expressions.FindObject("POS_AR_FORCADO_LE");
            Expression expression9 = (Expression)workPart.Expressions.FindObject("INICIO_PP_LD");
            Expression expression10 = (Expression)workPart.Expressions.FindObject("INICIO_PP_LE");
            //Expression expression11 = (Expression)workPart.Expressions.FindObject("QTD_PES_LD");
            //Expression expression12 = (Expression)workPart.Expressions.FindObject("QTD_PES_LE");
            //Expression expression26 = (Expression)workPart.Expressions.FindObject("QTD_PES_LD_AC");
            //Expression expression27 = (Expression)workPart.Expressions.FindObject("QTD_PES_LE_AC");

            try
            {
                Expression expression13 = (Expression)workPart.Expressions.FindObject("CARROCERIA");
                string valor_carroceria = "\"" + _txt_carroceria.Text + "\"";
                workPart.Expressions.EditWithUnits(expression13, null, valor_carroceria);
            }
            catch
            {


            }

            try
            {
                Expression expression14 = (Expression)workPart.Expressions.FindObject("QTD_LUZ_EME_LE");
                Unit unit12 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

                {
                    workPart.Expressions.EditWithUnits(expression14, unit12, _cmb_luz_eme_le.Text);
                }

            }
            catch
            {


            }
            try
            {
                Expression expression15 = (Expression)workPart.Expressions.FindObject("QTD_LUZ_EME_LD");
                Unit unit12 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

                {
                    workPart.Expressions.EditWithUnits(expression15, unit12, _cmb_luz_eme_ld.Text);
                }

            }
            catch
            {


            }
            Expression expression16 = (Expression)workPart.Expressions.FindObject("PRIMEIRA_POLT_IND");
            Expression expression17 = (Expression)workPart.Expressions.FindObject("QTD_PORTA_FOCO_LD");
            Expression expression18 = (Expression)workPart.Expressions.FindObject("QTD_PORTA_FOCO_LE");

            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

            workPart.Expressions.EditWithUnits(expression1, unit1, _txt_ct.Text);
            workPart.Expressions.EditWithUnits(expression2, unit1, _txt_pos_ac.Text);
            workPart.Expressions.EditWithUnits(expression3, unit1, _txt_vao_mad.Text);
            workPart.Expressions.EditWithUnits(expression4, unit1, _ID_txt_foco_1_ld.Text);
            workPart.Expressions.EditWithUnits(expression5, unit1, _ID_txt_foco_1_le.Text);
            if (_txt_wc.Text != "5.1.1 - Não")
            {
                workPart.Expressions.EditWithUnits(expression6, unit1, "1");
            }
            else
            {
                workPart.Expressions.EditWithUnits(expression6, unit1, "0");
            }

            workPart.Expressions.EditWithUnits(expression7, unit1, _txt_pos_forc_ar_ld.Text);
            workPart.Expressions.EditWithUnits(expression8, unit1, _txt_pos_forc_ar_le.Text);
            workPart.Expressions.EditWithUnits(expression9, unit1, _txt_dist_inicio_ld.Text);
            workPart.Expressions.EditWithUnits(expression10, unit1, _txt_dist_inicio_le.Text);
            workPart.Expressions.EditWithUnits(expression17, unit1, _txt_qtd_foco_ld.Text);
            workPart.Expressions.EditWithUnits(expression18, unit1, _txt_qtd_foco_le.Text);

            if (_cmb_polt_ind.Text == "SIM")
            {
                workPart.Expressions.EditWithUnits(expression16, unit1, "1");
            }
            else
            {
                workPart.Expressions.EditWithUnits(expression16, unit1, "0");
            }



            Expression expression19 = (Expression)workPart.Expressions.FindObject("M_LUZ_EME_LD_1");
            workPart.Expressions.EditWithUnits(expression19, unit1, luz_eme_ld_1);
            Expression expression20 = (Expression)workPart.Expressions.FindObject("M_LUZ_EME_LD_2");
            workPart.Expressions.EditWithUnits(expression20, unit1, luz_eme_ld_2);
            Expression expression21 = (Expression)workPart.Expressions.FindObject("M_LUZ_EME_LD_3");
            workPart.Expressions.EditWithUnits(expression20, unit1, luz_eme_ld_3);

            Expression expression22 = (Expression)workPart.Expressions.FindObject("M_LUZ_EME_LE_1");
            workPart.Expressions.EditWithUnits(expression22, unit1, luz_eme_le_1);
            Expression expression23 = (Expression)workPart.Expressions.FindObject("M_LUZ_EME_LE_2");
            workPart.Expressions.EditWithUnits(expression23, unit1, luz_eme_le_2);
            Expression expression24 = (Expression)workPart.Expressions.FindObject("M_LUZ_EME_LE_3");
            workPart.Expressions.EditWithUnits(expression24, unit1, luz_eme_le_3);

            Unit nullUnit = null;

            theSession.Preferences.Modeling.UpdatePending = true;

            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Session");
            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);


        }
        public void criar_itens()
        {
            if (itens_nov_entre_foco != null)
            {
                for (int i = 0; i <= itens_nov_entre_foco.Count - 1; i++)
                {
                    string[] qebra_str = itens_nov_entre_foco[i].Split(';');
                    string cod_fam = "214642";

                    double comp = Convert.ToDouble(qebra_str[1]);
                    string[] expressao = { "COMP", };
                    double[] valor = { comp };
                    double[] tolerancia = { 0.1, 0.1 };
                    string material = "";


                    if (Convert.ToInt32(comp) <= 700)
                    {
                        material = "169360 - PRF_PLAST_PVC_FRONT_PORTA_PAC_ROMA_370_700_MM|BibliotecaMascarello.xml";
                    }
                    if (Convert.ToInt32(comp) > 700 && Convert.ToInt32(comp) <= 800)
                    {
                        material = "169362 - PRF_PLAST_PVC_FRONT_PORTA_PAC_ROMA_370_800_MM|BibliotecaMascarello.xml";
                    }
                    if (Convert.ToInt32(comp) > 800 && Convert.ToInt32(comp) <= 900)
                    {
                        material = "169363 - PRF_PLAST_PVC_FRONT_PORTA_PAC_ROMA_370_900_MM|BibliotecaMascarello.xml";
                    }
                    if (Convert.ToInt32(comp) > 900 && Convert.ToInt32(comp) <= 2000)
                    {
                        material = "097518 - PRF_PLAST_PVC_FRONT_PORTA_PAC_ROMA_370_2000_MM|BibliotecaMascarello.xml";
                    }
                    string codigo = "";
                    string descricao = "PRF_ENTRE_FOCO_PORTA_PAC_" + comp + "mm";

                    string arquivo;
                   
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                    if (arquivo == "")
                    {
                        arquivo = Custom_Mascarello.Create_c_material.CreateMember(cod_fam, descricao, expressao, valor, 0, tolerancia, codigo, descricao, material);
                    }  
                }
            }
            if (itens_nov_243121!= null) 
            {
                for (int i = 0; i <= itens_nov_243121.Count - 1; i++)
                {
                    string[] qebra_str = itens_nov_243121[i].Split(';');
                    string cod_fam = "243121";
                    double LADO = Convert.ToDouble(qebra_str[1]);
                    double COMP = Convert.ToDouble(qebra_str[2]);
                    double POS_AC = Convert.ToDouble(qebra_str[3]);
                    double ABERT_MAD = Convert.ToDouble(qebra_str[4]);
                    double POS_FORCADO = Convert.ToDouble(qebra_str[5]);

                    string[] expressao = { "COMP", "LADO", "POS_AC", "ABERT_MAD", "POS_FORCADO" };
                    double[] valor = { COMP, LADO, POS_AC, ABERT_MAD, POS_FORCADO };
                    double[] tolerancia = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };

                    string lado = "LD";
                    if (LADO == 1)
                    {
                        lado = "LE";
                    }
                    string ac = "";
                    if(POS_AC != 0)
                    {
                        ac = "_C_AC"; 
                    }
                    string codigo = "";
                    string descricao = "MAD_PORTA_PAC_ROMA_" + COMP + "mm_" + "ABERT_MAD_" + valor[3].ToString() + "_POS_FORCADO_" +valor[4].ToString()+ lado + ac;
                    string arquivo = "";


                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");
                    if (arquivo == "")
                    {
                        //frm_codigo_datasul frm = new frm_codigo_datasul(descricao);
                        //frm.ShowDialog();
                        //codigo = frm.Codigo;
                        arquivo = Custom_Mascarello.Create.CreateMember(cod_fam, descricao, expressao, valor, 0, tolerancia, codigo, descricao);
                     // string save = Custom_Mascarello.frmPrincipal.Save_Fam(cod_fam);
                    }
                    itens_subst.Add(qebra_str[6] + ";" + arquivo + ";" + qebra_str[7]);               
                }

            }

            if (itens_nov_243175 != null)
            {
                for (int i = 0; i <= itens_nov_243175.Count - 1; i++)
                {                 
                    string[] qebra_str = itens_nov_243175[i].Split(';');
                    string cod_fam = "243175";
                   
                    double COMP = Convert.ToDouble(qebra_str[1]);
                  

                    string[] expressao = { "COMP"};
                    double[] valor = { COMP};
                    double[] tolerancia = { 0.1, 0.1};

                    string codigo = "";
                    string descricao = "PRF_Z_TRAS_PORTA_PAC_ROMA_" + COMP + "mm";

                    string arquivo = "";
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                    if (arquivo == "")
                    {
                    //frm_codigo_datasul frm = new frm_codigo_datasul(descricao);
                    //frm.ShowDialog();
                    //codigo = frm.Codigo;
                     arquivo = Custom_Mascarello.Create.CreateMember(cod_fam, descricao, expressao, valor, 0, tolerancia, codigo, descricao);
                      //string save = Custom_Mascarello.frmPrincipal.Save_Fam(cod_fam);

                    }
                    itens_subst.Add(qebra_str[3] + ";" + arquivo + ";" + qebra_str[2]);
                }
            }
        }
        public void criar_itens_WAVE()
        {
            if (itens_nov_entre_foco != null)
            {
                for (int i = 0; i <= itens_nov_entre_foco.Count - 1; i++)
                {
                    string[] qebra_str = itens_nov_entre_foco[i].Split(';');
                    string cod_fam = "288383";

                    double comp = Convert.ToDouble(qebra_str[1]);
                    string[] expressao = { "COMP", };
                    double[] valor = { comp };
                    double[] tolerancia = { 0.1, 0.1 };
                    string codigo = "";
                    string descricao = "PRF_ENTRE_FOCO_PORTA_PAC_CONV_2019_" + comp + "mm";
                    string arquivo;
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                    if (arquivo == "")
                    {
                        arquivo = Custom_Mascarello.Create.CreateMember(cod_fam, descricao, expressao, valor, 0, tolerancia, codigo, descricao);
                    }
                }
            }
            if (itens_nov_madeira != null)
            {
                for (int i = 0; i <= itens_nov_madeira.Count - 1; i++)
                {
                    string[] qebra_str = itens_nov_madeira[i].Split(';');
                    string cod_fam = "288324";
                    double LADO = Convert.ToDouble(qebra_str[2]);
                    double COMP = Convert.ToDouble(qebra_str[1]);
                    double POS_AC = Convert.ToDouble(qebra_str[3]);
                    double ABERT_MAD = Convert.ToDouble(qebra_str[4]);
                    double POS_FORCADO = Convert.ToDouble(qebra_str[5]);

                    string[] expressao = { "COMP", "LADO", "POS_AC", "ABERT_MAD", "POS_FORCADO" };
                    double[] valor = { COMP, LADO, POS_AC, ABERT_MAD, POS_FORCADO };
                    double[] tolerancia = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };

                    string lado = "LD";
                    if (LADO == 1)
                    {
                        lado = "LE";
                    }
                    string ac = "";
                    if (POS_AC != 0)
                    {
                        ac = "_C_AC";
                    }
                    string codigo = "";
                    string descricao = "MAD_PORTA_PAC_CONV_" + COMP + "mm_" + "ABERT_MAD_" + valor[3].ToString() + "_POS_FORCADO_" + valor[4].ToString() +"_"+ lado + ac;
                    string arquivo = "";


                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");
                    if (arquivo == "")
                    {
                        //frm_codigo_datasul frm = new frm_codigo_datasul(descricao);
                        //frm.ShowDialog();
                        //codigo = frm.Codigo;
                        arquivo = Custom_Mascarello.Create.CreateMember(cod_fam, descricao, expressao, valor, 0, tolerancia, codigo, descricao);
                        // string save = Custom_Mascarello.frmPrincipal.Save_Fam(cod_fam);
                    }

                    itens_subst.Add(qebra_str[0] + ";" + arquivo + ";" + lado);
                }

            }
        }

        public void add_foco_le_new( double pos_x_foco, int i)
        {
            Session theSession = Session.GetSession();
        
            Part wp_pp = theSession.Parts.Work;

            
            Part displayPart = theSession.Parts.Display;
            displayPart = theSession.Parts.Display;
           


            string codigo_foco = "315807";

            try
            {
                theSession.Parts.SetNonmasterSeedPartData("@DB/" + codigo_foco + "/001");

                BasePart basePart1;
                PartLoadStatus partLoadStatus1;
                basePart1 = theSession.Parts.OpenBase("@DB/" + codigo_foco + "/001", out partLoadStatus1);

                partLoadStatus1.Dispose();
                int nErrs1;
            }
            catch (Exception)
            {


            }
                Point3d basePoint1;
                Matrix3x3 orientation1;
                
                    basePoint1 = new Point3d(772.354303620, pos_x_foco-450, 1509.551904890);

                    orientation1.Xx = -0.999374200;
                    orientation1.Xy = 0.0;
                    orientation1.Xz = 0.035372429;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = -1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.035372429;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 0.999374200;

                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;

                try
                {
                    component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + codigo_foco + "/000", "REP_MONTAGEM", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                }
                catch
                {

                    component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + codigo_foco, "REP_MONTAGEM", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                }


                

                NXObject[] objects1 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);

            NXOpen.Positioning.ComponentPositioner componentPositioner1;
            componentPositioner1 = wp_pp.ComponentAssembly.Positioner;

            NXOpen.Assemblies.Arrangement arrangement1 = ((NXOpen.Assemblies.Arrangement)wp_pp.ComponentAssembly.Arrangements.FindObject("Arrangement 1"));
            componentPositioner1.PrimaryArrangement = arrangement1;


            NXOpen.Positioning.Constraint constraint1;
            constraint1 = componentPositioner1.CreateConstraint(true);
            NXOpen.Positioning.ComponentConstraint componentConstraint1 = ((NXOpen.Positioning.ComponentConstraint)constraint1);
            componentConstraint1.ConstraintType = NXOpen.Positioning.Constraint.Type.Distance;
            NXOpen.DatumPlane datumPlane1 = ((NXOpen.DatumPlane)wp_pp.Datums.FindObject("DATUM_CSYS(0) XY plane"));
            NXOpen.Positioning.ConstraintReference constraintReference1;
            constraintReference1 = componentConstraint1.CreateConstraintReference(wp_pp.ComponentAssembly, datumPlane1, false, false, false);

            NXOpen.Line line1 = ((NXOpen.Line)component1.FindObject("PROTO#.Features|LINE(39)|CURVE 1"));
            NXOpen.Positioning.ConstraintReference constraintReference2;
            constraintReference2 = componentConstraint1.CreateConstraintReference(component1, line1, false, false, false);



            constraintReference2.SetFixHint(true);

            componentConstraint1.SetExpression("0");

            componentConstraint1.SetExpression("v_f_le" + (i + 1).ToString());

        }
        public void add_polt(double pos_x, double pox_y, string name)
        {
            Session theSession = Session.GetSession();

            Part wp_pp = theSession.Parts.Work;


            Part displayPart = theSession.Parts.Display;
            displayPart = theSession.Parts.Display;



            string codigo_foco = "REP_POLTRONA";

            try
            {
                theSession.Parts.SetNonmasterSeedPartData("@DB/" + codigo_foco + "/000");

                BasePart basePart1;
                PartLoadStatus partLoadStatus1;
                basePart1 = theSession.Parts.OpenBase("@DB/" + codigo_foco + "/000", out partLoadStatus1);

                partLoadStatus1.Dispose();
                int nErrs1;
            }
            catch (Exception)
            {


            }
            Point3d basePoint1;
            Matrix3x3 orientation1;

            basePoint1 = new Point3d(pox_y, pos_x,0);

            orientation1.Xx = 1.0;
            orientation1.Xy = 0.0;
            orientation1.Xz = 0.0;
            orientation1.Yx = 0.0;
            orientation1.Yy = 1.0;
            orientation1.Yz = 0.0;
            orientation1.Zx = 0.0;
            orientation1.Zy = 0.0;
            orientation1.Zz = 1.0;

            PartLoadStatus partLoadStatus3;
            NXOpen.Assemblies.Component component1;

            try
            {
                component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + codigo_foco + "/000","MODEL", "REP_POLTRONA", basePoint1, orientation1, -1, out partLoadStatus3, true);
            }
            catch
            {

                component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + codigo_foco, "MODEL", "REP_POLTRONA", basePoint1, orientation1, -1, out partLoadStatus3, true);
            }




            NXObject[] objects1 = new NXObject[0];
            int nErrs3;
            nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);

            NXOpen.Positioning.ComponentPositioner componentPositioner1;
            componentPositioner1 = wp_pp.ComponentAssembly.Positioner;

            NXOpen.Assemblies.Arrangement arrangement1 = ((NXOpen.Assemblies.Arrangement)wp_pp.ComponentAssembly.Arrangements.FindObject("Arrangement 1"));
            componentPositioner1.PrimaryArrangement = arrangement1;


            NXOpen.Positioning.Constraint constraint1;
            constraint1 = componentPositioner1.CreateConstraint(true);
            NXOpen.Positioning.ComponentConstraint componentConstraint1 = ((NXOpen.Positioning.ComponentConstraint)constraint1);
            componentConstraint1.ConstraintType = NXOpen.Positioning.Constraint.Type.Distance;
            NXOpen.DatumPlane datumPlane1 = ((NXOpen.DatumPlane)wp_pp.Datums.FindObject("DATUM_CSYS(0) XY plane"));
            NXOpen.Positioning.ConstraintReference constraintReference1;
            constraintReference1 = componentConstraint1.CreateConstraintReference(wp_pp.ComponentAssembly, datumPlane1, false, false, false);

            NXOpen.Line line1 = ((NXOpen.Line)component1.FindObject("PROTO#.Features|SKETCH(2)|SKETCH_001|HANDLE R-11491"));
            NXOpen.Positioning.ConstraintReference constraintReference2;
            constraintReference2 = componentConstraint1.CreateConstraintReference(component1, line1, false, false, false);



            constraintReference2.SetFixHint(true);

            componentConstraint1.SetExpression("0");

            componentConstraint1.SetExpression(name);

        }
        public void add_foco_ld_new(double pos_x_foco, int i)
        {
            Session theSession = Session.GetSession();
        
            Part wp_pp = theSession.Parts.Work;

            
            Part displayPart = theSession.Parts.Display;
            displayPart = theSession.Parts.Display;
           


            string codigo_foco = "315807";

            try
            {
                theSession.Parts.SetNonmasterSeedPartData("@DB/" + codigo_foco + "/001");

                BasePart basePart1;
                PartLoadStatus partLoadStatus1;
                basePart1 = theSession.Parts.OpenBase("@DB/" + codigo_foco + "/001", out partLoadStatus1);

                partLoadStatus1.Dispose();
                int nErrs1;
            }
            catch (Exception)
            {


            }
            Point3d basePoint1;
            Matrix3x3 orientation1;

            basePoint1 = new Point3d(-772.469299297, pos_x_foco-450, 1509.393030205);

            orientation1.Xx = 0.999362018;
            orientation1.Xy = 0.0;
            orientation1.Xz = 0.035714939;
            orientation1.Yx = 0.0;
            orientation1.Yy = 1.0;
            orientation1.Yz = 0.0;
            orientation1.Zx = -0.035714939;
            orientation1.Zy = 0.0;
            orientation1.Zz = 0.999362018;

            PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;

                try
                {
                    component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + codigo_foco + "/000", "REP_MONTAGEM", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                }
                catch
                {

                    component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + codigo_foco, "REP_MONTAGEM", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                }


                

                NXObject[] objects1 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);

            
            NXOpen.Positioning.ComponentPositioner componentPositioner1;
            componentPositioner1 = wp_pp.ComponentAssembly.Positioner;

            NXOpen.Assemblies.Arrangement arrangement1 = ((NXOpen.Assemblies.Arrangement)wp_pp.ComponentAssembly.Arrangements.FindObject("Arrangement 1"));
            componentPositioner1.PrimaryArrangement = arrangement1;


            NXOpen.Positioning.Constraint constraint1;
            constraint1 = componentPositioner1.CreateConstraint(true);
            NXOpen.Positioning.ComponentConstraint componentConstraint1 = ((NXOpen.Positioning.ComponentConstraint)constraint1);
                    componentConstraint1.ConstraintType = NXOpen.Positioning.Constraint.Type.Distance;
                   NXOpen.DatumPlane datumPlane1 = ((NXOpen.DatumPlane)wp_pp.Datums.FindObject("DATUM_CSYS(0) XY plane"));
                 NXOpen.Positioning.ConstraintReference constraintReference1;
                  constraintReference1 = componentConstraint1.CreateConstraintReference(wp_pp.ComponentAssembly, datumPlane1, false, false, false);

            NXOpen.Line line1 = ((NXOpen.Line)component1.FindObject("PROTO#.Features|LINE(39)|CURVE 1"));
            NXOpen.Positioning.ConstraintReference constraintReference2;
            constraintReference2 = componentConstraint1.CreateConstraintReference(component1, line1, false, false, false);



            constraintReference2.SetFixHint(true);

            componentConstraint1.SetExpression("0");

            componentConstraint1.SetExpression("v_f_ld"+(i+1).ToString());


        }
        public void add_foco_le()
        {
            Session theSession = Session.GetSession();           
            Part wp_pai = theSession.Parts.Work;
            Part wp_pp = theSession.Parts.Work;

            NXOpen.Assemblies.Component pp_le;
            if (cmb_carroceria.Text == "S4")
            {
                 pp_le = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_MICRO_S4/000 1");
            }
            else
            {
                pp_le = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_LE_ROMA/000 1");
            }
            PartLoadStatus partLoadStatus2;

            theSession.Parts.SetWorkComponent(pp_le, out partLoadStatus2);
       

            wp_pp = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();


            string codigo_foco = "";

            if( cmb_tecla.Text  == "PUSH PULL" && cmb_usb.Text == "SIM")
            {
                codigo_foco = "199157";
            }
            if (cmb_tecla.Text == "PUSH PULL" && cmb_usb.Text == "NÃO")
            {
                codigo_foco = "199158";
            }
            if (cmb_tecla.Text == "GANGORRA" && cmb_usb.Text == "SIM")
            {
                codigo_foco = "189104";
            }
            if (cmb_tecla.Text == "GANGORRA" && cmb_usb.Text == "NÃO")
            {
                codigo_foco = "181897";
            }
            try
            {
                theSession.Parts.SetNonmasterSeedPartData("@DB/" + codigo_foco + "/000");

                BasePart basePart1;
                PartLoadStatus partLoadStatus1;
                basePart1 = theSession.Parts.OpenBase("@DB/" + codigo_foco + "/000", out partLoadStatus1);

                partLoadStatus1.Dispose();
                int nErrs1;
            }
            catch (Exception)
            {


            }
            
            int qtd_foco_le = Convert.ToInt32(txt_qtd_foco_le.Text);
            double pos_foc_le = Convert.ToDouble(txt_dist_inicio_le.Text) + Convert.ToDouble(ID_txt_foco_1_le.Text);

            for (int i = 0; i <= qtd_foco_le-2 ; i++) 
            {
                string  name_passo = "ID_passo_le_"+ Convert.ToString(i+2);
                foreach (Control control in tableLayoutPanel1.Controls)
                {
                    if (control is TabControl)
                    {
                        foreach (TabPage page in control.Controls)
                        {
                            if (page is TabPage && page.Name == "tabPage1")
                            {
                                    foreach ( Control txt_box in page.Controls)
                                    {
                                        if(txt_box is TextBox && txt_box.Name == name_passo)
                                    {
                                        pos_foc_le += Convert.ToDouble(txt_box.Text);
                                    }
                            }
                            }
                        }
                    }
                }
                Point3d basePoint1;
                Matrix3x3 orientation1;
                if (cmb_carroceria.Text == "S4")
                {
                    basePoint1 = new Point3d(725.537135158, pos_foc_le, 1509.413097544);
                    orientation1.Xx = -1.0;
                    orientation1.Xy = 0.0;
                    orientation1.Xz = 0.0;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = -1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.0;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 1.0;
                }
                else
                {
                    basePoint1 = new Point3d(752.354303620, pos_foc_le, 1509.551904890);

                    orientation1.Xx = -0.999374200;
                    orientation1.Xy = 0.0;
                    orientation1.Xz = 0.035372429;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = -1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.035372429;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 0.999374200;
                }

                    PartLoadStatus partLoadStatus3;
                    NXOpen.Assemblies.Component component1;

                    try
                    {
                        component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + codigo_foco + "/000", "REP_MONTAGEM", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                    }
                    catch
                    {

                        component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + codigo_foco, "REP_MONTAGEM", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                    }

              
                partLoadStatus2.Dispose();

                    NXObject[] objects1 = new NXObject[0];
                    int nErrs3;
                    nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
            }

      

            NXOpen.Assemblies.Component Volta_Assem_Comp = null;
            PartLoadStatus partLoadStatus_Volta;
            theSession.Parts.SetWorkComponent(Volta_Assem_Comp, out partLoadStatus_Volta);
            wp_pai = theSession.Parts.Work;
            partLoadStatus_Volta.Dispose();
        }
        public void add_foco_le_WAVE()
        {
            Session theSession = Session.GetSession();
            Part wp_pai = theSession.Parts.Work;
            Part wp_pp = theSession.Parts.Work;

            NXOpen.Assemblies.Component pp_le;
            pp_le = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_CJ_PORTA_PAC_LE_WAVE/000 1");
            PartLoadStatus partLoadStatus2;

            theSession.Parts.SetWorkComponent(pp_le, out partLoadStatus2);


            wp_pp = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();


            string codigo_foco = "267923";

            try
            {
                theSession.Parts.SetNonmasterSeedPartData("@DB/" + codigo_foco + "/000");

                BasePart basePart1;
                PartLoadStatus partLoadStatus1;
                basePart1 = theSession.Parts.OpenBase("@DB/" + codigo_foco + "/000", out partLoadStatus1);

                partLoadStatus1.Dispose();
                int nErrs1;
            }
            catch (Exception)
            {


            }

            int qtd_foco_le = Convert.ToInt32(_txt_qtd_foco_le.Text);
            double pos_foc_le = Convert.ToDouble(_txt_dist_inicio_le.Text) + Convert.ToDouble(_ID_txt_foco_1_le.Text);

            for (int i = 0; i <= qtd_foco_le - 2; i++)
            {

                string name_passo = "_ID_passo_le_" + Convert.ToString(i + 2);

                foreach (Control Tab in tableLayoutPanel1.Controls)
                {
                    //if (Tab is TabControl)
                    //{
                    foreach (Control Tabpage in Tab.Controls)
                    {
                        if (Tabpage.Name == "tabPage2")
                        {

                            foreach (Control txt_inf in Tabpage.Controls)
                            {
                                if (txt_inf is TextBox && txt_inf.Name == name_passo)
                                {
                                    try
                                    {
                                        pos_foc_le += Convert.ToDouble(txt_inf.Text);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
                Point3d basePoint1;
                Matrix3x3 orientation1;
                
                    basePoint1 = new Point3d(825, pos_foc_le, 1553);
                    orientation1.Xx = -1.0;
                    orientation1.Xy = 0.0;
                    orientation1.Xz = 0.0;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = -1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.0;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 1.0;
                

                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;

                try
                {
                    component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + codigo_foco + "/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                }
                catch
                {

                    component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + codigo_foco, "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                }


                partLoadStatus2.Dispose();

                NXObject[] objects1 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
            }

            NXOpen.Assemblies.Component Volta_Assem_Comp = null;
            PartLoadStatus partLoadStatus_Volta;
            theSession.Parts.SetWorkComponent(Volta_Assem_Comp, out partLoadStatus_Volta);
            wp_pai = theSession.Parts.Work;
            partLoadStatus_Volta.Dispose();

        }
        public void add_foco_ld_WAVE()
        {
            Session theSession = Session.GetSession();
            Part wp_pai = theSession.Parts.Work;
            Part wp_pp = theSession.Parts.Work;

            NXOpen.Assemblies.Component pp_ld;
           
                pp_ld = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_CJ_PORTA_PAC_LD_WAVE/000 1");


            PartLoadStatus partLoadStatus2;

            theSession.Parts.SetWorkComponent(pp_ld, out partLoadStatus2);


            wp_pp = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();


            string codigo_foco ="267923";
            try
            {
                theSession.Parts.SetNonmasterSeedPartData("@DB/" + codigo_foco + "/000");

                BasePart basePart1;
                PartLoadStatus partLoadStatus1;
                basePart1 = theSession.Parts.OpenBase("@DB/" + codigo_foco + "/000", out partLoadStatus1);

                partLoadStatus1.Dispose();
                int nErrs1;
            }
            catch (Exception)
            {


            }

            int qtd_foco_ld = Convert.ToInt32(_txt_qtd_foco_ld.Text);
            double pos_foc_ld = Convert.ToDouble(_txt_dist_inicio_ld.Text) + Convert.ToDouble(_ID_txt_foco_1_ld.Text.Replace('.',','));

            for (int i = 0; i <= qtd_foco_ld - 2; i++)
            {
                string name_passo = "_ID_passo_ld_" + Convert.ToString(i + 2);

                foreach (Control Tab in tableLayoutPanel1.Controls)
                {
                    //if (Tab is TabControl)
                    //{
                    foreach (Control Tabpage in Tab.Controls)
                    {
                        if (Tabpage.Name == "tabPage2")
                        {

                            foreach (Control txt_inf in Tabpage.Controls)
                            {
                                if (txt_inf is TextBox && txt_inf.Name == name_passo)
                                {
                                    try
                                    {
                                        pos_foc_ld += Convert.ToDouble(txt_inf.Text);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
             
                Point3d basePoint1;
                Matrix3x3 orientation1;

                    basePoint1 = new Point3d(-823, pos_foc_ld, 1553);
                    orientation1.Xx = 1.0;
                    orientation1.Xy = 0.0;
                    orientation1.Xz = 0.0;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = 1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.0;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 1.0;


                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;
                component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + codigo_foco + "/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);

                partLoadStatus2.Dispose();

                NXObject[] objects1 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
            }

            NXOpen.Assemblies.Component Volta_Assem_Comp = null;
            PartLoadStatus partLoadStatus_Volta;
            theSession.Parts.SetWorkComponent(Volta_Assem_Comp, out partLoadStatus_Volta);
            wp_pai = theSession.Parts.Work;
            partLoadStatus_Volta.Dispose();

        }
        public void add_eme_le()
        {
            Session theSession = Session.GetSession();
            Part wp_pai = theSession.Parts.Work;
            Part wp_pp = theSession.Parts.Work;

            NXOpen.Assemblies.Component pp_le;
            if (cmb_carroceria.Text == "S4")
            {
                pp_le = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_MICRO_S4/000 1");
            }
            else
            {
                pp_le = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_LE_ROMA/000 1");
            }

            PartLoadStatus partLoadStatus2;

            theSession.Parts.SetWorkComponent(pp_le, out partLoadStatus2);


            wp_pp = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();


            string codigo_luz_eme = "112532";

            
            try
            {
                theSession.Parts.SetNonmasterSeedPartData("@DB/" + codigo_luz_eme + "/000");

                BasePart basePart1;
                PartLoadStatus partLoadStatus1;
                basePart1 = theSession.Parts.OpenBase("@DB/" + codigo_luz_eme + "/000", out partLoadStatus1);

                partLoadStatus1.Dispose();
                int nErrs1;
            }
            catch (Exception)
            {


            }

            int qtd_luz_le = Convert.ToInt32(cmb_luz_eme_le.Text); 
            double pos_luz_le = 0;

            for (int i = 0; i <= qtd_luz_le - 1; i++)
            {

                string name_passo = "txt_eme_le_" + Convert.ToString(i + 1);
                foreach (Control control in tableLayoutPanel1.Controls)
                {
                    if (control is TabControl)
                    {
                        foreach (TabPage page in control.Controls)
                        {
                            if (page is TabPage && page.Name == "tabPage1")
                            {
                                foreach (Control txt_box in page.Controls)
                                {
                                    if (txt_box is TextBox && txt_box.Name == name_passo)
                                    {
                                        pos_luz_le = Convert.ToDouble(txt_box.Text);
                                    }
                                }
                            }
                        }
                    }
                }
                Point3d basePoint1 = new Point3d(703, pos_luz_le, 1500.013960788);
                Matrix3x3 orientation1;

                if (cmb_carroceria.Text == "S4")
                {
                    basePoint1 = new Point3d(703, pos_luz_le, 1500.013960788);
                    orientation1.Xx = 0.0;
                    orientation1.Xy = 1.0;
                    orientation1.Xz = 0.0;
                    orientation1.Yx = 1.0;
                    orientation1.Yy = 0.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.0;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = -1.0;
                }
                else
                {
                    basePoint1 = new Point3d(690.964847580, pos_luz_le, 1511.458229133);
                    orientation1.Xx = 0.0;
                    orientation1.Xy = 0.994521895;
                    orientation1.Xz = -0.104528463;
                    orientation1.Yx = 1.0;
                    orientation1.Yy = 0.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.0;
                    orientation1.Zy = -0.104528463;
                    orientation1.Zz = -0.994521895;
                }
                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;
                component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + codigo_luz_eme + "/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);

                partLoadStatus2.Dispose();

                NXObject[] objects1 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
            }

            NXOpen.Assemblies.Component Volta_Assem_Comp = null;
            PartLoadStatus partLoadStatus_Volta;
            theSession.Parts.SetWorkComponent(Volta_Assem_Comp, out partLoadStatus_Volta);
            wp_pai = theSession.Parts.Work;
            partLoadStatus_Volta.Dispose();

        }
        public void add_eme_ld()
        {
            Session theSession = Session.GetSession();
            Part wp_pai = theSession.Parts.Work;
            Part wp_pp = theSession.Parts.Work;

            NXOpen.Assemblies.Component pp_ld;
            if (cmb_carroceria.Text == "S4")
            {
                pp_ld = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_MICRO_S4_LD/000 1");
            }
            else
            {
                pp_ld = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_LD_ROMA/000 1");
            }

            PartLoadStatus partLoadStatus2;

            theSession.Parts.SetWorkComponent(pp_ld, out partLoadStatus2);


            wp_pp = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();


            string codigo_luz_eme = "112532";


            try
            {
                theSession.Parts.SetNonmasterSeedPartData("@DB/" + codigo_luz_eme + "/000");

                BasePart basePart1;
                PartLoadStatus partLoadStatus1;
                basePart1 = theSession.Parts.OpenBase("@DB/" + codigo_luz_eme + "/000", out partLoadStatus1);

                partLoadStatus1.Dispose();
                int nErrs1;
            }
            catch (Exception)
            {


            }

            int qtd_luz_ld = Convert.ToInt32(cmb_luz_eme_ld.Text);
            double pos_luz_ld = 0;

            for (int i = 0; i <= qtd_luz_ld - 1; i++)
            {

                string name_passo = "txt_eme_ld_" + Convert.ToString(i + 1);


                foreach (Control control in tableLayoutPanel1.Controls)
                {
                    if (control is TabControl)
                    {
                        foreach (TabPage page in control.Controls)
                        {
                            if (page is TabPage && page.Name == "tabPage1")
                            {
                                foreach (Control txt_box in page.Controls)
                                {
                                    if (txt_box is TextBox && txt_box.Name == name_passo)
                                    {
                                        pos_luz_ld = Convert.ToDouble(txt_box.Text);
                                    }
                                }
                            }
                        }
                    }
                }

                Point3d basePoint1; ;
                Matrix3x3 orientation1;
                if (cmb_carroceria.Text == "S4")
                {
                     basePoint1 = new Point3d(-703, pos_luz_ld, 1500.013960788);
                    orientation1.Xx = 0.0;
                    orientation1.Xy = -1.0;
                    orientation1.Xz = 0.0;
                    orientation1.Yx = -1.0;
                    orientation1.Yy = 0.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.0;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = -1.0;
                }
                else
                {
                    basePoint1 = new Point3d(-690.964847580, pos_luz_ld, 1511.458229133);
                    orientation1.Xx = 0.0;
                    orientation1.Xy = -0.994521895;
                    orientation1.Xz = 0.104528463;
                    orientation1.Yx = -1.0;
                    orientation1.Yy = 0.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.0;
                    orientation1.Zy = -0.104528463;
                    orientation1.Zz = -0.994521895;
                }
                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;
                component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + codigo_luz_eme + "/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);

                partLoadStatus2.Dispose();

                NXObject[] objects1 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
            }

            NXOpen.Assemblies.Component Volta_Assem_Comp = null;
            PartLoadStatus partLoadStatus_Volta;
            theSession.Parts.SetWorkComponent(Volta_Assem_Comp, out partLoadStatus_Volta);
            wp_pai = theSession.Parts.Work;
            partLoadStatus_Volta.Dispose();

        }
        public void add_entre_foco_le()
        {
            Session theSession = Session.GetSession();
            Part wp_pai = theSession.Parts.Work;
            Part wp_pp = theSession.Parts.Work;

            NXOpen.Assemblies.Component pp_le;
            if (cmb_carroceria.Text == "S4")
            {
                pp_le = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_MICRO_S4/000 1");
            }
            else
            {
                pp_le = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_LE_ROMA/000 1");
            }

            PartLoadStatus partLoadStatus2;

            theSession.Parts.SetWorkComponent(pp_le, out partLoadStatus2);


            wp_pp = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();

            int qtd_foco_le = Convert.ToInt32(txt_qtd_foco_le.Text);
            double pos_entre_foco_le = Convert.ToDouble(txt_dist_inicio_le.Text) + Convert.ToDouble(ID_txt_foco_1_le.Text);

            string arquivo = "";
            string comp_ant = "";
            string comp = "";
            for (int i = 1; i <= qtd_foco_le-1; i++)
            {
                string name_passo = "ID_passo_le_" + Convert.ToString(i + 1);

                    foreach (Control control in tableLayoutPanel1.Controls)
                    {
                        if (control is TabControl)
                        {
                            foreach (TabPage page in control.Controls)
                            {
                                if (page is TabPage && page.Name == "tabPage1")
                                {
                                    foreach (Control txt_box in page.Controls)
                                    {
                                        if (txt_box is TextBox && txt_box.Name == name_passo)
                                        {
                                            comp = Convert.ToString(Convert.ToDouble(txt_box.Text) - 162);
                                            pos_entre_foco_le += (Convert.ToDouble(txt_box.Text));
                                        }
                                    }
                                }
                            }
                        }
                    }
                
                if (comp != comp_ant)
                {
                    arquivo = "";
                }
                if (comp != comp_ant)
                {
                   
                    string cod_fam = "214642";

                    double var_Comp = Convert.ToDouble(comp);
                    string[] expressao = { "COMP" };
                    double[] valor = { var_Comp };
                    double[] tolerancia = { tol_entre_foco };

                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");
                    
                }


                try
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + arquivo);

                    BasePart basePart1;
                    PartLoadStatus partLoadStatus1;
                    basePart1 = theSession.Parts.OpenBase("@DB/" + arquivo, out partLoadStatus1);

                    partLoadStatus1.Dispose();
                    int nErrs1;
                }
                catch (Exception)
                {


                }

                Point3d basePoint1;
                Matrix3x3 orientation1;
                if (cmb_carroceria.Text == "S4")
                {
                    basePoint1 = new Point3d(916.516138807, pos_entre_foco_le - 81.0, 1500.020002648);
                    orientation1.Xx = -0.996960856;
                    orientation1.Xy = 0.0;
                    orientation1.Xz = 0.077904114;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = -1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.077904114;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 0.996960856;

                }
                else
                {
                    basePoint1 = new Point3d(942.858884121, pos_entre_foco_le - 81.0, 1494.059200783);
                    orientation1.Xx = -0.994034448;
                    orientation1.Xy = 0.0;
                    orientation1.Xz = 0.109066569;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = -1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.109066569;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 0.994034448;
                }
                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;
               // MessageBox.Show(arquivo);
                try
                {
                    component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + arquivo, "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);

                    partLoadStatus2.Dispose();

                    NXObject[] objects1 = new NXObject[0];
                    int nErrs3;
                    nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                }
                catch
                {


                }

                comp_ant = comp;

            }

            NXOpen.Assemblies.Component Volta_Assem_Comp = null;
            PartLoadStatus partLoadStatus_Volta;
            theSession.Parts.SetWorkComponent(Volta_Assem_Comp, out partLoadStatus_Volta);
            wp_pai = theSession.Parts.Work;
            partLoadStatus_Volta.Dispose();



        }
        public void add_entre_foco_le_WAVE()
        {
            Session theSession = Session.GetSession();
            Part wp_pai = theSession.Parts.Work;
            Part wp_pp = theSession.Parts.Work;

            NXOpen.Assemblies.Component pp_le;

            pp_le = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_CJ_PORTA_PAC_LE_WAVE/000 1");

            PartLoadStatus partLoadStatus2;

            theSession.Parts.SetWorkComponent(pp_le, out partLoadStatus2);


            wp_pp = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();

            int qtd_foco_le = Convert.ToInt32(_txt_qtd_foco_le.Text);
            double pos_entre_foco_le = Convert.ToDouble(_txt_dist_inicio_le.Text) + Convert.ToDouble(_ID_txt_foco_1_le.Text);

            string arquivo = "";
            string comp_ant = "";
            string comp = "";
            for (int i = 1; i <= qtd_foco_le; i++)
            {

                string name_passo = "_ID_passo_le_" + Convert.ToString(i + 1);
                foreach (Control Tab in tableLayoutPanel1.Controls)
                {
                    //if (Tab is TabControl)
                    //{
                    foreach (Control Tabpage in Tab.Controls)
                    {
                        if (Tabpage.Name == "tabPage2")
                        {

                            foreach (Control txt_inf in Tabpage.Controls)
                            {
                                if (txt_inf is TextBox && txt_inf.Name == name_passo)
                                {
                                    try
                                    {

                                        comp = Convert.ToString(Convert.ToDouble(txt_inf.Text) - 84);
                                        pos_entre_foco_le += (Convert.ToDouble(txt_inf.Text));
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
                if (comp != comp_ant)
                {
                    arquivo = "";
                }
                if (comp != comp_ant)
                {
                    string cod_fam = "288383";

                    double var_Comp = Convert.ToDouble(comp);
                    string[] expressao = { "COMP" };
                    double[] valor = { var_Comp };
                    double[] tolerancia = { tol_entre_foco };

                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }


                try
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + arquivo);

                    BasePart basePart1;
                    PartLoadStatus partLoadStatus1;
                    basePart1 = theSession.Parts.OpenBase("@DB/" + arquivo, out partLoadStatus1);

                    partLoadStatus1.Dispose();
                    int nErrs1;
                }
                catch (Exception)
                {


                }

                Point3d basePoint1;
                Matrix3x3 orientation1;

                double comp_deslocar = Convert.ToDouble(comp)+42;
                    basePoint1 = new Point3d(816.1, (pos_entre_foco_le)- comp_deslocar, 1549.9);
                    orientation1.Xx = 0.994139177;
                    orientation1.Xy = 0.0;
                    orientation1.Xz = -0.108107800;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = 1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.108107800;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 0.994139177;

             
                
                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;
               // MessageBox.Show(arquivo);
                try
                {
                    component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + arquivo, "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);

                    partLoadStatus2.Dispose();

                    NXObject[] objects1 = new NXObject[0];
                    int nErrs3;
                    nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                }
                catch
                {


                }

                comp_ant = comp;

            }
            bool flg = true;
            if (flg == true)
            {
                comp = Convert.ToString(Convert.ToDouble(_ID_txt_foco_1_le.Text) - 42 - 87);
                string cod_fam = "288383";

                double var_Comp = Convert.ToDouble(comp);
                string[] expressao = { "COMP" };
                double[] valor = { var_Comp };
                double[] tolerancia = { tol_entre_foco };

                arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                try
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + arquivo);

                    BasePart basePart1;
                    PartLoadStatus partLoadStatus1;
                    basePart1 = theSession.Parts.OpenBase("@DB/" + arquivo, out partLoadStatus1);

                    partLoadStatus1.Dispose();
                    int nErrs1;
                }
                catch (Exception)
                {


                }

                Point3d basePoint1;
                Matrix3x3 orientation1;

                double pos_perfil_1 = Convert.ToDouble(_txt_dist_inicio_le.Text) + 87;
                basePoint1 = new Point3d(816.1, pos_perfil_1, 1549.9);
                orientation1.Xx = 0.994139177;
                orientation1.Xy = 0.0;
                orientation1.Xz = -0.108107800;
                orientation1.Yx = 0.0;
                orientation1.Yy = 1.0;
                orientation1.Yz = 0.0;
                orientation1.Zx = 0.108107800;
                orientation1.Zy = 0.0;
                orientation1.Zz = 0.994139177;



                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;
                // MessageBox.Show(arquivo);
                try
                {
                    component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + arquivo, "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);

                    partLoadStatus2.Dispose();

                    NXObject[] objects1 = new NXObject[0];
                    int nErrs3;
                    nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                }
                catch
                {


                }
            }
            if (flg == true)
            {
                int comp_ult = (Convert.ToInt32(_txt_ct.Text) - 400) - (Convert.ToInt32(pos_entre_foco_le) + 87 + 42);
               
                string cod_fam = "288383";

                double var_Comp = Convert.ToDouble(comp_ult);
                string[] expressao = { "COMP" };
                double[] valor = { var_Comp };
                double[] tolerancia = { tol_entre_foco };

                arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                try
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + arquivo);

                    BasePart basePart1;
                    PartLoadStatus partLoadStatus1;
                    basePart1 = theSession.Parts.OpenBase("@DB/" + arquivo, out partLoadStatus1);

                    partLoadStatus1.Dispose();
                    int nErrs1;
                }
                catch (Exception)
                {


                }

                Point3d basePoint1;
                Matrix3x3 orientation1;

                double pos_perfil_2 = pos_entre_foco_le + 42;
                basePoint1 = new Point3d(816.1, pos_perfil_2, 1549.9);
                orientation1.Xx = 0.994139177;
                orientation1.Xy = 0.0;
                orientation1.Xz = -0.108107800;
                orientation1.Yx = 0.0;
                orientation1.Yy = 1.0;
                orientation1.Yz = 0.0;
                orientation1.Zx = 0.108107800;
                orientation1.Zy = 0.0;
                orientation1.Zz = 0.994139177;

                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;
                // MessageBox.Show(arquivo);
                try
                {
                    component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + arquivo, "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);

                    partLoadStatus2.Dispose();

                    NXObject[] objects1 = new NXObject[0];
                    int nErrs3;
                    nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                }
                catch
                {


                }
            }

            NXOpen.Assemblies.Component Volta_Assem_Comp = null;
            PartLoadStatus partLoadStatus_Volta;
            theSession.Parts.SetWorkComponent(Volta_Assem_Comp, out partLoadStatus_Volta);
            wp_pai = theSession.Parts.Work;
            partLoadStatus_Volta.Dispose();



        }
        public void add_entre_foco_ld_WAVE()
        {
            Session theSession = Session.GetSession();
            Part wp_pai = theSession.Parts.Work;
            Part wp_pp = theSession.Parts.Work;

            NXOpen.Assemblies.Component pp_le;

            pp_le = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_CJ_PORTA_PAC_LD_WAVE/000 1");

            PartLoadStatus partLoadStatus2;

            theSession.Parts.SetWorkComponent(pp_le, out partLoadStatus2);


            wp_pp = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();

            int qtd_foco_ld = Convert.ToInt32(_txt_qtd_foco_ld.Text);
            double pos_entre_foco_ld = Convert.ToDouble(_txt_dist_inicio_ld.Text) + Convert.ToDouble(_ID_txt_foco_1_ld.Text.Replace('.', ','));

            string arquivo = "";
            string comp_ant = "";
            string comp = "";
            for (int i = 1; i <= qtd_foco_ld; i++)
            {

                string name_passo = "_ID_passo_ld_" + Convert.ToString(i + 1);
                foreach (Control Tab in tableLayoutPanel1.Controls)
                {
                    //if (Tab is TabControl)
                    //{
                    foreach (Control Tabpage in Tab.Controls)
                    {
                        if (Tabpage.Name == "tabPage2")
                        {

                            foreach (Control txt_inf in Tabpage.Controls)
                            {
                                if (txt_inf is TextBox && txt_inf.Name == name_passo)
                                {
                                    try
                                    {

                                        comp = Convert.ToString(Convert.ToDouble(txt_inf.Text) - 84);
                                        pos_entre_foco_ld += (Convert.ToDouble(txt_inf.Text));
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
                if (comp != comp_ant)
                {
                    arquivo = "";
                }
                if (comp != comp_ant)
                {
                    string cod_fam = "288383";

                    double var_Comp = Convert.ToDouble(comp);
                    string[] expressao = { "COMP" };
                    double[] valor = { var_Comp };
                    double[] tolerancia = { tol_entre_foco };

                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }


                try
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + arquivo);

                    BasePart basePart1;
                    PartLoadStatus partLoadStatus1;
                    basePart1 = theSession.Parts.OpenBase("@DB/" + arquivo, out partLoadStatus1);

                    partLoadStatus1.Dispose();
                    int nErrs1;
                }
                catch (Exception)
                {


                }

                Point3d basePoint1;
                Matrix3x3 orientation1;

                double comp_deslocar = Convert.ToDouble(comp) + 42;
                if(_cmb_polt_ind.Text == "SIM" && i==1)
                {
                    comp_deslocar = Convert.ToDouble(comp) + 57-31;
                }
                basePoint1 = new Point3d(-816.1, (pos_entre_foco_ld) - comp_deslocar, 1549.9);
                orientation1.Xx = 0.994139177;
                orientation1.Xy = 0.0;
                orientation1.Xz = 0.108107800;
                orientation1.Yx = 0.0;
                orientation1.Yy = 1.0;
                orientation1.Yz = 0.0;
                orientation1.Zx = 0.108107800;
                orientation1.Zy = 0.0;
                orientation1.Zz = 0.994139177;

                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;
                // MessageBox.Show(arquivo);
                try
                {
                    component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + arquivo, "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);

                    partLoadStatus2.Dispose();

                    NXObject[] objects1 = new NXObject[0];
                    int nErrs3;
                    nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                }
                catch
                {


                }

                comp_ant = comp;

            }
            bool flg = true;
            if (flg == true && _cmb_polt_ind.Text != "SIM")
            {
                comp = Convert.ToString(Convert.ToDouble(_ID_txt_foco_1_ld.Text) - 42 - 87);
                string cod_fam = "288383";

                double var_Comp = Convert.ToDouble(comp);
                string[] expressao = { "COMP" };
                double[] valor = { var_Comp };
                double[] tolerancia = { tol_entre_foco};

                arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                try
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + arquivo);

                    BasePart basePart1;
                    PartLoadStatus partLoadStatus1;
                    basePart1 = theSession.Parts.OpenBase("@DB/" + arquivo, out partLoadStatus1);

                    partLoadStatus1.Dispose();
                    int nErrs1;
                }
                catch (Exception)
                {


                }

                Point3d basePoint1;
                Matrix3x3 orientation1;

                double pos_perfil_1 = Convert.ToDouble(_txt_dist_inicio_ld.Text) + 87;
                basePoint1 = new Point3d(-816.1, pos_perfil_1, 1549.9);
                orientation1.Xx = 0.994139177;
                orientation1.Xy = 0.0;
                orientation1.Xz = 0.108107800;
                orientation1.Yx = 0.0;
                orientation1.Yy = 1.0;
                orientation1.Yz = 0.0;
                orientation1.Zx = 0.108107800;
                orientation1.Zy = 0.0;
                orientation1.Zz = 0.994139177;



                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;
                // MessageBox.Show(arquivo);
                try
                {
                    component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + arquivo, "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);

                    partLoadStatus2.Dispose();

                    NXObject[] objects1 = new NXObject[0];
                    int nErrs3;
                    nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                }
                catch
                {


                }
            }
            if (flg == true)
            {
                int comp_ult = (Convert.ToInt32(_txt_ct.Text) - 400) - (Convert.ToInt32(pos_entre_foco_ld) + 87 + 42);

                string cod_fam = "288383";

                double var_Comp = Convert.ToDouble(comp_ult);
                string[] expressao = { "COMP" };
                double[] valor = { var_Comp };
                double[] tolerancia = {tol_entre_foco };

                arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                try
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + arquivo);

                    BasePart basePart1;
                    PartLoadStatus partLoadStatus1;
                    basePart1 = theSession.Parts.OpenBase("@DB/" + arquivo, out partLoadStatus1);

                    partLoadStatus1.Dispose();
                    int nErrs1;
                }
                catch (Exception)
                {


                }

                Point3d basePoint1;
                Matrix3x3 orientation1;

                double pos_perfil_2 = pos_entre_foco_ld + 42;
                basePoint1 = new Point3d(-816.1, pos_perfil_2, 1549.9);
                orientation1.Xx = 0.994139177;
                orientation1.Xy = 0.0;
                orientation1.Xz = 0.108107800;
                orientation1.Yx = 0.0;
                orientation1.Yy = 1.0;
                orientation1.Yz = 0.0;
                orientation1.Zx = 0.108107800;
                orientation1.Zy = 0.0;
                orientation1.Zz = 0.994139177;

                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;
                // MessageBox.Show(arquivo);
                try
                {
                    component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + arquivo, "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);

                    partLoadStatus2.Dispose();

                    NXObject[] objects1 = new NXObject[0];
                    int nErrs3;
                    nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                }
                catch
                {


                }
            }

            NXOpen.Assemblies.Component Volta_Assem_Comp = null;
            PartLoadStatus partLoadStatus_Volta;
            theSession.Parts.SetWorkComponent(Volta_Assem_Comp, out partLoadStatus_Volta);
            wp_pai = theSession.Parts.Work;
            partLoadStatus_Volta.Dispose();



        }
        public void verificar_itens_le()
        {
            itens_nov_entre_foco.Clear();
            itens_nov_243121.Clear();
            itens_nov_243175.Clear();
            Session theSession = Session.GetSession();
            Part wp_pai = theSession.Parts.Work;
            Part wp_pp = theSession.Parts.Work;

             NXOpen.Assemblies.Component pp_le;
             if (cmb_carroceria.Text == "S4")
            {
                 pp_le = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_MICRO_S4/000 1");
            }
            else
            {
                pp_le = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_LE_ROMA/000 1");
            }

            PartLoadStatus partLoadStatus2;

            theSession.Parts.SetWorkComponent(pp_le, out partLoadStatus2);


            wp_pp = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();

            int qtd_foco_le = Convert.ToInt32(txt_qtd_foco_le.Text);
            double pos_entre_foco_le = Convert.ToDouble(txt_dist_inicio_le.Text) + Convert.ToDouble(ID_txt_foco_1_le.Text);

            string arquivo = "";
            string comp_ant = "";
            string comp = "";
            string lista_ld = "";
            for (int i = 1; i <= qtd_foco_le; i++)
            {
                Registrar_no_LOG(" DEBUG -> (entre_foco_le): " + comp);
                string name_passo = "ID_passo_le_" + Convert.ToString(i + 1);
                foreach (Control control in tableLayoutPanel1.Controls)
                {
                    if (control is TabControl)
                    {
                        foreach (TabPage page in control.Controls)
                        {
                            if (page is TabPage && page.Name == "tabPage1")
                            {
                                foreach (Control txt_box in page.Controls)
                                {
                                    if (txt_box is TextBox && txt_box.Name == name_passo)
                                    {
                                        comp = Convert.ToString(Convert.ToDouble(txt_box.Text) - 162);
                                        pos_entre_foco_le += (Convert.ToDouble(txt_box.Text));
                                    }
                                }
                            }
                        }
                    }
                }
                if (comp != comp_ant)
                {
                    arquivo = "";
                }
                if (comp != comp_ant)
                {
                    lista_ld += comp + "\n";
                    string cod_fam = "214642";

                    double var_Comp = Convert.ToDouble(comp);
                    string[] expressao = { "COMP" };
                    double[] valor = { var_Comp };
                    double[] tolerancia = { tol_entre_foco };

                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                    if (arquivo == "")
                    {
                        itens_nov_entre_foco.Add(cod_fam + ";" + Convert.ToDouble(comp));
                        Registrar_no_LOG(" DEBUG -> (Criar novo item): Familia " + cod_fam + " valor " + Convert.ToDouble(comp));
                    }
                    else
                    {
                        Registrar_no_LOG(" DEBUG -> (item_existente): arquivo " + arquivo + " valor " + Convert.ToDouble(comp));
                    }
                }

                comp_ant = comp;
            }
           // MessageBox.Show(lista_ld);
            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();

            NXOpen.Assemblies.Component cmp = wp_pp.ComponentAssembly.RootComponent;

            Componentes.Add(cmp);


            string codigo_item = "";
            string total_itens = "";
            Componentes.ToArray()[0].GetChildren();


            for (int i = 0; i < 1; i++)
            {
                NXOpen.Assemblies.Component[] cmps = Componentes.ToArray()[i].GetChildren();
                foreach (NXOpen.Assemblies.Component cmp_1 in cmps)
                {
                    string name = Convert.ToString(cmp_1.Name);
                    string cod = Convert.ToString(cmp_1.DisplayName);
                    int index_item = 0;
                    codigo_item = cod;
                    foreach (NXOpen.Assemblies.Component qtd_cmp_1 in cmps)
                    {
                        string cod_pesq = Convert.ToString(qtd_cmp_1.DisplayName);
                        if (cod_pesq == cod)
                        {
                            index_item += 1;
                            total_itens = Convert.ToString(index_item);
                        }
                    }


                        NXObject[] objects1 = new NXObject[1];

                    Registrar_no_LOG(" DEBUG -> COMPONENT " + cod + " " + total_itens);
                    NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)(wp_pp.ComponentAssembly.RootComponent.FindObject("COMPONENT " + cod + " " + total_itens)));
                        objects1[0] = component1;
                        AttributePropertiesBuilder attributePropertiesBuilder1;
                        //MessageBox.Show(cod);

                        if (cod.Contains("243121") && !component1.IsSuppressed)// == "TPL_243121_B/000")
                        {
                            

                            NXObject.AttributeInformation[] attr = component1.GetUserAttributes();

                            double COMP = 0;
                            double ABERT_MAD = 0;
                            double POS_AC = 0;
                            double POS_FORCADO = 0;

                            foreach (var item in attr)
                            {

                                if (item.Title =="COMP")
                                {
                                    COMP = Convert.ToDouble(item.RealValue);

                                }
                                if (item.Title =="ABERT_MAD")
                                {
                                    ABERT_MAD = Convert.ToDouble(item.RealValue);
                                }
                        
                                if (item.Title =="POS_AC")
                                {
                                    POS_AC = Convert.ToDouble(item.RealValue); 

                                }
                                if (item.Title=="POS_FORCADO")
                                {
                                    POS_FORCADO = Convert.ToDouble(item.RealValue);

                                }
                            }

                            arquivo = "";
                            string cod_fam = "243121";


                            string[] expressao = { "COMP", "LADO", "POS_AC", "ABERT_MAD", "POS_FORCADO" };
                            double[] valor = { COMP, 1, POS_AC, ABERT_MAD, POS_FORCADO };
                            double[] tolerancia = { 0.1, 0.0, 0.1, 0.1, 0.1};

                            arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor,0, tolerancia, "", "");
                      
                        //MessageBox.Show(arquivo);
                        if (arquivo == "")
                            {
                                itens_nov_243121.Add("243121;1;" + COMP + ";" + POS_AC + ";" + ABERT_MAD + ";" + POS_FORCADO + ";" + cod + ";LE");
                                Registrar_no_LOG(" DEBUG -> (Criar novo item): Familia " + "243121;1;" + COMP + ";" + POS_AC + ";" + ABERT_MAD + ";" + POS_FORCADO + ";" + cod + ";LE");
                            }
                            else
                            {
                                itens_subst.Add(cod + ";" + arquivo+";LE");
                                Registrar_no_LOG(" DEBUG -> (item encontrado): " + cod + ";" + arquivo + ";LE");
                            }
                        }
                        if (cod.Contains("243175")&&!component1.IsSuppressed)
                        {


                            NXObject.AttributeInformation[] attr = component1.GetUserAttributes();

                            double COMP = 0;
                         
                            foreach (var item in attr)
                            {

                                if (item.Title == "COMP")
                                {
                                    COMP = Convert.ToDouble(item.RealValue);

                                }
                                
                            }
                            arquivo = "";
                            string cod_fam = "243175";

                            double var_Comp = Convert.ToDouble(comp);
                            string[] expressao = { "COMP" };
                            double[] valor = { COMP };
                            double[] tolerancia = { 0.1};

                            arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");
                       

                        if (arquivo == "")
                            {
                                itens_nov_243175.Add("243175;" + COMP + ";LE;"+cod);
                                Registrar_no_LOG(" DEBUG -> (Criar novo item): Familia " + "243175;" + COMP + ";LE;" + cod);
                            }
                            else
                            {
                                itens_subst.Add(cod + ";" + arquivo+";LE");
                                Registrar_no_LOG(" DEBUG -> (item encontrado): " + cod + ";" + arquivo + ";LE");
                            }
                        }
                }
            }
            NXOpen.Assemblies.Component Volta_Assem_Comp = null;
            PartLoadStatus partLoadStatus_Volta;
            theSession.Parts.SetWorkComponent(Volta_Assem_Comp, out partLoadStatus_Volta);
            wp_pai = theSession.Parts.Work;
            partLoadStatus_Volta.Dispose();

        }
        public void verificar_itens_le_WAVE()
        {
            itens_nov_entre_foco.Clear();

            itens_nov_madeira.Clear();
            //-------ENTRE  FOCO LE
            int qtd_foco_le = Convert.ToInt32(_txt_qtd_foco_le.Text);
            double pos_entre_foco_le = Convert.ToDouble(_txt_dist_inicio_le.Text) + Convert.ToDouble(_ID_txt_foco_1_le.Text);

            string arquivo = "";
            string comp_ant = "";
            string comp = "";
          
            int qtd_prf_entre_foco = qtd_foco_le+2;

            string cod_fam = "288383";
            
            comp =  Convert.ToString(Convert.ToDouble(_ID_txt_foco_1_le.Text)-42-87);
            double var_Comp = Convert.ToDouble(comp);
            string[] expressao = { "COMP" };
            double[] valor = { var_Comp };
            double[] tolerancia = { tol_entre_foco };

            arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

            if (arquivo == "")
            {
                itens_nov_entre_foco.Add(cod_fam + ";" + Convert.ToDouble(comp));
                Registrar_no_LOG(" DEBUG -> (Criar novo item): Familia " + cod_fam + " valor " + Convert.ToDouble(comp));
            }
             comp_ant = comp;
             arquivo = "";
             comp = "";
            for (int i = 1; i <= qtd_foco_le-1; i++)
            {
                string name_passo = "_ID_passo_le_" + Convert.ToString(i + 1);
                foreach (Control Tab in tableLayoutPanel1.Controls)
                {
                    //if (Tab is TabControl)
                    //{
                    foreach (Control Tabpage in Tab.Controls)
                    {
                        if (Tabpage.Name == "tabPage2")
                        {

                            foreach (Control txt_inf in Tabpage.Controls)
                            {
                                if (txt_inf is TextBox && txt_inf.Name == name_passo)
                                {
                                    try
                                    {

                                        comp = Convert.ToString(Convert.ToDouble(txt_inf.Text) - 84);
                                        pos_entre_foco_le += (Convert.ToDouble(txt_inf.Text));
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
              
                if (comp != comp_ant)
                {
                    arquivo = "";
                }
                if (comp != comp_ant)
                {
                   
                    string cod_fam_1 = "288383";
                    
                    double var_Comp_1 = Convert.ToDouble(comp);
                    string[] expressao_1 = { "COMP" };
                    double[] valor_1 = { var_Comp_1 };
                    double[] tolerancia_1 = { tol_entre_foco };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam_1, "", expressao_1, valor_1, 0, tolerancia_1, "", "");
                    if (arquivo == "")
                    {

                        itens_nov_entre_foco.Add(cod_fam + ";" + comp);
                        Registrar_no_LOG(" DEBUG -> (Criar novo item): Familia " + cod_fam + " valor " + Convert.ToDouble(comp));
                    }
                }
                comp_ant = comp;
            }
            int comp_ult = (Convert.ToInt32(_txt_ct.Text) - 400) - (Convert.ToInt32(pos_entre_foco_le) + 87 + 42);
            if (comp_ult.ToString() != comp_ant)
            {
                arquivo = "";
                string cod_fam_1 = "288383";

                double var_Comp_1 = Convert.ToDouble(comp_ult);
                string[] expressao_1 = { "COMP" };
                double[] valor_1 = { var_Comp_1 };
                double[] tolerancia_1 = { tol_entre_foco };
                arquivo = Custom_Mascarello.Search.CreateMember(cod_fam_1, "", expressao_1, valor_1, 0, tolerancia_1, "", "");
                if (arquivo == "")
                {
                    itens_nov_entre_foco.Add(cod_fam + ";" + comp_ult);
                    Registrar_no_LOG(" DEBUG -> (Criar novo item): Familia " + cod_fam + " valor " + Convert.ToDouble(comp_ult));
                }
            }
            //-------ENTRE  FOCO LD

            int qtd_foco_ld = Convert.ToInt32(_txt_qtd_foco_ld.Text);
            double pos_entre_foco_ld = Convert.ToDouble(_txt_dist_inicio_ld.Text) + Convert.ToDouble(_ID_txt_foco_1_ld.Text.Replace('.',','));
            if(_cmb_polt_ind.Text !="SIM")
            { 
            comp = Convert.ToString(Convert.ToDouble(_ID_txt_foco_1_ld.Text) - 42 - 87);
            double var_Comp_4 = Convert.ToDouble(comp);
            string[] expressao_4 = { "COMP" };
            double[] valor_4 = { var_Comp_4 };
            double[] tolerancia_4 = { 1.0 };

            arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao_4, valor_4, 0, tolerancia_4, "", "");

            if (arquivo == "")
            {
                itens_nov_entre_foco.Add(cod_fam + ";" + Convert.ToDouble(comp));
                Registrar_no_LOG(" DEBUG -> (Criar novo item): Familia " + cod_fam + " valor " + Convert.ToDouble(comp));
            }
            }
            comp_ant = comp;
            arquivo = "";
            comp = "";
            for (int i = 1; i <= qtd_foco_ld - 1; i++)
            {
                string name_passo = "_ID_passo_ld_" + Convert.ToString(i + 1);
                foreach (Control Tab in tableLayoutPanel1.Controls)
                {
                    //if (Tab is TabControl)
                    //{
                    foreach (Control Tabpage in Tab.Controls)
                    {
                        if (Tabpage.Name == "tabPage2")
                        {

                            foreach (Control txt_inf in Tabpage.Controls)
                            {
                                if (txt_inf is TextBox && txt_inf.Name == name_passo)
                                {
                                    try
                                    {
                                        if(i==1 && _cmb_polt_ind.Text == "SIM")
                                        {
                                          
                                            comp = Convert.ToString(Convert.ToDouble(txt_inf.Text) - 84-15);
                                           
                                            pos_entre_foco_ld += (Convert.ToDouble(txt_inf.Text));
                                        }
                                        else
                                        {
                                            comp = Convert.ToString(Convert.ToDouble(txt_inf.Text) - 84);
                                            pos_entre_foco_ld += (Convert.ToDouble(txt_inf.Text));
                                        }
                                        
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }

                if (comp != comp_ant)
                {
                    arquivo = "";
                }
              
                if (comp != comp_ant)
                {

                    string cod_fam_1 = "288383";

                    double var_Comp_1 = Convert.ToDouble(comp);
                    string[] expressao_1 = { "COMP" };
                    double[] valor_1 = { var_Comp_1 };
                    double[] tolerancia_1 = { tol_entre_foco };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam_1, "", expressao_1, valor_1, 0, tolerancia_1, "", "");
                    if (arquivo == "")
                    {

                        itens_nov_entre_foco.Add(cod_fam + ";" + comp);
                        Registrar_no_LOG(" DEBUG -> (Criar novo item): Familia " + cod_fam + " valor " + Convert.ToDouble(comp));
                    }
                }
                comp_ant = comp;
            }

            int folga_final = 400;
            if (_txt_wc.Text != "5.1.1 - Não")
            {
                folga_final = 1400;
            }
           // MessageBox.Show("POS EE " + pos_entre_foco_ld.ToString());
            int comp_ult_ld = (Convert.ToInt32(_txt_ct.Text) - folga_final) - (Convert.ToInt32(pos_entre_foco_ld) + 87 + 42);
           // MessageBox.Show(comp_ult_ld.ToString());
            if (comp_ult_ld.ToString() != comp_ant)
            {
                arquivo = "";
                string cod_fam_1 = "288383";

                double var_Comp_1 = Convert.ToDouble(comp_ult_ld);
                string[] expressao_1 = { "COMP" };
                double[] valor_1 = { var_Comp_1 };
                double[] tolerancia_1 = { tol_entre_foco };
                arquivo = Custom_Mascarello.Search.CreateMember(cod_fam_1, "", expressao_1, valor_1, 0, tolerancia_1, "", "");
                if (arquivo == "")
                {
                    itens_nov_entre_foco.Add(cod_fam + ";" + comp_ult_ld);
                    Registrar_no_LOG(" DEBUG -> (Criar novo item): Familia " + cod_fam + " valor " + Convert.ToDouble(comp_ult_ld));
                }
            }
            //for (int i = 0; i <= itens_nov_entre_foco.Count-1; i++)
            //{
            //    MessageBox.Show(itens_nov_entre_foco[i] + "LD");
            //}
            //----------------------------------ATE AQUI OK-------"
            cod_fam = "288324";

            int comp_mad = Convert.ToInt16(_txt_ct.Text) - Convert.ToInt16(_txt_dist_inicio_le.Text) - Convert.ToInt16(_txt_dist_B.Text)-174-80;
            int pos_ac = 0;
            if (_txt_pos_ac.Text != "0")
            {
                pos_ac = Convert.ToInt16(_txt_pos_ac.Text) - Convert.ToInt16(_txt_dist_inicio_le.Text) - 87;
            }
                int pos_ar_f = 0;
            if (_txt_pos_forc_ar_le.Text != "0")
            {
                 pos_ar_f = Convert.ToInt16(_txt_pos_forc_ar_le.Text) - 87;
            }
            var_Comp = Convert.ToDouble(comp_mad);
            string[] expressao_2 = { "COMP","LADO", "POS_AC", "ABERT_MAD", "POS_FORCADO"};
            double[] valor_2 = { var_Comp, 1, Convert.ToDouble(pos_ac), Convert.ToDouble(_txt_vao_mad.Text), Convert.ToDouble(pos_ar_f) };
            double[] tolerancia_2 = { 1.0, 1.0, 1.0, 1.0, 1.0 };

            arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao_2, valor_2, 0, tolerancia_2, "", "");

            if (arquivo == "")
            {
                itens_nov_madeira.Add(cod_fam + ";" + comp_mad.ToString()+";" + "1" + ";" + pos_ac.ToString() + ";" + _txt_vao_mad.Text+";"+ pos_ar_f.ToString());
                Registrar_no_LOG(" DEBUG -> (Criar novo item): Familia " + cod_fam + ";" + comp_mad.ToString() + ";" + "1" + ";" + pos_ac.ToString() + ";" + _txt_vao_mad.Text + ";" + pos_ar_f.ToString());
            }
            else
            {
                itens_subst.Add(cod_fam + ";" + arquivo + ";" + "LE");
            }
            //-------madeira LD

            comp_mad = Convert.ToInt16(_txt_ct.Text) - Convert.ToInt16(_txt_dist_inicio_ld.Text) - Convert.ToInt16(_txt_dist_C.Text) - 174;
             pos_ac = 0;
            if (_txt_pos_ac.Text != "0")
            {
                pos_ac = Convert.ToInt16(_txt_pos_ac.Text) - Convert.ToInt16(_txt_dist_inicio_ld.Text) - 87;
            }
            pos_ar_f = 0;
             if (_txt_pos_forc_ar_ld.Text != "0")
            {
                pos_ar_f = Convert.ToInt16(_txt_pos_forc_ar_ld.Text) - 87;
            }
             
             var_Comp = Convert.ToDouble(comp_mad);
            string[] expressao_3 = { "COMP", "LADO", "POS_AC", "ABERT_MAD", "POS_FORCADO" };
            double[] valor_3 = { var_Comp, 0, Convert.ToDouble(pos_ac), Convert.ToDouble(_txt_vao_mad.Text), Convert.ToDouble(pos_ar_f) };
            double[] tolerancia_3 = { 1.0, 1.0, 1.0, 1.0, 1.0 };

            arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao_3, valor_3, 0, tolerancia_3, "", "");

            if (arquivo == "")
            {
                itens_nov_madeira.Add(cod_fam + ";" + comp_mad.ToString() + ";" + "1" + ";" + pos_ac.ToString() + ";" + _txt_vao_mad.Text + ";" + pos_ar_f.ToString());
                Registrar_no_LOG(" DEBUG -> (Criar novo item): Familia " + cod_fam + ";" + comp_mad.ToString() + ";" + "1" + ";" + pos_ac.ToString() + ";" + _txt_vao_mad.Text + ";" + pos_ar_f.ToString());
            }
            else
            {
                itens_subst.Add(cod_fam + ";" + arquivo + ";" + "LD");
            }
        }
        public void verificar_itens_ld()
        {
            
            Session theSession = Session.GetSession();
            Part wp_pai = theSession.Parts.Work;
            Part wp_pp = theSession.Parts.Work;



            NXOpen.Assemblies.Component pp_ld;
            
            if (cmb_carroceria.Text == "S4")
            {
                pp_ld = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_MICRO_S4_LD/000 1");
            }
            else
            {
                pp_ld = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_LD_ROMA/000 1");
            }
          //  MessageBox.Show(pp_ld.DisplayName);
            PartLoadStatus partLoadStatus2;

            theSession.Parts.SetWorkComponent(pp_ld, out partLoadStatus2);


            wp_pp = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();

            int qtd_foco_ld = Convert.ToInt32(txt_qtd_foco_ld.Text);
            double pos_entre_foco_ld = Convert.ToDouble(txt_dist_inicio_ld.Text) + Convert.ToDouble(ID_txt_foco_1_ld.Text);

            string pos_ee_wc = "";
            string arquivo = "";
            string comp_ant = "";
            string comp = "";
            string lista_ld = "";

            for (int i = 1; i <= qtd_foco_ld - 1; i++)
            {
              //  string name_passo = "ID_passo_le_" + Convert.ToString(i + 1);
                string name_passo = "ID_passo_ld_" + Convert.ToString(i + 1);
                foreach (Control control in tableLayoutPanel1.Controls)
                {
                    if (control is TabControl)
                    {
                        foreach (TabPage page in control.Controls)
                        {
                            if (page is TabPage && page.Name == "tabPage1")
                            {
                                foreach (Control txt_box in page.Controls)
                                {
                                    if (txt_box is TextBox && txt_box.Name == name_passo)
                                    {
                                        comp = Convert.ToString(Convert.ToDouble(txt_box.Text) - 162);
                                        pos_entre_foco_ld += (Convert.ToDouble(txt_box.Text));
                                        pos_ee_wc = txt_box.Text;
                                    }
                                }
                            }
                        }
                    }
                }



               

                //foreach (Control control in tableLayoutPanel1.Controls)
                //{
                //    if (control.Name == name_passo)
                //    {
                       
                //    }
                //}
               // MessageBox.Show(comp);
                if (comp != comp_ant)
                {
                    arquivo = "";
                }
                if (comp != comp_ant)
                {
                    lista_ld += comp + "\n";
                    string cod_fam = "214642";

                    double var_Comp = Convert.ToDouble(comp);
                    
                    string[] expressao = { "COMP" };
                    double[] valor = { var_Comp };
                    double[] tolerancia = { tol_entre_foco, tol_entre_foco };

                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");
                    

                    if (arquivo == "")
                    {
                        if (! itens_nov_entre_foco.Contains(cod_fam + ";" + Convert.ToDouble(comp)))  
                        {
                            itens_nov_entre_foco.Add(cod_fam + ";"+Convert.ToDouble(comp));
                         }
                        else
                        {
                            Registrar_no_LOG(" DEBUG -> (item_existente): arquivo " + arquivo + " valor " + Convert.ToDouble(comp)+"   LD");
                        }
                    }
                }

                if (i == qtd_foco_ld - 1 && cmb_wc.Text == "SIM")
                {
                    lista_ld = comp += " - wc" + "\n";
                    int comp_entre_foco_wc = (Convert.ToInt32(txt_CT.Text) - Convert.ToInt32(pos_entre_foco_ld)) - (81 + Convert.ToInt32(txt_dist_C.Text) + 40);
                    string cod_fam = "214642";


                    double var_Comp = Convert.ToDouble(comp_entre_foco_wc);
                    string[] expressao = { "COMP" };
                    double[] valor = { var_Comp };
                    double[] tolerancia = { tol_entre_foco, tol_entre_foco};

                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");
                    

                    if (arquivo == "")
                    {
                        if (!itens_nov_entre_foco.Contains(cod_fam + ";" + Convert.ToDouble(comp_entre_foco_wc)))
                        {
                            itens_nov_entre_foco.Add(cod_fam + ";" + Convert.ToDouble(comp_entre_foco_wc));
                        }
                        else
                        {
                            Registrar_no_LOG(" DEBUG -> (item_existente): arquivo " + arquivo + " valor " + Convert.ToDouble(comp) + "   LD");
                        }

                    }


                    
                }
                comp_ant = comp;
            }
       ///     MessageBox.Show(lista_ld);
            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();

            NXOpen.Assemblies.Component cmp = wp_pp.ComponentAssembly.RootComponent;

            Componentes.Add(cmp);


            string codigo_item = "";
            string total_itens = "";
            Componentes.ToArray()[0].GetChildren();


            for (int i = 0; i < 1; i++)
            {
                NXOpen.Assemblies.Component[] cmps = Componentes.ToArray()[i].GetChildren();
                foreach (NXOpen.Assemblies.Component cmp_1 in cmps)
                {
                    string name = Convert.ToString(cmp_1.Name);
                    string cod = Convert.ToString(cmp_1.DisplayName);
                    int index_item = 0;
                    codigo_item = cod;
                    foreach (NXOpen.Assemblies.Component qtd_cmp_1 in cmps)
                    {
                        string cod_pesq = Convert.ToString(qtd_cmp_1.DisplayName);
                        if (cod_pesq == cod)
                        {
                            index_item += 1;
                            total_itens = Convert.ToString(index_item);
                        }
                    }


                    NXObject[] objects1 = new NXObject[1];


                    NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)(wp_pp.ComponentAssembly.RootComponent.FindObject("COMPONENT " + cod + " " + total_itens)));
                    objects1[0] = component1;
                    AttributePropertiesBuilder attributePropertiesBuilder1;
                    //MessageBox.Show(cod);

                    if (cod.Contains("243121") && !component1.IsSuppressed)// == "TPL_243121_B/000")
                    {


                        NXObject.AttributeInformation[] attr = component1.GetUserAttributes();

                        double COMP = 0;
                        double ABERT_MAD = 0;
                        double POS_AC = 0;
                        double POS_FORCADO = 0;

                        foreach (var item in attr)
                        {

                            if (item.Title == "COMP")
                            {
                                COMP = Convert.ToDouble(item.RealValue);

                            }
                            if (item.Title == "ABERT_MAD")
                            {
                                ABERT_MAD = Convert.ToDouble(item.RealValue);
                            }

                            if (item.Title == "POS_AC")
                            {
                                POS_AC = Convert.ToDouble(item.RealValue);

                            }
                            if (item.Title == "POS_FORCADO")
                            {
                                POS_FORCADO = Convert.ToDouble(item.RealValue);

                            }
                        }
                        arquivo = "";
                        string cod_fam = "243121";

                       
                        string[] expressao = { "COMP", "LADO", "POS_AC", "ABERT_MAD", "POS_FORCADO"};
                        double[] valor = { COMP, 0, POS_AC, ABERT_MAD, POS_FORCADO };
                        double[] tolerancia = { 0.1, 0, 0.1,0.1,0.1};

                        arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");
                       

                        if (arquivo == "")
                        {
                            itens_nov_243121.Add("243121;0;" + COMP + ";" + POS_AC + ";" + ABERT_MAD + ";" + POS_FORCADO + ";" + cod + ";LD");
                        }
                        else
                        {
                            itens_subst.Add(cod + ";" + arquivo+";LD");
                        }
                        
                    }
                    if (cod.Contains("243175") && !component1.IsSuppressed)
                    {
                        NXObject.AttributeInformation[] attr = component1.GetUserAttributes();
                        double COMP = 0;

                        foreach (var item in attr)
                        {
                            if (item.Title == "COMP")
                            {
                                COMP = Convert.ToDouble(item.RealValue);

                            }
                        }
                        arquivo = "";
                        string cod_fam = "243175";

                        double var_Comp = Convert.ToDouble(comp);
                        string[] expressao = { "COMP"};
                        double[] valor = { COMP};
                        double[] tolerancia = { 0.1, 0.1};

                        arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");
                       

                        if (arquivo == "")
                        {
                            itens_nov_243175.Add("243175;" + COMP + ";LD;"+cod);
                        }
                        else
                        {
                            itens_subst.Add(cod + ";" + arquivo + ";LD");
                        }                      
                    }
                }
            }
            
            NXOpen.Assemblies.Component Volta_Assem_Comp = null;
            PartLoadStatus partLoadStatus_Volta;
            theSession.Parts.SetWorkComponent(Volta_Assem_Comp, out partLoadStatus_Volta);
            wp_pai = theSession.Parts.Work;
            partLoadStatus_Volta.Dispose();
        }
        public void add_entre_foco_ld()
        {
            Session theSession = Session.GetSession();
            Part wp_pai = theSession.Parts.Work;
            Part wp_pp = theSession.Parts.Work;



            NXOpen.Assemblies.Component pp_ld;
            if (cmb_carroceria.Text == "S4")
            {
                pp_ld = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_MICRO_S4_LD/000 1");
            }
            else
            {
                pp_ld = (NXOpen.Assemblies.Component)wp_pai.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_LD_ROMA/000 1");
            }

            PartLoadStatus partLoadStatus2;

            theSession.Parts.SetWorkComponent(pp_ld, out partLoadStatus2);


            wp_pp = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();

            int qtd_foco_ld = Convert.ToInt32(txt_qtd_foco_ld.Text);
            double pos_entre_foco_ld = Convert.ToDouble(txt_dist_inicio_ld.Text) + Convert.ToDouble(ID_txt_foco_1_ld.Text);

            string pos_ee_wc = "";
            string arquivo = "";
            string comp_ant = "";
            string comp = "";


            for (int i = 1; i <= qtd_foco_ld-1; i++)
            {
                string name_passo = "ID_passo_ld_" + Convert.ToString(i + 1);
                foreach (Control control in tableLayoutPanel1.Controls)
                {
                    if (control is TabControl)
                    {
                        foreach (TabPage page in control.Controls)
                        {
                            if (page is TabPage && page.Name == "tabPage1")
                            {
                                foreach (Control txt_box in page.Controls)
                                {
                                    if (txt_box is TextBox && txt_box.Name == name_passo)
                                    {
                                        comp = Convert.ToString(Convert.ToDouble(txt_box.Text) - 162);
                                        pos_entre_foco_ld += (Convert.ToDouble(txt_box.Text));
                                        pos_ee_wc = txt_box.Text;
                                    }
                                }
                            }
                        }
                    }
                }
                if (comp != comp_ant)
                {
                    arquivo = "";
                }
                if (comp != comp_ant)
                {
                    string cod_fam = "214642";

                    double var_Comp = Convert.ToDouble(comp);
                    string[] expressao = { "COMP" };
                    double[] valor = { var_Comp };
                    double[] tolerancia = { tol_entre_foco };

                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");
                   

                }
               
               // MessageBox.Show(arquivo);
                try
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + arquivo );

                    BasePart basePart1;
                    PartLoadStatus partLoadStatus1;
                    basePart1 = theSession.Parts.OpenBase("@DB/" + arquivo, out partLoadStatus1);

                    partLoadStatus1.Dispose();
                    int nErrs1;
                }
                catch (Exception)
                {


                }

                Point3d basePoint1;
                Matrix3x3 orientation1;

                if (cmb_carroceria.Text == "S4")
                {
                    basePoint1 = new Point3d(-915.387978439, pos_entre_foco_ld - (Convert.ToDouble(comp) + 81), 1500.767850120);
                    orientation1.Xx = 0.996460534;
                    orientation1.Xy = 0.0;
                    orientation1.Xz = 0.084061903;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = 1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = -0.084061903;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 0.996460534;
                }
                else
                {
                    ///MessageBox.Show(arquivo + " aaa " + (pos_entre_foco_ld - (Convert.ToDouble(comp) + 81)).ToString());
                    basePoint1 = new Point3d(-942.858884121, pos_entre_foco_ld - (Convert.ToDouble(comp) + 81), 1494.059200776);
                    orientation1.Xx = 0.994034448;
                    orientation1.Xy = 0.0;
                    orientation1.Xz = 0.109066569;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = 1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = -0.109066569;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 0.994034448;
                }
                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;

                try
                {
                    component1 = wp_pp.ComponentAssembly.AddComponent("@DB/" + arquivo, "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                    partLoadStatus2.Dispose();

                    NXObject[] objects1 = new NXObject[0];
                    int nErrs3;
                    nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                }
                catch
                {


                }

                if (i == qtd_foco_ld-1 && cmb_wc.Text == "SIM")
                {

                    int comp_entre_foco_wc = (Convert.ToInt32(txt_CT.Text) - Convert.ToInt32(pos_entre_foco_ld)) - (81 + Convert.ToInt32(txt_dist_C.Text) + 40);
                    string cod_fam = "214642";


                    double var_Comp = Convert.ToDouble(comp_entre_foco_wc);
                    string[] expressao = { "COMP" };
                    double[] valor = { var_Comp };
                    double[] tolerancia = { tol_entre_foco};

                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");
                    


                    try
                    {
                        NXOpen.Assemblies.Component component2;
                        basePoint1 = new Point3d(-942.858884121, (pos_entre_foco_ld + Convert.ToInt32(pos_ee_wc)) - (Convert.ToDouble(comp) + 81), 1494.059200776);
                        component2 = wp_pp.ComponentAssembly.AddComponent("@DB/" + arquivo, "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                        partLoadStatus2.Dispose();

                        NXObject[] objects1 = new NXObject[0];
                        int nErrs3;
                        nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                    }
                    catch
                    {


                    }
                }
            }

            comp_ant = comp;

            NXOpen.Assemblies.Component Volta_Assem_Comp = null;
            PartLoadStatus partLoadStatus_Volta;
            theSession.Parts.SetWorkComponent(Volta_Assem_Comp, out partLoadStatus_Volta);
            wp_pai = theSession.Parts.Work;
            partLoadStatus_Volta.Dispose();
        }
        public void substituir_tpl()
        {

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            string teste = "";

            for (int i = 0; i <= itens_subst.Count - 1; i++)
            {
                teste += itens_subst[i].ToString() + "\n";
            }
            //MessageBox.Show(teste);
            
            for (int i = 0; i <= itens_subst.Count - 1; i++)
            {
                
                string[] quebra = itens_subst[i].Split(';');

                string tpl_subst = quebra[0];
                string new_arqivo = quebra[1];
                string lado = quebra[2];
                
                NXOpen.Session.UndoMarkId markId1;
                markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

                NXOpen.Assemblies.ReplaceComponentBuilder replaceComponentBuilder1;
                replaceComponentBuilder1 = workPart.AssemblyManager.CreateReplaceComponentBuilder();

                replaceComponentBuilder1.ReplaceAllOccurrences = true;
             ///   MessageBox.Show(tpl_subst + "_____" + lado);
                replaceComponentBuilder1.ComponentNameType = NXOpen.Assemblies.ReplaceComponentBuilder.ComponentNameOption.AsSpecified;
                NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_"+lado+"_ROMA/000 1");

                
                NXOpen.Assemblies.Component component2 = (NXOpen.Assemblies.Component)component1.FindObject("COMPONENT " + tpl_subst + " 1");

              //  MessageBox.Show(tpl_subst + "   por " + new_arqivo +"  Lado"+ lado);
                bool added1;
                added1 = replaceComponentBuilder1.ComponentsToReplace.Add(component2);
                if (new_arqivo.Contains("/000"))
                {
                   new_arqivo = new_arqivo.Remove(new_arqivo.Length - 4, 4);
                }
             //   MessageBox.Show(tpl_subst + "   por " + new_arqivo + "  Lado" + lado);
                try
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + new_arqivo + "/000");
                }
                catch
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + new_arqivo);
                }

                try
                {
                    replaceComponentBuilder1.ReplacementPart = "@DB/" + new_arqivo + "/000";
                }
                catch
                {
                    replaceComponentBuilder1.ReplacementPart = "@DB/" + new_arqivo;
                }
                replaceComponentBuilder1.SetComponentReferenceSetType(NXOpen.Assemblies.ReplaceComponentBuilder.ComponentReferenceSet.Maintain, null);

                PartLoadStatus partLoadStatus1;
                partLoadStatus1 = replaceComponentBuilder1.RegisterReplacePartLoadStatus();
                
                Part part1 = null;

                

                try
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + new_arqivo + "/000");
                    part1 = (Part)theSession.Parts.FindObject("@DB/" + new_arqivo + "/000");
                }
                catch
                {
                    try
                    {

                    }
                    catch (Exception)
                    {
                        
                        throw;
                    }
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + new_arqivo + "/000");

                    BasePart basePart1;
                    PartLoadStatus partLoadStatus12;
                    basePart1 = theSession.Parts.OpenBase("@DB/" + new_arqivo + "/000", out partLoadStatus12);
                    partLoadStatus12.Dispose();
                    part1 = (Part)theSession.Parts.FindObject("@DB/" + new_arqivo + "/000");
                   
                }
                NXOpen.Preferences.LoadDraftingStandardBuilder loadDraftingStandardBuilder1;
                loadDraftingStandardBuilder1 = part1.Preferences.DraftingPreference.CreateLoadDraftingStandardBuilder();

                loadDraftingStandardBuilder1.WelcomeMode = false;

                loadDraftingStandardBuilder1.Level = NXOpen.Preferences.LoadDraftingStandardBuilder.LoadLevel.User;

                loadDraftingStandardBuilder1.Name = "ISO(MASCARELLO)";

                NXObject nXObject1;
                nXObject1 = loadDraftingStandardBuilder1.Commit();

                NXObject nXObject2;
                nXObject2 = replaceComponentBuilder1.Commit();

                partLoadStatus1.Dispose();

                replaceComponentBuilder1.Destroy();
            }
            
        }
        public void substituir_tpl_WAVE()
        {

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            string teste = "";

            for (int i = 0; i <= itens_subst.Count - 1; i++)
            {
                teste += itens_subst[i].ToString() + "\n";
            }
            //MessageBox.Show(teste);

            for (int i = 0; i <= itens_subst.Count - 1; i++)
            {

                string[] quebra = itens_subst[i].Split(';');

                string tpl_subst = quebra[0];
                string new_arqivo = quebra[1];
                string lado = quebra[2];

                NXOpen.Session.UndoMarkId markId1;
                markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

                NXOpen.Assemblies.ReplaceComponentBuilder replaceComponentBuilder1;
                replaceComponentBuilder1 = workPart.AssemblyManager.CreateReplaceComponentBuilder();

                replaceComponentBuilder1.ReplaceAllOccurrences = true;
               // MessageBox.Show(tpl_subst + "   por " + new_arqivo + "  Lado" + lado);
                replaceComponentBuilder1.ComponentNameType = NXOpen.Assemblies.ReplaceComponentBuilder.ComponentNameOption.AsSpecified;
                NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_CJ_PORTA_PAC_" + lado + "_WAVE/000 1");

                string letra = "A";
                string codigo_tpl = "TPL_288324_A";
                if (lado == "LD")
                {
                   codigo_tpl = "TPL_288324_B";
                }
               
                NXOpen.Assemblies.Component component2 = (NXOpen.Assemblies.Component)component1.FindObject("COMPONENT "+ codigo_tpl + "/000 1");

               
                bool added1;
                added1 = replaceComponentBuilder1.ComponentsToReplace.Add(component2);
                if (new_arqivo.Contains("/000"))
                {
                    new_arqivo = new_arqivo.Remove(new_arqivo.Length - 4, 4);
                }
                //   MessageBox.Show(tpl_subst + "   por " + new_arqivo + "  Lado" + lado);
                try
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + new_arqivo + "/000");
                }
                catch
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + new_arqivo);
                }

                try
                {
                    replaceComponentBuilder1.ReplacementPart = "@DB/" + new_arqivo + "/000";
                }
                catch
                {
                    replaceComponentBuilder1.ReplacementPart = "@DB/" + new_arqivo;
                }
                replaceComponentBuilder1.SetComponentReferenceSetType(NXOpen.Assemblies.ReplaceComponentBuilder.ComponentReferenceSet.Maintain, null);

                PartLoadStatus partLoadStatus1;
                partLoadStatus1 = replaceComponentBuilder1.RegisterReplacePartLoadStatus();

                Part part1 = null;



                try
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + new_arqivo + "/000");
                    part1 = (Part)theSession.Parts.FindObject("@DB/" + new_arqivo + "/000");
                }
                catch
                {
                    try
                    {

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + new_arqivo + "/000");

                    BasePart basePart1;
                    PartLoadStatus partLoadStatus12;
                    basePart1 = theSession.Parts.OpenBase("@DB/" + new_arqivo + "/000", out partLoadStatus12);
                    partLoadStatus12.Dispose();
                    part1 = (Part)theSession.Parts.FindObject("@DB/" + new_arqivo + "/000");

                }
                NXOpen.Preferences.LoadDraftingStandardBuilder loadDraftingStandardBuilder1;
                loadDraftingStandardBuilder1 = part1.Preferences.DraftingPreference.CreateLoadDraftingStandardBuilder();

                loadDraftingStandardBuilder1.WelcomeMode = false;

                loadDraftingStandardBuilder1.Level = NXOpen.Preferences.LoadDraftingStandardBuilder.LoadLevel.User;

                loadDraftingStandardBuilder1.Name = "ISO(MASCARELLO)";

                NXObject nXObject1;
                nXObject1 = loadDraftingStandardBuilder1.Commit();

                NXObject nXObject2;
                nXObject2 = replaceComponentBuilder1.Commit();

                partLoadStatus1.Dispose();

                replaceComponentBuilder1.Destroy();
            }

        }
        private void btn_aplicar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

            aplicar_configuracao();
            btn_codificar.Enabled = true;
           
            string paramentro = Custom_Mascarello.Update_external.Main("0");
    
            this.WindowState = FormWindowState.Normal;
            MessageBox.Show("Configuração aplicada com sucesso");
        }
        public static void Registrar_no_LOG(string Mensagem)
        {
            string gLog_FileName = @"c:\temp\" + DateTime.Now.ToString("PP"+"yyyyMMdd") + ".log";
            System.IO.StreamWriter vWriter = new System.IO.StreamWriter(gLog_FileName, true, System.Text.Encoding.Unicode);
            vWriter.WriteLine(DateTime.Now.ToString() + " " + Mensagem);
            vWriter.Close();
        }
        private void btn_alto_falantes_le_Click(object sender, EventArgs e)
        {
           // alto_falantes_le.Clear();
           // Session theSession = Session.GetSession();
           // Part workPart = theSession.Parts.Work;
           // Part displayPart = theSession.Parts.Display;
           // NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_MICRO_S4/000 1");
           // PartLoadStatus partLoadStatus1;
           // theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

           // List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();

           // workPart = theSession.Parts.Work;
           // partLoadStatus1.Dispose();

           // UI ui = NXOpen.UI.GetUI();

           // string message = "Selecione os porta focos com alto falantes lado esquerdo";
           // string title = "Selecão";

           // NXObject[] selectedObjects = null;
           // Selection.SelectionScope scope = Selection.SelectionScope.AnyInAssembly;
           // bool keepHighlighted = false;
           // bool includeFeatures = false;
           // Selection.Response response = default(Selection.Response);

           // Selection.SelectionAction selectionAction = Selection.SelectionAction.ClearAndEnableSpecific;

           // Selection.MaskTriple[] selectionMask_array = new Selection.MaskTriple[100];
           // {
           //     selectionMask_array[0].Type = UFConstants.UF_component_type;
           //     selectionMask_array[0].Subtype = UFConstants.UF_all_subtype;
                
           // }

           //response = ui.SelectionManager.SelectObjects(message, title, scope, selectionAction, includeFeatures, keepHighlighted, selectionMask_array, out selectedObjects);


           //for (int i = 0; i <= selectedObjects.Length-1; i++)
           //{
           //    int cont = i + 1;
           //    alto_falantes_le.Add(cont.ToString()+ ";"+ selectedObjects[i].ToString());
           //    //MessageBox.Show(selectedObjects[i].ToString());
           //}

           //for (int i = 0; i <= alto_falantes_le.Count-1; i++)
           //{
           //   // MessageBox.Show(alto_falantes_le[i]);
           //}



           //int ini, fim, ini1, fim1;

           //NXOpen.Assemblies.Component cmp1 = workPart.ComponentAssembly.RootComponent;

           //Componentes.Add(cmp1);


           //ini1 = 0;
           //fim1 = 1;

           //Componentes.ToArray()[0].GetChildren();


           //while (ini1 < fim1)
           //{
           //    ini = ini1;
           //    fim = fim1;
           //    ini1 = fim;
           //    fim1 = ini1;
           //    for (int i = ini; i < fim; i++)
           //    {

           //        NXOpen.Assemblies.Component[] cmps = Componentes.ToArray()[i].GetChildren();

           //        for (int j = 0; j <= alto_falantes_le.Count - 1; j++)
           //        {
           //            MessageBox.Show(alto_falantes_le[j]);
           //            foreach (NXOpen.Assemblies.Component cmp in cmps)
           //            {
           //                MessageBox.Show(cmp.ToString());
           //                string[] quebra = alto_falantes_le[j].Split(';');

           //                if (cmp.ToString() == quebra[1])
           //                {
           //                    MessageBox.Show(alto_falantes_le[j] + "  >>>>  " + cmp.ToString());
           //                }
           //            }
           //        }
               }

        private void btn_codificar_Click(object sender, EventArgs e)
        {
            codificar();
        }
        public void codificar()
        {
            this.WindowState = FormWindowState.Minimized;


            criar_itens();
            substituir_tpl();
            // MessageBox.Show("1111");
            add_foco_le();
            add_entre_foco_le();
            add_eme_le();

            //add_foco_ld();
            add_entre_foco_ld();
            add_eme_ld();
            //  btn_cod_conj.Enabled = true;

            this.WindowState = FormWindowState.Normal;
            MessageBox.Show("Itens codificados com sucesso!!!!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            verificar_itens_le();
            verificar_itens_ld();
            //double COMP = Convert.ToDouble(txt_comp.Text);
            //double LADO = Convert.ToDouble(txt_lad.Text);
            //double ABERT_MAD = Convert.ToDouble(txtABERT_MAD.Text);
            //double POS_AC = Convert.ToDouble(txtPOS_AC.Text);
            //double POS_FORCADO = Convert.ToDouble(txtPOS_FORCADO.Text);

           
            //string arquivo = "";
            //string cod_fam = "243121";


            //string[] expressao = { "COMP", "LADO", "POS_AC", "ABERT_MAD", "POS_FORCADO" };
            //double[] valor = { COMP, LADO, POS_AC, ABERT_MAD, POS_FORCADO };
            //double[] tolerancia = { 0.1, 0.0, 0.1,0.1,0.1};

            //arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");
            //MessageBox.Show(arquivo);
            //if (arquivo == "")
            //{
            //     MessageBox.Show("nao existe");
            // //   itens_nov_243121.Add("243121;1;" + COMP + ";" + POS_AC + ";" + ABERT_MAD + ";" + POS_FORCADO);
            //}
        }

        private void btn_cod_conj_Click(object sender, EventArgs e)
        {
          //  saveas_cj();
        }

        private void saveas_cj()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PAC_LE_ROMA/000 1");
            PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

            workPart = theSession.Parts.Work;
            partLoadStatus1.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            // ----------------------------------------------
            //   Menu: File->Save As...
            // ----------------------------------------------
            //Part part1;
            //part1 = theSession.Parts.Work;

            //NXOpen.Assemblies.ComponentAssembly componentAssembly1;
            //componentAssembly1 = workPart.ComponentAssembly;

            //NXOpen.Assemblies.Component component2;
            //component2 = componentAssembly1.RootComponent;

            //NXOpen.Session.UndoMarkId markId2;
            //markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            NXOpen.PDM.PartOperationBuilder partOperationBuilder1;
            partOperationBuilder1 = theSession.PdmSession.CreateOperationBuilder(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            partOperationBuilder1.ReplaceAllComponents = true;

            partOperationBuilder1.DependentFileSaveAsOption = NXOpen.PDM.PartOperationBuilder.DependentFileSaveAs.All;

            //partOperationBuilder1.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.Revise);

            //BasePart[] selectedparts1 = new BasePart[1];
            //selectedparts1[0] = workPart;
            //BasePart[] failedparts1;
            //partOperationBuilder1.SetSelectedParts(selectedparts1, out failedparts1);

            //NXOpen.PDM.LogicalObject[] logicalobjects1;
            //partOperationBuilder1.CreateLogicalObjects(out logicalobjects1);

            //NXObject[] sourceobjects1;
            //sourceobjects1 = logicalobjects1[0].GetUserAttributeSourceObjects();

            //NXObject[] objects1 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder1;
            //attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects2 = new NXObject[1];
            //objects2[0] = sourceobjects1[0];
            //attributePropertiesBuilder1.SetAttributeObjects(objects2);

            //attributePropertiesBuilder1.Title = "DB_PART_REV";

            //attributePropertiesBuilder1.Category = "M4_it_mascarelloRevision";

            //attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder1.StringValue = "";

            //NXObject[] objects3 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder2;
            //attributePropertiesBuilder2 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects3, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects4 = new NXObject[1];
            //objects4[0] = sourceobjects1[0];
            //attributePropertiesBuilder2.SetAttributeObjects(objects4);

            //attributePropertiesBuilder2.Title = "DB_PART_NO";

            //attributePropertiesBuilder2.Category = "M4_it_mascarello";

            //attributePropertiesBuilder2.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder2.StringValue = "teste_save_as_1";

            //NXObject[] objects5 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder3;
            //attributePropertiesBuilder3 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects5, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects6 = new NXObject[1];
            //objects6[0] = sourceobjects1[0];
            //attributePropertiesBuilder3.SetAttributeObjects(objects6);

            //attributePropertiesBuilder3.Title = "DB_PART_NAME";

            //attributePropertiesBuilder3.Category = "M4_it_mascarello";

            //attributePropertiesBuilder3.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder3.StringValue = "teste_save_as_1";

            //theSession.SetUndoMarkName(markId2, "Save Parts As Dialog");

            //partOperationBuilder1.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            //NXObject[] sourceobjects2;
            //sourceobjects2 = logicalobjects1[0].GetUserAttributeSourceObjects();

            //NXObject[] objects7 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder4;
            //attributePropertiesBuilder4 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects7, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects8 = new NXObject[1];
            //objects8[0] = sourceobjects1[0];
            //attributePropertiesBuilder4.SetAttributeObjects(objects8);

            //attributePropertiesBuilder4.Title = "DB_PART_REV";

            //attributePropertiesBuilder4.Category = "M4_it_mascarelloRevision";

            //attributePropertiesBuilder4.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder4.StringValue = "";

            //NXObject[] objects9 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder5;
            //attributePropertiesBuilder5 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects9, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects10 = new NXObject[1];
            //objects10[0] = sourceobjects1[0];
            //attributePropertiesBuilder5.SetAttributeObjects(objects10);

            //attributePropertiesBuilder5.Title = "DB_PART_NO";

            //attributePropertiesBuilder5.Category = "M4_it_mascarello";

            //attributePropertiesBuilder5.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder5.StringValue = "teste_save_as_1";

            //NXObject[] objects11 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder6;
            //attributePropertiesBuilder6 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects11, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects12 = new NXObject[1];
            //objects12[0] = sourceobjects1[0];
            //attributePropertiesBuilder6.SetAttributeObjects(objects12);

            //attributePropertiesBuilder6.Title = "DB_PART_NAME";

            //attributePropertiesBuilder6.Category = "M4_it_mascarello";

            //attributePropertiesBuilder6.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder6.StringValue = "teste_save_as_1";

            BasePart[] selectedparts2 = new BasePart[1];
            selectedparts2[0] = workPart;
            BasePart[] failedparts2;
            partOperationBuilder1.SetSelectedParts(selectedparts2, out failedparts2);

            NXOpen.PDM.LogicalObject[] logicalobjects2;
            partOperationBuilder1.CreateLogicalObjects(out logicalobjects2);

            NXObject[] sourceobjects3;
            sourceobjects3 = logicalobjects2[0].GetUserAttributeSourceObjects();

            NXObject[] objects13 = new NXObject[0];
            AttributePropertiesBuilder attributePropertiesBuilder7;
            attributePropertiesBuilder7 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects13, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects14 = new NXObject[1];
            //objects14[0] = sourceobjects3[0];
            //attributePropertiesBuilder7.SetAttributeObjects(objects14);

            attributePropertiesBuilder7.Title = "DB_PART_REV";

            attributePropertiesBuilder7.Category = "M4_it_mascarelloRevision";

            attributePropertiesBuilder7.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            attributePropertiesBuilder7.StringValue = "";

            NXObject[] objects15 = new NXObject[0];
            AttributePropertiesBuilder attributePropertiesBuilder8;
            attributePropertiesBuilder8 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects15, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            ////NXObject[] objects16 = new NXObject[1];
            ////objects16[0] = sourceobjects3[0];
            //attributePropertiesBuilder8.SetAttributeObjects(objects16);

            attributePropertiesBuilder8.Title = "DB_PART_NO";

            attributePropertiesBuilder8.Category = "M4_it_mascarello";

            attributePropertiesBuilder8.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            attributePropertiesBuilder8.StringValue = "";

            NXObject[] objects17 = new NXObject[0];
            AttributePropertiesBuilder attributePropertiesBuilder9;
            attributePropertiesBuilder9 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects17, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            NXObject[] objects18 = new NXObject[1];
            objects18[0] = sourceobjects3[0];
            attributePropertiesBuilder9.SetAttributeObjects(objects18);

            attributePropertiesBuilder9.Title = "DB_PART_NAME";

            attributePropertiesBuilder9.Category = "M4_it_mascarello";

            attributePropertiesBuilder9.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            attributePropertiesBuilder9.StringValue = "";

            //attributePropertiesBuilder2.Destroy();

            //attributePropertiesBuilder1.Destroy();

            //attributePropertiesBuilder3.Destroy();

            //attributePropertiesBuilder5.Destroy();

            //attributePropertiesBuilder4.Destroy();

            //attributePropertiesBuilder6.Destroy();

            // ----------------------------------------------
            //   Dialog Begin Save Parts As
            // ----------------------------------------------

            frm_codigo_datasul frm = new frm_codigo_datasul("dddddddd");

            frm.ShowDialog();

            string descricao = "dddddddd";
           string codigo = frm.Codigo;
           attributePropertiesBuilder8.StringValue = codigo;

            bool changed1;
            changed1 = attributePropertiesBuilder8.CreateAttribute();

         

            attributePropertiesBuilder7.StringValue = "000";

            attributePropertiesBuilder9.StringValue = codigo;

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Save Parts As");

            attributePropertiesBuilder9.StringValue = descricao;

            bool changed2;
            changed2 = attributePropertiesBuilder9.CreateAttribute();

            theSession.PdmSession.SetDefaultFolder("renato.ferreira:Newstuff");

           

            // Journaling of this operation is not yet implemented
            // The part save as operation in NX Manager mode
            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component nullAssemblies_Component = null;
            PartLoadStatus partLoadStatus2;
            theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus2);

            workPart = theSession.Parts.Work;
            partLoadStatus2.Dispose();
            theSession.SetUndoMarkName(markId5, "Make Work Part");
        }

        private void btn_aplicar_2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            aplicar_configuracao_2_EM_PP_WAVE();
            aplicar_configuracao_2_PP_WAVE();
            btn_codificar_2.Enabled = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void btn_codificar_2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            

            verificar_itens_le_WAVE();
            criar_itens_WAVE();

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Displayed Part");

            Part part1 = (Part)theSession.Parts.FindObject("@DB/TPL_PORTA_PAC_WAVE/000");
            PartLoadStatus partLoadStatus1;
            NXOpen.PartCollection.SdpsStatus status1;
            status1 = theSession.Parts.SetDisplay(part1, false, true, out partLoadStatus1);

            workPart = theSession.Parts.Work;
            displayPart = theSession.Parts.Display;
            partLoadStatus1.Dispose();

            substituir_tpl_WAVE();
            add_foco_le_WAVE();
            add_foco_ld_WAVE();
            add_entre_foco_le_WAVE();
            add_entre_foco_ld_WAVE();

            Part part2 = (Part)theSession.Parts.FindObject("@DB/TPL_EM_PORTA_PAC_WAVE/000");
            PartLoadStatus partLoadStatus2;
            NXOpen.PartCollection.SdpsStatus status2;
            status2 = theSession.Parts.SetDisplay(part2, false, true, out partLoadStatus2);

            workPart = theSession.Parts.Work;
            displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();

            this.WindowState = FormWindowState.Normal;
            MessageBox.Show("Itens criados com sucesso");
        }
        public void add_polt_dupla_ld()
        {
            Session theSession = Session.GetSession();
            Part wp_pai = theSession.Parts.Work;
            PartLoadStatus partLoadStatus2;


            int qtd_polt = Convert.ToInt32(_txt_qtd_foco_ld.Text);
            double pos_entre_foco_ld = Convert.ToDouble(_txt_dist_inicio_ld.Text) + Convert.ToDouble(_ID_txt_foco_1_ld.Text.Replace('.', ','));
            Point3d basePoint1;
            Matrix3x3 orientation1;

            string arquivo = "polt_dupla_ld_m2";
            if (_cmb_polt_ind.Text == "NÃO")
            {
                try
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + arquivo);

                    BasePart basePart1;
                    PartLoadStatus partLoadStatus1;
                    basePart1 = theSession.Parts.OpenBase("@DB/" + arquivo, out partLoadStatus1);

                    partLoadStatus1.Dispose();
                    int nErrs1;
                }
                catch (Exception)
                {


                }
            

            


            basePoint1 = new Point3d(-739, pos_entre_foco_ld, 0);
            orientation1.Xx = 1.0;
            orientation1.Xy = 0.0;
            orientation1.Xz = 0.0;
            orientation1.Yx = 0.0;
            orientation1.Yy = 1.0;
            orientation1.Yz = 0.0;
            orientation1.Zx = 0.0;
            orientation1.Zy = 0.0;
            orientation1.Zz = 1.0;

            wp_pai.ComponentAssembly.AddComponent("@DB/" + arquivo + "/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus2, true);
           }
            else
            {
                string arquivo_1 = "polt_ind_m2_pp";
                try
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + arquivo_1);

                    BasePart basePart1;
                    PartLoadStatus partLoadStatus1;
                    basePart1 = theSession.Parts.OpenBase("@DB/" + arquivo_1, out partLoadStatus1);

                    partLoadStatus1.Dispose();
                    int nErrs1;
                }
                catch (Exception)
                {


                }
                basePoint1 = new Point3d(-960, pos_entre_foco_ld, 0);
                orientation1.Xx = 1.0;
                orientation1.Xy = 0.0;
                orientation1.Xz = 0.0;
                orientation1.Yx = 0.0;
                orientation1.Yy = 1.0;
                orientation1.Yz = 0.0;
                orientation1.Zx = 0.0;
                orientation1.Zy = 0.0;
                orientation1.Zz = 1.0;

                wp_pai.ComponentAssembly.AddComponent("@DB/" + arquivo_1 + "/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus2, true);
            }

            for (int i = 1; i <= qtd_polt; i++)
            {

                string name_passo = "_ID_passo_ld_" + Convert.ToString(i + 1);
                foreach (Control Tab in tableLayoutPanel1.Controls)
                {
                    //if (Tab is TabControl)
                    //{
                    foreach (Control Tabpage in Tab.Controls)
                    {
                        if (Tabpage.Name == "tabPage2")
                        {

                            foreach (Control txt_inf in Tabpage.Controls)
                            {
                                if (txt_inf is TextBox && txt_inf.Name == name_passo)
                                {
                                    try
                                    {
                                        pos_entre_foco_ld += (Convert.ToDouble(txt_inf.Text));
                                        basePoint1 = new Point3d(-739, pos_entre_foco_ld, 0);

                                        wp_pai.ComponentAssembly.AddComponent("@DB/" + arquivo + "/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus2, true);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void add_polt_dupla_le()
        {
            Session theSession = Session.GetSession();
            Part wp_pai = theSession.Parts.Work;
            PartLoadStatus partLoadStatus2;


            int qtd_polt = Convert.ToInt32(_txt_qtd_foco_le.Text);
            double pos_entre_foco_le = Convert.ToDouble(_txt_dist_inicio_le.Text) + Convert.ToDouble(_ID_txt_foco_1_le.Text.Replace('.', ','));

            string arquivo = "polt_dupla_le_m2";
            try
            {
                theSession.Parts.SetNonmasterSeedPartData("@DB/" + arquivo);

                BasePart basePart1;
                PartLoadStatus partLoadStatus1;
                basePart1 = theSession.Parts.OpenBase("@DB/" + arquivo, out partLoadStatus1);

                partLoadStatus1.Dispose();
                int nErrs1;
            }
            catch (Exception)
            {


            }

            Point3d basePoint1;
            Matrix3x3 orientation1;


            basePoint1 = new Point3d(739, pos_entre_foco_le, 0);
            orientation1.Xx = 1.0;
            orientation1.Xy = 0.0;
            orientation1.Xz = 0.0;
            orientation1.Yx = 0.0;
            orientation1.Yy = 1.0;
            orientation1.Yz = 0.0;
            orientation1.Zx = 0.0;
            orientation1.Zy = 0.0;
            orientation1.Zz = 1.0;

            wp_pai.ComponentAssembly.AddComponent("@DB/" + arquivo + "/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus2, true);

            for (int i = 1; i <= qtd_polt; i++)
            {

                string name_passo = "_ID_passo_le_" + Convert.ToString(i + 1);
                foreach (Control Tab in tableLayoutPanel1.Controls)
                {
                    //if (Tab is TabControl)
                    //{
                    foreach (Control Tabpage in Tab.Controls)
                    {
                        if (Tabpage.Name == "tabPage2")
                        {

                            foreach (Control txt_inf in Tabpage.Controls)
                            {
                                if (txt_inf is TextBox && txt_inf.Name == name_passo)
                                {
                                    try
                                    {
                                        pos_entre_foco_le += (Convert.ToDouble(txt_inf.Text));
                                        basePoint1 = new Point3d(739, pos_entre_foco_le, 0);

                                        wp_pai.ComponentAssembly.AddComponent("@DB/" + arquivo + "/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus2, true);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        static void UpdateForExternalChange()
        {
            bool flg = false;
            int cont = 0; 
            while (flg == false)
            {
                try
                {
                string ruleName;
                theUFSession.Cfi.GetUniqueFilename(out ruleName);
                //  MessageBox.Show(ruleName);
                workPart.RuleManager.CreateDynamicRule("root:", ruleName,
                    "Any", "%ug_updateForExternalChange(false)", "");
                workPart.RuleManager.Evaluate(ruleName + ":");
                workPart.RuleManager.DeleteDynamicRule("root:", ruleName);
                    flg = true;
                }
                catch 
                {

                    
                }
                if(cont>10)
                {
                    flg = true;
                }
                cont++;
            }
        }
        List<DataComNX> listaRecebida_teste;
        private void button1_Click_2(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Open Component");
            NXOpen.Assemblies.Component[] componentsToOpen1 = new NXOpen.Assemblies.Component[1];
            NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PACOTES_LUXO_2024/000 1"));
            componentsToOpen1[0] = component1;
            NXOpen.Assemblies.ComponentAssembly.OpenComponentStatus[] openStatus1;
            NXOpen.PartLoadStatus partLoadStatus1;
            partLoadStatus1 = workPart.ComponentAssembly.OpenComponents(NXOpen.Assemblies.ComponentAssembly.OpenOption.ComponentOnly, componentsToOpen1, out openStatus1);

            partLoadStatus1.Dispose();
            theSession.DeleteUndoMark(markId2, null);

            NXOpen.PartLoadStatus partLoadStatus2;
            theSession.Parts.SetWorkComponent(component1, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus2);

            workPart = theSession.Parts.Work; // TPL_PORTA_PACOTES_LUXO_2024/000;1
            partLoadStatus2.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            string marca_ac = busca_fm_tarefas(txt_fm_dados.Text, workPart);

            conexao_sim conn = new conexao_sim();
            DataSet ds = conn.busca_dados_fm(txt_fm_dados.Text);

            string carroceria = "";
            string chassi = "";



            foreach (DataRow row in ds.Tables["tab_pedidos"].Rows)
            {
                foreach (DataColumn column in ds.Tables["tab_pedidos"].Columns)
                {
                    if (column.ColumnName.Contains("v_"))
                    {
                        preenche_expression(column.ColumnName, row[column.ColumnName].ToString(), workPart);
                        list_log.Items.Add(column.ColumnName + " = " + row[column.ColumnName].ToString());
                    }

                    carroceria = row["carroceria"].ToString();

                   lbl_carroceria.Text =row["carr_titulo"].ToString();
                   lbl_chassi.Text = row["cha_titulo"].ToString();
                   lbl_comp.Text = row["v_ct"].ToString();
                    chassi = row["chassi"].ToString();
                   
                }
            }
            list_log.Items.Add("Carroceria = " + carroceria);
            list_log.Items.Add("Chassi = " + chassi);
            string pos_wc = conexao_bd.pos_wc(carroceria, chassi);
            string vao_rec_madeira = conexao_bd.busca_ac(marca_ac.ToUpper());

            
            lbl_ac.Text = marca_ac.ToUpper();
            preenche_expression("v_pos_wc", pos_wc, workPart);
            list_log.Items.Add("v_pos_wc = " + pos_wc);
            preenche_expression("v_recorte_madeira", vao_rec_madeira, workPart); preenche_expression("v_recorte_madeira", vao_rec_madeira, workPart);
            list_log.Items.Add("v_recorte_madeira = " + vao_rec_madeira);

            List<DataComNX> listaRecebida = AppCOM.DataComNX.ReceberListaViaSocket();
            listaRecebida_teste = listaRecebida;
            double pos_ac = 0.0;
            double inicio_pp_le = 0.0;
            double inicio_pp_ld = 0.0;
            double v2_LD_auxiliar = 0.0;
            double v2_LD_auxiliar_1 = 0.0;
            double v2_LE_auxiliar = 0.0;
            double v2_LE_auxiliar_1 = 0.0;
            foreach (var item in listaRecebida)
            {
                inicio_pp_le = item.Pos_Div_LE + 200;
                preenche_expression("dist_p0_inicio_pp_le", inicio_pp_le.ToString().Replace(",", "."), workPart);
                inicio_pp_ld = item.Pos_Div_LD + 200;
                preenche_expression("dist_p0_inicio_pp_ld", inicio_pp_ld.ToString().Replace(",", "."), workPart);

                preenche_expression("qtd_foco_le", item.Poltronas_LE.Count.ToString(), workPart);
                preenche_expression("qtd_foco_ld", item.Poltronas_LD.Count.ToString(), workPart);

                for (int i = 0; i <= item.Poltronas_LD.Count - 1; i++)
                {
                    double v1 = item.Poltronas_LD[i].POS_X - 450;
                    preenche_expression("v_f_ld" + (i + 1).ToString(), v1.ToString(), workPart);
                    add_foco_ld_new(item.Poltronas_LD[i].POS_X, i);
                    preenche_expression("v_polt_ld" + (i + 1).ToString(), item.Poltronas_LD[i].POS_X.ToString(), workPart);
                    add_polt(item.Poltronas_LD[i].POS_X, -800, "v_polt_ld" + (i + 1).ToString());
                }
                for (int i = 0; i <= item.Poltronas_LE.Count - 1; i++)
                {
                    double v1 = item.Poltronas_LE[i].POS_X - 450;
                    preenche_expression("v_f_le" + (i + 1).ToString(), v1.ToString(), workPart);
                    add_foco_le_new(item.Poltronas_LE[i].POS_X, i);
                    preenche_expression("v_polt_le" + (i + 1).ToString(), item.Poltronas_LE[i].POS_X.ToString(), workPart);
                    add_polt(item.Poltronas_LE[i].POS_X, 800, "v_polt_le" + (i + 1).ToString());
                }

                double v1_LD_auxiliar = busca_valor_expressao("pos_inicio_perfil_ext_ld", workPart);
                double v1_LE_auxiliar = busca_valor_expressao("pos_inicio_perfil_ext_le", workPart);

                 v2_LD_auxiliar = busca_valor_expressao("pos_inicio_perfil_ext_ld", workPart);
                 v2_LD_auxiliar_1 = busca_valor_expressao("v_comp_ext_perfil_ld", workPart);

                 v2_LE_auxiliar = busca_valor_expressao("pos_inicio_perfil_ext_le", workPart);
                 v2_LE_auxiliar_1 = busca_valor_expressao("v_comp_ext_perfil_le", workPart);
                remove_componente_referenset("MODEL", "REP_POLTRONA", workPart);
                remove_componente_referenset("DWG", "REP_POLTRONA", workPart);
                referenset_reguas_led();


                NXOpen.Session.UndoMarkId markId22;
                markId22 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");
                NXOpen.Assemblies.Component component2 = (NXOpen.Assemblies.Component)component1.FindObject("COMPONENT TPL_EM_ENTRE_FOCOS/000 1");
                PartLoadStatus partLoadStatus22;
                theSession.Parts.SetWorkComponent(component2, out partLoadStatus22);

                workPart = theSession.Parts.Work;
                partLoadStatus22.Dispose();
                theSession.SetUndoMarkName(markId22, "Make Work Part");
                int qtd_169362 = 0;
                int qtd_169360 = 0;
                int qtd_169363 = 0;
                for (int i = 0; i <= item.Poltronas_LD.Count - 1; i++)
                {
                    string name_1 = "v_inicio_entrefoco_ld_" + i;
                    string name_2 = "v_fim_entrefoco_ld_" + i;
                    double v1 = item.Poltronas_LD[i].POS_X - 450 + 81;

                    double v2 = 0.0;
                    if (i < item.Poltronas_LD.Count - 1)
                    {
                        v2 = item.Poltronas_LD[i + 1].POS_X - 450 - 81;

                    }
                    else
                    {

                        v2 = v2_LD_auxiliar;
                        v2 = v2 + v2_LD_auxiliar_1;

                    }
                    string name_link_1 = "v_f_ld" + (i + 1).ToString() + "+81";
                    string name_link_2 = "v_f_ld" + (i + 2).ToString() + " - 81";
                    if(i == item.Poltronas_LD.Count - 1)
                    {
                        name_link_2 = "v_comp_ext_perfil_ld" + "+0+"+"pos_inicio_perfil_ext_ld";
                       // v_comp_ext_perfil_ld + 0 +pos_inicio_perfil_ext_ld
                       // v_comp_ext_perfil_le + pos_inicio_perfil_ext_le
                    }
                    create_extrude_perfil(v1, v2, "LINKED_CURVE(2)", name_1, name_2, false, name_link_1, name_link_2);
                    if (i == 0)
                    {
                        name_1 = "v_inicio_entrefoco_ini_ld_" + i;
                        name_2 = "v_fim_entrefoco_ini_ld_" + i;
                        v1 = v1_LD_auxiliar;
                        v2 = item.Poltronas_LD[i].POS_X - 450 - 81;
                        name_link_1 = "pos_inicio_perfil_ext_ld";
                        name_link_2 = "v_f_ld1-81";

                        create_extrude_perfil(v1, v2, "LINKED_CURVE(2)", name_1, name_2, true, name_link_1, name_link_2);
                    }
                    if ((v2 - v1) <= 700)
                    {
                        qtd_169360++;
                    }
                    if ((v2 - v1) > 700 && (v2 - v1) <= 800)
                    {
                        qtd_169362++;
                    }
                    if ((v2 - v1) > 800)
                    {
                        qtd_169363++;
                    }

                }

                for (int i = 0; i <= item.Poltronas_LE.Count - 1; i++)
                {
                    string name_1 = "v_inicio_entrefoco_le_" + i;
                    string name_2 = "v_fim_entrefoco_le_" + i;
                    double v1 = item.Poltronas_LE[i].POS_X - 450 + 81;

                    double v2 = 0.0;
                    if (i < item.Poltronas_LE.Count - 1)
                    {
                        v2 = item.Poltronas_LE[i + 1].POS_X - 450 - 81;

                    }
                    else
                    {

                        v2 = v2_LE_auxiliar;
                        v2 = v2 + v2_LE_auxiliar_1;

                    }
                    string name_link_1 = "v_f_le" + (i + 1).ToString() + "+81";
                    string name_link_2 = "v_f_le" + (i + 2).ToString() + " - 81";

                    if (i == item.Poltronas_LE.Count - 1)
                    {
                        name_link_2 = "v_comp_ext_perfil_le" + "+0+"+"pos_inicio_perfil_ext_le";
                    }
                    create_extrude_perfil(v1, v2, "LINKED_CURVE(3)", name_1, name_2, false, name_link_1, name_link_2);
                    if (i == 0)
                    {
                        name_1 = "v_inicio_entrefoco_ini_le_" + i;
                        name_2 = "v_fim_entrefoco_ini_le_" + i;
                        v1 = v1_LE_auxiliar;
                        v2 = item.Poltronas_LE[i].POS_X - 450 - 81;

                        name_link_1 = "pos_inicio_perfil_ext_le";
                        name_link_2 = "v_f_le1-81";
                        create_extrude_perfil(v1, v2, "LINKED_CURVE(3)", name_1, name_2, true, name_link_1, name_link_2);
                    }
                    if ((v2 - v1) <= 700)
                    {
                        qtd_169360++;
                    }
                    if ((v2 - v1) > 700 && (v2 - v1) <= 800)
                    {
                        qtd_169362++;
                    }
                    if ((v2 - v1) > 800)
                    {
                        qtd_169363++;
                    }

                }
                preenche_expression("qtd_169360", qtd_169360.ToString(), workPart);
                preenche_expression("qtd_169362", qtd_169362.ToString(), workPart);
                for (int i = 0; i <= item.Saidas_Emergencia_LE.Count - 1; i++)
                {

                    string name_expression = "v_pos_saida_eme_le_" + (i + 1).ToString();
                    preenche_expression(name_expression, item.Saidas_Emergencia_LE[i].ToString(), workPart);
                }
                for (int i = 0; i <= item.Saidas_Emergencia_LD.Count - 1; i++)
                {

                    string name_expression = "v_pos_saida_eme_ld_" + (i + 1).ToString();
                    preenche_expression(name_expression, item.Saidas_Emergencia_LD[i].ToString(), workPart);
                }
                // recorte();
                pos_ac += item.AC;
            }

            remove_interpart("TPL_EM_ENTRE_FOCOS/000", workPart);
            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");
           
            PartLoadStatus partLoadStatus3;
            theSession.Parts.SetWorkComponent(component1, out partLoadStatus3);

            workPart = theSession.Parts.Work;
            partLoadStatus3.Dispose();
            theSession.SetUndoMarkName(markId3, "Make Work Part");
            
            aplicar_configuracao_new(pos_ac);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");
            remove_interpart("TPL_PORTA_PACOTES_LUXO_2024/000", workPart);
            NXOpen.Assemblies.Component nullAssemblies_Component = null;
            PartLoadStatus partLoadStatus4;
            theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus4);

            workPart = theSession.Parts.Work;
            partLoadStatus4.Dispose();
            theSession.SetUndoMarkName(markId4, "Make Work Part");
            double sup_1_ld = 0.0;
            double sup_1_le = 0.0;

            preenche_expression("marca_ac", "\"" + marca_ac.ToUpper() + "\"", workPart);
            preenche_expression("v_pos_wc", pos_wc, workPart);
            preenche_expression("v_recorte_madeira", vao_rec_madeira, workPart);
            preenche_expression("v_pos_ac", pos_ac.ToString().Replace(",", "."), workPart);

            double limite_frente_sup = 0.0;
            double limite_tras_sup = 0.0;
            if (marca_ac != "Não")
            {
                limite_frente_sup = (pos_ac - (Convert.ToDouble(vao_rec_madeira) / 2)) - 320;
                limite_tras_sup = (pos_ac + (Convert.ToDouble(vao_rec_madeira) / 2)) + 320;
            }
            NXOpen.Assemblies.Component cmp = busca_componente("SUP_LD");
            NXOpen.Assemblies.Component cmp2 = busca_componente("SUP_LD_MENOR");
            NXOpen.Assemblies.Component cmp3 = busca_componente("BUCHA_LD_1");
            NXOpen.Assemblies.Component cmp4 = busca_componente("BUCHA_LD_2");
            NXOpen.Assemblies.Component cmp5 = busca_componente("ACAB_LD");
            int qtd_029944 = 0;
            int qtd_057420 = 0;
            int qtd_003120 = 0;
            int cont_pe_menor = 0;
            double pos_x_pe_menor_1 = 0.0;
            foreach (var item in listaRecebida)
            {
                
                for (int i = 0; i <= item.Suportes_LD.Count - 1; i++)
                {
                   
                    if (i > 0)
                    {
                       //MessageBox.Show((item.Suportes_LD[i] - 27).ToString());
                       list_log.Items.Add("v_pos_ld_sup" + (i + 1).ToString() +" " + (item.Suportes_LD[i]).ToString().Replace(",", "."));
                        preenche_expression("v_pos_ld_sup" + (i + 1).ToString(), (item.Suportes_LD[i]).ToString().Replace(",", "."), workPart);
                        double pos_x = (item.Suportes_LD[i]);
                        if (marca_ac != "Não")
                        {
                            if (pos_x < limite_frente_sup || pos_x > limite_tras_sup)
                            {
                                copiar_item_em_x(pos_x, workPart, cmp, true, i, "ld");
                                copiar_item_em_x(pos_x, workPart, cmp3, false, 0,"");
                                copiar_item_em_x(pos_x, workPart, cmp4, false, 0, "");
                                copiar_item_em_x(pos_x, workPart, cmp5, false, 0, "");
                                qtd_029944++;


                            }
                            else
                            {
                                if (cont_pe_menor > 0)
                                {
                                    if (cont_pe_menor == 2)
                                    {
                                        copiar_item_em_x(pos_x, workPart, cmp2, false, 0, "");
                                        copiar_item_em_x(pos_x, workPart, cmp4, false, 0, "");

                                    }

                                    qtd_029944++;
                                    preenche_expression("v_pos_ld_sup" + (cont_pe_menor + 1).ToString() + "_menor", pos_x.ToString().Replace(",", "."), workPart);
                                    list_log.Items.Add("v_pos_ld_sup" + (cont_pe_menor + 1).ToString() + "_menor" + " " + pos_x.ToString().Replace(",", "."));
                            }
                                else
                                {
                                    pos_x_pe_menor_1 = pos_x;
                                }
                                cont_pe_menor++;
                            }
                        }
                        else
                        {
                            copiar_item_em_x(pos_x, workPart, cmp, true, i, "ld");
                            copiar_item_em_x(pos_x, workPart, cmp3, false, 0,"");
                            copiar_item_em_x(pos_x, workPart, cmp4, false, 0, "");
                            copiar_item_em_x(pos_x, workPart, cmp5, false, 0,"");
                            qtd_029944++;
                        }



                    }
                }

            }
            
            if (cont_pe_menor > 0)
            {
                preenche_expression("v_pos_ld_sup1_menor", pos_x_pe_menor_1.ToString().Replace(",", "."), workPart);
                list_log.Items.Add("v_pos_ld_sup1_menor" + " " + pos_x_pe_menor_1.ToString().Replace(",", "."));
                qtd_029944++;
            }
            cmp4 = null;
            cont_pe_menor = 0;
            cmp = busca_componente("SUP_LE");
            cmp2 = busca_componente("SUP_LE_MENOR");
            cmp3 = busca_componente("BUCHA_LE_1");
            cmp4 = busca_componente("BUCHA_LE_2");
            cmp5 = busca_componente("ACAB_LE");
            foreach (var item in listaRecebida)
            {

                for (int i = 0; i <= item.Suportes_LE.Count - 1; i++)
                {
                    if (i > 0)
                    { 
                    list_log.Items.Add("v_pos_le_sup" + (i + 1).ToString() + " " + (item.Suportes_LE[i]).ToString().Replace(",", "."));
                    preenche_expression("v_pos_le_sup" + (i + 1).ToString(), (item.Suportes_LE[i]).ToString().Replace(",", "."), workPart);
                        double pos_x = (item.Suportes_LE[i]);
                        if (marca_ac != "Não")
                        {
                            if (pos_x < limite_frente_sup || pos_x > limite_tras_sup)
                            {
                                copiar_item_em_x(pos_x, workPart, cmp, true, i, "le");
                                copiar_item_em_x(pos_x, workPart, cmp3, false, 0, "");
                                copiar_item_em_x(pos_x, workPart, cmp5, false, 0,"");
                                qtd_029944++;
                            }
                            else
                            {
                                if (cont_pe_menor > 0)
                                {
                                    if (cont_pe_menor == 2)
                                    {
                                        copiar_item_em_x(pos_x, workPart, cmp2, false, 0,"");
                                        copiar_item_em_x(pos_x, workPart, cmp4, false, 0,"");
                                    }
                                    preenche_expression("v_pos_le_sup" + (cont_pe_menor + 1).ToString() + "_menor", pos_x.ToString().Replace(",", "."), workPart);
                                    list_log.Items.Add("v_pos_le_sup" + (cont_pe_menor + 1).ToString() + "_menor" + " " + pos_x.ToString().Replace(",", "."));
                                    qtd_029944++;
                                }
                                else
                                {
                                    pos_x_pe_menor_1 = pos_x;
                                }
                                cont_pe_menor++;
                            }
                        }
                        else
                        {
                            copiar_item_em_x(pos_x, workPart, cmp, true,   i,"le");
                            copiar_item_em_x(pos_x, workPart, cmp3, false, 0,"");
                            copiar_item_em_x(pos_x, workPart, cmp5, false, 0,"");
                            qtd_029944++;
                        }

                    }

                }
            }
            if (cont_pe_menor > 0)
            {
                preenche_expression("v_pos_le_sup1_menor", pos_x_pe_menor_1.ToString().Replace(",", "."), workPart);
                qtd_029944++;
            }
            foreach (var item in listaRecebida)
            {
                for (int i = 0; i <= item.Suportes_LD.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        preenche_expression("v_pos_ld_sup" + (i + 1).ToString(), (item.Suportes_LD[i]).ToString().Replace(",", "."), workPart);
                        qtd_029944++;
                    }

                }
                for (int i = 0; i <= item.Suportes_LE.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        preenche_expression("v_pos_le_sup" + (i + 1).ToString(), (item.Suportes_LE[i]).ToString().Replace(",", "."), workPart);
                        qtd_029944++;
                    }
                }
            }
            qtd_057420 = qtd_029944*4;
            qtd_003120 = qtd_029944 * 3;
            preenche_expression("qtd_057420", qtd_057420.ToString(), workPart);
            preenche_expression("qtd_003120", qtd_003120.ToString(), workPart);
            preenche_expression("qtd_029944", qtd_029944.ToString(), workPart);

            calculo_fechamento_curva(workPart, marca_ac);
            
            remove_interpart("TPL_EM_PORTA_PACOTES_LUXO_2024/000", workPart);

        }
        private void calculo_fechamento_curva(Part workPart, string ac)
        {

            Session theSession;
            theSession = Session.GetSession();
         

            double[] lista_ld = busca_valor_expressao_list_double("lista_vao_fechamentos_ld", workPart);
            double[] lista_le = busca_valor_expressao_list_double("lista_vao_fechamentos_le", workPart);

            List<double> lista_final_ld = new List<double>();
            List<double> lista_final_le = new List<double>();

            foreach (object obj in lista_ld)
            {
                double valor = Convert.ToDouble(obj);
                if (valor != 0)
                {
                    lista_final_ld.Add(valor);

                }
            }
            foreach (object obj in lista_le)
            {
                double valor = Convert.ToDouble(obj);
                if (valor != 0)
                {
                    lista_final_le.Add(valor);

                }
            }

            double pos_supld1_central_ac = lista_final_ld[lista_final_ld.Count - 3];// Convert.ToDouble(busca_valor_expressao("v_pos_ld_sup2_menor", workPart));
            double pos_supld_central_ac = lista_final_ld[lista_final_ld.Count - 2];
            double pos_supld2_central_ac = lista_final_ld[lista_final_ld.Count - 1];  // double pos_suple2_central_ac = Convert.ToDouble(busca_valor_expressao("v_pos_le_sup2_menor", workPart));
            list_log.Items.Add("pos_supld_central_ac: " + pos_supld_central_ac);
            double pos_suple1_central_ac = lista_final_le[lista_final_le.Count - 3];// Convert.ToDouble(busca_valor_expressao("v_pos_ld_sup2_menor", workPart));
            double pos_suple_central_ac = lista_final_le[lista_final_le.Count - 2];
            double pos_suple2_central_ac = lista_final_le[lista_final_le.Count - 1];

            double comp_1_ld = 0.0;
            double comp_2_ld = 0.0;

            double comp_1_le = 0.0;
            double comp_2_le = 0.0;

            List<double> comprimentos = new List<double>();

            int[] sup_ld_exluir = new int[4];
            int[] sup_le_exluir = new int[4];

            bool flg = true;
            int m = 0;

            if (ac != "Não")
            { 
                while (flg)
                {
                    double pos_suporte = lista_final_ld[m];
                    list_log.Items.Add("M-" + m.ToString() + "     " + pos_suporte.ToString());
                    if (pos_suporte == pos_supld_central_ac)
                    {
                        list_log.Items.Add("Ac_M-"+m.ToString()+"     "+ pos_suporte.ToString());
                        double pos_suporte_1 = lista_final_ld[m - 2];
                        double pos_suporte_2 = lista_final_ld[m + 2];
                        comp_1_ld = pos_supld_central_ac - pos_suporte_1;
                        comp_2_ld = pos_suporte_2 - pos_supld_central_ac;

                        if (comp_1_ld >= 1397 && comp_1_ld <= 1403)
                        {
                            comp_1_ld = 1400;
                        }
                        if (comp_1_ld >= 1717 && comp_1_ld <= 1723)
                        {
                            comp_1_ld = 1720;
                        }
                        if (comp_1_ld >= 1147 && comp_1_ld <= 1153)
                        {
                            comp_1_ld = 1150;
                        }
                        if (comp_2_ld >= 1397 && comp_2_ld <= 1403)
                        {
                            comp_2_ld = 1400;
                        }
                        if (comp_2_ld >= 1717 && comp_2_ld <= 1723)
                        {
                            comp_2_ld = 1720;
                        }
                        if (comp_2_ld >= 1147 && comp_2_ld <= 1153)
                        {
                            comp_2_ld = 1150;
                        }
                        comprimentos.Add(comp_1_ld);
                        comprimentos.Add(comp_2_ld);
                        sup_ld_exluir[0] = m - 2;
                        sup_ld_exluir[1] = m - 1;
                        sup_ld_exluir[2] = m;
                        sup_ld_exluir[3] = m + 1;


                        flg = false;
                    }
                    m++;

                }

                flg = true;
                m = 0;
                while (flg)
                {
                    double pos_suporte = lista_final_le[m];
                    if (pos_suporte == pos_suple_central_ac)
                    {
                        double pos_suporte_1 = lista_final_le[m - 2];
                        double pos_suporte_2 = lista_final_le[m + 2];
                        comp_1_le = pos_suple_central_ac - pos_suporte_1;
                        comp_2_le = pos_suporte_2 - pos_suple_central_ac;

                        if (comp_1_le >= 1397 && comp_1_le <= 1403)
                        {
                            comp_1_le = 1400;
                        }
                        if (comp_1_le >= 1717 && comp_1_le <= 1723)
                        {
                            comp_1_le = 1720;
                        }
                        if (comp_1_le >= 1147 && comp_1_le <= 1153)
                        {
                            comp_1_le = 1150;
                        }
                        if (comp_2_ld >= 1397 && comp_2_le <= 1403)
                        {
                            comp_2_le = 1400;
                        }
                        if (comp_2_le >= 1717 && comp_2_le <= 1723)
                        {
                            comp_2_le = 1720;
                        }
                        if (comp_2_le >= 1147 && comp_2_le <= 1153)
                        {
                            comp_2_le = 1150;
                        }
                        comprimentos.Add(comp_1_le);
                        comprimentos.Add(comp_2_le);
                        sup_le_exluir[0] = m - 2;
                        sup_le_exluir[1] = m - 1;
                        sup_le_exluir[2] = m;
                        sup_le_exluir[3] = m + 1;


                        flg = false;
                    }
                    m++;

                }
            }
            double comprimento_resto = 0.0;
            double pos_ld = lista_final_ld[0];
            double pos_le = lista_final_le[0];

            double inicio_pp_ld = lista_final_ld[lista_final_ld.Count - 4];
            double inicio_pp_le = lista_final_le[lista_final_le.Count - 4];

            comprimento_resto = comprimento_resto + (pos_ld - inicio_pp_ld);

            comprimento_resto = comprimento_resto + (pos_le - inicio_pp_le);

            double v2_LD_auxiliar = 0.0;
            double v2_LD_auxiliar_1 = 0.0;

            double v2_LE_auxiliar = 0.0;
            double v2_LE_auxiliar_1 = 0.0;

            v2_LD_auxiliar = lista_final_ld[lista_final_ld.Count - 6];
            v2_LD_auxiliar_1 = lista_final_ld[lista_final_ld.Count - 5];

            v2_LE_auxiliar = lista_final_le[lista_final_le.Count - 6];
            v2_LE_auxiliar_1 = lista_final_le[lista_final_le.Count - 5];

            pos_ld = lista_final_ld[lista_final_ld.Count - 7];
            pos_le = lista_final_le[lista_final_le.Count - 7];

            comprimento_resto = comprimento_resto + (v2_LD_auxiliar + v2_LD_auxiliar_1) - pos_ld;

            comprimento_resto = comprimento_resto + (v2_LE_auxiliar + v2_LE_auxiliar_1) - pos_le;

            for (int i = 0; i <= lista_final_ld.Count - 8; i++)
            {
                if (!sup_ld_exluir.Contains(i))
                {
                    double comp = lista_final_ld[i + 1] - lista_final_ld[i];

                    if (comp >= 1397 && comp <= 1403)
                    {
                        comp = 1400;
                    }
                    if (comp >= 1717 && comp <= 1723)
                    {
                        comp = 1720;
                    }
                    if (comp >= 1147 && comp <= 1153)
                    {
                        comp = 1150;
                    }
                    comprimentos.Add(comp);
                }
            }
            for (int i = 0; i <= lista_final_le.Count - 8; i++)
            {
                if (!sup_le_exluir.Contains(i))
                {
                    double comp = lista_final_le[i + 1] - lista_final_le[i];
                    if (comp >= 1397 && comp <= 1403)
                    {
                        comp = 1400;
                    }
                    if (comp >= 1717 && comp <= 1723)
                    {
                        comp = 1720;
                    }
                    if (comp >= 1147 && comp <= 1153)
                    {
                        comp = 1150;
                    }
                    comprimentos.Add(comp);
                }
            }

            for (int i = 0; i <= lista_final_ld.Count - 8; i++)
            {
                if (lista_final_ld[i] == pos_supld1_central_ac)
                {
                  preenche_expression("v_pos_caixa_porta_pertence", (lista_final_ld[i-1]-190).ToString().Replace(",", "."), workPart);
                }
            }
            int qtd_252248 = 0;

            int qtd_275096 = 0;

            int qtd_275100 = 0;

            foreach (var comprimento in comprimentos)
            {
                if (comprimento <= 1150)
                {
                    qtd_275100++;
                }
                if (comprimento > 1150 && comprimento <= 1400)
                {
                    qtd_275096++;
                }
                if (comprimento > 1400 && comprimento <= 1720)
                {
                    qtd_252248++;
                }
            }
            if (comprimento_resto <= 1150)
            {
                qtd_275100++;
            }
            if (comprimento_resto > 1150 && comprimento_resto <= 1400)
            {
                qtd_275096++;
            }
            if (comprimento_resto > 1400 && comprimento_resto <= 1720)
            {
                qtd_252248++;
            }
            preenche_expression("qtd_275100", qtd_275100.ToString(), workPart);
            preenche_expression("qtd_275096", qtd_275096.ToString(), workPart);
            preenche_expression("qtd_252248", qtd_252248.ToString(), workPart);


        }
        public List<NXOpen.Assemblies.Component> busca_componente_led(string nome)
        {
            Session theSession;
            theSession = Session.GetSession();
            Part wp = theSession.Parts.Work;
            //item_pai = "";
            //item_pai = wp.FullPath;

         
            List<NXOpen.Assemblies.Component> leds = new List<NXOpen.Assemblies.Component>();
            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();
            List<NXOpen.Assemblies.Component> Componentes_n2 = new List<NXOpen.Assemblies.Component>();

            int ini, fim, ini1, fim1;

            NXOpen.Assemblies.Component cmp1 = wp.ComponentAssembly.RootComponent;
            NXOpen.Assemblies.Component cmp_return = null;
            //item_master = cmp1.DisplayName.Split('/');

            Componentes.Add(cmp1);

            ini1 = 0;
            fim1 = 1;

            Componentes.ToArray()[0].GetChildren();

            while (ini1 < fim1)
            {

                ini = ini1;
                fim = fim1;

                ini1 = fim;
                fim1 = ini1;
                for (int i = ini; i < fim; i++)
                {
                    NXOpen.Assemblies.Component[] cmps = Componentes.ToArray()[i].GetChildren();

                    foreach (NXOpen.Assemblies.Component cmp in cmps)
                    {
                        string desc = cmp.GetStringAttribute("DB_PART_NAME");

                        if (desc.Contains(nome))
                        {
                            leds.Add(cmp);
                        }
                    }
                }
            }
            return leds;
        }
        public NXOpen.Assemblies.Component busca_componente (string nome)
        {
            Session theSession;
            theSession = Session.GetSession();
            Part wp = theSession.Parts.Work;
            //item_pai = "";
            //item_pai = wp.FullPath;

            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();
            List<NXOpen.Assemblies.Component> Componentes_n2 = new List<NXOpen.Assemblies.Component>();

            int ini, fim, ini1, fim1;

            NXOpen.Assemblies.Component cmp1 = wp.ComponentAssembly.RootComponent;
            NXOpen.Assemblies.Component cmp_return = null;
            //item_master = cmp1.DisplayName.Split('/');

            Componentes.Add(cmp1);

            ini1 = 0;
            fim1 = 1;

            Componentes.ToArray()[0].GetChildren();

            while (ini1 < fim1)
            {

                ini = ini1;
                fim = fim1;

                ini1 = fim;
                fim1 = ini1;
                for (int i = ini; i < fim; i++)
                {
                    NXOpen.Assemblies.Component[] cmps = Componentes.ToArray()[i].GetChildren();

                    foreach (NXOpen.Assemblies.Component cmp in cmps)
                    {
                        string desc = cmp.GetStringAttribute("INF");
                      
                        if (desc == nome)
                        {
                           
                            cmp_return =  cmp;

                        }
                   


                    }
                }
            }
            return cmp_return;
        }

        private string busca_fm_tarefas(string fm, Part workPart)
        {
            string marca_ac = "";
            string colete_toracico = "";
            conexao_bd _busca_dados = new conexao_bd();

            string fm_retorno = busca_itens_tarefa.busca_tarefas(fm);

            JObject objeto_json = JObject.Parse(fm_retorno);

            JToken status = objeto_json["status"];

            List<item_tarefas.Item> lista = new List<item_tarefas.Item>();
            if (status.ToString() != "404")
            {

                foreach (var item in objeto_json["tarefas"])
                {
                    lista.Add(new item_tarefas.Item
                    {
                        oitem_pk_grupo = item["oitem_pk_grupo"].ToString(),
                        gru_titulo = item["gru_titulo"].ToString(),
                        sub_grupo = item["sub_grupo"].ToString(),
                        sub_referencia = item["sub_referencia"].ToString(),
                        sub_titulo = item["sub_titulo"].ToString(),
                        item_grupo = item["item_grupo"].ToString(),
                        item_referencia = item["item_referencia"].ToString(),
                        item_titulo = item["item_titulo"].ToString(),
                        item_id = item["item_id"].ToString(),
                    });
                }



                TreeNode n1 = new TreeNode();


                string tafera_sanitario = "";
               
                tree2.Nodes.Clear();
                n1.Nodes.Clear();
                string grupo = "";
                TreeNodeAdv n1_2 = new TreeNodeAdv();
                foreach (var item in objeto_json["tarefas"])
                {
                    TreeNode sub_item = new TreeNode();

                    TreeNode item_referencia = new TreeNode();
                    if (grupo == item["oitem_pk_grupo"].ToString())
                    {
                        sub_item = n1.Nodes.Add(item["sub_grupo"].ToString() + "." + item["sub_referencia"].ToString() + " - " + item["sub_titulo"].ToString());
                        sub_item.BackColor = System.Drawing.Color.FromArgb(201, 201, 201);
                        item_referencia = sub_item.Nodes.Add(item["item_grupo"].ToString() + "." + item["sub_referencia"].ToString() + "." + item["item_referencia"].ToString() + " - " + item["item_titulo"].ToString());
                        if (item["item_grupo"].ToString() == "7")
                        {
                            tafera_sanitario = item["item_grupo"].ToString() + "." + item["sub_referencia"].ToString() + "." + item["item_referencia"].ToString() + " - " + item["item_titulo"].ToString();
                        }
                        if (item["item_grupo"].ToString() == "4" && item["sub_referencia"].ToString() == "4")
                        {

                            marca_ac = item["item_titulo"].ToString();
                        }
                        if (item["item_grupo"].ToString() == "11" && item["sub_referencia"].ToString() == "15" && item["item_referencia"].ToString() =="4")
                        {
                            colete_toracico = item["item_titulo"].ToString();
                        }
                        item_referencia.BackColor = System.Drawing.Color.FromArgb(122, 122, 122);
                        item_referencia.ForeColor = System.Drawing.Color.WhiteSmoke;
                    }
                    else
                    {
                       
                        n1 = tree2.Nodes.Add(item["oitem_pk_grupo"].ToString() + " - " + item["gru_titulo"].ToString());
                        n1.BackColor = System.Drawing.Color.FromArgb(189, 189, 189);
                        sub_item = n1.Nodes.Add(item["sub_grupo"].ToString() + "." + item["sub_referencia"].ToString() + " - " + item["sub_titulo"].ToString());
                        sub_item.BackColor = System.Drawing.Color.FromArgb(201, 201, 201);
                        item_referencia = sub_item.Nodes.Add(item["item_grupo"].ToString() + "." + item["sub_referencia"].ToString() + "." + item["item_referencia"].ToString() + " - " + item["item_titulo"].ToString());
                        if (item["item_grupo"].ToString() == "7")
                        {
                            tafera_sanitario = item["item_grupo"].ToString() + "." + item["sub_referencia"].ToString() + "." + item["item_referencia"].ToString() + " - " + item["item_titulo"].ToString();
                        }
                        

                        item_referencia.BackColor = System.Drawing.Color.FromArgb(122, 122, 122);
                        item_referencia.ForeColor = System.Drawing.Color.WhiteSmoke;
                    }
                    grupo = item["oitem_pk_grupo"].ToString();
                }
                tafera_sanitario = "\""+tafera_sanitario+"\"";
                colete_toracico = "\"" + colete_toracico + "\"";

                lbl_wc.Text = tafera_sanitario;
                preenche_expression("t_sanitario", tafera_sanitario, workPart);
                preenche_expression("t_colete_toracico", colete_toracico, workPart);
            }
            
            return marca_ac;
        }
        public void create_extrude_perfil(double v1, double v2, string name_feature, string name_1, string name_2, bool recorte, string v_string_1, string v_string_2)
        {

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            
          

            workPart = theSession.Parts.Work;

            theSession.SetUndoMarkName(markId1, "Make Work Part");

            theSession.Preferences.Modeling.UpdatePending = false;



            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            Expression expression1;
            expression1 = workPart.Expressions.CreateWithUnits(name_1 + "=" + v_string_1, unit1);

            Expression expression2;
          
            expression2 = workPart.Expressions.CreateWithUnits(name_2 + "=" + v_string_2, unit1);
            extrude_entre_focos(workPart,expression1, expression2, v1, v2, name_feature, recorte);


            theSession.Preferences.Modeling.UpdatePending = false;
            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId1);


            //  MessageBox.Show(c);
        }
        private void button2_Click(object sender, EventArgs e)
        {
           
        }
        public static double busca_valor_expressao(string name_exp_1, Part workPart)
        {
            //Session theSession = Session.GetSession();
            //Part workPart = theSession.Parts.Work;
            //Part displayPart = theSession.Parts.Display;

            Expression expression1 = (Expression)workPart.Expressions.FindObject(name_exp_1);


            return expression1.Value;
           
        }
        public static double[] busca_valor_expressao_list_double(string name_exp_1, Part workPart)
        {
            //Session theSession = Session.GetSession();
            //Part workPart = theSession.Parts.Work;
            //Part displayPart = theSession.Parts.Display;

            Expression expression1 = (Expression)workPart.Expressions.FindObject(name_exp_1);

            object[] listObjects = (object[])expression1.GetListValue();

            // Converter o object[] para double[] manualmente
            double[] listValues = Array.ConvertAll(listObjects, item => (double)item);

            //foreach (double value in listValues)
            //{
            //   MessageBox.Show(value.ToString());
            //}
         

                return listValues;

        }

        public static void preenche_expression(string expression_name, string value, Part workPart)
        {
            Session theSession = Session.GetSession();
            
            theSession.Preferences.Modeling.UpdatePending = false;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Expression");

            Expression expression1 = (Expression)workPart.Expressions.FindObject(expression_name);
            Unit unit1 = null;
            workPart.Expressions.EditWithUnits(expression1, unit1, value);

            theSession.Preferences.Modeling.UpdatePending = false;

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId1);
        } 
        public static void remove_interpart(string part_name, Part workPart)
        {
            Session theSession = Session.GetSession();
            
            theSession.Preferences.Modeling.UpdatePending = false;

            theSession.Preferences.Modeling.UpdatePending = false;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Expression");

            bool found1;
            found1 = workPart.Expressions.RemoveInterpartReferences(part_name);

            theSession.Preferences.Modeling.UpdatePending = false;

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId1);
        }
        
        public void extrude_entre_focos( Part workPart, Expression expression_1, Expression expression_2, double v1, double v2, string name_feature, bool recorte)
        {
            NXOpen.Features.Feature nullFeatures_Feature = null;

            if (!workPart.Preferences.Modeling.GetHistoryMode())
            {
                throw new Exception("Create or edit of a Feature was recorded in History Mode but playback is in History-Free Mode.");
            }

            NXOpen.Features.ExtrudeBuilder extrudeBuilder1;
            extrudeBuilder1 = workPart.Features.CreateExtrudeBuilder(nullFeatures_Feature);

            Section section1;
            section1 = workPart.Sections.CreateSection(0.0095, 0.01, 0.5);

            extrudeBuilder1.Section = section1;

            extrudeBuilder1.AllowSelfIntersectingSection(true);

            Unit unit1;
            unit1 = extrudeBuilder1.Draft.FrontDraftAngle.Units;

            Expression expression1;
            expression1 = workPart.Expressions.CreateSystemExpressionWithUnits("2.00", unit1);

            extrudeBuilder1.DistanceTolerance = 0.01;

            extrudeBuilder1.BooleanOperation.Type = NXOpen.GeometricUtilities.BooleanOperation.BooleanType.Create;

            Body[] targetBodies1 = new Body[1];
            Body nullBody = null;
            targetBodies1[0] = nullBody;
            extrudeBuilder1.BooleanOperation.SetTargetBodies(targetBodies1);

            extrudeBuilder1.Limits.StartExtend.Value.RightHandSide = v1.ToString().Replace(",",".");

            extrudeBuilder1.Limits.EndExtend.Value.RightHandSide = v2.ToString().Replace(",", ".");

            extrudeBuilder1.BooleanOperation.Type = NXOpen.GeometricUtilities.BooleanOperation.BooleanType.Create;

            Body[] targetBodies2 = new Body[1];
            targetBodies2[0] = nullBody;
            extrudeBuilder1.BooleanOperation.SetTargetBodies(targetBodies2);

            extrudeBuilder1.Draft.FrontDraftAngle.RightHandSide = "2";

            extrudeBuilder1.Draft.BackDraftAngle.RightHandSide = "2";

            extrudeBuilder1.Offset.StartOffset.RightHandSide = "0";

            extrudeBuilder1.Offset.EndOffset.RightHandSide = "5";

            NXOpen.GeometricUtilities.SmartVolumeProfileBuilder smartVolumeProfileBuilder1;
            smartVolumeProfileBuilder1 = extrudeBuilder1.SmartVolumeProfile;

            smartVolumeProfileBuilder1.OpenProfileSmartVolumeOption = false;

            smartVolumeProfileBuilder1.CloseProfileRule = NXOpen.GeometricUtilities.SmartVolumeProfileBuilder.CloseProfileRuleType.Fci;

           // theSession.SetUndoMarkName(markId1, "Extrude Dialog");

            section1.DistanceTolerance = 0.01;

            section1.ChainingTolerance = 0.0095;

            section1.SetAllowedEntityTypes(NXOpen.Section.AllowTypes.OnlyCurves);

          

            NXOpen.Features.Feature[] features1 = new NXOpen.Features.Feature[1];
            NXOpen.Features.CompositeCurve compositeCurve1 = (NXOpen.Features.CompositeCurve)workPart.Features.FindObject(name_feature);
            features1[0] = compositeCurve1;
            CurveFeatureRule curveFeatureRule1;
            //features1[0].SetName = "_extrude";
            curveFeatureRule1 = workPart.ScRuleFactory.CreateRuleCurveFeature(features1);

            section1.AllowSelfIntersection(true);

            SelectionIntentRule[] rules1 = new SelectionIntentRule[1];
            rules1[0] = curveFeatureRule1;
            NXObject nullNXObject = null;
            Point3d helpPoint1 = new Point3d(0.0, 0.0, 0.0);
            section1.AddToSection(rules1, nullNXObject, nullNXObject, nullNXObject, helpPoint1, NXOpen.Section.Mode.Create, false);

      

            Point3d origin1 = new Point3d(-785.159512317505, 0.0, 1519.94446824039);
            Vector3d vector1 = new Vector3d(0.0, 1.0, 0.0);
            Direction direction1;
            direction1 = workPart.Directions.CreateDirection(origin1, vector1, NXOpen.SmartObject.UpdateOption.WithinModeling);

            extrudeBuilder1.Direction = direction1;

            Unit unit2;
            unit2 = extrudeBuilder1.Offset.StartOffset.Units;

            Expression expression2;
            expression2 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit2);


            extrudeBuilder1.Limits.StartExtend.Value.RightHandSide = expression_1.Name;

            extrudeBuilder1.Limits.EndExtend.Value.RightHandSide = expression_2.Name;

          
            
            extrudeBuilder1.ParentFeatureInternal = false;

            NXOpen.Features.Feature feature1;

            
            feature1 = extrudeBuilder1.CommitFeature();
            
            extrudeBuilder1.Destroy();
            if (recorte == true)
            {
                string name_extrude = "";
                foreach (NXOpen.Features.Feature item in workPart.Features)
                {
                    name_extrude = item.GetFeatureName();
                   
                }
                

                recorte_perfil(name_extrude.ToUpper(), workPart);
            }
            
        }
        public class MySelect
        {
            public static NXOpen.Assemblies.Component SelectAComponent(string msg, Selection.SelectionScope scope)
            {
                TaggedObject selObj = null;
                Point3d cursor;

                // Definindo a máscara para seleção de componentes
                var masks = new[] { new Selection.MaskTriple(UFConstants.UF_component_type, 0, 0) };

                // Realizando a seleção de um objeto marcado (TaggedObject)
                Selection.Response resp = UI.GetUI().SelectionManager.SelectTaggedObject(
                    msg, msg, // prompt e título
                    scope, // escopo de seleção
                    Selection.SelectionAction.ClearAndEnableSpecific,
                    false, false, // includeFeatures, keepHighlighted
                    masks,
                    out selObj, out cursor);

                // Verificar se um objeto foi selecionado
                if (resp != Selection.Response.ObjectSelected)
                    return null;

                // Verificar se o objeto selecionado é um componente do tipo NXOpen.Assemblies.Component
                NXOpen.Assemblies.Component comp = selObj as NXOpen.Assemblies.Component;
                return comp;
            }
            public static NXOpen.Assemblies.Component[] SelectMultipleComponents(string msg, Selection.SelectionScope scope, Part workpart)
            {
                
                TaggedObject[] selObjs = null;
                Point3d[] cursors;

                // Definindo a máscara para seleção de componentes
                var masks = new[] { new Selection.MaskTriple(UFConstants.UF_component_type, 0, 0) };

                // Realizando a seleção de múltiplos objetos marcados (TaggedObjects)
                Selection.Response resp = UI.GetUI().SelectionManager.SelectTaggedObjects(
                    msg, msg, // prompt e título
                    scope, // escopo de seleção
                    Selection.SelectionAction.ClearAndEnableSpecific,
                    false, false, // includeFeatures, keepHighlighted
                    masks,
                    out selObjs);

                
                NXOpen.Assemblies.Component[] components = new NXOpen.Assemblies.Component[selObjs.Length];
                
                int i = 0;
                foreach(var item in selObjs)
                {
                    components[i] = item as NXOpen.Assemblies.Component;
                    i++;
                }
                int ld = 1;
                int le = 1;
                foreach (var comp in components)
                {
                    // Obter o ponto de origem e a matriz de orientação de cada componente
                    Point3d ponto;
                    Matrix3x3 matriz;
                    comp.GetPosition(out ponto, out matriz);
                    if(ponto.X <0)
                    {

                        frm_PortaPacotes.preenche_expression("v_pos_af" + ld + "_ld", ponto.Y.ToString().Replace(",", "."), workpart);
                        ld++;
                    }
                    if(ponto.X > 0)
                    {
                        frm_PortaPacotes.preenche_expression("v_pos_af" + le + "_le", ponto.Y.ToString().Replace(",", "."), workpart);
                        le++;
                    }
                    // Exibir a posição de origem de cada componente
                   // MessageBox.Show($"Componente: {comp.Name}\nPosição de Origem:\nX: {ponto.X}\nY: {ponto.Y}\nZ: {ponto.Z}", "Ponto de Origem");
                }

                return components;
            }
            public static (string exp_name, double exp_valor) SelectMultipleComponents_edit(string msg, Selection.SelectionScope scope, Part workpart)
            {

                string exp_name = "";
                double exp_valor = 0.0;
                TaggedObject[] selObjs = null;
                Point3d[] cursors;

                // Definindo a máscara para seleção de componentes
                var masks = new[] { new Selection.MaskTriple(UFConstants.UF_component_type, 0, 0) };

                // Realizando a seleção de múltiplos objetos marcados (TaggedObjects)
                Selection.Response resp = UI.GetUI().SelectionManager.SelectTaggedObjects(
                    msg, msg, // prompt e título
                    scope, // escopo de seleção
                    Selection.SelectionAction.ClearAndEnableSpecific,
                    false, false, // includeFeatures, keepHighlighted
                    masks,
                    out selObjs);


                NXOpen.Assemblies.Component[] components = new NXOpen.Assemblies.Component[selObjs.Length];

                int i = 0;
                foreach (var item in selObjs)
                {
                    components[i] = item as NXOpen.Assemblies.Component;
                    i++;
                }
                int ld = 1;
                int le = 1;
                foreach (var comp in components)
                {
                    

                    NXOpen.Positioning.ComponentConstraint[] constraints = comp.GetConstraints();

                        // Assuming you want to work with the first constraint, you can access it like this:
                        if (constraints.Length > 0)
                        {
                            NXOpen.Positioning.ComponentConstraint componentConstraint1 = constraints[0];

                        exp_name = componentConstraint1.Expression.RightHandSide.ToString();
                        exp_valor = componentConstraint1.Expression.Value;
                        //MessageBox.Show(componentConstraint1.Expression.RightHandSide.ToString());
                    
                    }

                    // Obter o ponto de origem e a matriz de orientação de cada componente
                    //Point3d ponto;
                    //Matrix3x3 matriz;
                    //comp.GetPosition(out ponto, out matriz);
                    //if (ponto.X < 0)
                    //{

                    //    frm_PortaPacotes.preenche_expression("v_pos_af" + ld + "_ld", ponto.Y.ToString().Replace(",", "."), workpart);
                    //    ld++;
                    //}
                    //if (ponto.X > 0)
                    //{
                    //    frm_PortaPacotes.preenche_expression("v_pos_af" + le + "_le", ponto.Y.ToString().Replace(",", "."), workpart);
                    //    le++;
                    //}
                    // Exibir a posição de origem de cada componente
                    // MessageBox.Show($"Componente: {comp.Name}\nPosição de Origem:\nX: {ponto.X}\nY: {ponto.Y}\nZ: {ponto.Z}", "Ponto de Origem");
                }

                return (exp_name, exp_valor);
            }
        }
        
        private void btn_selecionar_af_Click(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PACOTES_LUXO_2024/000 1");
            PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

            workPart = theSession.Parts.Work;
            partLoadStatus1.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            // Selecionar múltiplos componentes
            NXOpen.Assemblies.Component[] components = MySelect.SelectMultipleComponents("Selecione os Focos para posicionar os AltoFalantes ", Selection.SelectionScope.WorkPartAndOccurrence, workPart);

        }
        private void remove_componente_referenset(string referenceset, string componente, Part workPart)
        {

            Session theSession;
            theSession = Session.GetSession();
         
            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Edit Objects of Reference Set");
            ReferenceSet referenceSet1 = null;
            ReferenceSet[] teste = workPart.GetAllReferenceSets();

            foreach (var item in teste)
            {

                if (item.Name ==referenceset)
                {
                    referenceSet1 = item;
                }

            }
            NXObject[] components1 = workPart.ComponentAssembly.RootComponent.GetChildren();

            int ii = 1;
            foreach (var item in components1)
            {

                if (item.Name.Contains(componente))
                {
                    ii++;
                }
            }
            NXObject[] components_remove = new NXObject[ii - 1];

            int jj = 0;
            foreach (var item in components1)
            {
                if (item.Name.Contains(componente))
                {
                    components_remove[jj] = item;
                    jj++;
                }
            }

            referenceSet1.RemoveObjectsFromReferenceSet(components_remove);

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId3);
        }
        public static void recorte_perfil(string perfil_recorte, Part workPart)
        {


            // ----------------------------------------------
            //   Menu: Insert->Design Feature->Extrude...
            // ----------------------------------------------
            Session theSession = Session.GetSession();
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.Features.Feature nullFeatures_Feature = null;

            if (!workPart.Preferences.Modeling.GetHistoryMode())
            {
                throw new Exception("Create or edit of a Feature was recorded in History Mode but playback is in History-Free Mode.");
            }

            NXOpen.Features.ExtrudeBuilder extrudeBuilder1;
            extrudeBuilder1 = workPart.Features.CreateExtrudeBuilder(nullFeatures_Feature);

            Section section1;
            section1 = workPart.Sections.CreateSection(0.0095, 0.01, 0.5);

            extrudeBuilder1.Section = section1;

            extrudeBuilder1.AllowSelfIntersectingSection(true);

            Unit unit1;
            unit1 = extrudeBuilder1.Draft.FrontDraftAngle.Units;

            Expression expression1;
            expression1 = workPart.Expressions.CreateSystemExpressionWithUnits("2.00", unit1);

            extrudeBuilder1.DistanceTolerance = 0.01;

            extrudeBuilder1.BooleanOperation.Type = NXOpen.GeometricUtilities.BooleanOperation.BooleanType.Create;

            Body[] targetBodies1 = new Body[1];
            Body nullBody = null;
            targetBodies1[0] = nullBody;
            extrudeBuilder1.BooleanOperation.SetTargetBodies(targetBodies1);

            extrudeBuilder1.Limits.StartExtend.Value.RightHandSide = "0";

            extrudeBuilder1.Limits.EndExtend.Value.RightHandSide = "5859.0288673269";

            extrudeBuilder1.BooleanOperation.Type = NXOpen.GeometricUtilities.BooleanOperation.BooleanType.Subtract;

            Body[] targetBodies2 = new Body[1];
            targetBodies2[0] = nullBody;
            extrudeBuilder1.BooleanOperation.SetTargetBodies(targetBodies2);

            extrudeBuilder1.Draft.FrontDraftAngle.RightHandSide = "2";

            extrudeBuilder1.Draft.BackDraftAngle.RightHandSide = "2";

            extrudeBuilder1.Offset.StartOffset.RightHandSide = "0";

            extrudeBuilder1.Offset.EndOffset.RightHandSide = "5";

            NXOpen.GeometricUtilities.SmartVolumeProfileBuilder smartVolumeProfileBuilder1;
            smartVolumeProfileBuilder1 = extrudeBuilder1.SmartVolumeProfile;

            smartVolumeProfileBuilder1.OpenProfileSmartVolumeOption = false;

            smartVolumeProfileBuilder1.CloseProfileRule = NXOpen.GeometricUtilities.SmartVolumeProfileBuilder.CloseProfileRuleType.Fci;

            theSession.SetUndoMarkName(markId1, "Extrude Dialog");

            section1.DistanceTolerance = 0.01;

            section1.ChainingTolerance = 0.0095;

            section1.SetAllowedEntityTypes(NXOpen.Section.AllowTypes.OnlyCurves);

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "section mark");

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, null);

            NXOpen.Features.Feature[] features1 = new NXOpen.Features.Feature[1];
            NXOpen.Features.SketchFeature sketchFeature1 = (NXOpen.Features.SketchFeature)workPart.Features.FindObject("SKETCH(4)");
            features1[0] = sketchFeature1;
            CurveFeatureRule curveFeatureRule1;
            curveFeatureRule1 = workPart.ScRuleFactory.CreateRuleCurveFeature(features1);

            section1.AllowSelfIntersection(true);

            SelectionIntentRule[] rules1 = new SelectionIntentRule[1];
            rules1[0] = curveFeatureRule1;
            NXObject nullNXObject = null;
            Point3d helpPoint1 = new Point3d(0.0, 0.0, 0.0);
            section1.AddToSection(rules1, nullNXObject, nullNXObject, nullNXObject, helpPoint1, NXOpen.Section.Mode.Create, false);

            theSession.DeleteUndoMark(markId3, null);

            Sketch sketch1 = (Sketch)workPart.Sketches.FindObject("SKETCH_000");
            Direction direction1;
            direction1 = workPart.Directions.CreateDirection(sketch1, Sense.Forward, NXOpen.SmartObject.UpdateOption.WithinModeling);

            extrudeBuilder1.Direction = direction1;

            theSession.DeleteUndoMark(markId2, null);

            extrudeBuilder1.BooleanOperation.Type = NXOpen.GeometricUtilities.BooleanOperation.BooleanType.Subtract;

            Body[] targetBodies3 = new Body[1];
            Body body1 = (Body)workPart.Bodies.FindObject(perfil_recorte);
            targetBodies3[0] = body1;
            extrudeBuilder1.BooleanOperation.SetTargetBodies(targetBodies3);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Extrude");

            theSession.DeleteUndoMark(markId4, null);

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Extrude");

            extrudeBuilder1.ParentFeatureInternal = false;

            NXOpen.Features.Feature feature1;
            feature1 = extrudeBuilder1.CommitFeature();

            theSession.DeleteUndoMark(markId5, null);

            theSession.SetUndoMarkName(markId1, "Extrude");

            Expression expression2 = extrudeBuilder1.Limits.StartExtend.Value;
            Expression expression3 = extrudeBuilder1.Limits.EndExtend.Value;
            extrudeBuilder1.Destroy();

            workPart.Expressions.Delete(expression1);

        }
        public static void copiar_item_em_x(double pos, Part workPart, NXOpen.Assemblies.Component comp, bool restringir, int i, string lado)
        {

            Session theSession = Session.GetSession();

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.Positioning.ComponentPositioner componentPositioner1;
            componentPositioner1 = workPart.ComponentAssembly.Positioner;

            componentPositioner1.ClearNetwork();

            NXOpen.Assemblies.Arrangement arrangement1 = (NXOpen.Assemblies.Arrangement)workPart.ComponentAssembly.Arrangements.FindObject("Arrangement 1");
            componentPositioner1.PrimaryArrangement = arrangement1;

            componentPositioner1.BeginMoveComponent();

            bool allowInterpartPositioning1;
            allowInterpartPositioning1 = theSession.Preferences.Assemblies.InterpartPositioning;

            NXOpen.Positioning.Network network1;
            network1 = componentPositioner1.EstablishNetwork();

            NXOpen.Positioning.ComponentNetwork componentNetwork1 = (NXOpen.Positioning.ComponentNetwork)network1;
            componentNetwork1.MoveObjectsState = true;

            NXOpen.Assemblies.Component nullAssemblies_Component = null;
            componentNetwork1.DisplayComponent = nullAssemblies_Component;

            componentNetwork1.NetworkArrangementsMode = NXOpen.Positioning.ComponentNetwork.ArrangementsMode.Existing;

            componentNetwork1.RemoveAllConstraints();

            NXObject[] movableObjects1 = new NXObject[1];
            NXOpen.Assemblies.Component component1 = comp;//(NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT "+codigo+"/003 1");
            movableObjects1[0] = component1;
            componentNetwork1.SetMovingGroup(movableObjects1);

            componentNetwork1.Solve();

            theSession.SetUndoMarkName(markId1, "Move Component Dialog");

            componentNetwork1.MoveObjectsState = true;

            componentNetwork1.NetworkArrangementsMode = NXOpen.Positioning.ComponentNetwork.ArrangementsMode.Existing;

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Move Component Update");

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Translate Along Y-axis");

            bool loaded1;
            loaded1 = componentNetwork1.IsReferencedGeometryLoaded();

            componentNetwork1.BeginDrag();

            componentNetwork1.EndDrag();

            componentNetwork1.ResetDisplay();

            componentNetwork1.ApplyToModel();

            componentNetwork1.Solve();

            componentPositioner1.ClearNetwork();

            int nErrs1;
            nErrs1 = theSession.UpdateManager.AddToDeleteList(componentNetwork1);

            

            NXOpen.Assemblies.Component[] components1 = new NXOpen.Assemblies.Component[1];
            components1[0] = component1;
            NXOpen.Assemblies.Component[] newComponents1;
            newComponents1 = workPart.ComponentAssembly.CopyComponents(components1);

            NXOpen.Positioning.Network network2;
            network2 = componentPositioner1.EstablishNetwork();

            NXOpen.Positioning.ComponentNetwork componentNetwork2 = (NXOpen.Positioning.ComponentNetwork)network2;
            componentNetwork2.MoveObjectsState = true;

            componentNetwork2.DisplayComponent = nullAssemblies_Component;

            componentNetwork2.NetworkArrangementsMode = NXOpen.Positioning.ComponentNetwork.ArrangementsMode.Existing;

            componentNetwork2.RemoveAllConstraints();

            NXObject[] movableObjects2 = new NXObject[1];
            movableObjects2[0] = component1;
            componentNetwork2.SetMovingGroup(movableObjects2);

            componentNetwork2.Solve();

            componentNetwork2.RemoveAllConstraints();

            NXObject[] movableObjects3 = new NXObject[1];
            movableObjects3[0] = newComponents1[0];
            componentNetwork2.SetMovingGroup(movableObjects3);

            componentNetwork2.Solve();

            bool loaded2;
            loaded2 = componentNetwork2.IsReferencedGeometryLoaded();

            componentNetwork2.BeginDrag();

            Vector3d translation1 = new Vector3d(0.0, pos, 0.0);
            componentNetwork2.DragByTranslation(translation1);

            componentNetwork2.EndDrag();

            componentNetwork2.ResetDisplay();

            componentNetwork2.ApplyToModel();

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Move Component");

            theSession.DeleteUndoMark(markId4, null);

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Move Component");

            componentNetwork2.Solve();

            componentPositioner1.ClearNetwork();

            int nErrs3;
            nErrs3 = theSession.UpdateManager.AddToDeleteList(componentNetwork2);

            int nErrs4;
            nErrs4 = theSession.UpdateManager.DoUpdate(markId2);

            componentPositioner1.DeleteNonPersistentConstraints();

            int nErrs5;
            nErrs5 = theSession.UpdateManager.DoUpdate(markId2);

            theSession.DeleteUndoMark(markId5, null);

            theSession.SetUndoMarkName(markId1, "Move Component");

            componentPositioner1.EndMoveComponent();

            NXOpen.Assemblies.Arrangement nullAssemblies_Arrangement = null;
            componentPositioner1.PrimaryArrangement = nullAssemblies_Arrangement;

            theSession.DeleteUndoMark(markId2, null);

            theSession.DeleteUndoMark(markId3, null);

            if (restringir == true)
            {
                //NX//Open.Positioning.ComponentPositioner componentPositioner1;
                componentPositioner1 = workPart.ComponentAssembly.Positioner;

                NXOpen.Assemblies.Arrangement arrangement12 = ((NXOpen.Assemblies.Arrangement)workPart.ComponentAssembly.Arrangements.FindObject("Arrangement 1"));
                componentPositioner1.PrimaryArrangement = arrangement12;


                NXOpen.Positioning.Constraint constraint1;
                constraint1 = componentPositioner1.CreateConstraint(true);
                NXOpen.Positioning.ComponentConstraint componentConstraint1 = ((NXOpen.Positioning.ComponentConstraint)constraint1);
                componentConstraint1.ConstraintType = NXOpen.Positioning.Constraint.Type.Distance;
                NXOpen.DatumPlane datumPlane1 = ((NXOpen.DatumPlane)workPart.Datums.FindObject("DATUM_CSYS(0) XY plane"));
                NXOpen.Positioning.ConstraintReference constraintReference1;
                constraintReference1 = componentConstraint1.CreateConstraintReference(workPart.ComponentAssembly, datumPlane1, false, false, false);

                NXOpen.Assemblies.Component component2 = ((NXOpen.Assemblies.Component)newComponents1[0].FindObject("COMPONENT 295186/007 1"));
                NXOpen.Face face1 = ((NXOpen.Face)component2.FindObject("PARTIAL_PROTO#.Bodies|Body8|HANDLE R-12401"));

                NXOpen.Positioning.ConstraintReference constraintReference2;
                constraintReference2 = componentConstraint1.CreateConstraintReference(newComponents1[0], face1, false, false, false);



                constraintReference2.SetFixHint(true);

                componentConstraint1.SetExpression("0");

                componentConstraint1.SetExpression("v_pos_"+lado+"_sup" + (i + 1).ToString());
            }

           

        }
        private void referenset_reguas_led()
        {
            Session theSession;
            theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;

            NXOpen.ReferenceSet referenceSet1 = null;

            foreach (NXOpen.ReferenceSet referenceSet in workPart.GetAllReferenceSets())
            {
                if (referenceSet.Name == "ILUMINACAO")
                {
                    referenceSet1 = referenceSet;
                    //MessageBox.Show(referenceSet.Name);
                }
            }

            List<NXOpen.Assemblies.Component> leds = busca_componente_led("REGUA_LED");

            NXOpen.NXObject[] components2 = new NXOpen.NXObject[leds.Count];
            int i = 0;
            foreach (NXOpen.Assemblies.Component item in leds)
            {
                components2[i] = item;
                i++;

            }
            referenceSet1.AddObjectsToReferenceSet(components2);

        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            Session theSession;
            theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;

            //NXOpen.ReferenceSet referenceSet1 = null;

            //foreach (NXOpen.ReferenceSet referenceSet in workPart.GetAllReferenceSets())
            //{
            //    if (referenceSet.Name == "ILUMINACAO")
            //    {
            //        referenceSet1 = referenceSet;
            //        //MessageBox.Show(referenceSet.Name);
            //    }
            //}
           
            //List<NXOpen.Assemblies.Component> leds = busca_componente_led("REGUA_LED");

            //NXOpen.NXObject[] components2 = new NXOpen.NXObject[leds.Count];
            //int i = 0;
            //foreach (NXOpen.Assemblies.Component item in leds)
            //{
            //    components2[i] = item;
            //    i++;

            //}
            //referenceSet1.AddObjectsToReferenceSet(components2);

            ////theSession.Preferences.Modeling.UpdatePending = false;

            ////NXOpen.Session.UndoMarkId markId1;
            ////markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Expression");

            ////bool found1;
            ////found1 = workPart.Expressions.RemoveInterpartReferences("TPL_PORTA_PACOTES_LUXO_2024/000");

            ////theSession.Preferences.Modeling.UpdatePending = false;

            ////int nErrs1;
            ////nErrs1 = theSession.UpdateManager.DoUpdate(markId1);

            //double pos = 1500;
            //for (int i = 1; i <= 18; i++)
            //{

            //    preenche_expression("v_f_le" + i.ToString(), "0", workPart);
            //    preenche_expression("v_f_ld" + i.ToString(), "0", workPart);
            //    pos = pos + 500;
            //}
            //NXOpen.Session.UndoMarkId markId1;
            //markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            //NXOpen.Features.Feature nullFeatures_Feature = null;

            //if (!workPart.Preferences.Modeling.GetHistoryMode())
            //{
            //    throw new Exception("Create or edit of a Feature was recorded in History Mode but playback is in History-Free Mode.");
            //}
            //theSession.Preferences.Modeling.UpdatePending = false;


            for (int i = 1; i <= 18; i++)
            {
           
            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            Expression expression1;
            expression1 = workPart.Expressions.CreateWithUnits("v_polt_ld"+i.ToString() + "=" + "0", unit1);
            Expression expression2;
            expression2 = workPart.Expressions.CreateWithUnits("v_polt_le" + i.ToString() + "=" + "0", unit1);
            }

            //theSession.Preferences.Modeling.UpdatePending = false;

            //int nErrs1;
            //nErrs1 = theSession.UpdateManager.DoUpdate(markId1);
            //NXOpen.Features.PatternFeatureBuilder patternFeatureBuilder1;
            //patternFeatureBuilder1 = workPart.Features.CreatePatternFeatureBuilder(nullFeatures_Feature);

            //Unit unit1;
            //unit1 = patternFeatureBuilder1.PatternService.SpiralDefinition.RadialPitch.Units;

            //Expression expression1;
            //expression1 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            //Expression expression2;
            //expression2 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            //Point3d origin1 = new Point3d(0.0, 0.0, 0.0);
            //Vector3d normal1 = new Vector3d(0.0, 0.0, 1.0);
            //Plane plane1;
            //plane1 = workPart.Planes.CreatePlane(origin1, normal1, NXOpen.SmartObject.UpdateOption.WithinModeling);

            //patternFeatureBuilder1.PatternService.MirrorDefinition.NewPlane = plane1;

            //Expression expression3;
            //expression3 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            //Expression expression4;
            //expression4 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            //Expression expression5;
            //expression5 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            //patternFeatureBuilder1.PatternService.PatternFill.FillMargin.RightHandSide = "0";

            //patternFeatureBuilder1.PatternService.PatternOrientation.CircularOrientationOption = NXOpen.GeometricUtilities.PatternOrientation.Enum.FollowPattern;

            //patternFeatureBuilder1.PatternService.PatternOrientation.AlongOrientationOption = NXOpen.GeometricUtilities.PatternOrientation.Enum.NormalToPath;

            //patternFeatureBuilder1.PatternService.PatternOrientation.PolygonOrientationOption = NXOpen.GeometricUtilities.PatternOrientation.Enum.FollowPattern;

            //patternFeatureBuilder1.PatternService.PatternOrientation.SpiralOrientationOption = NXOpen.GeometricUtilities.PatternOrientation.Enum.FollowPattern;

            //patternFeatureBuilder1.PatternService.PatternOrientation.MirrorOrientationOption = NXOpen.GeometricUtilities.PatternOrientation.Enum.FollowPattern;

            //patternFeatureBuilder1.PatternService.PatternOrientation.HelixOrientationOption = NXOpen.GeometricUtilities.PatternOrientation.Enum.FollowPattern;

            //patternFeatureBuilder1.PatternService.PatternOrientation.AlongPathRotationAngle.RightHandSide = "0";

            //patternFeatureBuilder1.PatternService.RectangularDefinition.XSpacing.NCopies.RightHandSide = "2";

            //patternFeatureBuilder1.PatternService.RectangularDefinition.XSpacing.PitchDistance.RightHandSide = "v_f_ld3";

            //patternFeatureBuilder1.PatternService.RectangularDefinition.XSpacing.SpanDistance.RightHandSide = "100";

            //patternFeatureBuilder1.PatternService.RectangularDefinition.YSpacing.NCopies.RightHandSide = "1";

            //patternFeatureBuilder1.PatternService.RectangularDefinition.YSpacing.PitchDistance.RightHandSide = "10";

            //patternFeatureBuilder1.PatternService.RectangularDefinition.YSpacing.SpanDistance.RightHandSide = "100";

            //patternFeatureBuilder1.PatternService.RectangularDefinition.HorizontalRef.RotationAngle.RightHandSide = "0";

            //patternFeatureBuilder1.PatternService.CircularDefinition.AngularSpacing.NCopies.RightHandSide = "12";

            //patternFeatureBuilder1.PatternService.CircularDefinition.AngularSpacing.PitchDistance.RightHandSide = "10";

            //patternFeatureBuilder1.PatternService.CircularDefinition.AngularSpacing.PitchAngle.RightHandSide = "10";

            //patternFeatureBuilder1.PatternService.CircularDefinition.AngularSpacing.SpanAngle.RightHandSide = "360";

            //patternFeatureBuilder1.PatternService.CircularDefinition.RadialSpacing.NCopies.RightHandSide = "1";

            //patternFeatureBuilder1.PatternService.CircularDefinition.RadialSpacing.PitchDistance.RightHandSide = "10";

            //patternFeatureBuilder1.PatternService.CircularDefinition.RadialSpacing.SpanDistance.RightHandSide = "100";

            //patternFeatureBuilder1.PatternService.CircularDefinition.HorizontalRef.RotationAngle.RightHandSide = "0";

            //patternFeatureBuilder1.PatternService.AlongPathDefinition.XPathOption = NXOpen.GeometricUtilities.AlongPathPattern.PathOptions.Offset;

            //patternFeatureBuilder1.PatternService.AlongPathDefinition.XOnPathSpacing.NCopies.RightHandSide = "2";

            //patternFeatureBuilder1.PatternService.AlongPathDefinition.XOnPathSpacing.OnPathPitchDistance.Expression.RightHandSide = "50";

            //patternFeatureBuilder1.PatternService.AlongPathDefinition.XOnPathSpacing.OnPathSpanDistance.Expression.RightHandSide = "100";

            //patternFeatureBuilder1.PatternService.AlongPathDefinition.YDirectionOption = NXOpen.GeometricUtilities.AlongPathPattern.YDirectionOptions.Section;

            //patternFeatureBuilder1.PatternService.AlongPathDefinition.YPathOption = NXOpen.GeometricUtilities.AlongPathPattern.PathOptions.Offset;

            //patternFeatureBuilder1.PatternService.AlongPathDefinition.YOnPathSpacing.NCopies.RightHandSide = "1";

            //patternFeatureBuilder1.PatternService.AlongPathDefinition.YOnPathSpacing.OnPathPitchDistance.Expression.RightHandSide = "50";

            //patternFeatureBuilder1.PatternService.AlongPathDefinition.YOnPathSpacing.OnPathSpanDistance.Expression.RightHandSide = "100";

            //patternFeatureBuilder1.PatternService.AlongPathDefinition.YSpacing.NCopies.RightHandSide = "1";

            //patternFeatureBuilder1.PatternService.AlongPathDefinition.YSpacing.PitchDistance.RightHandSide = "10";

            //patternFeatureBuilder1.PatternService.AlongPathDefinition.YSpacing.SpanDistance.RightHandSide = "100";

            //patternFeatureBuilder1.PatternService.SpiralDefinition.NumberOfTurns.RightHandSide = "1";

            //patternFeatureBuilder1.PatternService.SpiralDefinition.TotalAngle.RightHandSide = "360";

            //patternFeatureBuilder1.PatternService.SpiralDefinition.RadialPitch.RightHandSide = "50";

            //patternFeatureBuilder1.PatternService.SpiralDefinition.PitchAlongSpiral.NCopies.RightHandSide = "2";

            //patternFeatureBuilder1.PatternService.SpiralDefinition.PitchAlongSpiral.OnPathPitchDistance.Expression.RightHandSide = "50";

            //patternFeatureBuilder1.PatternService.SpiralDefinition.PitchAlongSpiral.OnPathSpanDistance.Expression.RightHandSide = "100";

            //patternFeatureBuilder1.PatternService.SpiralDefinition.HorizontalRef.RotationAngle.RightHandSide = "0";

            //patternFeatureBuilder1.PatternService.PolygonDefinition.PolygonSpacing.NCopies.RightHandSide = "4";

            //patternFeatureBuilder1.PatternService.PolygonDefinition.PolygonSpacing.PitchDistance.RightHandSide = "25";

            //patternFeatureBuilder1.PatternService.PolygonDefinition.PolygonSpacing.SpanAngle.RightHandSide = "360";

            //patternFeatureBuilder1.PatternService.PolygonDefinition.NumberOfSides.RightHandSide = "6";

            //patternFeatureBuilder1.PatternService.PolygonDefinition.RadialSpacing.NCopies.RightHandSide = "1";

            //patternFeatureBuilder1.PatternService.PolygonDefinition.RadialSpacing.PitchDistance.RightHandSide = "10";

            //patternFeatureBuilder1.PatternService.PolygonDefinition.RadialSpacing.SpanDistance.RightHandSide = "100";

            //patternFeatureBuilder1.PatternService.PolygonDefinition.HorizontalRef.RotationAngle.RightHandSide = "0";

            //patternFeatureBuilder1.PatternService.HelixDefinition.CountOfInstances.RightHandSide = "6";

            //patternFeatureBuilder1.PatternService.HelixDefinition.NumberOfTurns.RightHandSide = "2";

            //patternFeatureBuilder1.PatternService.HelixDefinition.AnglePitch.RightHandSide = "30";

            //patternFeatureBuilder1.PatternService.HelixDefinition.DistancePitch.RightHandSide = "10";

            //patternFeatureBuilder1.PatternService.HelixDefinition.HelixPitch.RightHandSide = "50";

            //patternFeatureBuilder1.PatternService.HelixDefinition.HelixSpan.RightHandSide = "100";

            //patternFeatureBuilder1.PatternMethod = NXOpen.Features.PatternFeatureBuilder.PatternMethodOptions.Variational;

            //theSession.SetUndoMarkName(markId1, "Pattern Feature Dialog");

            //Point3d origin2 = new Point3d(0.0, 0.0, 0.0);
            //Vector3d vector1 = new Vector3d(1.0, 0.0, 0.0);
            //Direction direction1;
            //direction1 = workPart.Directions.CreateDirection(origin2, vector1, NXOpen.SmartObject.UpdateOption.WithinModeling);

            //patternFeatureBuilder1.PatternService.RectangularDefinition.HorizontalRef.HorizontalRefVector = direction1;

            //Expression expression6;
            //expression6 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            //NXOpen.Features.SketchFeature sketchFeature1 = (NXOpen.Features.SketchFeature)workPart.Features.FindObject("SKETCH(54)");
            //bool added1;
            //added1 = patternFeatureBuilder1.FeatureList.Add(sketchFeature1);

            //// Reference Point (-770.671604, 1400.000000, 0.000000) inferred from selected features.
            //Point3d coordinates1 = new Point3d(835.0, -3.30291349825984e-015, 0.0);
            //Point point1;
            //point1 = workPart.Points.CreatePoint(coordinates1);

            //patternFeatureBuilder1.ReferencePointService.Point = point1;

            //patternFeatureBuilder1.PatternService.RectangularDefinition.XSpacing.PitchDistance.RightHandSide = "2500";

            //Expression expression7;
            //expression7 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            //Point3d origin3 = new Point3d(0.0, 0.0, 0.0);
            //Vector3d vector2 = new Vector3d(0.0, 1.0, 0.0);
            //Direction direction2;
            //direction2 = workPart.Directions.CreateDirection(origin3, vector2, NXOpen.SmartObject.UpdateOption.WithinModeling);

            //patternFeatureBuilder1.PatternService.RectangularDefinition.XDirection = direction2;

            //Point3d scaleAboutPoint1 = new Point3d(53.0715367868465, -179.017422594436, 0.0);
            //Point3d viewCenter1 = new Point3d(-53.071536786846, 179.017422594437, 0.0);
            //workPart.ModelingViews.WorkView.ZoomAboutPoint(0.8, scaleAboutPoint1, viewCenter1);

            //Point3d scaleAboutPoint2 = new Point3d(68.3197022069475, -225.752059466435, 0.0);
            //Point3d viewCenter2 = new Point3d(-68.3197022069472, 225.752059466435, 0.0);
            //workPart.ModelingViews.WorkView.ZoomAboutPoint(0.8, scaleAboutPoint2, viewCenter2);

            //patternFeatureBuilder1.PatternService.RectangularDefinition.XSpacing.PitchDistance.RightHandSide = "v_f_le"+i.ToString();

            //NXOpen.Session.UndoMarkId markId2;
            //markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Pattern Feature");

            //theSession.DeleteUndoMark(markId2, null);

            //NXOpen.Session.UndoMarkId markId3;
            //markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Pattern Feature");

            //patternFeatureBuilder1.ParentFeatureInternal = false;

            //NXObject nXObject1;
            //nXObject1 = patternFeatureBuilder1.Commit();

            //theSession.DeleteUndoMark(markId3, null);

            //theSession.SetUndoMarkName(markId1, "Pattern Feature");

            //Expression expression8 = patternFeatureBuilder1.PatternService.RectangularDefinition.YSpacing.NCopies;
            //Expression expression9 = patternFeatureBuilder1.PatternService.RectangularDefinition.YSpacing.SpanDistance;
            //Expression expression10 = patternFeatureBuilder1.PatternService.RectangularDefinition.YSpacing.PitchDistance;
            //Expression expression11 = patternFeatureBuilder1.PatternService.RectangularDefinition.XSpacing.NCopies;
            //Expression expression12 = patternFeatureBuilder1.PatternService.RectangularDefinition.XSpacing.SpanDistance;
            //Expression expression13 = patternFeatureBuilder1.PatternService.RectangularDefinition.XSpacing.PitchDistance;
            //Expression expression14 = patternFeatureBuilder1.PatternService.RectangularDefinition.HorizontalRef.RotationAngle;
            //patternFeatureBuilder1.Destroy();

            //try
            //{
            //    // Expression is still in use.
            //    workPart.Expressions.Delete(expression4);
            //}
            //catch (NXException ex)
            //{
            //    ex.AssertErrorCode(1050029);
            //}

            //workPart.Expressions.Delete(expression7);

            //workPart.Expressions.Delete(expression1);

            //workPart.Expressions.Delete(expression2);

            //try
            //{
            //    // Expression is still in use.
            //    workPart.Expressions.Delete(expression3);
            //}
            //catch (NXException ex)
            //{
            //    ex.AssertErrorCode(1050029);
            //}

            //workPart.Expressions.Delete(expression5);

            //workPart.Expressions.Delete(expression6);
            //}

            //double pos = 1500;
            //for (int i = 1; i <= 18; i++)
            //{
            //    preenche_expression("v_f_ld" + i.ToString(), pos.ToString(), workPart);
            //    pos =pos+ 500;
            //}
            //pos = 1500;
            //for (int i = 1; i <= 18; i++)
            //{
            //    preenche_expression("v_f_le" + i.ToString(), pos.ToString(), workPart);
            //    pos = pos + 500;
            //}





        }

        private void label54_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            //MessageBox.Show(workPart.Name);
            if (workPart.Name != "TPL_PORTA_PACOTES_LUXO_2024/000;1")
            {


                NXOpen.Session.UndoMarkId markId1;
                markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

                NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_PORTA_PACOTES_LUXO_2024/000 1");
                PartLoadStatus partLoadStatus1;
                theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

                workPart = theSession.Parts.Work;
                partLoadStatus1.Dispose();
                theSession.SetUndoMarkName(markId1, "Make Work Part");
            }

            // Selecionar múltiplos componentes
            (string exp_name, double exp_value) = MySelect.SelectMultipleComponents_edit("Selecione o Porta Foco que deseja editar a posicão ", Selection.SelectionScope.WorkPartAndOccurrence, workPart);
            
            frm_edit_pos_compcs frm_Edit_Pos_Compcs = new frm_edit_pos_compcs(exp_name, exp_value, workPart);
            frm_Edit_Pos_Compcs.StartPosition = FormStartPosition.CenterParent;
            frm_Edit_Pos_Compcs.ShowDialog();
           
        }
    }
}
