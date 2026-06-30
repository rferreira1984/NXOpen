using NXOpen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_Mascarello
{
    public class SaveAS
    {

        public static string saveas_x(string db_part_name)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            NXOpen.PDM.PartBuilder partbuilder = null;

            string new_id = "";

            partbuilder = theSession.Parts.PDMPartManager.NewPartFromPartBuilder();

            bool flg = false;
            while (flg) // inicio LOOPING 
            {
                try
                {
                    System.Threading.Thread.Sleep(3000); // temporizador looping para não ficar usando o processador, tempo 3 segundos
                    partbuilder = theSession.Parts.PDMPartManager.NewPartFromPartBuilder();
                    new_id = partbuilder.AssignPartNumber("M4_it_mascarello");

                    flg = false;
                    if (new_id == "" || new_id == "(null)")
                    {
                        flg = true;
                    }
                }
                catch
                {
                    flg = true;
                }
            }
           // new_id = partbuilder.AssignPartNumber("M4_it_mascarello");


            NXOpen.PDM.PartOperationCopyBuilder partOperationCopyBuilder1;
            partOperationCopyBuilder1 = theSession.PdmSession.CreateCopyOperationBuilder(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            partOperationCopyBuilder1.SetOperationSubType(NXOpen.PDM.PartOperationCopyBuilder.OperationSubType.Default);

            partOperationCopyBuilder1.DefaultDestinationFolder = ":CJ_X";

            partOperationCopyBuilder1.DependentFilesToCopyOption = NXOpen.PDM.PartOperationCopyBuilder.CopyDependentFiles.All;

            partOperationCopyBuilder1.ReplaceAllComponentsInSession = true;

            partOperationCopyBuilder1.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.Revise);

            NXOpen.BasePart[] selectedparts1 = new NXOpen.BasePart[1];
            selectedparts1[0] = workPart;
            NXOpen.BasePart[] failedparts1;
            partOperationCopyBuilder1.SetSelectedPartsToCopy(selectedparts1, out failedparts1);

            NXOpen.PDM.LogicalObject[] logicalobjects1;
            partOperationCopyBuilder1.CreateLogicalObjects(out logicalobjects1);

            NXOpen.NXObject[] sourceobjects1;
            sourceobjects1 = logicalobjects1[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] sourceobjects2;
            sourceobjects2 = logicalobjects1[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] sourceobjects3;
            sourceobjects3 = logicalobjects1[0].GetUserAttributeSourceObjects();



            NXOpen.NXObject[] sourceobjects4;
            sourceobjects4 = logicalobjects1[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] sourceobjects5;
            sourceobjects5 = logicalobjects1[0].GetUserAttributeSourceObjects();

            partOperationCopyBuilder1.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            NXOpen.NXObject[] sourceobjects6;
            sourceobjects6 = logicalobjects1[0].GetUserAttributeSourceObjects();

            NXOpen.BasePart[] selectedparts2 = new NXOpen.BasePart[1];
            selectedparts2[0] = workPart;
            NXOpen.BasePart[] failedparts2;
            partOperationCopyBuilder1.SetSelectedPartsToCopy(selectedparts2, out failedparts2);

            NXOpen.PDM.LogicalObject[] logicalobjects2;
            partOperationCopyBuilder1.CreateLogicalObjects(out logicalobjects2);

            NXOpen.NXObject[] sourceobjects7;
            sourceobjects7 = logicalobjects2[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] sourceobjects8;
            sourceobjects8 = logicalobjects2[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] sourceobjects9;
            sourceobjects9 = logicalobjects2[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] sourceobjects10;
            sourceobjects10 = logicalobjects2[0].GetUserAttributeSourceObjects();

            NXOpen.BasePart nullNXOpen_BasePart = null;
            NXOpen.NXObject[] objects6 = new NXOpen.NXObject[0];
            NXOpen.AttributePropertiesBuilder attributePropertiesBuilder2;
            attributePropertiesBuilder2 = theSession.AttributeManager.CreateAttributePropertiesBuilder(nullNXOpen_BasePart, objects6, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            NXOpen.NXObject[] objects7 = new NXOpen.NXObject[0];
            attributePropertiesBuilder2.SetAttributeObjects(objects7);

            NXOpen.NXObject[] objects8 = new NXOpen.NXObject[1];
            objects8[0] = sourceobjects7[0];
            attributePropertiesBuilder2.SetAttributeObjects(objects8);

            attributePropertiesBuilder2.Title = "DB_PART_NO";

            attributePropertiesBuilder2.Category = "M4_it_mascarello";

            attributePropertiesBuilder2.StringValue = new_id;

            attributePropertiesBuilder2.Category = "M4_it_mascarello";

          

            NXOpen.NXObject[] sourceobjects11;
            sourceobjects11 = logicalobjects2[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] objects9 = new NXOpen.NXObject[0];
            attributePropertiesBuilder2.SetAttributeObjects(objects9);

            NXOpen.NXObject[] objects10 = new NXOpen.NXObject[1];
            objects10[0] = sourceobjects7[0];
            attributePropertiesBuilder2.SetAttributeObjects(objects10);

            attributePropertiesBuilder2.Title = "DB_PART_NAME";

            attributePropertiesBuilder2.StringValue = "VAMOS_CRIAR_SAVEAS";

            attributePropertiesBuilder2.Category = "M4_it_mascarello";

            NXOpen.NXObject[] sourceobjects12;
            sourceobjects12 = logicalobjects2[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] objects11 = new NXOpen.NXObject[0];
            attributePropertiesBuilder2.SetAttributeObjects(objects11);

            string[] attributetitles1 = new string[1];
            attributetitles1[0] = "DB_PART_REV";
            string[] titlepatterns1 = new string[1];
            titlepatterns1[0] = "NNN";
            NXOpen.NXObject nXObject3;
            nXObject3 = partOperationCopyBuilder1.CreateAttributeTitleToNamingPatternMap(attributetitles1, titlepatterns1);

            partOperationCopyBuilder1.ValidateLogicalObjectsToCommit();


            NXOpen.NXObject nXObject4;
            nXObject4 = partOperationCopyBuilder1.Commit();

            partOperationCopyBuilder1.Destroy();

            attributePropertiesBuilder2.Destroy();

            theSession.CleanUpFacetedFacesAndEdges();
            return new_id;
        }
    }
}
