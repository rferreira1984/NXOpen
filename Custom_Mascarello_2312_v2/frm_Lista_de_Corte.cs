using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NXOpen;
using NXOpen.UIStyler;
using NXOpen.UF;
//using System.Collections.Generic;
using NXOpen.Assemblies;
using System.Xml;
using System.Xml.Linq;


namespace Custom_Mascarello
{
    public partial class frm_contraventamentos : Form
    {

    
        private static Session theSession;
        private static UI theUI;

        List<String> LISTA_SERRA = new List<String>();

        string[] item_master;
        string cod_cv = "";
        string codigo_agrupador="";

        string rev = "";
        string verificar = "";

        string item_pai = "";
        

        List<String> lista_mp_cod = new List<String>();
        List<Double> lista_mp_qtd = new List<Double>();
        public frm_contraventamentos()
        {
            InitializeComponent();
        }

        private void btn_criar_add_Click(object sender, EventArgs e)
        {
            this.Hide();
            lista_mp_cod.Clear();
            lista_mp_qtd.Clear();

            var retorno = Verificar_Agrupador();
         
           // gerar =  Verificar_Agrupador();
            if (retorno.Item1 == false)
            {
                if(retorno.Item2 == true)
                {
                    Criar_Agrupador();
                    Buscar_itens();
                }
                else
                {
                    Buscar_itens();
                }
            }
        }
        public void criacao_externa()
        {
            this.Hide();
            lista_mp_cod.Clear();
            lista_mp_qtd.Clear();

            var retorno = Verificar_Agrupador();

            // gerar =  Verificar_Agrupador();
            if (retorno.Item1 == false)
            {
                if (retorno.Item2 == true)
                {
                    Criar_Agrupador();
                    Buscar_itens();
                }
                else
                {
                    Buscar_itens();
                }
            }
        }
        public (bool, bool)  Verificar_Agrupador()
        {
            Session theSession;
            theSession = Session.GetSession();
            Part wp = theSession.Parts.Work;
            item_pai = "";
            item_pai = wp.FullPath;

            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();
            List<NXOpen.Assemblies.Component> Componentes_n2 = new List<NXOpen.Assemblies.Component>();

            int ini, fim, ini1, fim1;

            NXOpen.Assemblies.Component cmp1 = wp.ComponentAssembly.RootComponent;
            item_master = cmp1.DisplayName.Split('/');

            Componentes.Add(cmp1);

            ini1 = 0;
            fim1 = 1;

            Componentes.ToArray()[0].GetChildren();
            bool isReadOnly = false;
            bool criar = false;
            while (ini1 < fim1)
            {

                ini = ini1;
                fim = fim1;

                ini1 = fim;
                fim1 = ini1;
                for (int i = ini; i < fim; i++)
                {
                    NXOpen.Assemblies.Component[] cmp_agrupador = Componentes.ToArray()[i].GetChildren();

                    foreach (NXOpen.Assemblies.Component agrupador in cmp_agrupador)
                    {
                       string desc = agrupador.GetStringAttribute("DB_PART_NAME");
                       
                        //isReadOnly = agrupador.Prototype.OwningPart.IsReadOnly;

                        if (desc.Contains("CONTRAVENTAMENTOS"))
                        {
                            isReadOnly = agrupador.Prototype.OwningPart.IsReadOnly;
                            codigo_agrupador = agrupador.GetStringAttribute("DB_PART_NO");
                            rev = agrupador.GetStringAttribute("DB_PART_REV");
                           
                        }
                    }
                     string selectadmin = conexao_bd.select_user_admin(Environment.UserName);
           
                    if (codigo_agrupador != "" )
                    {
                        if (isReadOnly == true && selectadmin !="1")
                        {
                            MessageBox.Show("O Agrupador de contraventamentos "+ codigo_agrupador+"-"+rev+" está em Check-Out ou Aprovado, favor verificar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            
                        }
                        else
                        {
                             
                            Excluir_Itens_Agrupador();
                            criar = false;
                        }
                    }
                    else
                    {
                        criar = true;
                    }
                }
            }
            return (isReadOnly, criar) ;
        }
        public void Excluir_Itens_Agrupador() //LIMPA CODIGO AGRUPADOR
        {
            if(rev == "")
            {
                rev = "000";
            }
            List<String> lista_deletar = new List<String>();

            Session theSession = Session.GetSession();
            Part wp = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)wp.ComponentAssembly.RootComponent.FindObject("COMPONENT " + codigo_agrupador + "/"+rev+" 1");
            PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

            wp = theSession.Parts.Work;
            partLoadStatus1.Dispose();

            try
            {
                

                List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();


                int ini, fim, ini1, fim1;

                NXOpen.Assemblies.Component cmp1 = wp.ComponentAssembly.RootComponent;

                Componentes.Add(cmp1);


                ini1 = 0;
                fim1 = 1;

                Componentes.ToArray()[0].GetChildren();

                string codigo_item = "";
                string total_itens = "";

                while (ini1 < fim1)
                {
                    ini = ini1;
                    fim = fim1;
                    int contador = 0;
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
                            string add_lista_del = "";
                            foreach (NXOpen.Assemblies.Component qtd_cmp in cmps)
                            {
                                string cod_pesq = Convert.ToString(qtd_cmp.DisplayName);
                                if (cod_pesq == cod)
                                {
                                    index_item += 1;
                                    total_itens = Convert.ToString(index_item);
                                    add_lista_del = cod + " " + index_item;

                                    if (lista_deletar.Contains(add_lista_del))
                                    {
                                    }
                                    else
                                    {
                                        contador++;
                                        lista_deletar.Add(add_lista_del);
                                    }

                                }
                            }
                        }
                    }

                    ;
                    bool notifyOnDelete1;
                    notifyOnDelete1 = theSession.Preferences.Modeling.NotifyOnDelete;

                    theSession.UpdateManager.ClearErrorList();


                    NXOpen.Session.UndoMarkId markId3;
                    markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Delete");

                    NXObject[] Lista_Objetos = new NXObject[contador];

                    for (int h = 0; h <= contador - 1; h++)
                    {
                        NXOpen.Assemblies.Component Item_del = (NXOpen.Assemblies.Component)component1.FindObject("COMPONENT " + lista_deletar[h]);
                        Lista_Objetos[h] = Item_del;

                    }

                    int nErrs2;
                    nErrs2 = theSession.UpdateManager.AddToDeleteList(Lista_Objetos);

                    int nErrs3;
                    nErrs3 = theSession.UpdateManager.DoUpdate(markId3);

                    theSession.DeleteUndoMark(markId3, null);


                    NXOpen.Assemblies.Component nullAssemblies_Component = null;
                    PartLoadStatus partLoadStatus2;
                    theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus2);


                    wp = theSession.Parts.Work;
                    partLoadStatus2.Dispose();

                    // Mover_p_agrupador();
                }


            }
            catch (Exception)
            {
             ///   MessageBox.Show("2222");

                NXOpen.Assemblies.Component nullAssemblies_Component = null;
                PartLoadStatus partLoadStatus2;
                theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus2);


                wp = theSession.Parts.Work;
                partLoadStatus2.Dispose();

                //Mover_p_agrupador();
            }

        }
        public void Buscar_itens()
        {


                theSession = Session.GetSession();
                Part wp_m = theSession.Parts.Work;

                NXObject.AttributeInformation[] attr_pai = wp_m.GetUserAttributes();

                foreach (var item in attr_pai)
                {

                    if (item.Title=="DB_PART_NO")
                    {
                        cod_cv = codigo_agrupador;

                    }
                }
                
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

                        if (LISTA_SERRA.Contains(cod + " " + total_itens)) // se item ja está na lista volta para na assembly.
                        {
                           
                        }

                        else // se item ainda não foi veridficado acessa o mesmo pra verficar se é um contraventamento
                        {

                            NXObject[] objects1 = new NXObject[1];


                            NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)(wp_m.ComponentAssembly.RootComponent.FindObject("COMPONENT " + cod + " " + total_itens)));
                            objects1[0] = component1;
                            AttributePropertiesBuilder attributePropertiesBuilder1;

                            NXObject.AttributeInformation[] attr_contra = component1.GetUserAttributes();

                            double ver_contra = 0;


                  
                        //foreach (var item in attr_contra)
                        //{
                        //    if (item.StringValue.Contains("CNC"))
                        //    {
                        //        ver_contra = 1;

                        //    }
                        //}
                        if (codigo_item.Substring(0,2) == "CV") // o item for contra add na lista da serra.
                            {

                                LISTA_SERRA.Add(codigo_item + " " + total_itens);
                           // MessageBox.Show(codigo_item + total_itens);

                            }

                        }
                        
                    }
                }
                Cria_lista_de_corte();
            Add_mp_agrupador();
        }
        private void add_tpl()
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: Assemblies->Components->Add Component...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.Assemblies.AddComponentBuilder addComponentBuilder1;
            addComponentBuilder1 = workPart.AssemblyManager.CreateAddComponentBuilder();

            addComponentBuilder1.SetAllowMultipleAssemblyLocations(false);

            NXOpen.Positioning.ComponentPositioner componentPositioner1;
            componentPositioner1 = workPart.ComponentAssembly.Positioner;

            componentPositioner1.ClearNetwork();

            componentPositioner1.BeginAssemblyConstraints();

            bool allowInterpartPositioning1;
            allowInterpartPositioning1 = theSession.Preferences.Assemblies.InterpartPositioning;

            NXOpen.Positioning.Network network1;
            network1 = componentPositioner1.EstablishNetwork();

            NXOpen.Positioning.ComponentNetwork componentNetwork1 = ((NXOpen.Positioning.ComponentNetwork)network1);
            componentNetwork1.MoveObjectsState = true;

            NXOpen.Assemblies.Component nullNXOpen_Assemblies_Component = null;
            componentNetwork1.DisplayComponent = nullNXOpen_Assemblies_Component;

            theSession.SetUndoMarkName(markId1, "Add Component Dialog");

            componentNetwork1.MoveObjectsState = true;

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Assembly Constraints Update");

            NXOpen.Assemblies.ProductInterface.InterfaceObject nullNXOpen_Assemblies_ProductInterface_InterfaceObject = null;
            addComponentBuilder1.SetComponentAnchor(nullNXOpen_Assemblies_ProductInterface_InterfaceObject);

            addComponentBuilder1.SetInitialLocationType(NXOpen.Assemblies.AddComponentBuilder.LocationType.WorkPartAbsolute);

            addComponentBuilder1.SetCount(1);

            addComponentBuilder1.SetScatterOption(true);

            addComponentBuilder1.ReferenceSet = "Unknown";

            addComponentBuilder1.Layer = -1;

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

            theSession.Parts.SetNonmasterSeedPartData("@DB/TPL_CV/000");
            NXOpen.PartLoadStatus partLoadStatus1;
            NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.Open("@DB/TPL_CV/000", out partLoadStatus1));
            //NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.FindObject("@DB/TPL_CV/000"));

            //  theSession.Part.Open(partFilePath, out workPart)
            //NXOpen.Part part2 = ((NXOpen.Part)theSession.Parts.ReopenAll("@DB/TPL_CV/000"));
            // Potential journal callback detected. Pausing journal.
            // Journal callback ended. Unpausing
            //NXOpen.BasePart basePart1;
            //NXOpen.PartLoadStatus partLoadStatus1;
            //basePart1 = theSession.Parts.OpenBase("@DB/TPL_CV/000", out partLoadStatus1);

            //partLoadStatus1.Dispose();
            addComponentBuilder1.SetUseReferenceSetAndApplyInitialLocation(false);

            addComponentBuilder1.ReferenceSet = "Model";

            addComponentBuilder1.Layer = -1;

            NXOpen.BasePart[] partstouse1 = new NXOpen.BasePart[1];
            NXOpen.Part part2 = ((NXOpen.Part)part1);
            partstouse1[0] = part2;
            addComponentBuilder1.SetPartsToAdd(partstouse1);

            NXOpen.Assemblies.ProductInterface.InterfaceObject[] productinterfaceobjects1;
            addComponentBuilder1.GetAllProductInterfaceObjects(out productinterfaceobjects1);

           


            NXOpen.Assemblies.Arrangement[] arrangements1;
            workPart.ComponentAssembly.RootComponent.GetArrangements(out arrangements1);

            NXOpen.NXObject[] movableObjects1 = new NXOpen.NXObject[1];
            NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT TPL_CV/000 1"));
            movableObjects1[0] = component1;
            componentNetwork1.SetMovingGroup(movableObjects1);

            NXOpen.Session.UndoMarkId markId6;
            markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Add Component");

            theSession.DeleteUndoMark(markId6, null);

            NXOpen.Session.UndoMarkId markId7;
            markId7 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Add Component");

            NXOpen.Session.UndoMarkId markId8;
            markId8 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "AddComponent on_apply");

            componentNetwork1.Solve();

            componentPositioner1.ClearNetwork();

            int nErrs1;
            nErrs1 = theSession.UpdateManager.AddToDeleteList(componentNetwork1);

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId2);

            componentPositioner1.EndAssemblyConstraints();

            NXOpen.PDM.LogicalObject[] logicalobjects1;
            addComponentBuilder1.GetLogicalObjectsHavingUnassignedRequiredAttributes(out logicalobjects1);

            addComponentBuilder1.ComponentName = "TPL_CV";

            NXOpen.NXObject nXObject1;
            nXObject1 = addComponentBuilder1.Commit();

            NXOpen.ErrorList errorList1;
            errorList1 = addComponentBuilder1.GetOperationFailures();

            errorList1.Dispose();
            NXOpen.Positioning.ComponentPositioner componentPositioner2;
            componentPositioner2 = workPart.ComponentAssembly.Positioner;

            componentPositioner2.PrimaryArrangement = arrangements1[0];

            NXOpen.Positioning.Network network2;
            network2 = componentPositioner2.EstablishNetwork();

            componentPositioner2.BeginAssemblyConstraints();

            NXOpen.Positioning.ComponentNetwork componentNetwork2 = ((NXOpen.Positioning.ComponentNetwork)network2);
            componentNetwork2.NetworkArrangementsMode = NXOpen.Positioning.ComponentNetwork.ArrangementsMode.Existing;

            componentNetwork2.DisplayComponent = nullNXOpen_Assemblies_Component;

            NXOpen.Positioning.Constraint constraint1;
            constraint1 = componentPositioner2.CreateConstraint(true);

            NXOpen.Positioning.ComponentConstraint componentConstraint1 = ((NXOpen.Positioning.ComponentConstraint)constraint1);
            componentConstraint1.ConstraintType = NXOpen.Positioning.Constraint.Type.Fix;

            NXOpen.Assemblies.Component component2 = ((NXOpen.Assemblies.Component)nXObject1);
            NXOpen.Positioning.ConstraintReference constraintReference1;
            constraintReference1 = componentConstraint1.CreateConstraintReference(component2, component2, false, false, false);

            NXOpen.Point3d helpPoint1 = new NXOpen.Point3d(0.0, 0.0, 0.0);
            constraintReference1.HelpPoint = helpPoint1;

            componentNetwork2.Solve();

            NXOpen.Assemblies.Arrangement nullNXOpen_Assemblies_Arrangement = null;
            componentPositioner2.PrimaryArrangement = nullNXOpen_Assemblies_Arrangement;

            componentPositioner2.ClearNetwork();

            componentPositioner2.EndAssemblyConstraints();

            NXOpen.Session.UndoMarkId id1;
            id1 = theSession.NewestVisibleUndoMark;

            int nErrs3;
            nErrs3 = theSession.UpdateManager.DoUpdate(id1);

            addComponentBuilder1.ResetPartsToAdd();

            theSession.DeleteUndoMark(markId7, null);

            theSession.SetUndoMarkName(id1, "Add Component");

            addComponentBuilder1.Destroy();

            componentPositioner2.PrimaryArrangement = nullNXOpen_Assemblies_Arrangement;

            theSession.DeleteUndoMark(markId2, null);

            // ----------------------------------------------
            //   Menu: Tools->Automation->Journal->Stop Recording
            // ----------------------------------------------
        }
        public  void saveas()
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: File->Save As...
            // ----------------------------------------------

            //NXOpen.PDM.PartFromTemplateBuilder partFromTemplateBuilder1;
            //partFromTemplateBuilder1 = theSession.Parts.PDMPartManager.NewPartFromTemplateBuilder();
            //string  item_id = partFromTemplateBuilder1.AssignPartNumber("M4_it_mascarello");
            //MessageBox.Show(item_id);
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            NXOpen.PDM.PartOperationCopyBuilder partOperationCopyBuilder1;
            partOperationCopyBuilder1 = theSession.PdmSession.CreateCopyOperationBuilder(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            partOperationCopyBuilder1.SetOperationSubType(NXOpen.PDM.PartOperationCopyBuilder.OperationSubType.Default);

            partOperationCopyBuilder1.DefaultDestinationFolder = ":TESTE_PRD";

            partOperationCopyBuilder1.DependentFilesToCopyOption = NXOpen.PDM.PartOperationCopyBuilder.CopyDependentFiles.All;

            partOperationCopyBuilder1.ReplaceAllComponentsInSession = true;

            partOperationCopyBuilder1.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.Revise);

            theSession.SetUndoMarkName(markId1, "Save Parts As Dialog");

            partOperationCopyBuilder1.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.Revise);

            NXOpen.BasePart[] selectedparts1 = new NXOpen.BasePart[0];
            NXOpen.BasePart[] failedparts1;
            partOperationCopyBuilder1.SetSelectedPartsToCopy(selectedparts1, out failedparts1);

            NXOpen.PDM.LogicalObject[] logicalobjects1;
            partOperationCopyBuilder1.CreateLogicalObjects(out logicalobjects1);

            NXOpen.BasePart[] selectedparts2 = new NXOpen.BasePart[1];
            NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.FindObject("@DB/TPL_CV/000"));
            selectedparts2[0] = part1;
            NXOpen.BasePart[] failedparts2;
            partOperationCopyBuilder1.SetSelectedPartsToCopy(selectedparts2, out failedparts2);

            NXOpen.PDM.LogicalObject[] logicalobjects2;
            partOperationCopyBuilder1.CreateLogicalObjects(out logicalobjects2);

            NXOpen.NXObject[] sourceobjects1;
            sourceobjects1 = logicalobjects2[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] sourceobjects2;
            sourceobjects2 = logicalobjects2[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] sourceobjects3;
            sourceobjects3 = logicalobjects2[0].GetUserAttributeSourceObjects();

            partOperationCopyBuilder1.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            NXOpen.NXObject[] sourceobjects4;
            sourceobjects4 = logicalobjects2[0].GetUserAttributeSourceObjects();

            NXOpen.BasePart[] selectedparts3 = new NXOpen.BasePart[1];
            selectedparts3[0] = part1;
            NXOpen.BasePart[] failedparts3;
            partOperationCopyBuilder1.SetSelectedPartsToCopy(selectedparts3, out failedparts3);

            NXOpen.PDM.LogicalObject[] logicalobjects3;
            partOperationCopyBuilder1.CreateLogicalObjects(out logicalobjects3);

            NXOpen.NXObject[] sourceobjects5;
            sourceobjects5 = logicalobjects3[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] sourceobjects6;
            sourceobjects6 = logicalobjects3[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] sourceobjects7;
            sourceobjects7 = logicalobjects3[0].GetUserAttributeSourceObjects();

            string[] attributetitles1 = new string[1];
            attributetitles1[0] = "DB_PART_REV";
            string[] titlepatterns1 = new string[1];
            titlepatterns1[0] = "NNN";
            NXOpen.NXObject nXObject1;
            nXObject1 = partOperationCopyBuilder1.CreateAttributeTitleToNamingPatternMap(attributetitles1, titlepatterns1);

            NXOpen.NXObject[] objects1 = new NXOpen.NXObject[1];
            objects1[0] = logicalobjects3[0];
            NXOpen.NXObject[] properties1 = new NXOpen.NXObject[1];
            properties1[0] = nXObject1;
            NXOpen.ErrorList errorList1;
            errorList1 = partOperationCopyBuilder1.AutoAssignAttributesWithNamingPattern(objects1, properties1);

            errorList1.Dispose();
            NXOpen.PDM.ErrorMessageHandler errorMessageHandler1;
            errorMessageHandler1 = partOperationCopyBuilder1.GetErrorMessageHandler(true);

            NXOpen.BasePart nullNXOpen_BasePart = null;
            NXOpen.NXObject[] objects2 = new NXOpen.NXObject[0];
            NXOpen.AttributePropertiesBuilder attributePropertiesBuilder1;
            attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(nullNXOpen_BasePart, objects2, NXOpen.AttributePropertiesBuilder.OperationType.SaveAs);

            NXOpen.NXObject[] objects3 = new NXOpen.NXObject[0];
            attributePropertiesBuilder1.SetAttributeObjects(objects3);

            NXOpen.NXObject[] objects4 = new NXOpen.NXObject[1];
            objects4[0] = sourceobjects5[0];
            attributePropertiesBuilder1.SetAttributeObjects(objects4);
          //  attributePropertiesBuilder1.s
            attributePropertiesBuilder1.Title = "DB_PART_NAME";

            attributePropertiesBuilder1.Category = "M4_it_mascarello";

            attributePropertiesBuilder1.StringValue = "CONTRAVENTAMENTOS_" + item_master[0];
            
            attributePropertiesBuilder1.Category = "M4_it_mascarello";

            bool changed1;
            changed1 = attributePropertiesBuilder1.CreateAttribute();

            NXOpen.NXObject[] sourceobjects8;
            sourceobjects8 = logicalobjects3[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] objects5 = new NXOpen.NXObject[0];
            attributePropertiesBuilder1.SetAttributeObjects(objects5);

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Save Parts As");

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Save Parts As");

            partOperationCopyBuilder1.ValidateLogicalObjectsToCommit();

            NXOpen.PDM.ErrorMessageHandler errorMessageHandler2;
            errorMessageHandler2 = partOperationCopyBuilder1.GetErrorMessageHandler(true);

            NXOpen.PDM.ErrorMessageHandler errorMessageHandler3;
            errorMessageHandler3 = partOperationCopyBuilder1.GetErrorMessageHandler(true);

            // Potential journal callback detected. Pausing journal.
            // Journal callback ended. Unpausing
            NXOpen.NXObject nXObject2;
            nXObject2 = partOperationCopyBuilder1.Commit();

            NXOpen.PDM.ErrorMessageHandler errorMessageHandler4;
            errorMessageHandler4 = partOperationCopyBuilder1.GetErrorMessageHandler(true);

            theSession.DeleteUndoMark(markId3, null);

            partOperationCopyBuilder1.Destroy();

            attributePropertiesBuilder1.Destroy();

            theSession.DeleteUndoMark(markId1, null);

            theSession.CleanUpFacetedFacesAndEdges();

        }
        public void verificar_codigo_criado()
        {
            Session theSession;
            theSession = Session.GetSession();
            Part wp = theSession.Parts.Work;
            item_pai = "";
            item_pai = wp.FullPath;

            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();
            List<NXOpen.Assemblies.Component> Componentes_n2 = new List<NXOpen.Assemblies.Component>();

            int ini, fim, ini1, fim1;

            NXOpen.Assemblies.Component cmp1 = wp.ComponentAssembly.RootComponent;
            item_master = cmp1.DisplayName.Split('/');

            Componentes.Add(cmp1);

            ini1 = 0;
            fim1 = 1;

            Componentes.ToArray()[0].GetChildren();
            bool isReadOnly = false;
            bool criar = false;
            while (ini1 < fim1)
            {

                ini = ini1;
                fim = fim1;

                ini1 = fim;
                fim1 = ini1;
                for (int i = ini; i < fim; i++)
                {
                    NXOpen.Assemblies.Component[] cmp_agrupador = Componentes.ToArray()[i].GetChildren();

                    foreach (NXOpen.Assemblies.Component agrupador in cmp_agrupador)
                    {
                        string desc = agrupador.GetStringAttribute("DB_PART_NAME");
                        if (desc.Contains("CONTRAVENTAMENTOS"))
                        {
                            isReadOnly = agrupador.Prototype.OwningPart.IsReadOnly;
                            codigo_agrupador = agrupador.GetStringAttribute("DB_PART_NO");
                            rev = agrupador.GetStringAttribute("DB_PART_REV");
                        }
                    }
                }
            }
        }
        public void Criar_Agrupador()
        {
            add_tpl();
            saveas();
            verificar_codigo_criado();
           // Session theSession;
           // theSession = Session.GetSession();
           // Part wp = theSession.Parts.Work;

           // NXOpen.Session.UndoMarkId markId1;
           // markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Create New Component");

           // NXOpen.PDM.PartFromTemplateBuilder partFromTemplateBuilder1;
           // partFromTemplateBuilder1 = theSession.Parts.PDMPartManager.NewPartFromTemplateBuilder();
           // string item_id = "";

           // //frm_codigo_datasul frm = new frm_codigo_datasul("CONTRAVENTAMENTOS_"+ item_master[0]);

           // //frm.ShowDialog();

           // //item_id = frm.Codigo;

           // item_id = partFromTemplateBuilder1.AssignPartNumber("M4_it_mascarello");
           // string cod_agrupador = item_id;
           // partFromTemplateBuilder1.SetContextOperation(NXOpen.PDM.PartBuilder.Operation.AssemblyCreateNewComponent);

           // FileNew New_Item;
           // New_Item = theSession.Parts.FileNew();

           // partFromTemplateBuilder1.CreatePartSpec("M4_it_mascarello", item_id, "000", "master", "");

           // NXOpen.PDM.PartCreationObject partCreationObject1;
           // partCreationObject1 = partFromTemplateBuilder1.CreatePartCreationObject();

           // NXObject[] objects1 = new NXObject[1];
           // objects1[0] = partCreationObject1;
           // AttributePropertiesBuilder attributePropertiesBuilder1;
           // attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(wp, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);

           // attributePropertiesBuilder1.IsArray = false;

           // attributePropertiesBuilder1.IsArray = false;

           // attributePropertiesBuilder1.IsArray = false;

           // attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

           // attributePropertiesBuilder1.Units = "MilliMeter";

           // attributePropertiesBuilder1.LockOnSave = true;

           // NXObject[] objects2 = new NXObject[1];
           // objects2[0] = partCreationObject1;
           // attributePropertiesBuilder1.SetAttributeObjects(objects2);

           // attributePropertiesBuilder1.Title = "";

           // attributePropertiesBuilder1.IsArray = false;

           // attributePropertiesBuilder1.LockOnSave = false;

           // attributePropertiesBuilder1.StringValue = "";

           // attributePropertiesBuilder1.IsArray = false;

           // attributePropertiesBuilder1.IsArray = false;

           // attributePropertiesBuilder1.StringValue = "";

           // attributePropertiesBuilder1.IsArray = false;

           // attributePropertiesBuilder1.IsArray = false;

           // attributePropertiesBuilder1.Category = "M4_it_mascarello";

           // attributePropertiesBuilder1.Title = "DB_PART_NAME";

           // attributePropertiesBuilder1.IsArray = false;

           // attributePropertiesBuilder1.LockOnSave = true;

           // attributePropertiesBuilder1.StringValue = item_id;

           // attributePropertiesBuilder1.IsArray = false;

           // attributePropertiesBuilder1.IsArray = false;

           // attributePropertiesBuilder1.StringValue = "CONTRAVENTAMENTOS_" + item_master[0];

           // NXObject nXObject1;
           // nXObject1 = attributePropertiesBuilder1.Commit();
           // attributePropertiesBuilder1.Destroy();

           //// theSession.PdmSession.SetDefaultFolder("Teamcenter:NX_BUS:CONTRAVENTAMENTOS");

           // partFromTemplateBuilder1.CreatePartSpec("M4_it_mascarello", null, null, "master", "");

           // New_Item.TemplateFileName = "@DB/model-plain-1-mm-template/A";

           // New_Item.UseBlankTemplate = true;

           // New_Item.ApplicationName = "AssemblyTemplate";

           // New_Item.Units = NXOpen.Part.Units.Millimeters;

           // New_Item.RelationType = "master";

           // New_Item.UsesMasterModel = "No";

           // New_Item.TemplateType = FileNewTemplateType.Item;

           // New_Item.MasterFileName = "";

           // New_Item.UseBlankTemplate = false;

           // New_Item.MakeDisplayedPart = false;


           // NXOpen.Assemblies.CreateNewComponentBuilder createNewComponentBuilder1;
           // createNewComponentBuilder1 = wp.AssemblyManager.CreateNewComponentBuilder();

           // createNewComponentBuilder1.NewComponentName = item_id;

           // createNewComponentBuilder1.ReferenceSetName = "MODEL";

           // createNewComponentBuilder1.ComponentOrigin = NXOpen.Assemblies.CreateNewComponentBuilder.ComponentOriginType.Absolute;
           // createNewComponentBuilder1.NewComponentName = item_id;

           // createNewComponentBuilder1.NewFile = New_Item;
            
           // NXOpen.Session.UndoMarkId markId11;
           // markId11 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Create New component");
           // NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.FindObject("@DB/model-plain-1-mm-template/A"));
           // NXObject nXObject2;
           // nXObject2 = createNewComponentBuilder1.Commit();


           // createNewComponentBuilder1.Destroy();


           // NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)nXObject2;
           // PartLoadStatus partLoadStatus1;
           // theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

           // wp = theSession.Parts.Work;
           // partLoadStatus1.Dispose();


           // theSession.Preferences.Modeling.UpdatePending = false;

           // NXOpen.Session.UndoMarkId markId13;
           // markId13 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Expression");

           // Expression expression1;
           // expression1 = wp.Expressions.CreateExpression("String", "Agrupador_contra=\"1\"");

           // theSession.Preferences.Modeling.UpdatePending = false;

           // PartSaveStatus partSaveStatus1;
           // partSaveStatus1 = wp.Save(NXOpen.BasePart.SaveComponents.True, NXOpen.BasePart.CloseAfterSave.False);

           // partSaveStatus1.Dispose();

           // codigo_agrupador = item_id;
           // ///MessageBox.Show(codigo_agrupador);
           // NXOpen.Assemblies.Component nullAssemblies_Component1 = null;
           // PartLoadStatus partLoadStatus3;
           // theSession.Parts.SetWorkComponent(nullAssemblies_Component1, out partLoadStatus3);

           // wp = theSession.Parts.Work;
           // partLoadStatus3.Dispose();

           // PartSaveStatus partSaveStatus2;
           // partSaveStatus2 = wp.Save(NXOpen.BasePart.SaveComponents.True, NXOpen.BasePart.CloseAfterSave.False);

        }
        public void Add_mp_agrupador()
        {
            if (rev == "")
            {
                rev = "000";
            }
           
            Session theSession = Session.GetSession();
            Part wp = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            string path_pai = wp.FullPath;
          //  MessageBox.Show(path_pai);
            NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)wp.ComponentAssembly.RootComponent.FindObject("COMPONENT " + codigo_agrupador + "/"+rev+" 1");
            PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, out partLoadStatus1);

            wp = theSession.Parts.Work;
            partLoadStatus1.Dispose();
            string path_filho = wp.FullPath;

            for (int i = 0; i <= lista_mp_cod.Count-1; i++)
            {
                string cod_add = lista_mp_cod[i];
                Point3d basePoint1 = new Point3d(0.0, 0.0, 0.0);
                Matrix3x3 orientation1;
                orientation1.Xx = 1.0;
                orientation1.Xy = 0.0;
                orientation1.Xz = 0.0;
                orientation1.Yx = 0.0;
                orientation1.Yy = 1.0;
                orientation1.Yz = 0.0;
                orientation1.Zx = 0.0;
                orientation1.Zy = 0.0;
                orientation1.Zz = 1.0;

                PartLoadStatus partLoadStatus2;
                NXOpen.Assemblies.Component component2;
                component2 = wp.ComponentAssembly.AddComponent("@DB/" + cod_add + "/000", "MODEL", cod_add, basePoint1, orientation1, -1, out partLoadStatus1, true);
              /// , out partLoadStatus3, true
               // partLoadStatus1.Dispose();


                NXObject[] objects1 = new NXObject[1];

                objects1[0] = component2;
                NXOpen.Assemblies.AssembliesGeneralPropertiesBuilder assembliesGeneralPropertiesBuilder1;
                assembliesGeneralPropertiesBuilder1 = wp.PropertiesManager.CreateAssembliesGeneralPropertiesBuilder(objects1);

                

                NXObject[] objects3 = new NXObject[1];
                objects3[0] = component2;
                AttributePropertiesBuilder attributePropertiesBuilder1;
                attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(wp, objects3, NXOpen.AttributePropertiesBuilder.OperationType.None);

             

                assembliesGeneralPropertiesBuilder1.NonGeometric = false;

                NXOpen.Assemblies.SelectComponentList selectComponentList12;
                selectComponentList12 = assembliesGeneralPropertiesBuilder1.SelectedObjects;

                

                bool nonGeometricState2;
                nonGeometricState2 = component2.GetNonGeometricState();


                string qtd_busca = lista_mp_qtd[i].ToString();

                decimal qtd_mp = Convert.ToDecimal(qtd_busca);


                assembliesGeneralPropertiesBuilder1.RealQuantity = Convert.ToDouble(qtd_mp);


                NXOpen.Session.UndoMarkId markId8;
                markId8 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Component Properties");

                NXObject nXObject1;
                nXObject1 = assembliesGeneralPropertiesBuilder1.Commit();
                NXObject nXObject2;
                nXObject2 = attributePropertiesBuilder1.Commit();

             

                NXOpen.Session.UndoMarkId id1;
                id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

                int nErrs2;
                nErrs2 = theSession.UpdateManager.DoUpdate(id1);

                theSession.DeleteUndoMark(markId8, null);

               // theSession.SetUndoMarkName(id1, "Component Properties");

                assembliesGeneralPropertiesBuilder1.Destroy();

                attributePropertiesBuilder1.Destroy();

            }
            Part part1 = (Part)theSession.Parts.FindObject("@DB/"+ path_filho);
            PartLoadStatus partLoadStatus_save;
            NXOpen.PartCollection.SdpsStatus status1;
            status1 = theSession.Parts.SetDisplay(wp, false, true, out partLoadStatus_save);

            wp = theSession.Parts.Work;
            displayPart = theSession.Parts.Display;
            partLoadStatus_save.Dispose();


            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();

            NXOpen.Assemblies.Component cmp = wp.ComponentAssembly.RootComponent;

            Componentes.Add(cmp);

            Componentes.ToArray()[0].GetChildren();
            //for (int i = 0; i < 1; i++)
            //{
            //    NXOpen.Assemblies.Component[] cmps = Componentes.ToArray()[i].GetChildren();
            //    foreach (NXOpen.Assemblies.Component cmp_1 in cmps)
            //    {
            //       /// MessageBox.Show(cmp_1.DisplayName);
            //        NXObject[] objects3 = new NXObject[1];
            //        objects3[0] = cmp_1;
            //        AttributePropertiesBuilder attrs;
            //        attrs = theSession.AttributeManager.CreateAttributePropertiesBuilder(wp, objects3, NXOpen.AttributePropertiesBuilder.OperationType.None);

            //        attrs.Category = "DB Component Instance";

            //        attrs.Title = "CALLOUT";

            //        attrs.IsArray = false;

            //        attrs.StringValue = "";

            //        attrs.IsArray = false;

            //        attrs.IsArray = false;

            //        attrs.StringValue =  (i + 1).ToString();

            //        NXObject nXObject2;
            //        nXObject2 = attrs.Commit();

            //        attrs.Commit();
            //        attrs.Destroy();
            //    }

            //}
            
            Part part4 = (Part)theSession.Parts.FindObject("@DB/" + path_pai);
            PartLoadStatus partLoadStatus_volta_pai;
            NXOpen.PartCollection.SdpsStatus status2;
            status2 = theSession.Parts.SetDisplay(part4, false, true, out partLoadStatus_volta_pai);

            wp = theSession.Parts.Work;
            displayPart = theSession.Parts.Display;
            partLoadStatus_volta_pai.Dispose();

           
            NXOpen.Assemblies.Component filho = (NXOpen.Assemblies.Component)wp.ComponentAssembly.RootComponent.FindObject("COMPONENT "+path_filho+" 1");
            PartLoadStatus partLoadStatus_filho;
            theSession.Parts.SetWorkComponent(filho, out partLoadStatus_filho);

            wp = theSession.Parts.Work;
            partLoadStatus1.Dispose();
          

            NXOpen.Assemblies.Component nullAssemblies_Component = null;
            PartLoadStatus partLoadStatus_pai;
            theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus_pai);

            wp = theSession.Parts.Work;
            partLoadStatus_pai.Dispose();


            Part filho_make = (Part)theSession.Parts.FindObject("@DB/" + path_filho);

            PartLoadStatus partLoadStatus1_filho;
            NXOpen.PartCollection.SdpsStatus status1_filho;
            status1_filho = theSession.Parts.SetDisplay(filho_make, false, true, out partLoadStatus1_filho);

            Part workPart_filho = theSession.Parts.Work;
            displayPart = theSession.Parts.Display;
            partLoadStatus1.Dispose();



            Part item_pai_ret = (Part)theSession.Parts.FindObject("@DB/" + item_pai);
            PartLoadStatus partLoadStatus12;
            NXOpen.PartCollection.SdpsStatus status12;
            status12 = theSession.Parts.SetDisplay(item_pai_ret, false, true, out partLoadStatus12);

            MessageBox.Show("Lista de Corte Gerada em " + "S:\\serra cnc\\CORTE\\");

        }
        public void Cria_lista_de_corte()
        {

        Part wp = theSession.Parts.Work;
        Part displayPart = theSession.Parts.Display;

            if (rev == "")
            {
                rev = "000";
             }
        if (System.IO.File.Exists(@"S:\\serra cnc\\CORTE\\" + "00" + cod_cv+ "-" + rev+ ".txt"))       
        {
            System.IO.File.Delete(@"S:\\serra cnc\\CORTE\\" + "00" + cod_cv + "-" + rev + ".txt");
        }
        string codigo_item = "";
        string total_itens = "";
        
        for (int i = 0; i <= LISTA_SERRA.Count - 1; i++)
        {
            string[] quebra = LISTA_SERRA[i].Split(' ');


            codigo_item = quebra[0];
            total_itens = quebra[1];

            Part displayPart2 = theSession.Parts.Display;
            NXOpen.Assemblies.Component component11 = (NXOpen.Assemblies.Component)wp.ComponentAssembly.RootComponent.FindObject("COMPONENT " + codigo_item + " " + total_itens);
            PartLoadStatus partLoadStatus2;

            theSession.Parts.SetWorkComponent(component11, out partLoadStatus2);
            wp = theSession.Parts.Work;

                Expression expression1 = (Expression)wp.Expressions.FindObject("id_contra");
                Expression expression12 = (Expression)wp.Expressions.FindObject("mp");
                Expression expression13 = (Expression)wp.Expressions.FindObject("QTD_BRUTO");
             
                
                double ver_contra = expression1.Value;
                string mp = Convert.ToString(expression12.StringValue).Substring(0,6);
                double qtd_mp = expression13.Value*(Convert.ToDouble(total_itens));

                if (lista_mp_cod.Contains(mp))
                {
                    for (int j = 0; j <= lista_mp_cod.Count-1; j++)
                    {
                        if (lista_mp_cod[j] == mp)
                        {
                            lista_mp_qtd[j] = lista_mp_qtd[j] + qtd_mp;

                        }
                    }
                }
                else
                {
                    lista_mp_cod.Add(mp.Substring(0, 6));
                    lista_mp_qtd.Add(qtd_mp);
                }


               


                if (ver_contra == 1)
                {

                    Expression expression2 = (Expression)wp.Expressions.FindObject("COMP");
                    Expression expression3 = (Expression)wp.Expressions.FindObject("ANG_A_V");
                    Expression expression4 = (Expression)wp.Expressions.FindObject("ANG_B_V");
                    Expression expression8 = (Expression)wp.Expressions.FindObject("ANG_A_H");
                    Expression expression9 = (Expression)wp.Expressions.FindObject("ANG_B_H");
                    Expression expression5 = (Expression)wp.Expressions.FindObject("ALT");
                    Expression expression6 = (Expression)wp.Expressions.FindObject("LARG");
                    Expression expression7 = (Expression)wp.Expressions.FindObject("ESP");

                    double comp = expression2.Value;
                    double ang_A_V = expression3.Value;
                    double ang_B_V = expression4.Value;
                    double ang_A_H = expression8.Value;
                    double ang_B_H = expression9.Value;

                    double Ang_A = 0;
                    double Ang_B = 0;

                    if (ang_A_V != 0 || ang_B_V != 0)
                    {
                        Ang_A = ang_A_V;
                        Ang_B = ang_B_V;
                    }
                    if (ang_A_H != 0 || ang_B_H != 0)
                    {
                        Ang_A = ang_A_H;
                        Ang_B = ang_B_H;
                    }


                    double alt = expression6.Value;
                    double larg = expression5.Value;

                    if (ang_A_H != 0 || ang_B_H != 0)
                    {
                        alt = expression5.Value;
                        larg = expression6.Value;

                    }

                    double esp = expression7.Value;

                    string esp_final = Convert.ToString(esp);

                    string cod_mat ="00"+ mp;
                   
                    string[] codigo_it = codigo_item.Split('/');

                    string linha = cod_cv.PadLeft(6, ' ') + "0  ".PadLeft(6, ' ') + codigo_it[0].PadLeft(8, ' ') + cod_mat.PadLeft(9, ' ') +
                        "0".PadLeft(2, ' ') + Convert.ToString(alt).PadLeft(4, ' ') + Convert.ToString(larg).PadLeft(3, ' ') + Convert.ToString(esp).Replace(",", ".").PadLeft(6, ' ') +
                        Convert.ToString(Math.Round(comp)).PadLeft(5, ' ') + Convert.ToString(Math.Round(Ang_A)).PadLeft(4, ' ') +
                        Convert.ToString(Math.Round(Ang_B)).PadLeft(4, ' ') + total_itens.PadLeft(3, ' ') + cod_mat.PadLeft(9, ' ') + "0".PadLeft(2, ' ');

                    Registrar_no_Arquivo(linha, cod_cv, rev);

                    wp = theSession.Parts.Work;
                    partLoadStatus2.Dispose();

                    NXOpen.Assemblies.Component nullAssemblies_Component1 = null;

                    theSession.Parts.SetWorkComponent(nullAssemblies_Component1, out partLoadStatus2);

                    wp = theSession.Parts.Work;
                    partLoadStatus2.Dispose();
                  
                }
            }
            
            //MessageBox.Show(lista_mp_cod.Count.ToString() +  "    " + lista_mp_qtd.Count.ToString());

            //for (int i = 0; i <= lista_mp_cod.Count-1; i++)
            //{
            //   MessageBox.Show(lista_mp_cod[i] +  "   " + lista_mp_qtd[i].ToString());
            //}

      //  MessageBox.Show("Lista de Corte Gerada em " + "S:\\serra cnc\\CORTE\\");
        }
        private void txtbox_cod_contra_KeyPress(object sender, KeyPressEventArgs e)
        {  
                if (e.KeyChar == 13)
                {
                 //   Criar_Agrupador();
                    
                }
        }

        private void btn_gerar_lista_Click(object sender, EventArgs e)
        {
           
            codigo_agrupador = "";
            LISTA_SERRA.Clear();
            Cria_lista_de_corte();
        }
        public static void Registrar_no_Arquivo(string itens, string cod_pai, string rev)
        {
            
            string cod_arq = "";
            cod_arq = cod_pai;
            string item_pai = @"S:\\serra cnc\\CORTE\\" + "00" + cod_pai+ "-" + rev + ".txt";
            System.IO.StreamWriter vWriter = new System.IO.StreamWriter(item_pai, true);
            vWriter.WriteLine(itens);
            vWriter.Close();

        }
 
        private void btn_limpar_Click(object sender, EventArgs e)
        {     
            codigo_agrupador = "";
            LISTA_SERRA.Clear();
        }
     
        private void button1_Click(object sender, EventArgs e)
        {
           
        }
        private void frm_contraventamentos_Load(object sender, EventArgs e)
        {

        }
        //public void organiza_callout()
        //{
        //    Session theSession;
        //    theSession = Session.GetSession();
        //    Part wp = theSession.Parts.Work;
        //    item_pai = "";
        //    item_pai = wp.FullPath;

        //    List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();
        //    List<NXOpen.Assemblies.Component> Componentes_n2 = new List<NXOpen.Assemblies.Component>();

        //    int ini, fim, ini1, fim1;

        //    NXOpen.Assemblies.Component cmp1 = wp.ComponentAssembly.RootComponent;
        //    item_master = cmp1.DisplayName.Split('/');

        //    Componentes.Add(cmp1);

        //    ini1 = 0;
        //    fim1 = 1;

        //    Componentes.ToArray()[0].GetChildren();

        //    while (ini1 < fim1)
        //    {

        //        ini = ini1;
        //        fim = fim1;

        //        ini1 = fim;
        //        fim1 = ini1;
        //        for (int i = ini; i < fim; i++)
        //        {
        //            NXOpen.Assemblies.Component[] cmp_agrupador = Componentes.ToArray()[i].GetChildren();

        //            foreach (NXOpen.Assemblies.Component agrupador in cmp_agrupador)
        //            {
        //                string desc = agrupador.GetStringAttribute("DB_PART_NAME");


        //                if (desc.Contains("CONTRAVENTAMENTOS"))
        //                {
        //                    string codigo_agrupador = agrupador.GetStringAttribute("DB_PART_NO")+"/"+ agrupador.GetStringAttribute("DB_PART_REV");
                          
        //                    NXObject[] objects1 = new NXObject[1];
        //                    NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)(wp.ComponentAssembly.RootComponent.FindObject("COMPONENT " + agrupador + " 1")));
        //                    objects1[0] = component1;

        //                    NXOpen.Assemblies.AssembliesGeneralPropertiesBuilder assembliesGeneralPropertiesBuilder1;
        //                    assembliesGeneralPropertiesBuilder1 = wp.PropertiesManager.CreateAssembliesGeneralPropertiesBuilder(objects1);
        //                    AttributePropertiesBuilder attributePropertiesBuilder1;
        //                    attributePropertiesBuilder1 = Session.GetSession().AttributeManager.CreateAttributePropertiesBuilder(wp, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);
        //                    attributePropertiesBuilder1.SetAttributeObjects(objects1);
        //                    attributePropertiesBuilder1.Category = "";
        //                    attributePropertiesBuilder1.IsArray = false;
        //                    attributePropertiesBuilder1.Title = "CALLOUT";
        //                    attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;
        //                    attributePropertiesBuilder1.Units = "MilliMeter";
        //                    attributePropertiesBuilder1.StringValue = conta_comp.ToString();
        //                    attributePropertiesBuilder1.Commit();
        //                    attributePropertiesBuilder1.Destroy();
        //                    try
        //                    {
        //                        component1.SetInstanceUserAttribute("CALLOUT", -1, conta_comp.ToString(), NXOpen.Update.Option.Now);
        //                    }
        //                    catch
        //                    {

        //                    }
        //                    conta_comp++;

        //                }
        //            }
                   

        //        }
        //    }

        //    // List<String> Lista_itens_cnc = new List<String>();
        //    // List<String> Lista_itens_normal = new List<String>();

        //    theSession = Session.GetSession();
        //    Part wp_m = theSession.Parts.Work;

        //    List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();

        //    NXOpen.Assemblies.Component cmp = wp_m.ComponentAssembly.RootComponent;

        //    Componentes.Add(cmp);

        //    int cont_normal = 1;
        //    int cont_cnc = 200;
        //    string codigo_item = "";
        //    string total_itens = "";
        //    Componentes.ToArray()[0].GetChildren();
        //    double quant = 0;
        //    int conta_comp = 1;
        //    for (int i = 0; i < 1; i++)
        //    {

        //        NXOpen.Assemblies.Component[] cmps = Componentes.ToArray()[i].GetChildren();
        //        foreach (NXOpen.Assemblies.Component cmp_1 in cmps)
        //        {
        //            string name = Convert.ToString(cmp_1.Name);
        //            string cod = Convert.ToString(cmp_1.DisplayName);
        //            int index_item = 0;
        //            codigo_item = cod;
        //            foreach (NXOpen.Assemblies.Component qtd_cmp_1 in cmps)
        //            {
        //                string cod_pesq = Convert.ToString(qtd_cmp_1.DisplayName);
        //                if (cod_pesq == cod)
        //                {
        //                    index_item += 1;
        //                    total_itens = Convert.ToString(index_item);
        //                }
        //            }

        //            NXObject[] objects1 = new NXObject[1];


        //            NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)(wp_m.ComponentAssembly.RootComponent.FindObject("COMPONENT " + cod + " 1")));
        //            objects1[0] = component1;
                   
        //            NXOpen.Assemblies.AssembliesGeneralPropertiesBuilder assembliesGeneralPropertiesBuilder1;
        //            assembliesGeneralPropertiesBuilder1 = wp_m.PropertiesManager.CreateAssembliesGeneralPropertiesBuilder(objects1);
        //            AttributePropertiesBuilder attributePropertiesBuilder1;
        //            attributePropertiesBuilder1 = Session.GetSession().AttributeManager.CreateAttributePropertiesBuilder(wp_m, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);
        //            attributePropertiesBuilder1.SetAttributeObjects(objects1);
        //            attributePropertiesBuilder1.Category = "";
        //            attributePropertiesBuilder1.IsArray = false;
        //            attributePropertiesBuilder1.Title = "CALLOUT";
        //            attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;
        //            attributePropertiesBuilder1.Units = "MilliMeter";
        //            attributePropertiesBuilder1.StringValue = conta_comp.ToString();
        //            attributePropertiesBuilder1.Commit();
        //            attributePropertiesBuilder1.Destroy();
        //            try
        //            {
        //                component1.SetInstanceUserAttribute("CALLOUT", -1, conta_comp.ToString(), NXOpen.Update.Option.Now);
        //            }
        //            catch
        //            {

        //            }
        //            conta_comp++;

        //            //NXObject.AttributeInformation[] att = component1.GetUserAttributes();

        //            //double ver_contra = 0;



        //            //if (!Lista_itens_normal.Contains(cod + ";" + total_itens))
        //            //{
        //            //    Lista_itens_normal.Add(cod + ";" + total_itens);
        //            //}



        //        }
        //    }
            

            //if (Lista_itens_normal.Count != 0)
            //{

            //    List<String> Lista_itens_renum = new List<String>();
            //    List<String> Lista_itens_count = new List<String>();
            //    for (int i = 0; i <= Lista_itens_normal.Count - 1; i++)  // enumera normal
            //    {
            //        string[] quebra = Lista_itens_normal[i].Split(';');
            //        int qtd_itens = Convert.ToInt32(quebra[1]);
            //        string codigo = quebra[0];

            //        for (int j = 0; j <= qtd_itens - 1; j++)
            //        {

            //            NXOpen.Session.UndoMarkId markId1;
            //            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            //            NXObject[] objects1 = new NXObject[1];
            //            NXOpen.Assemblies.Component component1 = (NXOpen.Assemblies.Component)wp_m.ComponentAssembly.RootComponent.FindObject("COMPONENT " + codigo + " " + Convert.ToString(j + 1));
            //            objects1[0] = component1;
            //            NXOpen.Assemblies.AssembliesGeneralPropertiesBuilder assembliesGeneralPropertiesBuilder1;
            //            assembliesGeneralPropertiesBuilder1 = wp_m.PropertiesManager.CreateAssembliesGeneralPropertiesBuilder(objects1);
            //            if (!Lista_itens_renum.Contains(codigo))
            //            {
            //                AttributePropertiesBuilder attributePropertiesBuilder1;
            //                attributePropertiesBuilder1 = Session.GetSession().AttributeManager.CreateAttributePropertiesBuilder(wp_m, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);
            //                attributePropertiesBuilder1.SetAttributeObjects(objects1);
            //                attributePropertiesBuilder1.Category = "";
            //                attributePropertiesBuilder1.IsArray = false;
            //                attributePropertiesBuilder1.Title = "CALLOUT";
            //                attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;
            //                attributePropertiesBuilder1.Units = "MilliMeter";
            //                attributePropertiesBuilder1.StringValue = cont_normal.ToString();
            //                attributePropertiesBuilder1.Commit();
            //                attributePropertiesBuilder1.Destroy();
            //                try
            //                {
            //                    component1.SetInstanceUserAttribute("CALLOUT", -1, cont_normal.ToString(), NXOpen.Update.Option.Now);
            //                }
            //                catch
            //                {

            //                }
            //                Lista_itens_renum.Add(codigo);
            //                Lista_itens_count.Add(codigo + ":" + cont_normal);
            //                cont_normal++;
            //            }
            //            if (Lista_itens_renum.Contains(codigo))
            //            {
            //                string cont_existe = "";
            //                for (int k = 0; k <= Lista_itens_count.Count - 1; k++)
            //                {
            //                    string[] quebra_existe = Lista_itens_count[k].Split(':');
            //                    string codigo_existe = quebra_existe[0].Replace(" ", "");
            //                    if (codigo_existe == codigo)
            //                    {
            //                        cont_existe = quebra_existe[1];
            //                    }

            //                }
            //                AttributePropertiesBuilder attributePropertiesBuilder1;
            //                attributePropertiesBuilder1 = Session.GetSession().AttributeManager.CreateAttributePropertiesBuilder(wp_m, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);
            //                attributePropertiesBuilder1.SetAttributeObjects(objects1);
            //                attributePropertiesBuilder1.Category = "";
            //                attributePropertiesBuilder1.IsArray = false;
            //                attributePropertiesBuilder1.Title = "CALLOUT";
            //                attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;
            //                attributePropertiesBuilder1.Units = "MilliMeter";
            //                attributePropertiesBuilder1.StringValue = cont_existe;
            //                attributePropertiesBuilder1.Commit();
            //                attributePropertiesBuilder1.Destroy();
            //                try
            //                {
            //                    component1.SetInstanceUserAttribute("CALLOUT", -1, cont_existe, NXOpen.Update.Option.Now);
            //                }
            //                catch
            //                {

            //                }
            //            }


            //        }
            //    }
            //}

        }


        // ANTIGO SEM AGRUPADOR ----- 05/18

        //    namespace Custom_Mascarello
        //{
        //    public partial class frm_contraventamentos : Form
        //    {
        //        private static Session theSession;
        //        private static UI theUI;

        //        List<String> LISTA_SERRA = new List<String>();

        //        string[] item_master;
        //        string cod_pai = "";
        //        string codigo_agrupador;

        //        string rev = "";
        //        string verificar = "";




        //        public frm_contraventamentos()
        //        {
        //            InitializeComponent();



        //        }

        //        private void btn_criar_add_Click(object sender, EventArgs e)
        //        {
        //            Buscar_itens();
        //        }

        //        public void Buscar_itens()
        //        {


        //            theSession = Session.GetSession();
        //            Part wp_m = theSession.Parts.Work;

        //            NXObject.AttributeInformation[] attr_pai = wp_m.GetUserAttributes();

        //            foreach (var item in attr_pai)
        //            {

        //                if (item.Title == "DB_PART_NO")
        //                {
        //                    cod_pai = item.StringValue;

        //                }
        //                if (item.Title == "DB_PART_REV")
        //                {
        //                    rev = item.StringValue;

        //                }

        //            }

        //            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();

        //            NXOpen.Assemblies.Component cmp = wp_m.ComponentAssembly.RootComponent;

        //            Componentes.Add(cmp);


        //            string codigo_item = "";
        //            string total_itens = "";
        //            Componentes.ToArray()[0].GetChildren();


        //            for (int i = 0; i < 1; i++)
        //            {
        //                NXOpen.Assemblies.Component[] cmps = Componentes.ToArray()[i].GetChildren();
        //                foreach (NXOpen.Assemblies.Component cmp_1 in cmps)
        //                {
        //                    string name = Convert.ToString(cmp_1.Name);
        //                    string cod = Convert.ToString(cmp_1.DisplayName);
        //                    int index_item = 0;
        //                    codigo_item = cod;
        //                    foreach (NXOpen.Assemblies.Component qtd_cmp_1 in cmps)
        //                    {
        //                        string cod_pesq = Convert.ToString(qtd_cmp_1.DisplayName);
        //                        if (cod_pesq == cod)
        //                        {
        //                            index_item += 1;
        //                            total_itens = Convert.ToString(index_item);
        //                        }
        //                    }

        //                    if (LISTA_SERRA.Contains(cod + " " + total_itens)) // se item ja está na lista volta para na assembly.
        //                    {

        //                    }

        //                    else // se item ainda não foi veridficado acessa o mesmo pra verficar se é um contraventamento
        //                    {

        //                        NXObject[] objects1 = new NXObject[1];


        //                        NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)(wp_m.ComponentAssembly.RootComponent.FindObject("COMPONENT " + cod + " " + total_itens)));
        //                        objects1[0] = component1;
        //                        AttributePropertiesBuilder attributePropertiesBuilder1;

        //                        NXObject.AttributeInformation[] attr_contra = component1.GetUserAttributes();

        //                        double ver_contra = 0;

        //                        foreach (var item in attr_contra)
        //                        {

        //                            if (item.StringValue.Contains("CNC"))
        //                            {
        //                                ver_contra = 1;

        //                            }

        //                        }
        //                        //}
        //                        if (ver_contra == 1) // o item for contra add na lista da serra.
        //                        {

        //                            LISTA_SERRA.Add(codigo_item + " " + total_itens);

        //                        }

        //                    }

        //                }
        //            }
        //            Cria_lista_de_corte();
        //        }


        //        public void Cria_lista_de_corte()
        //        {

        //            Part wp = theSession.Parts.Work;
        //            Part displayPart = theSession.Parts.Display;


        //            if (System.IO.File.Exists(@"S:\\serra cnc\\CORTE\\" + "00" + cod_pai + "-" + rev + ".txt"))
        //            {
        //                System.IO.File.Delete(@"S:\\serra cnc\\CORTE\\" + "00" + cod_pai + "-" + rev + ".txt");
        //            }
        //            string codigo_item = "";
        //            string total_itens = "";

        //            for (int i = 0; i <= LISTA_SERRA.Count - 1; i++)
        //            {
        //                string[] quebra = LISTA_SERRA[i].Split(' ');


        //                codigo_item = quebra[0];
        //                total_itens = quebra[1];

        //                Part displayPart2 = theSession.Parts.Display;
        //                NXOpen.Assemblies.Component component11 = (NXOpen.Assemblies.Component)wp.ComponentAssembly.RootComponent.FindObject("COMPONENT " + codigo_item + " " + total_itens);
        //                PartLoadStatus partLoadStatus2;

        //                theSession.Parts.SetWorkComponent(component11, out partLoadStatus2);
        //                wp = theSession.Parts.Work;

        //                Expression expression1 = (Expression)wp.Expressions.FindObject("id_contra");

        //                double ver_contra = expression1.Value;


        //                if (ver_contra == 1)
        //                {

        //                    Expression expression2 = (Expression)wp.Expressions.FindObject("COMP");
        //                    Expression expression3 = (Expression)wp.Expressions.FindObject("ANG_A_V");
        //                    Expression expression4 = (Expression)wp.Expressions.FindObject("ANG_B_V");
        //                    Expression expression8 = (Expression)wp.Expressions.FindObject("ANG_A_H");
        //                    Expression expression9 = (Expression)wp.Expressions.FindObject("ANG_B_H");
        //                    Expression expression5 = (Expression)wp.Expressions.FindObject("ALT");
        //                    Expression expression6 = (Expression)wp.Expressions.FindObject("LARG");
        //                    Expression expression7 = (Expression)wp.Expressions.FindObject("ESP");

        //                    double comp = expression2.Value;
        //                    double ang_A_V = expression3.Value;
        //                    double ang_B_V = expression4.Value;
        //                    double ang_A_H = expression8.Value;
        //                    double ang_B_H = expression9.Value;

        //                    double Ang_A = 0;
        //                    double Ang_B = 0;

        //                    if (ang_A_V != 0 || ang_B_V != 0)
        //                    {
        //                        Ang_A = ang_A_V;
        //                        Ang_B = ang_B_V;
        //                    }
        //                    if (ang_A_H != 0 || ang_B_H != 0)
        //                    {
        //                        Ang_A = ang_A_H;
        //                        Ang_B = ang_B_H;
        //                    }


        //                    double alt = expression6.Value;
        //                    double larg = expression5.Value;

        //                    if (ang_A_H != 0 || ang_B_H != 0)
        //                    {
        //                        alt = expression5.Value;
        //                        larg = expression6.Value;

        //                    }

        //                    double esp = expression7.Value;

        //                    string esp_final = Convert.ToString(esp);

        //                    string cod_mat = "00000000";
        //                    string Material_Desc = alt + "x" + larg + "x" + esp_final.Replace(",", ".");
        //                    string Material_Desc_inv = larg + "x" + alt + "x" + esp_final.Replace(",", ".");

        //                    XmlDocument xml_cod_MP = new XmlDocument();
        //                    xml_cod_MP.Load(@"X:\\Xml\\Lista_Material_Contraventamentos.xml");
        //                    XmlNodeList Lista_Cod_MP = default(XmlNodeList);



        //                    Lista_Cod_MP = xml_cod_MP.SelectNodes("/Lista_Material_Contra/Material");


        //                    foreach (XmlNode Codigo_MP in Lista_Cod_MP)
        //                    {
        //                        string ver_mat = Codigo_MP.ChildNodes.Item(0).InnerText;

        //                        if (ver_mat == Material_Desc || ver_mat == Material_Desc_inv)
        //                        {
        //                            cod_mat = Codigo_MP.ChildNodes.Item(1).InnerText;
        //                            //MessageBox.Show(cod_mat);
        //                        }
        //                    }


        //                    string[] codigo_it = codigo_item.Split('/');


        //                    string linha = cod_pai.PadLeft(6, ' ') + "0".PadLeft(4, ' ') + codigo_it[0].PadLeft(8, ' ') + cod_mat.PadLeft(9, ' ') +
        //                        "0".PadLeft(2, ' ') + Convert.ToString(alt).PadLeft(4, ' ') + Convert.ToString(larg).PadLeft(3, ' ') + Convert.ToString(esp).Replace(",", ".").PadLeft(6, ' ') +
        //                        Convert.ToString(Math.Round(comp)).PadLeft(5, ' ') + Convert.ToString(Math.Round(Ang_A)).PadLeft(4, ' ') +
        //                        Convert.ToString(Math.Round(Ang_B)).PadLeft(4, ' ') + total_itens.PadLeft(3, ' ') + cod_mat.PadLeft(9, ' ') + "0".PadLeft(2, ' ');

        //                    Registrar_no_Arquivo(linha, cod_pai, rev);



        //                    wp = theSession.Parts.Work;
        //                    partLoadStatus2.Dispose();

        //                    NXOpen.Assemblies.Component nullAssemblies_Component1 = null;

        //                    theSession.Parts.SetWorkComponent(nullAssemblies_Component1, out partLoadStatus2);

        //                    wp = theSession.Parts.Work;
        //                    partLoadStatus2.Dispose();
        //                }
        //            }

        //            MessageBox.Show("Lista de Corte Gerada em " + "S:\\serra cnc\\CORTE\\");

        //        }
        //        private void txtbox_cod_contra_KeyPress(object sender, KeyPressEventArgs e)
        //        {
        //            if (e.KeyChar == 13)
        //            {
        //                //   Criar_Agrupador();

        //            }
        //        }

        //        private void btn_gerar_lista_Click(object sender, EventArgs e)
        //        {

        //            codigo_agrupador = "";
        //            LISTA_SERRA.Clear();
        //            Cria_lista_de_corte();
        //        }
        //        public static void Registrar_no_Arquivo(string itens, string cod_pai, string rev)
        //        {

        //            string cod_arq = "";
        //            cod_arq = cod_pai;
        //            string item_pai = @"S:\\serra cnc\\CORTE\\" + "00" + cod_pai + "-" + rev + ".txt";
        //            System.IO.StreamWriter vWriter = new System.IO.StreamWriter(item_pai, true);
        //            vWriter.WriteLine(itens);
        //            vWriter.Close();

        //        }

        //        private void btn_limpar_Click(object sender, EventArgs e)
        //        {

        //            codigo_agrupador = "";
        //            LISTA_SERRA.Clear();
        //        }

        //        private void button1_Click(object sender, EventArgs e)
        //        {

        //        }

        //        private void frm_contraventamentos_Load(object sender, EventArgs e)
        //        {

        //        }
        //public void Verificar_Agrupador() // ANTIGO
        //{
        //    string tp_agrupador = "";
        //    Session theSession;
        //    theSession = Session.GetSession();
        //    Part wp = theSession.Parts.Work;

        //    List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();
        //    List<NXOpen.Assemblies.Component> Componentes_n2 = new List<NXOpen.Assemblies.Component>();

        //    int ini, fim, ini1, fim1;

        //    NXOpen.Assemblies.Component cmp1 = wp.ComponentAssembly.RootComponent;
        //    item_master = cmp1.DisplayName.Split('/');

        //    Componentes.Add(cmp1);

        //    ini1 = 0;
        //    fim1 = 1;

        //    Componentes.ToArray()[0].GetChildren();


        //    while (ini1 < fim1)
        //    {

        //        ini = ini1;
        //        fim = fim1;

        //        ini1 = fim;
        //        fim1 = ini1;
        //        for (int i = ini; i < fim; i++)
        //        {
        //            NXOpen.Assemblies.Component[] cmp_agrupador = Componentes.ToArray()[i].GetChildren();

        //            foreach (NXOpen.Assemblies.Component agrupador in cmp_agrupador)
        //            {
        //                string name = Convert.ToString(agrupador.Name);
        //                string cod = Convert.ToString(agrupador.DisplayName);
        //                int index_item = 0;
        //                string total_itens_agrupador = "";
        //                foreach (NXOpen.Assemblies.Component qtd_cmp in cmp_agrupador)
        //                {
        //                    string cod_pesq = Convert.ToString(qtd_cmp.DisplayName);
        //                    if (cod_pesq == cod)
        //                    {
        //                        index_item += 1;
        //                        total_itens_agrupador = Convert.ToString(index_item);
        //                    }

        //                }

        //                Part displayPart = theSession.Parts.Display;
        //                NXOpen.Assemblies.Component Cod_Contraventamentos = (NXOpen.Assemblies.Component)wp.ComponentAssembly.RootComponent.FindObject("COMPONENT " + cod + " " + total_itens_agrupador);
        //                PartLoadStatus partLoadStatus1;
        //                theSession.Parts.SetWorkComponent(Cod_Contraventamentos, out partLoadStatus1);
        //                wp = theSession.Parts.Work;
        //                try
        //                {
        //                    Expression expression1 = (Expression)wp.Expressions.FindObject("Agrupador_contra");
        //                    tp_agrupador = "normal";
        //                    cod_agrupador = Cod_Contraventamentos.Name;
        //                    codigo_agrupador = Cod_Contraventamentos.DisplayName;
        //                    this.txtbox_cod_contra.Text = cod_agrupador;
        //                    this.txtbox_Desc.Text = "CONTRAVENTAMENTOS_" + cod_agrupador;
        //                    NXOpen.Assemblies.Component nullAssemblies_Component = null;
        //                    PartLoadStatus partLoadStatus2;
        //                    theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus2);

        //                    wp = theSession.Parts.Work;
        //                    partLoadStatus2.Dispose();

        //                }
        //                catch (Exception)
        //                {


        //                }
        //                try
        //                {
        //                    Expression expression1 = (Expression)wp.Expressions.FindObject("Agrupador_contra_fam");
        //                    tp_agrupador = "familia";
        //                    cod_agrupador = Cod_Contraventamentos.Name;
        //                    codigo_agrupador = Cod_Contraventamentos.DisplayName;
        //                    this.txtbox_cod_contra.Text = cod_agrupador;
        //                    this.txtbox_Desc.Text = "CONTRAVENTAMENTOS_" + cod_agrupador;
        //                    NXOpen.Assemblies.Component nullAssemblies_Component = null;
        //                    PartLoadStatus partLoadStatus2;
        //                    theSession.Parts.SetWorkComponent(nullAssemblies_Component, out partLoadStatus2);

        //                    wp = theSession.Parts.Work;
        //                    partLoadStatus2.Dispose();

        //                }
        //                catch (Exception)
        //                {


        //                }

        //                NXOpen.Assemblies.Component nullAssemblies_Component_1 = null;
        //                PartLoadStatus partLoadStatus_2;
        //                theSession.Parts.SetWorkComponent(nullAssemblies_Component_1, out partLoadStatus_2);

        //                wp = theSession.Parts.Work;
        //                partLoadStatus_2.Dispose();
        //            }
        //        }

        //    }
        //    NXOpen.Assemblies.Component nullAssemblies_Component1 = null;
        //    PartLoadStatus partLoadStatus21;
        //    theSession.Parts.SetWorkComponent(nullAssemblies_Component1, out partLoadStatus21);

        //    wp = theSession.Parts.Work;
        //    partLoadStatus21.Dispose();

        //    if (tp_agrupador == "normal" || tp_agrupador == "")
        //    {
        //        if (cod_agrupador == "")
        //        {
        //            MessageBox.Show("Digite um código para criação dos contraventamentos");
        //            txtbox_cod_contra.Enabled = true;
        //            btn_criar_add.Enabled = false;
        //            txtbox_cod_contra.Focus();
        //        }
        //        else
        //        {
        //            Excluir_Itens_Agrupador();
        //            Mover_p_agrupador();
        //        }

        //    }
        //    if (tp_agrupador == "familia")
        //    {

        //        if (cod_agrupador == "")
        //        {
        //            MessageBox.Show("\"Item Familia \" Crie um código agrupador na familia para depois gerar a lista");
        //            this.Close();
        //        }
        //        else
        //        {
        //            Lista_Agrupador_Fam();
        //        }
        //    }


        
    
}
