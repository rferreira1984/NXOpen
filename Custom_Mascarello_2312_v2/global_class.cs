using NXOpen;
using NXOpen.PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static NXOpen.UF.UFUgmgr;
using Expression = NXOpen.Expression;



namespace Custom_Mascarello
{
    public class global_class
    {
        public static void preenche_expression(string expression_name, string value, Part workPart)
        {
            Session theSession = Session.GetSession();

            theSession.Preferences.Modeling.UpdatePending = false;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Expression");

            Expression expression1 = (Expression)workPart.Expressions.FindObject(expression_name);
            expression1.IsNoEdit = false;
            Unit unit1 = null;
            workPart.Expressions.EditWithUnits(expression1, unit1, value);

            theSession.Preferences.Modeling.UpdatePending = false;

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId1);
        }

        public static string Create_fam_cv_2312_x(string cod_fam, string desc_padrao, Dictionary<string, double> Dict, Part displayAnterior)
        {
            Session theSession = Session.GetSession();
            PartLoadStatus loadStatus;


            // Part displayAnterior = theSession.Parts.Display;
            bool cnc = true;


            double tol_COMP = 0.0;
            double tol_ANG_A_V = 0.0;
            double tol_ANG_B_V = 0.0;
            double tol_ANG_A_H = 0.0;
            double tol_ANG_B_H = 0.0;


            string arquivo = "";
            //string cod_fam_desc = "";

            string item_pai = cod_fam;

            string[] attributos = Dict.Keys.ToArray();// { "COMP", "ANG_A_V", "ANG_B_V", "ANG_B_V", "ANG_B_H" };


            // string desc_padrao = conexao_bd.select_desc(cod_fam_desc);
            //  MessageBox.Show(desc_padrao);

            string[] descricao = desc_padrao.Split(';');

            string[] expressao = new string[attributos.Length];
            for (int i = 0; i <= attributos.Length - 1; i++)
            {
                expressao[i] = attributos[i];
            }


            string descricao_final = descricao[0];
            double[] valor = new double[attributos.Length];
            for (int i = 0; i <= attributos.Length - 1; i++)
            {
                var item = Dict.ElementAt(i);
                descricao_final = descricao_final + item.Value + descricao[i + 1];
                //LogMessage_x(descricao_final);
                valor[i] = item.Value;

            }
            descricao_final = descricao_final.Replace("\"", "");

            double[] tolerancia = new double[attributos.Length];
            for (int i = 0; i <= attributos.Length - 1; i++)
            {
                tolerancia[i] = Convert.ToDouble(0.1);
            }
            string codigo_novo = "";

            bool ver_pesquisa = false;
            bool existe = false;

            if (ver_pesquisa == false)
            {


                //LogMessage_x("Criando o item....aguarde");




                string item_type = "M4_it_cv";
                descricao_final = "CV_" + descricao_final;



                //  codigo_novo = "este item for criado - teste";
                (codigo_novo, existe) = Custom_Mascarello.Create.CreateMember_new(cod_fam, descricao_final, expressao, valor, 0, tolerancia, "", descricao_final, item_type, "", false, null, null);



                if (existe == false)
                {
                   // LogMessage_x($"Item CV {codigo_novo} criado");
                }
                if (existe == true)
                {
                    //LogMessage_x($"Item CV {codigo_novo} existente");
                }
                return codigo_novo;
            }

            theSession.Parts.SetDisplay(displayAnterior, true, true, out loadStatus);

            return codigo_novo;
        }
        public static string saveas_x(string db_name)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;


           
           

            NXOpen.PDM.PartOperationCopyBuilder partOperationCopyBuilder1;
            partOperationCopyBuilder1 = theSession.PdmSession.CreateCopyOperationBuilder(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            partOperationCopyBuilder1.SetOperationSubType(NXOpen.PDM.PartOperationCopyBuilder.OperationSubType.Default);

            partOperationCopyBuilder1.DefaultDestinationFolder = ":NX_BUS:REUSE_MASCARELLO:CONJUNTOS_X";

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
            

            attributePropertiesBuilder2.Category = "M4_it_mascarello";



            NXOpen.NXObject[] sourceobjects11;
            sourceobjects11 = logicalobjects2[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] objects9 = new NXOpen.NXObject[0];
            attributePropertiesBuilder2.SetAttributeObjects(objects9);

            NXOpen.NXObject[] objects10 = new NXOpen.NXObject[1];
            objects10[0] = sourceobjects7[0];
            attributePropertiesBuilder2.SetAttributeObjects(objects10);

            attributePropertiesBuilder2.Title = "DB_PART_NAME";

            attributePropertiesBuilder2.StringValue = db_name;

            attributePropertiesBuilder2.Category = "M4_it_mascarello";

            bool changed3;
            changed3 = attributePropertiesBuilder2.CreateAttribute();

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

            NXOpen.NXObject[] objects12 = new NXOpen.NXObject[1];
            objects12[0] = logicalobjects2[0];
            NXOpen.NXObject[] properties1 = new NXOpen.NXObject[1];
            properties1[0] = nXObject3;
            NXOpen.ErrorList errorList1;
            errorList1 = partOperationCopyBuilder1.AutoAssignAttributesWithNamingPattern(objects12, properties1);

            errorList1.Dispose();
            NXOpen.PDM.ErrorMessageHandler errorMessageHandler1;
            errorMessageHandler1 = partOperationCopyBuilder1.GetErrorMessageHandler(true);

            NXOpen.Session.UndoMarkId markId7;
            markId7 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Save Parts As");

            theSession.DeleteUndoMark(markId7, null);

            NXOpen.Session.UndoMarkId markId8;
            markId8 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Save Parts As");

            partOperationCopyBuilder1.ValidateLogicalObjectsToCommit();

            NXOpen.PDM.ErrorMessageHandler errorMessageHandler2;
            errorMessageHandler2 = partOperationCopyBuilder1.GetErrorMessageHandler(true);

            NXOpen.PDM.ErrorMessageHandler errorMessageHandler3;
            errorMessageHandler3 = partOperationCopyBuilder1.GetErrorMessageHandler(true);

            NXOpen.NXObject nXObject4;
            nXObject4 = partOperationCopyBuilder1.Commit();

            NXOpen.PDM.ErrorMessageHandler errorMessageHandler4;
            errorMessageHandler4 = partOperationCopyBuilder1.GetErrorMessageHandler(true);

            theSession.DeleteUndoMark(markId8, null);

            partOperationCopyBuilder1.Destroy();

            attributePropertiesBuilder2.Destroy();



            theSession.CleanUpFacetedFacesAndEdges();


            string new_id = "";
            new_id = workPart.GetStringUserAttribute("DB_PART_NO", -1);

            frm_contraventamentos frm_Contraventamentos = new frm_contraventamentos();

            frm_Contraventamentos.criacao_externa();

            return new_id;
        }
    }
}
