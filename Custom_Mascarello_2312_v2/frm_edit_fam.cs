using NXOpen.CAE;
using NXOpen.UF;
using NXOpen;
using NXOpen.UIStyler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NXOpen.Mechatronics.ElectricalPartBuilder;

namespace Custom_Mascarello
{
    public partial class frm_edit_fam : Form
    {
        private static Session theSession;
        private static UI theUI;
        private static UFSession theUfSession;

        private static string _fam;
        public frm_edit_fam(string fam)
        {
            InitializeComponent();
            _fam = fam;
        }

        private void frm_edit_fam_Load(object sender, EventArgs e)
        {
            Text = "Família: " + _fam;
            carregar_fam();
        }
        public static string TCSearchItem(string item_id)
        {
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
        public static void Registrar_no_LOG(string Mensagem)
        {
            string gLog_FileName = @"c:\temp\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            System.IO.StreamWriter vWriter = new System.IO.StreamWriter(gLog_FileName, true, System.Text.Encoding.Unicode);
            vWriter.WriteLine(DateTime.Now.ToString() + " " + Mensagem);
            vWriter.Close();
        }
        Part obj_familia;
        string Familia;
        private void carregar_fam()
        {

            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theUfSession = UFSession.GetUFSession();


            string encoded_familia = TCSearchItem(_fam.Substring(0,6));
            Familia = encoded_familia;

            PartLoadStatus LdSt;
           

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
                dtg_resultado.Columns.Add(atb, atb);
            }

            for (int member_idx = 0; member_idx < family_data.member_count; member_idx++)
            {
                UFFam.MemberData member_data;
                theUfSession.Fam.AskMemberRowData(families[0], member_idx, out member_data);
                dtg_resultado.Rows.Add(member_data.values[0]);

                for (int i = 1; i < member_data.values.Length; i++)
                {
                    dtg_resultado[i, member_idx].Value = member_data.values[i];
                }
            }
            obj_familia.Close(NXOpen.BasePart.CloseWholeTree.False, NXOpen.BasePart.CloseModified.CloseModified, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] item = new string[dtg_resultado.ColumnCount];
            int i = 0;
            foreach (DataGridViewRow row in dtg_resultado.SelectedRows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    item[i] = cell.Value.ToString();
                    i++;
                }
            }
            string dados = string.Join(";", item);
            frmPrincipal.Registrar_no_LOG("Atualizando Item: " + item[0] + " - " + dados);
            atualizacao_fam_2312.update_item_fam(Familia, item[0], obj_familia, item, false);
        }
    }
}
