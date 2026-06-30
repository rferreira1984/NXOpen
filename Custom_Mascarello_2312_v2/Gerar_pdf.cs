using NXOpen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Custom_Mascarello
{
    public class Gerar_pdf
    {
        public static void GerarPDF(string codigo, string rev)
        {
            MessageBox.Show("PDF_ITEM_FAM");
            NXOpen.Session theSession = NXOpen.Session.GetSession();
       

            NXOpen.BasePart basePart1;
            NXOpen.PartLoadStatus partLoadStatus1;
            basePart1 = theSession.Parts.OpenActiveDisplay("@DB/" + codigo + "/" + rev, NXOpen.DisplayPartOption.AllowAdditional, out partLoadStatus1);

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
    }
}
