using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;
using NXOpen;
using NXOpen.UF;
using NXOpen.UIStyler;
using PLMComponents.Parasolid.PK_.Unsafe;
using static NXOpen.Mechatronics.ElectricalPartBuilder;

namespace Custom_Mascarello
{
    class Create
    {
        private static Session theSession;
        private static UI theUI;
        private static UFSession theUfSession;

        //private static string DAL_Default_Item_Type = "";


        public static string CreateMember(string fam, string Inicio_DB_PART_NAME, string[] Expressao, double[] valor_expressao, int Nr_Attr_Concatenar_Nome, double[] Tolerancia, string Cod_Atual, string descricao)
        {

            string arquivo_substituto = "";

            try
            {
                theSession = Session.GetSession();
                theUI = UI.GetUI();
                theUfSession = UFSession.GetUFSession();

                string encoded_familia = TCSearchItem(fam);


                arquivo_substituto = Localizar_Membro_da_Familia(encoded_familia, Inicio_DB_PART_NAME, Expressao, valor_expressao, Nr_Attr_Concatenar_Nome, Tolerancia, Cod_Atual, descricao, fam);

            }
            catch (NXOpen.NXException ex)
            {
                // ---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("UI Styler", NXMessageBox.DialogType.Error, ex.Message + "\n " + ex.StackTrace + "\n " + ex.Source);

            }
            return arquivo_substituto;
        }
        
        public static (string, bool) CreateMember_new(string fam, string Inicio_DB_PART_NAME, string[] Expressao, double[] valor_expressao, int Nr_Attr_Concatenar_Nome, double[] Tolerancia, string Cod_Atual, string descricao, string item_type, string material, bool boletim, List<string>bt, List<string>btp)
        {

            string arquivo_substituto = "";
            bool existe = false;
            try
            {
                theSession = Session.GetSession();
                theUI = UI.GetUI();
                theUfSession = UFSession.GetUFSession();

                string encoded_familia = TCSearchItem(fam);

                if (material == "")
                {
                    if (item_type == "M4_it_cv")
                    {
                        (arquivo_substituto, existe) = Localizar_Membro_da_Familia_cv(encoded_familia, Inicio_DB_PART_NAME, Expressao, valor_expressao, Nr_Attr_Concatenar_Nome, Tolerancia, Cod_Atual, descricao, fam, item_type, boletim, bt, btp);
                    }
                    else
                    {
                        (arquivo_substituto, existe) = Localizar_Membro_da_Familia_new(encoded_familia, Inicio_DB_PART_NAME, Expressao, valor_expressao, Nr_Attr_Concatenar_Nome, Tolerancia, Cod_Atual, descricao, fam, item_type, boletim, bt, btp);
                    }
                }
                else
                {
                    (arquivo_substituto, existe) = Localizar_Membro_da_Familia_new_material(encoded_familia, Inicio_DB_PART_NAME, Expressao, valor_expressao, Nr_Attr_Concatenar_Nome, Tolerancia, Cod_Atual, descricao, fam, item_type, material);
                }
                   // (arquivo_substituto, existe) = Localizar_Membro_da_Familia_new_material(encoded_familia, Inicio_DB_PART_NAME, Expressao, valor_expressao, Nr_Attr_Concatenar_Nome, Tolerancia, Cod_Atual, descricao, fam, item_type, material);

            }
            catch (NXOpen.NXException ex)
            {
                // ---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("UI Styler", NXMessageBox.DialogType.Error, ex.Message + "\n " + ex.StackTrace + "\n " + ex.Source);

            }
            return (arquivo_substituto, existe);
        }
        public static (string, bool) CreateMember_new_automatico(string fam, string Inicio_DB_PART_NAME, string[] Expressao, double[] valor_expressao, int Nr_Attr_Concatenar_Nome, double[] Tolerancia, string Cod_Atual, string descricao, string item_type, string material, string codigo_new, bool boletim, List<string> bt, List<string> btp)
        {
            string attributos_str = string.Join(";", descricao);
            Registrar_no_LOG(" DEBUG -> (CreateMember_new_automatico) descricao:" + attributos_str);
            Registrar_no_LOG(" DEBUG -> (CreateMember_new_automatico) Codigo new:" + codigo_new);
            string arquivo_substituto = "";
            bool existe = false;
            try
            {
                theSession = Session.GetSession();
                theUI = UI.GetUI();
                theUfSession = UFSession.GetUFSession();

                string encoded_familia = TCSearchItem(fam);

                if (material == "")
                    (arquivo_substituto, existe) = Localizar_Membro_da_Familia_new_automatico(encoded_familia, Inicio_DB_PART_NAME, Expressao, valor_expressao, Nr_Attr_Concatenar_Nome, Tolerancia, Cod_Atual, descricao, fam, item_type, codigo_new, boletim, bt, btp);
                else
                    (arquivo_substituto, existe) = Localizar_Membro_da_Familia_new_material(encoded_familia, Inicio_DB_PART_NAME, Expressao, valor_expressao, Nr_Attr_Concatenar_Nome, Tolerancia, Cod_Atual, descricao, fam, item_type, material);

            }
            catch (NXOpen.NXException ex)
            {
                // ---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("UI Styler", NXMessageBox.DialogType.Error, ex.Message + "\n " + ex.StackTrace + "\n " + ex.Source);

            }
            return (arquivo_substituto, existe);
        }
        public static (string, bool) CreateMember_new_automatico_create_demanda(string fam, string Inicio_DB_PART_NAME, string[] Expressao, double[] valor_expressao, int Nr_Attr_Concatenar_Nome, double[] Tolerancia, string Cod_Atual, string descricao, string item_type, string material, string codigo_new, bool boletim, List<string> bt, List<string> btp)
        {
            string attributos_str = string.Join(";", descricao);
            Registrar_no_LOG(" DEBUG -> (CreateMember_new_automatico) descricao:" + attributos_str);
            Registrar_no_LOG(" DEBUG -> (CreateMember_new_automatico) Codigo new:" + codigo_new);
            string arquivo_substituto = "";
            bool existe = false;
            try
            {
                theSession = Session.GetSession();
                theUI = UI.GetUI();
                theUfSession = UFSession.GetUFSession();

                string encoded_familia = TCSearchItem(fam);

                if (material == "")
                    (arquivo_substituto, existe) = Localizar_Membro_da_Familia_new_automatico_demanda(encoded_familia, Inicio_DB_PART_NAME, Expressao, valor_expressao, Nr_Attr_Concatenar_Nome, Tolerancia, Cod_Atual, descricao, fam, item_type, codigo_new, boletim, bt, btp);
                else
                    (arquivo_substituto, existe) = Localizar_Membro_da_Familia_new_material(encoded_familia, Inicio_DB_PART_NAME, Expressao, valor_expressao, Nr_Attr_Concatenar_Nome, Tolerancia, Cod_Atual, descricao, fam, item_type, material);

            }
            catch (NXOpen.NXException ex)
            {
                // ---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("UI Styler", NXMessageBox.DialogType.Error, ex.Message + "\n " + ex.StackTrace + "\n " + ex.Source);

            }
            return (arquivo_substituto, existe);
        }
        public static string TCSearchItem(string item_id)
        {
            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theUfSession = UFSession.GetUFSession();
            string encodedname = "";

            string[] entries = { "item_id" };
            string[] values = { item_id };

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
       
        public static (string, bool) Localizar_Membro_da_Familia_new(string Familia, string Inicio_DB_PART_NAME, string[] Atributo, double[] Valor, int Nr_Attr_Concatenar_Nome, double[] Tolerancia, string Cod_Atual, string descricao, string fam, string item_type, bool boletim, List<string>bt, List<string>btp)
        {

           
            bool flg1 = true;
            string membro = "";
            bool existe = false;
            while (flg1)
            {

                try
                {
                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Abrindo Familia:" + Familia);
                    PartLoadStatus LdSt;
                    Part obj_familia;
                    Part obj_atual;
                    try
                    {
                        obj_familia = (Part)theSession.Parts.FindObject(Familia);
                    }
                    catch
                    {
                        obj_familia = theSession.Parts.Open(Familia, out LdSt);
                    }


                    
                    if (boletim == true)
                    {

                        if (obj_familia != null)
                        {
                            theSession.Parts.SetDisplay(obj_familia, true, true, out LdSt);

                            theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");
                        }


                        frmPrincipal frm = new frmPrincipal();

                    frm.insert_boletin_criacao(bt, btp, obj_familia);
                    }
                    else
                    {
                        if (obj_familia != null)
                        {
                            theSession.Parts.SetDisplay(obj_familia, true, true, out LdSt);

                            theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");
                        }
                    }
                    frmPrincipal.deletar_tabela_fam("REVISAO_ITENS", obj_familia);

                    int j;
                    Tag[] families;
                    UFFam.FamilyData family_data;
                    string atb;
                    int[] att_idx = new int[Atributo.Length];


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

                                //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) member_data.values[0]: " + member_data.values[0]);
                                //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) aux_valor_1: " + aux_valor_1);
                                //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) aux_valor_2: " + aux_valor_2);
                                //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) resultado..: " + resultado + " Tol: " + Tolerancia[j]);
                                if (member_idx == 0)
                                  //  Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Tolerancia.: " + Tolerancia[j]);

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
                        existe = true;
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
                                item_id = partbuilder.AssignPartNumber(item_type);

                                flg = false;
                                if (item_id == "" || item_id == "(null)")
                                {
                                    flg = true;
                                }
                            }
                            catch
                            {
                                flg = true;
                            }
                        }
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Assigned item_id:" + item_id);

                        new_member_data.values[0] = item_id;
                        new_member_data.values[1] = descricao;



                        new_member_data.values[3] = item_type;
                        new_member_data.values[4] = "000";

                        string DB_PART_NAME = Inicio_DB_PART_NAME;

                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) DB_PART_NAME:" + DB_PART_NAME);

                        if (Inicio_DB_PART_NAME != "")
                            for (j = 0; j < Nr_Attr_Concatenar_Nome; j++)
                                DB_PART_NAME += Math.Round(Valor[j], 1).ToString().Replace(",", ".");



                        if (DB_PART_NAME != "")
                            new_member_data.values[2] = descricao;


                        string linha = string.Format(new_member_data.values[0]).PadRight(10, ' ') + "|" +
                                       string.Format("000").PadRight(13, ' ') + "|" +

                                       string.Format(DB_PART_NAME);
                       
                        // lw.WriteLine(linha);



                        for (int k = 0; k < Atributo.Length; k++)
                        {
                            string aux_valor = Math.Round(Valor[k], 1).ToString("#.#").Replace(',', '.');

                            if (aux_valor == "") aux_valor = "0";

                            new_member_data.values[att_idx[k]] = aux_valor;
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + att_idx[k] +"  valor:"+ aux_valor);
                        }


                        member_index = family_data.member_count;

                      

                        theUfSession.Fam.AddMember(families[0], ref new_member_data, out member_index);

                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + new_member_data.values);
                        theUfSession.Part.CreateFamilyInstance(families[0], member_index, out part_tag, out inst_tag);

                        try
                        {
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + families[0]);
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + families[0]);
                            theUfSession.Part.UpdateFamilyInstance(families[0], member_index, true, out part_tag, out saved, out count, out part_list, out erro_list, out info);
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + "1");
                        }
                        catch
                {
                           Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + "2");
                        }
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + "3");
                        theUfSession.Modl.Update();
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + "4");
                        Part new_item;
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + "5");
                        theSession = Session.GetSession();
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + "6");
                        theUI = UI.GetUI();
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + "7");
                        theUfSession = UFSession.GetUFSession();
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + "8");

                        string ja_criou = TCSearchItem(item_id);
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + "9");
                        if (ja_criou != "")
                        {
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + "10");
                            BasePart obj_membro = (BasePart)NXOpen.Utilities.NXObjectManager.Get(part_tag);
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + "11");
                            membro = obj_membro.FullPath;
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Membro criado:" + Convert.ToString(item_id));
                            frmPrincipal.Registrar_no_LOG("FAM:" + fam + "  item:" + item_id);
                            atualizacao_fam_2312.update_create_item_fam(fam, item_id, obj_familia);
                            if (boletim == true)
                            {
                                frmPrincipal frm = new frmPrincipal();

                                frm.deletar_tabelas(obj_familia);
                            }
                            obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
                        }
                        else
                        {
                            theUfSession.Fam.AddMember(families[0], ref new_member_data, out member_index);
                            theUfSession.Part.CreateFamilyInstance(families[0], member_index, out part_tag, out inst_tag);
                            theUfSession.Part.UpdateFamilyInstance(families[0], member_index, true, out part_tag, out saved, out count, out part_list, out erro_list, out info);
                            theUfSession.Modl.Update();

                            BasePart obj_membro = (BasePart)NXOpen.Utilities.NXObjectManager.Get(part_tag);
                            membro = obj_membro.FullPath;

                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Membro criado:" + Convert.ToString(item_id));
                            frmPrincipal.Registrar_no_LOG("FAM:" + fam + "  item:" + item_id);
                            atualizacao_fam_2312.update_create_item_fam(fam, item_id, obj_familia);

                            if (boletim == true)
                            {

                                frmPrincipal frm = new frmPrincipal();

                                frm.deletar_tabelas( obj_familia);
                            }
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
            return (membro, existe);
        }
        public static (string, bool) Localizar_Membro_da_Familia_new_mp_automatico(string Familia, string Inicio_DB_PART_NAME, string[] Atributo, double[] Valor, int Nr_Attr_Concatenar_Nome, double[] Tolerancia, string Cod_Atual, string descricao, string fam, string item_type, bool boletim, List<string> bt, List<string> btp, string mp)
        {


            bool flg1 = true;
            string membro = "";
            bool existe = false;
            while (flg1)
            {

                //try
                //{
                

                Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_mp) Abrindo Familia:" + Familia);
                PartLoadStatus LdSt;
                Part obj_familia;
                Part obj_atual;
                try
                {
                    obj_familia = (Part)theSession.Parts.FindObject(Familia);
                }
                catch
                {
                    obj_familia = theSession.Parts.Open(Familia, out LdSt);
                }



                if (boletim == true)
                {

                    if (obj_familia != null)
                    {
                        theSession.Parts.SetDisplay(obj_familia, true, true, out LdSt);

                        theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");
                    }


                    frmPrincipal frm = new frmPrincipal();

                    frm.insert_boletin_criacao(bt, btp, obj_familia);
                }
                else
                {
                    if (obj_familia != null)
                    {
                        theSession.Parts.SetDisplay(obj_familia, true, true, out LdSt);

                        theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");
                    }
                }
                frmPrincipal.deletar_tabela_fam("REVISAO_ITENS", obj_familia);

                int j;
                Tag[] families;
                UFFam.FamilyData family_data;
                string atb;
                int[] att_idx = new int[Atributo.Length];


                theUfSession.Part.AskFamilies(obj_familia.Tag, out j, out families);
                theUfSession.Fam.AskFamilyData(families[0], out family_data);
                Tag[] attr = family_data.attributes;
                for (j = 0; j < attr.Length; j++)
                {
                    theUfSession.Obj.AskName(attr[j], out atb);
                    for (int i = 0; i < Atributo.Length; i++)
                        if (Atributo[i] == atb)
                        {
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_mp) " + Atributo[i] + " Index: " + j.ToString());
                            att_idx[i] = j;
                        }
                }

                Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_mp) Procurando por Membros com os Atributos:");
                for (j = 0; j < Atributo.Length; j++)
                {
                    Registrar_no_LOG("                                        -> " + Atributo[j] + " = " + Math.Round(Valor[j], 1));
                    Registrar_no_LOG("                               Tolerancia-> = " + Tolerancia[j]);
                }


                /*
                 * Localizar o Membro cujos atributos são iguais aos recebidos
                 * como parametro.
                 */

                theUfSession.Part.AskFamilies(obj_familia.Tag, out j, out families);
                theUfSession.Fam.AskFamilyData(families[0], out family_data);
                Tag[] attributos = family_data.attributes;
                //theUI.NXMessageBox.Show("UI Styler", NXMessageBox.DialogType.Error, "111");

                Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_mp) attributos.Length:" + attributos.Length.ToString());

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

                            //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) member_data.values[0]: " + member_data.values[0]);
                            //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) aux_valor_1: " + aux_valor_1);
                            //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) aux_valor_2: " + aux_valor_2);
                            //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) resultado..: " + resultado + " Tol: " + Tolerancia[j]);
                            if (member_idx == 0)
                                // Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Tolerancia.: " + Tolerancia[j]);

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
                    existe = true;
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
                            item_id = partbuilder.AssignPartNumber(item_type);
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_mp) Assigned item_id:" + item_id);
                            flg = false;
                            if (item_id == "" || item_id == "(null)")
                            {
                                flg = true;
                            }
                        }
                        catch
                        {
                            flg = true;
                        }
                    }
                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_mp) Assigned item_id:" + item_id);

                    new_member_data.values[0] = item_id;
                    new_member_data.values[1] = descricao;



                    new_member_data.values[3] = item_type;
                    new_member_data.values[4] = "000";
                   // new_member_data.values[Atributo.Length + 4] = mp;
                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_mp) material:" + mp);

                    string DB_PART_NAME = Inicio_DB_PART_NAME;

                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_mp) DB_PART_NAME:" + DB_PART_NAME);

                    if (Inicio_DB_PART_NAME != "")
                        for (j = 0; j < Nr_Attr_Concatenar_Nome; j++)
                            DB_PART_NAME += Math.Round(Valor[j], 1).ToString().Replace(",", ".");



                    if (DB_PART_NAME != "")
                        new_member_data.values[2] = descricao;


                    string linha = string.Format(new_member_data.values[0]).PadRight(10, ' ') + "|" +
                                   string.Format("000").PadRight(13, ' ') + "|" +

                                   string.Format(DB_PART_NAME);

                    // lw.WriteLine(linha);



                    for (int k = 0; k < Atributo.Length; k++)
                    {
                        string aux_valor = Math.Round(Valor[k], 1).ToString("#.#").Replace(',', '.');

                        if (aux_valor == "") aux_valor = "0";

                        new_member_data.values[att_idx[k]] = aux_valor;
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_mp):" + att_idx[k] + "  valor:" + aux_valor);
                    }
                    new_member_data.values[Atributo.Length + 5] = mp;

                    foreach (string dataColumn in new_member_data.values)
                    {
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_mp):  valor:" + dataColumn);
                    }
                    
                    member_index = family_data.member_count;



                    theUfSession.Fam.AddMember(families[0], ref new_member_data, out member_index);

                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_mp):" + new_member_data.values);
                    theUfSession.Part.CreateFamilyInstance(families[0], member_index, out part_tag, out inst_tag);

                    try
                    {
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + families[0]);
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + families[0]);
                        theUfSession.Part.UpdateFamilyInstance(families[0], member_index, true, out part_tag, out saved, out count, out part_list, out erro_list, out info);
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + "1");
                    }
                    catch
                    {
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + "2");
                    }
                
                    theUfSession.Modl.Update();
                
                    Part new_item;
                  
                    theSession = Session.GetSession();
                   
                    theUI = UI.GetUI();
      
                    theUfSession = UFSession.GetUFSession();
                   

                    string ja_criou = TCSearchItem(item_id);
                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + "9");
                    if (ja_criou != "")
                    {
                        BasePart obj_membro = (BasePart)NXOpen.Utilities.NXObjectManager.Get(part_tag);

                        membro = obj_membro.FullPath;
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Membro criado:" + Convert.ToString(item_id));
                        frmPrincipal.Registrar_no_LOG("FAM:" + fam + "  item:" + item_id);
                        atualizacao_fam_2312.update_create_item_fam(fam, item_id, obj_familia);
                        if (boletim == true)
                        {
                            frmPrincipal frm = new frmPrincipal();

                            frm.deletar_tabelas(obj_familia);
                        }
                        obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
                    }
                    else
                    {
                        theUfSession.Fam.AddMember(families[0], ref new_member_data, out member_index);
                        theUfSession.Part.CreateFamilyInstance(families[0], member_index, out part_tag, out inst_tag);
                        theUfSession.Part.UpdateFamilyInstance(families[0], member_index, true, out part_tag, out saved, out count, out part_list, out erro_list, out info);
                        theUfSession.Modl.Update();

                        BasePart obj_membro = (BasePart)NXOpen.Utilities.NXObjectManager.Get(part_tag);
                        membro = obj_membro.FullPath;

                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Membro criado:" + Convert.ToString(item_id));
                        frmPrincipal.Registrar_no_LOG("FAM:" + fam + "  item:" + item_id);
                        atualizacao_fam_2312.update_create_item_fam(fam, item_id, obj_familia);

                        if (boletim == true)
                        {

                            frmPrincipal frm = new frmPrincipal();

                            frm.deletar_tabelas(obj_familia);
                        }
                        obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
                    }


                }

                flg1 = false;
                //}
                //catch
                //{
                //    flg1 = true;
                //}
            }
            return (membro, existe);
        }
        public static (string, bool) Localizar_Membro_da_Familia_cv(string Familia, string Inicio_DB_PART_NAME, string[] Atributo, double[] Valor, int Nr_Attr_Concatenar_Nome, double[] Tolerancia, string Cod_Atual, string descricao, string fam, string item_type, bool boletim, List<string> bt, List<string> btp)
        {

            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_cv) Metodo Item CV");
            bool flg1 = true;
            string membro = "";
            bool existe = false;
            while (flg1)
            {

                try
                {
                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Abrindo Familia:" + Familia);
                    PartLoadStatus LdSt;
                    Part obj_familia;
                    Part obj_atual;
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


                    theUfSession.Part.AskFamilies(obj_familia.Tag, out j, out families);
                    theUfSession.Fam.AskFamilyData(families[0], out family_data);
                    Tag[] attr = family_data.attributes;
                    for (j = 0; j < attr.Length; j++)
                    {
                        theUfSession.Obj.AskName(attr[j], out atb);
                        for (int i = 0; i < Atributo.Length; i++)
                            if (Atributo[i] == atb)
                            {
                               // Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) " + Atributo[i] + " Index: " + j.ToString());
                                att_idx[i] = j;
                            }
                    }


                    /*
                     * Localizar o Membro cujos atributos são iguais aos recebidos
                     * como parametro.
                     */
                   // Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) TESTANDO");
                    theUfSession.Part.AskFamilies(obj_familia.Tag, out j, out families);
                    theUfSession.Fam.AskFamilyData(families[0], out family_data);
                    Tag[] attributos = family_data.attributes;
                    //theUI.NXMessageBox.Show("UI Styler", NXMessageBox.DialogType.Error, "111");

                  //  Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) attributos.Length:" + attributos.Length.ToString());

                    /*
                     * Verifico se o Membro já existe na familia
                     */

                    bool[] verificacao_membros = new bool[Atributo.Length];
                    string[] aux_valor_na_familia = new string[Atributo.Length];
                    bool Membro_Ja_Existe = false;
                    for (int member_idx = 0; member_idx < family_data.member_count; member_idx++)
                    {
                        if (!Membro_Ja_Existe)
                        {
                            UFFam.MemberData member_data;
                            theUfSession.Fam.AskMemberRowData(families[0], member_idx, out member_data);

                            for (j = 0; j < Atributo.Length; j++)
                            {
                                double aux_valor_1 = Convert.ToDouble(member_data.values[att_idx[j]].Replace(".", ","));
                                double aux_valor_2 = Math.Round(Valor[j], 1);
                                double resultado = Math.Abs(aux_valor_1 - aux_valor_2);

                                //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) member_data.values[0]: " + member_data.values[0]);
                                //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) aux_valor_1: " + aux_valor_1);
                                //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) aux_valor_2: " + aux_valor_2);
                                //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) resultado..: " + resultado + " Tol: " + Tolerancia[j]);
                                //if (member_idx == 0)
                                //{
                                   

                                    if (resultado <= Tolerancia[j])
                                    {
                                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) resultado..: " + member_data.values[0] + " serve");
                                    verificacao_membros[j] = true;
                                        aux_valor_na_familia[j] = member_data.values[att_idx[j]];
                                        membro = member_data.values[0];
                                    }
                                    else
                                    {
                                        verificacao_membros[j] = false;
                                    }
                               // }
                            }
                            Membro_Ja_Existe = true;
                            foreach (bool aux_membro in verificacao_membros)
                                if (!aux_membro)
                                {
                                    Membro_Ja_Existe = false;
                                    break;
                                }
                        }
                    }

                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) membro existe?: "+ Membro_Ja_Existe.ToString());
                    /*
                    * DEBUG
                    */
                    if (Membro_Ja_Existe)
                    {
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Valor Encontrado:");
                        for (int i = 0; i < Atributo.Length; i++)
                            Registrar_no_LOG("                                        -> " + Atributo[i] + ": " + aux_valor_na_familia[i]);
                        Registrar_no_LOG("                                        -> " + membro);
                        existe = true;
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
                                item_id = partbuilder.AssignPartNumber(item_type);

                                flg = false;
                                if (item_id == "" || item_id == "(null)")
                                {
                                    flg = true;
                                }
                            }
                            catch
                            {
                                flg = true;
                            }
                        }
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Assigned item_id:" + item_id);

                        new_member_data.values[0] = item_id;
                        new_member_data.values[1] = descricao;



                        new_member_data.values[3] = item_type;
                        new_member_data.values[4] = "000";

                        string DB_PART_NAME = Inicio_DB_PART_NAME;

                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) DB_PART_NAME:" + DB_PART_NAME);

                        if (Inicio_DB_PART_NAME != "")
                            for (j = 0; j < Nr_Attr_Concatenar_Nome; j++)
                                DB_PART_NAME += Math.Round(Valor[j], 1).ToString().Replace(",", ".");



                        if (DB_PART_NAME != "")
                            new_member_data.values[2] = descricao;


                        string linha = string.Format(new_member_data.values[0]).PadRight(10, ' ') + "|" +
                                       string.Format("000").PadRight(13, ' ') + "|" +

                                       string.Format(DB_PART_NAME);

                        // lw.WriteLine(linha);



                        for (int k = 0; k < Atributo.Length; k++)
                        {
                            string aux_valor = Math.Round(Valor[k], 1).ToString("#.#").Replace(',', '.');

                            if (aux_valor == "") aux_valor = "0";

                            new_member_data.values[att_idx[k]] = aux_valor;
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + att_idx[k] + "  valor:" + aux_valor);
                        }


                        member_index = family_data.member_count;



                        theUfSession.Fam.AddMember(families[0], ref new_member_data, out member_index);

                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro__CV):" + new_member_data.values);
                        theUfSession.Part.CreateFamilyInstance(families[0], member_index, out part_tag, out inst_tag);

                        try
                        {
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + families[0]);
                           
                            theUfSession.Part.UpdateFamilyInstance(families[0], member_index, true, out part_tag, out saved, out count, out part_list, out erro_list, out info);

                        }
                        catch
                        {
                           
                        }
                       
                        theUfSession.Modl.Update();
                      
                        Part new_item;
                       
                        theSession = Session.GetSession();
                      
                        theUI = UI.GetUI();
                       
                        theUfSession = UFSession.GetUFSession();
                        

                        string ja_criou = TCSearchItem(item_id);
                      
                        if (ja_criou != "")
                        {
                           
                            BasePart obj_membro = (BasePart)NXOpen.Utilities.NXObjectManager.Get(part_tag);
                          
                            membro = obj_membro.FullPath;
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Membro criado:" + Convert.ToString(item_id));

                            obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
                        }
                        else
                        {
                            theUfSession.Fam.AddMember(families[0], ref new_member_data, out member_index);
                            theUfSession.Part.CreateFamilyInstance(families[0], member_index, out part_tag, out inst_tag);
                            theUfSession.Part.UpdateFamilyInstance(families[0], member_index, true, out part_tag, out saved, out count, out part_list, out erro_list, out info);
                            theUfSession.Modl.Update();

                            BasePart obj_membro = (BasePart)NXOpen.Utilities.NXObjectManager.Get(part_tag);
                            membro = obj_membro.FullPath;
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
            return (membro, existe);
        }
        public static (string, bool) Localizar_Membro_da_Familia_new_automatico_demanda(string Familia, string Inicio_DB_PART_NAME, string[] Atributo, double[] Valor, int Nr_Attr_Concatenar_Nome, double[] Tolerancia, string Cod_Atual, string descricao, string fam, string item_type, string codigo, bool boletim, List<string> bt, List<string> btp)
        {


            bool flg1 = true;
            string membro = "";
            bool existe = false;
            while (flg1)
            {
                Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) boletim:" + boletim.ToString());
                try
                {
                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Abrindo Familia:" + Familia);
                    PartLoadStatus LdSt;
                    Part obj_familia;
                    Part obj_atual;
                    try
                    {
                        obj_familia = (Part)theSession.Parts.FindObject(Familia);
                    }
                    catch
                    {
                        obj_familia = theSession.Parts.Open(Familia, out LdSt);
                    }
                    if (boletim == true)
                    {

                        if (obj_familia != null)
                        {
                            theSession.Parts.SetDisplay(obj_familia, true, true, out LdSt);

                            theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");
                        }

                        frmPrincipal frm = new frmPrincipal();

                        frm.insert_boletin_criacao(bt, btp, obj_familia);

                    }

                  

                    int j;
                    Tag[] families;
                    UFFam.FamilyData family_data;
                    string atb;
                    int[] att_idx = new int[Atributo.Length];


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
                        existe = true;
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
                        if (codigo != "0")
                        {
                            item_id = codigo;
                        }
                        else
                        {
                            while (flg) // inicio LOOPING 
                            {
                                try
                                {
                                    System.Threading.Thread.Sleep(3000); // temporizador looping para não ficar usando o processador, tempo 3 segundos
                                    partbuilder = theSession.Parts.PDMPartManager.NewPartFromPartBuilder();
                                    item_id = partbuilder.AssignPartNumber(item_type);

                                    flg = false;
                                    if (item_id == "" || item_id == "(null)")
                                    {
                                        flg = true;
                                    }
                                }
                                catch
                                {
                                    flg = true;
                                }
                            }
                        }


                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Assigned item_id:" + item_id);

                        new_member_data.values[0] = item_id;
                        new_member_data.values[1] = descricao;



                        new_member_data.values[3] = item_type;
                        new_member_data.values[4] = "000";

                        string DB_PART_NAME = Inicio_DB_PART_NAME;

                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) DB_PART_NAME:" + DB_PART_NAME);

                        if (Inicio_DB_PART_NAME != "")
                            for (j = 0; j < Nr_Attr_Concatenar_Nome; j++)
                                DB_PART_NAME += Math.Round(Valor[j], 1).ToString().Replace(",", ".");



                        if (DB_PART_NAME != "")
                            new_member_data.values[2] = descricao;


                        string linha = string.Format(new_member_data.values[0]).PadRight(10, ' ') + "|" +
                                       string.Format("000").PadRight(13, ' ') + "|" +

                                       string.Format(DB_PART_NAME);

                        // lw.WriteLine(linha);



                        for (int k = 0; k < Atributo.Length; k++)
                        {
                            string aux_valor = Math.Round(Valor[k], 1).ToString("#.#").Replace(',', '.');

                            if (aux_valor == "") aux_valor = "0";

                            new_member_data.values[att_idx[k]] = aux_valor;
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + att_idx[k] + "  valor:" + aux_valor);
                        }


                        member_index = family_data.member_count;



                        theUfSession.Fam.AddMember(families[0], ref new_member_data, out member_index);

                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + new_member_data.values);
                        theUfSession.Part.CreateFamilyInstance(families[0], member_index, out part_tag, out inst_tag);

                        try
                        {
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + families[0]);
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + families[0]);
                            theUfSession.Part.UpdateFamilyInstance(families[0], member_index, true, out part_tag, out saved, out count, out part_list, out erro_list, out info);
                        }
                        catch
                        {
                        }

                        theUfSession.Modl.Update();
                        Part new_item;

                        theSession = Session.GetSession();
                        theUI = UI.GetUI();
                        theUfSession = UFSession.GetUFSession();

                        string ja_criou = TCSearchItem(item_id);

                        if (ja_criou != "")
                        {
                            BasePart obj_membro = (BasePart)NXOpen.Utilities.NXObjectManager.Get(part_tag);
                            membro = obj_membro.FullPath;
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Membro criado:" + Convert.ToString(item_id));
                            frmPrincipal.Registrar_no_LOG("FAM:" + fam + "  item:" + item_id);
                            atualizacao_fam_2312.update_create_item_fam(fam, item_id, obj_familia);
                            if (boletim == true)
                            {
                                frmPrincipal frm = new frmPrincipal();

                                frm.deletar_tabelas(obj_familia);
                            }
                           // obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
                        }
                        else
                        {
                            theUfSession.Fam.AddMember(families[0], ref new_member_data, out member_index);
                            theUfSession.Part.CreateFamilyInstance(families[0], member_index, out part_tag, out inst_tag);
                            theUfSession.Part.UpdateFamilyInstance(families[0], member_index, true, out part_tag, out saved, out count, out part_list, out erro_list, out info);
                            theUfSession.Modl.Update();

                            BasePart obj_membro = (BasePart)NXOpen.Utilities.NXObjectManager.Get(part_tag);
                            membro = obj_membro.FullPath;

                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Membro criado:" + Convert.ToString(item_id));
                            frmPrincipal.Registrar_no_LOG("FAM:" + fam + "  item:" + item_id);
                            atualizacao_fam_2312.update_create_item_fam(fam, item_id, obj_familia);
                           // obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
                            if (boletim == true)
                            {

                                frmPrincipal frm = new frmPrincipal();

                                frm.deletar_tabelas(obj_familia);
                            }
                          
                        }


                    }
                      obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.False);
                    flg1 = false;
                }
                catch
                {
                    flg1 = true;
                }
                  
            }
           
            return (membro, existe);
        }
        public static (string, bool) Localizar_Membro_da_Familia_new_automatico(string Familia, string Inicio_DB_PART_NAME, string[] Atributo, double[] Valor, int Nr_Attr_Concatenar_Nome, double[] Tolerancia, string Cod_Atual, string descricao, string fam, string item_type, string codigo, bool boletim, List<string> bt, List<string> btp)
        {

           
            bool flg1 = true;
            string membro = "";
            bool existe = false;
            while (flg1)
            {
                Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) boletim:" + boletim.ToString());
                try
                {
                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Abrindo Familia:" + Familia);
                    PartLoadStatus LdSt;
                    Part obj_familia;
                    Part obj_atual;
                    try
                    {
                        obj_familia = (Part)theSession.Parts.FindObject(Familia);
                    }
                    catch
                    {
                        obj_familia = theSession.Parts.Open(Familia, out LdSt);
                    }
                    if (boletim == true)
                    {

                        if (obj_familia != null)
                        {
                            theSession.Parts.SetDisplay(obj_familia, true, true, out LdSt);

                            theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");
                        }

                        frmPrincipal frm = new frmPrincipal();

                        frm.insert_boletin_criacao(bt, btp, obj_familia);
                       
                    }
                    //else
                    //{
                    //    if (obj_familia != null)
                    //    {
                    //        theSession.Parts.SetDisplay(obj_familia, true, true, out LdSt);

                    //        theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");
                    //    }
                    //}
                    int j;
                    Tag[] families;
                    UFFam.FamilyData family_data;
                    string atb;
                    int[] att_idx = new int[Atributo.Length];


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
                        existe = true;
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
                        if (codigo != "0")
                        {
                            item_id = codigo;
                        }
                        else
                        {
                            while (flg) // inicio LOOPING 
                            {
                                try
                                {
                                    System.Threading.Thread.Sleep(3000); // temporizador looping para não ficar usando o processador, tempo 3 segundos
                                    partbuilder = theSession.Parts.PDMPartManager.NewPartFromPartBuilder();
                                    item_id = partbuilder.AssignPartNumber(item_type);

                                    flg = false;
                                    if (item_id == "" || item_id == "(null)")
                                    {
                                        flg = true;
                                    }
                                }
                                catch
                                {
                                    flg = true;
                                }
                            }
                        }


                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Assigned item_id:" + item_id);

                        new_member_data.values[0] = item_id;
                        new_member_data.values[1] = descricao;



                        new_member_data.values[3] = item_type;
                        new_member_data.values[4] = "000";

                        string DB_PART_NAME = Inicio_DB_PART_NAME;

                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) DB_PART_NAME:" + DB_PART_NAME);

                        if (Inicio_DB_PART_NAME != "")
                            for (j = 0; j < Nr_Attr_Concatenar_Nome; j++)
                                DB_PART_NAME += Math.Round(Valor[j], 1).ToString().Replace(",", ".");



                        if (DB_PART_NAME != "")
                            new_member_data.values[2] = descricao;


                        string linha = string.Format(new_member_data.values[0]).PadRight(10, ' ') + "|" +
                                       string.Format("000").PadRight(13, ' ') + "|" +

                                       string.Format(DB_PART_NAME);
                       
                        // lw.WriteLine(linha);



                        for (int k = 0; k < Atributo.Length; k++)
                        {
                            string aux_valor = Math.Round(Valor[k], 1).ToString("#.#").Replace(',', '.');

                            if (aux_valor == "") aux_valor = "0";

                            new_member_data.values[att_idx[k]] = aux_valor;
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + att_idx[k] +"  valor:"+ aux_valor);
                        }


                        member_index = family_data.member_count;

                      

                        theUfSession.Fam.AddMember(families[0], ref new_member_data, out member_index);

                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + new_member_data.values);
                        theUfSession.Part.CreateFamilyInstance(families[0], member_index, out part_tag, out inst_tag);
                        
                        try
                        {
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + families[0]);
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia):" + families[0]);
                            theUfSession.Part.UpdateFamilyInstance(families[0], member_index, true, out part_tag, out saved, out count, out part_list, out erro_list, out info);
                        }
                        catch
                        {
                        }

                        theUfSession.Modl.Update();
                        Part new_item;

                        theSession = Session.GetSession();
                        theUI = UI.GetUI();
                        theUfSession = UFSession.GetUFSession();

                        string ja_criou = TCSearchItem(item_id);

                        if (ja_criou != "")
                        {
                            BasePart obj_membro = (BasePart)NXOpen.Utilities.NXObjectManager.Get(part_tag);
                            membro = obj_membro.FullPath;
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Membro criado:" + Convert.ToString(item_id));
                            frmPrincipal.Registrar_no_LOG("FAM:" + fam + "  item:" + item_id);
                            atualizacao_fam_2312.update_create_item_fam(fam, item_id, obj_familia);
                            if (boletim == true)
                            {
                                frmPrincipal frm = new frmPrincipal();

                                frm.deletar_tabelas(obj_familia);
                            }
                            obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
                        }
                        else
                        {
                            theUfSession.Fam.AddMember(families[0], ref new_member_data, out member_index);
                            theUfSession.Part.CreateFamilyInstance(families[0], member_index, out part_tag, out inst_tag);
                            theUfSession.Part.UpdateFamilyInstance(families[0], member_index, true, out part_tag, out saved, out count, out part_list, out erro_list, out info);
                            theUfSession.Modl.Update();

                            BasePart obj_membro = (BasePart)NXOpen.Utilities.NXObjectManager.Get(part_tag);
                            membro = obj_membro.FullPath;

                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Membro criado:" + Convert.ToString(item_id));
                            frmPrincipal.Registrar_no_LOG("FAM:" + fam + "  item:" + item_id);
                            atualizacao_fam_2312.update_create_item_fam(fam, item_id, obj_familia);
                            obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
                            if (boletim == true)
                            {

                                frmPrincipal frm = new frmPrincipal();

                                frm.deletar_tabelas(obj_familia);
                            }
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
            return (membro, existe);
        }
        public static (string, bool) Localizar_Membro_da_Familia_new_material(string Familia, string Inicio_DB_PART_NAME, string[] Atributo, double[] Valor, int Nr_Attr_Concatenar_Nome, double[] Tolerancia, string Cod_Atual, string descricao, string fam, string item_type, string material)
        {
            bool flg1 = true;
            string membro = "";
            bool existe = false;
            while (flg1)
            {

                try
                {
                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) Abrindo Familia:" + Familia);
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
                    //if(obj_familia.IsReadOnly == false)
                    //{
                    //     theUI.NXMessageBox.Show("UI Styler", NXMessageBox.DialogType.Error, obj_familia.IsReadOnly.ToString());
                    //}

                    theUfSession.Part.AskFamilies(obj_familia.Tag, out j, out families);
                    theUfSession.Fam.AskFamilyData(families[0], out family_data);
                    Tag[] attr = family_data.attributes;
                    for (j = 0; j < attr.Length; j++)
                    {
                        theUfSession.Obj.AskName(attr[j], out atb);
                        for (int i = 0; i < Atributo.Length; i++)
                            if (Atributo[i] == atb)
                            {
                                Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) " + Atributo[i] + " Index: " + j.ToString());
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
                  
                    theUfSession.Part.AskFamilies(obj_familia.Tag, out j, out families);
                    theUfSession.Fam.AskFamilyData(families[0], out family_data);
                    Tag[] attributos = family_data.attributes;
                    //theUI.NXMessageBox.Show("UI Styler", NXMessageBox.DialogType.Error, "111");

                    Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) attributos.Length:" + attributos.Length.ToString());

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

                                //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) member_data.values[0]: " + member_data.values[0]);
                                //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) aux_valor_1: " + aux_valor_1);
                                //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) aux_valor_2: " + aux_valor_2);
                                //Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) resultado..: " + resultado + " Tol: " + Tolerancia[j]);
                                if (member_idx == 0)
                                  //  Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) Tolerancia.: " + Tolerancia[j]);

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
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) Valor Encontrado:");
                        for (int i = 0; i < Atributo.Length; i++)
                            Registrar_no_LOG("                                        -> " + Atributo[i] + ": " + aux_valor_na_familia[i]);
                        Registrar_no_LOG("                                        -> " + membro);
                        existe = true;
                    }
                    if (!Membro_Ja_Existe)
                    {
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) Criando Novo Membro da Familia.");
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
                                item_id = partbuilder.AssignPartNumber(item_type);

                                flg = false;
                                if (item_id == "" || item_id == "(null)")
                                {
                                    flg = true;
                                }
                            }
                            catch
                            {
                                flg = true;
                            }
                        }
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) Assigned item_id:" + item_id);
                        //System.Windows.Forms.MessageBox.Show(item_id);
                        new_member_data.values[0] = item_id;
                        new_member_data.values[1] = descricao;



                        new_member_data.values[3] = item_type;
                        new_member_data.values[4] = "000";
                        new_member_data.values[Atributo.Length + 5] = material;

                        string DB_PART_NAME = Inicio_DB_PART_NAME;
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) Material:" + material);
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) DB_PART_NAME:" + DB_PART_NAME);

                        //if (Inicio_DB_PART_NAME != "")
                        //    for (j = 0; j < Nr_Attr_Concatenar_Nome; j++)
                        //        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) " + DB_PART_NAME);
                        //        DB_PART_NAME += Math.Round(Valor[j], 1).ToString().Replace(",", ".");



                        //if (DB_PART_NAME != "")
                        new_member_data.values[2] = descricao;


                        string linha = string.Format(new_member_data.values[0]).PadRight(10, ' ') + "|" +
                                       string.Format("000").PadRight(13, ' ') + "|" +

                                       string.Format(DB_PART_NAME);

                        // lw.WriteLine(linha);

                        //System.Windows.Forms.MessageBox.Show("3333");

                        for (int k = 0; k < Atributo.Length; k++)
                        {
                            string aux_valor = Math.Round(Valor[k], 1).ToString("#.#").Replace(',', '.');

                            if (aux_valor == "") aux_valor = "0";

                            new_member_data.values[att_idx[k]] = aux_valor;

                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) new_member_data:" + att_idx[k] + " valor=" + aux_valor);

                        }


                        member_index = family_data.member_count;

                        theUfSession.Fam.AddMember(families[0], ref new_member_data, out member_index);

                        theUfSession.Part.CreateFamilyInstance(families[0], member_index, out part_tag, out inst_tag);
                      //  Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) new_member_data:CreateFamilyInstance");

                        theUfSession.Part.UpdateFamilyInstance(families[0], member_index, true, out part_tag, out saved, out count, out part_list, out erro_list, out info);

                        theUfSession.Modl.Update();
                        Part new_item;

                        theSession = Session.GetSession();
                        theUI = UI.GetUI();
                        theUfSession = UFSession.GetUFSession();


                        string ja_criou = TCSearchItem(item_id);
                        // System.Windows.Forms.MessageBox.Show(existe);

                        if (ja_criou != "")
                        {
                           

                            BasePart obj_membro = (BasePart)NXOpen.Utilities.NXObjectManager.Get(part_tag);
                            membro = obj_membro.FullPath;
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) Membro criado:" + Convert.ToString(item_id));
                            frmPrincipal.Registrar_no_LOG("FAM:" + fam + "  item:" + item_id);
                            atualizacao_fam_2312.update_create_item_fam(fam, item_id, obj_familia);
                            obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
                        }
                        else
                        {
                           

                            theUfSession.Fam.AddMember(families[0], ref new_member_data, out member_index);
                            theUfSession.Part.CreateFamilyInstance(families[0], member_index, out part_tag, out inst_tag);
                            theUfSession.Part.UpdateFamilyInstance(families[0], member_index, true, out part_tag, out saved, out count, out part_list, out erro_list, out info);
                            theUfSession.Modl.Update();

                            BasePart obj_membro = (BasePart)NXOpen.Utilities.NXObjectManager.Get(part_tag);
                            membro = obj_membro.FullPath;

                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia_new_material) Membro criado:" + Convert.ToString(item_id));
                            frmPrincipal.Registrar_no_LOG("FAM:" + fam + "  item:" + item_id);
                            atualizacao_fam_2312.update_create_item_fam(fam, item_id, obj_familia);
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
            return (membro, existe);
        }
      
        public static string Localizar_Membro_da_Familia(string Familia, string Inicio_DB_PART_NAME, string[] Atributo, double[] Valor, int Nr_Attr_Concatenar_Nome, double[] Tolerancia, string Cod_Atual, string descricao, string fam)
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
                    //if(obj_familia.IsReadOnly == false)
                    //{
                    //     theUI.NXMessageBox.Show("UI Styler", NXMessageBox.DialogType.Error, obj_familia.IsReadOnly.ToString());
                    //}
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
                                item_id = partbuilder.AssignPartNumber("M4_it_mascarello");

                                flg = false;
                                if (item_id == "" || item_id == "(null)")
                                {
                                    flg = true;
                                }
                            }
                            catch
                            {
                                flg = true;
                            }
                        }
                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Assigned item_id:" + item_id);
                        //System.Windows.Forms.MessageBox.Show(item_id);
                        new_member_data.values[0] = item_id;
                        new_member_data.values[1] = descricao;



                        new_member_data.values[3] = "M4_it_mascarello";
                        new_member_data.values[4] = "000";



                        string DB_PART_NAME = Inicio_DB_PART_NAME;

                        Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) DB_PART_NAME:" + DB_PART_NAME);

                        if (Inicio_DB_PART_NAME != "")
                            for (j = 0; j < Nr_Attr_Concatenar_Nome; j++)
                                DB_PART_NAME += Math.Round(Valor[j], 1).ToString().Replace(",", ".");



                        if (DB_PART_NAME != "")
                            new_member_data.values[2] = descricao;


                        string linha = string.Format(new_member_data.values[0]).PadRight(10, ' ') + "|" +
                                       string.Format("000").PadRight(13, ' ') + "|" +

                                       string.Format(DB_PART_NAME);

                        // lw.WriteLine(linha);

                        //System.Windows.Forms.MessageBox.Show("3333");

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


                        theSession = Session.GetSession();
                        theUI = UI.GetUI();
                        theUfSession = UFSession.GetUFSession();


                        string ja_criou = TCSearchItem(item_id);


                        if (ja_criou != "")
                        {
                            BasePart obj_membro = (BasePart)NXOpen.Utilities.NXObjectManager.Get(part_tag);
                            membro = obj_membro.FullPath;
                            Registrar_no_LOG(" DEBUG -> (Localizar_Membro_da_Familia) Membro criado:" + Convert.ToString(item_id));
                            frmPrincipal.Registrar_no_LOG("FAM:"+fam + "  item:" + item_id);
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
        public static string gerar_rev_item_fam(string Familia, string[] item)
        {
            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theUfSession = UFSession.GetUFSession();

            string encoded_familia = TCSearchItem(Familia);
            PartLoadStatus LdSt;
            Part obj_familia;




            try
            {
                obj_familia = (Part)theSession.Parts.FindObject(encoded_familia);
            }
            catch
            {
                obj_familia = theSession.Parts.Open(encoded_familia, out LdSt);
            }

            // frmPrincipal.Insertb_alt(obj_familia);
            int j;
            Tag[] families;
            UFFam.FamilyData family_data;
            string atb;

            theUfSession.Part.AskFamilies(obj_familia.Tag, out j, out families);
            theUfSession.Fam.AskFamilyData(families[0], out family_data);

            //  // Obter dados da família
            UFFam.FamilyData familyData;
            theUfSession.Fam.AskFamilyData(families[0], out familyData);

            //  // Localizar o membro pelo nome
            int memberIndex = -1;
            for (int i = 0; i < familyData.member_count; i++)
            {
                UFFam.MemberData memberData;
                theUfSession.Fam.AskMemberRowData(families[0], i, out memberData);

                if (memberData.values[0] == item[0])
                {
                    memberIndex = i;
                    break;
                }
            }

            if (memberIndex == -1)
            {
                 Registrar_no_LOG($"Membro '{item[0]}' não encontrado na família.");
                //hrow new Exception($"Membro '{item[0]}' não encontrado na família.");
            }

            //  // Revisar o membro
            //  Registrar_no_LOG($"Atualizando Revisão do Membro: {item[0]}");
            //  UFFam.MemberData existingMemberData;
            //  theUfSession.Fam.AskMemberRowData(families[0], memberIndex, out existingMemberData);

            //  // Modificar o valor da revisão no campo apropriado (ex.: índice 1 para revisão)
            //  for (int i = 0; i < existingMemberData.value_count-1; i++)
            //  {
            //      existingMemberData.values[i] = item[i];
            //  }
            ////  existingMemberData.values[4] = item[4];

            //  // Atualizar o membro na tabela de famílias
            //  theUfSession.Fam.EditMember(families[0], memberIndex, ref existingMemberData);

            //  // Atualizar a instância do part
            Tag partTag, instTag;
            bool saved;
            theUfSession.Part.CreateFamilyInstance(families[0], memberIndex, out partTag, out instTag);
            // theUfSession.Part(families[0], memberIndex, true, out partTag, out saved, out int count, out Tag[] partList, out int[] errorList, out string info);

            //  // Salvar alterações
            //  BasePart objFamiliaPart = (BasePart)NXOpen.Utilities.NXObjectManager.Get(partTag);
            //  objFamiliaPart.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);

           // atualizacao_fam_2312.update_create_item_fam(Familia, item[0], obj_familia);

            //  obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);

            //  //atualizacao_fam_2312.gerar_pdf_criacao_familia(item[0], item[4]);

            //  Registrar_no_LOG($"Revisão '{item[4]}' criada para o membro '{item[0]}'.");
            //  return item[0];
            //}
            //catch (Exception ex)
            //{
            //    Registrar_no_LOG($"Erro: {ex.Message}");
            return null;
            //}
        }
    }
}
