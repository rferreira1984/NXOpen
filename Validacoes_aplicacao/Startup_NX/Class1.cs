using System;
using NXOpen;
using NXOpen.UF;
using NXOpen.Assemblies;
using NXOpen.Drawings;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using NXOpen.Features;
using System.Xml.Linq;
using System.Diagnostics;
using Rotinas_Startup;
using static NXOpen.ProductDemo;

public class Program
{
    // class members
    private static Session theSession;
    private static UI theUI;
    private static UFSession theUfSession;
    public static Program theProgram;
    public static bool isDisposeCalled;
    public static int valor = 0;
    public static string fator = "";
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
            isDisposeCalled = false;
        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----
            //UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
        }
    }

    //------------------------------------------------------------------------------
    //  Explicit Activation
    //      This entry point is used to activate the application explicitly
    //------------------------------------------------------------------------------
    public static int Main(string[] args)
    {
        int retValue = 0;
        try
        {
            theProgram = new Program();

            //TODO: Add your application code here 

            theProgram.Dispose();
        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----

        }
        return retValue;
    }

    //------------------------------------------------------------------------------
    //  NX Startup
    //      This entry point activates the application at NX startup

    //Will work when complete path of the dll is provided to Environment Variable 
    //USER_STARTUP or USER_DEFAULT
    //------------------------------------------------------------------------------
    public static int Startup()
    {
        int retValue = 0;
        try
        {

            theProgram = new Program();

            //TODO: Add your application code here 

            //theSession.Parts.AddPartSavedHandler(new NXOpen.PartCollection.PartSavedHandler(item_fam));

            //theSession.Parts.AddPartCreatedHandler(OnPartCreated);

            /// theSession.Parts.AddPartClosedHandler(OnPartCreated);
            // Registrar o evento para monitorar a criação de peças
         //   theSession.Parts.AddPartClosedHandler(new NXOpen.PartCollection.PartClosedHandler(item_fam));
            // theSession.Parts.AddPartCreatedHandler(new NXOpen.PartCollection.PartCreatedHandler(OnPartCreated));
            //theSession.Parts.AddPartCreatedHandler(new NXOpen.PartCollection.PartCreatedHandler(OnPartCreated));
           // theSession.Parts.AddPartOpenedHandler(new NXOpen.PartCollection.PartOpenedHandler(item_fam));
            // theSession.Parts.AddPartSavedAsHandler(new NXOpen.PartCollection.PartSavedAsHandler(OnPartClose));
            theSession.Parts.AddPartSavedAsHandler(new NXOpen.PartCollection.PartSavedAsHandler(PartCreated_26));
            
            ///    MessageBox.Show("aaaaaa");
            //   System.Diagnostics.Process.Start(@"Y:\Custom_Mascarello\dll\atualizador_dll\Atualizar_startup.exe");

            //  MessageBox.Show("qqqqq");

        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----
            // theUI.NXMessageBox.Show("UI Styler", NXMessageBox.DialogType.Error, ex.Message);
        }
        return retValue;
    }

  

    private static void OnPartCreated(BasePart createdPart)
    {
        UI.GetUI().NXMessageBox.Show("Peça Criada", NXMessageBox.DialogType.Information, "aaaaaaaaaaaaaaa");
        //try
        //{
        //    if (createdPart != null)
        //    {
        //        string message = $"Nova peça criada: {createdPart.Name}";
        //        UI.GetUI().NXMessageBox.Show("Peça Criada", NXMessageBox.DialogType.Information, message);

        //        // Verificar se a peça é membro de uma Part Family
        //        if (createdPart is Part newPart && IsPartFamilyInstance(newPart))
        //        {
        //            UI.GetUI().NXMessageBox.Show("Part Family", NXMessageBox.DialogType.Information,
        //                $"A peça '{createdPart.Name}' é membro de uma Part Family.");
        //            Rotinas_Startup.Program.Verificar_Blank_Fam();
        //            Rotinas_Startup.Program.item_fam();
        //        }
        //        else
        //        {


        //            UI.GetUI().NXMessageBox.Show("Part Family", NXMessageBox.DialogType.Information,
        //                $"A peça '{createdPart.Name}' é membro não de uma Part Family.");
        //        }

        //    }
        //}
        //catch (Exception ex)
        //{
        //    UI.GetUI().NXMessageBox.Show("Erro no Evento PartCreated", NXMessageBox.DialogType.Error, ex.Message);
        //}
    }
    public static void OnPartClose(BasePart newPart)
    {
        try
        {

            // Verificar se a peça recém-criada é um membro de família de peças
            if (IsPartFamilyInstance(newPart))
            {
                string name = newPart.Name;
                string codigo = name.Substring(0, 6);
                string rev = name.Substring(7, 3);


                Rotinas_Startup.Program.item_fam();

            }

         }
        catch (Exception ex)
        {

        }
    }

    public static void item_fam(BasePart p)
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
            //Rotinas_Startup.Program.item_fam();
            //MessageBox.Show(codigo + " " + rev + " -- Este item é de familia vamos gerar o PDF ");
             GerarPDF(codigo, rev);
        }
        else
        {
            MessageBox.Show("Este item não é de familia não vamos gerar o PDF ");
        }
    }
    public static bool IsPartFamilyInstance(BasePart thePart)
    {
        bool isFamilyInstance = false;

        // Obtém a sessão UF (User Function)
        UFSession ufSession = UFSession.GetUFSession();

        // Verifica se a peça é um modelo de família usando o método IsFamilyTemplate
        ufSession.Part.IsFamilyInstance(thePart.Tag, out isFamilyInstance);

        return isFamilyInstance;
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

        NXOpen.NXObject nXObject1;
        nXObject1 = printPDFBuilder1.Commit();

        theSession.DeleteUndoMark(markId3, null);

        theSession.SetUndoMarkName(markId1, "Export PDF");

        printPDFBuilder1.Destroy();

        theSession.DeleteUndoMark(markId1, null);
    }
    
  

    //------------------------------------------------------------------------------
    //  New Part
    //      This user exit is invoked after the following menu item is activated:
    //      "File->New"  

    //Will work when complete path of the dll is provided to Environment Variable 
    //USER_CREATE or USER_DEFAULT
    //------------------------------------------------------------------------------
    public static void PartCreated1(BasePart p)
    {
        string ver_exp = "0";

        Session theSession = Session.GetSession();
        Part wp_m = theSession.Parts.Work;
        Part displayPart = theSession.Parts.Display;

        Unit nullUnit = null;

        try
        {
            Expression Busca_Material = (Expression)wp_m.Expressions.FindObject("mp");
            ver_exp = "1";
        }
        catch (Exception)
        {
            ver_exp = "0";
        }
        if (ver_exp == "1")
        {

            // ----------------------------------------------
            //   Menu: File->Properties
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXObject[] objects1 = new NXObject[1];
            objects1[0] = wp_m;
            AttributePropertiesBuilder attributePropertiesBuilder1;
            attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(wp_m, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            attributePropertiesBuilder1.Units = "MilliMeter";

            NXObject[] objects2 = new NXObject[1];
            objects2[0] = wp_m;
            MassPropertiesBuilder massPropertiesBuilder1;
            massPropertiesBuilder1 = wp_m.PropertiesManager.CreateMassPropertiesBuilder(objects2);

            SelectNXObjectList selectNXObjectList1;
            selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

            NXObject[] objects3;
            objects3 = selectNXObjectList1.GetArray();

            massPropertiesBuilder1.LoadPartialComponents = true;

            massPropertiesBuilder1.Accuracy = 0.99;

            NXObject[] objects4 = new NXObject[1];
            objects4[0] = wp_m;
            PreviewPropertiesBuilder previewPropertiesBuilder1;
            previewPropertiesBuilder1 = wp_m.PropertiesManager.CreatePreviewPropertiesBuilder(objects4);

            attributePropertiesBuilder1.DateValue.FromDateItem.Year = "2015";

            attributePropertiesBuilder1.DateValue.ToDateItem.Year = "2015";

            previewPropertiesBuilder1.StorePartPreview = true;

            previewPropertiesBuilder1.StoreModelViewPreview = true;

            previewPropertiesBuilder1.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

            NXObject[] objects5 = new NXObject[1];
            objects5[0] = wp_m;
            attributePropertiesBuilder1.SetAttributeObjects(objects5);

            attributePropertiesBuilder1.Units = "MilliMeter";

            theSession.SetUndoMarkName(markId1, "Displayed Part Properties Dialog");

            attributePropertiesBuilder1.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day13;

            attributePropertiesBuilder1.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Jan;

            attributePropertiesBuilder1.DateValue.DateItem.Year = "2016";

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

            attributePropertiesBuilder1.Title = "DB_QTD_BRUTO";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Link to Expression");

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Launch Expression Editor");

            // ----------------------------------------------
            //   Dialog Begin Expressions
            // ----------------------------------------------
            attributePropertiesBuilder1.StringValue = "0.00";

            attributePropertiesBuilder1.IsReferenceType = false;

            Expression expression1 = (Expression)wp_m.Expressions.FindObject("format_qtd_bruto");
            attributePropertiesBuilder1.Expression = expression1;

            theSession.DeleteUndoMarksUpToMark(markId3, null, false);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            NXObject nXObject1;
            nXObject1 = attributePropertiesBuilder1.Commit();

            NXOpen.MassPropertiesBuilder.UpdateOptions updateoption1;
            updateoption1 = massPropertiesBuilder1.UpdateOnSave;

            NXObject nXObject2;
            nXObject2 = massPropertiesBuilder1.Commit();

            wp_m.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

            NXObject nXObject3;
            nXObject3 = previewPropertiesBuilder1.Commit();

            NXOpen.Session.UndoMarkId id1;
            id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(id1);

            theSession.DeleteUndoMark(markId4, null);

            theSession.SetUndoMarkName(markId1, "Displayed Part Properties");

            attributePropertiesBuilder1.Destroy();

            massPropertiesBuilder1.Destroy();

            previewPropertiesBuilder1.Destroy();

            theSession.DeleteUndoMark(id1, null);

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXObject[] objects6 = new NXObject[1];
            objects6[0] = wp_m;
            AttributePropertiesBuilder attributePropertiesBuilder2;
            attributePropertiesBuilder2 = theSession.AttributeManager.CreateAttributePropertiesBuilder(wp_m, objects6, NXOpen.AttributePropertiesBuilder.OperationType.None);

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            attributePropertiesBuilder2.Units = "MilliMeter";

            NXObject[] objects7 = new NXObject[1];
            objects7[0] = wp_m;
            MassPropertiesBuilder massPropertiesBuilder2;
            massPropertiesBuilder2 = wp_m.PropertiesManager.CreateMassPropertiesBuilder(objects7);

            SelectNXObjectList selectNXObjectList2;
            selectNXObjectList2 = massPropertiesBuilder2.SelectedObjects;

            NXObject[] objects8;
            objects8 = selectNXObjectList2.GetArray();

            massPropertiesBuilder2.LoadPartialComponents = true;

            massPropertiesBuilder2.Accuracy = 0.99;

            NXObject[] objects9 = new NXObject[1];
            objects9[0] = wp_m;
            PreviewPropertiesBuilder previewPropertiesBuilder2;
            previewPropertiesBuilder2 = wp_m.PropertiesManager.CreatePreviewPropertiesBuilder(objects9);

            attributePropertiesBuilder2.DateValue.FromDateItem.Year = "2015";

            attributePropertiesBuilder2.DateValue.ToDateItem.Year = "2015";

            previewPropertiesBuilder2.StorePartPreview = true;

            previewPropertiesBuilder2.StoreModelViewPreview = true;

            previewPropertiesBuilder2.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

            NXObject[] objects10 = new NXObject[1];
            objects10[0] = wp_m;
            attributePropertiesBuilder2.SetAttributeObjects(objects10);

            attributePropertiesBuilder2.Units = "MilliMeter";

            theSession.SetUndoMarkName(markId5, "Displayed Part Properties Dialog");

            attributePropertiesBuilder2.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day13;

            attributePropertiesBuilder2.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Jan;

            attributePropertiesBuilder2.DateValue.DateItem.Year = "2016";

            attributePropertiesBuilder2.DateValue.DateItem.Time = "00:00:00";

            massPropertiesBuilder2.UpdateOnSave = NXOpen.MassPropertiesBuilder.UpdateOptions.No;

            // ----------------------------------------------
            //   Dialog Begin Displayed Part Properties
            // ----------------------------------------------
            attributePropertiesBuilder2.Title = "";

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.StringValue = "";

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.StringValue = "";

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.Category = "M4_it_mascarelloRevisionMaster";

            attributePropertiesBuilder2.Title = "DB_FATOR_PERDA";

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.StringValue = "";

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.IsArray = false;

            NXOpen.Session.UndoMarkId markId6;
            markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Link to Expression");

            NXOpen.Session.UndoMarkId markId7;
            markId7 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Launch Expression Editor");

            // ----------------------------------------------
            //   Dialog Begin Expressions
            // ----------------------------------------------
            attributePropertiesBuilder2.StringValue = fator;

            attributePropertiesBuilder2.IsReferenceType = false;

            Expression expression2 = (Expression)wp_m.Expressions.FindObject("format_fator_perda");
            attributePropertiesBuilder2.Expression = expression2;

            theSession.DeleteUndoMarksUpToMark(markId7, null, false);

            NXOpen.Session.UndoMarkId markId8;
            markId8 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            NXObject nXObject4;
            nXObject4 = attributePropertiesBuilder2.Commit();

            NXOpen.MassPropertiesBuilder.UpdateOptions updateoption2;
            updateoption2 = massPropertiesBuilder2.UpdateOnSave;

            NXObject nXObject5;
            nXObject5 = massPropertiesBuilder2.Commit();

            wp_m.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

            NXObject nXObject6;
            nXObject6 = previewPropertiesBuilder2.Commit();

            NXOpen.Session.UndoMarkId id2;
            id2 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(id2);

            theSession.DeleteUndoMark(markId8, null);

            theSession.SetUndoMarkName(markId5, "Displayed Part Properties");

            attributePropertiesBuilder2.Destroy();

            massPropertiesBuilder2.Destroy();

            previewPropertiesBuilder2.Destroy();

            theSession.DeleteUndoMark(id2, null);

            NXOpen.Session.UndoMarkId markId9;
            markId9 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXObject[] objects11 = new NXObject[1];
            objects11[0] = wp_m;
            AttributePropertiesBuilder attributePropertiesBuilder3;
            attributePropertiesBuilder3 = theSession.AttributeManager.CreateAttributePropertiesBuilder(wp_m, objects11, NXOpen.AttributePropertiesBuilder.OperationType.None);

            attributePropertiesBuilder3.IsArray = false;

            attributePropertiesBuilder3.IsArray = false;

            attributePropertiesBuilder3.IsArray = false;

            attributePropertiesBuilder3.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            attributePropertiesBuilder3.Units = "MilliMeter";

            NXObject[] objects12 = new NXObject[1];
            objects12[0] = wp_m;
            MassPropertiesBuilder massPropertiesBuilder3;
            massPropertiesBuilder3 = wp_m.PropertiesManager.CreateMassPropertiesBuilder(objects12);

            SelectNXObjectList selectNXObjectList3;
            selectNXObjectList3 = massPropertiesBuilder3.SelectedObjects;

            NXObject[] objects13;
            objects13 = selectNXObjectList3.GetArray();

            massPropertiesBuilder3.LoadPartialComponents = true;

            massPropertiesBuilder3.Accuracy = 0.99;

            NXObject[] objects14 = new NXObject[1];
            objects14[0] = wp_m;
            PreviewPropertiesBuilder previewPropertiesBuilder3;
            previewPropertiesBuilder3 = wp_m.PropertiesManager.CreatePreviewPropertiesBuilder(objects14);

            attributePropertiesBuilder3.DateValue.FromDateItem.Year = "2015";

            attributePropertiesBuilder3.DateValue.ToDateItem.Year = "2015";

            previewPropertiesBuilder3.StorePartPreview = true;

            previewPropertiesBuilder3.StoreModelViewPreview = true;

            previewPropertiesBuilder3.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

            NXObject[] objects15 = new NXObject[1];
            objects15[0] = wp_m;
            attributePropertiesBuilder3.SetAttributeObjects(objects15);

            attributePropertiesBuilder3.Units = "MilliMeter";

            theSession.SetUndoMarkName(markId9, "Displayed Part Properties Dialog");

            attributePropertiesBuilder3.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day13;

            attributePropertiesBuilder3.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Jan;

            attributePropertiesBuilder3.DateValue.DateItem.Year = "2016";

            attributePropertiesBuilder3.DateValue.DateItem.Time = "00:00:00";

            massPropertiesBuilder3.UpdateOnSave = NXOpen.MassPropertiesBuilder.UpdateOptions.No;

            // ----------------------------------------------
            //   Dialog Begin Displayed Part Properties
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId10;
            markId10 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            theSession.DeleteUndoMark(markId10, null);

            NXOpen.Session.UndoMarkId markId11;
            markId11 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            NXObject nXObject7;
            nXObject7 = attributePropertiesBuilder3.Commit();

            NXOpen.MassPropertiesBuilder.UpdateOptions updateoption3;
            updateoption3 = massPropertiesBuilder3.UpdateOnSave;

            NXObject nXObject8;
            nXObject8 = massPropertiesBuilder3.Commit();

            wp_m.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

            NXObject nXObject9;
            nXObject9 = previewPropertiesBuilder3.Commit();

            NXOpen.Session.UndoMarkId id3;
            id3 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

            int nErrs3;
            nErrs3 = theSession.UpdateManager.DoUpdate(id3);

            theSession.DeleteUndoMark(markId11, null);

            theSession.SetUndoMarkName(id3, "Displayed Part Properties");

            attributePropertiesBuilder3.Destroy();

            massPropertiesBuilder3.Destroy();

            previewPropertiesBuilder3.Destroy();



        }

    }

    public static void PartCreated_26(BasePart p)
    {

        Session theSession = Session.GetSession();
        Part wp = theSession.Parts.Work;
        Part displayPart = theSession.Parts.Display;

        string name = wp.Name;
        string[] _rev = name.Split('/');
        string rev = _rev[1].Substring(0, 3);

        if (rev == "000")
        {
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
                            wp.ComponentAssembly.RemoveComponent(comp);
                            MessageBox.Show($"O código Agrupador de Contraventamentos foi removido.\n" +
                                $"Gere um novo codigo de Agrupador para este Item.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }
    }

    //------------------------------------------------------------------------------
    //  QUANDO É CRIADO UM COMPONENTE A PARTIR DO COMANDO NO NX 
    //      This user exit is invoked after the following menu item is activated:
    //      "File->New"  

    //Will work when complete path of the dll is provided to Environment Variable 
    //USER_CREATE or USER_DEFAULT
    //------------------------------------------------------------------------------
    public static void PartSave(BasePart p)
    {
       
    }
  

    public static void seta_padrao_drafting(BasePart p)
    {


        try
        {
            theProgram = new Program();



            //TODO: setagem do Drafting standards quando abre o arquivo


            Part part = (Part)p;

            Session theSession = Session.GetSession();


            NXOpen.Preferences.LoadDraftingStandardBuilder loadDraftingStandardBuilder1;
            loadDraftingStandardBuilder1 = part.Preferences.DraftingPreference.CreateLoadDraftingStandardBuilder();

            loadDraftingStandardBuilder1.WelcomeMode = false;

            loadDraftingStandardBuilder1.Level = NXOpen.Preferences.LoadDraftingStandardBuilder.LoadLevel.User;

            loadDraftingStandardBuilder1.Name = "ISO(MASCARELLO)";

            NXObject nXObject1;
            nXObject1 = loadDraftingStandardBuilder1.Commit();
        }
        catch (NXOpen.NXException)
        {

        }


    }
    //------------------------------------------------------------------------------
    // Following method disposes all the class members
    //------------------------------------------------------------------------------
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

        }
    }
    private void teste(object sender, KeyPressEventArgs e)
    {

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



