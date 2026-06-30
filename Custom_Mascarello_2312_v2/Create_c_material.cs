using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NXOpen;
using NXOpen.UF;
using NXOpen.UIStyler;

namespace Custom_Mascarello
{
    class Create_c_material
    {
        private static Session theSession;
        private static UI theUI;
        private static UFSession theUfSession;

        //private static string DAL_Default_Item_Type = "";


        public static string CreateMember(string fam, string Inicio_DB_PART_NAME, string[] Expressao, double[] valor_expressao, int Nr_Attr_Concatenar_Nome, double[] Tolerancia, string Cod_Atual, string descricao, string material)
        {
            
            string arquivo_substituto = "";

            try
            {
                theSession = Session.GetSession();
                theUI = UI.GetUI();
                theUfSession = UFSession.GetUFSession();

                string encoded_familia = TCSearchItem(fam);
                
                arquivo_substituto = Localizar_Membro_da_Familia(encoded_familia, Inicio_DB_PART_NAME, Expressao, valor_expressao, Nr_Attr_Concatenar_Nome, Tolerancia, Cod_Atual, descricao, material, fam);

            }
            catch (NXOpen.NXException ex)
            {
                // ---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("UI Styler", NXMessageBox.DialogType.Error, ex.Message + "\n " + ex.StackTrace + "\n " + ex.Source);

            }
            return arquivo_substituto;
        }



        public static string TCSearchItem(string item_id)
        {
            string encodedname = "";

            string[] entries = { "item_id"};
            string[] values = { item_id};

            Registrar_no_LOG(" DEBUG -> (TCSearchItem) Pesquisando Item:" + item_id);

            NXOpen.PDM.PdmSearch mySearch = theSession.PdmSearchManager.NewPdmSearch();
            NXOpen.PDM.SearchResult mySearchResult = mySearch.Advanced(entries, values);

            string[] results = mySearchResult.GetResultItemIds();
            
            Registrar_no_LOG(" DEBUG -> (TCSearchItem) Itens Encontrados:" + results.Length.ToString());
            if (results.Length > 0)
            {
                Registrar_no_LOG(" DEBUG -> (TCSearchItem) results[0]:" + results[0]);

                encodedname = GetEncondedPartName(results[0]);

                Registrar_no_LOG(" DEBUG -> (TCSearchItem) encodedname:" + encodedname);

                return encodedname;
               
            }
               
            else return "";
        }

        public static void Registrar_no_LOG(string Mensagem)
        {
            string gLog_FileName = @"c:\temp\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            System.IO.StreamWriter vWriter = new System.IO.StreamWriter(gLog_FileName, true, System.Text.Encoding.Unicode);
            vWriter.WriteLine(DateTime.Now.ToString() + " " + Mensagem);
            vWriter.Close();
        }

        public static string GetEncondedPartName(string item_id)
        {
            string encodedname = "";
            Tag part_tag;
            Tag[] rev_tags;
            int nrev;
            string revision_id;

            theUfSession.Ugmgr.AskPartTag(item_id, out part_tag);
            theUfSession.Ugmgr.ListPartRevisions(part_tag, out nrev, out rev_tags);
            theUfSession.Ugmgr.AskPartRevisionId(rev_tags[nrev - 1], out revision_id);
            theUfSession.Ugmgr.EncodePartFilename(item_id, revision_id, "", "", out encodedname);

            return encodedname;
        }

        public static string Localizar_Membro_da_Familia(string Familia, string Inicio_DB_PART_NAME, string[] Atributo, double[] Valor, int Nr_Attr_Concatenar_Nome, double[] Tolerancia, string Cod_Atual, string descricao, string material, string fam)
        {
            bool flg1 = true;
            string membro = "";
            while (flg1)
            {

                try
                {
                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Abrindo Familia:" + Familia);
                    PartLoadStatus LdSt;
                    Part obj_familia;




                    try
                    {
                        obj_familia = (Part)theSession.Parts.FindObject(Familia);
                    }
                    catch
                    {
                        obj_familia = theSession.Parts.Open(Familia, out LdSt);
                    }

                    
                    int j;
                    Tag[] families;
                    UFFam.FamilyData family_data;
                    string atb;
                    int[] att_idx = new int[Atributo.Length];
                    /*
                     * Localizar o Index do Atributo dentro do Excel e 
                     * armazenar no array att_idx
                     */
                    theUfSession.Part.AskFamilies(obj_familia.Tag, out j, out families);
                    theUfSession.Fam.AskFamilyData(families[0], out family_data);
                    Tag[] attr = family_data.attributes;
                    for (j = 0; j < attr.Length; j++)
                    {
                        theUfSession.Obj.AskName(attr[j], out atb);
                        for (int i = 0; i < Atributo.Length; i++)
                            if (Atributo[i] == atb)
                            {
                                Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) " + Atributo[i] + " Index: " + j.ToString());
                                att_idx[i] = j;
                            }
                    }

                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Procurando por Membros com os Atributos:");
                    for (j = 0; j < Atributo.Length; j++)
                    {
                        Registrar_no_LOG("                                        -> " + Atributo[j] + " = " + Math.Round(Valor[j], 1));
                        Registrar_no_LOG("                               Tolerancia-> = " + Tolerancia[j]);
                    }


                    /*
                     * Localizar o Membro cujos atributos são iguais aos recebidos
                     * como parametro.
                     */
                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) TESTANDO");
                    theUfSession.Part.AskFamilies(obj_familia.Tag, out j, out families);
                    theUfSession.Fam.AskFamilyData(families[0], out family_data);
                    Tag[] attributos = family_data.attributes;
                    //theUI.NXMessageBox.Show("UI Styler", NXMessageBox.DialogType.Error, "111");

                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) attributos.Length:" + attributos.Length.ToString());

                    /*
                     * Verifico se o Membro já existe na familia
                     */

                    bool[] verificacao_membros = new bool[Atributo.Length];
                    string[] aux_valor_na_familia = new string[Atributo.Length];
                    bool Membro_Ja_Existe = false;
                    for (int member_idx = 0; member_idx < family_data.member_count; member_idx++)
                        if (!Membro_Ja_Existe)
                        {
                            UFFam.MemberData member_data;
                            theUfSession.Fam.AskMemberRowData(families[0], member_idx, out member_data);
                            for (j = 0; j < Atributo.Length; j++)
                            {
                                double aux_valor_1 = Convert.ToDouble(member_data.values[att_idx[j]].Replace(".", ","));
                                double aux_valor_2 = Math.Round(Valor[j], 1);
                                double resultado = Math.Abs(aux_valor_1 - aux_valor_2);

                                Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) member_data.values[0]: " + member_data.values[0]);
                                Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) aux_valor_1: " + aux_valor_1);
                                Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) aux_valor_2: " + aux_valor_2);
                                Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) resultado..: " + resultado + " Tol: " + Tolerancia[j]);
                                if (member_idx == 0)
                                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Tolerancia.: " + Tolerancia[j]);

                                if (resultado <= Tolerancia[j])
                                {
                                    //Membro_Ja_Existe = true;
                                    verificacao_membros[j] = true;
                                    aux_valor_na_familia[j] = member_data.values[att_idx[j]];
                                    membro = member_data.values[0];
                                }
                                else
                                {
                                    verificacao_membros[j] = false;
                                }
                            }
                            Membro_Ja_Existe = true;
                            foreach (bool aux_membro in verificacao_membros)
                                if (!aux_membro)
                                {
                                    Membro_Ja_Existe = false;
                                    break;
                                }
                        }

                    /*
                    * DEBUG
                    */
                    if (Membro_Ja_Existe)
                    {
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Valor Encontrado:");
                        for (int i = 0; i < Atributo.Length; i++)
                            Registrar_no_LOG("                                        -> " + Atributo[i] + ": " + aux_valor_na_familia[i]);
                        Registrar_no_LOG("                                        -> " + membro);

                    }

                    if (!Membro_Ja_Existe)
                    {
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Criando Novo Membro da Familia.");
                        string[] dados = new string[3];
                        Tag part_tag, inst_tag;
                        int member_index, count;
                        Tag[] part_list;
                        int[] erro_list;
                        string info;
                        bool saved;

                        UFFam.MemberData new_member_data;
                        theUfSession.Fam.AskMemberRowData(families[0], family_data.member_count - 1, out new_member_data);


                        NXOpen.PDM.PartBuilder partbuilder = null;


                        bool flg = true;
                        string item_id = "";
                        while (flg) // inicio LOOPING 
                        {
                            try
                            {
                                System.Threading.Thread.Sleep(3000); // temporizador looping para não ficar usando o processador, tempo 3 segundos

                                partbuilder = theSession.Parts.PDMPartManager.NewPartFromPartBuilder();
                                //if (Cod_Atual == "")// verifica se o campo código atual  está vazio, se verdadeiro então busca codigo no daasul
                                //{
                                    

                                    
                                    item_id = partbuilder.AssignPartNumber("M4_it_mascarello");
                                   

                               // }
                                //else // se o campo codigo atual estivr preenchido entã o cria o item com o codigo informado
                                //{
                                //    item_id = Cod_Atual;
                                //}
                                flg = false;
                            }
                            catch
                            {
                                flg = true;
                            }
                        }
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Assigned item_id:" + item_id);
                   
                        new_member_data.values[0] = item_id;
                        new_member_data.values[1] = descricao;



                        new_member_data.values[3] = "M4_it_mascarello";
                        new_member_data.values[4] = "000";
                        new_member_data.values[6] = material;

                        string DB_PART_NAME = Inicio_DB_PART_NAME;

                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) DB_PART_NAME:" + DB_PART_NAME);

                        if (Inicio_DB_PART_NAME != "")
                            for (j = 0; j < Nr_Attr_Concatenar_Nome; j++)
                                DB_PART_NAME += Math.Round(Valor[j], 1).ToString().Replace(",", ".");

                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) DB_PART_NAME:" + DB_PART_NAME);

                        if (DB_PART_NAME != "")
                            new_member_data.values[2] = descricao;

                        
                        string linha = string.Format(new_member_data.values[0]).PadRight(10, ' ') + "|" +
                                       string.Format("000").PadRight(13, ' ') + "|" +
                         
                                       string.Format(DB_PART_NAME);

                        for (int k = 0; k < Atributo.Length; k++)
                        {
                            string aux_valor = Math.Round(Valor[k], 1).ToString("#.#").Replace(',', '.');

                            if (aux_valor == "") aux_valor = "0";

                            new_member_data.values[att_idx[k]] = aux_valor;
                            
                        }

                        
                        member_index = family_data.member_count;
                        theUfSession.Fam.AddMember(families[0], ref new_member_data, out member_index);
                        theUfSession.Part.CreateFamilyInstance(families[0], member_index, out part_tag, out inst_tag);
                        theUfSession.Part.UpdateFamilyInstance(families[0], member_index, true, out part_tag, out saved, out count, out part_list, out erro_list, out info);
                        theUfSession.Modl.Update();

                        Part new_item;
                        
                            theSession = Session.GetSession();
                            theUI = UI.GetUI();
                            theUfSession = UFSession.GetUFSession();
                           

                            string ja_criou = TCSearchItem(item_id);
                           // System.Windows.Forms.MessageBox.Show(existe);

                            if (ja_criou !="")
                            {
                                BasePart obj_membro = (BasePart)NXOpen.Utilities.NXObjectManager.Get(part_tag);
                                membro = obj_membro.FullPath;
                                Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Membro criado:" + Convert.ToString(item_id));
                            frmPrincipal.Registrar_no_LOG("FAM:" + fam + "  item:" + item_id);
                            atualizacao_fam_2312.update_create_item_fam(fam, item_id, obj_familia);
                            
                                obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
                            }

                        else
                        {
                          //  System.Windows.Forms.MessageBox.Show("criando novamente");
                            // member_index = family_data.member_count;
                            //  theUfSession.Fam.AddMember(families[0], ref new_member_data, out member_index);
                            theUfSession.Part.CreateFamilyInstance(families[0], member_index, out part_tag, out inst_tag);
                            theUfSession.Part.UpdateFamilyInstance(families[0], member_index, true, out part_tag, out saved, out count, out part_list, out erro_list, out info);
                            theUfSession.Modl.Update();

                            BasePart obj_membro = (BasePart)NXOpen.Utilities.NXObjectManager.Get(part_tag);
                            membro = obj_membro.FullPath;
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Membro criado:" + Convert.ToString(item_id));
                            
                            frmPrincipal.Registrar_no_LOG("FAM:" + fam + "  item:" + item_id);
                            atualizacao_fam_2312.update_create_item_fam(fam, item_id, obj_familia);

                            //obj_membro.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
                            obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
                        }
        
                    }
                    flg1 = false;

                }
                catch
                {
                    flg1 = true;
                }
                
            }
            
            return membro;
        }
    }
}
