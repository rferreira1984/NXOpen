using System;
using NXOpen;
using NXOpen.UF;
using NXOpen.UIStyler;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using MySql.Data.MySqlClient;
using NXOpen.Features;

namespace Rotinas_Startup
{
    public class Program
    {
        // class members
        private static Session theSession;
        private static UI theUI;
        private static UFSession theUfSession;
        public static Program theProgram;
        public static bool isDisposeCalled;
        public static bool IsTc = false;
        public static string DAL_Default_Item_Type = "";

        public string ver_exp = "0";
        public static string fator = "";
        public string COD = "";

        //------------------------------------------------------------------------------
        // Constructor
        //------------------------------------------------------------------------------
        public Program()
        {
            try
            {
                theSession = Session.GetSession();
                theUI = UI.GetUI();
                theUfSession = UFSession.GetUFSession();
                isDisposeCalled = true;


            }
            catch (NXOpen.NXException ex)
            {
                // ---- Enter your exception handling code here -----
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }
        }

        //------------------------------------------------------------------------------
        //  Explicit Activation
        //      This entry point is used to activate the application explicitly
        //------------------------------------------------------------------------------
        public static void Main(string[] args)
        {

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            bool is_fam = IsPartFamilyInstance(workPart);
            try
            {
                if (is_fam == false)//Verifica blank de itens que não são de familia
                {
                    Verificar_Blank();
                }
                else
                {
                   /// MessageBox.Show("Item Familia");
                    Verificar_Blank_Fam();
                    item_fam();
                }
            }
            catch
            {

            }
            try
            {
                Remove_blank();
            }
            catch
            {

            }

            try
            {
                validar_cv();
            }
            catch 
            {

             
            }
            try
            {
                //  UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, "aaaaaa");
                bool result = Recriar_link_mp();
              
                if (result == false)
                {
                    recriar_link_mp_model_nx9();
                }// atualiza o quantidade de materia prima para itens templates// MP em conjuntos.
            }
            catch
            {

            }



        }
        public static void validar_cv()
        {
            Session theSession = Session.GetSession();
            Part wp = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            string CodRevisao_CV = "";
            foreach (NXOpen.Assemblies.Component comp in wp.ComponentAssembly.RootComponent.GetChildren())
            {
                NXOpen.NXObject[] objects = new NXOpen.NXObject[1];
                objects[0] = comp;
                NXObject.AttributeInformation[] stringAttrTitles = objects[0].GetAttributeTitlesByType(NXObject.AttributeType.String);

                foreach (NXObject.AttributeInformation attr in stringAttrTitles)
                {
                    if (attr.Title.Contains("DB_PART_NAME"))
                    {
                        string value = objects[0].GetStringAttribute(attr.Title);
                        if (value.Contains("CONTRAVENTAMENTOS"))
                        {
                            CodRevisao_CV = comp.DisplayName.Substring(0, 10).Replace('/', '-').PadLeft(12, '0');
                        }
                    }
                }
            }
            Dictionary<string, int> lista_txt_cv = new Dictionary<string, int>();
            Dictionary<string, int> lista_proj_cv = new Dictionary<string, int>();
            StringBuilder msg = new StringBuilder();
            string path_lista = @"S:\serra cnc\CORTE\" + CodRevisao_CV + ".txt";
            if (CodRevisao_CV != "")
            {
                if (CodRevisao_CV != "")
                {               
                    if (File.Exists(path_lista))
                    {
                        StreamReader sr = new StreamReader(path_lista);
                        using (sr)
                        {
                            string linha = "";
                            while ((linha = sr.ReadLine()) != null)
                            {
                                lista_txt_cv.Add(linha.Substring(12, 8), Convert.ToInt16(linha.Substring(57, 3).TrimStart()));
                            }
                        }
                    }
                    else
                    {
                        msg.Append($"Lista TXT para o código {CodRevisao_CV} não encontrada.");
                    }
                }
            }
            foreach (NXOpen.Assemblies.Component comp in wp.ComponentAssembly.RootComponent.GetChildren())
            {
                NXOpen.NXObject[] objects = new NXOpen.NXObject[1];
                objects[0] = comp;
                NXObject.AttributeInformation[] stringAttrTitles = objects[0].GetAttributeTitlesByType(NXObject.AttributeType.String);
                foreach (NXObject.AttributeInformation attr in stringAttrTitles)
                {
                    if (attr.Title.Contains("DB_PART_TYPE"))
                    {
                        string value = objects[0].GetStringAttribute(attr.Title);
                        if (value == "M4_it_cv")
                        {
                            //textBox2.AppendText(comp.DisplayName + "\n");
                            if (!lista_proj_cv.ContainsKey(comp.DisplayName.Substring(0, 8)))
                            {
                                lista_proj_cv.Add(comp.DisplayName.Substring(0, 8), 1);
                            }
                            else
                            {
                                lista_proj_cv[comp.DisplayName.Substring(0, 8)] = lista_proj_cv[comp.DisplayName.Substring(0, 8)] + 1;
                            }
                        }
                    }
                }
            }
            if (CodRevisao_CV != "")
            {
                if (lista_proj_cv.Count > 0 || lista_txt_cv.Count > 0)
                {
                    foreach (var item in lista_proj_cv)
                    {
                        if (lista_txt_cv.ContainsKey(item.Key))
                        {
                            if (item.Value != lista_txt_cv[item.Key])
                            {
                                msg.Append($"Diferença para o item {item.Key}: Projeto = {item.Value} | Lista TXT = {lista_txt_cv[item.Key]}\n");
                            }
                        }
                        else
                        {
                            if (File.Exists(path_lista))
                            {
                                msg.Append($"Item {item.Key} presente no projeto mas não encontrado na lista TXT.\n");
                            }
                        }
                    }
                    if (lista_txt_cv.Count > 0)
                    {
                        foreach (var item in lista_txt_cv)
                        {
                            if (lista_proj_cv.ContainsKey(item.Key))
                            {
                                if (item.Value != lista_proj_cv[item.Key])
                                {
                                    msg.Append($"Projeto - Item: {item.Key} Txt: {item.Value} Proj:{lista_proj_cv[item.Key]}\n");
                                }
                            }
                            else
                            {
                                msg.Append($"Item {item.Key} presente na lista TXT mas não encontrado no projeto.\n");
                            }
                        }
                    }
                }
            }
            if (lista_proj_cv.Count > 0 && CodRevisao_CV == "")
            {
                msg.Append($"O Projeto contém ITEM CV.\nGere o código agrupador de Contraventamentos.\n");
            }
            if (msg.Length > 0) {
                MessageBox.Show(msg.ToString(), "Contraventamentos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static void Verificar_Blank()
        {
            Session theSession = Session.GetSession();
            Part part1 = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            string codigo = part1.Name.Split('/')[0];
            string rev = part1.Name.Split('/')[1].Substring(0, 3);

            // partLoadStatus1.Dispose();

            string name_blank = "";
            foreach (Feature blank in part1.Features)
            {

                if (blank.GetFeatureName().Contains("Flat Pattern") && (blank.Name == "BLANK" || blank.Name == "blank"))
                {
                    //MessageBox.Show(item.GetFeatureName() + "  " + item.Name);
                    name_blank = blank.GetFeatureName().Replace(' ', '_').ToLower();
                }

            }
            name_blank = name_blank.Replace(' ', '_').ToUpper();
            // ----------------------------------------------
            //   Menu: Insert->Flat Pattern->Export Flat Pattern...
            // ----------------------------------------------

            if (name_blank != "")
            {
                DialogResult result = MessageBox.Show("Deseja gerar o BLANK?", "Gerar BLANK", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    theSession.ApplicationSwitchImmediate("UG_APP_SBSM");
                    string path = @"S:\Desenhos\Temp_DXF\" + codigo + "-" + rev + ".dxf";
                    //  LOG_Message("BLANK encontrado: " + name_blank);
                    NXOpen.Session.UndoMarkId markId4;
                    markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

                    NXOpen.Features.SheetMetal.ExportFlatPatternBuilder exportFlatPatternBuilder1;
                    exportFlatPatternBuilder1 = part1.Features.SheetmetalManager.CreateExportFlatPatternBuilder();
                    exportFlatPatternBuilder1.ExportLocation = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.ExportLocationOptions.Native;
                    exportFlatPatternBuilder1.OutputFile = path;

                    exportFlatPatternBuilder1.BendUp = false;

                    exportFlatPatternBuilder1.BendDown = false;

                    exportFlatPatternBuilder1.InteriorCutout = true;
                    exportFlatPatternBuilder1.InteriorFeature = true;

                    NXOpen.Part part2 = ((NXOpen.Part)part1);
                    NXOpen.Features.FlatPattern flatPattern1 = ((NXOpen.Features.FlatPattern)part2.Features.FindObject(name_blank));
                    exportFlatPatternBuilder1.FlatPattern.Value = flatPattern1;

                    exportFlatPatternBuilder1.FlatPattern.Value = flatPattern1;

                    theSession.SetUndoMarkName(markId4, "Export Flat Pattern Dialog");

                    exportFlatPatternBuilder1.DxfRevision = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.DxfRevisionType.R12;

                    NXOpen.Session.UndoMarkId markId5;
                    markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export Flat Pattern");

                    theSession.DeleteUndoMark(markId5, null);

                    NXOpen.Session.UndoMarkId markId6;
                    markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export Flat Pattern");

                    // Potential journal callback detected. Pausing journal.
                    // Journal callback ended. Unpausing
                    NXOpen.NXObject nXObject1;
                    nXObject1 = exportFlatPatternBuilder1.Commit();

                    theSession.DeleteUndoMark(markId6, null);

                    exportFlatPatternBuilder1.Destroy();

                }

            }
        }
        public static void Verificar_Blank_Fam()
        {
            Session theSession = Session.GetSession();
            Part part1 = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            string codigo = part1.Name.Split('/')[0];
            string rev = part1.Name.Split('/')[1].Substring(0, 3);

            // partLoadStatus1.Dispose();

            string name_blank = "";
            foreach (Feature blank in part1.Features)
            {

                if (blank.GetFeatureName().Contains("Flat Pattern") && (blank.Name == "BLANK" || blank.Name == "blank"))
                {
                    //MessageBox.Show(item.GetFeatureName() + "  " + item.Name);
                    name_blank = blank.GetFeatureName().Replace(' ', '_').ToLower();
                }

            }
            name_blank = name_blank.Replace(' ', '_').ToUpper();
            // ----------------------------------------------
            //   Menu: Insert->Flat Pattern->Export Flat Pattern...
            // ----------------------------------------------

            if (name_blank != "")
            {

                theSession.ApplicationSwitchImmediate("UG_APP_SBSM");
                string path = @"S:\Desenhos\Temp_DXF\" + codigo + "-" + rev + ".dxf";
                NXOpen.Session.UndoMarkId markId4;
                markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

                NXOpen.Features.SheetMetal.ExportFlatPatternBuilder exportFlatPatternBuilder1;
                exportFlatPatternBuilder1 = part1.Features.SheetmetalManager.CreateExportFlatPatternBuilder();
                exportFlatPatternBuilder1.ExportLocation = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.ExportLocationOptions.Native;
                exportFlatPatternBuilder1.OutputFile = path;

                exportFlatPatternBuilder1.BendUp = false;

                exportFlatPatternBuilder1.BendDown = false;

                exportFlatPatternBuilder1.InteriorCutout = true;
                exportFlatPatternBuilder1.InteriorFeature = true;

                NXOpen.Part part2 = ((NXOpen.Part)part1);
                NXOpen.Features.FlatPattern flatPattern1 = ((NXOpen.Features.FlatPattern)part2.Features.FindObject(name_blank));
                exportFlatPatternBuilder1.FlatPattern.Value = flatPattern1;

                exportFlatPatternBuilder1.FlatPattern.Value = flatPattern1;

                theSession.SetUndoMarkName(markId4, "Export Flat Pattern Dialog");

                exportFlatPatternBuilder1.DxfRevision = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.DxfRevisionType.R12;

                NXOpen.Session.UndoMarkId markId5;
                markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export Flat Pattern");

                theSession.DeleteUndoMark(markId5, null);

                NXOpen.Session.UndoMarkId markId6;
                markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export Flat Pattern");

                NXOpen.NXObject nXObject1;
                nXObject1 = exportFlatPatternBuilder1.Commit();

                theSession.DeleteUndoMark(markId6, null);

                exportFlatPatternBuilder1.Destroy();


            }
        }
        public static void item_fam()
        {
            
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            //Part displayPart = theSession.Parts.Display;
            string name = workPart.Name;
            string codigo = name.Substring(0, 6);
            string rev = name.Substring(7, 3);
            bool is_fam = IsPartFamilyInstance(workPart);

            if (is_fam == true)
            {

              //  MessageBox.Show(name.Substring(0, 6) + "-" + name.Substring(7, 3) + " -- Este item é de familia vamos gerar o PDF ");
                GerarPDF(codigo, rev);
            }
            else
            {
                // MessageBox.Show(name.Substring(0, 6) + "-" + name.Substring(7, 3) + "Este item não é de familia não vamos gerar o PDF ");
            }
        }

        public static void GerarPDF(string codigo, string rev)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: File->Export->PDF...
            // ----------------------------------------------
            theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            NXOpen.PrintPDFBuilder printPDFBuilder1;
            printPDFBuilder1 = workPart.PlotManager.CreatePrintPdfbuilder();

            printPDFBuilder1.Relation = NXOpen.PrintPDFBuilder.RelationOption.Manifestation;

            printPDFBuilder1.DeleteDatasets = true;

            printPDFBuilder1.DatasetType = "PDF";

            printPDFBuilder1.NamedReferenceType = "PDF_Reference";

            printPDFBuilder1.Scale = 1.0;

            printPDFBuilder1.Action = NXOpen.PrintPDFBuilder.ActionOption.Overwrite;

            printPDFBuilder1.DatasetName = "513414_000-PDF-2";

            printPDFBuilder1.Size = NXOpen.PrintPDFBuilder.SizeOption.ScaleFactor;

            printPDFBuilder1.Units = NXOpen.PrintPDFBuilder.UnitsOption.English;

            printPDFBuilder1.XDimension = 8.5;

            printPDFBuilder1.YDimension = 11.0;

            printPDFBuilder1.OutputText = NXOpen.PrintPDFBuilder.OutputTextOption.Polylines;

            printPDFBuilder1.RasterImages = true;

            NXOpen.PDM.PartBuilder.PartFileNameData partInfo1;
            // No object found for the subject of the next call.
            // This may be because of previous exceptions
            // partInfo1 = <NXOpen.PDM.PartBuilder>.AssignPartFileName("512728", "000", "manifestation", "512728_000-PDF-1");

            printPDFBuilder1.Assign();

            theSession.SetUndoMarkName(markId1, "Export PDF Dialog");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export PDF");

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export PDF");

            printPDFBuilder1.Watermark = "";

            NXOpen.NXObject[] sheets1 = new NXOpen.NXObject[1];
            NXOpen.Drawings.DraftingDrawingSheet draftingDrawingSheet1 = ((NXOpen.Drawings.DraftingDrawingSheet)workPart.DraftingDrawingSheets.FindObject("Sheet 1"));
            sheets1[0] = draftingDrawingSheet1;
            printPDFBuilder1.SourceBuilder.SetSheets(sheets1);

            printPDFBuilder1.CreateNewFromUi = false;

            printPDFBuilder1.DatasetName = codigo + "_" + rev + "-PDF-1";

            printPDFBuilder1.Commit();


            theSession.DeleteUndoMark(markId3, null);

            theSession.SetUndoMarkName(markId1, "Export PDF");

            printPDFBuilder1.Destroy();

            theSession.DeleteUndoMark(markId1, null);
        }
        public static void Remove_blank()
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;

            if (theSession.ApplicationName == "UG_APP_DRAFTING")
            {

                var shetts = workPart.DraftingDrawingSheets.ToArray();
                
                //foreach (NXOpen.Drawings.DraftingDrawingSheet item1 in shetts)
                //{
                //    if (item1.Name.Contains("BLANK") || item1.Name.Contains("Blan"))
                //    {
                //        NXOpen.TaggedObject[] objects1 = new NXOpen.TaggedObject[1];
                //        objects1[0] = item1;
                //        int nErrs1;
                //        nErrs1 = theSession.UpdateManager.AddObjectsToDeleteList(objects1);

                //    }
                //    if (shetts.Length>1)
                //    {
                //        MessageBox.Show("Não é permitido mais de uma SHEET .\nDeseja Deletar?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                //        //NXOpen.TaggedObject[] objects1 = new NXOpen.TaggedObject[1];
                //        //objects1[0] = item1;
                //        //int nErrs1;
                //        //nErrs1 = theSession.UpdateManager.AddObjectsToDeleteList(objects1);

                //    }

                //}
                if(shetts.Length>1)
                {
                    List<string> listaShetts = new List<string>();

                    foreach (NXOpen.Drawings.DraftingDrawingSheet item1 in shetts)
                    {
                        listaShetts.Add(item1.Name);
                    }
                    string s = String.Join("\n", listaShetts);
                    DialogResult response =  MessageBox.Show($"É permitido somente UMA folha de Drawing.\n\nForam encontradas as seguinte Folhas:\n{s}", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    Form1 form1 = new Form1(listaShetts);
                    form1.ShowDialog();
                    List<string> shettSelect = form1.ShettName;

                    foreach (NXOpen.Drawings.DraftingDrawingSheet item1 in shetts)
                    {
                        if (shettSelect.Contains(item1.Name))// == shettSelect)
                        {
                            NXOpen.TaggedObject[] objects1 = new NXOpen.TaggedObject[1];
                            objects1[0] = item1;
                            int nErrs1;
                            nErrs1 = theSession.UpdateManager.AddObjectsToDeleteList(objects1);
                        }
                    }
                }

            }

            if (theSession.ApplicationName == "UG_APP_MODELING")
            {

               

                NXObject[] objects2 = new NXObject[1];
                objects2[0] = workPart;
                string spec = null;

                NXOpen.NXObject.AttributeInformation[] atributo = objects2[0].GetAttributeTitlesByType(NXObject.AttributeType.String);
                
                foreach (NXOpen.NXObject.AttributeInformation att in atributo)
                {
                 
                    if (att.Title == "NX_NON_MASTER")
                    {
                        spec = "No_Deletar";
                        
                          break;
                    }
                }

                if (spec == null)
                {
                    var shetts = workPart.DraftingDrawingSheets.ToArray();

                    foreach (NXOpen.Drawings.DraftingDrawingSheet item1 in shetts)
                    {
                        DialogResult response = MessageBox.Show("Não são permitidas \"SHEETS\" dentro do Model.\nDeseja Deletar?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (response == DialogResult.Yes)
                        {
                            NXOpen.TaggedObject[] objects1 = new NXOpen.TaggedObject[1];
                            objects1[0] = item1;
                            int nErrs1;
                            nErrs1 = theSession.UpdateManager.AddObjectsToDeleteList(objects1);
                        }
                    }
                }
            }
        }

        public static bool IsPartFamilyInstance(Part thePart)
        {
            bool isFamilyInstance = false;

            // Obtém a sessão UF (User Function)
            UFSession ufSession = UFSession.GetUFSession();

            // Verifica se a peça é um modelo de família usando o método IsFamilyTemplate
            ufSession.Part.IsFamilyInstance(thePart.Tag, out isFamilyInstance);

            return isFamilyInstance;
        }

        public static bool Recriar_link_mp()
        {
            bool result = false;
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;

            string ver_exp = "";
            try
            {
                 ver_exp = workPart.Expressions.FindObject("NX_Material").StringValue;
            }
            catch
            {
                ver_exp = "";
            }

          
            if(ver_exp != "")
            {
                result = true;
               
           
            // ----------------------------------------------
            //   Menu: File->Properties
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.NXObject[] objects1 = new NXOpen.NXObject[1];
            objects1[0] = workPart;
            NXOpen.AttributePropertiesBuilder attributePropertiesBuilder1;
            attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            attributePropertiesBuilder1.Units = "MilliMeter";

            NXOpen.NXObject[] objects2 = new NXOpen.NXObject[1];
            objects2[0] = workPart;
            NXOpen.MassPropertiesBuilder massPropertiesBuilder1;
            massPropertiesBuilder1 = workPart.PropertiesManager.CreateMassPropertiesBuilder(objects2);

            NXOpen.SelectNXObjectList selectNXObjectList1;
            selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

            NXOpen.NXObject[] objects3;
            objects3 = selectNXObjectList1.GetArray();

            massPropertiesBuilder1.LoadPartialComponents = true;

            massPropertiesBuilder1.Accuracy = 0.98999999999999999;

            NXOpen.NXObject[] objects4 = new NXOpen.NXObject[1];
            objects4[0] = workPart;
            NXOpen.PreviewPropertiesBuilder previewPropertiesBuilder1;
            previewPropertiesBuilder1 = workPart.PropertiesManager.CreatePreviewPropertiesBuilder(objects4);

            previewPropertiesBuilder1.StorePartPreview = true;

            previewPropertiesBuilder1.StoreModelViewPreview = true;

            previewPropertiesBuilder1.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

            NXOpen.NXObject[] objects5 = new NXOpen.NXObject[1];
            objects5[0] = workPart;
            attributePropertiesBuilder1.SetAttributeObjects(objects5);

            attributePropertiesBuilder1.Units = "MilliMeter";

            theSession.SetUndoMarkName(markId1, "Displayed Part Properties Dialog");

            attributePropertiesBuilder1.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day11;

            attributePropertiesBuilder1.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Nov;

            attributePropertiesBuilder1.DateValue.DateItem.Year = "2024";

            attributePropertiesBuilder1.DateValue.DateItem.Time = "00:00:00";

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Category = "M4_it_mascarelloRevisionMaster";

            attributePropertiesBuilder1.Title = "Material_Produto";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "p";

            NXOpen.Expression expression1;
            expression1 = workPart.Expressions.CreateSystemExpression("String", @"""""");

            expression1.RightHandSide = "NX_Material";

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Add New Attribute from Template");

            attributePropertiesBuilder1.IsReferenceType = false;

            attributePropertiesBuilder1.Expression = expression1;

            attributePropertiesBuilder1.ValueAlias = "";

            bool changed1;
            changed1 = attributePropertiesBuilder1.CreateAttribute();

            attributePropertiesBuilder1.Category = "";

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsReferenceType = false;

            NXOpen.Expression nullNXOpen_Expression = null;
            attributePropertiesBuilder1.Expression = nullNXOpen_Expression;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Category = "M4_it_mascarelloRevisionMaster";

            attributePropertiesBuilder1.Title = "Material_Produto";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsReferenceType = false;

            attributePropertiesBuilder1.Expression = expression1;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            theSession.DeleteUndoMark(markId3, null);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Attribute Properties");

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId5);

            NXOpen.MassPropertiesBuilder.UpdateOptions updateoption1;
            updateoption1 = massPropertiesBuilder1.UpdateOnSave;

            NXOpen.NXObject nXObject1;
            nXObject1 = massPropertiesBuilder1.Commit();

            workPart.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

            NXOpen.NXObject nXObject2;
            nXObject2 = previewPropertiesBuilder1.Commit();

            NXOpen.Session.UndoMarkId id1;
            id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(id1);

            theSession.DeleteUndoMark(markId4, null);

            theSession.SetUndoMarkName(markId1, "Displayed Part Properties");

            attributePropertiesBuilder1.Destroy();

            massPropertiesBuilder1.Destroy();

            previewPropertiesBuilder1.Destroy();

            theSession.DeleteUndoMark(id1, null);

            theSession.DeleteUndoMark(markId2, null);
            }
           
            return result;

        }
        public static void recriar_link_mp_model_nx9()
        {

            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: File->Properties
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.NXObject[] objects1 = new NXOpen.NXObject[1];
            objects1[0] = workPart;
            NXOpen.AttributePropertiesBuilder attributePropertiesBuilder1;
            attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            attributePropertiesBuilder1.Units = "MilliMeter";

            NXOpen.NXObject[] objects2 = new NXOpen.NXObject[1];
            objects2[0] = workPart;
            NXOpen.MassPropertiesBuilder massPropertiesBuilder1;
            massPropertiesBuilder1 = workPart.PropertiesManager.CreateMassPropertiesBuilder(objects2);

            NXOpen.SelectNXObjectList selectNXObjectList1;
            selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

            NXOpen.NXObject[] objects3;
            objects3 = selectNXObjectList1.GetArray();

            massPropertiesBuilder1.LoadPartialComponents = true;

            massPropertiesBuilder1.Accuracy = 0.98999999999999999;

            NXOpen.NXObject[] objects4 = new NXOpen.NXObject[1];
            objects4[0] = workPart;
            NXOpen.PreviewPropertiesBuilder previewPropertiesBuilder1;
            previewPropertiesBuilder1 = workPart.PropertiesManager.CreatePreviewPropertiesBuilder(objects4);

            previewPropertiesBuilder1.StorePartPreview = true;

            previewPropertiesBuilder1.StoreModelViewPreview = true;

            previewPropertiesBuilder1.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

            NXOpen.NXObject[] objects5 = new NXOpen.NXObject[1];
            objects5[0] = workPart;
            attributePropertiesBuilder1.SetAttributeObjects(objects5);

            attributePropertiesBuilder1.Units = "MilliMeter";

            theSession.SetUndoMarkName(markId1, "Displayed Part Properties Dialog");

            attributePropertiesBuilder1.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day11;

            attributePropertiesBuilder1.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Nov;

            attributePropertiesBuilder1.DateValue.DateItem.Year = "2024";

            attributePropertiesBuilder1.DateValue.DateItem.Time = "00:00:00";

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Category = "M4_it_mascarelloRevisionMaster";

            attributePropertiesBuilder1.Title = "Material_Produto";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "p";

            NXOpen.Expression expression1;
            expression1 = workPart.Expressions.CreateSystemExpression("String", @"""""");

            expression1.RightHandSide = "p11";

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Add New Attribute from Template");

            attributePropertiesBuilder1.IsReferenceType = false;

            attributePropertiesBuilder1.Expression = expression1;

            attributePropertiesBuilder1.ValueAlias = "";

            bool changed1;
            changed1 = attributePropertiesBuilder1.CreateAttribute();

            attributePropertiesBuilder1.Category = "";

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsReferenceType = false;

            NXOpen.Expression nullNXOpen_Expression = null;
            attributePropertiesBuilder1.Expression = nullNXOpen_Expression;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Category = "M4_it_mascarelloRevisionMaster";

            attributePropertiesBuilder1.Title = "Material_Produto";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsReferenceType = false;

            attributePropertiesBuilder1.Expression = expression1;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            theSession.DeleteUndoMark(markId3, null);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Attribute Properties");

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId5);

            NXOpen.MassPropertiesBuilder.UpdateOptions updateoption1;
            updateoption1 = massPropertiesBuilder1.UpdateOnSave;

            NXOpen.NXObject nXObject1;
            nXObject1 = massPropertiesBuilder1.Commit();

            workPart.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

            NXOpen.NXObject nXObject2;
            nXObject2 = previewPropertiesBuilder1.Commit();

            NXOpen.Session.UndoMarkId id1;
            id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(id1);

            theSession.DeleteUndoMark(markId4, null);

            theSession.SetUndoMarkName(markId1, "Displayed Part Properties");

            attributePropertiesBuilder1.Destroy();

            massPropertiesBuilder1.Destroy();

            previewPropertiesBuilder1.Destroy();

            theSession.DeleteUndoMark(id1, null);

            theSession.DeleteUndoMark(markId2, null);

        }
        public static void link_fp_qtd()
        {
            string ver_exp = "0";

            Session theSession = Session.GetSession();
            Part wp_m = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            Unit nullUnit = null;
            string fp = "";
            string qb = "";
            string qtd = "";

            try
            {
                Expression Busca_Material = (Expression)wp_m.Expressions.FindObject("mp");

                Expression fator_perda = (Expression)wp_m.Expressions.FindObject("format_fator_perda");
                Expression qtd_bruto = (Expression)wp_m.Expressions.FindObject("format_qtd_bruto");
                Expression qtd_liq = (Expression)wp_m.Expressions.FindObject("format_qtd");
                fp = fator_perda.StringValue;
                qb = qtd_bruto.StringValue;
                qtd = qtd_liq.StringValue;

                ver_exp = "1";
            }
            catch (Exception)
            {
                ver_exp = "0";
            }


            if (ver_exp == "1")
            {
                //  Session theSession = Session.GetSession();
                Part workPart = theSession.Parts.Work;
                Part displayPart1 = theSession.Parts.Display;
                // ----------------------------------------------
                //   Menu: File->Properties
                // ----------------------------------------------
                NXOpen.Session.UndoMarkId markId1;
                markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

                NXObject[] objects1 = new NXObject[1];
                objects1[0] = workPart;
                AttributePropertiesBuilder attributePropertiesBuilder1;
                attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

                attributePropertiesBuilder1.Units = "MilliMeter";

                NXObject[] objects2 = new NXObject[1];
                objects2[0] = workPart;
                MassPropertiesBuilder massPropertiesBuilder1;
                massPropertiesBuilder1 = workPart.PropertiesManager.CreateMassPropertiesBuilder(objects2);

                SelectNXObjectList selectNXObjectList1;
                selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

                NXObject[] objects3;
                objects3 = selectNXObjectList1.GetArray();

                massPropertiesBuilder1.LoadPartialComponents = true;

                massPropertiesBuilder1.Accuracy = 0.99;

                NXObject[] objects4 = new NXObject[1];
                objects4[0] = workPart;
                PreviewPropertiesBuilder previewPropertiesBuilder1;
                previewPropertiesBuilder1 = workPart.PropertiesManager.CreatePreviewPropertiesBuilder(objects4);

                previewPropertiesBuilder1.StorePartPreview = true;

                previewPropertiesBuilder1.StoreModelViewPreview = true;

                previewPropertiesBuilder1.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

                attributePropertiesBuilder1.DateValue.FromDateItem.Year = "2018";

                attributePropertiesBuilder1.DateValue.ToDateItem.Year = "2018";

                NXObject[] objects5 = new NXObject[1];
                objects5[0] = workPart;
                attributePropertiesBuilder1.SetAttributeObjects(objects5);

                attributePropertiesBuilder1.Units = "MilliMeter";

                theSession.SetUndoMarkName(markId1, "Displayed Part Properties Dialog");

                attributePropertiesBuilder1.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day12;

                attributePropertiesBuilder1.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Feb;

                attributePropertiesBuilder1.DateValue.DateItem.Year = "2019";

                attributePropertiesBuilder1.DateValue.DateItem.Time = "00:00:00";

                massPropertiesBuilder1.UpdateOnSave = NXOpen.MassPropertiesBuilder.UpdateOptions.No;

                attributePropertiesBuilder1.Title = "";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.StringValue = "";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.StringValue = "";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.Category = "M4_it_mascarelloRevisionMaster";

                attributePropertiesBuilder1.Title = "DB_FATOR_PERDA";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.StringValue = "";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                NXOpen.Session.UndoMarkId markId2;
                markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Link to Expression");

                NXOpen.Session.UndoMarkId markId3;
                markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Launch Expression Editor");

                attributePropertiesBuilder1.StringValue = fp;

                attributePropertiesBuilder1.IsReferenceType = false;

                Expression expression1 = (Expression)workPart.Expressions.FindObject("format_fator_perda");
                attributePropertiesBuilder1.Expression = expression1;

                theSession.DeleteUndoMarksUpToMark(markId3, null, false);

                NXOpen.Session.UndoMarkId markId4;
                markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Accept Edit");

                bool changed1;
                changed1 = attributePropertiesBuilder1.CreateAttribute();

                attributePropertiesBuilder1.Category = "";

                attributePropertiesBuilder1.Title = "";

                attributePropertiesBuilder1.IsReferenceType = false;

                Expression nullExpression = null;
                attributePropertiesBuilder1.Expression = nullExpression;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.StringValue = "";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.StringValue = "";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.Title = "";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.StringValue = "";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.StringValue = "";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.Category = "M4_it_mascarelloRevisionMaster";

                attributePropertiesBuilder1.Title = "DB_QTD_BRUTO";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.StringValue = "";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                NXOpen.Session.UndoMarkId markId5;
                markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Link to Expression");

                NXOpen.Session.UndoMarkId markId6;
                markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Launch Expression Editor");

                // ----------------------------------------------
                //   Dialog Begin Expressions
                // ----------------------------------------------
                attributePropertiesBuilder1.StringValue = qb;

                attributePropertiesBuilder1.IsReferenceType = false;

                Expression expression2 = (Expression)workPart.Expressions.FindObject("format_qtd_bruto");
                attributePropertiesBuilder1.Expression = expression2;

                theSession.DeleteUndoMarksUpToMark(markId6, null, false);

                NXOpen.Session.UndoMarkId markId7;
                markId7 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

                NXObject nXObject1;
                nXObject1 = attributePropertiesBuilder1.Commit();

                NXOpen.MassPropertiesBuilder.UpdateOptions updateoption1;
                updateoption1 = massPropertiesBuilder1.UpdateOnSave;

                NXObject nXObject2;
                nXObject2 = massPropertiesBuilder1.Commit();

                workPart.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

                NXObject nXObject3;
                nXObject3 = previewPropertiesBuilder1.Commit();

                NXOpen.Session.UndoMarkId id1;
                id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

                int nErrs1;
                nErrs1 = theSession.UpdateManager.DoUpdate(id1);

                theSession.DeleteUndoMark(markId7, null);

                theSession.SetUndoMarkName(markId1, "Displayed Part Properties");

                attributePropertiesBuilder1.Destroy();

                massPropertiesBuilder1.Destroy();

                previewPropertiesBuilder1.Destroy();

                theSession.DeleteUndoMark(id1, null);

                theSession.DeleteUndoMark(markId4, null);

                theSession.DeleteUndoMark(markId2, null);

                NXOpen.Session.UndoMarkId markId8;
                markId8 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

                NXObject[] objects6 = new NXObject[1];
                objects6[0] = workPart;
                AttributePropertiesBuilder attributePropertiesBuilder2;
                attributePropertiesBuilder2 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects6, NXOpen.AttributePropertiesBuilder.OperationType.None);

                attributePropertiesBuilder2.IsArray = false;

                attributePropertiesBuilder2.IsArray = false;

                attributePropertiesBuilder2.IsArray = false;

                attributePropertiesBuilder2.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

                attributePropertiesBuilder2.Units = "MilliMeter";

                NXObject[] objects7 = new NXObject[1];
                objects7[0] = workPart;
                MassPropertiesBuilder massPropertiesBuilder2;
                massPropertiesBuilder2 = workPart.PropertiesManager.CreateMassPropertiesBuilder(objects7);

                SelectNXObjectList selectNXObjectList2;
                selectNXObjectList2 = massPropertiesBuilder2.SelectedObjects;

                NXObject[] objects8;
                objects8 = selectNXObjectList2.GetArray();

                massPropertiesBuilder2.LoadPartialComponents = true;

                massPropertiesBuilder2.Accuracy = 0.99;

                NXObject[] objects9 = new NXObject[1];
                objects9[0] = workPart;
                PreviewPropertiesBuilder previewPropertiesBuilder2;
                previewPropertiesBuilder2 = workPart.PropertiesManager.CreatePreviewPropertiesBuilder(objects9);

                previewPropertiesBuilder2.StorePartPreview = true;

                previewPropertiesBuilder2.StoreModelViewPreview = true;

                previewPropertiesBuilder2.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

                attributePropertiesBuilder2.DateValue.FromDateItem.Year = "2018";

                attributePropertiesBuilder2.DateValue.ToDateItem.Year = "2018";

                NXObject[] objects10 = new NXObject[1];
                objects10[0] = workPart;
                attributePropertiesBuilder2.SetAttributeObjects(objects10);

                attributePropertiesBuilder2.Units = "MilliMeter";

                theSession.SetUndoMarkName(markId8, "Displayed Part Properties Dialog");

                attributePropertiesBuilder2.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day12;

                attributePropertiesBuilder2.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Feb;

                attributePropertiesBuilder2.DateValue.DateItem.Year = "2019";

                attributePropertiesBuilder2.DateValue.DateItem.Time = "00:00:00";

                massPropertiesBuilder2.UpdateOnSave = NXOpen.MassPropertiesBuilder.UpdateOptions.No;

                // ----------------------------------------------
                //   Dialog Begin Displayed Part Properties
                // ----------------------------------------------
                NXOpen.Session.UndoMarkId markId9;
                markId9 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

                theSession.DeleteUndoMark(markId9, null);

                NXOpen.Session.UndoMarkId markId10;
                markId10 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

                NXObject nXObject4;
                nXObject4 = attributePropertiesBuilder2.Commit();

                NXOpen.MassPropertiesBuilder.UpdateOptions updateoption2;
                updateoption2 = massPropertiesBuilder2.UpdateOnSave;

                NXObject nXObject5;
                nXObject5 = massPropertiesBuilder2.Commit();

                workPart.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

                NXObject nXObject6;
                nXObject6 = previewPropertiesBuilder2.Commit();

                NXOpen.Session.UndoMarkId id2;
                id2 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

                int nErrs2;
                nErrs2 = theSession.UpdateManager.DoUpdate(id2);

                theSession.DeleteUndoMark(markId10, null);

                theSession.SetUndoMarkName(id2, "Displayed Part Properties");

                attributePropertiesBuilder2.Destroy();

                massPropertiesBuilder2.Destroy();

                previewPropertiesBuilder2.Destroy();

                // ----------------------------------------------
                //   Menu: Tools->Journal->Stop Recording
                // ----------------------------------------------





            }

        }
        public static void link_qtd_liquido()
        {
            string ver_exp = "0";

            Session theSession = Session.GetSession();
            Part wp_m = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            Unit nullUnit = null;

            string qtd = "";

            try
            {
                Expression Busca_Material = (Expression)wp_m.Expressions.FindObject("mp");

                Expression qtd_liq = (Expression)wp_m.Expressions.FindObject("format_qtd");
                qtd = qtd_liq.StringValue;

                ver_exp = "1";
            }
            catch (Exception)
            {
                ver_exp = "0";
            }


            if (ver_exp == "1")
            {
                Part workPart = theSession.Parts.Work;
                Part displayPart1 = theSession.Parts.Display;

                NXOpen.Session.UndoMarkId markId1;
                markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

                NXObject[] objects1 = new NXObject[1];
                objects1[0] = workPart;
                AttributePropertiesBuilder attributePropertiesBuilder1;
                attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

                attributePropertiesBuilder1.Units = "MilliMeter";

                NXObject[] objects2 = new NXObject[1];
                objects2[0] = workPart;
                MassPropertiesBuilder massPropertiesBuilder1;
                massPropertiesBuilder1 = workPart.PropertiesManager.CreateMassPropertiesBuilder(objects2);

                SelectNXObjectList selectNXObjectList1;
                selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

                NXObject[] objects3;
                objects3 = selectNXObjectList1.GetArray();

                massPropertiesBuilder1.LoadPartialComponents = true;

                massPropertiesBuilder1.Accuracy = 0.99;

                NXObject[] objects4 = new NXObject[1];
                objects4[0] = workPart;
                PreviewPropertiesBuilder previewPropertiesBuilder1;
                previewPropertiesBuilder1 = workPart.PropertiesManager.CreatePreviewPropertiesBuilder(objects4);

                previewPropertiesBuilder1.StorePartPreview = true;

                previewPropertiesBuilder1.StoreModelViewPreview = true;

                previewPropertiesBuilder1.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

                attributePropertiesBuilder1.DateValue.FromDateItem.Year = "2018";

                attributePropertiesBuilder1.DateValue.ToDateItem.Year = "2018";

                NXObject[] objects5 = new NXObject[1];
                objects5[0] = workPart;
                attributePropertiesBuilder1.SetAttributeObjects(objects5);

                attributePropertiesBuilder1.Units = "MilliMeter";

                theSession.SetUndoMarkName(markId1, "Displayed Part Properties Dialog");

                attributePropertiesBuilder1.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day05;

                attributePropertiesBuilder1.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Apr;

                attributePropertiesBuilder1.DateValue.DateItem.Year = "2019";

                attributePropertiesBuilder1.DateValue.DateItem.Time = "00:00:00";

                massPropertiesBuilder1.UpdateOnSave = NXOpen.MassPropertiesBuilder.UpdateOptions.No;

                attributePropertiesBuilder1.Title = "";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.StringValue = "";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.StringValue = "";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.Title = "DB_QTD";

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.StringValue = qtd;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                NXOpen.Session.UndoMarkId markId2;
                markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Link to Expression");

                NXOpen.Session.UndoMarkId markId3;
                markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Launch Expression Editor");

                // ----------------------------------------------
                //   Dialog Begin Expressions
                // ----------------------------------------------
                attributePropertiesBuilder1.StringValue = qtd;

                attributePropertiesBuilder1.IsReferenceType = false;

                Expression expression1 = (Expression)workPart.Expressions.FindObject("format_qtd");
                attributePropertiesBuilder1.Expression = expression1;

                theSession.DeleteUndoMarksUpToMark(markId3, null, false);

                NXOpen.Session.UndoMarkId markId4;
                markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

                theSession.DeleteUndoMark(markId4, null);

                NXOpen.Session.UndoMarkId markId5;
                markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

                NXObject nXObject1;
                nXObject1 = attributePropertiesBuilder1.Commit();

                NXOpen.MassPropertiesBuilder.UpdateOptions updateoption1;
                updateoption1 = massPropertiesBuilder1.UpdateOnSave;

                NXObject nXObject2;
                nXObject2 = massPropertiesBuilder1.Commit();

                workPart.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

                NXObject nXObject3;
                nXObject3 = previewPropertiesBuilder1.Commit();

                NXOpen.Session.UndoMarkId id1;
                id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

                int nErrs1;
                nErrs1 = theSession.UpdateManager.DoUpdate(id1);

                theSession.DeleteUndoMark(markId5, null);

                theSession.SetUndoMarkName(markId1, "Displayed Part Properties");

                attributePropertiesBuilder1.Destroy();

                massPropertiesBuilder1.Destroy();

                previewPropertiesBuilder1.Destroy();

                theSession.DeleteUndoMark(id1, null);


            }

        }
        public static void atualiza_qtd_mp()
        {

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
                    ///  MessageBox.Show(cod);
                    double ver_contra = 0;

                    foreach (var item in attr_contra)
                    {

                        if (item.StringValue.Contains("MP"))
                        {
                            ver_contra = 1;

                        }

                    }
                    //}
                    if (ver_contra == 1)
                    {
                        string muda_qtd = "0";

                        string expression = "QTD_" + cod.Substring(0, 6);

                        ///double valor_exp;
                        string busca = busca_qtd_mp(expression);

                        // valor_exp = Convert.ToDouble();


                        if (busca != "0")
                        {
                            muda_qtd = "1";
                        }


                        if (muda_qtd == "1")
                        {
                            NXOpen.Assemblies.AssembliesGeneralPropertiesBuilder assembliesGeneralPropertiesBuilder1;
                            assembliesGeneralPropertiesBuilder1 = wp_m.PropertiesManager.CreateAssembliesGeneralPropertiesBuilder(objects1);

                            NXOpen.Assemblies.SelectComponentList selectComponentList1;
                            selectComponentList1 = assembliesGeneralPropertiesBuilder1.SelectedObjects;

                            NXOpen.Assemblies.Component[] objects2;
                            objects2 = selectComponentList1.GetArray();

                            assembliesGeneralPropertiesBuilder1.LayerOption = NXOpen.Assemblies.AssembliesGeneralPropertiesBuilder.LayerOptions.Mixed;

                            assembliesGeneralPropertiesBuilder1.QuantityType = NXOpen.Assemblies.AssembliesGeneralPropertiesBuilder.QuantityOptions.AsRequired;

                            assembliesGeneralPropertiesBuilder1.QuantityType = NXOpen.Assemblies.AssembliesGeneralPropertiesBuilder.QuantityOptions.Number;

                            NXObject[] objects3 = new NXObject[1];
                            objects3[0] = component1;
                            AttributePropertiesBuilder attributePropertiesBuilder11;
                            attributePropertiesBuilder11 = theSession.AttributeManager.CreateAttributePropertiesBuilder(wp_m, objects3, NXOpen.AttributePropertiesBuilder.OperationType.None);

                            attributePropertiesBuilder11.IsArray = false;

                            attributePropertiesBuilder11.IsArray = false;

                            attributePropertiesBuilder11.IsArray = false;

                            attributePropertiesBuilder11.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

                            attributePropertiesBuilder11.Units = "MilliMeter";

                            NXObject[] objects4 = new NXObject[1];
                            objects4[0] = component1;
                            MassPropertiesBuilder massPropertiesBuilder1;
                            massPropertiesBuilder1 = wp_m.PropertiesManager.CreateMassPropertiesBuilder(objects4);

                            SelectNXObjectList selectNXObjectList1;
                            selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

                            NXObject[] objects5;
                            objects5 = selectNXObjectList1.GetArray();

                            massPropertiesBuilder1.LoadPartialComponents = true;

                            massPropertiesBuilder1.Accuracy = 0.99;

                            NXObject[] objects6 = new NXObject[1];
                            objects6[0] = component1;
                            ObjectGeneralPropertiesBuilder objectGeneralPropertiesBuilder1;
                            objectGeneralPropertiesBuilder1 = wp_m.PropertiesManager.CreateObjectGeneralPropertiesBuilder(objects6);

                            SelectNXObjectList selectNXObjectList2;
                            selectNXObjectList2 = objectGeneralPropertiesBuilder1.SelectedObjects;

                            objectGeneralPropertiesBuilder1.NameLocationSpecified = false;

                            objectGeneralPropertiesBuilder1.Index = 1;

                            NXObject[] objects7 = new NXObject[1];
                            objects7[0] = component1;
                            NXOpen.Assemblies.AssembliesParameterPropertiesBuilder assembliesParameterPropertiesBuilder1;
                            assembliesParameterPropertiesBuilder1 = wp_m.PropertiesManager.CreateAssembliesParameterPropertiesBuilder(objects7);

                            NXOpen.Assemblies.SelectComponentList selectComponentList2;
                            selectComponentList2 = assembliesParameterPropertiesBuilder1.SelectedObjects;

                            NXOpen.Assemblies.Component[] objects8;
                            objects8 = selectComponentList2.GetArray();

                            NXOpen.Assemblies.SelectComponentList selectComponentList3;
                            selectComponentList3 = assembliesParameterPropertiesBuilder1.SelectedObjects;

                            NXOpen.Assemblies.Component[] objects9;
                            objects9 = selectComponentList3.GetArray();

                            assembliesGeneralPropertiesBuilder1.LayerOption = NXOpen.Assemblies.AssembliesGeneralPropertiesBuilder.LayerOptions.OriginalLayer;

                            assembliesGeneralPropertiesBuilder1.IntegerQuantity = 874;

                            assembliesGeneralPropertiesBuilder1.RealQuantity = 1.0;

                            attributePropertiesBuilder11.DateValue.FromDateItem.Year = "2016";

                            attributePropertiesBuilder11.DateValue.ToDateItem.Year = "2016";

                            assembliesParameterPropertiesBuilder1.Arrangements = NXOpen.Assemblies.AssembliesParameterPropertiesBuilder.ArrangementOptions.SamePositionInAll;

                            NXObject[] objects10 = new NXObject[1];
                            objects10[0] = component1;
                            attributePropertiesBuilder11.SetAttributeObjects(objects10);

                            attributePropertiesBuilder11.Units = "MilliMeter";

                            NXOpen.Assemblies.SelectComponentList selectComponentList4;
                            selectComponentList4 = assembliesGeneralPropertiesBuilder1.SelectedObjects;

                            NXOpen.Assemblies.Component[] objects11;
                            objects11 = selectComponentList4.GetArray();

                            NXOpen.Assemblies.SelectComponentList selectComponentList5;
                            selectComponentList5 = assembliesGeneralPropertiesBuilder1.SelectedObjects;

                            NXOpen.Assemblies.Component[] objects12;
                            objects12 = selectComponentList5.GetArray();

                            NXOpen.Assemblies.SelectComponentList selectComponentList6;
                            selectComponentList6 = assembliesGeneralPropertiesBuilder1.SelectedObjects;

                            NXOpen.Assemblies.Component[] objects13;
                            objects13 = selectComponentList6.GetArray();

                            NXOpen.Assemblies.SelectComponentList selectComponentList7;
                            selectComponentList7 = assembliesGeneralPropertiesBuilder1.SelectedObjects;

                            NXOpen.Assemblies.Component[] objects14;
                            objects14 = selectComponentList7.GetArray();

                            NXOpen.Assemblies.SelectComponentList selectComponentList8;
                            selectComponentList8 = assembliesGeneralPropertiesBuilder1.SelectedObjects;

                            NXOpen.Assemblies.Component[] objects15;
                            objects15 = selectComponentList8.GetArray();

                            bool nonGeometricState1;
                            nonGeometricState1 = component1.GetNonGeometricState();

                            assembliesGeneralPropertiesBuilder1.ReferenceComponent = NXOpen.Assemblies.AssembliesGeneralPropertiesBuilder.ReferenceComponentOptions.No;

                            NXOpen.Assemblies.SelectComponentList selectComponentList9;
                            selectComponentList9 = assembliesGeneralPropertiesBuilder1.SelectedObjects;

                            NXOpen.Assemblies.Component[] objects16;
                            objects16 = selectComponentList9.GetArray();

                            assembliesGeneralPropertiesBuilder1.Hidden = NXOpen.Assemblies.AssembliesGeneralPropertiesBuilder.HiddenOptions.No;

                            NXOpen.Assemblies.SelectComponentList selectComponentList10;
                            selectComponentList10 = assembliesGeneralPropertiesBuilder1.SelectedObjects;

                            NXOpen.Assemblies.Component[] objects17;
                            objects17 = selectComponentList10.GetArray();

                            assembliesGeneralPropertiesBuilder1.IntegerQuantity = 1;

                            assembliesGeneralPropertiesBuilder1.IntegerQuantity = 874;

                            attributePropertiesBuilder11.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day02;

                            attributePropertiesBuilder11.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Feb;

                            attributePropertiesBuilder11.DateValue.DateItem.Year = "2018";

                            attributePropertiesBuilder11.DateValue.DateItem.Time = "00:00:00";

                            massPropertiesBuilder1.UpdateOnSave = NXOpen.MassPropertiesBuilder.UpdateOptions.No;

                            SelectNXObjectList selectNXObjectList3;
                            selectNXObjectList3 = objectGeneralPropertiesBuilder1.SelectedObjects;

                            NXOpen.Assemblies.SelectComponentList selectComponentList11;
                            selectComponentList11 = assembliesParameterPropertiesBuilder1.SelectedObjects;

                            NXOpen.Assemblies.Component[] objects18;
                            objects18 = selectComponentList11.GetArray();

                            NXOpen.Session.UndoMarkId markId2;
                            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Component Properties");


                            if (busca.ToString().Contains("."))
                            {
                                double x_1 = Double.Parse(busca);
                                assembliesGeneralPropertiesBuilder1.RealQuantity = x_1 / 100;
                            }
                            else
                            {
                                assembliesGeneralPropertiesBuilder1.IntegerQuantity = Int32.Parse(busca);

                            }


                            theSession.DeleteUndoMark(markId2, null);

                            NXOpen.Session.UndoMarkId markId3;
                            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Component Properties");

                            NXObject nXObject1;
                            nXObject1 = assembliesGeneralPropertiesBuilder1.Commit();

                            NXObject nXObject2;
                            nXObject2 = attributePropertiesBuilder11.Commit();

                            NXOpen.MassPropertiesBuilder.UpdateOptions updateoption1;
                            updateoption1 = massPropertiesBuilder1.UpdateOnSave;

                            NXObject nXObject3;
                            nXObject3 = massPropertiesBuilder1.Commit();

                            //NXObject nXObject4;
                            //nXObject4 = objectGeneralPropertiesBuilder1.Commit();

                            NXObject nXObject5;
                            nXObject5 = assembliesParameterPropertiesBuilder1.Commit();

                            NXOpen.Session.UndoMarkId id1;
                            id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

                            int nErrs1;
                            nErrs1 = theSession.UpdateManager.DoUpdate(id1);

                            theSession.DeleteUndoMark(markId3, null);

                            theSession.SetUndoMarkName(id1, "Component Properties");

                            assembliesGeneralPropertiesBuilder1.Destroy();

                            attributePropertiesBuilder11.Destroy();

                            massPropertiesBuilder1.Destroy();

                            objectGeneralPropertiesBuilder1.Destroy();

                            assembliesParameterPropertiesBuilder1.Destroy();
                        }

                    }

                }

            }
        }
        public static string busca_qtd_mp(string expression_name)
        {
            string valor = "";
            theSession = Session.GetSession();
            Part wp_pai = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            try
            {
                Expression Busca_Material = (Expression)wp_pai.Expressions.FindObject(expression_name);

                valor = (Busca_Material.StringValue);

            }
            catch
            {

                valor = "0";
            }
            return valor;
        }
        public static void verifica_revisao_autocad()
        {
            string valor = "";
            theSession = Session.GetSession();
            Part wp_pai = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;


            string[] dados = wp_pai.FullPath.Split('/');

            string codigo = dados[0];
            int rev = Convert.ToInt32(dados[1]);

            int rev_cad = Convert.ToInt32(select_rev(codigo));



            if (rev_cad > rev)
            {
                MessageBox.Show($"Existe uma revisão do item {codigo} no Autocad que é maior que a revisão do NX.\n\nA liberação desta revisão em NX poderá gerar conflito nas informações." +
                    $"\n\nRevisão no AutoCad: {rev_cad}\n\nRevisão no NX: {rev}", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (rev_cad == rev)
            {
                MessageBox.Show($"Já um existe uma revisão \"{rev}\" do item {codigo} no Autocad com a mesma revisão do NX.\n\nA liberação desta revisão em NX poderá gerar conflito nas informações." +
                    $"\n\nRevisão no AutoCad: {rev_cad}\n\nRevisão no NX: {rev}", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        public static string select_rev(string codigo)
        {
            string conexao = @"Server=10.1.1.134;Database=gerenciadorprojeto;User ID=renato_bd;Password=QHXqr0LldujNHDEz;SslMode=none";
            string revisao = "";


            List<int> rev = new List<int>();
            var connection = new MySqlConnection(conexao);
            var command = connection.CreateCommand();
            try
            {
                command.CommandText = "SELECT * FROM cadastroitem " +
                     "where codigo ='" + codigo + "'ORDER BY revisao ASC LIMIT 0, 1";

                connection.Open();

                MySqlDataReader dr;
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    revisao = dr["revisao"].ToString();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            return revisao;
        }
        public void Dispose()
        {
            try
            {
                if (isDisposeCalled == false)
                {
                    //TODO: Add your application code here 
                }
                isDisposeCalled = true;
            }
            catch (NXOpen.NXException ex)
            {
                // ---- Enter your exception handling code here -----
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);

            }
        }

        public static int GetUnloadOption(string arg)
        {
            //Unloads the image explicitly, via an unload dialog
            //return System.Convert.ToInt32(Session.LibraryUnloadOption.Explicitly);

            //Unloads the image immediately after execution within NX
            return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);

            //Unloads the image when the NX session terminates
            // return System.Convert.ToInt32(Session.LibraryUnloadOption.AtTermination);
        }


    }
}