using NXOpen.UF;
using NXOpen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen.CAE;
using System.Windows.Documents;
using System.Windows;
using NXOpen.Annotations;
using PLMComponents.Parasolid.PK_.Unsafe;
using NXOpen.Die;
using System.Security.Cryptography;
using static System.Net.WebRequestMethods;

namespace Custom_Mascarello
{
    public class atualizacao_fam_2312
    {
        public static void update_create_item_fam(string Familia, string Codigo, Part obj_familia)
        {

            //try
            //{
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            // Abrir a família
            frmPrincipal.Registrar_no_LOG($"Abrindo Família: {Familia}");
            PartLoadStatus loadStatus;
           

            // Obter a primeira tabela de famílias
            UFSession theUfSession = UFSession.GetUFSession();
            Tag[] families;
            theUfSession.Part.AskFamilies(obj_familia.Tag, out int numFamilies, out families);

            if (numFamilies == 0)
            {
              frmPrincipal.Registrar_no_LOG(Codigo + " - Nenhuma tabela de família encontrada.");
                throw new Exception("Nenhuma tabela de família encontrada.");
            }

            // Obter dados da família
            UFFam.FamilyData familyData;
            theUfSession.Fam.AskFamilyData(families[0], out familyData);

            // Localizar o membro pelo nome
            int memberIndex = -1;
            for (int i = 0; i < familyData.member_count; i++)
            {
                UFFam.MemberData memberData;
                theUfSession.Fam.AskMemberRowData(families[0], i, out memberData);

                if (memberData.values[0] == Codigo)
                {
                    memberIndex = i;
                    break;
                }
            }

            if (memberIndex == -1)
            {
                frmPrincipal.Registrar_no_LOG(Codigo + " não encontrado na família.");
                throw new Exception($"Membro '{Codigo}' não encontrado na família.");
            }
            
            // Revisar o membro
            frmPrincipal.Registrar_no_LOG($"Codigo filho encontrado: {Codigo}");
            UFFam.MemberData existingMemberData;
            theUfSession.Fam.AskMemberRowData(families[0], memberIndex, out existingMemberData);

            // Modificar o valor da revisão no campo apropriado (ex.: índice 1 para revisão)


            // Atualizar a instância do part
            Tag partTag, instTag;
            bool saved;
            theUfSession.Part.UpdateFamilyInstance(families[0], memberIndex, true, out partTag, out saved, out int count, out Tag[] partList, out int[] errorList, out string info);

            // Salvar alterações
            BasePart objFamiliaPart = (BasePart)NXOpen.Utilities.NXObjectManager.Get(partTag);
            objFamiliaPart.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);

            frmPrincipal.Registrar_no_LOG($"Update '{Familia}' para o membro '{Codigo}'.");
        }
        public static void gerar_pdf_criacao_familia(string codigo, string rev)
        {
            frmPrincipal.Registrar_no_LOG($"Gerar pdf {codigo} - {rev} ");
            string item = "@DB/" + codigo + "/" + rev;
            NXOpen.Session theSession = NXOpen.Session.GetSession();

            // ----------------------------------------------
            //   Menu: File->Close->Selected Parts...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            theSession.SetUndoMarkName(markId1, "Close Part Dialog");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Close Part");

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Close Part");

            theSession.DeleteUndoMark(markId3, null);

            theSession.SetUndoMarkName(markId1, "Close Part");
            try
            { // File already exists
                NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.FindObject(item));
                part1.Close(NXOpen.BasePart.CloseWholeTree.False, NXOpen.BasePart.CloseModified.CloseModified, null);
            }
            catch
            {
            }

            theSession.DeleteUndoMark(markId3, null);


            theSession.DeleteUndoMark(markId1, null);

            theSession.Parts.SetNonmasterSeedPartData(item);

            NXOpen.BasePart basePart1;
            NXOpen.PartLoadStatus partLoadStatus1;
            try
            {
                basePart1 = theSession.Parts.OpenActiveDisplay(item, NXOpen.DisplayPartOption.AllowAdditional, out partLoadStatus1);
                
                frmPrincipal.GerarPDF(codigo, rev);
                frmPrincipal.Verificar_Blank_Fam(codigo, rev);
            }
            catch
            {
                frmPrincipal.Registrar_no_LOG($"Errooo pdf {codigo} - {rev} ");
            }

            try
            { // File already exists
                NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.FindObject(item));
                part1.Close(NXOpen.BasePart.CloseWholeTree.False, NXOpen.BasePart.CloseModified.CloseModified, null);
            }
            catch
            {
            }

        }
        public static string verificar_formato()
        {

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            string formato = "";
            foreach (NXObject obj in workPart.Notes)
            {

                if (obj.GetType().Name == "Note")
                {
                    Note note = (Note)obj;
                    string[] text = note.GetText();
                    foreach (string item in text)
                    {
                        if (item == "A4")
                        {
                            formato = "A4";
                        }
                        if (item == "A3")
                        {
                            formato = "A3";
                        }

                        if (item == "A2")
                        {
                            formato = "A2";
                        }
                        if (item == "A1")
                        {
                            formato = "A1";
                        }
                        if (item == "A0")
                        {
                            formato = "A0";
                        }
                    }
                }

            }

            return formato;



        }
        public static void Insertb_bt()//(string codigo, string rev)
        {
            double pos_x = 0.0;
            double pos_y = 0.0;
            string format = verificar_formato();
            if (format == "A3")
            {
                pos_x = 414;
                pos_y = 276;
            }
            if (format == "A4")
            {
                pos_x = 204.00;
                pos_y = 276;
            }
            if (format == "A2")
            {
                pos_x = 588;
                pos_y = 399;
            }

            NXOpen.UF.UFSession ufSession = NXOpen.UF.UFSession.GetUFSession();
            double[] origem = { pos_x, pos_y, 0 };
            ufSession.Tabnot.CreateFromTemplate("@DB/Boletim_Tecnico_metric/A", origem, out NXOpen.Tag tabular_note);

        }

        public static void Insertb_bp()//(string codigo, string rev)
        {
            double pos_x = 0.0;
            double pos_y = 0.0;
            string format = verificar_formato();
            if (format == "A3")
            {
                pos_x = 338;
                pos_y = 276;
            }
            if (format == "A4")
            {
                pos_x = 128;
                pos_y = 276;
            }
            if (format == "A2")
            {
                pos_x = 512;
                pos_y = 399;
            }

            NXOpen.UF.UFSession ufSession = NXOpen.UF.UFSession.GetUFSession();
            double[] origem = { pos_x, pos_y, 0 };
            ufSession.Tabnot.CreateFromTemplate("@DB/Boletim_Pintura_metric/A", origem, out NXOpen.Tag tabular_note);

        }
        public static void update_item_fam(string Familia, string Codigo, Part obj_familia, string[] item, bool boletim)
        {

            //try
            //{
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            // Abrir a família
            frmPrincipal.Registrar_no_LOG($"Abrindo Família: {Familia}");
            PartLoadStatus LdSt;

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

               
                Insertb_bt();
                Insertb_bp();

            }
            // Obter a primeira tabela de famílias
            UFSession theUfSession = UFSession.GetUFSession();
            Tag[] families;
            theUfSession.Part.AskFamilies(obj_familia.Tag, out int numFamilies, out families);

            if (numFamilies == 0)
            {
                frmPrincipal.Registrar_no_LOG(Codigo + " - Nenhuma tabela de família encontrada.");
                throw new Exception("Nenhuma tabela de família encontrada.");
            }

          
            // Obter dados da família
            UFFam.FamilyData familyData;
            theUfSession.Fam.AskFamilyData(families[0], out familyData);

            // Localizar o membro pelo nome
            int memberIndex = -1;
            for (int i = 0; i < familyData.member_count; i++)
            {
                UFFam.MemberData memberData;
                theUfSession.Fam.AskMemberRowData(families[0], i, out memberData);

                if (memberData.values[0] == Codigo)
                {
                    memberIndex = i;
                    break;
                }
            }

            if (memberIndex == -1)
            {
                frmPrincipal.Registrar_no_LOG(Codigo + " não encontrado na família.");
                throw new Exception($"Membro '{Codigo}' não encontrado na família.");
            }

            // Revisar o membro
            frmPrincipal.Registrar_no_LOG($"Codigo filho encontrado: {Codigo}");
            UFFam.MemberData existingMemberData;
            theUfSession.Fam.AskMemberRowData(families[0], memberIndex, out existingMemberData);


            // Modificar o valor da revisão no campo apropriado (ex.: índice 1 para revisão)
            for (int i = 5; i < existingMemberData.values.Length; i++)
            {
                frmPrincipal.Registrar_no_LOG($"valor_antigo: {existingMemberData.values[i]}");
                existingMemberData.values[i] = item[i];
                frmPrincipal.Registrar_no_LOG($"valor novo: {existingMemberData.values[i]}");
            }

            existingMemberData.values[1] = item[1];
            existingMemberData.values[2] = item[2];
            theUfSession.Fam.EditMember(families[0], memberIndex, ref existingMemberData);
            string rev = item[4];
            frmPrincipal.Registrar_no_LOG($"rev: {item[4]}");

            // Atualizar a instância do part
            Tag partTag, instTag;
            bool saved;
            theUfSession.Part.UpdateFamilyInstance(families[0], memberIndex, true, out partTag, out saved, out int count, out Tag[] partList, out int[] errorList, out string info);

      
            frmPrincipal.deletar_tabela_fam("BOLETIM_TECNICO", obj_familia);
            frmPrincipal.deletar_tabela_fam("BOLETIM_PINTURA", obj_familia);
        // Salvar alterações
        BasePart objFamiliaPart = (BasePart)NXOpen.Utilities.NXObjectManager.Get(partTag);
            objFamiliaPart.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
            objFamiliaPart.Close(BasePart.CloseWholeTree.False, BasePart.CloseModified.CloseModified, null);
            obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
            // obj_familia.Close(BasePart.CloseWholeTree.False, BasePart.CloseModified.CloseModified, null);

            // MessageBox.Show(Codigo + "-" + rev);

           // frmPrincipal.Verificar_Blank_Fam(Codigo, rev);
            gerar_pdf_update(Codigo, rev, item[1]);
          
            frmPrincipal.Registrar_no_LOG($"Update '{Familia}' para o membro '{Codigo}'.");
        }
        public static void criar_item_fam(string Familia, string Codigo, Part obj_familia, string[] item)
        {

            //try
            //{
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            // Abrir a família
            frmPrincipal.Registrar_no_LOG($"Abrindo Família: {Familia}");
            PartLoadStatus LdSt;

            try
            {
                obj_familia = (Part)theSession.Parts.FindObject(Familia);
            }
            catch
            {
                obj_familia = theSession.Parts.Open(Familia, out LdSt);
            }

            // Obter a primeira tabela de famílias
            UFSession theUfSession = UFSession.GetUFSession();
            Tag[] families;
            theUfSession.Part.AskFamilies(obj_familia.Tag, out int numFamilies, out families);

            if (numFamilies == 0)
            {
                frmPrincipal.Registrar_no_LOG(Codigo + " - Nenhuma tabela de família encontrada.");
                throw new Exception("Nenhuma tabela de família encontrada.");
            }


            // Obter dados da família
            UFFam.FamilyData familyData;
            theUfSession.Fam.AskFamilyData(families[0], out familyData);

            // Localizar o membro pelo nome
            int memberIndex = -1;
            for (int i = 0; i < familyData.member_count; i++)
            {
                UFFam.MemberData memberData;
                theUfSession.Fam.AskMemberRowData(families[0], i, out memberData);

                if (memberData.values[0] == Codigo)
                {
                    memberIndex = i;
                    break;
                }
            }

            if (memberIndex == -1)
            {
                frmPrincipal.Registrar_no_LOG(Codigo + " não encontrado na família.");
                throw new Exception($"Membro '{Codigo}' não encontrado na família.");
            }

            // Revisar o membro
            frmPrincipal.Registrar_no_LOG($"Codigo filho encontrado: {Codigo}");
            UFFam.MemberData existingMemberData;
            theUfSession.Fam.AskMemberRowData(families[0], memberIndex, out existingMemberData);


            // Modificar o valor da revisão no campo apropriado (ex.: índice 1 para revisão)
            for (int i = 0; i < existingMemberData.values.Length; i++)
            {
                frmPrincipal.Registrar_no_LOG($"valor_antigo: {existingMemberData.values[i]}");
                existingMemberData.values[i] = item[i];
                frmPrincipal.Registrar_no_LOG($"valor novo: {existingMemberData.values[i]}");
            }

            existingMemberData.values[1] = item[1];
            existingMemberData.values[2] = item[2];
            
            //theUfSession.Fam.EditMember(families[0], memberIndex, ref existingMemberData);
            string rev = item[4];
            //frmPrincipal.Registrar_no_LOG($"rev: {item[4]}");

            //// Atualizar a instância do part
            Tag partTag, instTag;
            bool saved;
           
            theUfSession.Part.CreateFamilyInstance(families[0], memberIndex, out partTag, out instTag);

            //// Salvar alterações
            BasePart objFamiliaPart = (BasePart)NXOpen.Utilities.NXObjectManager.Get(partTag);
            objFamiliaPart.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
            //objFamiliaPart.Close(BasePart.CloseWholeTree.False, BasePart.CloseModified.CloseModified, null);
            //obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
            // obj_familia.Close(BasePart.CloseWholeTree.False, BasePart.CloseModified.CloseModified, null);

            // MessageBox.Show(Codigo + "-" + rev);

            //frmPrincipal.Verificar_Blank_Fam(Codigo, rev);
            //gerar_pdf_update(Codigo, rev, item[1]);

            frmPrincipal.Registrar_no_LOG($"Criado '{Familia}' para o membro '{Codigo}'.");
        }
        public static void criar_rev_item_fam(string Familia, string Codigo, Part obj_familia, string[] item, string alteracao, bool boletim, List<string> bt, List<string> btp, int index_pintura)
        {

            //try
            //{
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            // Abrir a família
            frmPrincipal.Registrar_no_LOG($"Abrindo Família: {Familia}");
            PartLoadStatus LdSt;

            try
            {
                obj_familia = (Part)theSession.Parts.FindObject(Familia);
            }
            catch
            {
                obj_familia = theSession.Parts.Open(Familia, out LdSt);
            }
            theSession.Parts.SetDisplay(obj_familia, true, true, out LdSt);

            theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");

            int _rev = Convert.ToInt32(item[4]) + 1;
            string rev = _rev.ToString().PadLeft(3, '0');
            NXObject[] objects1 = new NXObject[1];
            objects1[0] = obj_familia;
            AttributePropertiesBuilder attributePropertiesBuilder1;
            attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(obj_familia, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);
            attributePropertiesBuilder1.Title = "ALTERACAO";
            attributePropertiesBuilder1.StringValue = alteracao;
            NXObject nXObject1;
            nXObject1 = attributePropertiesBuilder1.Commit();
            attributePropertiesBuilder1.Destroy();
            frmPrincipal.Insert_tab_alteracao();

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

            UFSession theUfSession = UFSession.GetUFSession();
            Tag[] families;
            theUfSession.Part.AskFamilies(obj_familia.Tag, out int numFamilies, out families);

            if (numFamilies == 0)
            {
                frmPrincipal.Registrar_no_LOG(Codigo + " - Nenhuma tabela de família encontrada.");
                throw new Exception("Nenhuma tabela de família encontrada.");
            }
            UFFam.FamilyData familyData;
            theUfSession.Fam.AskFamilyData(families[0], out familyData);

            int memberIndex = -1;
            for (int i = 0; i < familyData.member_count; i++)
            {
                UFFam.MemberData memberData;
                theUfSession.Fam.AskMemberRowData(families[0], i, out memberData);

                if (memberData.values[0] == Codigo)
                {
                    memberIndex = i;
                    break;
                }
            }

            if (memberIndex == -1)
            {
                frmPrincipal.Registrar_no_LOG(Codigo + " não encontrado na família.");
                throw new Exception($"Membro '{Codigo}' não encontrado na família.");
            }

            frmPrincipal.Registrar_no_LOG($"Codigo filho encontrado: {Codigo}");
            UFFam.MemberData existingMemberData;
            theUfSession.Fam.AskMemberRowData(families[0], memberIndex, out existingMemberData);

           
            string desc_atual = existingMemberData.values[1];
            string pintura = "0";
            if (boletim == true)
            {

                pintura = "1";
                if (!desc_atual.Contains("_C/_PINTURA"))
                {
                    //  MessageBox.Show("O item será revisado com alteração na pintura. A descrição do item será atualizada para refletir essa mudança.");
                    desc_atual = desc_atual + "_C/_PINTURA";

                }

            }
            for (int i = 0; i < existingMemberData.values.Length; i++)
            {
                frmPrincipal.Registrar_no_LOG($"valor_antigo: {existingMemberData.values[i]}");
                //existingMemberData.values[i] = item[i];
                frmPrincipal.Registrar_no_LOG($"valor novo: {existingMemberData.values[i]}");
            }
            existingMemberData.values[1] = desc_atual;
            existingMemberData.values[2] = desc_atual;
            if (boletim == true)
            {
                existingMemberData.values[index_pintura] = pintura;
            }
           
            existingMemberData.values[4] = rev;
            frmPrincipal.Registrar_no_LOG($"nova_revisao: {existingMemberData.values[4]}");
            ///MessageBox.Show($" {existingMemberData.values[1]} {existingMemberData.values[4]} {existingMemberData.values[index_pintura]}");
            theUfSession.Fam.EditMember(families[0], memberIndex, ref existingMemberData);

            Tag partTag, instTag;
            int member_index, count;
            Tag[] part_list;
            int[] erro_list;
            string info;
            bool saved;

            theUfSession.Part.CreateFamilyInstance(families[0], memberIndex, out partTag, out instTag);
            theUfSession.Part.UpdateFamilyInstance(families[0], memberIndex, true, out partTag, out saved, out count, out part_list, out erro_list, out info);
            theUfSession.Modl.Update();
            atualizacao_fam_2312.update_create_item_fam(Familia, existingMemberData.values[0], obj_familia);
            frmPrincipal.deletar_tabela_fam("REVISAO_ITENS", obj_familia);
            if (boletim == true)
            {
                frmPrincipal frm = new frmPrincipal();
                frm.deletar_tabelas(obj_familia);
            }

            obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
            AtualizarDados(Codigo, rev, desc_atual);
            gerar_pdf_criacao_familia(Codigo, rev);
            frmPrincipal.Registrar_no_LOG($"Revisao de item  '{Familia}' para o membro '{Codigo}'.");
        }
        public static void criar_rev_item_fam_demanda(string Familia, string Codigo, Part obj_familia, string[] item, string alteracao, bool boletim, List<string> bt, List<string> btp, int index_pintura)
        {

            //try
            //{
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            // Abrir a família
            frmPrincipal.Registrar_no_LOG($"Abrindo Família: {Familia}");
            PartLoadStatus LdSt;

            try
            {
                obj_familia = (Part)theSession.Parts.FindObject(Familia);
            }
            catch
            {
                obj_familia = theSession.Parts.Open(Familia, out LdSt);
            }
            theSession.Parts.SetDisplay(obj_familia, true, true, out LdSt);

            theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");

            int _rev = Convert.ToInt32(item[4]) + 1;
            string rev = _rev.ToString().PadLeft(3, '0');
            NXObject[] objects1 = new NXObject[1];
            objects1[0] = obj_familia;
            AttributePropertiesBuilder attributePropertiesBuilder1;
            attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(obj_familia, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);
            attributePropertiesBuilder1.Title = "ALTERACAO";
            attributePropertiesBuilder1.StringValue = alteracao;
            NXObject nXObject1;
            nXObject1 = attributePropertiesBuilder1.Commit();
            attributePropertiesBuilder1.Destroy();
            frmPrincipal.Insert_tab_alteracao();

            if (boletim == true)
            {

                if (obj_familia != null)
                {
                    theSession.Parts.SetDisplay(obj_familia, true, true, out LdSt);

                    theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");
                }
                frmPrincipal frm = new frmPrincipal();

                frm.insert_boletin_criacao_demanda(bt, btp, obj_familia);
            }

            UFSession theUfSession = UFSession.GetUFSession();
            Tag[] families;
            theUfSession.Part.AskFamilies(obj_familia.Tag, out int numFamilies, out families);

            if (numFamilies == 0)
            {
                frmPrincipal.Registrar_no_LOG(Codigo + " - Nenhuma tabela de família encontrada.");
                throw new Exception("Nenhuma tabela de família encontrada.");
            }
            UFFam.FamilyData familyData;
            theUfSession.Fam.AskFamilyData(families[0], out familyData);

            int memberIndex = -1;
            for (int i = 0; i < familyData.member_count; i++)
            {
                UFFam.MemberData memberData;
                theUfSession.Fam.AskMemberRowData(families[0], i, out memberData);

                if (memberData.values[0] == Codigo)
                {
                    memberIndex = i;
                    break;
                }
            }

            if (memberIndex == -1)
            {
                frmPrincipal.Registrar_no_LOG(Codigo + " não encontrado na família.");
                throw new Exception($"Membro '{Codigo}' não encontrado na família.");
            }

            frmPrincipal.Registrar_no_LOG($"Codigo filho encontrado: {Codigo}");
            UFFam.MemberData existingMemberData;
            theUfSession.Fam.AskMemberRowData(families[0], memberIndex, out existingMemberData);


            string desc_atual = existingMemberData.values[1];
            string pintura = "0";
            if (boletim == true)
            {

                pintura = "1";
                if (!desc_atual.Contains("_C/_PINTURA"))
                {
                    //  MessageBox.Show("O item será revisado com alteração na pintura. A descrição do item será atualizada para refletir essa mudança.");
                    desc_atual = desc_atual + "_C/_PINTURA";

                }

            }
            for (int i = 0; i < existingMemberData.values.Length; i++)
            {
                frmPrincipal.Registrar_no_LOG($"valor_antigo: {existingMemberData.values[i]}");
                //existingMemberData.values[i] = item[i];
                frmPrincipal.Registrar_no_LOG($"valor novo: {existingMemberData.values[i]}");
            }
            existingMemberData.values[1] = desc_atual;
            existingMemberData.values[2] = desc_atual;
            if (boletim == true)
            {
                existingMemberData.values[index_pintura] = pintura;
            }

            existingMemberData.values[4] = rev;
            frmPrincipal.Registrar_no_LOG($"nova_revisao: {existingMemberData.values[4]}");
            ///MessageBox.Show($" {existingMemberData.values[1]} {existingMemberData.values[4]} {existingMemberData.values[index_pintura]}");
            theUfSession.Fam.EditMember(families[0], memberIndex, ref existingMemberData);

            Tag partTag, instTag;
            int member_index, count;
            Tag[] part_list;
            int[] erro_list;
            string info;
            bool saved;

            theUfSession.Part.CreateFamilyInstance(families[0], memberIndex, out partTag, out instTag);
            theUfSession.Part.UpdateFamilyInstance(families[0], memberIndex, true, out partTag, out saved, out count, out part_list, out erro_list, out info);
            theUfSession.Modl.Update();
            atualizacao_fam_2312.update_create_item_fam(Familia, existingMemberData.values[0], obj_familia);
            frmPrincipal.deletar_tabela_fam("REVISAO_ITENS", obj_familia);
            if (boletim == true)
            {
                frmPrincipal frm = new frmPrincipal();
                frm.deletar_tabelas(obj_familia);
            }

            obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
            AtualizarDados(Codigo, rev, desc_atual);
            gerar_pdf_criacao_familia(Codigo, rev);
            frmPrincipal.Registrar_no_LOG($"Revisao de item  '{Familia}' para o membro '{Codigo}'.");
        }


        public static void AtualizarDados(string codigo, string rev, string descricao)
        {
            conexao_db_TeamCenter conn = new conexao_db_TeamCenter();

            var (pitem, pitemrev) = conn.busca_dados_alt_desc_fam(codigo, rev );


            string up = conn.update_desc(descricao, pitem, pitemrev);
        }
        private static TableSection _nxTableSection;
        private static NXOpen.Annotations.Table _nxTable;
        public static void gerar_pdf_update(string codigo, string rev, string desc)
        {
           
             NXOpen.Session theSession = Session.GetSession();
             UI theUI = UI.GetUI();
             UFSession theUfSession = UFSession.GetUFSession();

           
            string encoded_familia = frmPrincipal.TCSearchItem(codigo, theSession,theUI, theUfSession );
            string Familia = encoded_familia;
            frmPrincipal.Registrar_no_LOG("gerar pdf:"+Familia);
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

            if (obj_familia != theSession.Parts.Display)
            {
                theSession.Parts.SetDisplay(obj_familia, false, false, out LdSt);
            }


            NXOpen.Drawings.DrawingSheetCollection sheets = obj_familia.DrawingSheets;

            foreach (NXOpen.Drawings.DrawingSheet item in sheets)
            {
                // MessageBox.Show(item.Name);

                item.Open();

                foreach (TableSection obj in obj_familia.Annotations.TableSections)
                {

                     _nxTableSection = obj;

                    UFSession.GetUFSession().Tabnot.AskTabularNoteOfSection(_nxTableSection.Tag, out Tag nxTableTag);

                    _nxTable = Session.GetSession().GetObjectManager().GetTaggedObject(nxTableTag) as NXOpen.Annotations.Table;
                    

                    string text = GetSetText(1, 1);

                    if (text == "Descrição:")
                    {

                        SetText(2, 1, desc);
                    }


                }

                NXOpen.Annotations.PartsList[] partsList1 = obj_familia.Annotations.PartsLists.ToArray();
              
                foreach (PartsList part in partsList1)
                {

                    part.Update();
                }

                NXOpen.PrintPDFBuilder printPDFBuilder1;
                printPDFBuilder1 = obj_familia.PlotManager.CreatePrintPdfbuilder();

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





                NXOpen.NXObject[] sheets1 = new NXOpen.NXObject[1];

                sheets1[0] = item;
                printPDFBuilder1.SourceBuilder.SetSheets(sheets1);

                printPDFBuilder1.CreateNewFromUi = false;

                printPDFBuilder1.DatasetName = codigo+"_" + rev + "-PDF-1";


                NXOpen.NXObject nXObject1 = printPDFBuilder1.Commit();

                printPDFBuilder1.Commit();



                printPDFBuilder1.Destroy();


            }
            frmPrincipal.Verificar_Blank_Fam(codigo, rev);


            obj_familia.Close(BasePart.CloseWholeTree.False, BasePart.CloseModified.CloseModified, null);
        }
        public static int GetNumRows()

        {

            UFSession.GetUFSession().Tabnot.AskNmRows(_nxTable.Tag, out int numRows);

            return numRows;

        }
        public static string GetSetText(int row, int column)

        {

            Tag cellTag = GetCell(row, column);





            UFSession.GetUFSession().Tabnot.AskCellText(cellTag, out string cell);


            return cell;


            // return string.Empty;

        }
        public static void SetText(int row, int column, string desc)

        {

            Tag cellTag = GetCell(row, column);





            UFSession.GetUFSession().Tabnot.SetCellText(cellTag, desc);




            // return string.Empty;

        }
        private static Tag GetCell(int row, int column)

        {

            //if (row >= 1 && row <= GetNumRows() && column >= 1 && column <= GetNumColumns())

            //{

            UFSession.GetUFSession().Tabnot.AskNthRow(_nxTable.Tag, row - 1, out Tag rowTag);

            UFSession.GetUFSession().Tabnot.AskNthColumn(_nxTable.Tag, column - 1, out Tag columnTag);



            UFSession.GetUFSession().Tabnot.AskCellAtRowCol(rowTag, columnTag, out Tag cellTag);



            return cellTag;

            //}
            //else
            //{
            //    return cellTag;
            //}



        }
        public int GetNumColumns()

        {

            UFSession.GetUFSession().Tabnot.AskNmColumns(_nxTable.Tag, out int numColumns);

            return numColumns;

        }

    }
}
