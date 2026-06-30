using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NXOpen;
using NXOpen.UF;
using NXOpen.UIStyler;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Windows;
using System.Data;

namespace Custom_Mascarello
{
    class Codificar
    {
        private static Session theSession;
        private static UI theUI;
        private static UFSession theUfSession;
        public static List<String> Lista_itens = new List<String>();
        public static List<String> Lista_itens_attr = new List<String>();
        public static List<String> Lista_itens_qtd = new List<String>();
        public static List<String> Lista_replace = new List<String>();

        public static string Codificar_item(string abrir)
        {

            Lista_itens.Clear();
            Lista_itens_attr.Clear();
            Lista_itens_qtd.Clear();
            Lista_replace.Clear();
            string arq_ret = "";


            theSession = Session.GetSession();
            Part wp_m = theSession.Parts.Work;





            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();

            NXOpen.Assemblies.Component cmp = wp_m.ComponentAssembly.RootComponent;

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

                    
                    NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)(wp_m.ComponentAssembly.RootComponent.FindObject("COMPONENT " + cod + " " + total_itens)));
                    objects1[0] = component1;
                    AttributePropertiesBuilder attributePropertiesBuilder1;

                    NXObject.AttributeInformation[] attr_contra = component1.GetUserAttributes();
                    string cod_tpl = "";
                    if (!component1.IsSuppressed)
                        foreach (var item in attr_contra)
                        {
                            if (item.Title == "DB_PART_NO")
                            {
                                if (item.StringValue.Contains("TPL"))
                                {
                                    cod_tpl = item.StringValue;

                                    if (!Lista_itens.Contains(cod + ">" + total_itens))
                                    {

                                        Lista_itens.Add(cod + ">" + total_itens);
                                    }
                                }
                            }                          
                        }
                }
            }
            for (int i = 0; i <= Lista_itens.Count - 1; i++)
            {
              //  System.Windows.Forms.MessageBox.Show(Lista_itens[i]);
                XmlDocument xml_attr = new XmlDocument();
                xml_attr.Load(@"X:\\Xml\\xml_atributos_fam.xml");
                XmlNodeList Lista_attr = default(XmlNodeList);



                Lista_attr = xml_attr.SelectNodes("/Atributos_fam/attr");

                string fam = Lista_itens[i].Substring(4,6);

             

                string attributos = "";
                foreach (XmlNode atrr in Lista_attr)
                {
                        
                    string fam_attr = atrr.ChildNodes.Item(0).InnerText;
                    if (fam_attr == fam)
                    {
                        attributos = atrr.ChildNodes.Item(1).InnerText;
                       
                    }
                }
                string[] quebra = Lista_itens[i].Split('>');


                string codigo_attr = quebra[0];
                string total_itens_attr = quebra[1];

                NXObject[] objects2 = new NXObject[1];


                NXOpen.Assemblies.Component component2 = ((NXOpen.Assemblies.Component)(wp_m.ComponentAssembly.RootComponent.FindObject("COMPONENT " + codigo_attr + " " + total_itens_attr)));
                objects2[0] = component2;


                NXObject.AttributeInformation[] attr = component2.GetUserAttributes();
                string temp_attr = "";
                temp_attr = codigo_attr;
             
                string[] quebra_attrs = attributos.Split(';');
                for (int j = 0; j < quebra_attrs.Length; j++)
                {
                   
                    foreach (var item in attr)
                    {
                        if (item.Title == quebra_attrs[j].ToString())
                        {
                            temp_attr += ";" + Math.Round(item.RealValue,1).ToString();
                          //  System.Windows.Forms.MessageBox.Show(temp_attr);

                        }
                    }


                }

                
                Lista_itens_attr.Add(temp_attr);
                Lista_itens_qtd.Add(total_itens_attr);
                temp_attr = "";       
            }

            string result = "";
            for (int i = 0; i <= Lista_itens_attr.Count - 1; i++)
            {
                XmlDocument xml_attr = new XmlDocument();
                xml_attr.Load(@"X:\\Xml\\xml_atributos_fam.xml");
                XmlNodeList Lista_attr = default(XmlNodeList);

              

                Lista_attr = xml_attr.SelectNodes("/Atributos_fam/attr");
               
                string cod_fam = Lista_itens_attr[i].Substring(4,6);
                string categoria = "";
                string desc = "";
                string atributos_fam = "";
                foreach (XmlNode atrr in Lista_attr)
                {

                    string fam_attr = atrr.ChildNodes.Item(0).InnerText;
                    if (fam_attr == cod_fam)
                    {
                        atributos_fam = atrr.ChildNodes.Item(1).InnerText;
                        categoria = atrr.ChildNodes.Item(2).InnerText;
                        desc = atrr.ChildNodes.Item(3).InnerText;
                        try
                        {
                            desc += ";"+atrr.ChildNodes.Item(4).InnerText;
                        }
                        catch
                        {                           
                           
                        }
                        try
                        {
                            desc += ";" + atrr.ChildNodes.Item(5).InnerText;
                        }
                        catch 
                        {
                           
                        }
                        try
                        {
                            desc += ";" + atrr.ChildNodes.Item(6).InnerText;
                        }
                        catch 
                        {
                            
                        }
                        try
                        {
                            desc += ";" + atrr.ChildNodes.Item(7).InnerText;
                        }
                        catch
                        {


                        }
                    }
                }

                
                string dados_item = Lista_itens_attr[i];
               // System.Windows.Forms.MessageBox.Show(dados_item, atributos_fam);
                string cod_new = "";
                if(categoria == "TUBO")
                {
                     cod_new = Create_fam_tubos(dados_item, atributos_fam);
                     string []quebra_tpl = Lista_itens_attr[i].Split(';');
                     Lista_replace.Add(quebra_tpl[0] + ";" + Lista_itens_qtd[i] + ";" + cod_new);
                }
                if (categoria == "GERAL")
                {
                    cod_new = Create_fam(dados_item, atributos_fam, desc);
                    string[] quebra_tpl = Lista_itens_attr[i].Split(';');
                    Lista_replace.Add(quebra_tpl[0] + ";" + Lista_itens_qtd[i] + ";" + cod_new);
                }
                
                cod_new = "";
                
            }

            string substituir = Substituir("");
            return arq_ret;
        }
        public static string Create_fam_tubos(string dados, string atributos)
        {

          
            List<string> gerar_tubo_cv_fam_cv = new List<string>();
            List<string> gerar_tubo_cv_fam_item = new List<string>();

            
            gerar_tubo_cv_fam_cv.Clear();
            gerar_tubo_cv_fam_item.Clear();

            DataSet dadosXML = new DataSet();
            // referencia ao arquivo XML            
            dadosXML.ReadXml(@"X:\Xml\Lista_Familia\Lista_Familias.xml");

                foreach (DataRow dRow in dadosXML.Tables["Tubos_1"].Rows)
                {              
                    gerar_tubo_cv_fam_cv.Add(dRow["Fam_cv"].ToString());
                    gerar_tubo_cv_fam_item.Add(dRow["Fam_item"].ToString());
                }
          
                string codigo_novo = "";
            string cod_fam = dados.Substring(4, 6);
            string cod_fam_desc = "";
            cod_fam_desc = cod_fam;
            string[] quebra_dados = dados.Split(';');
            double Comprimento = Convert.ToDouble(quebra_dados[1]);
            double ANG_A_H = Convert.ToDouble(quebra_dados[2]);
            double ANG_A_V = Convert.ToDouble(quebra_dados[3]);
            double ANG_B_H = Convert.ToDouble(quebra_dados[4]);
            double ANG_B_V = Convert.ToDouble(quebra_dados[5]);

            string desc_1 = "";
            string desc_2 = "";
            string desc_3 = "";
            string desc_4 = "";

            bool cnc = false;

            if (((ANG_A_H >= 0 || ANG_A_H <= 0) && ANG_A_H <= 68 && (ANG_B_H >= 0 || ANG_B_H <= 0) && ANG_B_H <= 68 && ANG_A_V == 0 && ANG_B_V == 0 && Comprimento <= 3000) ||
                 ((ANG_A_V >= 0 || ANG_A_V <= 0) && ANG_A_V <= 68 && (ANG_B_V >= 0 || ANG_B_V <= 0) && ANG_B_V <= 68 && ANG_A_H == 0 && ANG_B_H == 0 && Comprimento <= 3000))
            {
                cnc = true;

                for (int i = 0; i <= gerar_tubo_cv_fam_item.Count - 1; i++)
                {
                    if (gerar_tubo_cv_fam_item[i] == cod_fam)
                    {
                        cod_fam = gerar_tubo_cv_fam_cv[i];                   
                    }
                }
            }
           
            desc_1 = "_AV" + ANG_A_V.ToString() + "°";
            desc_2 = "_BV" + ANG_B_V.ToString() + "°";
            desc_3 = "_AH" + ANG_A_H.ToString() + "°";
            desc_4 = "_BH" + ANG_B_H.ToString() + "°";
            string Material = "";
            string busca_descricao = Custom_Mascarello.Search_Material_Tubos.Busca_Descricao(cod_fam_desc);

            string[] expressao = { "COMP", "ANG_A_V", "ANG_B_V", "ANG_A_H", "ANG_B_H" };
            string descricao = busca_descricao + "x" + Comprimento.ToString() + "mm" + desc_1 + desc_2 + desc_3 + desc_4;

            double tol_comp = Convert.ToDouble(0.1);
            double tol_av = Convert.ToDouble(0.1);
            double tol_bv = Convert.ToDouble(0.1);
            double tol_ah = Convert.ToDouble(0.1);
            double tol_bh = Convert.ToDouble(0.1);

            double[] valor = { Comprimento, ANG_A_V, ANG_B_V, ANG_A_H, ANG_B_H };
            double[] tolerancia = { tol_comp, tol_av, tol_bv, tol_ah, tol_bh };

            string codigo = "";

            if (cnc == false)
            {
                Material = Custom_Mascarello.Search_Material_Tubos.Busca_Material(cod_fam, Comprimento.ToString());
                codigo_novo = Custom_Mascarello.Search_Tubos.SearchMember(cod_fam, descricao, expressao, valor, 0, tolerancia, "", descricao, Material);
            }
            if (cnc == true)
            {
               
                descricao = "CV_" + descricao;
                codigo_novo = Custom_Mascarello.Search_Tubos_cnc.SearchMember(cod_fam, descricao, expressao, valor, 0, tolerancia, "", descricao);
            }

            codigo_novo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

            if (codigo_novo == "")
            {

                if (cnc == false)
                {

                    codigo_novo = Custom_Mascarello.Create_Tubos.CreateMember(cod_fam, descricao, expressao, valor, 0, tolerancia, codigo, descricao, Material);
                }
                if (cnc == true)
                {

                    codigo_novo = Custom_Mascarello.Create_Tubos_cnc.CreateMember(cod_fam, descricao, expressao, valor, 0, tolerancia, "", descricao);

                }

            }
            else
            {

                
            }
             Comprimento = 0;
             ANG_A_H = 0;
             ANG_A_V = 0;
             ANG_B_H = 0;
             ANG_B_V = 0;
          
            return codigo_novo;
        }
        public static string Create_fam(string dados, string atributos, string desc)
        {
            string codigo_novo = "";

            string cod_fam = dados.Substring(4, 6);
 
            string[] quebra_dados = dados.Split(';');
             string[] quebra_atributos = atributos.Split(';');
           //System.Windows.MessageBox.Show(dados);
           //System.Windows.MessageBox.Show(atributos);
            string[] expressao = new string[quebra_atributos.Length];
            for (int i = 0; i <= quebra_atributos.Length-1; i++)
			{
			 expressao[i] = quebra_atributos[i];
			}

            double[] valor = new double[quebra_atributos.Length];
             for (int i = 0; i <= quebra_atributos.Length-1; i++)
             {
                 valor[i] = Convert.ToDouble(quebra_dados[i+1]);
             }
             double[] tolerancia = new double[quebra_dados.Length];
            for (int i = 1; i <= quebra_atributos.Length - 1; i++)
            {
                tolerancia[i] = Convert.ToDouble(0.1);
            }
            string codigo = "";
            string[] desc_item = desc.Split(';');
            string descricao = desc_item[0] + quebra_dados[1]+ desc_item[1];
            
            if (quebra_dados.Length >2)
            {
                for (int i = 3; i <= desc_item.Length; i++)
                {            
                    descricao += desc_item[i-1] + "_" + quebra_dados[i-1];
                }
            }
            codigo_novo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");
        
            if (codigo_novo == "")
            {
                codigo_novo = Custom_Mascarello.Create.CreateMember(cod_fam, descricao, expressao, valor, 0, tolerancia, codigo, descricao);
            }
            return codigo_novo;
        }
        public static string Substituir(string sust)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            //for (int i = 0; i <= Lista_replace.Count - 1; i++)
            //{
            //    System.Windows.Forms.MessageBox.Show(Lista_replace[i]);
            //}
            for (int i = 0; i <= Lista_replace.Count - 1; i++)
            {

                string[] quebra = Lista_replace[i].Split(';');

                string tpl_subst = quebra[0];
                string tpl_qtd = quebra[1];
                string new_arqivo = quebra[2];

                NXOpen.Session.UndoMarkId markId1;
                markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

                NXOpen.Assemblies.ReplaceComponentBuilder replaceComponentBuilder1;
                replaceComponentBuilder1 = workPart.AssemblyManager.CreateReplaceComponentBuilder();

                replaceComponentBuilder1.ReplaceAllOccurrences = true;

                replaceComponentBuilder1.ComponentNameType = NXOpen.Assemblies.ReplaceComponentBuilder.ComponentNameOption.AsSpecified;

                replaceComponentBuilder1.ComponentName = "212970";

                theSession.SetUndoMarkName(markId1, "Replace Component Dialog");

                replaceComponentBuilder1.ComponentName = "";
                
                NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT " + tpl_subst + " "+tpl_qtd);
                bool added1;
                added1 = replaceComponentBuilder1.ComponentsToReplace.Add(component1);

                replaceComponentBuilder1.ComponentName = tpl_subst;

                replaceComponentBuilder1.ComponentName = new_arqivo;

                NXOpen.Session.UndoMarkId markId2;
                markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Replace Component");

                theSession.DeleteUndoMark(markId2, null);

                NXOpen.Session.UndoMarkId markId3;
                markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Replace Component");
               
                replaceComponentBuilder1.ReplacementPart = "@DB/" + new_arqivo;

                replaceComponentBuilder1.SetComponentReferenceSetType(NXOpen.Assemblies.ReplaceComponentBuilder.ComponentReferenceSet.Maintain, null);

                PartLoadStatus partLoadStatus1;
                partLoadStatus1 = replaceComponentBuilder1.RegisterReplacePartLoadStatus();

                NXObject nXObject1;
                nXObject1 = replaceComponentBuilder1.Commit();

                partLoadStatus1.Dispose();
                theSession.DeleteUndoMark(markId3, null);

                theSession.SetUndoMarkName(markId1, "Replace Component");

                replaceComponentBuilder1.Destroy();
               
            }
            return "";
        }          
        }
    }

