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
using MiniSnap;
using Snap;
using System.Collections;
using NXOpen.Tooling;
namespace Custom_Mascarello
{
    public partial class frm_Tubo : Form
    {
        public frm_Tubo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            //Session theSession = Session.GetSession();
            //Part workPart = theSession.Parts.Work;
            //Part displayPart = theSession.Parts.Display;

            //Selection.TaggedObjectSelectionCallback teste;
            //teste = 
            //SelectionIntentRule[] rules1 = new SelectionIntentRule[3];

            //NXOpen.Session.UndoMarkId markId1;
            //markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");
            //NXOpen.Features.Feature nullFeatures_Feature = null;

            //theUI = UI.GetUI();
            //NXOpen.NXObject selobj;
            //Point3d cursor;
            //Selection.SelectionType[] typeArray = { Selection.SelectionType.Curves };
            //Selection.Response resp = theUI.SelectionManager.SelectObject("Selecione o caminho", "Selecione o caminho", Selection.SelectionScope.WorkPart, false, typeArray, out selobj, out cursor);
           
           

            //    Curve line1 = (Curve)selobj;
            
            //SelectionHandle selectH;
            //selectH = changeDialog.GetSelectionHandle();
            //NXOpen.Selection.MaskTriple[] selectionMask_array = new NXOpen.Selection.MaskTriple[1];
            //NXOpen.Selection.MaskTriple selectionMask_arrayElem;
            //selectionMask_arrayElem.Type = NXOpen.Selection.SelectionType.Curves;
            //selectionMask_arrayElem.Subtype = NXOpen.UF.UFConstants.line;
            //selectionMask_arrayElem.SolidBodySubtype = NXOpen.UF.UFConstants.UF_UI_SEL_FEATURE_ANY_EDGE;
            //selectionMask_array[0] = selectionMask_arrayElem;
            //Following sets the Selection mask for Edge
            //theUI.SelectionManager.SetSelectionMask(selectH, NXOpen.Selection.SelectionAction.ClearAndEnableSpecific, selectionMask_array);
 
            //NXOpen.Features.DatumPlaneBuilder datumPlane;
            //datumPlane = workPart.Features.CreateDatumPlaneBuilder(nullFeatures_Feature);
            //Plane plane1;
            //plane1 = datumPlane.GetPlane();
            //plane1.SetMethod(PlaneTypes.MethodType.CurvePoint);
            //NXOpen.Point point3;
            //Scalar scalar2;
            //scalar2 = workPart.Scalars.CreateScalar(0.0, NXOpen.Scalar.DimensionalityType.None, NXOpen.SmartObject.UpdateOption.WithinModeling);
            //point3 = workPart.Points.CreatePoint(line1, scalar2, NXOpen.SmartObject.UpdateOption.WithinModeling);
            //NXObject[] geom2 = new NXObject[2];
            //Scalar scalar3 = (Scalar)workPart.FindObject("ENTITY 215 1");
            //Direction direction1;
            //direction1 = workPart.Directions.CreateDirection(line1, scalar3, NXOpen.Direction.OnCurveOption.Tangent, Sense.Forward, NXOpen.SmartObject.UpdateOption.WithinModeling);
            //geom2[0] = point3;
            //geom2[1] = direction1;
            //plane1.SetGeometry(geom2);
            //plane1.SetAlternate(NXOpen.PlaneTypes.AlternateType.One);
            //plane1.Evaluate();
            //NXOpen.Features.Feature feature1;
            //feature1 = datumPlane.CommitFeature();
            //NXOpen.Features.DatumPlaneFeature datumPlaneFeature1 = (NXOpen.Features.DatumPlaneFeature)feature1;
            //DatumPlane datumPlane1;
            //datumPlane1 = datumPlaneFeature1.DatumPlane;
            //datumPlane1.SetReverseSection(false);

            //plane1 = workPart.Planes.CreatePlane(datumPlaneFeature1);
            //-------------------------------------------------------------/-------------------------




            //NXOpen.Scalar scalar1;
            //scalar1 = workPart.Scalars.CreateScalar(0.0, NXOpen.Scalar.DimensionalityType.None, NXOpen.SmartObject.UpdateOption.WithinModeling);
            //NXOpen.Point point2;
            //point2 = workPart.Points.CreatePoint(line1, scalar1, NXOpen.SmartObject.UpdateOption.WithinModeling);


            //NXOpen.Sketch nullNXOpen_Sketch = null;
            //NXOpen.SketchInPlaceBuilder sketchInPlaceBuilder1;
            //sketchInPlaceBuilder1 = workPart.Sketches.CreateNewSketchInPlaceBuilder(nullNXOpen_Sketch);
            //NXOpen.SelectISurface selectISurface1;
            //selectISurface1 = sketchInPlaceBuilder1.PlaneOrFace;

            //selectISurface1.Value = plane1;
            //NXOpen.SelectIReferenceAxis selectIReferenceAxis1;
            //selectIReferenceAxis1 = sketchInPlaceBuilder1.Axis;


            //NXOpen.Point origem = point3;
            //sketchInPlaceBuilder1.SketchOrigin = origem;
            //sketchInPlaceBuilder1.PlaneOption = NXOpen.Sketch.PlaneOption.Inferred;
            //sketchInPlaceBuilder1.CreateIntermediateDatumCsys = false;
            //sketchInPlaceBuilder1.MakeOriginAssociative = true;
            //sketchInPlaceBuilder1.ProjectWorkPartOrigin = false;
            //theSession.Preferences.Sketch.CreateInferredConstraints = true;
            //theSession.Preferences.Sketch.ContinuousAutoDimensioning = true;
            //theSession.Preferences.Sketch.DimensionLabel = NXOpen.Preferences.SketchPreferences.DimensionLabelType.Expression;
            //theSession.Preferences.Sketch.TextSizeFixed = true;
            //theSession.Preferences.Sketch.FixedTextSize = 3.0;
            //theSession.Preferences.Sketch.ConstraintSymbolSize = 3.0;
            //theSession.Preferences.Sketch.DisplayObjectColor = false;
            //theSession.Preferences.Sketch.DisplayObjectName = true;


            //sketchInPlaceBuilder1 = workPart.Sketches.CreateNewSketchInPlaceBuilder(nullNXOpen_Sketch);
            //NXOpen.Unit unit1 = (NXOpen.Unit)workPart.UnitCollection.FindObject("MilliMeter");
            //NXOpen.Expression expression1;
            //expression1 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);
            //NXOpen.Expression expression2;
            //expression2 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);
            //NXOpen.SketchAlongPathBuilder sketchAlongPathBuilder1;
            //sketchAlongPathBuilder1 = workPart.Sketches.CreateSketchAlongPathBuilder(nullNXOpen_Sketch);
            //sketchAlongPathBuilder1.PlaneLocation.Expression.RightHandSide = "0";
            //theSession.SetUndoMarkName(markId1, "Create Sketch Dialog");
            //NXOpen.Point3d coordinates1 = new NXOpen.Point3d(line1.StartPoint.X, line1.StartPoint.Y, line1.StartPoint.Z);
            //NXOpen.Point point1;
            //point1 = workPart.Points.CreatePoint(coordinates1);
            //NXOpen.Point3d point22 = new NXOpen.Point3d(line1.StartPoint.X, line1.StartPoint.Y, line1.StartPoint.Z);
            //sketchInPlaceBuilder1.PlaneOrFace.SetValue(plane1, workPart.ModelingViews.WorkView, point22);
            //sketchInPlaceBuilder1.SketchOrigin = point1;
            //sketchInPlaceBuilder1.PlaneOrFace.Value = null;
            //sketchInPlaceBuilder1.PlaneOrFace.Value = datumPlane1;
            //NXOpen.DatumAxis datumAxis1 = (NXOpen.DatumAxis)workPart.Datums.FindObject("DATUM_CSYS(0) X axis");

            //sketchInPlaceBuilder1.Axis.Value = datumAxis1;
            //int nErrs1;
            //nErrs1 = theSession.UpdateManager.AddToDeleteList(point1);
            //NXOpen.Session.UndoMarkId markId2;
            //markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Create Sketch");
            //theSession.DeleteUndoMark(markId2, null);
            //NXOpen.Session.UndoMarkId markId3;
            //markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Create Sketch");
            //theSession.Preferences.Sketch.CreateInferredConstraints = true;
            //theSession.Preferences.Sketch.ContinuousAutoDimensioning = true;
            //theSession.Preferences.Sketch.DimensionLabel = NXOpen.Preferences.SketchPreferences.DimensionLabelType.Expression;
            //theSession.Preferences.Sketch.TextSizeFixed = true;
            //theSession.Preferences.Sketch.FixedTextSize = 3.0;
            //theSession.Preferences.Sketch.ConstraintSymbolSize = 3.0;
            //theSession.Preferences.Sketch.DisplayObjectColor = false;
            //theSession.Preferences.Sketch.DisplayObjectName = true;
            //NXOpen.NXObject nXObject1;
            //nXObject1 = sketchInPlaceBuilder1.Commit();
            //NXOpen.Sketch sketch2 = (NXOpen.Sketch)nXObject1;
            //NXOpen.Features.Feature feature2;
            //feature2 = sketch2.Feature;
            //sketch2.Activate(NXOpen.Sketch.ViewReorient.True);
            //BasePart tb_lines;
            //PartLoadStatus partLoadStatus1;
            //try
            //{
            //            tb_lines = (Part)theSession.Parts.FindObject("C:\\LIBRARY\\TUBOS\\TB_20X20x2,65.prt");
            //        }
            //        catch
            //        {
            //            tb_lines = theSession.Parts.Open("C:\\LIBRARY\\TUBOS\\TB_20X20x2,65.prt", out partLoadStatus1);
            //        }


            //Sketch[] sketches1 = new Sketch[1];
            //Part part1 = (Part)tb_lines;
            //Sketch sketch1 = (Sketch)part1.Sketches.FindObject("SKETCH_000");
            //sketches1[0] = sketch1;


            //NXOpen.SketchPasteBuilder sketchPasteBuilder1;
            //sketchPasteBuilder1 = workPart.Sketches.CreateSketchPasteBuilder(sketches1);
            //NXOpen.Point3d droplocation1 = new NXOpen.Point3d(line1.StartPoint.X, line1.StartPoint.Y, line1.StartPoint.Z);
            //sketchPasteBuilder1.InitialPasteLocation = droplocation1;

            //NXOpen.NXObject nXObject12;
            //nXObject12 = sketchPasteBuilder1.Commit();
            //NXOpen.NXObject[] objects12;
            //objects12 = sketchPasteBuilder1.GetCommittedObjects();

            //sketchPasteBuilder1.Destroy();
            //theSession.ToolingSession.ClosePart(part1, NXOpen.BasePart.CloseWholeTree.False, NXOpen.BasePart.CloseModified.CloseModified);

            //NXOpen.Session.UndoMarkId markId5;
            //markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Deactivate Sketch");
            //theSession.ActiveSketch.Deactivate(NXOpen.Sketch.ViewReorient.False, NXOpen.Sketch.UpdateLevel.Model);
            
         
        }

        public static void Write(string mystring)
        {

        }

        public UI theUI { get; set; }

        private void frm_Tubo_Load(object sender, EventArgs e)
        {

        }
    }
}