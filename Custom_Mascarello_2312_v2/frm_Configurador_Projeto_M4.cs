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

namespace Custom_Mascarello
{
    public partial class frm_Configurador_Projeto_M4 : Form
    {
        List<string> List = new List<string>(new string[] { });

        string Chassi = "";
        string CT = "";
        double vao_port_tras11 = 0;
        string Pos_Ponto_0 = "0";
        string EE = "0";
        string INICIO_BAG_TRASEIRO = "0";
        string BD = "0";
        private frm_Configurar_Bagageiros_M4 frm_Configurar_Bagageiros_M4;

        public frm_Configurador_Projeto_M4()
        {
            InitializeComponent();
            
        }

        public frm_Configurador_Projeto_M4(frm_Configurar_Bagageiros_M4 frm_Configurar_Bagageiros_M4)
        {
            // TODO: Complete member initialization
            this.frm_Configurar_Bagageiros_M4 = frm_Configurar_Bagageiros_M4;
        }



        private void Configurador_Projeto_Load(object sender, EventArgs e)
        {
            //carregar_opçoes();
            btn_add_port.Enabled = false;
        }
        private void carregar_opçoes()
        {
            XmlDocument xmlBiblioteca = new XmlDocument();
            xmlBiblioteca.Load(@"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\NX\xml_opcoes_M4.xml");
            XElement xml = XElement.Load(@"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\NX\xml_opcoes_M4.xml");
            foreach (XElement x in xml.Elements())
            {
            }

            XmlNodeList ListaFator = default(XmlNodeList);


            ListaFator = xmlBiblioteca.SelectNodes("/Opcoes_roma_M4/Opcao");
            List<string> list = new List<string>(new string[] { });

            int contador = 0;
            foreach (XmlNode fatores in ListaFator)
            {
                contador++;

                int contador1 = fatores.ChildNodes.Count;

                list.Add(fatores.ChildNodes.Item(0).InnerText);
                
                //for (int i = 1; i <= contador1 - 1; i++)
                //{


                //    if (fatores.ChildNodes.Item(0).InnerText == "Chassi")
                //    {
                //        Chassi.Items.Add(fatores.ChildNodes.Item(i).InnerText);
                //    }
                //    if (fatores.ChildNodes.Item(0).InnerText == "Comprimento")
                //    {
                //        Comprimento.Items.Add(fatores.ChildNodes.Item(i).InnerText);
                //    }
                //    if (fatores.ChildNodes.Item(0).InnerText == "EntreEixos")
                //    {
                //        EntreEixos.Items.Add(fatores.ChildNodes.Item(i).InnerText);
                //    }

                //    if (fatores.ChildNodes.Item(0).InnerText == "Ar_condicionado")
                //    {
                //        Ar_condicionado.Items.Add(fatores.ChildNodes.Item(i).InnerText);
                //    }
                //}

            }

            for (int i = 0; i <= list.Count; i++)
            {
                MessageBox.Show(list[i]);
            }
        
        }
        private void btn_aplicar_itens_Click(object sender, EventArgs e)
        {
            Configurar();
            bagageiros();
        }
        public void bagageiros()
        {
            lbl_proccess.Text = "Configurando Bagageiros...";

            

            //---------------------------------------------
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            /// BAG LD
            string vao_1_ld = "";
            string vao_2_ld = "";
            string vao_3_ld = cmbox_vao_3_ld.Text;
            string vao_4_ld = cmbox_vao_4_ld.Text;
            if (cmbox_vao_1_ld.Text == "TQ_ARLA")
            {
                vao_1_ld = "1100";
            }
            if (cmbox_vao_1_ld.Text == "BATERIA")
            {
                vao_1_ld = "600";
            }
            if (cmbox_vao_1_ld.Text != "BATERIA" && cmbox_vao_1_ld.Text != "TQ_ARLA")
            {
                vao_1_ld = cmbox_vao_1_ld.Text;
            }
            if (cmbox_vao_2_ld.Text == "TQ_ARLA")
            {
                vao_2_ld = "1100";
            }
            if (cmbox_vao_2_ld.Text == "BATERIA")
            {
                vao_2_ld = "600";
            }
            if (cmbox_vao_2_ld.Text != "BATERIA" && cmbox_vao_2_ld.Text != "TQ_ARLA")
            {
                vao_2_ld = cmbox_vao_2_ld.Text;
            }
            if (cmbox_vao_4_ld.Text == "NÃO")
            {
                vao_4_ld = "0";
            }

            if (cmbox_vao_4_ld.Text != "NÃO")
            {
                vao_4_ld = cmbox_vao_4_ld.Text;
            }
             ///BAG LE
            string vao_1_le = "";
            string vao_2_le = "";
            string vao_3_le = cmbox_vao_3_le.Text;
            string vao_4_le = cmbox_vao_4_le.Text;
            if (cmbox_vao_1_le.Text == "TQ_ARLA")
            {
                vao_1_le = "1100";
            }
            if (cmbox_vao_1_le.Text == "BATERIA")
            {
                vao_1_le = "600";
            }
            if (cmbox_vao_1_le.Text != "BATERIA" && cmbox_vao_1_le.Text != "TQ_ARLA")
            {
                vao_1_le = cmbox_vao_1_le.Text;
            }
            if (cmbox_vao_2_le.Text == "TQ_ARLA")
            {
                vao_2_le = "1100";
            }
            if (cmbox_vao_2_le.Text == "BATERIA")
            {
                vao_2_le = "600";
            }
            if (cmbox_vao_2_le.Text != "BATERIA" && cmbox_vao_2_le.Text != "TQ_ARLA")
            {
                vao_2_le = cmbox_vao_2_le.Text;
            }
            if (cmbox_vao_4_le.Text == "NÃO")
            {
                vao_4_le = "0";
            }

            if (cmbox_vao_4_le.Text != "NÃO")
            {
                vao_4_le = cmbox_vao_4_le.Text;
            }

            string vao_bag_tras ="";
            if (cmbox_vao_bag_tras.Text == "NÃO") 
            {
                vao_bag_tras = "0";
            }

            if (cmbox_vao_bag_tras.Text != "NÃO")
            {
                vao_bag_tras = cmbox_vao_bag_tras.Text;
            }

            theSession.Preferences.Modeling.UpdatePending = false;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Expression");
            Expression expression1 = (Expression)workPart.Expressions.FindObject("VAO_1_LD");
            Expression expression2 = (Expression)workPart.Expressions.FindObject("VAO_2_LD");
            Expression expression3 = (Expression)workPart.Expressions.FindObject("VAO_3_LD");
            Expression expression4 = (Expression)workPart.Expressions.FindObject("VAO_4_LD");

            Expression expression5 = (Expression)workPart.Expressions.FindObject("VAO_1_LE");
            Expression expression6 = (Expression)workPart.Expressions.FindObject("VAO_2_LE");
            Expression expression7 = (Expression)workPart.Expressions.FindObject("VAO_3_LE");
            Expression expression8 = (Expression)workPart.Expressions.FindObject("VAO_4_LE");

            Expression expression9 = (Expression)workPart.Expressions.FindObject("VAO_BAG_TRAS");
          
            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            workPart.Expressions.EditWithUnits(expression1, unit1, vao_1_ld); 
            workPart.Expressions.EditWithUnits(expression2, unit1, vao_2_ld);
            workPart.Expressions.EditWithUnits(expression3, unit1, vao_3_ld);
            workPart.Expressions.EditWithUnits(expression4, unit1, vao_4_ld);
            workPart.Expressions.EditWithUnits(expression5, unit1, vao_1_le);
            workPart.Expressions.EditWithUnits(expression6, unit1, vao_2_le);
            workPart.Expressions.EditWithUnits(expression7, unit1, vao_3_le);
            workPart.Expressions.EditWithUnits(expression8, unit1, vao_4_le);
            workPart.Expressions.EditWithUnits(expression9, unit1, vao_bag_tras);

            theSession.Preferences.Modeling.UpdatePending = false;

            Expression expression10 = (Expression)workPart.Expressions.FindObject("VAO_TAMPA_TRAS");
            vao_port_tras11 = expression10.Value;
            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Expression Data");

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId2);

            theSession.DeleteUndoMark(markId2, "Update Expression Data");

            theSession.Preferences.Modeling.UpdatePending = true;


            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Expression Data");

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId3);
            theSession.DeleteUndoMark(markId3, "Update Expression Data");
            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

            theSession.UpdateManager.DoInterpartUpdate(markId2);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId2);
            lbl_proccess.Text = "Casulo configurado";
        }

        public void saveas()
        {
            //Session theSession = Session.GetSession();
            //Part workPart = theSession.Parts.Work;
            //Part displayPart = theSession.Parts.Display;
            //NXOpen.Session.UndoMarkId markId1;
            //markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            //lbl_proccess.Text = "Codificando Teto...";
            //NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_TETO_M4/A 1");
            //PartLoadStatus partLoadStatus1;
            //theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

            //workPart = theSession.Parts.Work;
            //partLoadStatus1.Dispose();
            //theSession.SetUndoMarkName(markId1, "Make Work Part");

            //NXOpen.Session.UndoMarkId markId2;
            //markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            //NXOpen.PDM.PartOperationBuilder partOperationBuilder1;
            //partOperationBuilder1 = theSession.PdmSession.CreateOperationBuilder(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            //partOperationBuilder1.ReplaceAllComponents = true;

            //partOperationBuilder1.DependentFileSaveAsOption = NXOpen.PDM.PartOperationBuilder.DependentFileSaveAs.All;

            //partOperationBuilder1.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.Revise);

            //BasePart[] selectedparts1 = new BasePart[1];
            //selectedparts1[0] = workPart;
            //BasePart[] failedparts1;
            //partOperationBuilder1.SetSelectedParts(selectedparts1, out failedparts1);

            //NXOpen.PDM.LogicalObject[] logicalobjects1;
            //partOperationBuilder1.CreateLogicalObjects(out logicalobjects1);

            //NXObject[] sourceobjects1;
            //sourceobjects1 = logicalobjects1[0].GetUserAttributeSourceObjects();

            //NXObject[] objects1 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder1;
            //attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects2 = new NXObject[1];
            //objects2[0] = sourceobjects1[0];
            //attributePropertiesBuilder1.SetAttributeObjects(objects2);

            //attributePropertiesBuilder1.Title = "DB_PART_REV";

            //attributePropertiesBuilder1.Category = "ItemRevision";

            //attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder1.StringValue = "";

            //NXObject[] objects3 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder2;
            //attributePropertiesBuilder2 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects3, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects4 = new NXObject[1];
            //objects4[0] = sourceobjects1[0];
            //attributePropertiesBuilder2.SetAttributeObjects(objects4);

            //attributePropertiesBuilder2.Title = "DB_PART_NO";

            //attributePropertiesBuilder2.Category = "Item";

            //attributePropertiesBuilder2.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder2.StringValue = "TPL_TETO_M4";

            //NXObject[] objects5 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder3;
            //attributePropertiesBuilder3 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects5, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects6 = new NXObject[1];
            //objects6[0] = sourceobjects1[0];
            //attributePropertiesBuilder3.SetAttributeObjects(objects6);

            //attributePropertiesBuilder3.Title = "DB_PART_NAME";

            //attributePropertiesBuilder3.Category = "Item";

            //attributePropertiesBuilder3.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder3.StringValue = "TPL_TETO_M4";

            //theSession.SetUndoMarkName(markId2, "Save Parts As Dialog");

            //partOperationBuilder1.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            //NXObject[] sourceobjects2;
            //sourceobjects2 = logicalobjects1[0].GetUserAttributeSourceObjects();

            //NXObject[] objects7 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder4;
            //attributePropertiesBuilder4 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects7, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects8 = new NXObject[1];
            //objects8[0] = sourceobjects1[0];
            //attributePropertiesBuilder4.SetAttributeObjects(objects8);

            //attributePropertiesBuilder4.Title = "DB_PART_REV";

            //attributePropertiesBuilder4.Category = "ItemRevision";

            //attributePropertiesBuilder4.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder4.StringValue = "";

            //NXObject[] objects9 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder5;
            //attributePropertiesBuilder5 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects9, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects10 = new NXObject[1];
            //objects10[0] = sourceobjects1[0];
            //attributePropertiesBuilder5.SetAttributeObjects(objects10);

            //attributePropertiesBuilder5.Title = "DB_PART_NO";

            //attributePropertiesBuilder5.Category = "Item";

            //attributePropertiesBuilder5.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder5.StringValue = "TPL_TETO_M4";

            //NXObject[] objects11 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder6;
            //attributePropertiesBuilder6 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects11, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects12 = new NXObject[1];
            //objects12[0] = sourceobjects1[0];
            //attributePropertiesBuilder6.SetAttributeObjects(objects12);

            //attributePropertiesBuilder6.Title = "DB_PART_NAME";

            //attributePropertiesBuilder6.Category = "Item";

            //attributePropertiesBuilder6.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder6.StringValue = "TPL_TETO_M4";

            //BasePart[] selectedparts2 = new BasePart[1];
            //selectedparts2[0] = workPart;
            //BasePart[] failedparts2;
            //partOperationBuilder1.SetSelectedParts(selectedparts2, out failedparts2);

            //NXOpen.PDM.LogicalObject[] logicalobjects2;
            //partOperationBuilder1.CreateLogicalObjects(out logicalobjects2);

            //NXObject[] sourceobjects3;
            //sourceobjects3 = logicalobjects2[0].GetUserAttributeSourceObjects();

            //NXObject[] objects13 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder7;
            //attributePropertiesBuilder7 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects13, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects14 = new NXObject[1];
            //objects14[0] = sourceobjects3[0];
            //attributePropertiesBuilder7.SetAttributeObjects(objects14);

            //attributePropertiesBuilder7.Title = "DB_PART_REV";

            //attributePropertiesBuilder7.Category = "ItemRevision";

            //attributePropertiesBuilder7.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder7.StringValue = "";

            //NXObject[] objects15 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder8;
            //attributePropertiesBuilder8 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects15, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects16 = new NXObject[1];
            //objects16[0] = sourceobjects3[0];
            //attributePropertiesBuilder8.SetAttributeObjects(objects16);

            //attributePropertiesBuilder8.Title = "DB_PART_NO";

            //attributePropertiesBuilder8.Category = "Item";

            //attributePropertiesBuilder8.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder8.StringValue = "";

            //NXObject[] objects17 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder9;
            //attributePropertiesBuilder9 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects17, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects18 = new NXObject[1];
            //objects18[0] = sourceobjects3[0];
            //attributePropertiesBuilder9.SetAttributeObjects(objects18);

            //attributePropertiesBuilder9.Title = "DB_PART_NAME";

            //attributePropertiesBuilder9.Category = "Item";

            //attributePropertiesBuilder9.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder9.StringValue = "";

            //attributePropertiesBuilder2.Destroy();

            //attributePropertiesBuilder1.Destroy();

            //attributePropertiesBuilder3.Destroy();

            //attributePropertiesBuilder5.Destroy();

            //attributePropertiesBuilder4.Destroy();

            //attributePropertiesBuilder6.Destroy();


            //NXOpen.PDM.PartBuilder partbuilder = null;
            //partbuilder = theSession.Parts.PDMPartManager.NewPartFromPartBuilder();
            //string item_id = "";
            //item_id = partbuilder.AssignPartNumber("Item");
            //attributePropertiesBuilder8.StringValue = item_id;
            //attributePropertiesBuilder7.StringValue = "A";
            //attributePropertiesBuilder9.StringValue = item_id;

            //NXOpen.Session.UndoMarkId markId3;
            //markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Save Parts As");

            //theSession.DeleteUndoMark(markId3, null);

            //NXOpen.Session.UndoMarkId markId4;
            //markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Save Parts As");

            //partOperationBuilder1.ValidateLogicalObjectsToCommit();

            //ErrorList errorList2;
            //errorList2 = partOperationBuilder1.GetOperationFailures();

            //NXObject nXObject1;
            //nXObject1 = partOperationBuilder1.Commit();

            //ErrorList errorList3;
            //errorList3 = partOperationBuilder1.GetOperationFailures();

            //theSession.DeleteUndoMark(markId4, null);

            //partOperationBuilder1.Destroy();

            //attributePropertiesBuilder8.Destroy();

            //attributePropertiesBuilder7.Destroy();

            //attributePropertiesBuilder9.Destroy();

            //theSession.DeleteUndoMark(markId2, null);
            /////---INICIO LATERAL DIR
            //// Journaling of this operation is not yet implemented
            //// The part save as operation in NX Manager mode
            //NXOpen.Session.UndoMarkId markId5;
            //markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");
            //lbl_proccess.Text = "Codificando Lateral Direita...";
            //NXOpen.Assemblies.Component component2 = (NXOpen.Assemblies.Component)displayPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_LAT_DIR_M4/A 1");
            //PartLoadStatus partLoadStatus2;
            //theSession.Parts.SetWorkComponent(component2, out partLoadStatus2);

            //workPart = theSession.Parts.Work;
            //partLoadStatus2.Dispose();
            //theSession.SetUndoMarkName(markId5, "Make Work Part");

            //NXOpen.Session.UndoMarkId markId6;
            //markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            //NXOpen.PDM.PartOperationBuilder partOperationBuilder2;
            //partOperationBuilder2 = theSession.PdmSession.CreateOperationBuilder(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            //partOperationBuilder2.ReplaceAllComponents = true;

            //partOperationBuilder2.DependentFileSaveAsOption = NXOpen.PDM.PartOperationBuilder.DependentFileSaveAs.All;

            //partOperationBuilder2.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.Revise);

            //BasePart[] selectedparts3 = new BasePart[1];
            //selectedparts3[0] = workPart;
            //BasePart[] failedparts3;
            //partOperationBuilder2.SetSelectedParts(selectedparts3, out failedparts3);

            //NXOpen.PDM.LogicalObject[] logicalobjects3;
            //partOperationBuilder2.CreateLogicalObjects(out logicalobjects3);

            //NXObject[] sourceobjects4;
            //sourceobjects4 = logicalobjects3[0].GetUserAttributeSourceObjects();

            //NXObject[] objects20 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder10;
            //attributePropertiesBuilder10 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects20, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects21 = new NXObject[1];
            //objects21[0] = sourceobjects4[0];
            //attributePropertiesBuilder10.SetAttributeObjects(objects21);

            //attributePropertiesBuilder10.Title = "DB_PART_REV";

            //attributePropertiesBuilder10.Category = "ItemRevision";

            //attributePropertiesBuilder10.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder10.StringValue = "";

            //NXObject[] objects22 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder11;
            //attributePropertiesBuilder11 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects22, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects23 = new NXObject[1];
            //objects23[0] = sourceobjects4[0];
            //attributePropertiesBuilder11.SetAttributeObjects(objects23);

            //attributePropertiesBuilder11.Title = "DB_PART_NO";

            //attributePropertiesBuilder11.Category = "Item";

            //attributePropertiesBuilder11.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder11.StringValue = "TPL_LAT_DIR_M4";

            //NXObject[] objects24 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder12;
            //attributePropertiesBuilder12 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects24, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects25 = new NXObject[1];
            //objects25[0] = sourceobjects4[0];
            //attributePropertiesBuilder12.SetAttributeObjects(objects25);

            //attributePropertiesBuilder12.Title = "DB_PART_NAME";

            //attributePropertiesBuilder12.Category = "Item";

            //attributePropertiesBuilder12.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder12.StringValue = "TPL_LAT_DIR_M4";

            //theSession.SetUndoMarkName(markId6, "Save Parts As Dialog");

            //partOperationBuilder2.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            //NXObject[] sourceobjects5;
            //sourceobjects5 = logicalobjects3[0].GetUserAttributeSourceObjects();

            //NXObject[] objects26 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder13;
            //attributePropertiesBuilder13 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects26, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects27 = new NXObject[1];
            //objects27[0] = sourceobjects4[0];
            //attributePropertiesBuilder13.SetAttributeObjects(objects27);

            //attributePropertiesBuilder13.Title = "DB_PART_REV";

            //attributePropertiesBuilder13.Category = "ItemRevision";

            //attributePropertiesBuilder13.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder13.StringValue = "";

            //NXObject[] objects28 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder14;
            //attributePropertiesBuilder14 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects28, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects29 = new NXObject[1];
            //objects29[0] = sourceobjects4[0];
            //attributePropertiesBuilder14.SetAttributeObjects(objects29);

            //attributePropertiesBuilder14.Title = "DB_PART_NO";

            //attributePropertiesBuilder14.Category = "Item";

            //attributePropertiesBuilder14.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder14.StringValue = "TPL_LAT_DIR_M4";

            //NXObject[] objects30 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder15;
            //attributePropertiesBuilder15 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects30, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects31 = new NXObject[1];
            //objects31[0] = sourceobjects4[0];
            //attributePropertiesBuilder15.SetAttributeObjects(objects31);

            //attributePropertiesBuilder15.Title = "DB_PART_NAME";

            //attributePropertiesBuilder15.Category = "Item";

            //attributePropertiesBuilder15.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder15.StringValue = "TPL_LAT_DIR_M4";

            //BasePart[] selectedparts4 = new BasePart[1];
            //selectedparts4[0] = workPart;
            //BasePart[] failedparts4;
            //partOperationBuilder2.SetSelectedParts(selectedparts4, out failedparts4);

            //NXOpen.PDM.LogicalObject[] logicalobjects4;
            //partOperationBuilder2.CreateLogicalObjects(out logicalobjects4);

            //NXObject[] sourceobjects6;
            //sourceobjects6 = logicalobjects4[0].GetUserAttributeSourceObjects();

            //NXObject[] objects32 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder16;
            //attributePropertiesBuilder16 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects32, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects33 = new NXObject[1];
            //objects33[0] = sourceobjects6[0];
            //attributePropertiesBuilder16.SetAttributeObjects(objects33);

            //attributePropertiesBuilder16.Title = "DB_PART_REV";

            //attributePropertiesBuilder16.Category = "ItemRevision";

            //attributePropertiesBuilder16.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder16.StringValue = "";

            //NXObject[] objects34 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder17;
            //attributePropertiesBuilder17 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects34, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects35 = new NXObject[1];
            //objects35[0] = sourceobjects6[0];
            //attributePropertiesBuilder17.SetAttributeObjects(objects35);

            //attributePropertiesBuilder17.Title = "DB_PART_NO";

            //attributePropertiesBuilder17.Category = "Item";

            //attributePropertiesBuilder17.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder17.StringValue = "";

            //NXObject[] objects36 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder18;
            //attributePropertiesBuilder18 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects36, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects37 = new NXObject[1];
            //objects37[0] = sourceobjects6[0];
            //attributePropertiesBuilder18.SetAttributeObjects(objects37);

            //attributePropertiesBuilder18.Title = "DB_PART_NAME";

            //attributePropertiesBuilder18.Category = "Item";

            //attributePropertiesBuilder18.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder18.StringValue = "";

            //attributePropertiesBuilder11.Destroy();

            //attributePropertiesBuilder10.Destroy();

            //attributePropertiesBuilder12.Destroy();

            //attributePropertiesBuilder14.Destroy();

            //attributePropertiesBuilder13.Destroy();

            //attributePropertiesBuilder15.Destroy();


            //partbuilder = theSession.Parts.PDMPartManager.NewPartFromPartBuilder();
            //item_id = "";
            //item_id = partbuilder.AssignPartNumber("Item");
            //attributePropertiesBuilder17.StringValue = item_id;
            //attributePropertiesBuilder16.StringValue = "A";
            //attributePropertiesBuilder18.StringValue = item_id;

            //NXOpen.Session.UndoMarkId markId7;
            //markId7 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Save Parts As");

            //theSession.DeleteUndoMark(markId7, null);

            //NXOpen.Session.UndoMarkId markId8;
            //markId8 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Save Parts As");

            //partOperationBuilder2.ValidateLogicalObjectsToCommit();

            //ErrorList errorList5;
            //errorList5 = partOperationBuilder2.GetOperationFailures();

            //NXObject nXObject2;
            //nXObject2 = partOperationBuilder2.Commit();

            //ErrorList errorList6;
            //errorList6 = partOperationBuilder2.GetOperationFailures();

            //theSession.DeleteUndoMark(markId8, null);

            //partOperationBuilder2.Destroy();

            //attributePropertiesBuilder17.Destroy();

            //attributePropertiesBuilder16.Destroy();

            //attributePropertiesBuilder18.Destroy();

            //theSession.DeleteUndoMark(markId6, null);

            ///// ------ INICIO BASE--
            //// Journaling of this operation is not yet implemented
            //// The part save as operation in NX Manager mode
            //NXOpen.Session.UndoMarkId markId9;
            //markId9 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");
            //lbl_proccess.Text = "Codificando Lateral Esquerda...";
            //NXOpen.Assemblies.Component component3 = (NXOpen.Assemblies.Component)displayPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_LAT_ESQ_M4/A 1");
            //PartLoadStatus partLoadStatus3;
            //theSession.Parts.SetWorkComponent(component3, out partLoadStatus3);

            //workPart = theSession.Parts.Work;
            //partLoadStatus3.Dispose();
            //theSession.SetUndoMarkName(markId9, "Make Work Part");

            //// ----------------------------------------------
            ////   Menu: File->Save As...
            //// ----------------------------------------------
            //NXOpen.Session.UndoMarkId markId10;
            //markId10 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            //NXOpen.PDM.PartOperationBuilder partOperationBuilder3;
            //partOperationBuilder3 = theSession.PdmSession.CreateOperationBuilder(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            //partOperationBuilder3.ReplaceAllComponents = true;

            //partOperationBuilder3.DependentFileSaveAsOption = NXOpen.PDM.PartOperationBuilder.DependentFileSaveAs.All;

            //partOperationBuilder3.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.Revise);

            //BasePart[] selectedparts5 = new BasePart[1];
            //selectedparts5[0] = workPart;
            //BasePart[] failedparts5;
            //partOperationBuilder3.SetSelectedParts(selectedparts5, out failedparts5);

            //NXOpen.PDM.LogicalObject[] logicalobjects5;
            //partOperationBuilder3.CreateLogicalObjects(out logicalobjects5);

            //NXObject[] sourceobjects7;
            //sourceobjects7 = logicalobjects5[0].GetUserAttributeSourceObjects();

            //NXObject[] objects39 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder19;
            //attributePropertiesBuilder19 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects39, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects40 = new NXObject[1];
            //objects40[0] = sourceobjects7[0];
            //attributePropertiesBuilder19.SetAttributeObjects(objects40);

            //attributePropertiesBuilder19.Title = "DB_PART_REV";

            //attributePropertiesBuilder19.Category = "ItemRevision";

            //attributePropertiesBuilder19.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder19.StringValue = "";

            //NXObject[] objects41 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder20;
            //attributePropertiesBuilder20 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects41, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects42 = new NXObject[1];
            //objects42[0] = sourceobjects7[0];
            //attributePropertiesBuilder20.SetAttributeObjects(objects42);

            //attributePropertiesBuilder20.Title = "DB_PART_NO";

            //attributePropertiesBuilder20.Category = "Item";

            //attributePropertiesBuilder20.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder20.StringValue = "TPL_LAT_ESQ_M4";

            //NXObject[] objects43 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder21;
            //attributePropertiesBuilder21 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects43, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects44 = new NXObject[1];
            //objects44[0] = sourceobjects7[0];
            //attributePropertiesBuilder21.SetAttributeObjects(objects44);

            //attributePropertiesBuilder21.Title = "DB_PART_NAME";

            //attributePropertiesBuilder21.Category = "Item";

            //attributePropertiesBuilder21.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder21.StringValue = "TPL_LAT_ESQ_M4";

            //theSession.SetUndoMarkName(markId10, "Save Parts As Dialog");

            //partOperationBuilder3.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            //NXObject[] sourceobjects8;
            //sourceobjects8 = logicalobjects5[0].GetUserAttributeSourceObjects();

            //NXObject[] objects45 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder22;
            //attributePropertiesBuilder22 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects45, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects46 = new NXObject[1];
            //objects46[0] = sourceobjects7[0];
            //attributePropertiesBuilder22.SetAttributeObjects(objects46);

            //attributePropertiesBuilder22.Title = "DB_PART_REV";

            //attributePropertiesBuilder22.Category = "ItemRevision";

            //attributePropertiesBuilder22.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder22.StringValue = "";

            //NXObject[] objects47 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder23;
            //attributePropertiesBuilder23 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects47, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects48 = new NXObject[1];
            //objects48[0] = sourceobjects7[0];
            //attributePropertiesBuilder23.SetAttributeObjects(objects48);

            //attributePropertiesBuilder23.Title = "DB_PART_NO";

            //attributePropertiesBuilder23.Category = "Item";

            //attributePropertiesBuilder23.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder23.StringValue = "TPL_LAT_ESQ_M4";

            //NXObject[] objects49 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder24;
            //attributePropertiesBuilder24 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects49, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects50 = new NXObject[1];
            //objects50[0] = sourceobjects7[0];
            //attributePropertiesBuilder24.SetAttributeObjects(objects50);

            //attributePropertiesBuilder24.Title = "DB_PART_NAME";

            //attributePropertiesBuilder24.Category = "Item";

            //attributePropertiesBuilder24.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder24.StringValue = "TPL_LAT_ESQ_M4";

            //BasePart[] selectedparts6 = new BasePart[1];
            //selectedparts6[0] = workPart;
            //BasePart[] failedparts6;
            //partOperationBuilder3.SetSelectedParts(selectedparts6, out failedparts6);

            //NXOpen.PDM.LogicalObject[] logicalobjects6;
            //partOperationBuilder3.CreateLogicalObjects(out logicalobjects6);

            //NXObject[] sourceobjects9;
            //sourceobjects9 = logicalobjects6[0].GetUserAttributeSourceObjects();

            //NXObject[] objects51 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder25;
            //attributePropertiesBuilder25 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects51, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects52 = new NXObject[1];
            //objects52[0] = sourceobjects9[0];
            //attributePropertiesBuilder25.SetAttributeObjects(objects52);

            //attributePropertiesBuilder25.Title = "DB_PART_REV";

            //attributePropertiesBuilder25.Category = "ItemRevision";

            //attributePropertiesBuilder25.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder25.StringValue = "";

            //NXObject[] objects53 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder26;
            //attributePropertiesBuilder26 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects53, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects54 = new NXObject[1];
            //objects54[0] = sourceobjects9[0];
            //attributePropertiesBuilder26.SetAttributeObjects(objects54);

            //attributePropertiesBuilder26.Title = "DB_PART_NO";

            //attributePropertiesBuilder26.Category = "Item";

            //attributePropertiesBuilder26.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder26.StringValue = "";

            //NXObject[] objects55 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder27;
            //attributePropertiesBuilder27 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects55, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects56 = new NXObject[1];
            //objects56[0] = sourceobjects9[0];
            //attributePropertiesBuilder27.SetAttributeObjects(objects56);

            //attributePropertiesBuilder27.Title = "DB_PART_NAME";

            //attributePropertiesBuilder27.Category = "Item";

            //attributePropertiesBuilder27.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder27.StringValue = "";

            //attributePropertiesBuilder20.Destroy();

            //attributePropertiesBuilder19.Destroy();

            //attributePropertiesBuilder21.Destroy();

            //attributePropertiesBuilder23.Destroy();

            //attributePropertiesBuilder22.Destroy();

            //attributePropertiesBuilder24.Destroy();

            //// ----------------------------------------------
            ////   Dialog Begin Save Parts As
            //// ----------------------------------------------

            //partbuilder = theSession.Parts.PDMPartManager.NewPartFromPartBuilder();
            //item_id = "";
            //item_id = partbuilder.AssignPartNumber("Item");
            //attributePropertiesBuilder26.StringValue = item_id;
            //attributePropertiesBuilder25.StringValue = "A";
            //attributePropertiesBuilder27.StringValue = item_id;

            //NXOpen.Session.UndoMarkId markId11;
            //markId11 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Save Parts As");

            //theSession.DeleteUndoMark(markId11, null);

            //NXOpen.Session.UndoMarkId markId12;
            //markId12 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Save Parts As");

            //partOperationBuilder3.ValidateLogicalObjectsToCommit();

            //ErrorList errorList8;
            //errorList8 = partOperationBuilder3.GetOperationFailures();

            //NXObject nXObject3;
            //nXObject3 = partOperationBuilder3.Commit();

            //ErrorList errorList9;
            //errorList9 = partOperationBuilder3.GetOperationFailures();

            //theSession.DeleteUndoMark(markId12, null);

            //partOperationBuilder3.Destroy();

            //attributePropertiesBuilder26.Destroy();

            //attributePropertiesBuilder25.Destroy();

            //attributePropertiesBuilder27.Destroy();

            //theSession.DeleteUndoMark(markId10, null);

            /////-----INICIO DO BASE----
            //// Journaling of this operation is not yet implemented
            //// The part save as operation in NX Manager mode
            //NXOpen.Session.UndoMarkId markId13;
            //markId13 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");
            //lbl_proccess.Text = "Codificando Base...";
            //NXOpen.Assemblies.Component component4 = (NXOpen.Assemblies.Component)displayPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_BASE_M4/A 1");
            //PartLoadStatus partLoadStatus4;
            //theSession.Parts.SetWorkComponent(component4, out partLoadStatus4);

            //workPart = theSession.Parts.Work;
            //partLoadStatus4.Dispose();
            //theSession.SetUndoMarkName(markId13, "Make Work Part");

            //// ----------------------------------------------
            ////   Menu: File->Save As...
            //// ----------------------------------------------
            //NXOpen.Session.UndoMarkId markId14;
            //markId14 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            //NXOpen.PDM.PartOperationBuilder partOperationBuilder4;
            //partOperationBuilder4 = theSession.PdmSession.CreateOperationBuilder(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            //partOperationBuilder4.ReplaceAllComponents = true;

            //partOperationBuilder4.DependentFileSaveAsOption = NXOpen.PDM.PartOperationBuilder.DependentFileSaveAs.All;

            //partOperationBuilder4.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.Revise);

            //BasePart[] selectedparts7 = new BasePart[1];
            //selectedparts7[0] = workPart;
            //BasePart[] failedparts7;
            //partOperationBuilder4.SetSelectedParts(selectedparts7, out failedparts7);

            //NXOpen.PDM.LogicalObject[] logicalobjects7;
            //partOperationBuilder4.CreateLogicalObjects(out logicalobjects7);

            //NXObject[] sourceobjects10;
            //sourceobjects10 = logicalobjects7[0].GetUserAttributeSourceObjects();

            //NXObject[] objects58 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder28;
            //attributePropertiesBuilder28 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects58, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects59 = new NXObject[1];
            //objects59[0] = sourceobjects10[0];
            //attributePropertiesBuilder28.SetAttributeObjects(objects59);

            //attributePropertiesBuilder28.Title = "DB_PART_REV";

            //attributePropertiesBuilder28.Category = "ItemRevision";

            //attributePropertiesBuilder28.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder28.StringValue = "";

            //NXObject[] objects60 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder29;
            //attributePropertiesBuilder29 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects60, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects61 = new NXObject[1];
            //objects61[0] = sourceobjects10[0];
            //attributePropertiesBuilder29.SetAttributeObjects(objects61);

            //attributePropertiesBuilder29.Title = "DB_PART_NO";

            //attributePropertiesBuilder29.Category = "Item";

            //attributePropertiesBuilder29.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder29.StringValue = "TPL_BASE_M4";

            //NXObject[] objects62 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder30;
            //attributePropertiesBuilder30 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects62, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects63 = new NXObject[1];
            //objects63[0] = sourceobjects10[0];
            //attributePropertiesBuilder30.SetAttributeObjects(objects63);

            //attributePropertiesBuilder30.Title = "DB_PART_NAME";

            //attributePropertiesBuilder30.Category = "Item";

            //attributePropertiesBuilder30.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder30.StringValue = "TPL_BASE_M4";

            //theSession.SetUndoMarkName(markId14, "Save Parts As Dialog");

            //partOperationBuilder4.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            //NXObject[] sourceobjects11;
            //sourceobjects11 = logicalobjects7[0].GetUserAttributeSourceObjects();

            //NXObject[] objects64 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder31;
            //attributePropertiesBuilder31 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects64, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects65 = new NXObject[1];
            //objects65[0] = sourceobjects10[0];
            //attributePropertiesBuilder31.SetAttributeObjects(objects65);

            //attributePropertiesBuilder31.Title = "DB_PART_REV";

            //attributePropertiesBuilder31.Category = "ItemRevision";

            //attributePropertiesBuilder31.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder31.StringValue = "";

            //NXObject[] objects66 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder32;
            //attributePropertiesBuilder32 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects66, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects67 = new NXObject[1];
            //objects67[0] = sourceobjects10[0];
            //attributePropertiesBuilder32.SetAttributeObjects(objects67);

            //attributePropertiesBuilder32.Title = "DB_PART_NO";

            //attributePropertiesBuilder32.Category = "Item";

            //attributePropertiesBuilder32.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder32.StringValue = "TPL_BASE_M4";

            //NXObject[] objects68 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder33;
            //attributePropertiesBuilder33 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects68, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects69 = new NXObject[1];
            //objects69[0] = sourceobjects10[0];
            //attributePropertiesBuilder33.SetAttributeObjects(objects69);

            //attributePropertiesBuilder33.Title = "DB_PART_NAME";

            //attributePropertiesBuilder33.Category = "Item";

            //attributePropertiesBuilder33.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder33.StringValue = "TPL_BASE_M4";

            //BasePart[] selectedparts8 = new BasePart[1];
            //selectedparts8[0] = workPart;
            //BasePart[] failedparts8;
            //partOperationBuilder4.SetSelectedParts(selectedparts8, out failedparts8);

            //NXOpen.PDM.LogicalObject[] logicalobjects8;
            //partOperationBuilder4.CreateLogicalObjects(out logicalobjects8);

            //NXObject[] sourceobjects12;
            //sourceobjects12 = logicalobjects8[0].GetUserAttributeSourceObjects();

            //NXObject[] objects70 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder34;
            //attributePropertiesBuilder34 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects70, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects71 = new NXObject[1];
            //objects71[0] = sourceobjects12[0];
            //attributePropertiesBuilder34.SetAttributeObjects(objects71);

            //attributePropertiesBuilder34.Title = "DB_PART_REV";

            //attributePropertiesBuilder34.Category = "ItemRevision";

            //attributePropertiesBuilder34.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder34.StringValue = "";

            //NXObject[] objects72 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder35;
            //attributePropertiesBuilder35 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects72, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects73 = new NXObject[1];
            //objects73[0] = sourceobjects12[0];
            //attributePropertiesBuilder35.SetAttributeObjects(objects73);

            //attributePropertiesBuilder35.Title = "DB_PART_NO";

            //attributePropertiesBuilder35.Category = "Item";

            //attributePropertiesBuilder35.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder35.StringValue = "";

            //NXObject[] objects74 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder36;
            //attributePropertiesBuilder36 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects74, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects75 = new NXObject[1];
            //objects75[0] = sourceobjects12[0];
            //attributePropertiesBuilder36.SetAttributeObjects(objects75);

            //attributePropertiesBuilder36.Title = "DB_PART_NAME";

            //attributePropertiesBuilder36.Category = "Item";

            //attributePropertiesBuilder36.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder36.StringValue = "";

            //attributePropertiesBuilder29.Destroy();

            //attributePropertiesBuilder28.Destroy();

            //attributePropertiesBuilder30.Destroy();

            //attributePropertiesBuilder32.Destroy();

            //attributePropertiesBuilder31.Destroy();

            //attributePropertiesBuilder33.Destroy();

            //// ----------------------------------------------
            ////   Dialog Begin Save Parts As
            //// ----------------------------------------------


            //partbuilder = theSession.Parts.PDMPartManager.NewPartFromPartBuilder();
            //item_id = "";
            //item_id = partbuilder.AssignPartNumber("Item");
            //attributePropertiesBuilder35.StringValue = item_id;
            //attributePropertiesBuilder34.StringValue = "A";
            //attributePropertiesBuilder36.StringValue = item_id;

            //NXOpen.Session.UndoMarkId markId15;
            //markId15 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Save Parts As");

            //theSession.DeleteUndoMark(markId15, null);

            //NXOpen.Session.UndoMarkId markId16;
            //markId16 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Save Parts As");

            //partOperationBuilder4.ValidateLogicalObjectsToCommit();

            //ErrorList errorList11;
            //errorList11 = partOperationBuilder4.GetOperationFailures();

            //NXObject nXObject4;
            //nXObject4 = partOperationBuilder4.Commit();

            //ErrorList errorList12;
            //errorList12 = partOperationBuilder4.GetOperationFailures();

            //theSession.DeleteUndoMark(markId16, null);

            //partOperationBuilder4.Destroy();

            //attributePropertiesBuilder35.Destroy();

            //attributePropertiesBuilder34.Destroy();

            //attributePropertiesBuilder36.Destroy();

            //theSession.DeleteUndoMark(markId14, null);

            /////----INICIO CASULO---
            //// Journaling of this operation is not yet implemented
            //// The part save as operation in NX Manager mode
            //NXOpen.Session.UndoMarkId markId17;
            //markId17 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");
            //lbl_proccess.Text = "Codificando Casulo...";
            //NXOpen.Assemblies.Component nullAssemblies_Component = null;
            //PartLoadStatus partLoadStatus5;
            //theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus5);

            //workPart = theSession.Parts.Work;
            //partLoadStatus5.Dispose();
            //theSession.SetUndoMarkName(markId17, "Make Work Part");

            //// ----------------------------------------------
            ////   Menu: File->Save As...
            //// ----------------------------------------------
            //NXOpen.Session.UndoMarkId markId18;
            //markId18 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            //NXOpen.PDM.PartOperationBuilder partOperationBuilder5;
            //partOperationBuilder5 = theSession.PdmSession.CreateOperationBuilder(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            //partOperationBuilder5.ReplaceAllComponents = true;

            //partOperationBuilder5.DependentFileSaveAsOption = NXOpen.PDM.PartOperationBuilder.DependentFileSaveAs.All;

            //partOperationBuilder5.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.Revise);

            //BasePart[] selectedparts9 = new BasePart[1];
            //selectedparts9[0] = workPart;
            //BasePart[] failedparts9;
            //partOperationBuilder5.SetSelectedParts(selectedparts9, out failedparts9);

            //NXOpen.PDM.LogicalObject[] logicalobjects9;
            //partOperationBuilder5.CreateLogicalObjects(out logicalobjects9);

            //NXObject[] sourceobjects13;
            //sourceobjects13 = logicalobjects9[0].GetUserAttributeSourceObjects();

            //NXObject[] objects77 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder37;
            //attributePropertiesBuilder37 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects77, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects78 = new NXObject[1];
            //objects78[0] = sourceobjects13[0];
            //attributePropertiesBuilder37.SetAttributeObjects(objects78);

            //attributePropertiesBuilder37.Title = "DB_PART_REV";

            //attributePropertiesBuilder37.Category = "ItemRevision";

            //attributePropertiesBuilder37.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder37.StringValue = "";

            //NXObject[] objects79 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder38;
            //attributePropertiesBuilder38 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects79, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects80 = new NXObject[1];
            //objects80[0] = sourceobjects13[0];
            //attributePropertiesBuilder38.SetAttributeObjects(objects80);

            //attributePropertiesBuilder38.Title = "DB_PART_NO";

            //attributePropertiesBuilder38.Category = "Item";

            //attributePropertiesBuilder38.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder38.StringValue = "TPL_CASULO";

            //NXObject[] objects81 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder39;
            //attributePropertiesBuilder39 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects81, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            //NXObject[] objects82 = new NXObject[1];
            //objects82[0] = sourceobjects13[0];
            //attributePropertiesBuilder39.SetAttributeObjects(objects82);

            //attributePropertiesBuilder39.Title = "DB_PART_NAME";

            //attributePropertiesBuilder39.Category = "Item";

            //attributePropertiesBuilder39.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder39.StringValue = "TPL_CASULO";

            //theSession.SetUndoMarkName(markId18, "Save Parts As Dialog");

            //partOperationBuilder5.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            //NXObject[] sourceobjects14;
            //sourceobjects14 = logicalobjects9[0].GetUserAttributeSourceObjects();

            //NXObject[] objects83 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder40;
            //attributePropertiesBuilder40 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects83, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects84 = new NXObject[1];
            //objects84[0] = sourceobjects13[0];
            //attributePropertiesBuilder40.SetAttributeObjects(objects84);

            //attributePropertiesBuilder40.Title = "DB_PART_REV";

            //attributePropertiesBuilder40.Category = "ItemRevision";

            //attributePropertiesBuilder40.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder40.StringValue = "";

            //NXObject[] objects85 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder41;
            //attributePropertiesBuilder41 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects85, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects86 = new NXObject[1];
            //objects86[0] = sourceobjects13[0];
            //attributePropertiesBuilder41.SetAttributeObjects(objects86);

            //attributePropertiesBuilder41.Title = "DB_PART_NO";

            //attributePropertiesBuilder41.Category = "Item";

            //attributePropertiesBuilder41.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder41.StringValue = "TPL_CASULO";

            //NXObject[] objects87 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder42;
            //attributePropertiesBuilder42 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects87, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects88 = new NXObject[1];
            //objects88[0] = sourceobjects13[0];
            //attributePropertiesBuilder42.SetAttributeObjects(objects88);

            //attributePropertiesBuilder42.Title = "DB_PART_NAME";

            //attributePropertiesBuilder42.Category = "Item";

            //attributePropertiesBuilder42.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder42.StringValue = "TPL_CASULO";

            //BasePart[] selectedparts10 = new BasePart[1];
            //selectedparts10[0] = workPart;
            //BasePart[] failedparts10;
            //partOperationBuilder5.SetSelectedParts(selectedparts10, out failedparts10);

            //NXOpen.PDM.LogicalObject[] logicalobjects10;
            //partOperationBuilder5.CreateLogicalObjects(out logicalobjects10);

            //NXObject[] sourceobjects15;
            //sourceobjects15 = logicalobjects10[0].GetUserAttributeSourceObjects();

            //NXObject[] objects89 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder43;
            //attributePropertiesBuilder43 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects89, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects90 = new NXObject[1];
            //objects90[0] = sourceobjects15[0];
            //attributePropertiesBuilder43.SetAttributeObjects(objects90);

            //attributePropertiesBuilder43.Title = "DB_PART_REV";

            //attributePropertiesBuilder43.Category = "ItemRevision";

            //attributePropertiesBuilder43.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder43.StringValue = "";

            //NXObject[] objects91 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder44;
            //attributePropertiesBuilder44 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects91, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects92 = new NXObject[1];
            //objects92[0] = sourceobjects15[0];
            //attributePropertiesBuilder44.SetAttributeObjects(objects92);

            //attributePropertiesBuilder44.Title = "DB_PART_NO";

            //attributePropertiesBuilder44.Category = "Item";

            //attributePropertiesBuilder44.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder44.StringValue = "";

            //NXObject[] objects93 = new NXObject[0];
            //AttributePropertiesBuilder attributePropertiesBuilder45;
            //attributePropertiesBuilder45 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects93, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            //NXObject[] objects94 = new NXObject[1];
            //objects94[0] = sourceobjects15[0];
            //attributePropertiesBuilder45.SetAttributeObjects(objects94);

            //attributePropertiesBuilder45.Title = "DB_PART_NAME";

            //attributePropertiesBuilder45.Category = "Item";

            //attributePropertiesBuilder45.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            //attributePropertiesBuilder45.StringValue = "";

            //attributePropertiesBuilder38.Destroy();

            //attributePropertiesBuilder37.Destroy();

            //attributePropertiesBuilder39.Destroy();

            //attributePropertiesBuilder41.Destroy();

            //attributePropertiesBuilder40.Destroy();

            //attributePropertiesBuilder42.Destroy();

            //// ----------------------------------------------
            ////   Dialog Begin Save Parts As
            //// ----------------------------------------------

            //partbuilder = theSession.Parts.PDMPartManager.NewPartFromPartBuilder();
            //item_id = "";
            //item_id = partbuilder.AssignPartNumber("Item");
            //attributePropertiesBuilder44.StringValue = item_id;
            //attributePropertiesBuilder43.StringValue = "A";
            //attributePropertiesBuilder45.StringValue = "CASULO_M4_"+txt_comp_carro.Text+ "_CHASSI_"+ cmb_chassi.Text;

            //NXOpen.Session.UndoMarkId markId19;
            //markId19 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Save Parts As");

            //theSession.DeleteUndoMark(markId19, null);

            //NXOpen.Session.UndoMarkId markId20;
            //markId20 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Save Parts As");

            //partOperationBuilder5.ValidateLogicalObjectsToCommit();

            //ErrorList errorList14;
            //errorList14 = partOperationBuilder5.GetOperationFailures();

            //NXObject nXObject5;
            //nXObject5 = partOperationBuilder5.Commit();

            //ErrorList errorList15;
            //errorList15 = partOperationBuilder5.GetOperationFailures();

            //theSession.DeleteUndoMark(markId20, null);

            //partOperationBuilder5.Destroy();

            //attributePropertiesBuilder44.Destroy();

            //attributePropertiesBuilder43.Destroy();

            //attributePropertiesBuilder45.Destroy();

            //theSession.DeleteUndoMark(markId18, null);

            //// Journaling of this operation is not yet implemented
            //// The part save as operation in NX Manager mode
            //// ----------------------------------------------
            ////   Menu: Tools->Journal->Stop Recording
            // ----------------------------------------------
        }

        public void codificar_pecas_lat_dir()
        {
            lbl_proccess.Text = "Codificando itens da Lateral Direita";
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_LAT_DIR_M4/A 1");
            PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

            workPart = theSession.Parts.Work;
            partLoadStatus1.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.Assemblies.ReplaceComponentBuilder replaceComponentBuilder1;
            replaceComponentBuilder1 = workPart.AssemblyManager.CreateReplaceComponentBuilder();

            replaceComponentBuilder1.ComponentNameType = NXOpen.Assemblies.ReplaceComponentBuilder.ComponentNameOption.AsSpecified;

            replaceComponentBuilder1.ComponentName = "000317";

            theSession.SetUndoMarkName(markId2, "Replace Component Dialog");

            replaceComponentBuilder1.ComponentName = "";

            NXOpen.Assemblies.Component component2 = (NXOpen.Assemblies.Component)component1.FindObject("COMPONENT TPL_TEMINAL_SAIA/A 1");
            bool added1;
            added1 = replaceComponentBuilder1.ComponentsToReplace.Add(component2);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Replace Component");

            theSession.DeleteUndoMark(markId3, null);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Replace Component");

            Expression COMP_TERMINAL_DA_SAIA = (Expression)workPart.Expressions.FindObject("COMP_TERMINAL_DA_SAIA");

            string codigo = "";

                string FAM = "000333";

                string[] expressao = { "COMP_SAIA" };

                double[] valor = { COMP_TERMINAL_DA_SAIA.Value };
                double[] tolerancia = { 1, 1 };
                string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");
                string[] quebra = arquivo.Split('/');
                codigo = quebra[0];

                replaceComponentBuilder1.ReplacementPart = "@DB/" + codigo + "/A"; ;

            replaceComponentBuilder1.SetComponentReferenceSetType(NXOpen.Assemblies.ReplaceComponentBuilder.ComponentReferenceSet.Maintain, null);

            PartLoadStatus partLoadStatus2;
            partLoadStatus2 = replaceComponentBuilder1.RegisterReplacePartLoadStatus();

            NXObject nXObject1;
            nXObject1 = replaceComponentBuilder1.Commit();

            partLoadStatus2.Dispose();
            theSession.DeleteUndoMark(markId4, null);


            replaceComponentBuilder1.Destroy();

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component nullAssemblies_Component = null;
            PartLoadStatus partLoadStatus3;
            theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus3);

            workPart = theSession.Parts.Work;
            partLoadStatus3.Dispose();
            theSession.SetUndoMarkName(markId5, "Make Work Part");

        }
        public void codificar_pecas_lat_esq()
        {
            lbl_proccess.Text = "Codificando itens da Lateral Esquerda";
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_LAT_ESQ_M4/A 1");
            PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

            workPart = theSession.Parts.Work;
            partLoadStatus1.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.Assemblies.ReplaceComponentBuilder replaceComponentBuilder1;
            replaceComponentBuilder1 = workPart.AssemblyManager.CreateReplaceComponentBuilder();

            replaceComponentBuilder1.ComponentNameType = NXOpen.Assemblies.ReplaceComponentBuilder.ComponentNameOption.AsSpecified;

            replaceComponentBuilder1.ComponentName = "000317";

            theSession.SetUndoMarkName(markId2, "Replace Component Dialog");

            replaceComponentBuilder1.ComponentName = "";

            NXOpen.Assemblies.Component component2 = (NXOpen.Assemblies.Component)component1.FindObject("COMPONENT TPL_TEMINAL_SAIA_LE/A 1");
            bool added1;
            added1 = replaceComponentBuilder1.ComponentsToReplace.Add(component2);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Replace Component");

            theSession.DeleteUndoMark(markId3, null);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Replace Component");

            Expression COMP_TERMINAL_DA_SAIA = (Expression)workPart.Expressions.FindObject("COMP_TEMINAL_DA_SAIA");

            string codigo = "";

            string FAM = "000336";

            string[] expressao = { "COMP_SAIA" };

            double[] valor = { COMP_TERMINAL_DA_SAIA.Value };
            double[] tolerancia = { 1, 1 };
            string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");
            string[] quebra = arquivo.Split('/');
            codigo = quebra[0];

            replaceComponentBuilder1.ReplacementPart = "@DB/" + codigo + "/A"; 

            replaceComponentBuilder1.SetComponentReferenceSetType(NXOpen.Assemblies.ReplaceComponentBuilder.ComponentReferenceSet.Maintain, null);

            PartLoadStatus partLoadStatus2;
            partLoadStatus2 = replaceComponentBuilder1.RegisterReplacePartLoadStatus();

            NXObject nXObject1;
            nXObject1 = replaceComponentBuilder1.Commit();

            partLoadStatus2.Dispose();
            theSession.DeleteUndoMark(markId4, null);


            replaceComponentBuilder1.Destroy();

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component nullAssemblies_Component = null;
            PartLoadStatus partLoadStatus3;
            theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus3);

            workPart = theSession.Parts.Work;
            partLoadStatus3.Dispose();
            theSession.SetUndoMarkName(markId5, "Make Work Part");

        }
        public void codificar_pecas_teto()
        {
            lbl_proccess.Text = "Criando e substituindo itens do Teto";
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_TETO_M4/A 1");
            PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

            workPart = theSession.Parts.Work;
            partLoadStatus1.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            // ----------------------------------------------
            //   Menu: Assemblies->Components->Replace Component...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.Assemblies.ReplaceComponentBuilder replaceComponentBuilder1;
            replaceComponentBuilder1 = workPart.AssemblyManager.CreateReplaceComponentBuilder();

            replaceComponentBuilder1.ComponentNameType = NXOpen.Assemblies.ReplaceComponentBuilder.ComponentNameOption.AsSpecified;

            replaceComponentBuilder1.ComponentName = "000436";

            theSession.SetUndoMarkName(markId2, "Replace Component Dialog");

            replaceComponentBuilder1.ComponentName = "";

            DisplayableObject[] objects1 = new DisplayableObject[3];
            NXOpen.Assemblies.Component component2 = (NXOpen.Assemblies.Component)component1.FindObject("COMPONENT TPL_ULTIMO_Z/A 1");
            objects1[0] = component2;
            NXOpen.Assemblies.Component component3 = (NXOpen.Assemblies.Component)component1.FindObject("COMPONENT TPL_ULTIMO_Z/A 3");
            objects1[1] = component3;
            NXOpen.Assemblies.Component component4 = (NXOpen.Assemblies.Component)component1.FindObject("COMPONENT TPL_ULTIMO_Z/A 2");
            objects1[2] = component4;
            bool added1;
            added1 = replaceComponentBuilder1.ComponentsToReplace.Add(objects1);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            theSession.SetUndoMarkName(markId3, "Part file name Dialog");

            // ----------------------------------------------
            //   Dialog Begin Part file name
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Part file name");

            theSession.DeleteUndoMark(markId4, null);

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Part file name");

            theSession.DeleteUndoMark(markId5, null);

            theSession.SetUndoMarkName(markId3, "Part file name");

            theSession.DeleteUndoMark(markId3, null);

            theSession.Parts.SetNonmasterSeedPartData("@DB/000315/A");

            replaceComponentBuilder1.ComponentName = "000315";

            NXOpen.Session.UndoMarkId markId6;
            markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Replace Component");

            theSession.DeleteUndoMark(markId6, null);

            NXOpen.Session.UndoMarkId markId7;
            markId7 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Replace Component");

            string codigo = "";
            //-------------------------------------------------------------------------
            Expression busca_comp_ultimo_z = (Expression)workPart.Expressions.FindObject("COMP_ULTIMO_Z");

            if (busca_comp_ultimo_z.Value > 150)
            {
                string FAM = "000304";

                string[] expressao = { "COMP_PERFIL" };

                double[] valor = { busca_comp_ultimo_z.Value };
                double[] tolerancia = { 1, 1 };
                string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");
                string[] quebra = arquivo.Split('/');
                codigo = quebra[0];
            }

            replaceComponentBuilder1.ReplacementPart = "@DB/" + codigo + "/A";

            replaceComponentBuilder1.SetComponentReferenceSetType(NXOpen.Assemblies.ReplaceComponentBuilder.ComponentReferenceSet.Maintain, null);

            PartLoadStatus partLoadStatus2;
            partLoadStatus2 = replaceComponentBuilder1.RegisterReplacePartLoadStatus();

            NXObject nXObject1;
            nXObject1 = replaceComponentBuilder1.Commit();

            partLoadStatus2.Dispose();
            theSession.DeleteUndoMark(markId7, null);

            replaceComponentBuilder1.Destroy();

            NXOpen.Session.UndoMarkId markId8;
            markId8 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component nullAssemblies_Component = null;
            PartLoadStatus partLoadStatus3;
            theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus3);

            workPart = theSession.Parts.Work;
            partLoadStatus3.Dispose();
            theSession.SetUndoMarkName(markId8, "Make Work Part");
     
        }

        private void btn_codificar_Click(object sender, EventArgs e)
        {
            lbl_proccess.Text = "";
            codificar_pecas_teto();
            codificar_pecas_lat_dir();
            codificar_pecas_lat_esq();
            saveas();
            lbl_proccess.Text = "Casulo Criado";
        }

        public void add_portinholas_ld()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_CHAPEAMENTO/A 1");
            PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

            workPart = theSession.Parts.Work;
            partLoadStatus1.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Add Component");

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            theSession.SetUndoMarkName(markId3, "Add Component Dialog");

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId2);

            double vao_port_1 = 0;
            double vao_port_2 = 0;
            double vao_port_3 = 0;
            double vao_port_4 = 0;
            double vao_port_1_le = 0;
            double vao_port_2_le = 0;
            double vao_port_3_le = 0;
            double vao_port_4_le = 0;
            double vao_bag_tras = 0;

            string codigo_cmp1 = "";
            string codigo_cmp2 = "";
            string codigo_cmp3 = "";
            string codigo_cmp4 = "";

            string codigo_cmp1_le = "";
            string codigo_cmp2_le = "";
            string codigo_cmp3_le = "";
            string codigo_cmp4_le = "";

            string codigo_cmp5_tras_ld = "";
            string codigo_cmp5_tras_le = "";

            // condiçoes portinhola 1
            // LD
            if (cmbox_vao_1_ld.Text != "TQ_ARLA" && cmbox_vao_1_ld.Text != "BATERIA")
            {
                vao_port_1 = Convert.ToDouble(cmbox_vao_1_ld.Text);
                string FAM = "000708";
                string[] expressao = { "VAO_PORT" };

                double[] valor = { vao_port_1 };
                double[] tolerancia = { 1, 1 };
                string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");
                string[] sem_rev = arquivo.Split('/');
                codigo_cmp1 = sem_rev[0];
            }

            if (cmbox_vao_1_ld.Text == "TQ_ARLA")
            {
                vao_port_1 = 1100;
                codigo_cmp1 = "000715";

            }
            if (cmbox_vao_1_ld.Text == "BATERIA")
            {
                vao_port_1 = 600;
                codigo_cmp1 = "000716";

            }
            // LE
            if (cmbox_vao_1_le.Text != "TQ_ARLA" && cmbox_vao_1_le.Text != "BATERIA")
            {
                vao_port_1_le = Convert.ToDouble(cmbox_vao_1_le.Text);
                string FAM = "000708";
                string[] expressao = { "VAO_PORT" };

                double[] valor = { vao_port_1_le };
                double[] tolerancia = { 1, 1 };
                string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");
                string[] sem_rev = arquivo.Split('/');
                codigo_cmp1_le = sem_rev[0];
            }

            if (cmbox_vao_1_le.Text == "TQ_ARLA")
            {
                vao_port_1_le = 1100;
                codigo_cmp1_le = "000715";

            }
            if (cmbox_vao_1_le.Text == "BATERIA")
            {
                vao_port_1_le = 600;
                codigo_cmp1_le = "000716";

            }

            //condiçoes portinhola 2
            // LD
            if (cmbox_vao_2_ld.Text != "TQ_ARLA" && cmbox_vao_2_ld.Text != "BATERIA")
            {
                vao_port_2 = Convert.ToDouble(cmbox_vao_2_ld.Text);
                string FAM = "000708";
                string[] expressao = { "VAO_PORT" };

                double[] valor = { vao_port_2 };
                double[] tolerancia = { 1, 1 };
                string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");
                string[] sem_rev = arquivo.Split('/');
                codigo_cmp2 = sem_rev[0];
            }

            if (cmbox_vao_2_ld.Text == "TQ_ARLA")
            {
                vao_port_2 = 1100;
                codigo_cmp2 = "000715";

            }
            if (cmbox_vao_2_ld.Text == "BATERIA")
            {
                vao_port_2 = 600;
                codigo_cmp2 = "000716";

            }
            // LE
            if (cmbox_vao_2_le.Text != "TQ_ARLA" && cmbox_vao_2_le.Text != "BATERIA")
            {
                vao_port_2_le = Convert.ToDouble(cmbox_vao_2_le.Text);
                string FAM = "000708";
                string[] expressao = { "VAO_PORT" };

                double[] valor = { vao_port_2_le };
                double[] tolerancia = { 1, 1 };
                string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");
                string[] sem_rev = arquivo.Split('/');
                codigo_cmp2_le = sem_rev[0];
            }

            if (cmbox_vao_2_le.Text == "TQ_ARLA")
            {
                vao_port_2_le = 1100;
                codigo_cmp2_le = "000715";

            }
            if (cmbox_vao_2_le.Text == "BATERIA")
            {
                vao_port_2_le = 600;
                codigo_cmp2_le = "000716";

            }
            //condiçoes portinhola 3
            //LD
            if (cmbox_vao_3_ld.Text != "")
            {
                vao_port_3 = Convert.ToDouble(cmbox_vao_3_ld.Text);
                string FAM = "000708";
                string[] expressao = { "VAO_PORT" };

                double[] valor = { vao_port_3 };
                double[] tolerancia = { 1, 1 };
                string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");
                string[] sem_rev = arquivo.Split('/');
                codigo_cmp3 = sem_rev[0];
            }
            //LE
            if (cmbox_vao_3_le.Text != "")
            {
                vao_port_3_le = Convert.ToDouble(cmbox_vao_3_le.Text);
                string FAM = "000708";
                string[] expressao = { "VAO_PORT" };

                double[] valor = { vao_port_3_le };
                double[] tolerancia = { 1, 1 };
                string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");
                string[] sem_rev = arquivo.Split('/');
                codigo_cmp3_le = sem_rev[0];

            }
            //condiçoes portinhola 4
            //LD
            if (cmbox_vao_4_ld.Text != "NÃO")
            {
                vao_port_4 = Convert.ToDouble(cmbox_vao_4_ld.Text);
                string FAM = "000708";
                string[] expressao = { "VAO_PORT" };

                double[] valor = { vao_port_4 };
                double[] tolerancia = { 1, 1 };
                string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");
                string[] sem_rev = arquivo.Split('/');
                codigo_cmp4 = sem_rev[0];

            }
            //LE
            if (cmbox_vao_4_le.Text != "NÃO")
            {
                vao_port_4_le = Convert.ToDouble(cmbox_vao_4_le.Text);
                string FAM = "000708";
                string[] expressao = { "VAO_PORT" };

                double[] valor = { vao_port_4_le };
                double[] tolerancia = { 1, 1 };
                string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");
                string[] sem_rev = arquivo.Split('/');
                codigo_cmp4_le = sem_rev[0];

            }
            //// bag tras ld
            if (cmbox_vao_bag_tras.Text != "NÃO") 
            {
                vao_bag_tras = Convert.ToDouble(cmbox_vao_bag_tras.Text);
                string FAM = "000720";
                string[] expressao = { "VAO_BAG", "VAO_PORT" };

                double[] valor = { vao_bag_tras, vao_port_tras11 };
                double[] tolerancia = { 1, 1, 1, 1 };
                string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");
                string[] sem_rev = arquivo.Split('/');
                codigo_cmp5_tras_ld = sem_rev[0];

            }
            //// bag tras le
            if (cmbox_vao_bag_tras.Text != "NÃO")
            {
                vao_bag_tras = Convert.ToDouble(cmbox_vao_bag_tras.Text);
                string FAM = "000722";
                string[] expressao = { "VAO_BAG", "VAO_PORT" };

                double[] valor = { vao_bag_tras, vao_port_tras11 };
                double[] tolerancia = { 1, 1, 1, 1 };
                string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");
                string[] sem_rev = arquivo.Split('/');
                codigo_cmp5_tras_le = sem_rev[0];

            }

            double posy_port_1 = Convert.ToDouble(Pos_Ponto_0)+((vao_port_1)/2+700);
            double posy_port_2 = posy_port_1 + ((vao_port_1 / 2)+(vao_port_2 / 2));
            double posy_port_3 = posy_port_2 + ((vao_port_2 / 2)+(vao_port_3 / 2));
            double posy_port_4 = posy_port_3 + ((vao_port_3 / 2)+(vao_port_4 / 2));
            double posy_port_1_le = Convert.ToDouble(Pos_Ponto_0) + ((vao_port_1_le) / 2 + 700);
            double posy_port_2_le = posy_port_1_le + ((vao_port_1_le / 2) + (vao_port_2_le / 2));
            double posy_port_3_le = posy_port_2_le + ((vao_port_2_le / 2) + (vao_port_3_le / 2));
            double posy_port_4_le = posy_port_3_le + ((vao_port_3_le / 2) + (vao_port_4_le / 2));
            double posy_port_tq_comb = (Convert.ToDouble(EE) + Convert.ToDouble(Pos_Ponto_0) + Convert.ToDouble(INICIO_BAG_TRASEIRO)) - (35 + (540 / 2));
            double posy_port_bag_tras  = (posy_port_tq_comb+7.5) +((540 / 2) + (vao_port_tras11/2));


            Point3d basePoint1_cmp1 = new Point3d(0.0, posy_port_1, 0.0); // x,y,z
            Point3d basePoint1_cmp2 = new Point3d(0.0, posy_port_2, 0.0); // x,y,z
            Point3d basePoint1_cmp3 = new Point3d(0.0, posy_port_3, 0.0); // x,y,z
            Point3d basePoint1_cmp4 = new Point3d(0.0, posy_port_4, 0.0); // x,y,z
            Point3d basePoint1_cmp1_le = new Point3d(0.0, posy_port_1_le, 0.0); // x,y,z
            Point3d basePoint1_cmp2_le = new Point3d(0.0, posy_port_2_le, 0.0); // x,y,z
            Point3d basePoint1_cmp3_le = new Point3d(0.0, posy_port_3_le, 0.0); // x,y,z
            Point3d basePoint1_cmp4_le = new Point3d(0.0, posy_port_4_le, 0.0); // x,y,z
            Point3d basePoint1_cmp5_tq_comb = new Point3d(0.0, posy_port_tq_comb, 0.0); // x,y,z
            Point3d basePoint1_tamp_bag_tras = new Point3d(0.0, posy_port_bag_tras, 0.0); // x,y,z

            Matrix3x3 orientacao_pc_ld;
            orientacao_pc_ld.Xx = 1.0;
            orientacao_pc_ld.Xy = 0.0;
            orientacao_pc_ld.Xz = 0.0;
            orientacao_pc_ld.Yx = 0.0;
            orientacao_pc_ld.Yy = 1.0;
            orientacao_pc_ld.Yz = 0.0;
            orientacao_pc_ld.Zx = 0.0;
            orientacao_pc_ld.Zy = 0.0;
            orientacao_pc_ld.Zz = 1.0;

            Matrix3x3 orientacao_pc_le;
            orientacao_pc_le.Xx = -1.0;
            orientacao_pc_le.Xy = 0.0;
            orientacao_pc_le.Xz = 0.0;
            orientacao_pc_le.Yx = 0.0;
            orientacao_pc_le.Yy = -1.0;
            orientacao_pc_le.Yz = 0.0;
            orientacao_pc_le.Zx = 0.0;
            orientacao_pc_le.Zy = 0.0;
            orientacao_pc_le.Zz = 1.0;
            PartLoadStatus partLoadStatus2;

           
            NXOpen.Assemblies.Component port_1;
            NXOpen.Assemblies.Component port_2;
            NXOpen.Assemblies.Component port_3;
            NXOpen.Assemblies.Component port_4;
            NXOpen.Assemblies.Component port_1_LE;
            NXOpen.Assemblies.Component port_2_LE;
            NXOpen.Assemblies.Component port_3_LE;
            NXOpen.Assemblies.Component port_4_LE;
            NXOpen.Assemblies.Component port_tq_comb;
            NXOpen.Assemblies.Component port_tq_bag_tras_ld;
            NXOpen.Assemblies.Component port_tq_bag_tras_le;

            port_1 = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_cmp1 + "/" + "A", "MODEL", "", basePoint1_cmp1, orientacao_pc_ld, -1, out partLoadStatus2, true);
            port_1_LE = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_cmp1_le + "/" + "A", "MODEL", "", basePoint1_cmp1_le, orientacao_pc_le, -1, out partLoadStatus2, true);
            port_2 = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_cmp2 + "/" + "A", "MODEL", "", basePoint1_cmp2, orientacao_pc_ld, -1, out partLoadStatus2, true);
            port_2_LE = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_cmp2_le + "/" + "A", "MODEL", "", basePoint1_cmp2_le, orientacao_pc_le, -1, out partLoadStatus2, true);
            port_3 = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_cmp3 + "/" + "A", "MODEL", "", basePoint1_cmp3, orientacao_pc_ld, -1, out partLoadStatus2, true);
            port_3_LE = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_cmp3_le + "/" + "A", "MODEL", "", basePoint1_cmp3_le, orientacao_pc_le, -1, out partLoadStatus2, true);

            if (cmbox_vao_4_ld.Text != "NAO")
            {
                port_4 = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_cmp4 + "/" + "A", "MODEL", "", basePoint1_cmp4, orientacao_pc_ld, -1, out partLoadStatus2, true);
            }
            if (cmbox_vao_4_le.Text != "NAO")
            {
                port_4_LE = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_cmp4_le + "/" + "A", "MODEL", "", basePoint1_cmp4_le, orientacao_pc_le, -1, out partLoadStatus2, true);
            }

            string codigo_cmp_tq_comb = "000724";
            port_tq_comb = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_cmp_tq_comb + "/" + "A", "MODEL", "", basePoint1_cmp5_tq_comb, orientacao_pc_ld, -1, out partLoadStatus2, true);
            port_tq_comb = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_cmp_tq_comb + "/" + "A", "MODEL", "", basePoint1_cmp5_tq_comb, orientacao_pc_le, -1, out partLoadStatus2, true);

            //port_4 = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_cmp4 + "/" + "A", "MODEL", "", basePoint1_cmp4, orientacao_pc_ld, -1, out partLoadStatus2, true);
            port_tq_bag_tras_ld = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_cmp5_tras_ld + "/" + "A", "MODEL", "", basePoint1_tamp_bag_tras, orientacao_pc_ld, -1, out partLoadStatus2, true);
            port_tq_bag_tras_le = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_cmp5_tras_le + "/" + "A", "MODEL", "", basePoint1_tamp_bag_tras, orientacao_pc_ld, -1, out partLoadStatus2, true);

            partLoadStatus2.Dispose();
            NXObject[] objects1 = new NXObject[0];
            int nErrs2;
            nErrs2 = theSession.UpdateManager.AddToDeleteList(objects1);

            //theSession.SetUndoMarkName(markId3, "Add Component");

            //theSession.DeleteUndoMark(markId3, null);

            NXOpen.Session.UndoMarkId markId9;
            markId9 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component nullAssemblies_Component = null;
            PartLoadStatus partLoadStatus4;
            theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus4);
           
            workPart = theSession.Parts.Work;
            partLoadStatus4.Dispose();
            theSession.SetUndoMarkName(markId9, "Make Work Part");

        }
        public void Add_Itens()
    {
         //Session theSession = Session.GetSession();
         //   Part workPart = theSession.Parts.Work;
         //   Part displayPart = theSession.Parts.Display;
         //   NXOpen.Session.UndoMarkId markId1;
         //   markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

         //   NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_CHAPEAMENTO/A 1");
         //   PartLoadStatus partLoadStatus1;
         //   theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

         //   workPart = theSession.Parts.Work;
         //   partLoadStatus1.Dispose();
         //   theSession.SetUndoMarkName(markId1, "Make Work Part");

         //   NXOpen.Session.UndoMarkId markId2;
         //   markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Add Component");

         //   NXOpen.Session.UndoMarkId markId3;
         //   markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

         //   theSession.SetUndoMarkName(markId3, "Add Component Dialog");

         //   int nErrs1;
         //   nErrs1 = theSession.UpdateManager.DoUpdate(markId2);


         //   string codigo_fibras_frente = "000749";

         //   string codigo_fibras_tras = "000752";

         //   string codigo_teto = "";
         //   double comp_teto = 0;
             
         //   if (cmb_chassi.Text != "")
         //   {
         //       comp_teto = (Convert.ToDouble(txt_comp_carro.Text) - Convert.ToDouble(BD) - 1085 - 1335) + Convert.ToDouble(Pos_Ponto_0);
         //       string FAM = "000755";
         //       string[] expressao = { "COMP"};

         //       double[] valor = { comp_teto };
         //       double[] tolerancia = { 1, 1};
         //       string arquivo = Custom_Mascarello.Create.CreateMember(FAM, "TESTE", expressao, valor, 0, tolerancia, "", "TESTE");
         //       string[] sem_rev = arquivo.Split('/');
         //       codigo_teto = sem_rev[0];

         //   }
          

         //   double pos_fibras_frente = Convert.ToDouble(BD)-Convert.ToDouble(Pos_Ponto_0);
         //   double pos_fibras_fibra = (Convert.ToDouble(txt_comp_carro.Text) - Convert.ToDouble(BD)) + Convert.ToDouble(Pos_Ponto_0);


         //   Point3d basePoint1_fibras_frente = new Point3d(0.0, -pos_fibras_frente, 0.0); // x,y,z
         //   Point3d basePoint1_fibras_tras = new Point3d(0.0, pos_fibras_fibra, 0.0); // x,y,z
         //   Point3d basePoint1_fibras_teto = new Point3d(0.0, 0.0, 0.0); // x,y,z
         //   Matrix3x3 orientacao_pc_ld;
         //   orientacao_pc_ld.Xx = 1.0;
         //   orientacao_pc_ld.Xy = 0.0;
         //   orientacao_pc_ld.Xz = 0.0;
         //   orientacao_pc_ld.Yx = 0.0;
         //   orientacao_pc_ld.Yy = 1.0;
         //   orientacao_pc_ld.Yz = 0.0;
         //   orientacao_pc_ld.Zx = 0.0;
         //   orientacao_pc_ld.Zy = 0.0;
         //   orientacao_pc_ld.Zz = 1.0;
         //   PartLoadStatus partLoadStatus2;
             


         //   NXOpen.Assemblies.Component fibras_frente;
         //   NXOpen.Assemblies.Component fibras_tras;
         //   NXOpen.Assemblies.Component fibras_teto;
         //   fibras_frente = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_fibras_frente + "/" + "A", "MODEL", "", basePoint1_fibras_frente, orientacao_pc_ld, -1, out partLoadStatus2, true);
         //   fibras_tras = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_fibras_tras + "/" + "A", "MODEL", "", basePoint1_fibras_tras, orientacao_pc_ld, -1, out partLoadStatus2, true);
         //   fibras_teto = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_teto + "/" + "A", "MODEL", "", basePoint1_fibras_teto, orientacao_pc_ld, -1, out partLoadStatus2, true);
         //   partLoadStatus2.Dispose();
         //   NXObject[] objects1 = new NXObject[0];
         //   int nErrs2;
         //   nErrs2 = theSession.UpdateManager.AddToDeleteList(objects1);

         //   //theSession.SetUndoMarkName(markId3, "Add Component");

         //   //theSession.DeleteUndoMark(markId3, null);

         //   NXOpen.Session.UndoMarkId markId9;
         //   markId9 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

         //   NXOpen.Assemblies.Component nullAssemblies_Component = null;
         //   PartLoadStatus partLoadStatus4;
         //   theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus4);
           
         //   workPart = theSession.Parts.Work;
         //   partLoadStatus4.Dispose();
         //   theSession.SetUndoMarkName(markId9, "Make Work Part");

        }
        private void button1_Click(object sender, EventArgs e)
        {
            add_portinholas_ld();
            Add_Itens();
        }

        private void btn_import_dna_Click(object sender, EventArgs e)
        {
            dtg_view_dna.Rows.Clear();
            dtg_view_dna.Refresh();
       
            XmlDocument xmlBiblioteca = new XmlDocument();
            xmlBiblioteca.Load(@"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\NX\xml_opcoes_M4.xml");
            XElement xml = XElement.Load(@"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\NX\xml_opcoes_M4.xml");

            XmlNodeList ListaFator = default(XmlNodeList);

            ListaFator = xmlBiblioteca.SelectNodes("/Opcoes_roma_M4/Opcao");

            foreach (XmlNode fatores in ListaFator)
            {
                List.Add(fatores.ChildNodes.Item(0).InnerText);
                
            }

            

            string path = @"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\NX\DNA_M4\" + txt_fm_dna.Text + ".xml";
            DataSet dna_XML = new DataSet();
            dna_XML.ReadXml(path);
            string table = "DNA" + txt_fm_dna.Text;

            for (int i = 0; i <= List.Count - 1; i++)
            {
                string Item = CultureInfo.CurrentCulture.TextInfo.ToUpper(List[i].ToString());
                string Valor = dna_XML.Tables[table].Rows[0][List[i]].ToString();
                string[] rowi = { Item, Valor };
                dtg_view_dna.Rows.Add(rowi);
                
            }



            lbl_criacao.Text = "Dna Criado Por: " + CultureInfo.CurrentCulture.TextInfo.ToUpper(dna_XML.Tables[table].Rows[0]["CRIACAO"].ToString()) + "  Data: " + dna_XML.Tables[table].Rows[0]["DATACRIACAO"].ToString();
            lbl_alteracao.Text = "Alterado Por: " + CultureInfo.CurrentCulture.TextInfo.ToUpper(dna_XML.Tables[table].Rows[0]["ALTERACAO"].ToString()) + "  Data: " + dna_XML.Tables[table].Rows[0]["DATAALTERACAO"].ToString() + "   " + dna_XML.Tables[table].Rows[0]["HORAALTERACAO"].ToString();

            List.Clear();
       
        }

        private void btn_rditar_dna_Click(object sender, EventArgs e)
        {
            string item_pai = @"S:\\Cad.Minibuss\\CONFIGCAD_2014_MRP\\DLL\NX\\temp\\" + txt_fm_dna.Text;
            System.IO.StreamWriter vWriter = new System.IO.StreamWriter(item_pai, true);
            vWriter.Close();
            System.Diagnostics.Process.Start(@"S:\Eng.Produto\RENATO FERREIRA\C_SHARP\Desenvolvedor\App_Geracao_DNA\App_Geracao_DNA\bin\Debug\App_Geracao_DNA.exe");
        }

        private void btn_aplicar_configuracoes_Click(object sender, EventArgs e)
        {
            Configurar();
        }
        private void Configurar()
        {
            lbl_proccess.Text = "Configurando Dimenções do Casulo...";

            string Chassi = dtg_view_dna.Rows[0].Cells[1].Value.ToString();
            string CT = dtg_view_dna.Rows[1].Cells[1].Value.ToString();
            BD = "0";
            Pos_Ponto_0 = "0";
            EE = "0";
            INICIO_BAG_TRASEIRO = "0";

            if (Chassi == "MBB OF-1724 EURO V" || Chassi == "MBB OF-1721 EURO V")
            {
                BD = "2510";
                Pos_Ponto_0 = "1205";
                INICIO_BAG_TRASEIRO = "1890";

                if (Convert.ToInt32(CT) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }

            if (Chassi == "VW 17.230 OD - EURO V")
            {
                BD = "2365";
                Pos_Ponto_0 = "1060";
                INICIO_BAG_TRASEIRO = "1895";
                if (Convert.ToInt32(CT) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }

            if (Chassi == "VW 17.260 OD - EURO V")
            {
                BD = "2570";
                Pos_Ponto_0 = "1265";
                INICIO_BAG_TRASEIRO = "1895";
                if (Convert.ToInt32(CT) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }
            if (Chassi == "VOLVO - B 270F (EURO V)")
            {
                BD = "2510";
                Pos_Ponto_0 = "1205";
                INICIO_BAG_TRASEIRO = "1902";
                if (Convert.ToInt32(CT) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }
            if (Chassi == "IVECO - 170 S 28")
            {
                BD = "2510";
                Pos_Ponto_0 = "1205";
                INICIO_BAG_TRASEIRO = "2122";
                if (Convert.ToInt32(CT) >= 12800)
                {
                    EE = "6500";
                }
                else
                {
                    EE = "5950";
                }
            }

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            theSession.Preferences.Modeling.UpdatePending = false;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Expression");

            Expression expression1 = (Expression)workPart.Expressions.FindObject("CT");
            Expression expression2 = (Expression)workPart.Expressions.FindObject("BD");
            Expression expression3 = (Expression)workPart.Expressions.FindObject("EE");
            Expression expression4 = (Expression)workPart.Expressions.FindObject("DIST_P0_EIXO_DIANT");
            Expression expression5 = (Expression)workPart.Expressions.FindObject("INICIO_BAG_TRASEIRO");
            Expression expression6 = (Expression)workPart.Expressions.FindObject("NAME_CHASSI");

            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            workPart.Expressions.EditWithUnits(expression1, unit1, CT);
            workPart.Expressions.EditWithUnits(expression2, unit1, BD);
            workPart.Expressions.EditWithUnits(expression3, unit1, EE);
            workPart.Expressions.EditWithUnits(expression4, unit1, Pos_Ponto_0);
            workPart.Expressions.EditWithUnits(expression5, unit1, INICIO_BAG_TRASEIRO);

            string name_chassi_value = "\"" + Chassi + "\"";
            workPart.Expressions.EditWithUnits(expression6, null, name_chassi_value);

            theSession.Preferences.Modeling.UpdatePending = false;

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Expression Data");

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId2);

            theSession.DeleteUndoMark(markId2, "Update Expression Data");

            theSession.Preferences.Modeling.UpdatePending = true;
            

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Expression Data");

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId3);
            theSession.DeleteUndoMark(markId3, "Update Expression Data");

            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

            theSession.UpdateManager.DoInterpartUpdate(markId2);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId2);

            lbl_proccess.Text = "Casulo configurado";
            btn_add_port.Enabled = true;
            MessageBox.Show("Configurações aplicadas");
        }

        private void btn_configurar_bagageiros_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            frm_Configurar_Bagageiros_M4 frm = new frm_Configurar_Bagageiros_M4();
            frm._chassi = dtg_view_dna.Rows[0].Cells[1].Value.ToString();
            frm._ct = dtg_view_dna.Rows[1].Cells[1].Value.ToString();
            frm._entre_eixo = dtg_view_dna.Rows[2].Cells[1].Value.ToString();
            frm.Show();
            
        }

        private void txt_fm_dna_TextChanged(object sender, EventArgs e)
        {

        }


     
    }
}
