using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NXOpen;
using NXOpen.UF;
using NXOpen.UIStyler;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using NXOpen.Assemblies;
//using MaterialSkin.Controls;
using MetroFramework;
using MetroFramework.Forms;

namespace Custom_Mascarello
{
    public partial class frm_Configurador_S4 : Form
    {
        List<string> List_temp_opcao = new List<string>(new string[] { });
        List<string> Itens_Master = new List<string>(new string[] { });
        List<string> Itens_Filhos = new List<string>(new string[] { });
        string[,] itens_filhos = new string [50,3];

        string[] item_master;

        string CHASSI = "";
        string COMP = "";
        string EE = "0";
        string BD = "0";
        string POS_TQ_ARLA = "0";
        string VAO_TQ_ARLA = "0";
        string POS_TQ = "0";
        string POS_CX_BATERIA = "0";
        string VAO_CX_BATERIA = "0";    
        string POS_ESTEPE = "0";
        string VAO_ESTEPE = "0";
        string POS_PORTA = "0";
        string POS_DIV = "0";
        string WC = "0";
        string SUSP = "0";
        string QTD_ALCAPAO = "0";
        string MODELO_JAN = "";
        string MODELO_JAN_MOT = "";
        string MODELO_AC = "";
        string BAG_EE = "";
        string TIPO_DIV = "";
        string P_BALSA = "";
        string VIGIA_TRAS = "";
        string PORTA_ESTEPE = "";
        string BAG_TRAS = "";

        string _xml_opcao = @"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\NX\MICRO_S4\xml_opcoes_S4.xml"; 
        public frm_Configurador_S4()
        {
            InitializeComponent();

           // var SkinManager = MaterialSkin.MaterialSkinManager.Instance;

           // SkinManager.AddFormToManage(this);
           // SkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT;

           // SkinManager.ColorScheme = new MaterialSkin.ColorScheme(MaterialSkin.Primary.Teal700, MaterialSkin.Primary.Teal900, MaterialSkin.Primary.BlueGrey200, MaterialSkin.Accent.Green200, MaterialSkin.TextShade.WHITE);
        }

        private void btn_import_dna_Click(object sender, EventArgs e)
        {
            dtg_view_dna.Rows.Clear();
            dtg_view_dna.Refresh();

            XmlDocument xmlBiblioteca = new XmlDocument();
            xmlBiblioteca.Load(_xml_opcao);
            XElement xml = XElement.Load(_xml_opcao);

            XmlNodeList lista_opcoes = default(XmlNodeList);

            lista_opcoes = xmlBiblioteca.SelectNodes("/Opcoes_micro_S4/Opcao");

            foreach (XmlNode opcao in lista_opcoes)
            {
                List_temp_opcao.Add(opcao.ChildNodes.Item(0).InnerText);

            }

            string path = @"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\NX\MICRO_S4\DNA\" + txt_fm_dna.Text + ".xml";
            DataSet dna_XML = new DataSet();
            dna_XML.ReadXml(path);
            string table = "DNA" + txt_fm_dna.Text;

            for (int i = 0; i <= List_temp_opcao.Count - 1; i++)
            {
                string Item = CultureInfo.CurrentCulture.TextInfo.ToUpper(List_temp_opcao[i].ToString());
                string Valor = dna_XML.Tables[table].Rows[0][List_temp_opcao[i]].ToString();
                string[] rowi = { Item, Valor };
                dtg_view_dna.Rows.Add(rowi);

            }

            CHASSI = dtg_view_dna.Rows[1].Cells[1].Value.ToString();
            SUSP = dtg_view_dna.Rows[2].Cells[1].Value.ToString();
            COMP =dtg_view_dna.Rows[3].Cells[1].Value.ToString();
            EE = dtg_view_dna.Rows[4].Cells[1].Value.ToString();
            WC = dtg_view_dna.Rows[9].Cells[1].Value.ToString();
            TIPO_DIV = dtg_view_dna.Rows[10].Cells[1].Value.ToString();
            QTD_ALCAPAO = dtg_view_dna.Rows[8].Cells[1].Value.ToString();
            MODELO_AC = dtg_view_dna.Rows[7].Cells[1].Value.ToString();
            VIGIA_TRAS = dtg_view_dna.Rows[12].Cells[1].Value.ToString();
            MODELO_JAN = dtg_view_dna.Rows[13].Cells[1].Value.ToString();
            MODELO_JAN_MOT = dtg_view_dna.Rows[14].Cells[1].Value.ToString();
            BAG_EE = dtg_view_dna.Rows[15].Cells[1].Value.ToString();
            P_BALSA = dtg_view_dna.Rows[20].Cells[1].Value.ToString();
            PORTA_ESTEPE = dtg_view_dna.Rows[19].Cells[1].Value.ToString();
            BAG_TRAS = dtg_view_dna.Rows[16].Cells[1].Value.ToString();
          
            busca_dados_chassi();
            if((CHASSI == "MBB LO-916 PODEST AVANCADO" || CHASSI == "MBB LO-916 PODEST RECUADO") && EE == "4800")
            {
              
                POS_TQ = "2775";
            }
            else
            {
              
                POS_TQ = "2475";
            }
          
             //lbl_criacao.Text = "Dna Criado Por: " + CultureInfo.CurrentCulture.TextInfo.ToUpper(dna_XML.Tables[table].Rows[0]["CRIACAO"].ToString()) + "  Data: " + dna_XML.Tables[table].Rows[0]["DATACRIACAO"].ToString();
           // lbl_alteracao.Text = "Alterado Por: " + CultureInfo.CurrentCulture.TextInfo.ToUpper(dna_XML.Tables[table].Rows[0]["ALTERACAO"].ToString()) + "  Data: " + dna_XML.Tables[table].Rows[0]["DATAALTERACAO"].ToString() + "   " + dna_XML.Tables[table].Rows[0]["HORAALTERACAO"].ToString();
           
            List_temp_opcao.Clear();
        }
        private void busca_dados_chassi()
        {
             XmlDocument xmlBiblioteca = new XmlDocument();
            xmlBiblioteca.Load(@"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\NX\MICRO_S4\xml_dados_S4.xml");
            XElement xml = XElement.Load(@"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\NX\MICRO_S4\xml_dados_S4.xml");

            XmlNodeList Lista_Opçoes = default(XmlNodeList);

            Lista_Opçoes = xmlBiblioteca.SelectNodes("/Dados_S4/Dados");
            foreach (XmlNode opcao in Lista_Opçoes)
            {
                if (opcao.ChildNodes.Item(0).InnerText == CHASSI)
                {
                    BD = opcao.ChildNodes.Item(1).InnerText;
                    POS_TQ_ARLA = opcao.ChildNodes.Item(2).InnerText;
                    VAO_TQ_ARLA = opcao.ChildNodes.Item(3).InnerText;
                    POS_TQ = opcao.ChildNodes.Item(4).InnerText;
                    POS_CX_BATERIA = opcao.ChildNodes.Item(5).InnerText;
                    VAO_CX_BATERIA = opcao.ChildNodes.Item(6).InnerText;
                    POS_ESTEPE = opcao.ChildNodes.Item(7).InnerText;
                    VAO_ESTEPE = opcao.ChildNodes.Item(8).InnerText;
                    POS_PORTA = opcao.ChildNodes.Item(9).InnerText;
                    POS_DIV = opcao.ChildNodes.Item(10).InnerText; 
                }
              
                 
            }

           
            
        }
        private void btn_rditar_dna_Click(object sender, EventArgs e)
        {
            string item_pai = @"S:\\Cad.Minibuss\\CONFIGCAD_2014_MRP\\DLL\NX\\temp\\" + txt_fm_dna.Text;
            System.IO.StreamWriter vWriter = new System.IO.StreamWriter(item_pai, true);
            vWriter.Close();
            System.Diagnostics.Process.Start(@"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\APLICATIVOS\GERACAO_DNA_PLM\App_Geracao_DNA.exe");
        }

        private void btn_aplicar_configuracoes_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            Configurar();
            this.WindowState = FormWindowState.Normal;
        }
        private void Configurar()
        {
            lbl_proccess.Text = "Configurando Dimenções do Casulo...";
           //MessageBox.Show("CHASSI: " + CHASSI +"EE: " + EE+ "BD: " + BD + "   POS_TQ_ARLA: " + POS_TQ_ARLA + "   POS_TQ: "
           //            + POS_TQ + "   POS_BATERIAS: " + POS_CX_BATERIA + "   POS_ESTEPE: " + POS_ESTEPE);
           
          
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            theSession.Preferences.Modeling.UpdatePending = false;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Expression");

            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            try
            {
                Expression expression1 = (Expression)workPart.Expressions.FindObject("COMP");
                workPart.Expressions.EditWithUnits(expression1, unit1, COMP);
            }
            catch (Exception)
            {
                
              
            }
            try
            {
                Expression expression2 = (Expression)workPart.Expressions.FindObject("BD");
                workPart.Expressions.EditWithUnits(expression2, unit1, BD);
            }
            catch (Exception)
            {

              
            }
            try
            {
                Expression expression3 = (Expression)workPart.Expressions.FindObject("EE");
                workPart.Expressions.EditWithUnits(expression3, unit1, EE);
               
            }
            catch (Exception)
            {

            
            }
            try
            {
                Expression expression4 = (Expression)workPart.Expressions.FindObject("POS_TQ_ARLA");
                workPart.Expressions.EditWithUnits(expression4, unit1, POS_TQ_ARLA);
            }
            catch (Exception)
            {

                
            }
            try
            {
                Expression expression5 = (Expression)workPart.Expressions.FindObject("POS_TQ");
                workPart.Expressions.EditWithUnits(expression5, unit1, POS_TQ);
            }
            catch (Exception)
            {

              
            }
            try
            {
                Expression expression6 = (Expression)workPart.Expressions.FindObject("CHASSI");
                string name_chassi_value = "\"" + CHASSI + "\"";
                workPart.Expressions.EditWithUnits(expression6, null, name_chassi_value);
            }
            catch (Exception)
            {

              
            }
            try
            {
                Expression expression7 = (Expression)workPart.Expressions.FindObject("POS_CX_BATERIA");

                if(CHASSI.Contains("MBB"))
                {
                    int pos_bat = Convert.ToInt32(EE)-580;
                    POS_CX_BATERIA = pos_bat.ToString();
                }
                workPart.Expressions.EditWithUnits(expression7, unit1, POS_CX_BATERIA);

            }
            catch (Exception)
            {

            }
            try
            {
                Expression expression8 = (Expression)workPart.Expressions.FindObject("POS_ESTEPE");
                workPart.Expressions.EditWithUnits(expression8, unit1, POS_ESTEPE);
            }
            catch (Exception)
            {

            }
            try
            {
                Expression expression9 = (Expression)workPart.Expressions.FindObject("SUSP");
                if (SUSP == "METALICA")
                {
                    workPart.Expressions.EditWithUnits(expression9, unit1, "0");
                }
                else
                {
                    workPart.Expressions.EditWithUnits(expression9, unit1, "1");
                }
            }
            catch (Exception)
            {

               
            }
            try
            {
                Expression expression10 = (Expression)workPart.Expressions.FindObject("WC");
                if (WC == "5.1.1-NAO")
                {
                    workPart.Expressions.EditWithUnits(expression10, unit1, "0");
                }
                else
                {
                    workPart.Expressions.EditWithUnits(expression10, unit1, "1");
                }
            }
            catch (Exception)
            {

               
            }
            try
            {
                Expression expression11 = (Expression)workPart.Expressions.FindObject("POS_PORTA");
                workPart.Expressions.EditWithUnits(expression11, unit1, POS_PORTA);
            }
            catch (Exception)
            {

                
            }
            try
            {
                Expression expression12 = (Expression)workPart.Expressions.FindObject("VAO_TQ_ARLA");
                workPart.Expressions.EditWithUnits(expression12, unit1, VAO_TQ_ARLA);
                
            }
            catch (Exception)
            {

                
            }

            try
            {
                Expression expression13 = (Expression)workPart.Expressions.FindObject("VAO_CX_BATERIA");

                if (CHASSI.Contains("MBB"))
                {
                    int vao_bat = Convert.ToInt32(VAO_CX_BATERIA)*(-1);
                    VAO_CX_BATERIA = vao_bat.ToString();
                }
                workPart.Expressions.EditWithUnits(expression13, unit1, VAO_CX_BATERIA);
                
            }
            catch (Exception)
            {

            }
            try
            {
                Expression expression14 = (Expression)workPart.Expressions.FindObject("VAO_ESTEPE");
                workPart.Expressions.EditWithUnits(expression14, unit1, VAO_ESTEPE);

            }
            catch (Exception)
            {

               
            }
            try
            {
                Expression expression15 = (Expression)workPart.Expressions.FindObject("QTD_ALC");
                if (QTD_ALCAPAO == "3.4.12-DOIS DE EMERGENCIA C\\ VENTILADOR ACOPLADO")
                {
                    workPart.Expressions.EditWithUnits(expression15, unit1, "2");
                }
                else
                {
                    workPart.Expressions.EditWithUnits(expression15, unit1, "1");
                }
            }
            catch (Exception)
            {

                
            }
            try
            {
                Expression expression16 = (Expression)workPart.Expressions.FindObject("MODELO_JAN");
                if (MODELO_JAN == "JANELAS RODOVIARIAS")
                {
                    workPart.Expressions.EditWithUnits(expression16, unit1, "0");
                }
                else
                {
                    workPart.Expressions.EditWithUnits(expression16, unit1, "1");
                }
            }
            catch (Exception)
            {

                
            }
            try
            {
                Expression expression16 = (Expression)workPart.Expressions.FindObject("MODELO_JAN");
                string modelo_jan = "\"" + MODELO_JAN + "\"";
                workPart.Expressions.EditWithUnits(expression16, null, modelo_jan);
            }
            catch (Exception)
            {


            }
            try
            {
                Expression expression17 = (Expression)workPart.Expressions.FindObject("BAG_EE");
                string bag_ee_value = "\"" + BAG_EE + "\"";
                workPart.Expressions.EditWithUnits(expression17, null, bag_ee_value);
            }
            catch (Exception)
            {

               
            }
            try
            {
                Expression expression18 = (Expression)workPart.Expressions.FindObject("POS_DIV");
                workPart.Expressions.EditWithUnits(expression18, unit1, POS_DIV);
            }
            catch (Exception)
            {

               
            }
            try
            {
                Expression expression19 = (Expression)workPart.Expressions.FindObject("MODELO_AC");
                string modelo_ac = "\"" + MODELO_AC + "\"";
                workPart.Expressions.EditWithUnits(expression19, null, modelo_ac);
            }
            catch (Exception)
            {

               
            }
            try
            {
                Expression expression20 = (Expression)workPart.Expressions.FindObject("TIPO_DIV");
                string tipo_divisoria = "\"" + TIPO_DIV + "\"";
                workPart.Expressions.EditWithUnits(expression20, null, tipo_divisoria);
            }
            catch (Exception)
            {

               
            }
            try
            {
                Expression expression21 = (Expression)workPart.Expressions.FindObject("P_BALSA");
                string passa_balsa = "\"" + P_BALSA + "\"";
                workPart.Expressions.EditWithUnits(expression21, null, passa_balsa);
            }
            catch (Exception)
            {

               
            }
            try
            {
                Expression expression22 = (Expression)workPart.Expressions.FindObject("JAN_MOT");
                string passa_balsa = "\"" + MODELO_JAN_MOT + "\"";
                workPart.Expressions.EditWithUnits(expression22, null, passa_balsa);
            }
            catch (Exception)
            {

            }
            try
            {
                Expression expression23 = (Expression)workPart.Expressions.FindObject("VIGIA");
                string vigia = "\"" + VIGIA_TRAS + "\"";
                workPart.Expressions.EditWithUnits(expression23, null, vigia);
            }
            catch (Exception)
            {

            }

             try
            {
                Expression expression24 = (Expression)workPart.Expressions.FindObject("PORTA_ESTEPE");
                string estepe_value = "\"" + PORTA_ESTEPE + "\"";
                workPart.Expressions.EditWithUnits(expression24, null, estepe_value);
            }
            catch (Exception)
            {

               
            }
             try
             {
                 Expression expression25 = (Expression)workPart.Expressions.FindObject("BAG_TRAS");
                 string bag_tras_value = "\"" + BAG_TRAS + "\"";
                 workPart.Expressions.EditWithUnits(expression25, null, bag_tras_value);
             }
             catch (Exception)
             {


             }
            
            theSession.Preferences.Modeling.UpdatePending = false;
            theSession.Preferences.Modeling.UpdatePending = true;


            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Expression Data");

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId3);
            theSession.DeleteUndoMark(markId3, "Update Expression Data");

            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

           

            lbl_proccess.Text = "Casulo configurado";
            btn_add_port.Enabled = true;
            MessageBox.Show("Configurações aplicadas");
        }

        private void frm_Configurador_S4_Load(object sender, EventArgs e)
        {

        }

        private void btn_configurar_teto_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            frm_Configurador_Teto frm = new frm_Configurador_Teto();
            frm.Modelo_AC = MODELO_AC;
            frm.carroceria = "";
            frm.norma = "";
            frm.Show();
        }
        public void codificar_pecas_lat_dir()
        {
            lbl_proccess.Text = "Codificando itens da Lateral Direita";
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_LAT_LD_S4/000 1");
            PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

            workPart = theSession.Parts.Work;
            partLoadStatus1.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            //NXOpen.Session.UndoMarkId markId2;
            //markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            //NXOpen.Assemblies.ReplaceComponentBuilder replaceComponentBuilder1;
            //replaceComponentBuilder1 = workPart.AssemblyManager.CreateReplaceComponentBuilder();

            //replaceComponentBuilder1.ComponentNameType = NXOpen.Assemblies.ReplaceComponentBuilder.ComponentNameOption.AsSpecified;

            //replaceComponentBuilder1.ComponentName = "000317";

            //theSession.SetUndoMarkName(markId2, "Replace Component Dialog");

            //replaceComponentBuilder1.ComponentName = "";

            //NXOpen.Assemblies.Component component2 = (NXOpen.Assemblies.Component)component1.FindObject("COMPONENT TPL_TEMINAL_SAIA/A 1");
            //bool added1;
            //added1 = replaceComponentBuilder1.ComponentsToReplace.Add(component2);

            //NXOpen.Session.UndoMarkId markId3;
            //markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Replace Component");

            //theSession.DeleteUndoMark(markId3, null);

            //NXOpen.Session.UndoMarkId markId4;
            //markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Replace Component");

            //Expression COMP_TERMINAL_DA_SAIA = (Expression)workPart.Expressions.FindObject("COMP_TERMINAL_DA_SAIA");

            //string codigo = "";

            //string FAM = "000333";

            //string[] expressao = { "COMP_SAIA" };

            //double[] valor = { COMP_TERMINAL_DA_SAIA.Value };
            //double[] tolerancia = { 1, 1 };
            //string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");
            //string[] quebra = arquivo.Split('/');
            //codigo = quebra[0];

            //replaceComponentBuilder1.ReplacementPart = "@DB/" + codigo + "/A"; ;

            //replaceComponentBuilder1.SetComponentReferenceSetType(NXOpen.Assemblies.ReplaceComponentBuilder.ComponentReferenceSet.Maintain, null);

            //PartLoadStatus partLoadStatus2;
            //partLoadStatus2 = replaceComponentBuilder1.RegisterReplacePartLoadStatus();

            //NXObject nXObject1;
            //nXObject1 = replaceComponentBuilder1.Commit();

            //partLoadStatus2.Dispose();
            //theSession.DeleteUndoMark(markId4, null);


            //replaceComponentBuilder1.Destroy();

            //NXOpen.Session.UndoMarkId markId5;
            //markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            //NXOpen.Assemblies.Component nullAssemblies_Component = null;
            //PartLoadStatus partLoadStatus3;
            //theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus3);

            //workPart = theSession.Parts.Work;
            //partLoadStatus3.Dispose();
            //theSession.SetUndoMarkName(markId5, "Make Work Part");

        }
        private void btn_codificar_Click(object sender, EventArgs e)
        {
           // codificar_pecas_lat_dir();
            MessageBox.Show("Substituindo e criando os novos itens","INFORMAÇÕES", MessageBoxButtons.OK, MessageBoxIcon.Information );
        }

        private void btn_add_port_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            Session theSession = Session.GetSession();
            Part wp = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();

            int ini, fim, ini1, fim1;

            
            NXOpen.Assemblies.Component cmp1 = wp.ComponentAssembly.RootComponent;
            item_master = cmp1.DisplayName.Split('/');

            Componentes.Add(cmp1);

          
            ini1 = 0;
            fim1 = 1;

            Componentes.ToArray()[0].GetChildren();

            string codigo_item = "";
            string total_itens = "";

            bool flag = true;

            while (flag)
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
                        fim1++;
                        string name = Convert.ToString(cmp.Name);
                        string cod = Convert.ToString(cmp.DisplayName);
                        int index_item = 0;
                        codigo_item = cod;

                        if (cmp.Name.Contains("TPL"))
                        {
                            if (Itens_Master.Contains(cmp.DisplayName + ";New"))
                            {

                            }
                            {
                                Itens_Master.Add(cmp.DisplayName + ";New");
                            }
                        }
            

                    }
                }
                flag = false;


            }


            string itens_novos = "";

            //frm_informacao_itens abrirfrm = new frm_informacao_itens();

            //abrirfrm.Propriedade = Itens_Master;
            //abrirfrm.ShowDialog();

           // MessageBox.Show("RELACAO DE ITENS NOVOS \n" + itens_novos, "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);



            //NXOpen.Assemblies.Component nullAssemblies_Component2 = null;
            //PartLoadStatus partLoadStatus3;
            //theSession.Parts.SetWorkComponent(nullAssemblies_Component2, out partLoadStatus3);

                wp = theSession.Parts.Work;
            //   // partLoadStatus3.Dispose();

                verficar_itens_abaixo();

                frm_informacao_itens abrirfrm = new frm_informacao_itens();

                abrirfrm.item_filho = Itens_Filhos;
                abrirfrm.item_master = Itens_Master;

                abrirfrm.ShowDialog();
                this.WindowState = FormWindowState.Normal;
            }

        public void verficar_itens_abaixo()
    {

     
     //   MessageBox.Show(Convert.ToString(Itens_Master.Count));
        for (int k = 0; k <= Itens_Master.Count-1; k++)
        {

           
            string[] quebra = Itens_Master[k].Split(';');

                
                Session theSession = Session.GetSession();
                Part workPart = theSession.Parts.Work;
                Part displayPart = theSession.Parts.Display;
                NXOpen.Session.UndoMarkId markId1;
                markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");
                NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT " + quebra[0] + " 1");
                PartLoadStatus partLoadStatus1;
                theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

                workPart = theSession.Parts.Work;
              partLoadStatus1.Dispose();

             Itens_Filhos.Add("   ;");
             Itens_Filhos.Add("ITEM PAI --" + quebra[0] + ";");

            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();

            int ini, fim, ini1, fim1;


            NXOpen.Assemblies.Component cmp1 = workPart.ComponentAssembly.RootComponent;
            item_master = cmp1.DisplayName.Split('/');

            Componentes.Add(cmp1);

          
            ini1 = 0;
            fim1 = 1;

            Componentes.ToArray()[0].GetChildren();

            string codigo_item = "";
            string total_itens = "";

            bool flag = true;

            while (flag)
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
                        fim1++;
                        string name = Convert.ToString(cmp.Name);
                        string cod = Convert.ToString(cmp.DisplayName);
                        int index_item = 0;
                        codigo_item = cod;

                        if (cmp.Name.Contains("TPL"))
                        {
                            if (Itens_Filhos.Contains(cmp.Name))
                            {

                            }
                            else
                            {
                                if (cmp.Name.Contains("222284") ||cmp.Name.Contains("222796") )
                                {
                                    
                                    Expression expression1 = (Expression)workPart.Expressions.FindObject("COMP_" + cmp.Name);
                                    string Valor_Busca = Convert.ToString(expression1.Value);

                                    string[] verifica_fam = cmp.Name.Split('_');
                                    string cod_fam = verifica_fam[1];
                                    //MessageBox.Show(cod_fam + " acessando....");
                                    double var_A = Convert.ToDouble(Valor_Busca);
                                    

                                    string Material = "cmp.Name";
                                    string arquivo = "";

                                   
                                    string DataSul = "";
                                  
                                    string[] expressao = { "COMP"};
                                    string descricao = "";

                                    double[] valor = { var_A };
                                    double[] tolerancia = { 5, 5};

                                    arquivo = Custom_Mascarello.Search_Chapas.SearchMember(cod_fam, descricao, expressao, valor, 0, tolerancia, DataSul, descricao);
                                    if (arquivo == "")
                                    {

                                        Itens_Filhos.Add(cmp.DisplayName + ";New");
                                    }
                                    if (arquivo != "")
                                    {
                                        Itens_Filhos.Add(cmp.DisplayName + ";"+ arquivo);
                                    }
                                }
                                else
                                {                             
                                         Itens_Filhos.Add(cmp.DisplayName + ";New");
                                }
                            }
                        }              
                    }

                }
                flag = false;


            }
            string itens_novos = "";

           

           // MessageBox.Show("RELACAO DE ITENS NOVOS \n" + itens_novos, "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);



            NXOpen.Assemblies.Component nullAssemblies_Component12 = null;
            PartLoadStatus partLoadStatus12;
            theSession.Parts.SetWorkComponent(nullAssemblies_Component12, out partLoadStatus12);
            workPart = theSession.Parts.Work;
            partLoadStatus12.Dispose();
        }

        
    }

        private void btn_aplicar_itens_Click(object sender, EventArgs e)
        {

        }

        private void btn_import_Click(object sender, EventArgs e)
        {
            dtg_view_dna.Rows.Clear();
            dtg_view_dna.Refresh();

            XmlDocument xmlBiblioteca = new XmlDocument();
            xmlBiblioteca.Load(_xml_opcao);
            XElement xml = XElement.Load(_xml_opcao);

            XmlNodeList lista_opcoes = default(XmlNodeList);

            lista_opcoes = xmlBiblioteca.SelectNodes("/Opcoes_micro_S4/Opcao");

            foreach (XmlNode opcao in lista_opcoes)
            {
                List_temp_opcao.Add(opcao.ChildNodes.Item(0).InnerText);

            }

            string path = @"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\NX\MICRO_S4\DNA\" + txt_fm_dna.Text + ".xml";
            DataSet dna_XML = new DataSet();
            dna_XML.ReadXml(path);
            string table = "DNA" + txt_fm_dna.Text;

            for (int i = 0; i <= List_temp_opcao.Count - 1; i++)
            {
                string Item = CultureInfo.CurrentCulture.TextInfo.ToUpper(List_temp_opcao[i].ToString());
                string Valor = dna_XML.Tables[table].Rows[0][List_temp_opcao[i]].ToString();
                string[] rowi = { Item, Valor };
                dtg_view_dna.Rows.Add(rowi);

            }

            CHASSI = dtg_view_dna.Rows[1].Cells[1].Value.ToString();
            SUSP = dtg_view_dna.Rows[2].Cells[1].Value.ToString();
            COMP = dtg_view_dna.Rows[3].Cells[1].Value.ToString();
            EE = dtg_view_dna.Rows[4].Cells[1].Value.ToString();
            WC = dtg_view_dna.Rows[9].Cells[1].Value.ToString();
            TIPO_DIV = dtg_view_dna.Rows[10].Cells[1].Value.ToString();
            QTD_ALCAPAO = dtg_view_dna.Rows[8].Cells[1].Value.ToString();
            MODELO_AC = dtg_view_dna.Rows[7].Cells[1].Value.ToString();
            MODELO_JAN = dtg_view_dna.Rows[13].Cells[1].Value.ToString();
            BAG_EE = dtg_view_dna.Rows[14].Cells[1].Value.ToString();

            busca_dados_chassi();


            //lbl_criacao.Text = "Dna Criado Por: " + CultureInfo.CurrentCulture.TextInfo.ToUpper(dna_XML.Tables[table].Rows[0]["CRIACAO"].ToString()) + "  Data: " + dna_XML.Tables[table].Rows[0]["DATACRIACAO"].ToString();
            // lbl_alteracao.Text = "Alterado Por: " + CultureInfo.CurrentCulture.TextInfo.ToUpper(dna_XML.Tables[table].Rows[0]["ALTERACAO"].ToString()) + "  Data: " + dna_XML.Tables[table].Rows[0]["DATAALTERACAO"].ToString() + "   " + dna_XML.Tables[table].Rows[0]["HORAALTERACAO"].ToString();

            List_temp_opcao.Clear();
        }

        private void btn_editar_Click(object sender, EventArgs e)
        {
            string item_pai = @"S:\\Cad.Minibuss\\CONFIGCAD_2014_MRP\\DLL\NX\\temp\\" + txt_fm_dna.Text;
            System.IO.StreamWriter vWriter = new System.IO.StreamWriter(item_pai, true);
            vWriter.Close();
            System.Diagnostics.Process.Start(@"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\APLICATIVOS\GERACAO_DNA_PLM\App_Geracao_DNA.exe");
        }

       

        private void btn_close_micro_s4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_criar_dna_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\APLICATIVOS\GERACAO_DNA_PLM\App_Geracao_DNA.exe");
        }

        private void btn_especificos_Click(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            foreach (Expression exp in workPart.Expressions)
            {
                if (exp.Name == "IT_CONFIGURADO")//IT_CONFIG
                {
                    this.WindowState = FormWindowState.Minimized;
                    frm_Especificos frm = new frm_Especificos();
                    frm.Show();
                }
            }
        }
    }
}
