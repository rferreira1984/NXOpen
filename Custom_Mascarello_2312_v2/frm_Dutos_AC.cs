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
using NXOpen;
using NXOpen.UF;
using NXOpen.Assemblies;
using System.Collections;
using System.Xml.Linq;

namespace Custom_Mascarello
{
    public partial class frm_Dutos_AC : Form
    {
        int n_sel = 0;
        int n_ps_sel = 0;
        int n_af = 4;
        int n_ps = 4;

        double final_duto = 350;
        double inicio_duto = 1860;

        double pos_manutencao_LD = 0.0;
        double pos_manutencao_LE = 0.0;

        public frm_Dutos_AC()
        {
            InitializeComponent();
            panel_select.Enabled = false;
            txtbox_pc_grvia.Visible = false;
            txtbox_pt_grvia.Visible = false;

            cota_1_pc_grvia.Visible = false;
            cota_2_pc_grvia.Visible = false;
            cota_3_pc_grvia.Visible = false;
            cota_EE_grvia.Visible = false;
            cota_1_pt_grvia.Visible = false;
            cota_2_pt_grvia.Visible = false;
            cota_3_pt_grvia.Visible = false;

            lbl_pc.Visible = false;
            lbl_pt.Visible = false;
        }

        private void frm_Dutos_AC_Load(object sender, EventArgs e)
        {
            
        }

            private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btn_import_Click(object sender, EventArgs e)

        {
            picture_ld_grvia.Image = Properties.Resources.VISTA_LD_GRVIA;
            txtbox_pc_grvia.Visible = false;
            txtbox_pt_grvia.Visible = false;

            cota_1_pc_grvia.Visible = false;
            cota_2_pc_grvia.Visible = false;
            cota_3_pc_grvia.Visible = false;
            cota_EE_grvia.Visible = false;
            cota_1_pt_grvia.Visible = false;
            cota_2_pt_grvia.Visible = false;
            cota_3_pt_grvia.Visible = false;

            cmbbox_pc.Text = "1.3.1 - Nao ";
            cmbox_PT.Text = "1.4.1 - Nao ";

            lbl_pc.Visible = false;
            lbl_pt.Visible = false;

            string chassi = "";
            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://192.168.0.199:86/webservicesimples/pegatarefapedido.php?id=" + txt_fm.Text);

            XmlNodeList Lista_Postos = default(XmlNodeList);
            Lista_Postos = doc.SelectNodes(" /webpedidotarefa");

            List<string> lista_tarefas = new List<string> { "PORTA_DIANTEIRA", "PORTA_CENTRAL", "PORTA_TRASEIRA", "CHASSI", "COMPRIMENTO","ENTREEIXO", "CARROCERIA"};

            foreach (XmlNode posto in Lista_Postos)
            {
                int cont = posto.ChildNodes.Count;
                for (int i = 0; i <= cont - 1; i++)
                {
                    if (lista_tarefas.Contains(posto.ChildNodes.Item(i).Name))
                    {
                       if(posto.ChildNodes.Item(i).Name == "CHASSI")
                        {
                            lbl_result_chassi.Text = posto.ChildNodes.Item(i).InnerText;
                        }
                        if (posto.ChildNodes.Item(i).Name == "PORTA_DIANTEIRA")
                        {
                          cmbox_pd.Text = posto.ChildNodes.Item(i).InnerText;
                        }
                        if (posto.ChildNodes.Item(i).Name == "PORTA_CENTRAL")
                        {
                            cmbbox_pc.Text = posto.ChildNodes.Item(i).InnerText;
                        }
                        if (posto.ChildNodes.Item(i).Name == "PORTA_TRASEIRA")
                        {
                            cmbox_PT.Text = posto.ChildNodes.Item(i).InnerText;
                        }
                        if (posto.ChildNodes.Item(i).Name == "COMPRIMENTO")
                        {
                            txt_comp_grvia.Text = posto.ChildNodes.Item(i).InnerText;
                        }
                        if (posto.ChildNodes.Item(i).Name == "ENTREEIXO")
                        {
                          txt_ee_grvia.Text = posto.ChildNodes.Item(i).InnerText;
                        }
                        if (posto.ChildNodes.Item(i).Name == "CARROCERIA")
                        {
                            lbl_carroceria.Text = posto.ChildNodes.Item(i).InnerText;
                        } 
                    }
                }

            }
            //path
            //GRAN VIA
            string path = "";
            string node = "";
            if (lbl_carroceria.Text == "GRAN VIA" || lbl_carroceria.Text == "GRAN VIA MIDI"|| lbl_carroceria.Text == "GRAN MIDI")
            {
                path = @"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\NX\URBANO\GRAN_VIA\AC\xml_dados_AC.xml";

                node = "/Dados_GranVia/Dados";

                if (cmbbox_pc.Text != "1.3.1 - Nao " && cmbox_PT.Text == "1.4.1 - Nao ")
                {
                    picture_ld_grvia.Image = Properties.Resources.VISTA_LD_PD_PC_GRVIA;
                    txtbox_pc_grvia.Visible = true;

                    cota_1_pc_grvia.Visible = true;
                    cota_2_pc_grvia.Visible = true;
                    cota_3_pc_grvia.Visible = true;

                    lbl_pc.Visible = true;

                }
                if (cmbbox_pc.Text != "1.3.1 - Nao " && cmbox_PT.Text != "1.4.1 - Nao ")
                {
                    picture_ld_grvia.Image = Properties.Resources.VISTA_LD_PD_PC_PT_GRVIA;
                    txtbox_pc_grvia.Visible = true;
                    txtbox_pt_grvia.Visible = true;

                    cota_1_pc_grvia.Visible = true;
                    cota_2_pc_grvia.Visible = true;
                    cota_3_pc_grvia.Visible = true;
                    cota_EE_grvia.Visible = true;
                    cota_1_pt_grvia.Visible = true;
                    cota_2_pt_grvia.Visible = true;
                    cota_3_pt_grvia.Visible = true;

                    lbl_pc.Visible = true;
                    lbl_pt.Visible = true;

                }
                if (cmbbox_pc.Text == "1.3.1 - Nao " && cmbox_PT.Text != "1.4.1 - Nao ") 
                {
                    picture_ld_grvia.Image = Properties.Resources.VISTA_LD_PD_PT_GRVIA;
                    txtbox_pt_grvia.Visible = true;
                    cota_EE_grvia.Visible = true;
                    cota_1_pt_grvia.Visible = true;
                    cota_2_pt_grvia.Visible = true;
                    cota_3_pt_grvia.Visible = true;

                    lbl_pt.Visible = true;
                }
            }

            XmlDocument xmlBiblioteca = new XmlDocument();
            xmlBiblioteca.Load(path);
            //  XElement xml = XElement.Load(@"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\NX\MICRO_S3\xml_dados_S3.xml");

            XmlNodeList Lista_Opçoes = default(XmlNodeList);

            Lista_Opçoes = xmlBiblioteca.SelectNodes(node);
            foreach (XmlNode opcao in Lista_Opçoes)
            {
                if (opcao.ChildNodes.Item(0).InnerText == lbl_result_chassi.Text)
                {
                    txt_bd_grvia.Text = opcao.ChildNodes.Item(1).InnerText;
                   
                }
            } 
        }

        public void aplicar_configuracao_tpl_acab()
        {


            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_EM_DUTOS_AC_INJETADO_GRV/000 1");
            Part part1 = (Part)theSession.Parts.FindObject("@DB/TPL_EM_DUTOS_AC_INJETADO_GRV/000");

            PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

            workPart = theSession.Parts.Work;
            partLoadStatus1.Dispose();

            theSession.Preferences.Modeling.UpdatePending = false;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Session");

            


            try
            {
                Expression expression1 = (Expression)workPart.Expressions.FindObject("COMP");
                Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

                {
                    workPart.Expressions.EditWithUnits(expression1, unit1, txt_comp_grvia.Text); 
                }

            }
            catch
            {
            }
            try
            {
                Expression expression2 = (Expression)workPart.Expressions.FindObject("POS_AC");
                Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

                {
                    workPart.Expressions.EditWithUnits(expression2, unit1, txt_pos_ac_grvia.Text);
                }

            }
            catch
            {
            }
            try
            {
                Expression expression3 = (Expression)workPart.Expressions.FindObject("POS_PORTA_CENTRAL");
                Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

                {
                    workPart.Expressions.EditWithUnits(expression3, unit1, txtbox_pc_grvia.Text);
                }

            }
            catch
            {
            }
            try
            {
                Expression expression3 = (Expression)workPart.Expressions.FindObject("POS_PORTA_TRASEIRA");
                Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

                {
                    workPart.Expressions.EditWithUnits(expression3, unit1, txtbox_pt_grvia.Text); 
                }

            }
            catch
            {
            }

            try
            {
                Expression expression4 = (Expression)workPart.Expressions.FindObject("PORTA_DIANTEIRA");
                string valor = "\"" + cmbox_pd.Text + "\"";
                workPart.Expressions.EditWithUnits(expression4, null, valor);
            }
            catch
            {
            }
            try
            {
                Expression expression5 = (Expression)workPart.Expressions.FindObject("PORTA_CENTRAL");
                string valor = "\"" + cmbbox_pc.Text + "\"";
                workPart.Expressions.EditWithUnits(expression5, null, valor);
            }
            catch
            {}
            try
            {
                Expression expression5 = (Expression)workPart.Expressions.FindObject("PORTA_CENTRAL");
                string valor = "\"" + cmbbox_pc.Text + "\"";
                workPart.Expressions.EditWithUnits(expression5, null, valor);
            }
            catch
            {
            }
            try
            {
                Expression expression6 = (Expression)workPart.Expressions.FindObject("PORTA_TRASEIRA");
                string valor = "\"" + cmbox_PT.Text + "\"";
                workPart.Expressions.EditWithUnits(expression6, null, valor);
            }
            catch
            {
            }

            int pos_ac = Convert.ToInt32(txt_pos_ac_grvia.Text);
            double limite_ini_man = pos_ac +300;
            int comp = Convert.ToInt32(txt_comp_grvia.Text);

            int area_livre_le = comp - Convert.ToInt32(inicio_duto) - Convert.ToInt32(final_duto);
            double array_le = Math.Floor(Convert.ToDouble(area_livre_le / 800)) + 1;

            bool flg = true;
            double pos_inical = 1740 + 40 + 480.1705;
            double pos_inical_acab = inicio_duto;
            double folga = 0;
            for (int i = 0; i <= array_le - 1; i++)
            {
                double tam_acab = 800;

                double pos_acab = pos_inical + (tam_acab * i) + folga + 3;

                if (pos_acab + 200 > limite_ini_man && flg == true)
                {
                    folga = 115;
                    pos_acab = pos_acab + folga;
                    flg = false;
                    pos_manutencao_LE = pos_acab - 400-57;
                }
                double pos_real_acab = pos_inical_acab + (tam_acab * i);
                
            }
            string pos_manu_le = Convert.ToString(pos_manutencao_LE);
            pos_manu_le = pos_manu_le.Replace(",", ".");

            try
            {
                Expression expression6 = (Expression)workPart.Expressions.FindObject("POS_MANUTENCAO_LE");
                Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

                {
                    workPart.Expressions.EditWithUnits(expression6, unit1, pos_manu_le);
                }

            }
            catch
            {
            }

            int area_livre_ld = comp - Convert.ToInt32(inicio_duto) - Convert.ToInt32(final_duto);
            double array_ld = Math.Floor(Convert.ToDouble(area_livre_ld / 800)) + 1;

            bool flg_ld = true;
            double pos_inical_ld = 1740+40+ 480.1705;
            double pos_inical_acab_ld = inicio_duto;
            double folga_ld = 0;
            for (int i = 0; i <= array_ld - 1; i++)
            {

                double tam_acab = 800;

                double pos_acab = pos_inical_ld + (tam_acab * i) + folga_ld + 3;

                if ((pos_acab + 200) > limite_ini_man && flg_ld == true)
                {
                    folga_ld = 115;
                    pos_acab = pos_acab + folga_ld;
                    flg_ld = false;
                    pos_manutencao_LD = pos_acab - 400 - 57;
                }
                double pos_real_acab = pos_inical_acab_ld + (tam_acab * i);
            }
            string pos_manu_ld = Convert.ToString(pos_manutencao_LD);
            pos_manu_ld = pos_manu_ld.Replace(",", ".");

            try
            {
                Expression expression7 = (Expression)workPart.Expressions.FindObject("POS_MANUTENCAO_LD");
                Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

                {
                    workPart.Expressions.EditWithUnits(expression7, unit1, pos_manu_ld);
                }

            }
            catch
            {
            }
            theSession.Preferences.Modeling.UpdatePending = true;

            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Session");
            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

            panel_select.Enabled = true;
        }
        public void aplicar_configuracao_tpl_chap()
        {

          ///  MessageBox.Show("11111111111111");
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_EM_PERFIS_SUPORTE_DUTOS_AC_GRV/000 1");
            Part part1 = (Part)theSession.Parts.FindObject("@DB/TPL_EM_PERFIS_SUPORTE_DUTOS_AC_GRV/000");

            PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

            workPart = theSession.Parts.Work;
            partLoadStatus1.Dispose();

            theSession.Preferences.Modeling.UpdatePending = false;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Session");

            try
            {
                Expression expression1 = (Expression)workPart.Expressions.FindObject("COMP");
                Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

                {
                    workPart.Expressions.EditWithUnits(expression1, unit1, txt_comp_grvia.Text);
                }

            }
            catch
            {
            }
            try
            {
                Expression expression2 = (Expression)workPart.Expressions.FindObject("POS_AC");
                Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

                {
                    workPart.Expressions.EditWithUnits(expression2, unit1, txt_pos_ac_grvia.Text);
                }

            }
            catch
            {
            }
            try
            {
                Expression expression3 = (Expression)workPart.Expressions.FindObject("POS_PORTA_CENTRAL");
                Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

                {
                    workPart.Expressions.EditWithUnits(expression3, unit1, txtbox_pc_grvia.Text);
                }

            }
            catch
            {
            }
            try
            {
                Expression expression3 = (Expression)workPart.Expressions.FindObject("POS_PORTA_TRASEIRA");
                Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");

                {
                    workPart.Expressions.EditWithUnits(expression3, unit1, txtbox_pt_grvia.Text);
                }

            }
            catch
            {
            }

            try
            {
                Expression expression4 = (Expression)workPart.Expressions.FindObject("PORTA_DIANTEIRA");
                string valor = "\"" + cmbox_pd.Text + "\"";
                workPart.Expressions.EditWithUnits(expression4, null, valor);
            }
            catch
            {
            }
            try
            {
                Expression expression5 = (Expression)workPart.Expressions.FindObject("PORTA_CENTRAL");
                string valor = "\"" + cmbbox_pc.Text + "\"";
                workPart.Expressions.EditWithUnits(expression5, null, valor);
            }
            catch
            { }
            try
            {
                Expression expression5 = (Expression)workPart.Expressions.FindObject("PORTA_CENTRAL");
                string valor = "\"" + cmbbox_pc.Text + "\"";
                workPart.Expressions.EditWithUnits(expression5, null, valor);
            }
            catch
            {
            }
            try
            {
                Expression expression6 = (Expression)workPart.Expressions.FindObject("PORTA_TRASEIRA");
                string valor = "\"" + cmbox_PT.Text + "\"";
                workPart.Expressions.EditWithUnits(expression6, null, valor);
            }
            catch
            {
            }

            
            theSession.Preferences.Modeling.UpdatePending = true;

            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Session");
            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

           /// panel_select.Enabled = true;
        }
        public void LE()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            int pos_ac = Convert.ToInt32(txt_pos_ac_grvia.Text);
            double limite_ini_man = pos_ac + 300;
            int comp = Convert.ToInt32(txt_comp_grvia.Text);

            int area_livre_le = comp - 1860 - 360;
            double array_le = Math.Floor(Convert.ToDouble(area_livre_le / 800)) + 1;

            bool flg = true;
            double pos_inical = 1740+40+480.1705;
            double pos_inical_acab = 1860;
            double folga = 0;
            for (int i = 0; i <= array_le - 1; i++)
            {

                double tam_acab = 800;

                double pos_acab = pos_inical + (tam_acab * i) + folga+3;

                if (pos_acab + 200 > limite_ini_man && flg == true)
                {
                    folga = 115;
                    pos_acab = pos_acab + folga;
                    flg = false;
                }
                double pos_real_acab = pos_inical_acab + (tam_acab * i);

                Point3d basePoint1;
                Matrix3x3 orientation1;

                basePoint1 = new Point3d(750, pos_acab, 1752.2);
                orientation1.Xx = -0.966297307;
                orientation1.Xy = 0.0;
                orientation1.Xz = 0.257428657;
                orientation1.Yx = 0.0;
                orientation1.Yy = -1.0;
                orientation1.Yz = 0.0;
                orientation1.Zx = 0.257428657;
                orientation1.Zy = 0.0;
                orientation1.Zz = 0.966297307;


                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;

                try
                {
                    component1 = workPart.ComponentAssembly.AddComponent("@DB/" + "236831" + "/000", "MODEL", "LE_" + (i+1).ToString(), basePoint1, orientation1, -1, out partLoadStatus3, true);
                }
                catch
                {

                    component1 = workPart.ComponentAssembly.AddComponent("@DB/" + "236831", "MODEL", "LE_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus3, true);
                }


                partLoadStatus3.Dispose();

                NXObject[] objects1 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);


            }

        }
        public void LD()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            int pos_ac = Convert.ToInt32(txt_pos_ac_grvia.Text);
            double limite_ini_man = pos_ac + 300;
            int comp = Convert.ToInt32(txt_comp_grvia.Text);

            int area_livre_ld = comp - 1860 - 360;
            double array_ld = Math.Floor(Convert.ToDouble(area_livre_ld / 800)) + 1;

            bool flg = true;
            double pos_inical = 1740+40+480.1705;
            double pos_inical_acab = 1860;
            double folga = 0;
            for (int i = 0; i <= array_ld - 1; i++)
            {

                double tam_acab = 800;

                double pos_acab = pos_inical + (tam_acab * i) + folga+3;

                if ((pos_acab +200)> limite_ini_man && flg == true)
                {
                    folga = 115;
                    pos_acab = pos_acab + folga;
                    flg = false;
                }
                double pos_real_acab = pos_inical_acab + (tam_acab * i);

                Point3d basePoint1;
                Matrix3x3 orientation1;

                basePoint1 = new Point3d(-750, pos_acab, 1752.2);
                orientation1.Xx = 0.966297307;
                orientation1.Xy = 0.0;
                orientation1.Xz = 0.257428657;
                orientation1.Yx = 0.0;
                orientation1.Yy = 1.0;
                orientation1.Yz = 0.0;
                orientation1.Zx = -0.257428657;
                orientation1.Zy = 0.0;
                orientation1.Zz = 0.966297307;


                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;

                try
                {
                    component1 = workPart.ComponentAssembly.AddComponent("@DB/" + "236831" + "/000", "MODEL","LD_"+ (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus3, true);
                }
                catch
                {

                    component1 = workPart.ComponentAssembly.AddComponent("@DB/" + "236831", "MODEL", "LD_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus3, true);
                }


                partLoadStatus3.Dispose();

                NXObject[] objects1 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);


            }

        }
        public static void SelectComponet( ref NXObject[] selectedObjects)
        {
           

            UI ui = NXOpen.UI.GetUI();

            string message = "Selecione os acabamentos para substituir";
            string title = "Selecão";

            Selection.SelectionScope scope = Selection.SelectionScope.WorkPart;
            bool keepHighlighted = false;
            bool includeFeatures = false;
            Selection.Response response = default(Selection.Response);

            Selection.SelectionAction selectionAction = Selection.SelectionAction.ClearAndEnableSpecific;

            Selection.MaskTriple[] selectionMask_array = new Selection.MaskTriple[100];
            {
                selectionMask_array[0].Type = UFConstants.UF_component_type;

                selectionMask_array[0].Subtype = 0;

            }



            response = ui.SelectionManager.SelectObjects(message, title, scope, selectionAction, includeFeatures, keepHighlighted, selectionMask_array, out selectedObjects);
            if (response == Selection.Response.Cancel | response == Selection.Response.Back)
            {
                return;
            }
        }
            private void btn_aplicar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            aplicar_configuracao_tpl_acab();           
            LE();
            LD();
            
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            NXOpen.Assemblies.Component nullAssemblies_Component = null;
            PartLoadStatus partLoadStatus2;
            theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus2);

            workPart = theSession.Parts.Work;
            partLoadStatus2.Dispose();

            aplicar_configuracao_tpl_chap();

            //PartLoadStatus partLoadStatus2;
            theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus2);
            workPart = theSession.Parts.Work;
            partLoadStatus2.Dispose();

            MessageBox.Show("Configuração aplicada", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            this.WindowState = FormWindowState.Normal;

        }

        private void btn_altof_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;


            

            NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_EM_DUTOS_AC_INJETADO_GRV/000 1");
            NXOpen.Assemblies.Component[] components1 = new NXOpen.Assemblies.Component[1];
            components1[0] = component1;
            ErrorList errorList1;
            errorList1 = component1.DisplayComponentsExact(components1);

            errorList1.Clear();

            theSession.Preferences.Modeling.UpdatePending = false;

            theSession.Preferences.Modeling.UpdatePending = false;

            Part part1 = (Part)theSession.Parts.FindObject("@DB/TPL_EM_DUTOS_AC_INJETADO_GRV/000");
            PartLoadStatus partLoadStatus1;
            NXOpen.PartCollection.SdpsStatus status1;
            status1 = theSession.Parts.SetDisplay(part1, false, true, out partLoadStatus1);

            workPart = theSession.Parts.Work;
            displayPart = theSession.Parts.Display;
            partLoadStatus1.Dispose();


            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Displayed Part");



            NXObject[] objects1 = null;
            SelectComponet(ref objects1);


            List<String> lista_sub = new List<String>();
            List<String> lista_le = new List<String>();
            List<String> lista_ld = new List<String>();
            for (int i = 0; i <= objects1.Length - 1; i++)
            {
                lista_sub.Add(objects1[i].Name);
                List<NXOpen.Assemblies.Component> Componentes_color = new List<NXOpen.Assemblies.Component>();

                NXOpen.Assemblies.Component cmp_color = workPart.ComponentAssembly.RootComponent;

                Componentes_color.Add(cmp_color);
                Componentes_color.ToArray()[0].GetChildren();

                for (int j = 0; j < 1; j++)
                {
                    NXOpen.Assemblies.Component[] cmps = Componentes_color.ToArray()[j].GetChildren();

                    foreach (NXOpen.Assemblies.Component cmp_trocar_cor in cmps)
                    {

                        if (objects1[i].Name == cmp_trocar_cor.Name)
                        {
                            if(objects1[i].Name.Contains("LE"))
                            {
                               
                                lista_le.Add(cmp_trocar_cor.NameLocation.Y.ToString());
                            }
                            if (objects1[i].Name.Contains("LD"))
                            {
                                lista_ld.Add(cmp_trocar_cor.NameLocation.Y.ToString());

                            }
                            DisplayModification modificar_cor;
                            modificar_cor = theSession.DisplayManager.NewDisplayModification();

                            modificar_cor.ApplyToAllFaces = false;

                            modificar_cor.ApplyToOwningParts = false;

                            modificar_cor.NewColor = 103;

                            DisplayableObject[] objects12 = new DisplayableObject[1];
                            objects12[0] = cmp_trocar_cor;
                            modificar_cor.Apply(objects12);

                            modificar_cor.Dispose();
                        }
                    }
                }
            }


            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();

            NXOpen.Assemblies.Component cmp = workPart.ComponentAssembly.RootComponent;

            Componentes.Add(cmp);

            Componentes.ToArray()[0].GetChildren();

            for (int i = 0; i < 1; i++)
            {
                NXOpen.Assemblies.Component[] cmps = Componentes.ToArray()[i].GetChildren();

                foreach (NXOpen.Assemblies.Component cmp_subsituir in cmps)
                {

                    if (lista_sub.Contains(cmp_subsituir.Name))
                    {
                        NXOpen.Assemblies.ReplaceComponentBuilder replaceComponentBuilder1;
                        replaceComponentBuilder1 = workPart.AssemblyManager.CreateReplaceComponentBuilder();

                        replaceComponentBuilder1.ReplaceAllOccurrences = false;
                        replaceComponentBuilder1.ComponentNameType = NXOpen.Assemblies.ReplaceComponentBuilder.ComponentNameOption.AsSpecified;
                        replaceComponentBuilder1.SetComponentReferenceSetType(NXOpen.Assemblies.ReplaceComponentBuilder.ComponentReferenceSet.Maintain, null);

                        bool added1;
                        added1 = replaceComponentBuilder1.ComponentsToReplace.Add(cmp_subsituir);

                        replaceComponentBuilder1.ComponentName = cmp_subsituir.Name;
                        try
                        {
                            replaceComponentBuilder1.ReplacementPart = "@DB/" + "236829/000";
                        }
                        catch
                        {
                            replaceComponentBuilder1.ReplacementPart = "@DB/" + "236829";
                        }
                        PartLoadStatus partLoadStatus3;
                        partLoadStatus3 = replaceComponentBuilder1.RegisterReplacePartLoadStatus();

                        replaceComponentBuilder1.Commit();

                        partLoadStatus3.Dispose();

                        replaceComponentBuilder1.Destroy();


                    }
                }
            }
            lista_ld.Sort();
            lista_le.Sort();
            for (int i = 0; i <= lista_ld.Count-1; i++)
            {
                Point3d basePoint1;
                Matrix3x3 orientation1;

                decimal pos_y = Convert.ToDecimal(lista_ld[i]);
               // MessageBox.Show(Convert.ToString(pos_y));
                basePoint1 = new Point3d(-722.377483860, Convert.ToDouble(pos_y), 1987.836475831);

                orientation1.Xx = 0.639075960;
                orientation1.Xy = 0.001787428;
                orientation1.Xz = -0.769141549;
                orientation1.Yx = -0.000151958;
                orientation1.Yy = 0.999997574;
                orientation1.Yz = -0.002197660;
                orientation1.Zx = 0.769143611;
                orientation1.Zy = 0.001287594;
                orientation1.Zz = 0.639074681;


                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component tampa_af;

                try
                {
                    tampa_af = workPart.ComponentAssembly.AddComponent("@DB/" + "271639" + "/000", "EM_DUTOS_LD", "AF_LD_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus3, true);
                }
                catch
                {

                    tampa_af = workPart.ComponentAssembly.AddComponent("@DB/" + "271639", "EM_DUTOS_LD", "AF_LD_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus3, true);
                }


                partLoadStatus3.Dispose();
                NXObject[] objects12 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects12);
            }
            for (int i = 0; i <= lista_ld.Count - 1; i++)
            {
                Point3d basePoint1;
                Matrix3x3 orientation1;

                decimal pos_y = Convert.ToDecimal(lista_ld[i]);
                // MessageBox.Show(Convert.ToString(pos_y));
                basePoint1 = new Point3d(-804.720627399, Convert.ToDouble(pos_y), 1977.274238784);

                orientation1.Xx = 0.543680207;
                orientation1.Xy = 0.707107418;
                orientation1.Xz = 0.452118272;

                //XC = 0.543680207          X = 0.543680207
                //     YC = 0.707107418          Y = 0.707107418
                //     ZC = 0.452118272          Z = 0.452118272

                orientation1.Yx = -0.544048579;
                orientation1.Yy = 0.707106028;
                orientation1.Yz = -0.451677108;

                //XC = -0.544048579          X = -0.544048579
                //     YC = 0.707106028          Y = 0.707106028
                //     ZC = -0.451677108          Z = -0.451677108

                orientation1.Zx = -0.639079789;
                orientation1.Zy = -0.000406400;
                orientation1.Zz = 0.769140337;

                //XC = -0.639079789          X = -0.639079789
                //     YC = -0.000406400          Y = -0.000406400
                //     ZC = 0.769140337          Z = 0.769140337


                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component af;

                try
                {
                    af = workPart.ComponentAssembly.AddComponent("@DB/" + "004408" + "/000", "MODEL", "ALTO_LD_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus3, true);
                }
                catch
                {

                    af = workPart.ComponentAssembly.AddComponent("@DB/" + "004408", "MODEL", "ALTO_LD_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus3, true);
                }


                partLoadStatus3.Dispose();
                NXObject[] objects12 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects12);
            }
            for (int i = 0; i <= lista_le.Count - 1; i++)
            {
                Point3d basePoint1;
                Matrix3x3 orientation1;

                decimal pos_y = Convert.ToDecimal(lista_le[i]);
                // MessageBox.Show(Convert.ToString(pos_y));
                basePoint1 = new Point3d(722.377483860, Convert.ToDouble(pos_y), 1987.836475831);

                orientation1.Xx = -0.639079822;
                orientation1.Xy = -0.000151959;
                orientation1.Xz = -0.769140402;
                orientation1.Yx = 0.000406709;
                orientation1.Yy = -0.999999907;
                orientation1.Yz = -0.000140365;
                orientation1.Zx = -0.769140310;
                orientation1.Zy = -0.000402521;
                orientation1.Zz = 0.639079824;


                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component tampa_af;

                try
                {
                    tampa_af = workPart.ComponentAssembly.AddComponent("@DB/" + "271639" + "/000", "EM_DUTOS_LE", "AF_LE_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus3, true);
                }
                catch
                {

                    tampa_af = workPart.ComponentAssembly.AddComponent("@DB/" + "271639", "EM_DUTOS_LE", "AF_LE_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus3, true);
                }


                partLoadStatus3.Dispose();
                NXObject[] objects12 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects12);
            }
            for (int i = 0; i <= lista_le.Count - 1; i++)
            {
                Point3d basePoint1;
                Matrix3x3 orientation1;

                decimal pos_y = Convert.ToDecimal(lista_le[i]);

                basePoint1 = new Point3d(804.720627399, Convert.ToDouble(pos_y), 1977.274238784);

                orientation1.Xx = -0.543680207;
                orientation1.Xy = 0.707107418;
                orientation1.Xz = 0.452118272;


                orientation1.Yx = -0.544048579;
                orientation1.Yy = -0.707106028;
                orientation1.Yz = 0.451677108;

                orientation1.Zx = 0.639079789;
                orientation1.Zy = -0.000406400;
                orientation1.Zz = 0.769140337;


                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component af;

                try
                {
                    af = workPart.ComponentAssembly.AddComponent("@DB/" + "004408" + "/000", "MODEL", "ALTO_LE_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus3, true);
                }
                catch
                {

                    af = workPart.ComponentAssembly.AddComponent("@DB/" + "004408", "MODEL", "ALTO_LE_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus3, true);
                }


                partLoadStatus3.Dispose();
                NXObject[] objects12 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects12);
            }
            int n_sel_atual = lista_sub.Count;
            if (n_sel == 0)
            {
                n_sel = n_sel_atual;
            }
            else
            {
                n_sel = n_sel + n_sel_atual;
            }
            if (n_sel == n_af)
            {
                lbl_inf_alto_falantes.BackColor = Color.MediumTurquoise;
                lbl_inf_alto_falantes.ForeColor = Color.CornflowerBlue;
                this.lbl_inf_alto_falantes.Text = "Configuração dos AF Finalizada";
            }
            if (n_sel < n_af)
            {
                int falta_af = n_af - n_sel;
                lbl_inf_alto_falantes.BackColor = Color.Red;
                this.lbl_inf_alto_falantes.Text = "Faltam Selecionar " + falta_af.ToString() + " AF's";
            }
            Part part2 = (Part)theSession.Parts.FindObject("@DB/TPL_CONFIGURADOR_DUTOS_AC_GRV/000");
            PartLoadStatus partLoadStatus2;
            NXOpen.PartCollection.SdpsStatus status2;
            status2 = theSession.Parts.SetDisplay(part2, false, true, out partLoadStatus2);

            workPart = theSession.Parts.Work;
            displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();
              this.WindowState = FormWindowState.Normal;

        }

        private void btn_parada_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;




            NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_EM_DUTOS_AC_INJETADO_GRV/000 1");
            NXOpen.Assemblies.Component[] components1 = new NXOpen.Assemblies.Component[1];
            components1[0] = component1;
            ErrorList errorList1;
            errorList1 = component1.DisplayComponentsExact(components1);

            errorList1.Clear();

            theSession.Preferences.Modeling.UpdatePending = false;

            theSession.Preferences.Modeling.UpdatePending = false;

            Part part1 = (Part)theSession.Parts.FindObject("@DB/TPL_EM_DUTOS_AC_INJETADO_GRV/000");
            PartLoadStatus partLoadStatus1;
            NXOpen.PartCollection.SdpsStatus status1;
            status1 = theSession.Parts.SetDisplay(part1, false, true, out partLoadStatus1);

            workPart = theSession.Parts.Work;
            displayPart = theSession.Parts.Display;
            partLoadStatus1.Dispose();


            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Displayed Part");


            workPart = theSession.Parts.Work;
            partLoadStatus1.Dispose();
            NXObject[] objects1 = null;
            SelectComponet(ref objects1);

            
            List<String> lista_sub = new List<String>();
            List<String> lista_le = new List<String>();
            List<String> lista_ld = new List<String>();
            for (int i = 0; i <= objects1.Length - 1; i++)
            {
                lista_sub.Add(objects1[i].Name);
                List<NXOpen.Assemblies.Component> Componentes_color = new List<NXOpen.Assemblies.Component>();

                NXOpen.Assemblies.Component cmp_color = workPart.ComponentAssembly.RootComponent;

                Componentes_color.Add(cmp_color);
                Componentes_color.ToArray()[0].GetChildren();

                for (int j = 0; j < 1; j++)
                {
                    NXOpen.Assemblies.Component[] cmps = Componentes_color.ToArray()[j].GetChildren();

                    foreach (NXOpen.Assemblies.Component cmp_trocar_cor in cmps)
                    {
                        if (objects1[i].Name == cmp_trocar_cor.Name)
                        {
                            if (objects1[i].Name.Contains("LE"))
                            {
                                lista_le.Add(cmp_trocar_cor.NameLocation.Y.ToString());

                            }
                            if (objects1[i].Name.Contains("LD"))
                            {
                                lista_ld.Add(cmp_trocar_cor.NameLocation.Y.ToString());
                               
                            }
                            DisplayModification modificar_cor;
                            modificar_cor = theSession.DisplayManager.NewDisplayModification();

                            modificar_cor.ApplyToAllFaces = false;

                            modificar_cor.ApplyToOwningParts = false;

                            modificar_cor.NewColor = 186;

                            DisplayableObject[] objects12 = new DisplayableObject[1];
                            objects12[0] = cmp_trocar_cor;
                            modificar_cor.Apply(objects12);

                            modificar_cor.Dispose();

                        }
                    }
                }
            }

            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();

            NXOpen.Assemblies.Component cmp = workPart.ComponentAssembly.RootComponent;

            Componentes.Add(cmp);

            Componentes.ToArray()[0].GetChildren();

            for (int i = 0; i < 1; i++)
            {
                NXOpen.Assemblies.Component[] cmps = Componentes.ToArray()[i].GetChildren();

                foreach (NXOpen.Assemblies.Component cmp_subsituir in cmps)
                {
                    int cont = 0;

                    if (lista_sub.Contains(cmp_subsituir.Name))
                    {                      
                        NXOpen.Assemblies.ReplaceComponentBuilder replaceComponentBuilder1;
                        replaceComponentBuilder1 = workPart.AssemblyManager.CreateReplaceComponentBuilder();

                        replaceComponentBuilder1.ReplaceAllOccurrences = false;
                        replaceComponentBuilder1.ComponentNameType = NXOpen.Assemblies.ReplaceComponentBuilder.ComponentNameOption.AsSpecified;
                        replaceComponentBuilder1.SetComponentReferenceSetType(NXOpen.Assemblies.ReplaceComponentBuilder.ComponentReferenceSet.Maintain, null);

                        bool added1;
                        added1 = replaceComponentBuilder1.ComponentsToReplace.Add(cmp_subsituir);

                        replaceComponentBuilder1.ComponentName = cmp_subsituir.Name;
                        try
                        {
                            replaceComponentBuilder1.ReplacementPart = "@DB/" + "236829/000";
                        }
                        catch
                        {
                            replaceComponentBuilder1.ReplacementPart = "@DB/" + "236829";
                        }
                        PartLoadStatus partLoadStatus2;
                        partLoadStatus2 = replaceComponentBuilder1.RegisterReplacePartLoadStatus();

                        replaceComponentBuilder1.Commit();

                        partLoadStatus2.Dispose();

                        replaceComponentBuilder1.Destroy();

                    }
                }
            }
            lista_ld.Sort();
            lista_le.Sort();
            for (int i = 0; i <= lista_ld.Count - 1; i++)
            {
                Point3d basePoint1;
                Matrix3x3 orientation1;

                decimal pos_y = Convert.ToDecimal(lista_ld[i]);
                // MessageBox.Show(Convert.ToString(pos_y));
                basePoint1 = new Point3d(-722.377483860, Convert.ToDouble(pos_y), 1987.836475831);

                orientation1.Xx = 0.639075960;
                orientation1.Xy = 0.001787428;
                orientation1.Xz = -0.769141549;
                orientation1.Yx = -0.000151958;
                orientation1.Yy = 0.999997574;
                orientation1.Yz = -0.002197660;
                orientation1.Zx = 0.769143611;
                orientation1.Zy = 0.001287594;
                orientation1.Zz = 0.639074681;


                PartLoadStatus partLoadStatus33;
                NXOpen.Assemblies.Component tampa_af;

                try
                {
                    tampa_af = workPart.ComponentAssembly.AddComponent("@DB/" + "271638" + "/000", "EM_DUTOS_LD", "PS_LD_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus33, true);
                }
                catch
                {

                    tampa_af = workPart.ComponentAssembly.AddComponent("@DB/" + "271638", "EM_DUTOS_LD", "PS_LD_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus33, true);
                }

                partLoadStatus33.Dispose();
                NXObject[] objects12 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects12);
            }
            for (int i = 0; i <= lista_ld.Count - 1; i++)
            {
                Point3d basePoint1;
                Matrix3x3 orientation1;

                decimal pos_y = Convert.ToDecimal(lista_ld[i]);
                // MessageBox.Show(Convert.ToString(pos_y));
                basePoint1 = new Point3d(-779.693285805, Convert.ToDouble(pos_y), 1951.671953499);

                orientation1.Xx = -0.769156461;
                orientation1.Xy = 0.000000000;
                orientation1.Xz = -0.639060512;

                orientation1.Yx = 0.000000000;
                orientation1.Yy = 1.000000000;
                orientation1.Yz = 0.000000000;

                orientation1.Zx = 0.639060512;
                orientation1.Zy = 0.000000000;
                orientation1.Zz = -0.769156461;


                PartLoadStatus partLoadStatus33;
                NXOpen.Assemblies.Component tampa_af;

                try
                {
                    tampa_af = workPart.ComponentAssembly.AddComponent("@DB/" + "093508" + "/000", "MODEL", "LED_LD_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus33, true);
                }
                catch
                {

                    tampa_af = workPart.ComponentAssembly.AddComponent("@DB/" + "093508", "MODEL", "LED_LD_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus33, true);
                }

                partLoadStatus33.Dispose();
                NXObject[] objects12 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects12);
            }
            for (int i = 0; i <= lista_le.Count - 1; i++)
            {
                Point3d basePoint1;
                Matrix3x3 orientation1;

                decimal pos_y = Convert.ToDecimal(lista_le[i]);
                // MessageBox.Show(Convert.ToString(pos_y));
                basePoint1 = new Point3d(722.377483860, Convert.ToDouble(pos_y), 1987.836475831);

                orientation1.Xx = -0.639079822;
                orientation1.Xy = -0.000151959;
                orientation1.Xz = -0.769140402;
                orientation1.Yx = 0.000406709;
                orientation1.Yy = -0.999999907;
                orientation1.Yz = -0.000140365;
                orientation1.Zx = -0.769140310;
                orientation1.Zy = -0.000402521;
                orientation1.Zz = 0.639079824;


                PartLoadStatus partLoadStatus33;
                NXOpen.Assemblies.Component tampa_af;

                try
                {
                    tampa_af = workPart.ComponentAssembly.AddComponent("@DB/" + "271638" + "/000", "EM_DUTOS_LE", "PS_LE_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus33, true);
                }
                catch
                {

                    tampa_af = workPart.ComponentAssembly.AddComponent("@DB/" + "271638", "EM_DUTOS_LE", "PS_LE_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus33, true);
                }


                partLoadStatus33.Dispose();
                NXObject[] objects12 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects12);
            }
            for (int i = 0; i <= lista_le.Count - 1; i++)
            {
                Point3d basePoint1;
                Matrix3x3 orientation1;

                decimal pos_y = Convert.ToDecimal(lista_le[i]);
                // MessageBox.Show(Convert.ToString(pos_y));
                basePoint1 = new Point3d(779.693285805, Convert.ToDouble(pos_y), 1951.671953499);

                orientation1.Xx = -0.769156461;
                orientation1.Xy = 0.000000000;
                orientation1.Xz = 0.639060512;

                orientation1.Yx = 0.000000000;
                orientation1.Yy = 1.000000000;
                orientation1.Yz = 0.000000000;

                orientation1.Zx = -0.639060512;
                orientation1.Zy = 0.000000000;
                orientation1.Zz = -0.769156461;


                PartLoadStatus partLoadStatus33;
                NXOpen.Assemblies.Component tampa_af;

                try
                {
                    tampa_af = workPart.ComponentAssembly.AddComponent("@DB/" + "093508" + "/000", "MODEL", "LED_LE_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus33, true);
                }
                catch
                {

                    tampa_af = workPart.ComponentAssembly.AddComponent("@DB/" + "093508", "MODEL", "LED_LE_" + (i + 1).ToString(), basePoint1, orientation1, -1, out partLoadStatus33, true);
                }

                partLoadStatus33.Dispose();
                NXObject[] objects12 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects12);
            }
            //if(cmbox_PT.Text ==" 1.4.1 - Não" && cmbbox_pc == )
            //{
            //    n_ps = 3;
            //}
            int n_sel_atual = lista_sub.Count;
            // MessageBox.Show(n_sel_atual.ToString());

            if (n_ps_sel == 0)
            {
                n_ps_sel = n_sel_atual;
            }
            else
            {
                n_ps_sel = n_ps_sel + n_sel_atual;
            }
            if (n_ps_sel == n_ps)
            {
                //  MessageBox.Show("a "+n_sel.ToString() + "    " + n_af.ToString());
                lbl_inf_ps.BackColor = Color.MediumTurquoise;
                lbl_inf_ps.ForeColor = Color.CornflowerBlue;
                this.lbl_inf_ps.Text = "Configuração das PS Finalizada";
            }
            if (n_ps_sel < n_ps)
            {
                //  MessageBox.Show("b  "+n_sel.ToString() + "    " + n_af.ToString());
                int falta_af = n_ps - n_ps_sel;

                lbl_inf_ps.BackColor = Color.Red;
                this.lbl_inf_ps.Text = "Faltam Selecionar " + falta_af.ToString() + " PS's";
            }
            Part part2 = (Part)theSession.Parts.FindObject("@DB/TPL_CONFIGURADOR_DUTOS_AC_GRV/000");
            PartLoadStatus partLoadStatus22;
            NXOpen.PartCollection.SdpsStatus status2;
            status2 = theSession.Parts.SetDisplay(part2, false, true, out partLoadStatus22);

            workPart = theSession.Parts.Work;
            displayPart = theSession.Parts.Display;
            partLoadStatus22.Dispose();

            this.WindowState = FormWindowState.Normal;
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();

            NXOpen.Assemblies.Component cmp = workPart.ComponentAssembly.RootComponent;

            Componentes.Add(cmp);

            Componentes.ToArray()[0].GetChildren();

            for (int i = 0; i < 1; i++)
            {
                NXOpen.Assemblies.Component[] cmps = Componentes.ToArray()[i].GetChildren();

                foreach (NXOpen.Assemblies.Component cmp_subsituir in cmps)
                {

                    if (cmp_subsituir.DisplayName == "236829/000")
                    {
                        DisplayModification displayModification1;
                        displayModification1 = theSession.DisplayManager.NewDisplayModification();

                        displayModification1.ApplyToAllFaces = false;

                        displayModification1.ApplyToOwningParts = false;

                        displayModification1.NewColor = 1;

                        DisplayableObject[] comp_trocar_cor = new DisplayableObject[1];
                        /// NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 236831/000 12");
                        comp_trocar_cor[0] = cmp_subsituir;
                        displayModification1.Apply(comp_trocar_cor);

                        displayModification1.Dispose();

                        NXOpen.Assemblies.ReplaceComponentBuilder replaceComponentBuilder1;
                        replaceComponentBuilder1 = workPart.AssemblyManager.CreateReplaceComponentBuilder();

                        replaceComponentBuilder1.ReplaceAllOccurrences = false;
                        replaceComponentBuilder1.ComponentNameType = NXOpen.Assemblies.ReplaceComponentBuilder.ComponentNameOption.AsSpecified;
                        replaceComponentBuilder1.SetComponentReferenceSetType(NXOpen.Assemblies.ReplaceComponentBuilder.ComponentReferenceSet.Maintain, null);

                        bool added1;
                        added1 = replaceComponentBuilder1.ComponentsToReplace.Add(cmp_subsituir);

                        replaceComponentBuilder1.ComponentName = cmp_subsituir.Name;
                        try
                        {
                            replaceComponentBuilder1.ReplacementPart = "@DB/" + "236831/000";
                        }
                        catch
                        {
                            replaceComponentBuilder1.ReplacementPart = "@DB/" + "236831";
                        }
                        PartLoadStatus partLoadStatus1;
                        partLoadStatus1 = replaceComponentBuilder1.RegisterReplacePartLoadStatus();

                        replaceComponentBuilder1.Commit();

                        partLoadStatus1.Dispose();

                        replaceComponentBuilder1.Destroy();

                    }
                }
            }

            this.lbl_inf_alto_falantes.Text= "Selecione a posição dos AF";
            n_sel = 0;
            this.lbl_inf_ps.Text = "Selecione a posição das PS";
            n_ps_sel = 0;
        }

        private void txt_comp_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void cmbbox_pc_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
