using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Windows.Forms;
using MySqlX.XDevAPI.Relational;
using NXOpen;
using NXOpen.Annotations;
using NXOpen.Assemblies;
using NXOpen.BlockStyler;
using NXOpen.CAE;
using NXOpen.CAE.AeroStructures.Author;
using NXOpen.CAM;
using NXOpen.Drawings;
using NXOpen.Features;
using NXOpen.Features.AECDesign;
using NXOpen.Features.SheetMetal;
using NXOpen.Gateway;
using NXOpen.PDM;
using NXOpen.UF;
using System.Reflection;
using System.Text.Json;

using NXOpen.UIStyler;
using PLMComponents.Parasolid.PK_.Unsafe;
using Snap.NX;
using static NXOpen.CAE.Post;
using static NXOpen.CAE.UnifiedDatabaseOptions;
using static NXOpen.Features.BridgeCurveBuilder;
using static NXOpen.Mechatronics.ElectricalPartBuilder;
using static NXOpen.UF.UFFam;
using static NXOpen.UF.UFModlTrex;
using Body = NXOpen.Body;
using DataSet = System.Data.DataSet;
using Expression = NXOpen.Expression;
using ExtractFace = NXOpen.Features.ExtractFace;
using Face = NXOpen.Face;
using Feature = NXOpen.Features.Feature;
using Note = NXOpen.Annotations.Note;
using NXObject = NXOpen.NXObject;
using Part = NXOpen.Part;
using Path = System.IO.Path;
using Point = NXOpen.Point;
using Unit = NXOpen.Unit;
using View = NXOpen.View;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using System.Windows.Documents;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using MiniSnap.NX;
using Snap.Geom;

using NXOpen.CAE.Xyplot;
using System.Threading;
using static NXOpen.CAE.AssignNodalCSBuilder;
using Snap.UI.Block;
using System.Net;
using System.Net.Sockets;
using static NXOpen.CAM.FBM.ThreadFeatureGeometry;
using Form = System.Windows.Forms.Form;
using static NXOpen.UF.UFMb;
using System.Collections.Concurrent;
using Action = System.Action;
using AppCOM;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Label = System.Windows.Forms.Label;
using Button = System.Windows.Forms.Button;
using static NXOpen.Tooling.FormabilitySimProcessConfigBuilder;
using System.ServiceModel.Description;
using NXOpen.Weld;

namespace Custom_Mascarello
{
    public partial class frmPrincipal : Syncfusion.WinForms.Controls.SfForm
    {
        private static Session theSession;
        private static UI theUI;
        private static UFSession theUfSession;

        public static List<string> Resultado_Listar = new List<string>();
        public static string lbl_informacao;
        string pesquisa = "0";
        bool btn_pesquisa_pc = false;
        string codigo_item = "";

        public static List<string> gerar_tubo_cv_dim = new List<string>();
        public static List<string> gerar_tubo_cv_fam_cv = new List<string>();
        public static List<string> gerar_tubo_cv_fam_item = new List<string>();

        double hip;
        double altura;
        double largura;
        double alt_tubo;

        bool inverter = false;

        bool flg = false;
        double alfa;
        double beta;

        double ang_corte;

        double comp_final;
        public frmPrincipal()
        {

            InitializeComponent();



        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
           
        }
       
        public static string TCSearchItem(string item_id)
        {
            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theUfSession = UFSession.GetUFSession();
            string encodedname = "";

            string[] entries = { "item_id" };
            string[] values = { item_id };

          //  Registrar_no_LOG(" DEBUG -> (TCSearchItem) Pesquisando Item:" + item_id);

            NXOpen.PDM.PdmSearch mySearch = theSession.PdmSearchManager.NewPdmSearch();
            NXOpen.PDM.SearchResult mySearchResult = mySearch.Advanced(entries, values);

            string[] results = mySearchResult.GetResultItemIds();

           // Registrar_no_LOG(" DEBUG -> (TCSearchItem) Itens Encontrados:" + results.Length.ToString());
            if (results.Length > 0)
            {
               // Registrar_no_LOG(" DEBUG -> (TCSearchItem) results[0]:" + results[0]);

                encodedname = GetEncondedPartName(results[0]);

               // Registrar_no_LOG(" DEBUG -> (TCSearchItem) encodedname:" + encodedname);

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

        private async Task task_aguardar()
        {
            System.Threading.Thread.Sleep(20000);
        }
        private async void button13_Click_1(object sender, EventArgs e)
        {
            int cont = 0;
            while (true)
            {
                await Task.Run(async () =>
            {

                this.Invoke(new Action(() =>

                {
                    string cod = "443448";
                    string encoded_familia = TCSearchItem(cod);



                    PartLoadStatus LdSt;
                    Part obj_familia;
                    Part obj_atual;
                    try
                    {
                        obj_familia = (Part)theSession.Parts.FindObject(encoded_familia);
                    }
                    catch
                    {
                        obj_familia = theSession.Parts.Open(encoded_familia, out LdSt);
                    }

                    obj_familia.Close(BasePart.CloseWholeTree.False, BasePart.CloseModified.CloseModified, null);




                }));
                cont++;
                label1.Text = cont.ToString();
                System.Threading.Thread.Sleep(1800000);

            });
            }
        }
        
    }
}

