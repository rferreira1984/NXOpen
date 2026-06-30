using Custom_Mascarello;
using NXOpen;
using NXOpen.UF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NXOpen.CAE.UnifiedDatabaseOptions;


namespace ConjuntoX
{
    public class metodosX
    {
        public static void aplicar_config(string _dims, string larg, string alt, string folga)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;


            Part part1 = null;
            try
            {
                part1 = (Part)theSession.Parts.FindObject("@DB/TPL_CJ_X/000");
                if (part1 != null)
                {
                    //NXMessageBox.Show("Part encontrada no display. Exibindo part.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //PartLoadStatus loadStatus;
                    //theSession.Parts.SetDisplay(part1, true, true, out loadStatus);

                }


            }
            catch
            {

                //MessageBox.Show("Part não encontrada no display. Abrindo part para exibição.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (part1 == null)
            {
                BasePart basePart1;
                PartLoadStatus partLoadStatus1;
                basePart1 = theSession.Parts.OpenBaseDisplay("@DB/TPL_CJ_X/000", out partLoadStatus1);
                part1 = (Part)basePart1;
            }

            // preencher expression 
            workPart = theSession.Parts.Work;
            string[] dims = _dims.Split('x');

            string altura = dims[0].Trim();
            string largura = dims[1].Trim();
            string espessura = dims[2].Trim();


            Custom_Mascarello.global_class.preenche_expression("VAO_LARGURA", larg, part1);
            Custom_Mascarello.global_class.preenche_expression("VAO_ALTURA", alt, part1);
            Custom_Mascarello.global_class.preenche_expression("FOLGA", folga, part1);

            Custom_Mascarello.global_class.preenche_expression("ESP", espessura.Replace(",", "."), part1);
            Custom_Mascarello.global_class.preenche_expression("TUBO_ALTURA", altura.Replace(",", "."), part1);
            Custom_Mascarello.global_class.preenche_expression("TUBO_LARGURA", largura.Replace(",", "."), part1);

            NXOpen.Assemblies.Component[] cmp = part1.ComponentAssembly.RootComponent.GetChildren();
            foreach (NXOpen.Assemblies.Component comp in cmp)
            {
                NXOpen.PartLoadStatus partLoadStatus;
                NXOpen.Assemblies.ComponentAssembly.OpenComponentStatus[] openStatus1;
                NXOpen.Assemblies.Component[] componentsToOpen1 = new NXOpen.Assemblies.Component[1];
                componentsToOpen1[0] = comp;
                partLoadStatus = part1.ComponentAssembly.OpenComponents(NXOpen.Assemblies.ComponentAssembly.OpenOption.ComponentOnly, componentsToOpen1, out openStatus1);

                partLoadStatus.Dispose();

                NXOpen.PartLoadStatus partLoadStatus2;
                theSession.Parts.SetWorkComponent(comp, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus2);

                workPart = theSession.Parts.Work; // CV018119/000;1-TP_CV1_CJ_X
                partLoadStatus2.Dispose();

                NXOpen.Session.UndoMarkId markId6;
                markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

                NXOpen.Assemblies.Component nullNXOpen_Assemblies_Component = null;
                NXOpen.PartLoadStatus partLoadStatus3;
                theSession.Parts.SetWorkComponent(nullNXOpen_Assemblies_Component, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus3);

                workPart = theSession.Parts.Work; // TPL_CJ_X/000;1
                partLoadStatus3.Dispose();
                theSession.SetUndoMarkName(markId6, "Make Work Part");

            }

        }
        public static string criarX(string _dims, string larg, string alt, string folga, bool invert)
        {
            Session theSession = Session.GetSession();

            string[] dims = _dims.Split('x');
            string _dim_larg = dims[0];
            string _dim_alt = dims[1];
            string _espessura = dims[2];

            if (invert == true)
            {
                _dim_larg = dims[1];
                _dim_alt = dims[0];
                _espessura = dims[2];
            }

            Part displayAnterior = theSession.Parts.Display;
            Part wp_pai = theSession.Parts.Work;
            Part wp = theSession.Parts.Work;
            NXOpen.Assemblies.Component[] compts = new NXOpen.Assemblies.Component[2];
            //NXOpen.Assemblies.Component component2 = (NXOpen.Assemblies.Component)wp.ComponentAssembly.RootComponent.FindObject("COMPONENT " + "CV018120/000" + " " + "2");
            compts[0] = (NXOpen.Assemblies.Component)wp.ComponentAssembly.RootComponent.FindObject("COMPONENT " + "CV018119/000" + " " + "1");
            compts[1] = (NXOpen.Assemblies.Component)wp.ComponentAssembly.RootComponent.FindObject("COMPONENT " + "CV018120/000" + " " + "2");

            string familias = "";
            string desc_padrao = "";

            Dictionary<string, Dictionary<string, double>> dic = new Dictionary<string, Dictionary<string, double>>();

            foreach (NXOpen.Assemblies.Component item in compts)
            {


                PartLoadStatus partLoadStatus2;

                theSession.Parts.SetWorkComponent(item, out partLoadStatus2);


                wp = theSession.Parts.Work;


                Expression expression2 = (Expression)wp.Expressions.FindObject("COMP");
                Expression expression3 = (Expression)wp.Expressions.FindObject("ANG_A_V");
                Expression expression4 = (Expression)wp.Expressions.FindObject("ANG_B_V");
                Expression expression8 = (Expression)wp.Expressions.FindObject("ANG_A_H");
                Expression expression9 = (Expression)wp.Expressions.FindObject("ANG_B_H");

                double comp = Convert.ToDouble(expression2.Value);
                double ang_a_v = Convert.ToDouble(expression3.Value);
                double ang_b_v = Convert.ToDouble(expression4.Value);
                double ang_a_h = Convert.ToDouble(expression8.Value);
                double ang_b_h = Convert.ToDouble(expression9.Value);

                Dictionary<string, double> atributos = new Dictionary<string, double>();
                atributos.Add("COMP", comp);
                atributos.Add("ANG_A_V", ang_a_v);
                atributos.Add("ANG_B_V", ang_b_v);
                atributos.Add("ANG_A_H", ang_a_h);
                atributos.Add("ANG_B_H", ang_b_h);

                dic.Add(item.DisplayName, atributos);


                //  LogMessage_x($"COMP: {comp} | ANG_A_V: {ang_a_v} | ANG_B_V: {ang_b_v} | ANG_A_H: {ang_a_h} | ANG_B_H: {ang_b_h}");

                familias = "FAM_CV_" + _dims;

                DataSet dados_fam = new DataSet();
                Custom_Mascarello.conexao_bd conn = new Custom_Mascarello.conexao_bd();
                dados_fam = conn.busca_fam_tubos(familias);

                foreach (DataRow dRow in dados_fam.Tables["tab_fam_nx"].Rows)
                {

                    desc_padrao = dRow["descricao_padrao"].ToString();
                }

                // itens_replace.Add(item, "");
            }
            theSession.Parts.SetWork(wp_pai);


            Dictionary<NXOpen.Assemblies.Component, string> itens_replace = new Dictionary<NXOpen.Assemblies.Component, string>();
            foreach (var item2 in dic)
            {
                //LogMessage_x($"Item: {item2.Key}");
                foreach (var att in item2.Value)
                {
                    // string cv = Create_fam_cv_2312_x(familias, desc_padrao, item2.Value, displayAnterior);
                    // LogMessage_x($"Atributo: {att.Key} - Valor: {att.Value}");

                }
                string cv = global_class.Create_fam_cv_2312_x(familias, desc_padrao, item2.Value, displayAnterior);

                foreach (var item in compts)
                {
                    if (item.DisplayName == item2.Key)
                    {
                        itens_replace.Add(item, cv);
                    }

                }
            }
            foreach (var item in itens_replace)
            {
                //LogMessage_x($"Item para substituir: {item.Key.DisplayName} : novo item{item.Value}");
            }




            foreach (var item in itens_replace)
            {
                NXOpen.Assemblies.ReplaceComponentBuilder replaceComponentBuilder1;
                replaceComponentBuilder1 = wp_pai.AssemblyManager.CreateReplaceComponentBuilder();

                replaceComponentBuilder1.ReplaceAllOccurrences = true;

                replaceComponentBuilder1.ComponentNameType = NXOpen.Assemblies.ReplaceComponentBuilder.ComponentNameOption.AsSpecified;
                //LogMessage_x($"Substituindo item: {item.Key.DisplayName} por {item.Value}");
                string _item = item.Value;
                if (_item.Contains("/000"))
                {
                    _item = _item.Remove(_item.Length - 4, 4);
                }
                bool added1;
                added1 = replaceComponentBuilder1.ComponentsToReplace.Add(item.Key);

                try
                {
                    // LogMessage_x($"Part @DB/{_item}/000 encontrada para substituição.");
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + _item + "/000");

                }
                catch
                {
                    // LogMessage_x($"Part @DB/{_item}/000 não encontrada no display. Tentando abrir para substituição.");
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + _item + "/000");

                }

                try
                {
                    replaceComponentBuilder1.ReplacementPart = "@DB/" + _item + "/000";
                    //LogMessage_x($"Part @DB/{_item}/000 definida para substituição.");
                }
                catch
                {
                    replaceComponentBuilder1.ReplacementPart = "@DB/" + _item + "/000";
                    //LogMessage_x($"Part @DB/{_item}/000 não encontrada para substituição. Verifique se o item foi criado corretamente.");
                }

                replaceComponentBuilder1.SetComponentReferenceSetType(NXOpen.Assemblies.ReplaceComponentBuilder.ComponentReferenceSet.Maintain, null);
                replaceComponentBuilder1.Commit();

            }
            // agora vamos fazer um saveas do item pai para um novo item    
            // gere codigo
            // saveas_x("teste_saveas");
            //wp_pai.SaveAs("@DB/" + "teste_saveas" + "/000");
            string _dims_final = $"{ _dim_larg}x{_dim_alt}x{_espessura}";
            string name = $"CJ_X_TB_{_dims.Replace(" ", "")}_VAO_{alt}x{larg}mm";
            string id_new = global_class.saveas_x(name);


            //if (id_new != null)
            //{
            //   MessageBox.Show($"Item salvo com sucesso: {id_new} - {name}");
            //}
          
            conexao_bd conn_bd = new conexao_bd();
            
            conn_bd.insert_cj_x(id_new, alt, larg, _dim_larg, _dim_alt, _espessura.Replace(",", "."), folga);

            atualizar_drafting(id_new, wp_pai);
            return id_new;


            //  wp_pai.Save();

        }
        public static void teste(string _dims, string larg, string alt, string folga, bool invert)
        {
            string[] dims = _dims.Split('x');
            string _dim_larg = dims[0];
            string _dim_alt = dims[1];
            string _espessura = dims[2];

            if (invert == true)
            {
                _dim_larg = dims[1];
                _dim_alt = dims[0];
                _espessura = dims[2];
            }
            conexao_bd conn_bd = new conexao_bd();
            MessageBox.Show($"Altura: {alt} | Largura: {larg} | Dim_Larg: {_dim_larg} | Dim_Alt: {_dim_alt} | Espessura: {_espessura.Replace(",", ".")} | Folga: {folga}");
            conn_bd.insert_cj_x("9999999", alt, larg, _dim_larg, _dim_alt, _espessura.Replace(",", "."), folga);
        }

        public static void atualizar_drafting(string item, Part part_atual)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;

            NXOpen.PartCloseResponses partCloseResponses1;
            partCloseResponses1 = theSession.Parts.NewPartCloseResponses();
            // ----------------------------------------------
            //   Menu: File->Open...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            theSession.SetUndoMarkName(markId1, "Open Part File Dialog");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

            theSession.DeleteUndoMark(markId3, null);

            theSession.SetUndoMarkName(markId1, "Open Part File");

            theSession.DeleteUndoMark(markId1, null);

            theSession.Parts.SetNonmasterSeedPartData($"@DB/{item}/000/specification/{item}-000-dwg1");

            NXOpen.BasePart basePart1;
            NXOpen.PartLoadStatus partLoadStatus1;
            basePart1 = theSession.Parts.OpenActiveDisplay($"@DB/{item}/000/specification/{item}-000-dwg1", NXOpen.DisplayPartOption.AllowAdditional, out partLoadStatus1);

            workPart = theSession.Parts.Work; 
            displayPart = theSession.Parts.Display; 
            partLoadStatus1.Dispose();
            theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");

            workPart.Drafting.EnterDraftingApplication();

            // ----------------------------------------------
            //   Menu: Application->Document->PMI
            // ----------------------------------------------
            workPart.Views.WorkView.UpdateCustomSymbols();

            workPart.Drafting.SetTemplateInstantiationIsComplete(true);

            theSession.CleanUpFacetedFacesAndEdges();

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Views");

            NXOpen.NXObject[] inputviews1 = new NXOpen.NXObject[1];
            inputviews1[0] = workPart.Views.WorkView;
            workPart.DraftingViews.UpdateSheetsAndViews(inputviews1);

            // ----------------------------------------------
            //   Menu: Edit->Table->Update Parts List
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Parts List");

            NXOpen.Annotations.PartsList partsList1 = ((NXOpen.Annotations.PartsList)workPart.Annotations.Tables.FindObject("HANDLE R-351339"));
            partsList1.Update();

            NXOpen.Session.UndoMarkId markId6;
            markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Save");

            NXOpen.PDM.SmartSaveContext smartSaveContext1;
            smartSaveContext1 = theSession.PdmSession.CreateSmartSaveContext(NXOpen.PDM.SmartSaveBuilder.SaveType.SaveAndClose);

            NXOpen.PDM.SmartSaveBuilder smartSaveBuilder1;
            smartSaveBuilder1 = theSession.PdmSession.CreateSmartSaveBuilderWithContext(smartSaveContext1);

            NXOpen.PDM.SmartSaveObject[] smartsaveobjects1;
            smartSaveBuilder1.GetSmartSaveObjects(out smartsaveobjects1);

            NXOpen.NXObject[] objects1 = new NXOpen.NXObject[1];
            objects1[0] = smartsaveobjects1[0];
            NXOpen.ErrorList errorList1;
            errorList1 = smartSaveBuilder1.AutoAssignAttributes(objects1);

            smartSaveBuilder1.ValidateSmartSaveObjects();

            NXOpen.PDM.ErrorMessageHandler errorMessageHandler1;
            errorMessageHandler1 = smartSaveBuilder1.GetErrorMessageHandler(true);

            NXOpen.NXObject nXObject1;
            nXObject1 = smartSaveBuilder1.Commit();

            NXOpen.PDM.ErrorMessageHandler errorMessageHandler2;
            errorMessageHandler2 = smartSaveBuilder1.GetErrorMessageHandler(true);

            smartSaveBuilder1.Destroy();

            smartSaveContext1.Dispose();


            UFSession ufSession = UFSession.GetUFSession(); // Obtain the UFSession instance.  
            UFPart voltar_model = ufSession.Part;

            workPart.Close(NXOpen.BasePart.CloseWholeTree.False, NXOpen.BasePart.CloseModified.UseResponses, partCloseResponses1);



            voltar_model.SetDisplayPart(part_atual.Tag);

            

            theSession.ApplicationSwitchImmediate("UG_APP_MODELING");

        }
        public static void invert(string Arrangement, string valor)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;


            NXOpen.Assemblies.Arrangement[] arrangements1;
            workPart.ComponentAssembly.RootComponent.GetArrangements(out arrangements1);


            NXOpen.Assemblies.Arrangement arrangement1 = ((NXOpen.Assemblies.Arrangement)workPart.ComponentAssembly.Arrangements.FindObject(Arrangement));
            workPart.ComponentAssembly.RootComponent.SetUsedArrangement(arrangement1);


           Custom_Mascarello.global_class.preenche_expression("invert", valor, workPart);
        }

        public static DataSet busca_x(string _dims, string larg, string alt, string folga, bool invert)
        {

           

            string[] dims = _dims.Split('x');
            string _dim_larg = dims[0];
            string _dim_alt = dims[1];
            string _espessura = dims[2];

            if (invert == true)
            {
                _dim_larg = dims[1];
                _dim_alt = dims[0];
                _espessura = dims[2];
            }
          

            conexao_bd conn_bd = new conexao_bd();
            DataSet ds = conn_bd.busca_x(alt, larg, _dim_larg, _dim_alt, _espessura.Replace(",","."), folga);

            return ds;
           
        }


    }
}
