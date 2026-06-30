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
using static Custom_Mascarello.frm_PortaPacotes;
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
using static NXOpen.Tooling.ReusablePocketBuilder;
using Teamcenter.Services.Internal.Strong.Core._2011_06.ICT;
using Teamcenter.Soa.Internal.Client;
using static NXOpen.UF.UFEvalsf;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;
using ComboBox = System.Windows.Forms.ComboBox;
using TreeView = System.Windows.Forms.TreeView;
using System.Text.Json.Serialization;
using static NXOpen.Tooling.InsertQRBarCodeBuilder;
using Teamcenter.Schemas.Structuremanagement._2012_02.Structureverification;
using static Custom_Mascarello.frmPrincipal;
using static NXOpen.CAM.CAMObject;
using Teamcenter.Soa.Client.Model.Strong;
using Session = NXOpen.Session;
using Image = System.Drawing.Image;
using Teamcenter.Services.Strong.Core._2008_06.DataManagement;
using static NXOpen.UF.UFPart;
using static NXOpen.BodyDes.OnestepUnformBuilder;
using static PLMComponents.Parasolid.PK_.Unsafe.SESSION;

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

            for (int i = 1; i < dtg_resultado.Columns.Count; i++)
            {
                dtg_resultado.Columns[i].Visible = false;
            }

        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            Image image = Image.FromFile(@"X:\Xml\Imagens\IMG.JPG");
            this.picbox_fams.Image = image;

            //DataSet dadosXML = new DataSet();
            //dadosXML.ReadXml(@"X:\Xml\Lista_Familia\Lista_Familias_Geral.xml");
            //string item_pai = cmb_tipo_mp.Text;

            conexao_bd con = new conexao_bd();
            DataSet dataSet = con.busca_categorias_fam("Todos");

            foreach (DataRow dRow in dataSet.Tables["tab_fam_nx"].Rows)
            {
                if (dRow["classificacao"].ToString() != "")
                {
                    cmb_tipo_mp.Items.Add(dRow["classificacao"].ToString());
                }

            }
            cmb_tipo_mp.Sorted = true;

            string versao = GetBuildDate();

            Text = "Custom Mascarello 2312 versao" + versao;
        }
        static string GetBuildDate()
        {
            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            DateTime buildDate = File.GetLastWriteTime(assemblyPath);
            return buildDate.ToString("ddMMyy.HHmmss");

        }
        private void Create_Conjuntos()
        {

        }
        private void Create_fam_tubos()
        {
            bool cnc = false;
            this.lbl_inf.Text = "";
            string arquivo = "";
            string cod_fam = "";
            string cod_fam_desc = "";
            cod_fam_desc = cod_fam;
            // cmb_familia_pc.Text.Substring(0, 6);
            string comp_tubo = COMP.Text;
            double Comprimento = Convert.ToDouble(COMP.Text);
            double ANG_A_V = Convert.ToDouble(this.ANG_A_V.Text);
            double ANG_B_V = Convert.ToDouble(this.ANG_B_V.Text);
            double ANG_A_H = Convert.ToDouble(this.ANG_A_H.Text);
            double ANG_B_H = Convert.ToDouble(this.ANG_B_H.Text);


            if (((ANG_A_H >= 0 || ANG_A_H <= 0) && ANG_A_H <= 68 && (ANG_B_H >= 0 || ANG_B_H <= 0) && ANG_B_H <= 68 && ANG_A_V == 0 && ANG_B_V == 0 && Comprimento <= 3000) ||
                 ((ANG_A_V >= 0 || ANG_A_V <= 0) && ANG_A_V <= 68 && (ANG_B_V >= 0 || ANG_B_V <= 0) && ANG_B_V <= 68 && ANG_A_H == 0 && ANG_B_H == 0 && Comprimento <= 3000))
            {

                cnc = true;

                for (int i = 0; i <= gerar_tubo_cv_dim.Count - 1; i++)
                {
                    if (gerar_tubo_cv_dim[i] == cmb_familia_pc.Text)
                    {
                        cod_fam = gerar_tubo_cv_fam_cv[i];
                        cod_fam_desc = gerar_tubo_cv_fam_item[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i <= gerar_tubo_cv_dim.Count - 1; i++)
                {
                    if (gerar_tubo_cv_dim[i] == cmb_familia_pc.Text)
                    {
                        cod_fam = gerar_tubo_cv_fam_item[i];
                        cod_fam_desc = cod_fam;
                    }
                }
            }
            string desc_1 = "";
            string desc_2 = "";
            string desc_3 = "";
            string desc_4 = "";


            desc_1 = "_AV" + ANG_A_V.ToString() + "°";
            desc_2 = "_BV" + ANG_B_V.ToString() + "°";
            desc_3 = "_AH" + ANG_A_H.ToString() + "°";
            desc_4 = "_BH" + ANG_B_H.ToString() + "°";

            string Material = "";
            string busca_descricao = Custom_Mascarello.Search_Material_Tubos.Busca_Descricao(cod_fam_desc);

            string[] expressao = { "COMP", "ANG_A_V", "ANG_B_V", "ANG_A_H", "ANG_B_H" };
            string descricao = busca_descricao + "x" + comp_tubo + "mm" + desc_1 + desc_2 + desc_3 + desc_4;

            double tol_comp = Convert.ToDouble(Num_Tol_Comp_tubo.Value);
            double tol_av = Convert.ToDouble(Num_Tol_AV.Value);
            double tol_bv = Convert.ToDouble(Num_Tol_BV.Value);
            double tol_ah = Convert.ToDouble(Num_Tol_AH.Value);
            double tol_bh = Convert.ToDouble(Num_Tol_BH.Value);

            if (Convert.ToDouble(Num_Tol_Comp_tubo.Value) == 0)
            {
                tol_comp = 0.1;
            }
            if (Convert.ToDouble(Num_Tol_AV.Value) == 0)
            {
                tol_av = 0.1;
            }
            if (Convert.ToDouble(Num_Tol_BV.Value) == 0)
            {
                tol_bv = 0.1;
            }
            if (Convert.ToDouble(Num_Tol_AH.Value) == 0)
            {
                tol_ah = 0.1;
            }
            if (Convert.ToDouble(Num_Tol_BH.Value) == 0)
            {
                tol_bh = 0.1;
            }
            double[] valor = { Comprimento, ANG_A_V, ANG_B_V, ANG_A_H, ANG_B_H };
            double[] tolerancia = { tol_comp, tol_av, tol_bv, tol_ah, tol_bh };

            string DataSul = "";
            //   txt_codigo_pc.ResetText();

            dtg_resultado.Columns.Clear();
            dtg_resultado.Columns.Add("CODIGO", "CODIGO");
            for (int i = 0; i <= expressao.Length - 1; i++)
            {
                dtg_resultado.Columns.Add(expressao[i], expressao[i]);//Acrescenta colunas
            }

            if (pesquisa == "0")
            {
                if (cnc == false)
                {
                    Material = Custom_Mascarello.Search_Material_Tubos.Busca_Material(cod_fam, comp_tubo);
                    arquivo = Custom_Mascarello.Search_Tubos.SearchMember(cod_fam, descricao, expressao, valor, 0, tolerancia, DataSul, descricao, Material);
                }
                if (cnc == true)
                {
                    descricao = "CV_" + descricao;
                    arquivo = Custom_Mascarello.Search_Tubos_cnc.SearchMember(cod_fam, descricao, expressao, valor, 0, tolerancia, "", descricao);
                }
                if (arquivo == "")
                {
                    this.lbl_inf.Text = "Criando o item....";
                    // this.txt_Cod_DataSul.Text = "";
                    if (cnc == false)
                    {
                        //frm_codigo_datasul frm = new frm_codigo_datasul(descricao);
                        //frm.ShowDialog();
                        //DataSul = frm.Codigo;
                        arquivo = Custom_Mascarello.Create_Tubos.CreateMember(cod_fam, descricao, expressao, valor, 0, tolerancia, DataSul, descricao, Material);
                        atualizacao_fam_2312.gerar_pdf_criacao_familia(arquivo.Substring(0, 6), "000");
                    }
                    if (cnc == true)
                    {
                        arquivo = Custom_Mascarello.Create_Tubos_cnc.CreateMember(cod_fam, descricao, expressao, valor, 0, tolerancia, DataSul, descricao);
                        string ok_save = Save_Fam(cod_fam);
                    }
                    this.lbl_inf.Text = "Item criado";


                    dtg_resultado.Rows.Add(arquivo, COMP.Text, this.ANG_A_V.Text, this.ANG_B_V.Text, this.ANG_A_H.Text, this.ANG_B_H.Text);
                    arquivo = "";
                }

                if (arquivo != "")
                {

                    for (int i = 0; i < Resultado_Listar.Count; i++)
                    {
                        string[] quebra = Resultado_Listar[i].Split(';');
                        dtg_resultado.Rows.Add(quebra[0], quebra[1], quebra[2], quebra[3], quebra[4], quebra[5]);
                    }
                    this.lbl_inf.Text = "Item já existe";
                }
            }
            if (pesquisa == "1" && btn_pesquisa_pc == true)
            {
                if (cnc == false)
                {
                    //   this.txt_Cod_DataSul.Text = "";
                    arquivo = Custom_Mascarello.Search_Tubos.SearchMember(cod_fam, descricao, expressao, valor, 0, tolerancia, DataSul, descricao, Material);
                    if (arquivo == "")
                    {
                        lbl_informacao = "Não existe item com os parametros solicitados!";
                        this.lbl_inf.Text = lbl_informacao;
                    }
                }
                if (cnc == true)
                {
                    descricao = "CV_" + descricao;
                    arquivo = Custom_Mascarello.Search_Tubos_cnc.SearchMember(cod_fam, descricao, expressao, valor, 0, tolerancia, "", descricao);
                    if (arquivo == "")
                    {
                        lbl_informacao = "Não existe item com os parametros solicitados!";
                        this.lbl_inf.Text = lbl_informacao;
                    }
                }
            }
        }
        private void Create_fam_chapas()
        {
            this.lbl_inf.Text = "";
            string arquivo = "";
            string cod_fam = cmb_familia_pc.Text.Substring(0, 6);

            double var_A = Convert.ToDouble(txt_a_chapa.Text);
            double var_B = Convert.ToDouble(txt_b_chapa.Text);

            string Material = "";

            string temp_desc = cmb_familia_pc.Text;
            string desc_padrao = conexao_bd.select_desc(cod_fam);

            string[] descricao = desc_padrao.Split(';');


            string descricao_final = descricao[0];


            string[] expressao = { "LARG", "ALT" };
            // string descricao = busca_descricao + "x" + Convert.ToString(var_A) + "x" + Convert.ToString(var_B) + "mm";

            double[] valor = { var_A, var_B };

            for (int i = 0; i <= 1; i++)
            {
                descricao_final = descricao_final + valor[i] + descricao[i + 1];
            }
            double[] valor_inv = { var_B, var_A };


            double tol_a = Convert.ToDouble(Num_Tol_A.Value);

            double tol_b = Convert.ToDouble(Num_Tol_B.Value);


            if (Convert.ToDouble(Num_Tol_A.Value) == 0)
            {
                tol_a = 0.1;
            }
            if (Convert.ToDouble(Num_Tol_B.Value) == 0)
            {
                tol_b = 0.1;
            }


            double[] tolerancia = { tol_a, tol_b };


            string DataSul = "";
            tab_itens.ResetText();

            dtg_resultado.Columns.Clear();
            dtg_resultado.Columns.Add("CODIGO", "CODIGO");
            for (int i = 0; i <= expressao.Length - 1; i++)
            {
                dtg_resultado.Columns.Add(expressao[i], expressao[i]);//Acrescenta colunas
            }

            if (pesquisa == "0")
            {
                arquivo = Custom_Mascarello.Search_Chapas.SearchMember(cod_fam, descricao_final, expressao, valor, 0, tolerancia, DataSul, descricao_final);

                if (arquivo == "")
                {
                    List<string> mp_select = new List<string>();
                    DataSet dadosXML_1 = new DataSet();
                    dadosXML_1.ReadXml(@"X:\Xml\Lista_Familia\Selecao_Material_Geral.xml");

                    foreach (DataRow dRow in dadosXML_1.Tables["Fam"].Rows)
                    {
                        if (dRow["familia"].ToString() == cod_fam)
                        {
                            mp_select.Add(dRow["codigo"].ToString());
                        }
                    }

                    if (mp_select.Count > 0)
                    {
                        frm_selecionar_mp abrir = new frm_selecionar_mp();
                        abrir.lista_mp = mp_select;
                        //string mp_sel = 
                        abrir.ShowDialog();


                        string mp_ret = abrir.MP_returnada;
                        if (mp_ret != "")
                        {
                            mp_ret = mp_ret + "|BibliotecaMascarello.xml";
                            this.lbl_inf.Text = "Criando o item....";
                            // this.txt_Cod_DataSul.Text = "";
                            arquivo = Custom_Mascarello.Create_Chapas.CreateMember_material(cod_fam, descricao_final, expressao, valor, 0, tolerancia, DataSul, descricao_final, mp_ret);
                            string ok_save = Save_Fam(cod_fam);
                            this.lbl_inf.Text = "Item criado";
                            dtg_resultado.Rows.Add(arquivo, txt_a_chapa.Text, txt_b_chapa.Text);
                            atualizacao_fam_2312.gerar_pdf_criacao_familia(arquivo.Substring(0, 6), "000");
                            arquivo = "";
                        }
                        else
                        {
                            this.lbl_inf.Text = "Item não criado....MP não especificada";
                        }
                    }

                    else
                    {
                        this.lbl_inf.Text = "Criando o item....";
                        // this.txt_Cod_DataSul.Text = "";
                        arquivo = Custom_Mascarello.Create_Chapas.CreateMember(cod_fam, descricao_final, expressao, valor, 0, tolerancia, DataSul, descricao_final);
                        string ok_save = Save_Fam(cod_fam);
                        this.lbl_inf.Text = "Item criado";
                        dtg_resultado.Rows.Add(arquivo, txt_a_chapa.Text, txt_b_chapa.Text);
                        atualizacao_fam_2312.gerar_pdf_criacao_familia(arquivo.Substring(0, 6), "000");
                        arquivo = "";
                    }

                }
                if (arquivo != "")
                {
                    for (int i = 0; i < Resultado_Listar.Count; i++)
                    {
                        string[] quebra = Resultado_Listar[i].Split(';');

                        dtg_resultado.Rows.Add(quebra[0], quebra[1], quebra[2]);

                    }
                    this.lbl_inf.Text = "Item já existe";
                }

            }
            if (pesquisa == "1" && btn_pesquisa_pc == true)
            {
                //this.txt_Cod_DataSul.Text = "";
                arquivo = Custom_Mascarello.Search_Chapas.SearchMember(cod_fam, descricao_final, expressao, valor, 0, tolerancia, DataSul, descricao_final);
                //if (arquivo == "")
                //{
                //arquivo = Custom_Mascarello.Search_Chapas.SearchMember(cod_fam, descricao, expressao, valor_inv, 0, tolerancia, DataSul, descricao);

                if (arquivo == "")
                {
                    lbl_informacao = "Não existe item com os parametros solicitados!";
                    this.lbl_inf.Text = lbl_informacao;
                }

                //}
                //if (arquivo != "")
                //{

                //    this.lbl_inf.Text = lbl_informacao;
                //    this.txt_Cod_DataSul.Text = arquivo;
                //}
            }

        }

        private void Create_fam_geral()
        {
            string arquivo = "";
            string cod_fam = cmb_familia_pc.Text.Substring(0, 6);

            DataSet dadosXML = new DataSet();
            dadosXML.ReadXml(@"X:\Xml\Lista_Familia\Lista_Familias_Geral.xml");
            string item_pai = cmb_tipo_mp.Text;
            string[] n_attr;
            int cont = 0;
            foreach (DataRow dRow in dadosXML.Tables["Fam"].Rows)
            {
                if (dRow["Codigo"].ToString() == cod_fam)
                {
                    n_attr = dRow["attr"].ToString().Split(';');
                    for (int i = 0; i <= n_attr.Length - 1; i++)
                    {
                        cont++;
                    }
                }
            }
            string[] attributos = new string[cont];
            string[] descricao = new string[cont + 1];

            dtg_resultado.Columns.Clear();
            foreach (DataRow dRow in dadosXML.Tables["Fam"].Rows)
            {
                if (dRow["Codigo"].ToString() == cod_fam)
                {

                    attributos = dRow["attr"].ToString().Split(';');
                    descricao = dRow["desc_item"].ToString().Split(';');
                }
            }
            dtg_resultado.Columns.Add("CODIGO", "CODIGO");
            for (int i = 0; i <= attributos.Length - 1; i++)
            {
                dtg_resultado.Columns.Add(attributos[i], attributos[i]);
            }
            string[] expressao = new string[attributos.Length];
            for (int i = 0; i <= attributos.Length - 1; i++)
            {
                expressao[i] = attributos[i];
            }
            double[] valor = new double[attributos.Length];
            foreach (Control tabcontrol in Controls)
            {

                if (tabcontrol.Name == "tab_itens")
                {

                    foreach (Control tabpage in tabcontrol.Controls)
                    {

                        if (tabpage.Name == "tab_fam")
                        {

                            foreach (Control tabcontrol_1 in tabpage.Controls)
                            {

                                foreach (Control tabcontrol_2 in tabpage.Controls)
                                {
                                    foreach (Control tabcontrol_6 in tabcontrol_2.Controls)
                                    {




                                        if (tabcontrol_6.Name == "tab_pecas")
                                        {

                                            foreach (Control tabcontrol_3 in tabcontrol_6.Controls)
                                            {
                                                if (tabcontrol_3.Name == "grp_geral")
                                                {

                                                    for (int i = 0; i <= expressao.Length - 1; i++)
                                                    {
                                                        foreach (Control txt_attr in tabcontrol_3.Controls)
                                                        {
                                                            if (txt_attr is TextBox)
                                                            {
                                                                if (txt_attr.Name == expressao[i])
                                                                {
                                                                    valor[i] = Convert.ToDouble(txt_attr.Text);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            string descricao_final = descricao[0];
            for (int i = 0; i <= attributos.Length - 1; i++)
            {
                descricao_final = descricao_final + valor[i] + descricao[i + 1];
            }
            double[] tolerancia = new double[attributos.Length];
            for (int i = 1; i <= attributos.Length - 1; i++)
            {
                tolerancia[i] = Convert.ToDouble(0.1);
            }
            string codigo_novo = "";
            if (pesquisa == "1" && btn_pesquisa_pc == true)
            {

                codigo_novo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");
                if (codigo_novo != "")
                {
                    dtg_resultado.Rows.Add(codigo_novo);

                    for (int i = 0; i <= valor.Length - 1; i++)
                    {
                        dtg_resultado[i + 1, 0].Value = valor[i];
                    }
                }
                else
                {
                    lbl_informacao = "Não existe item com os parametros solicitados!";
                    this.lbl_inf.Text = lbl_informacao;
                }
            }
            if (pesquisa == "0")
            {

                arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, descricao_final, expressao, valor, 0, tolerancia, "", descricao_final);
                if (arquivo != "")
                {
                    codigo_novo = arquivo;
                    dtg_resultado.Rows.Add(codigo_novo);

                    for (int i = 0; i <= valor.Length - 1; i++)
                    {
                        dtg_resultado[i + 1, 0].Value = valor[i];
                    }
                }
                if (arquivo == "")
                {
                    List<string> mp_select = new List<string>();
                    DataSet dadosXML_1 = new DataSet();
                    dadosXML_1.ReadXml(@"X:\Xml\Lista_Familia\Selecao_Material_Geral.xml");

                    foreach (DataRow dRow in dadosXML_1.Tables["Fam"].Rows)
                    {
                        if (dRow["familia"].ToString() == cod_fam)
                        {
                            mp_select.Add(dRow["codigo"].ToString());
                        }
                    }

                    if (mp_select.Count > 0)
                    {
                        frm_selecionar_mp abrir = new frm_selecionar_mp();
                        abrir.lista_mp = mp_select;
                        //string mp_sel = 
                        abrir.ShowDialog();

                        string mp_ret = abrir.MP_returnada;
                        if (mp_ret != "")
                        {
                            mp_ret = mp_ret + "|BibliotecaMascarello.xml";
                            this.lbl_inf.Text = "Criando o item....";
                            codigo_novo = "";// Custom_Mascarello.Create.CreateMember(cod_fam, descricao_final, expressao, valor, 0, tolerancia, "", descricao_final, mp_ret);

                            dtg_resultado.Rows.Add(codigo_novo);

                            for (int i = 0; i <= valor.Length - 1; i++)
                            {
                                dtg_resultado[i + 1, 0].Value = valor[i];
                            }
                        }
                        else
                        {
                            this.lbl_inf.Text = "Item não criado....MP não especificada";
                        }
                    }
                    else
                    {
                        this.lbl_inf.Text = "Criando o item....";
                        //  MessageBox.Show(cod_fam  +" " + descricao_final+ "  "+expressao +"  "+ valor);
                        codigo_novo = Custom_Mascarello.Create.CreateMember(cod_fam, descricao_final, expressao, valor, 0, tolerancia, "", descricao_final);

                        dtg_resultado.Rows.Add(codigo_novo);

                        for (int i = 0; i <= valor.Length - 1; i++)
                        {
                            dtg_resultado[i + 1, 0].Value = valor[i];
                        }
                        //MessageBox.Show(codigo_novo);
                        atualizacao_fam_2312.gerar_pdf_criacao_familia(codigo_novo.Substring(0, 6), "000");

                    }
                }
            }
        }
        List<string> lista_atributos = new List<string>();

        public void sub_fam(string jsonString, List<double> lista_valores_cad, string fam)
        {
           
            Modelo dados_familia = JsonConvert.DeserializeObject<Modelo>(jsonString);

           // string fam = "";

            bool create_fluxo = false;
            foreach (var item in dados_familia.Itens)
            {
                List<string> lista_atributos = new List<string>();
                List<double> lista_valores = new List<double>();
                List<double> lista_ajustes = new List<double>();
                List<double> lista_tol = new List<double>();
                foreach (var attr in item.Ajuste)
                {
                    lista_ajustes.Add(attr);
                }
                foreach (var attr in item.Atributos)
                {
                    lista_atributos.Add(attr);
                    lista_tol.Add(0);
                }
                int i = 0;
                if (lista_valores_cad is null )
                {
                    foreach (var valores in item.Valor)
                    {
                        if (valores.Length > 1)
                        {
                            foreach (Control ctrl in grp_geral.Controls)
                            {
                                if (ctrl is TextBox)
                                {
                                    if (ctrl.Name == valores)
                                    {
                                        lista_valores.Add(Convert.ToDouble(ctrl.Text) + (lista_ajustes[i]));
                                    }
                                }
                                if (ctrl is ComboBox)
                                {
                                    if (ctrl.Name == valores)
                                    {
                                        string[] _valor = ctrl.Text.Split('-');
                                        lista_valores.Add(Convert.ToDouble(_valor[0].TrimEnd()) + (lista_ajustes[i]));
                                    }
                                }
                            }
                        }
                        else
                        {
                            //MessageBox.Show(valores);
                            lista_valores.Add(Convert.ToDouble(valores) + (lista_ajustes[i]));
                        }
                    }
                }
                else
                {
                    foreach (double valores in lista_valores_cad)
                    {
                        lista_valores.Add(valores + (lista_ajustes[i]));
                        i++;
                    }
                    create_fluxo = true;
                }
                string desc_attibuto = "";
               
                
                string jsondata = conexao_bd.busca_atributos_json(fam);
                Atributos jsonData = JsonConvert.DeserializeObject<Atributos>(jsondata);



                    //if (jsonData != null)
                    //{
                    //    foreach (var item in jsonData.Lista_atributos)
                    //    {


                    //        foreach (var val in item.Valor)
                    //        {
                    //            if (val.Substring(0, 1) == valor[attributos.Length - 1].ToString())
                    //            {
                    //                string[] quebra = val.Split('-');
                    //                desc_attibuto = quebra[2];
                    //            }
                    //        }

                    //    }
                    //}
               
                string val = string.Join(";", lista_valores);
              
                fam = item.Item;

                string[] att = lista_atributos.ToArray();
                double[] att_valores = lista_valores.ToArray();
                double[] tol = lista_tol.ToArray();

                string desc_padrao = conexao_bd.select_desc(fam);



                string[] descricao = desc_padrao.Split(';');

                string descricao_final = descricao[0];

                for (int j = 0; j <= att.Length - 1; j++)
                {
                    descricao_final = descricao_final + att_valores[j] + descricao[j + 1];
                }

                busca_default(fam, descricao_final, att, att_valores, tol, create_fluxo);
              
            }
        }

        private void Create_fam_geral_Sincrona()
        {
            Session theSession = Session.GetSession();
            PartLoadStatus loadStatus;


            Part displayAnterior = theSession.Parts.Display;


            string arquivo = "";
            string cod_fam = cmb_familia_pc.Text.Substring(0, 6);


            string jsonString = conexao_bd.busca_subfam_json(cod_fam);

            if (jsonString != "")
            {
                sub_fam(jsonString, null, cod_fam);
            }

            string item_pai = cmb_tipo_mp.Text;
            string[] attributos = lista_atributos.ToArray();


            string desc_padrao = conexao_bd.select_desc(cod_fam);

            string[] descricao = desc_padrao.Split(';');

            string[] expressao = new string[attributos.Length];
            for (int i = 0; i <= attributos.Length - 1; i++)
            {
                expressao[i] = attributos[i];
            }
            bool attr_pintura = false;
            string busca_valores = "";
            string desc_lista = "";
            List<string> desc_list_1 = new List<string>();
            foreach (string item in attributos)
            {


                foreach (Control txt_valores in grp_geral.Controls)
                {
                    if (txt_valores is TextBox)
                    {

                        if (txt_valores.Text != "" && txt_valores.Name == item)
                        {
                            if (item == "PINTURA" && txt_valores.Text == "1")
                            {
                                attr_pintura = true;
                            }
                            busca_valores = busca_valores + txt_valores.Text + ";";
                        }
                    }
                    if (txt_valores is ComboBox)
                    {

                        if (txt_valores.Text != "" && txt_valores.Name == item)
                        {
                            string[] _valor = txt_valores.Text.Split('-');
                          
                            busca_valores = busca_valores + _valor[0].TrimEnd() + ";";
                            desc_lista = txt_valores.Text;
                            desc_list_1.Add(desc_lista);
                        }
                    }

                }
            }

            busca_valores = busca_valores.Remove(busca_valores.Length - 1, 1);

            double[] valor = new double[attributos.Length];

            for (int i = 0; i <= busca_valores.Split(';').Length - 1; i++)
            {
                valor[i] = Convert.ToDouble(busca_valores.Split(';')[i]);

            }


            string descricao_final = descricao[0];


            for (int i = 0; i <= attributos.Length - 1; i++)
            {

                descricao_final = descricao_final + valor[i] + descricao[i + 1];
              
                /// aqui está o problema, o que é eu não sei
            }

            descricao_final = descricao_final.Replace("\"", "");

            if (valores_att.Count == 1)
            {
                descricao_final = descricao_final.Remove(descricao_final.Length - 1, 1);
            }
            if (valores_att.Count == 2)
            {
                descricao_final = descricao_final.Remove(descricao_final.Length - 2, 2);

            }
            if (valores_att.Count == 3)
            {
                descricao_final = descricao_final.Remove(descricao_final.Length - 3, 3);
            }
            if (valores_att.Count == 4)
            {
                descricao_final = descricao_final.Remove(descricao_final.Length - 4, 4);
            }
            if (valores_att.Count>0)
            { 
                
                int cont = 0;
                  if(desc_list_1.Count > 0)
                {
                    descricao_final= descricao_final.Remove(descricao_final.Length - 1, 1);
                }
                foreach (string desc in desc_list_1)
                {
                    string aux = "";
                    string[] valor_desc = desc.Split('-');
                    if (cont >= 1 && valor_desc[2].TrimStart() !="")
                    { aux = "_"; }
                    descricao_final = descricao_final + aux + valor_desc[2].TrimStart();
                    cont++;
                }
               
            }
            //  MessageBox.Show(descricao_final);
            double[] tolerancia = new double[attributos.Length];
            for (int i = 0; i <= attributos.Length - 1; i++)
            {
                tolerancia[i] = Convert.ToDouble(0.1);
            }

            string codigo_novo = "";
            bool existe = false;

            string[] mp_auto = conexao_bd.busca_mp_fam(cod_fam).Split(';');

            string attDefMP = "";
            int posicao = 0;
            string mp = "";
            if (mp_auto.Length > 1)
            {
                attDefMP = mp_auto[mp_auto.Length - 1];
                for (int i = 0; i <= attributos.Length - 1; i++)
                {
                    if (attributos[i].Trim() == attDefMP.Trim())
                    {
                        posicao = i;
                    }
                }
                double valor_attDefMP = valor[posicao];
                mp = mp_auto[Convert.ToInt32(valor_attDefMP)];


                var lista = expressao.ToList();
                lista.RemoveAt(lista.Count - 1);
                expressao = lista.ToArray();

                var lista_valor = valor.ToList();
                lista_valor.RemoveAt(lista_valor.Count - 1);
                valor = lista_valor.ToArray();

                LogMessage_fam($"MP que será usada {mp}");
            }

            bool ver_pesquisa = pesquisa_new("1");

           


            if (ver_pesquisa == false)
            {


                List<string> mp_select = new List<string>();
                DataSet dadosXML_1 = new DataSet();
                dadosXML_1.ReadXml(@"X:\Xml\Lista_Familia\Selecao_Material_Geral.xml");

                foreach (DataRow dRow in dadosXML_1.Tables["Fam"].Rows)
                {
                    if (dRow["familia"].ToString() == cod_fam)
                    {
                        mp_select.Add(dRow["codigo"].ToString());
                    }
                }

                if (mp_select.Count > 0)
                {
                    frm_selecionar_mp abrir = new frm_selecionar_mp();
                    abrir.lista_mp = mp_select;
                    //string mp_sel = 
                    abrir.ShowDialog();

                    string mp_ret = abrir.MP_returnada;
                    if (mp_ret != "")
                    {
                        mp_ret = mp_ret + "|BibliotecaMascarello.xml";
                        LogMessage_fam("Criando o item....aguarde");

                        string[] _expressao = new string[expressao.Length - 1];
                        for (int i = 0; i < expressao.Length - 1; i++)
                        {
                            _expressao[i] = expressao[i];
                        }
                        double[] _valor = new double[valor.Length - 1];
                        for (int i = 0; i < valor.Length - 1; i++)
                        {
                            _valor[i] = valor[i];
                        }

                        (codigo_novo, existe) = Custom_Mascarello.Create.CreateMember_new(cod_fam, descricao_final, _expressao, _valor, 0, tolerancia, "", descricao_final, "M4_it_mascarello", mp_ret, false, null, null);

                        List<object> rowData = new List<object>
                        {
                            codigo_novo.Substring(0,6),
                            descricao_final,
                            descricao_final,
                            "M4_it_mascarello",
                            "000"
                        };
                        rowData.AddRange(_valor.Cast<object>());
                        rowData.Add(mp_ret);
                        int rowIndex = dtg_resultado.Rows.Add(rowData.ToArray());
                        dtg_resultado.Rows[rowIndex].Selected = true;
                        dtg_resultado.FirstDisplayedScrollingRowIndex = rowIndex;
                        atualizacao_fam_2312.gerar_pdf_criacao_familia(codigo_novo.Substring(0, 6), "000");

                        DialogResult result = MessageBox.Show("Deseje criar o Fluxo de Liberação e enviar o item para o PROCESSO?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            conexao_bd conexao = new conexao_bd();
                            conexao.insert_wf_auto(codigo_novo.Substring(0, 6), "000");
                        }
                        cadastrar_atributos_ge_fam(codigo_novo.Substring(0, 6) + "/000", cod_fam);
                        LogMessage_fam($"Item {codigo_novo} criado e enviado para o Fluxo de Liberação");
                    }
                    else
                    {

                        LogMessage_fam($"Item não criado....MP não especificada");
                    }
                }
                else
                {

                    //  this.lbl_inf.Text = "Criando o item....";
                    LogMessage_fam("Criando o item....aguarde");
                    bool boletim = false;

                    lista_boletins.Items.Clear();
                    List<string> listaPintura = new List<string>();
                    List<string> listaSelecionados = new List<string>();

                    string ver_boletim = conexao_bd.verifica_boletim_fam(cmb_familia_pc.Text.Substring(0, 6));

                    if (ver_boletim != "" && attr_pintura == true)
                    {

                        boletim = true;
                        (listaSelecionados, listaPintura) = insert_boletin_fam_sincrona(ver_boletim);
                        descricao_final = descricao_final.Remove(descricao_final.Length - 1, 1);
                        descricao_final = descricao_final + " " + "_C/_PINTURA";

                    }
                    if (ver_boletim != "" && attr_pintura == false)
                    {
                        descricao_final = descricao_final.Remove(descricao_final.Length - 1, 1);
                    }

                    // string atsss = string.Join(";", attributos);
                    //// MessageBox.Show(atsss);
                    // string atssss = string.Join(";", valor);
                    // MessageBox.Show(atssss);

                    (codigo_novo, existe) = Custom_Mascarello.Create.CreateMember_new(cod_fam, descricao_final, expressao, valor, 0, tolerancia, "", descricao_final, "M4_it_mascarello", mp, boletim, listaSelecionados, listaPintura);

                    if (boletim == true)
                    {
                        registro_boletim_fam(listaSelecionados, listaPintura, codigo_novo.Substring(0, 6), "000");
                    }

                    List<object> rowData = new List<object>
                        {
                            codigo_novo.Substring(0,6),
                            descricao_final,
                            descricao_final,
                            "M4_it_mascarello",
                            "000"
                        };
                    rowData.AddRange(valor.Cast<object>());
                    if(mp !="")
                    {
                        rowData.Add(mp);
                    }
                    int rowIndex = dtg_resultado.Rows.Add(rowData.ToArray());
                    dtg_resultado.Rows[rowIndex].Selected = true;
                    dtg_resultado.FirstDisplayedScrollingRowIndex = rowIndex;
                    atualizacao_fam_2312.gerar_pdf_criacao_familia(codigo_novo.Substring(0, 6), "000");

                    DialogResult result = MessageBox.Show("Criar o Fluxo de Liberação e enviar o item para o PROCESSO?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        conexao_bd conexao = new conexao_bd();
                        conexao.insert_wf_auto(codigo_novo.Substring(0, 6), "000");
                    }
                    cadastrar_atributos_ge_fam(codigo_novo.Substring(0, 6) + "/000", cod_fam);

                    LogMessage_fam($"Item {codigo_novo} criado e enviado para o Fluxo de Liberação");


                }
            }
            try
            {
                theSession.Parts.SetDisplay(displayAnterior, true, true, out loadStatus);
            }
            catch
            {


            }

        }

       
      

        private string Create_fam_geral_Sincrona_AutoCAD_create(Dictionary<string, double> _dados)
        {
            LogMessage("Iniciando Geração item fam");
            Session theSession = Session.GetSession();
            PartLoadStatus loadStatus;


            Part displayAnterior = theSession.Parts.Display;

            string cod_fam = _dados["FAM"].ToString();
            string codigo_ds = _dados["COD"].ToString().PadLeft(6, '0');

            lista_atributos.Clear();

            foreach (var item in _dados)
            {
                if (item.Key != "FAM" && item.Key != "COD" && item.Key != "BT" && item.Key != "BTP" && item.Key != "USER")
                {
                    lista_atributos.Add(item.Key);
                }
            }

            string[] attributos = lista_atributos.ToArray();

            string attributos_str = string.Join(";", attributos);
            LogMessage(attributos_str);

            string desc_padrao = conexao_bd.select_desc(cod_fam);

            string[] descricao = desc_padrao.Split(';');

            string[] expressao = new string[attributos.Length];

            for (int i = 0; i <= attributos.Length - 1; i++)
            {
                expressao[i] = attributos[i];

            }
            bool attr_pintura = false;
            string busca_valores = "";
            foreach (string item in attributos)
            {

                if (item == "PINTURA")
                {
                    if (_dados[item] == 1)
                    {
                        attr_pintura = true;
                    }
                }
                busca_valores = busca_valores + _dados[item] + ";";

            }



            busca_valores = busca_valores.Remove(busca_valores.Length - 1, 1);


            double[] valor = new double[attributos.Length];

            for (int i = 0; i <= busca_valores.Split(';').Length - 1; i++)
            {
                valor[i] = Convert.ToDouble(busca_valores.Split(';')[i]);

            }


            string descricao_final = descricao[0];


            for (int i = 0; i <= attributos.Length - 1; i++)
            {

                descricao_final = descricao_final + valor[i] + descricao[i + 1];

            }

            descricao_final = descricao_final.Replace("\"", "");

            double[] tolerancia = new double[attributos.Length];
            for (int i = 0; i <= attributos.Length - 1; i++)
            {
                tolerancia[i] = Convert.ToDouble(0.1);
            }

            string codigo_novo = "";
            bool existe = false;




            this.lbl_inf.Text = "Criando o item....";

            bool boletim = false;

            lista_boletins.Items.Clear();
            List<string> listaPintura = new List<string>();
            List<string> listaSelecionados = new List<string>();

            string ver_boletim = conexao_bd.verifica_boletim_fam(cod_fam);


            if (ver_boletim != "" && attr_pintura == true)
            {
                string bt = _dados["BT"].ToString();
                string btp = _dados["BTP"].ToString();
                boletim = true;
                (listaSelecionados, listaPintura) = insert_boletin_fam_sincrona_auto(bt, btp, "0");
                descricao_final = descricao_final.Remove(descricao_final.Length - 1, 1);
                descricao_final = descricao_final + " " + "_C/_PINTURA";
            }
            if (ver_boletim != "" && attr_pintura == false)
            {
                descricao_final = descricao_final.Remove(descricao_final.Length - 1, 1);
            }

            (codigo_novo, existe) = Custom_Mascarello.Create.CreateMember_new_automatico_create_demanda(cod_fam, descricao_final, expressao, valor, 0, tolerancia, "", descricao_final, "M4_it_mascarello", "", codigo_ds, boletim, listaSelecionados, listaPintura);

            if (boletim == true)
            {
                registro_boletim_fam(listaSelecionados, listaPintura, codigo_novo.Substring(0, 6), "000");
            }
            LogMessage("Código gerado: " + codigo_novo);
            atualizacao_fam_2312.gerar_pdf_criacao_familia(codigo_novo.Substring(0, 6), "000");
            cadastrar_atributos(codigo_novo.Substring(0, 6) + "/000", cod_fam);
            lbl_inf.Text = codigo_novo;

            try
            {
                theSession.Parts.SetDisplay(displayAnterior, true, true, out loadStatus);
            }
            catch
            {

            }
            return codigo_novo.Substring(0, 6);
        }
        private string Create_fam_geral_Sincrona_AutoCAD(Dictionary<string, double> _dados, string user_create)
        {
            LogMessage("Iniciando Geração item fam");
            Session theSession = Session.GetSession();
            PartLoadStatus loadStatus;


            Part displayAnterior = theSession.Parts.Display;

            string cod_fam = _dados["FAM"].ToString();
            string codigo_ds = _dados["COD"].ToString();
            lista_atributos.Clear();

            foreach (var item in _dados)
            {
                if (item.Key != "FAM" && item.Key != "COD" && item.Key != "BT" && item.Key != "BTP" && item.Key != "ACAO")
                {
                    lista_atributos.Add(item.Key);
                }
            }
            string[] attributos = lista_atributos.ToArray();

            string attributos_str = string.Join(";", attributos);
            LogMessage(attributos_str);

            string desc_padrao = conexao_bd.select_desc(cod_fam);
            LogMessage(desc_padrao);

            string[] descricao = desc_padrao.Split(';');

            string[] expressao = new string[attributos.Length];
            for (int i = 0; i <= attributos.Length - 1; i++)
            {
                expressao[i] = attributos[i];
            }
            bool attr_pintura = false;
            string busca_valores = "";
            foreach (string item in attributos)
            {

                if (item == "PINTURA")
                {
                    if (_dados[item] == 1)
                    {
                        attr_pintura = true;
                    }
                }
                busca_valores = busca_valores + _dados[item] + ";";

            }



            busca_valores = busca_valores.Remove(busca_valores.Length - 1, 1);


            double[] valor = new double[attributos.Length];

            for (int i = 0; i <= busca_valores.Split(';').Length - 1; i++)
            {
                valor[i] = Convert.ToDouble(busca_valores.Split(';')[i]);

            }


            string descricao_final = descricao[0];
           
            LogMessage(descricao.Length.ToString());
            LogMessage(valor.Length.ToString());
            for (int i = 0; i <= attributos.Length - 1; i++)
            {
               // MessageBox.Show(attributos[i] + " "+valor[i] + "   " + descricao[i + 1]);
                descricao_final = descricao_final + valor[i] + descricao[i + 1];
                
            }
            string desc_attibuto = "";
            if (attributos[attributos.Length-1] =="COR" || attributos[attributos.Length - 1] == "LADO" || attributos[attributos.Length - 1] == "ABA" )
            {
                
                string jsondata = conexao_bd.busca_atributos_json(cod_fam);

                Atributos jsonData = JsonConvert.DeserializeObject<Atributos>(jsondata);

               
              
                if (jsonData != null)
                {
                    foreach (var item in jsonData.Lista_atributos)
                    {
                      

                        foreach (var val in item.Valor)
                        {
                            if (val.Substring(0, 1) == valor[attributos.Length - 1].ToString())
                            {
                                string[] quebra = val.Split('-');
                                desc_attibuto = quebra[2];
                            }
                        }

                    }
                }
            }
           // descricao_final = descricao_final.Substring(0, descricao_final.Length - 1);
            if (desc_attibuto != "")
            {
                descricao_final = descricao_final.Remove(descricao_final.Length-1,1) + desc_attibuto;
            }


            LogMessage(descricao_final);
            string jsonString = conexao_bd.busca_subfam_json(cod_fam);

            if (jsonString != "")
            {
                List<double> lista_valores_cad = new List<double>();
                foreach (double val in valor)
                {
                    lista_valores_cad.Add(val);
                }
               
                if(desc_attibuto != "")
                {
                    lista_valores_cad.RemoveAt(lista_valores_cad.Count - 1);
                }
                string teste = string.Join(";", lista_valores_cad);
                LogMessage(teste);
                sub_fam(jsonString, lista_valores_cad,"");
            }
           
            descricao_final = descricao_final.Replace("\"", "");

            double[] tolerancia = new double[attributos.Length];
            for (int i = 0; i <= attributos.Length - 1; i++)
            {
                tolerancia[i] = Convert.ToDouble(0.1);
            }

            string codigo_novo = "";
            bool existe = false;
           
        
            string[] mp_auto = conexao_bd.busca_mp_fam(cod_fam).Split(';');
            string attDefMP = "";
            int posicao = 0;
            string mp = "";
           
            if (mp_auto.Length > 1)
            {
                attDefMP = mp_auto[mp_auto.Length - 1];
                for (int i = 0; i <= attributos.Length - 1; i++)
                {
                    if (attributos[i].Trim() == attDefMP.Trim())
                    {
                        posicao = i;
                    }
                }
                double valor_attDefMP = valor[posicao];
                mp = mp_auto[Convert.ToInt32(valor_attDefMP)];


                //var lista = expressao.ToList();
                //lista.RemoveAt(lista.Count - 1);
                //expressao = lista.ToArray();

                //var lista_valor = valor.ToList();
                //lista_valor.RemoveAt(lista_valor.Count - 1);
                //valor = lista_valor.ToArray();
                LogMessage($"MP que será usada {mp}");
            }
            this.lbl_inf.Text = "Criando o item....";

            bool boletim = false;

            lista_boletins.Items.Clear();
            List<string> listaPintura = new List<string>();
            List<string> listaSelecionados = new List<string>();

            string ver_boletim = conexao_bd.verifica_boletim_fam(cod_fam);


            if (ver_boletim != "" && attr_pintura == true)
            {
                string bt = _dados["BT"].ToString();
                string btp = _dados["BTP"].ToString();
                boletim = true;
                (listaSelecionados, listaPintura) = insert_boletin_fam_sincrona_auto(bt, btp, "0");
                descricao_final = descricao_final.Remove(descricao_final.Length - 1, 1);
                descricao_final = descricao_final + " " + "_C/_PINTURA";
            }
            if (ver_boletim != "" && attr_pintura == false)
            {
              //  descricao_final = descricao_final.Remove(descricao_final.Length - 1, 1);
            }

             ;


            (codigo_novo, existe) = Custom_Mascarello.Create.CreateMember_new_automatico(cod_fam, descricao_final, expressao, valor, 0, tolerancia, "", descricao_final, "M4_it_mascarello",mp, codigo_ds, boletim, listaSelecionados, listaPintura);

            if (boletim == true)
            {
                registro_boletim_fam(listaSelecionados, listaPintura, codigo_novo.Substring(0, 6), "000");
            }
            if (existe == false)
            {
                string[] args = { };
                LogMessage($"alterado usuário para {user_create.ToLower()}");

                string MSG = TC_SOA_TEAMCENTER.Principal.Main(args, codigo_novo.Substring(0, 6), "000", user_create.ToLower());
              
                //LogMessage($"{MSG} para {user_create.ToLower()}");
                cadastrar_atributos_ge_fam(codigo_novo.Substring(0, 6) + "/000", cod_fam);

                System.Threading.Thread.Sleep(3000);
                atualizacao_fam_2312.gerar_pdf_criacao_familia(codigo_novo.Substring(0, 6), "000");
            }
            lbl_inf.Text = codigo_novo;

            try
            {
                theSession.Parts.SetDisplay(displayAnterior, true, true, out loadStatus);
            }
            catch
            {

            }
            return codigo_novo.Substring(0, 6);
        }
        private void Create_fam_cv_2312()
        {
            Session theSession = Session.GetSession();
            PartLoadStatus loadStatus;


            Part displayAnterior = theSession.Parts.Display;
            bool cnc = false;
            double COMP = 0.0;
            double ANG_A_V = 0.0;
            double ANG_B_V = 0.0;
            double ANG_A_H = 0.0;
            double ANG_B_H = 0.0;

            double tol_COMP = 0.0;
            double tol_ANG_A_V = 0.0;
            double tol_ANG_B_V = 0.0;
            double tol_ANG_A_H = 0.0;
            double tol_ANG_B_H = 0.0;
            foreach (Control control in grp_geral.Controls)
            {
                if (control is TextBox)
                {
                    if (control.Name == "COMP")
                    {
                        COMP = Convert.ToDouble(control.Text);
                    }
                    if (control.Name == "ANG_A_V")
                    {
                        ANG_A_V = Convert.ToDouble(control.Text);
                    }
                    if (control.Name == "ANG_B_V")
                    {
                        ANG_B_V = Convert.ToDouble(control.Text);
                    }
                    if (control.Name == "ANG_A_H")
                    {
                        ANG_A_H = Convert.ToDouble(control.Text);
                    }
                    if (control.Name == "ANG_B_H")
                    {
                        ANG_B_H = Convert.ToDouble(control.Text);
                    }
                    if (control.Name == "tol_COMP")
                    {
                        tol_COMP = Convert.ToDouble(control.Text);
                    }
                    if (control.Name == "tol_ANG_A_V")
                    {
                        tol_ANG_A_V = Convert.ToDouble(control.Text);
                    }
                    if (control.Name == "tol_ANG_B_V")
                    {
                        tol_ANG_B_V = Convert.ToDouble(control.Text);
                    }
                    if (control.Name == "tol_ANG_A_H")
                    {
                        tol_ANG_A_H = Convert.ToDouble(control.Text);
                    }
                    if (control.Name == "tol_ANG_B_H")
                    {
                        tol_ANG_B_H = Convert.ToDouble(control.Text);
                    }
                }
            }

            string arquivo = "";
            string cod_fam = "";
            string cod_fam_desc = "";
           
            if ((ANG_A_H <= 68 && ANG_B_H <= 68 && ANG_A_H >= -68 && ANG_B_H >= -68 && ANG_A_V == 0 && ANG_B_V == 0 && COMP <= 3000) ||
              (ANG_A_V <= 68 && ANG_B_V <= 68 && ANG_A_V >= -68 && ANG_B_V >= -68 && ANG_A_H == 0 && ANG_B_H == 0 && COMP <= 3000))
            {

                cnc = true;


                DataSet dados_fam = new DataSet();
                conexao_bd conn = new conexao_bd();
                dados_fam = conn.busca_fam_tubos("FAM_CV_" + cmb_familia_pc.Text);

               

                foreach (DataRow dRow in dados_fam.Tables["tab_fam_nx"].Rows)
                {
                    cod_fam = dRow["fam_cv"].ToString();
                    cod_fam_desc = dRow["codigo"].ToString();

                }


            }
            else
            {

                DataSet dados_fam = new DataSet();
                conexao_bd conn = new conexao_bd();
                dados_fam = conn.busca_fam_tubos("FAM_CV_" + cmb_familia_pc.Text);

                foreach (DataRow dRow in dados_fam.Tables["tab_fam_nx"].Rows)
                {
                    cod_fam = dRow["codigo"].ToString();
                    cod_fam_desc = dRow["codigo"].ToString();
                }

            }




            string item_pai = cod_fam;
            string[] attributos = lista_atributos.ToArray();

            string desc_padrao = conexao_bd.select_desc(cod_fam_desc);

            //MessageBox.Show(desc_padrao);

            string[] descricao = desc_padrao.Split(';');

            string[] expressao = new string[attributos.Length];
            for (int i = 0; i <= attributos.Length - 1; i++)
            {
                expressao[i] = attributos[i];
            }
            string busca_valores = "";
            foreach (string item in attributos)
            {


                foreach (Control txt_valores in grp_geral.Controls)
                {
                    if (txt_valores is TextBox)
                    {

                        if (txt_valores.Text != "" && txt_valores.Name == item)
                        {
                            busca_valores = busca_valores + txt_valores.Text + ";";
                        }
                    }
                }
            }

            busca_valores = busca_valores.Remove(busca_valores.Length - 1, 1);

            double[] valor = new double[attributos.Length];

            for (int i = 0; i <= busca_valores.Split(';').Length - 1; i++)
            {
                valor[i] = Convert.ToDouble(busca_valores.Split(';')[i]);
            }

            string descricao_final = descricao[0];

            for (int i = 0; i <= attributos.Length - 1; i++)
            {
                descricao_final = descricao_final + valor[i] + descricao[i + 1];
            }
            descricao_final = descricao_final.Replace("\"", "");

            double[] tolerancia = new double[attributos.Length];
            for (int i = 0; i <= attributos.Length - 1; i++)
            {
                tolerancia[i] = Convert.ToDouble(0.1);
            }
            string codigo_novo = "";

            bool ver_pesquisa = pesquisa_new("1");

            if (ver_pesquisa == false)
            {
                List<string> mp_select = new List<string>();
                DataSet dadosXML_1 = new DataSet();
                dadosXML_1.ReadXml(@"X:\Xml\Lista_Familia\Selecao_Material_Geral.xml");

                foreach (DataRow dRow in dadosXML_1.Tables["Fam"].Rows)
                {
                    if (dRow["familia"].ToString() == cod_fam)
                    {
                        mp_select.Add(dRow["codigo"].ToString());
                    }
                }
                bool existe;
                string mp_ret = "";
                if (mp_select.Count > 0)
                {
                    frm_selecionar_mp abrir = new frm_selecionar_mp();
                    abrir.lista_mp = mp_select;
                    //string mp_sel = 
                    abrir.ShowDialog();

                    mp_ret = abrir.MP_returnada;
                    LogMessage_fam("Criando o item....aguarde");

                    string item_type = "M4_it_mascarello";
                    if (cnc == true)
                    {
                        item_type = "M4_it_cv";
                        descricao_final = "CV_" + descricao_final;
                        Registrar_no_LOG($"Criar  item {item_type} {cod_fam} {descricao_final} {expressao} {valor} {mp_ret}");
                    }
                    else
                    {
                        Registrar_no_LOG($"Criar  item {item_type} {cod_fam} {descricao_final} {expressao} {valor} {mp_ret}");
                    }

                    (codigo_novo, existe) = Custom_Mascarello.Create.CreateMember_new(cod_fam, descricao_final, expressao, valor, 0, tolerancia, "", descricao_final, item_type, mp_ret, false, null, null);

                    if (existe == false)
                    {
                        codigo_novo = codigo_novo.Substring(0, codigo_novo.Length - 4);
                    }
                    List<object> rowData = new List<object>
                        {
                            codigo_novo,
                            descricao_final,
                            descricao_final,
                            item_type,
                            "000"
                        };
                    rowData.AddRange(valor.Cast<object>());
                    int rowIndex = dtg_resultado.Rows.Add(rowData.ToArray());
                    dtg_resultado.Rows[rowIndex].Selected = true;
                    dtg_resultado.FirstDisplayedScrollingRowIndex = rowIndex;
                    if (cnc == false && existe == false)
                    {
                        atualizacao_fam_2312.gerar_pdf_criacao_familia(codigo_novo.Substring(0, 6), "000");
                        DialogResult result = MessageBox.Show("Deseje criar o Fluxo de Liberação e enviar o item para o PROCESSO?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            conexao_bd conexao = new conexao_bd();
                            conexao.insert_wf_auto(codigo_novo.Substring(0, 6), "000");
                        }
                        cadastrar_atributos_ge_fam(codigo_novo.Substring(0, 6) + "/000", cod_fam);

                        LogMessage_fam($"Item {codigo_novo} criado e enviado para o Fluxo de Liberação");
                    }
                    if (cnc == true && existe == false)
                    {
                        LogMessage_fam($"Item CV {codigo_novo} criado");
                    }

                }
                else
                {

                    LogMessage_fam("Criando o item....aguarde");

                    string item_type = "M4_it_mascarello";

                    if (cnc == true)
                    {
                        item_type = "M4_it_cv";
                        descricao_final = "CV_" + descricao_final;
                        Registrar_no_LOG($"Criar  item {item_type} {cod_fam} {descricao_final} {expressao} {valor} ");
                    }
                    else
                    {
                        Registrar_no_LOG($"Criar  item {item_type} {cod_fam} {descricao_final} {expressao} {valor} ");
                    }

                    (codigo_novo, existe) = Custom_Mascarello.Create.CreateMember_new(cod_fam, descricao_final, expressao, valor, 0, tolerancia, "", descricao_final, item_type, "", false, null, null);


                    List<object> rowData = new List<object>
                        {
                            codigo_novo,
                            descricao_final,
                            descricao_final,
                            item_type,
                            "000"
                        };
                    rowData.AddRange(valor.Cast<object>());
                    int rowIndex = dtg_resultado.Rows.Add(rowData.ToArray());
                    dtg_resultado.Rows[rowIndex].Selected = true;
                    dtg_resultado.FirstDisplayedScrollingRowIndex = rowIndex;
                    if (cnc == false && existe == false)
                    {
                        atualizacao_fam_2312.gerar_pdf_criacao_familia(codigo_novo.Substring(0, 6), "000");
                        DialogResult result = MessageBox.Show("Deseje criar o Fluxo de Liberação e enviar o item para o PROCESSO?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            conexao_bd conexao = new conexao_bd();
                            conexao.insert_wf_auto(codigo_novo.Substring(0, 6), "000");
                        }

                        cadastrar_atributos_ge_fam(codigo_novo.Substring(0, 6) + "/000", cod_fam);

                        LogMessage_fam($"Item {codigo_novo} criado e enviado para o Fluxo de Liberação");
                    }
                    if (cnc == true && existe == false)
                    {
                        LogMessage_fam($"Item CV {codigo_novo} criado");
                    }
                    if (cnc == true && existe == true)
                    {
                        LogMessage_fam($"Item CV {codigo_novo} existente");
                    }
                }
            }
            try
            {
                theSession.Parts.SetDisplay(displayAnterior, true, true, out loadStatus);
            }
            catch
            {


            }
        }

        private void btn_criar_Click(object sender, EventArgs e)
        {

        }
        private void btn_pesquisar_fam_pc_Click(object sender, EventArgs e)
        {
            bool _pesquisa = pesquisa_new("0");
        }
        private bool pesquisa_new(string tipo)
        {
            bool _pesquisa = false;
            dtg_resultado.ClearSelection();
            var parametrosDeBusca = new Dictionary<string, (double valor, double tolerancia)>();
            var parametrosDeBusca_invert = new Dictionary<string, (double valor, double tolerancia)>();

            List<TextBox> list_txtbox = new List<TextBox>();
            string valor_cmb = "";
            string valor_cmb_name = "";


            foreach (Control txt_valores in grp_geral.Controls)
            {
                if (txt_valores is TextBox)
                {

                    if (txt_valores.Text == "")
                    {
                        txt_valores.TextChanged -= TextBox1_TextChanged;
                        txt_valores.Text = "0";

                    }
                }
            }
            foreach (var item in lista_atributos)
            {
                foreach (Control txt_valores in grp_geral.Controls)
                {
                    if (txt_valores is TextBox)
                    {
                        //   MessageBox.Show(txt_valores.Name);
                        if (txt_valores.Text != "" && txt_valores.Name == item || txt_valores.Name.Substring(3, txt_valores.Name.Length - 3) == item)
                        {
                            list_txtbox.Add((TextBox)txt_valores);
                        }

                    }
                    if (txt_valores is ComboBox)
                    {
                        if (txt_valores.Text != "" && txt_valores.Name == item || txt_valores.Name.Substring(3, txt_valores.Name.Length - 3) == item)
                        {
                            string[] valor = txt_valores.Text.Split('-');
                            valor_cmb = valor_cmb + valor[0].TrimEnd() + ";";
                            valor_cmb_name = valor_cmb_name+ txt_valores.Name + ";";
                        }
                    }

                }
            }
            
            foreach (string item in lista_atributos)
            {
                foreach (var txtbox in list_txtbox)
                {
                    if (txtbox.Name == item)
                    {
                        double tol = 0.1;
                        foreach (var txtboxTol in list_txtbox)
                        {
                            if (txtboxTol.Name.Remove(0, 3) == item)
                            {
                                tol = double.Parse(txtboxTol.Text);
                            }
                        }
                        parametrosDeBusca.Add(txtbox.Name, (double.Parse(txtbox.Text), tol));
                    }
                }
            }
            if (valor_cmb != "")
            {
                valor_cmb = valor_cmb.Remove(valor_cmb.Length - 1, 1);
                for (int i = 0; i < valor_cmb.Split(';').Length; i++)
                {
                    parametrosDeBusca.Add(valor_cmb_name.Split(';')[i], (double.Parse(valor_cmb.Split(';')[i]), 0.0));

                }
            }
           
            string resultado = string.Empty;
            List<string> valoresResultado = new List<string>();
            valoresResultado.Clear();
            DataTable tabela = DataFamily.Tables["Familia"];
            foreach (DataRow row in tabela.Rows)
            {
                bool coincidencia = true;

                // Iterar apenas nas colunas a partir do índice 4 (5ª coluna, já que o índice começa em 0)
                for (int i = 5; i < tabela.Columns.Count; i++)
                {
                    string nomeColuna = tabela.Columns[i].ColumnName;

                    if (parametrosDeBusca.ContainsKey(nomeColuna) &&
                        double.TryParse(row[nomeColuna].ToString(), out double valorAtual))
                    {
                        var (valorBusca, tol) = parametrosDeBusca[nomeColuna];
                        double minValor = valorBusca - tol;
                        double maxValor = valorBusca + tol;

                        // Se o valor estiver fora da tolerância, marca como não coincidente
                        if (valorAtual < minValor || valorAtual > maxValor)
                        {
                            coincidencia = false;

                        }
                    }
                }

                // Se todos os parâmetros coincidirem, obtém o valor da primeira coluna
                if (coincidencia)
                {
                    resultado = row[0].ToString();
                    valoresResultado.Add(resultado);
                    _pesquisa = true;
                }
            }


            if (valoresResultado.Count == 0)
            {
                if (tipo == "0")
                {
                    MessageBox.Show("Nenhum item encontrado com os parâmetros informados.", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                _pesquisa = false;
            }

            int total = 0;
            foreach (DataGridViewRow row in dtg_resultado.Rows)
            {
                if (row.Cells[0].Value != null &&
                    valoresResultado.Contains(row.Cells[0].Value.ToString()))
                {
                    row.Selected = true;
                    if (dtg_resultado.FirstDisplayedScrollingRowIndex == -1)
                        dtg_resultado.FirstDisplayedScrollingRowIndex = row.Index;

                    DataGridViewRow selectedRow = dtg_resultado.SelectedRows[0];

                    // Clona a linha e adiciona no topo
                    int index = selectedRow.Index;
                    dtg_resultado.Rows.RemoveAt(index);
                    dtg_resultado.Rows.Insert(0, selectedRow);

                    // Seleciona novamente a linha movida
                    dtg_resultado.Rows[0].Selected = true;
                    dtg_resultado.CurrentCell = dtg_resultado.Rows[0].Cells[0];
                    total++;
                }
            }
            if (total > 0)
            {
                if (total > 1)
                {
                    lbl_inf.Text = $"{total.ToString()} itens encontrados";
                }
                if (total == 1)
                {
                    lbl_inf.Text = $"{total.ToString()} item encontrado";
                }
            }

            foreach (string item in valoresResultado)
            {
                foreach (DataGridViewRow row in dtg_resultado.Rows)
                {
                    if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == item)
                    {
                        row.Selected = true;
                    }
                }
            }
            return _pesquisa;
        }
        private void btn_lista_fam_pc_Click(object sender, EventArgs e)
        {
            //  Create_Tubos.Lista_Membro_da_Familia("212960");
            //lbl_inf.Text = "";
            //if (cmb_tipo_mp.Text == "Tubos")
            //{

            //    pesquisa = "1";
            //    btn_pesquisa_pc = true;
            //    Create_fam_tubos();


            //    for (int i = 0; i < Resultado_Listar.Count; i++)
            //    {

            //        string[] quebra = Resultado_Listar[i].Split(';');

            //        dtg_resultado.Rows.Add(quebra[0], quebra[1], quebra[2], quebra[3], quebra[4], quebra[5]);
            //        //   dtg_resultado.Rows[i].
            //    }
            //    pesquisa = "0";
            //    btn_pesquisa_pc = false;
            //}
            //if (cmb_tipo_mp.Text == "Chapas")
            //{


            //    pesquisa = "1";
            //    btn_pesquisa_pc = true;
            //    Create_fam_chapas();

            //    for (int i = 0; i < Resultado_Listar.Count; i++)
            //    {
            //        string[] quebra = Resultado_Listar[i].Split(';');

            //        dtg_resultado.Rows.Add(quebra[0], quebra[1], quebra[2]);

            //    }
            //    pesquisa = "0";
            //    btn_pesquisa_pc = false;
            //}
            ////if (cmb_tipo_mp.Text == "Base" && cmb_familia_pc.Text == "PERFIL C")
            ////{
            ////    string tipo_perfil = cmb_tipo_perfil.Text + cmb_larg_perfil.Text;
            ////    string codigo_fam = "";
            ////    pesquisa = "1";
            ////    btn_pesquisa_pc = true;
            ////    dtg_resultado.Rows.Clear();
            ////    Create_fam_perfil_c_base(tipo_perfil);
            ////    pesquisa = "0";
            ////    btn_pesquisa_pc = false;
            ////    if (tipo_perfil == "A50" || tipo_perfil == "A100")
            ////    {
            ////        codigo_fam = "213592";
            ////        if (tipo_perfil == "A100")
            ////        {
            ////            codigo_fam = "213593";
            ////        }

            ////        pesquisa = "1";
            ////        btn_pesquisa_pc = true;
            ////        Create_fam_perfil_c_base(codigo_fam);

            ////        for (int i = 0; i < Resultado_Listar.Count; i++)
            ////        {
            ////            string[] quebra = Resultado_Listar[i].Split(';');

            ////            dtg_resultado.Rows.Add(quebra[0], "", "", quebra[1], quebra[2], quebra[3], quebra[4], quebra[5]);

            ////        }
            ////        pesquisa = "0";
            ////        btn_pesquisa_pc = false;
            ////    }

            ////    if (tipo_perfil == "B50" || tipo_perfil == "B100")
            ////    {
            ////        codigo_fam = "213574";
            ////        if (tipo_perfil == "B100")
            ////        {
            ////            codigo_fam = "213578";
            ////        }


            ////        pesquisa = "1";
            ////        btn_pesquisa_pc = true;
            ////        Create_fam_perfil_c_base(codigo_fam);

            ////        for (int i = 0; i < Resultado_Listar.Count; i++)
            ////        {
            ////            string[] quebra = Resultado_Listar[i].Split(';');

            ////            dtg_resultado.Rows.Add(quebra[0], "", "", quebra[1], quebra[2], quebra[3], "", "");

            ////        }
            ////        pesquisa = "0";
            ////        btn_pesquisa_pc = false;
            ////    }

            ////    if (tipo_perfil == "C50" || tipo_perfil == "C100")
            ////    {
            ////        codigo_fam = "213584";
            ////        if (tipo_perfil == "C100")
            ////        {
            ////            codigo_fam = "213582";
            ////        }


            ////        pesquisa = "1";
            ////        btn_pesquisa_pc = true;
            ////        Create_fam_perfil_c_base(codigo_fam);

            ////        for (int i = 0; i < Resultado_Listar.Count; i++)
            ////        {
            ////            string[] quebra = Resultado_Listar[i].Split(';');
            ////            dtg_resultado.Rows.Add(quebra[0], "", "", quebra[1], "", "", "", "");
            ////        }
            ////        pesquisa = "0";
            ////        btn_pesquisa_pc = false;
            ////    }
            ////}
            //if (cmb_familia_pc.Text != "PERFIL C" && cmb_tipo_mp.Text != "Tubos" && cmb_tipo_mp.Text != "Chapas")
            //{
            //    pesquisa = "1";
            //    btn_pesquisa_pc = true;
            //    Create_fam_geral();
            //    pesquisa = "0";
            //    btn_pesquisa_pc = false;
            //}
        }
        
        private void btn_criar_fam_pc_new_Click(object sender, EventArgs e)
        {
            if (cmb_tipo_mp.Text == "Tubos CV")
            {
                Create_fam_cv_2312();
            }
            if (cmb_tipo_mp.Text != "Tubos CV" && cmb_tipo_mp.Text != "Conjunto_X")
            {

                // Create_fam_geral_newAsync();
                Create_fam_geral_Sincrona();
            }
        }
        private void Create_fam_cv(string familia, double comprimento, double ANG_A_H, double ANG_B_H)
        {
            bool cnc = true;
            this.lbl_inf.Text = "";
            string arquivo = "";
            string cod_fam = familia;
            string cod_fam_desc = "";
            cod_fam_desc = cod_fam;
            // cmb_familia_pc.Text.Substring(0, 6);
            string comp_tubo = comprimento.ToString();
            double Comprimento = comprimento;
            double ANG_A_V = 0.0;
            double ANG_B_V = 0.0;



            if (((ANG_A_H >= 0 || ANG_A_H <= 0) && ANG_A_H <= 68 && (ANG_B_H >= 0 || ANG_B_H <= 0) && ANG_B_H <= 68 && ANG_A_V == 0 && ANG_B_V == 0 && Comprimento <= 3000) ||
                 ((ANG_A_V >= 0 || ANG_A_V <= 0) && ANG_A_V <= 68 && (ANG_B_V >= 0 || ANG_B_V <= 0) && ANG_B_V <= 68 && ANG_A_H == 0 && ANG_B_H == 0 && Comprimento <= 3000))
            {

                cnc = true;

                for (int i = 0; i <= gerar_tubo_cv_dim.Count - 1; i++)
                {
                    if (gerar_tubo_cv_dim[i] == cmb_familia_pc.Text)
                    {
                        cod_fam = gerar_tubo_cv_fam_cv[i];
                        cod_fam_desc = gerar_tubo_cv_fam_item[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i <= gerar_tubo_cv_dim.Count - 1; i++)
                {
                    if (gerar_tubo_cv_dim[i] == cmb_familia_pc.Text)
                    {
                        cod_fam = gerar_tubo_cv_fam_item[i];
                        cod_fam_desc = cod_fam;
                    }
                }
            }
            string desc_1 = "";
            string desc_2 = "";
            string desc_3 = "";
            string desc_4 = "";


            desc_1 = "_AV" + ANG_A_V.ToString() + "°";
            desc_2 = "_BV" + ANG_B_V.ToString() + "°";
            desc_3 = "_AH" + ANG_A_H.ToString() + "°";
            desc_4 = "_BH" + ANG_B_H.ToString() + "°";

            string Material = "";
            string busca_descricao = Custom_Mascarello.Search_Material_Tubos.Busca_Descricao("301778");

            string[] expressao = { "COMP", "ANG_A_V", "ANG_B_V", "ANG_A_H", "ANG_B_H" };
            string descricao = busca_descricao + "x" + comp_tubo + "mm" + desc_1 + desc_2 + desc_3 + desc_4;

            double tol_comp = 0.0;
            double tol_av = 0.0;
            double tol_bv = 0.0;
            double tol_ah = 0.0;
            double tol_bh = 0.0;

            if (Convert.ToDouble(Num_Tol_Comp_tubo.Value) == 0)
            {
                tol_comp = 0.1;
            }
            if (Convert.ToDouble(Num_Tol_AV.Value) == 0)
            {
                tol_av = 0.1;
            }
            if (Convert.ToDouble(Num_Tol_BV.Value) == 0)
            {
                tol_bv = 0.1;
            }
            if (Convert.ToDouble(Num_Tol_AH.Value) == 0)
            {
                tol_ah = 0.1;
            }
            if (Convert.ToDouble(Num_Tol_BH.Value) == 0)
            {
                tol_bh = 0.1;
            }
            double[] valor = { Comprimento, ANG_A_V, ANG_B_V, ANG_A_H, ANG_B_H };
            double[] tolerancia = { tol_comp, tol_av, tol_bv, tol_ah, tol_bh };

            string DataSul = "";
            //   txt_codigo_pc.ResetText();

            dtg_resultado.Columns.Clear();
            dtg_resultado.Columns.Add("CODIGO", "CODIGO");
            for (int i = 0; i <= expressao.Length - 1; i++)
            {
                dtg_resultado.Columns.Add(expressao[i], expressao[i]);//Acrescenta colunas
            }

            if (pesquisa == "0")
            {
                if (cnc == true)
                {
                    descricao = "CV_" + descricao;
                    arquivo = Custom_Mascarello.Search_Tubos_cnc.SearchMember(cod_fam, descricao, expressao, valor, 0, tolerancia, "", descricao);
                }
                if (arquivo == "")
                {
                    this.lbl_inf.Text = "Criando o item....";
                    // this.txt_Cod_DataSul.Text = "";
                    if (cnc == false)
                    {
                        //frm_codigo_datasul frm = new frm_codigo_datasul(descricao);
                        //frm.ShowDialog();
                        //DataSul = frm.Codigo;
                        arquivo = Custom_Mascarello.Create_Tubos.CreateMember(cod_fam, descricao, expressao, valor, 0, tolerancia, DataSul, descricao, Material);
                        //   string ok_save = Save_Fam(cod_fam);
                    }
                    if (cnc == true)
                    {
                        arquivo = Custom_Mascarello.Create_Tubos_cnc.CreateMember(cod_fam, descricao, expressao, valor, 0, tolerancia, DataSul, descricao);
                        string ok_save = Save_Fam(cod_fam);
                    }
                    this.lbl_inf.Text = "Item criado";


                    dtg_resultado.Rows.Add(arquivo, COMP.Text, this.ANG_A_V.Text, this.ANG_B_V.Text, this.ANG_A_H.Text, this.ANG_B_H.Text);
                    arquivo = "";
                }

                if (arquivo != "")
                {

                    for (int i = 0; i < Resultado_Listar.Count; i++)
                    {
                        string[] quebra = Resultado_Listar[i].Split(';');
                        dtg_resultado.Rows.Add(quebra[0], quebra[1], quebra[2], quebra[3], quebra[4], quebra[5]);
                    }
                    this.lbl_inf.Text = "Item já existe";
                }
            }
            if (pesquisa == "1" && btn_pesquisa_pc == true)
            {
                descricao = "CV_" + descricao;
                arquivo = Custom_Mascarello.Search_Tubos_cnc.SearchMember(cod_fam, descricao, expressao, valor, 0, tolerancia, "", descricao);
                if (arquivo == "")
                {
                    lbl_informacao = "Não existe item com os parametros solicitados!";
                    this.lbl_inf.Text = lbl_informacao;
                }
            }
        }
        private void btn_acess_Config_Projeto_Click(object sender, EventArgs e)
        {
            frm_Configurador_S4 abrir = new frm_Configurador_S4();
            abrir.Show();
            this.Close();
        }
        private void cmb_tipo_mp_SelectedIndexChanged(object sender, EventArgs e)
        {
            gerar_tubo_cv_dim.Clear();
            gerar_tubo_cv_fam_cv.Clear();
            gerar_tubo_cv_fam_item.Clear();

            cmb_familia_pc.Items.Clear();
            cmb_familia_pc.Text = "";
            conexao_bd conexao = new conexao_bd();
            DataSet _familias = conexao.busca_fam(cmb_tipo_mp.Text);

            foreach (DataRow dRow in _familias.Tables["tab_fam_nx"].Rows)
            {
                if (cmb_tipo_mp.Text != "Tubos CV")
                {
                    cmb_familia_pc.Items.Add(dRow["codigo"].ToString().PadLeft(6, '0') + " - " + dRow["descricao"].ToString());
                }
                else
                {
                    string fam_cv = dRow["fam_cv"].ToString().Remove(0, 7);
                    cmb_familia_pc.Items.Add(fam_cv);

                    string[] _dim = dRow["fam_cv"].ToString().Split('_');
                    gerar_tubo_cv_dim.Add(_dim[2]);
                    gerar_tubo_cv_fam_cv.Add(dRow["fam_cv"].ToString());
                    gerar_tubo_cv_fam_item.Add(dRow["codigo"].ToString());
                }
            }
            cmb_familia_pc.Sorted = true;

            if (cmb_tipo_mp.Text == "Tubos CV")
            {
                picbox_fams.Image = Properties.Resources.FAM_TB;
                Var_fam_tubos.Visible = true;
                Var_fam_chapas.Visible = false;
                grp_geral.Visible = false;
                COMP.Multiline = false;
                ANG_A_H.Multiline = false;
                ANG_B_H.Multiline = false;
                ANG_A_V.Multiline = false;
                ANG_B_V.Multiline = false;
                ANG_A_H.Text = "0";
                ANG_B_H.Text = "0";
                ANG_A_V.Text = "0";
                ANG_B_V.Text = "0";
                if (ckd_vao.Checked == true)
                {
                    COMP.ReadOnly = true;
                    ANG_A_V.ReadOnly = true;
                    ANG_A_H.ReadOnly = true;
                    ANG_B_V.ReadOnly = true;
                    ANG_B_H.ReadOnly = true;

                    txt_larg.Visible = true;
                    txt_alt.Visible = true;
                    txt_folga_inf.Visible = true;
                    txt_folga_sup.Visible = true;
                    txt_alt_tubo.Visible = true;
                    btn_inverter.Visible = true;

                    string[] quebra = cmb_familia_pc.Text.Split('x');
                    txt_alt_tubo.Text = quebra[0];
                    picbox_fams.Image = Properties.Resources.IMG;
                }
            }
            if (cmb_tipo_mp.Text == "Chapa AXB" || cmb_tipo_mp.Text == "Chapa AXB Aluminio")
            {
                picbox_fams.Image = Properties.Resources.FAM_CHAPA;

            }
            //{
            //    picbox_fams.Image = Properties.Resources.FAM_CHAPA;
            //    Var_fam_tubos.Visible = false;
            //    Var_fam_chapas.Visible = true;

            //    grp_geral.Visible = false;
            //    txt_a_chapa.Multiline = false;
            //    txt_b_chapa.Multiline = false;

            //    txt_larg.Visible = false;
            //    txt_alt.Visible = false;
            //    txt_folga_inf.Visible = false;
            //    txt_folga_sup.Visible = false;
            //    txt_alt_tubo.Visible = false;
            //    btn_inverter.Visible = false;


            //}
        }
        private void txt_comp_tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsDigit(e.KeyChar)) && (!char.IsControl(e.KeyChar)) && (!char.IsPunctuation(e.KeyChar)))
            {
                e.Handled = true;
            }
        }
        private void btn_lista_de_corte_Click(object sender, EventArgs e)
        {
            frm_contraventamentos abrir = new frm_contraventamentos();
            abrir.Show();
            this.Close();

        }

        private void btn_registro_Click(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            UFSession theUfSession = UFSession.GetUFSession();

            NXObject[] objects1 = new NXObject[1];
            objects1[0] = workPart;

            string codigo = objects1[0].GetStringAttribute("DB_PART_NO");
            int revisao = Convert.ToInt16(objects1[0].GetStringAttribute("DB_PART_REV"));

            if (revisao > 0)
            {
                frm_motivo_alteracao abrir_mot = new frm_motivo_alteracao();
                this.WindowState = FormWindowState.Minimized;
                abrir_mot.ShowDialog();

                string tipo = abrir_mot.Tipo;
                string desc = abrir_mot.Desc;
                string decisao = abrir_mot.Decisao;

                if (decisao == "1")
                {
                    AttributePropertiesBuilder attributePropertiesBuilder1;
                    attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);
                    attributePropertiesBuilder1.Title = "ALTERACAO";
                    attributePropertiesBuilder1.StringValue = tipo.Substring(0, 1) + "-" + desc;
                    NXObject nXObject1;
                    nXObject1 = attributePropertiesBuilder1.Commit();
                    attributePropertiesBuilder1.Destroy();

                    string data = DateTime.Now.ToString("dd/MM/yyyy");
                    string usuario = Environment.UserName;

                    Registrar_alteracao(codigo + ";" + revisao + ";" + data + ";" + usuario + ";" + tipo.Substring(0, 1) + ";" + desc);
                    System.Diagnostics.Process.Start(@"C:\app_projeto\Sistema_Controle_Alteracao.exe");
                }
            }
            else
            {
                MessageBox.Show("A revisão deve ser diferente de \"000\"", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void Registrar_alteracao(string linha)
        {
            string log = @"C:\\temp\\" + "reg_alt.txt";
            System.IO.StreamWriter vWriter = new System.IO.StreamWriter(log, true);
            vWriter.WriteLine(linha);
            vWriter.Close();
        }
        private void btn_pp_Click(object sender, EventArgs e)
        {
            frm_PortaPacotes abrir = new frm_PortaPacotes();
            abrir.Show();
            this.Close();
        }

        private void btn_acess_Config_Projeto_s3_Click(object sender, EventArgs e)
        {
            frm_Configurador_S3 abrir = new frm_Configurador_S3();
            abrir.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theUfSession = UFSession.GetUFSession();

        }
        public static string TCSearchItem(string item_id, NXOpen.Session theSession, UI theUI, UFSession theUfSession)
        {


            string encodedname = "";

            string[] entries = { "item_id" };
            string[] values = { item_id };

            // Registrar_no_LOG(" DEBUG -> (TCSearchItem) Pesquisando Item:" + item_id);

            NXOpen.PDM.PdmSearch mySearch = theSession.PdmSearchManager.NewPdmSearch();
            NXOpen.PDM.SearchResult mySearchResult = mySearch.Advanced(entries, values);

            string[] results = mySearchResult.GetResultItemIds();

            //   Registrar_no_LOG(" DEBUG -> (TCSearchItem) Itens Encontrados:" + results.Length.ToString());
            if (results.Length > 0)
            {
                // Registrar_no_LOG(" DEBUG -> (TCSearchItem) results[0]:" + results[0]);

                encodedname = GetEncondedPartName(results[0], theSession, theUI, theUfSession);

                ///     Registrar_no_LOG(" DEBUG -> (TCSearchItem) encodedname:" + encodedname);

                return encodedname;

            }

            else return "";
        }
        public static string GetEncondedPartName(string item_id, NXOpen.Session theSession, UI theUI, UFSession theUfSession)
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

        private void btn_Codificar_itens_Click(object sender, EventArgs e)
        {
            string arquivo = Custom_Mascarello.Codificar.Codificar_item("");

            MessageBox.Show("Criação e substutição de Subtemplates finalizada", "INFORMAÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_clean_Click(object sender, EventArgs e)
        {
            DialogResult pergunta = MessageBox.Show("Todos os componentes suprimindos serão exlcuidos do Assembly! \n Deseja continuar?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (pergunta == DialogResult.Yes)
            {
                string arquivo = Custom_Mascarello.Clean.clean_supressed("");
                MessageBox.Show("limpo", "INFORMAÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }




        private void btn_gerenciador_nx_Click(object sender, EventArgs e)
        {

            frm_Gerenciador_NX frm = new frm_Gerenciador_NX();
            this.WindowState = FormWindowState.Minimized;
            frm.Show();
        }

        private void btn_dutos_ac_Click(object sender, EventArgs e)
        {
            frm_Dutos_AC frm = new frm_Dutos_AC();
            this.WindowState = FormWindowState.Minimized;
            frm.Show();
        }

        private void tab_principal_Click(object sender, EventArgs e)
        {

        }

        private void cmb_familia_pc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_tipo_mp.Text != "Tubos CV")
            {
                string cod_fam = "";
                carregar_dados_fam_teamcenter();
                if (ckd_fam_tb_cod.Checked == true)
                {

                    DataSet dados_fam = new DataSet();
                    conexao_bd conn = new conexao_bd();
                    dados_fam = conn.busca_fam_tubos("FAM_CV_" + cmb_familia_pc.Text);

                    foreach (DataRow dRow in dados_fam.Tables["tab_fam_nx"].Rows)
                    {
                        cod_fam = dRow["codigo"].ToString();
                    }
                }
            }
            else
            {
                carregar_dados_fam_cv_teamcenter();
            }
            //if (cmb_tipo_mp.Text == "Tubos")
            //{
            //    string[] quebra = cmb_familia_pc.Text.Split('x');
            //    txt_alt_tubo.Text = quebra[0];
            //    atualizar();
            //}
            //else
            //{
            //    txt_larg.Visible = false;
            //    txt_alt.Visible = false;
            //    txt_folga_inf.Visible = false;
            //    txt_folga_sup.Visible = false;
            //    txt_alt_tubo.Visible = false;
            //    btn_inverter.Visible = false;
            //}
            //if (cmb_tipo_mp.Text == "Base" && cmb_familia_pc.Text == "PERFIL C")
            //{
            //    picbox_fams.Image = Properties.Resources.PERFIL_BASE;

            //    Var_fam_tubos.Visible = false;
            //    Var_fam_chapas.Visible = false;
            //    grp_geral.Visible = false;

            //}
            //if (cmb_familia_pc.Text != "PERFIL C" && cmb_tipo_mp.Text != "Tubos" && cmb_tipo_mp.Text != "Chapas")
            //{
            //    foreach (Control tabcontrol in Controls)
            //    {
            //        if (tabcontrol.Name == "tab_itens")
            //        {
            //            foreach (Control tabpage in tabcontrol.Controls)
            //            {
            //                if (tabpage.Name == "tab_fam")
            //                {

            //                    foreach (Control tabcontrol_1 in tabpage.Controls)
            //                    {
            //                        if (tabcontrol_1.Name == "Tab_fams")
            //                        {

            //                            foreach (Control tabcontrol_4 in tabcontrol_1.Controls)
            //                            {

            //                                if (tabcontrol_4.Name == "tab_pecas")
            //                                {

            //                                    foreach (Control tabcontrol_3 in tabcontrol_4.Controls)
            //                                    {
            //                                        if (tabcontrol_3.Name == "grp_geral")
            //                                        {
            //                                            tabcontrol_3.Controls.Clear();
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }

            //                    }
            //                }
            //            }
            //        }
            //        //if (tabcontrol is System.Windows.Forms.TabControl)
            //        //{
            //        //    foreach (Control tabpage in tabcontrol.Controls)
            //        //    {
            //        //        if (tabpage is TabPage && tabpage.Name == "tab_familias")
            //        //        {
            //        //            foreach (Control tabcontrol_1 in tabpage.Controls)
            //        //            {
            //        //                foreach (Control tabcontrol_2 in tabcontrol_1.Controls)
            //        //                {
            //        //                    if (tabcontrol_2 is TabPage && tabcontrol_2.Name == "tab_pecas")
            //        //                    {
            //        //                        foreach (Control tabcontrol_3 in tabcontrol_2.Controls)
            //        //                        {
            //        //                            if (tabcontrol_3.Name == "grp_geral")
            //        //                            {
            //        //                                    tabcontrol_3.Controls.Clear();
            //        //                            }
            //        //                        }
            //        //                    }                                       
            //        //                }    
            //        //            } 
            //        //        } 
            //        //    }
            //        //}
            //    }


            //    string cod = cmb_familia_pc.Text.Substring(0, 6);

            //    Image image = Image.FromFile(@"X:\Xml\Imagens\" + cod + ".JPG");
            //    this.picbox_fams.Image = image;
            //    Var_fam_tubos.Visible = false;
            //    Var_fam_chapas.Visible = false;
            //    grp_geral.Visible = true;

            //    DataSet dadosXML = new DataSet();
            //    dadosXML.ReadXml(@"X:\Xml\Lista_Familia\Lista_Familias_Geral.xml"); /// ler arquivo xml ( indique o local onde está salvo o xml)
            //    string item_pai = cmb_tipo_mp.Text;


            //    foreach (DataRow dRow in dadosXML.Tables["Fam"].Rows) // ler a tag Fam para carregar os attibutos
            //    {
            //        if (dRow["Codigo"].ToString() == cod) // atributo codigo igual codigo do familia selecionada 
            //        {

            //            string[] attributos = dRow["attr"].ToString().Split(';'); //busca  dos valores dos atributos que faz um array

            //            for (int i = 0; i <= attributos.Length - 1; i++)
            //            {
            //                int pos_y = 25 + (i * 30); ///  aqui está o calculo posição dentro groupbox (descrição da variavel)
            //                string lbl = "aut_lbl_" + attributos[i];
            //                System.Windows.Forms.Label lbl_desc_add = new System.Windows.Forms.Label();
            //                lbl_desc_add.Name = lbl;
            //                lbl_desc_add.Text = attributos[i];
            //                lbl_desc_add.Font = new System.Drawing.Font("Calibri Light", 11);
            //                lbl_desc_add.Location = new System.Drawing.Point(10, pos_y);
            //                lbl_desc_add.Visible = true;
            //                lbl_desc_add.Size = new Size(90, 20); /// posição do label baseado no calculo
            //                lbl_desc_add.TextAlign = ContentAlignment.MiddleRight;
            //                grp_geral.Controls.Add(lbl_desc_add);// adiciona a label ao groupbox( troque o nome grp_geral pelo nome que vc tem criado)

            //                lbl_desc_add.BringToFront();

            //                TextBox txt_att = new TextBox(); /// cria textbox para prenchimento dos valores 
            //                string txt = attributos[i];
            //                txt_att.Name = txt; // nome do textbox igual a ao nome da variavel, para usar na criação ou pesquisa de itens 
            //                txt_att.Multiline = false;
            //                txt_att.Font = new System.Drawing.Font("Calibri Light", 10);
            //                txt_att.Size = new Size(70, 15); // tamanho do textbox
            //                txt_att.Location = new System.Drawing.Point(110, pos_y - 3); /// posição do textbox baseado no calculo
            //                txt_att.Visible = true;

            //                grp_geral.Controls.Add(txt_att); // adiciona a textbox ao groupbox( troque o nome grp_geral pelo nome que vc tem criado)
            //                txt_att.BringToFront();
            //            }
            //        }
            //    }




            //    //  picbox_fams.Image = Properties.Resources._213320;

            //    //object o = Properties.Resources.ResourceManager.GetObject("_213520.jpg");
            //    //MessageBox.Show(o.ToString());
            //    //if (o is Image)
            //    //{
            //    //    picbox_fams.Image = o as Image;

            //    //}
            //    //object O = Properties.Resources.ResourceManager.GetObject("_213520.jpg");


            //    //Image[] images = 

            //    //// string a = "213520"+".jpg";
            //    ////// var bmp = Properties.Resources..ImageName;
            //    //// // Object rm = Properties.Resources.ResourceManager.GetObject(FormStrings.MyImageNames);
            //    //// // Bitmap myImage = (Bitmap)rm.GetObject("myImage");
            //    //// foreach (var item in Properties.Resources)
            //    //// {

            //    //// }

            //    //picbox_fams.Image = (Image)O;
            //}

        }
        public static string Save_Fam(string fam_codigo)
        {
            Session theSession = Session.GetSession();
            Part basePart_atual;
            PartLoadStatus partLoadStatus2;
            try
            {
                basePart_atual = theSession.Parts.Work;
            }
            catch
            {
                basePart_atual = theSession.Parts.Work;
            }
            try
            {
                Part basePart1;
                PartLoadStatus partLoadStatus1 = null;
                basePart1 = (Part)theSession.Parts.FindObject("@DB/" + fam_codigo + "/000");
                basePart1.Save(BasePart.SaveComponents.False, BasePart.CloseAfterSave.True);
            }
            catch
            {
                //Part basePart1;
                //PartLoadStatus partLoadStatus1 = null;
                //basePart1 = (Part)theSession.Parts.FindObject("@DB/" + fam_codigo + "/000");
                //NXOpen.PartCollection.SdpsStatus status1;
                //status1 = theSession.Parts.SetDisplay(basePart1, true, false, out partLoadStatus1);
                //basePart1.Save(BasePart.SaveComponents.False, BasePart.CloseAfterSave.True);
            }
            try
            {
                NXOpen.PartCollection.SdpsStatus status2;
                status2 = theSession.Parts.SetDisplay(basePart_atual, false, true, out partLoadStatus2);
            }
            catch
            {


            }
            string flg = "ok";
            return flg;
        }

        private void ckd_vao_CheckedChanged(object sender, EventArgs e)
        {
            if (ckd_vao.Checked == true)
            {
                COMP.ReadOnly = true;
                ANG_A_V.ReadOnly = true;
                ANG_A_H.ReadOnly = true;
                ANG_B_V.ReadOnly = true;
                ANG_B_H.ReadOnly = true;

                txt_larg.Visible = true;
                txt_alt.Visible = true;
                txt_folga_inf.Visible = true;
                txt_folga_sup.Visible = true;
                txt_alt_tubo.Visible = true;
                btn_inverter.Visible = true;

                string[] quebra = cmb_familia_pc.Text.Split('x');
                txt_alt_tubo.Text = quebra[0];
                picbox_fams.Image = Properties.Resources.IMG;

            }
            else
            {
                COMP.ReadOnly = false;
                ANG_A_V.ReadOnly = false;
                ANG_A_H.ReadOnly = false;
                ANG_B_V.ReadOnly = false;
                ANG_B_H.ReadOnly = false;

                txt_larg.Visible = false;
                txt_alt.Visible = false;
                txt_folga_inf.Visible = false;
                txt_folga_sup.Visible = false;
                txt_alt_tubo.Visible = false;
                btn_inverter.Visible = false;

                picbox_fams.Image = Properties.Resources.FAM_TB;
            }
        }

        private void btn_inverter_Click(object sender, EventArgs e)
        {
            if (inverter == false)
            {
                inverter = true;
                string[] quebra = cmb_familia_pc.Text.Split('x');
                txt_alt_tubo.Text = quebra[1];
                atualizar();
            }
            else
            {
                inverter = false;
                string[] quebra = cmb_familia_pc.Text.Split('x');
                txt_alt_tubo.Text = quebra[0];
                atualizar();
            }
        }

        private void txt_larg_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_larg_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                atualizar();
            }
        }
        private void txt_folga_inf_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                atualizar();
            }
        }

        private void txt_alt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                atualizar();
            }
        }
        private void txt_folga_sup_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                atualizar();
            }
        }

        private void txt_folga_inf_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_larg_Leave(object sender, EventArgs e)
        {

            atualizar();

        }

        private void txt_alt_Leave(object sender, EventArgs e)
        {
            atualizar();
        }
        public void atualizar()
        {
            if (txt_alt.Text != "" && txt_larg.Text != "" && txt_folga_inf.Text != "" && txt_folga_sup.Text != "")
            {
                double hip;
                double altura = Convert.ToDouble(txt_alt.Text);
                double largura = Convert.ToDouble(txt_larg.Text) - Convert.ToDouble(txt_folga_inf.Text) - Convert.ToDouble(txt_folga_sup.Text);
                double alt_tubo = Convert.ToDouble(txt_alt_tubo.Text);

                hip = Math.Sqrt(Math.Pow(altura, 2) + Math.Pow(largura, 2));

                double alfa = (Math.Atan(altura / largura)) * 180 / Math.PI;
                double beta = (Math.Atan(alt_tubo / hip)) * 180 / Math.PI;

                double ang_corte = (90 - (alfa + beta));

                double comp_final = Math.Cos(beta / (180 / Math.PI)) * hip;

                decimal comp_tubo = Convert.ToDecimal(comp_final);
                decimal ang_tubo = Convert.ToDecimal(ang_corte);
                if (inverter == false)
                {
                    foreach (Control control in grp_geral.Controls)
                    {
                        if (control is TextBox)
                        {
                            if (control.Name == "COMP")
                            {
                                control.Text = Convert.ToString(Math.Ceiling(comp_tubo));
                            }
                            if (control.Name == "ANG_A_V")
                            {
                                control.Text = Convert.ToString(Math.Round(ang_tubo));
                            }
                            if (control.Name == "ANG_B_V")
                            {
                                control.Text = Convert.ToString(Math.Round(ang_tubo));
                            }
                            if (control.Name == "ANG_A_H")
                            {
                                control.Text = "0";
                            }
                            if (control.Name == "ANG_B_H")
                            {
                                control.Text = "0";
                            }
                        }
                    }
                }
                else
                {
                    foreach (Control control in grp_geral.Controls)
                    {
                        if (control is TextBox)
                        {
                            if (control.Name == "COMP")
                            {
                                control.Text = Convert.ToString(Math.Ceiling(comp_tubo));
                            }
                            if (control.Name == "ANG_A_V")
                            {
                                control.Text = "0";
                            }
                            if (control.Name == "ANG_B_V")
                            {
                                control.Text = "0";
                            }
                            if (control.Name == "ANG_A_H")
                            {
                                control.Text = Convert.ToString(Math.Round(ang_tubo));
                            }
                            if (control.Name == "ANG_B_H")
                            {
                                control.Text = Convert.ToString(Math.Round(ang_tubo));
                            }
                        }
                    }
                }

            }
        }

        private void txt_folga_inf_Leave(object sender, EventArgs e)
        {
            atualizar();
        }

        private void txt_folga_sup_Leave(object sender, EventArgs e)
        {
            atualizar();
        }

        private void txt_larg_DoubleClick(object sender, EventArgs e)
        {

        }

        private void txt_alt_DoubleClick(object sender, EventArgs e)
        {

        }

        public static void SelectCurves1(ref NXObject[] selectedObjects)
        {


        }

        private void btn_mcc_Click(object sender, EventArgs e)
        {

            frm_MCC abrir = new frm_MCC();
            abrir.Show();
            this.Close();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            foreach (Control tabcontrol in Controls)
            {
                if (tabcontrol is System.Windows.Forms.TabControl)
                {
                    foreach (Control tabpage in tabcontrol.Controls)
                    {
                        if (tabpage is TabPage && tabpage.Name == "tab_familias")
                        {
                            foreach (Control tabcontrol_1 in tabpage.Controls)
                            {
                                foreach (Control tabcontrol_2 in tabcontrol_1.Controls)
                                {
                                    if (tabcontrol_2 is TabPage && tabcontrol_2.Name == "tab_pecas")
                                    {
                                        foreach (Control tabcontrol_3 in tabcontrol_2.Controls)
                                        {
                                            if (tabcontrol_3.Name == "grp_geral")
                                            {
                                                tabcontrol_3.Controls.Clear();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void txt_codigo_pc_Selected(object sender, TabControlEventArgs e)
        {
            //if (txt_codigo_pc.SelectedIndex == 0 && flg == false)
            //{
            //    DataSet dadosXML = new DataSet();
            //    dadosXML.ReadXml(@"X:\Xml\Lista_Familia\Lista_Familias_Geral.xml");
            //    string item_pai = cmb_tipo_mp.Text;


            //    foreach (DataRow dRow in dadosXML.Tables["Class"].Rows)
            //    {
            //         cmb_tipo_mp.Items.Add(dRow["item"].ToString());
            //    }
            //}
        }

        private void tabcontrol_principal_Selected(object sender, TabControlEventArgs e)
        {
            if (tab_itens.SelectedIndex == 1 && flg == false)
            {
                Image image = Image.FromFile(@"X:\Xml\Imagens\IMG.JPG");
                this.picbox_fams.Image = image;

                DataSet dadosXML = new DataSet();
                dadosXML.ReadXml(@"X:\Xml\Lista_Familia\Lista_Familias_Geral.xml");
                string item_pai = cmb_tipo_mp.Text;


                foreach (DataRow dRow in dadosXML.Tables["Class"].Rows)
                {
                    cmb_tipo_mp.Items.Add(dRow["item"].ToString());
                }
                flg = true;
            }
        }

        private void btn_verificar_reg_Click(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            UFSession theUfSession = UFSession.GetUFSession();

            NXObject[] objects1 = new NXObject[1];
            objects1[0] = workPart;

            string codigo = objects1[0].GetStringAttribute("DB_PART_NO");
            int revisao = Convert.ToInt16(objects1[0].GetStringAttribute("DB_PART_REV"));
            string codigo_rev = codigo + "-" + revisao;

            if (revisao > 0)
            {
                string registro_alt = conexao_bd.verifica_alteracao(codigo, revisao.ToString());
                //  bool registro_alt = false;

                if (registro_alt == "1")
                {
                    MessageBox.Show("Alteração registrada.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (registro_alt == "0")
                {
                    MessageBox.Show("Alteração \"NÃO\" registrada.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else
            {
                MessageBox.Show("Revisão 000, não é necessário registro", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {

            if (ckd_2023.Checked == true)
            {
                busca_tampa_2023();
            }
            else
            {


                string[] expressao = { "E_COMPRIMENTO_VAO_DA_BASE", "E_LARGURA_VAO_DA_BASE", "E_TIPO_ASSOALHO", "E_COR_PASSADEIRA", "E_QTD_FICAXAO", "E_VEDACAO", "E_ISOLAMENTO" };
                dtg_tampas.Columns.Clear();
                dtg_tampas.Columns.Add("CODIGO", "CODIGO");
                for (int i = 0; i <= expressao.Length - 1; i++)
                {
                    dtg_tampas.Columns.Add(expressao[i], expressao[i]);//Acrescenta colunas
                }
                double comprimento_vao = Convert.ToDouble(txt_comp_tp.Text);
                double larg_vao = Convert.ToDouble(txt_larg_tp.Text);
                double num_fixacao = Convert.ToDouble(cmb_fixadores.Text);
                double[] valor_expressoes = { comprimento_vao, larg_vao, cmb_assoalho.SelectedIndex + 1, cmb_passadeira.SelectedIndex, num_fixacao, cmb_vedacao.SelectedIndex, cmb_isolamento.SelectedIndex };
                double[] tolerancia = { 0, 0, 0, 0, 0, 0, 0 };
                string codigo_fam = "393397";


                string codigo_ret = Search.CreateMember(codigo_fam, "", expressao, valor_expressoes, 0, tolerancia, "", "");
                if (codigo_ret != "")
                {
                    dtg_tampas.Rows.Add(codigo_ret, comprimento_vao.ToString(), larg_vao, cmb_assoalho.Text, cmb_passadeira.Text, num_fixacao, cmb_vedacao.Text, cmb_isolamento.Text);
                }
                else
                {
                    dtg_tampas.Rows.Add("Item não existe", comprimento_vao.ToString(), larg_vao, cmb_assoalho.Text, cmb_passadeira.Text, num_fixacao, cmb_vedacao.Text, cmb_isolamento.Text);
                    //MessageBox.Show("Item não existe");
                }
            }
        }
        public void busca_tampa_2023()
        {

            string[] expressao = { "E_COMPRIMENTO_VAO_DA_BASE", "E_LARGURA_VAO_DA_BASE", "E_TIPO_ASSOALHO", "E_COR_PASSADEIRA", "E_QTD_FICAXAO", "E_VEDACAO", "E_ISOLAMENTO", "E_TRAVESSA_MOVEL" };
            dtg_tampas.Columns.Clear();
            dtg_tampas.Columns.Add("CODIGO", "CODIGO");
            for (int i = 0; i <= expressao.Length - 1; i++)
            {
                dtg_tampas.Columns.Add(expressao[i], expressao[i]);//Acrescenta colunas
            }
            double comprimento_vao = Convert.ToDouble(txt_comp_tp.Text);
            double larg_vao = Convert.ToDouble(txt_larg_tp.Text);
            double num_fixacao = Convert.ToDouble(cmb_fixadores.Text);
            double[] valor_expressoes = { comprimento_vao, larg_vao, cmb_assoalho.SelectedIndex + 1, cmb_passadeira.SelectedIndex, num_fixacao, cmb_vedacao.SelectedIndex, cmb_isolamento.SelectedIndex, cmb_trav_movel.SelectedIndex };
            double[] tolerancia = { 0, 0, 0, 0, 0, 0, 0, 0 };
            string codigo_fam = "448508";

            string codigo_ret = Search.CreateMember(codigo_fam, "", expressao, valor_expressoes, 0, tolerancia, "", "");
            if (codigo_ret != "")
            {
                dtg_tampas.Rows.Add(codigo_ret, comprimento_vao.ToString(), larg_vao, cmb_assoalho.Text, cmb_passadeira.Text, num_fixacao, cmb_vedacao.Text, cmb_isolamento.Text, cmb_trav_movel.Text);
            }
            else
            {
                dtg_tampas.Rows.Add("Item não existe", comprimento_vao.ToString(), larg_vao, cmb_assoalho.Text, cmb_passadeira.Text, num_fixacao, cmb_vedacao.Text, cmb_isolamento.Text, cmb_trav_movel.Text);
                //MessageBox.Show("Item não existe");
            }
        }


        public void create_tampa(string codigo_fam)
        {

            string fam = codigo_fam;
            bool existe = false;
            string arquivo = "";
            string item_type = "M4_it_mascarello";
            string[] expressao = { "E_COMPRIMENTO_VAO_DA_BASE", "E_LARGURA_VAO_DA_BASE", "E_TIPO_ASSOALHO", "E_COR_PASSADEIRA", "E_QTD_FICAXAO", "E_VEDACAO", "E_ISOLAMENTO" };
            dtg_tampas.Columns.Clear();
            dtg_tampas.Columns.Add("CODIGO", "CODIGO");
            for (int i = 0; i <= expressao.Length - 1; i++)
            {
                dtg_tampas.Columns.Add(expressao[i], expressao[i]);//Acrescenta colunas
            }
            double comprimento_vao = Convert.ToDouble(txt_comp_tp.Text);
            double larg_vao = Convert.ToDouble(txt_larg_tp.Text);
            double num_fixacao = Convert.ToDouble(cmb_fixadores.Text);
            string assoalho = "ASS_ALUM_LAV_";
            string passadeira = "";

            if (cmb_assoalho.SelectedIndex == 1)
            {
                assoalho = "ASS_MAD/ALUM_LAV_VIRADO_";
                if (cmb_passadeira.SelectedIndex == 1)
                {
                    passadeira = "PASSAD_CZ_";
                }
                if (cmb_passadeira.SelectedIndex == 2)
                {
                    passadeira = "PASSAD_MAD_";
                }
                if (cmb_passadeira.SelectedIndex == 3)
                {
                    passadeira = "PASSAD_AZUL_";
                }
                if (cmb_passadeira.SelectedIndex == 4)
                {
                    passadeira = "PASSAD_CZ_IND_";
                }
                if (cmb_passadeira.SelectedIndex == 5)
                {
                    passadeira = "PASSAD_TARABUS_YEL_";
                }
                if (cmb_passadeira.SelectedIndex == 6)
                {
                    passadeira = "PASSAD_TARABUS_HAR_TIJ_";
                }
                if (cmb_passadeira.SelectedIndex == 7)
                {
                    passadeira = "PASSAD_TARABUS_TIJ_YORK_";
                }

                // cmb_passadeira
            }
            string fixadores = num_fixacao.ToString() + "FIX_";
            string ved = "";

            if (cmb_vedacao.SelectedIndex == 1)
            {
                ved = "VED_";

            }
            string isol = "";
            if (cmb_isolamento.SelectedIndex == 1)
            {
                isol = "ISOL_TERM_";

            }
            string trav = "";
            if (cmb_trav_movel.SelectedIndex == 1)
            {
                trav = "_TRAV_MOVEL";
            }

            string descricao = "CJ_TAMPA_INSP_" + assoalho + passadeira + fixadores + ved + isol + comprimento_vao + "x" + larg_vao + "mm";
            double[] valor_expressoes = { comprimento_vao, larg_vao, cmb_assoalho.SelectedIndex + 1, cmb_passadeira.SelectedIndex, num_fixacao, cmb_vedacao.SelectedIndex, cmb_isolamento.SelectedIndex };
            double[] tolerancia = { 0, 0, 0, 0, 0, 0, 0 };

            (arquivo, existe) = Custom_Mascarello.Create.CreateMember_new(fam, descricao, expressao, valor_expressoes, 0, tolerancia, "", descricao, item_type, "", false, null, null);
            //  string arquivo = Custom_Mascarello.Create.CreateMember(fam, descricao, expressao, valor_expressoes, 0, tolerancia, "", descricao);
            // string codigo_ret = Search_Tampa_Inspec.SearchMember("393397", "", expressao, valor_expressoes, 0, tolerancia, "", "");
            if (arquivo != "")
            {
                atualizacao_fam_2312.gerar_pdf_criacao_familia(arquivo.Substring(0, 6), "000");
                dtg_tampas.Rows.Add(arquivo, comprimento_vao.ToString(), larg_vao, cmb_assoalho.Text, cmb_passadeira.Text, num_fixacao, cmb_vedacao.Text, cmb_isolamento.Text, cmb_trav_movel.Text);
            }
            //else
            //{
            //    MessageBox.Show("Item não criado");
            //}
        }
        public void create_tampa_1(string codigo_fam)
        {

            string fam = codigo_fam;
            bool existe = false;
            string arquivo = "";
            string item_type = "M4_it_mascarello";
            string[] expressao = { "E_COMPRIMENTO_VAO_DA_BASE", "E_LARGURA_VAO_DA_BASE", "E_TIPO_ASSOALHO", "E_COR_PASSADEIRA", "E_QTD_FICAXAO", "E_VEDACAO", "E_ISOLAMENTO", "E_TRAVESSA_MOVEL" };
            dtg_tampas.Columns.Clear();
            dtg_tampas.Columns.Add("CODIGO", "CODIGO");
            for (int i = 0; i <= expressao.Length - 1; i++)
            {
                dtg_tampas.Columns.Add(expressao[i], expressao[i]);//Acrescenta colunas
            }
            double comprimento_vao = Convert.ToDouble(txt_comp_tp.Text);
            double larg_vao = Convert.ToDouble(txt_larg_tp.Text);
            double num_fixacao = Convert.ToDouble(cmb_fixadores.Text);
            string assoalho = "ASS_ALUM_LAV_";
            string passadeira = "";

            if (cmb_assoalho.SelectedIndex == 1)
            {
                assoalho = "ASS_MAD/ALUM_LAV_VIRADO_";
                if (cmb_passadeira.SelectedIndex == 1)
                {
                    passadeira = "PASSAD_CZ_";
                }
                if (cmb_passadeira.SelectedIndex == 2)
                {
                    passadeira = "PASSAD_MAD_";
                }
                if (cmb_passadeira.SelectedIndex == 3)
                {
                    passadeira = "PASSAD_AZUL_";
                }
                if (cmb_passadeira.SelectedIndex == 4)
                {
                    passadeira = "PASSAD_CZ_IND_";
                }
                if (cmb_passadeira.SelectedIndex == 5)
                {
                    passadeira = "PASSAD_TARABUS_YEL_";
                }
                if (cmb_passadeira.SelectedIndex == 6)
                {
                    passadeira = "PASSAD_TARABUS_HAR_TIJ_";
                }

                // cmb_passadeira
            }
            string fixadores = num_fixacao.ToString() + "FIX_";
            string ved = "";

            if (cmb_vedacao.SelectedIndex == 1)
            {
                ved = "VED_";

            }
            string isol = "";
            if (cmb_isolamento.SelectedIndex == 1)
            {
                isol = "ISOL_TERM_";

            }
            string trav = "";
            if (cmb_trav_movel.SelectedIndex == 1)
            {
                trav = "_TRAV_MOVEL";
            }

            string descricao = "CJ_TAMPA_INSP_" + assoalho + passadeira + fixadores + ved + isol + comprimento_vao + "x" + larg_vao + "mm" + trav;
            double[] valor_expressoes = { comprimento_vao, larg_vao, cmb_assoalho.SelectedIndex + 1, cmb_passadeira.SelectedIndex, num_fixacao, cmb_vedacao.SelectedIndex, cmb_isolamento.SelectedIndex, cmb_trav_movel.SelectedIndex };
            double[] tolerancia = { 0, 0, 0, 0, 0, 0, 0, 0 };

            (arquivo, existe) = Custom_Mascarello.Create.CreateMember_new(fam, descricao, expressao, valor_expressoes, 0, tolerancia, "", descricao, item_type, "", false, null, null);
            //  string arquivo = Custom_Mascarello.Create.CreateMember(fam, descricao, expressao, valor_expressoes, 0, tolerancia, "", descricao);
            // string codigo_ret = Search_Tampa_Inspec.SearchMember("393397", "", expressao, valor_expressoes, 0, tolerancia, "", "");
            if (arquivo != "")
            {
                atualizacao_fam_2312.gerar_pdf_criacao_familia(arquivo.Substring(0, 6), "000");
                dtg_tampas.Rows.Add(arquivo, comprimento_vao.ToString(), larg_vao, cmb_assoalho.Text, cmb_passadeira.Text, num_fixacao, cmb_vedacao.Text, cmb_isolamento.Text, cmb_trav_movel.Text);
            }
            //else
            //{
            //    MessageBox.Show("Item não criado");
            //}
        }
        public void busca_default(string fam, string descricao, string[] att_familia, double[] valores, double[] tol, bool create_fluxo)
        {
            // lbl_informar_tampas.Text = "Verificando filhos na familia " + fam+" ....";
            string[] expressao = att_familia;
            double[] valor_expressoes = valores;
            double[] tolerancia = tol;

            bool boletim = false;
            List<string> listaPintura = new List<string>();
            List<string> listaSelecionados = new List<string>();
            string descricao_final = "";
            if (expressao.Contains("PINTURA"))
            {
                bool attr_pintura = true;
                string ver_boletim = conexao_bd.verifica_boletim_fam(fam.Substring(0, 6));

                if (ver_boletim != "" && attr_pintura == true)
                {

                    boletim = true;
                    listaSelecionados.Add("BT-1");
                    listaPintura.Add("BT-1");
                    listaPintura.Add("PPPT");
                    listaPintura.Add("PINTURA PÓ PRETO TEXTURIZADO");

                    // (listaSelecionados, listaPintura) = insert_boletin_fam_sincrona(ver_boletim);
                    descricao_final = descricao_final.Remove(descricao_final.Length - 1, 1);
                    descricao_final = descricao_final + " " + "_C/_PINTURA";
                    descricao = descricao + descricao_final;
                }
                if (ver_boletim != "" && attr_pintura == false)
                {
                    descricao_final = descricao_final.Remove(descricao_final.Length - 1, 1);
                }
            }
            else
            {
                listaPintura = null;
                listaSelecionados = null;
            }

            string codigo_ret = Search_Tampa_Inspec.SearchMember(fam, "", expressao, valor_expressoes, 0, tolerancia, "", "");

            if (codigo_ret != "")
            {
             //   dtg_tampas.Rows.Add(fam, codigo_ret);

            }
            if (codigo_ret == "")
            {
                bool existe = false;
                string arquivo = "";
                codigo_ret = "criar_item";
                string item_type = "M4_it_mascarello";

                (arquivo, existe) = Custom_Mascarello.Create.CreateMember_new(fam, descricao, expressao, valor_expressoes, 0, tolerancia, "", descricao, item_type, "", boletim, listaSelecionados, listaPintura);

                if (boletim == true)
                {
                    registro_boletim_fam(listaSelecionados, listaPintura, arquivo.Substring(0, 6), "000");
                }
                atualizacao_fam_2312.gerar_pdf_criacao_familia(arquivo.Substring(0, 6), "000");

                if (create_fluxo == true)
                {
                    conexao_bd conexao = new conexao_bd();
                    conexao.insert_wf_auto(arquivo.Substring(0, 6), "000");
                }

            }

        }
        public void busca(string fam, string descricao, string[] att_familia, double[] valores, double[] tol)
        {
            // lbl_informar_tampas.Text = "Verificando filhos na familia " + fam+" ....";
            string[] expressao = att_familia;
            double[] valor_expressoes = valores;
            double[] tolerancia = tol;

            bool boletim = false;
            List<string> listaPintura = new List<string>();
            List<string> listaSelecionados = new List<string>();
            string descricao_final = descricao;
            if (expressao.Contains("PINTURA"))
            {
                bool attr_pintura = true;
                string ver_boletim = conexao_bd.verifica_boletim_fam(fam.Substring(0, 6));

                if (ver_boletim != "" && attr_pintura == true)
                {

                    boletim = true;
                    listaSelecionados.Add("BT-1");
                    listaPintura.Add("BT-1");
                    listaPintura.Add("PPPT");
                    listaPintura.Add("PINTURA PÓ PRETO TEXTURIZADO");

                    // (listaSelecionados, listaPintura) = insert_boletin_fam_sincrona(ver_boletim);
                    //descricao_final = descricao_final.Remove(descricao_final.Length - 1, 1);
                    descricao_final = descricao_final + "_C/_PINTURA";
                    descricao = descricao_final;
                }
                if (ver_boletim != "" && attr_pintura == false)
                {
                    descricao_final = descricao_final.Remove(descricao_final.Length - 1, 1);
                }
            }
            else
            {
                listaPintura = null;
                listaSelecionados = null;
            }

            string codigo_ret = Search_Tampa_Inspec.SearchMember(fam, "", expressao, valor_expressoes, 0, tolerancia, "", "");

            if (codigo_ret != "")
            {
                dtg_tampas.Rows.Add(fam, codigo_ret);

            }
            if (codigo_ret == "")
            {


                bool existe = false;
                string arquivo = "";
                codigo_ret = "criar_item";
                string item_type = "M4_it_mascarello";

                (arquivo, existe) = Custom_Mascarello.Create.CreateMember_new(fam, descricao, expressao, valor_expressoes, 0, tolerancia, "", descricao, item_type, "", boletim, listaSelecionados, listaPintura);

                if (boletim == true)
                {
                    registro_boletim_fam(listaSelecionados, listaPintura, arquivo.Substring(0, 6), "000");
                }
                atualizacao_fam_2312.gerar_pdf_criacao_familia(arquivo.Substring(0, 6), "000");

                dtg_tampas.Rows.Add(fam, arquivo);
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (ckd_antigo.Checked == true)
            {

                dtg_tampas.Columns.Clear();
                dtg_tampas.Columns.Add("FAM", "FAM");
                dtg_tampas.Columns.Add("CODIGO", "CODIGO");

                lbl_informar_tampas.Text = "Verificando itens necessários....";
                System.Threading.Thread.Sleep(2000);

                double comprimento_vao = Convert.ToDouble(txt_comp_tp.Text);
                double larg_vao = Convert.ToDouble(txt_larg_tp.Text);

                double[] tolerancia = { 0.1, 0.1 };
                string desc = "CH_AL_LAV_TAMPA_INSP_VAO_" + comprimento_vao + "x" + larg_vao;
                //MessageBox.Show(comprimento_vao + " " +larg_vao);
                //MessageBox.Show("213298");
                busca("213298", desc, new string[] { "COMP_VAO", "LARG_VAO" }, new double[] { comprimento_vao, larg_vao }, tolerancia);

                //aqui não chega

                comprimento_vao = Convert.ToDouble(txt_comp_tp.Text) - 20;
                larg_vao = Convert.ToDouble(txt_larg_tp.Text) - 20;
                desc = "CH_GALVALUME_CSA_ASTM-A792_" + comprimento_vao + "x" + larg_vao + "x0,75mm";
                busca("393725", desc, new string[] { "LARG", "ALT" }, new double[] { comprimento_vao, larg_vao }, tolerancia);

                comprimento_vao = Convert.ToDouble(txt_comp_tp.Text) + 27;
                larg_vao = Convert.ToDouble(txt_larg_tp.Text) + 27;

                desc = "PRF_AL_TAMPA_INSP_" + comprimento_vao + "mm";
                busca("393443", desc, new string[] { "COMP" }, new double[] { comprimento_vao }, tolerancia);

                desc = "PRF_AL_TAMPA_INSP_" + larg_vao + "mm";
                busca("393443", desc, new string[] { "COMP" }, new double[] { larg_vao }, tolerancia);

                larg_vao = Convert.ToDouble(txt_larg_tp.Text);

                desc = "PRF_C_50x20_REF_TAMPA_INSP_VAO_" + larg_vao + "mm";

                busca("213357", desc, new string[] { "VAO" }, new double[] { larg_vao }, tolerancia);

                comprimento_vao = Convert.ToDouble(txt_comp_tp.Text);
                larg_vao = Convert.ToDouble(txt_larg_tp.Text);

                desc = "CH_COMPENSADO_TAMPA_INSP_VAO_" + comprimento_vao + "x" + larg_vao;
                busca("213278", desc, new string[] { "COMP_VAO", "LARG_VAO" }, new double[] { comprimento_vao, larg_vao }, tolerancia);

                lbl_informar_tampas.Text = "Criando CJ da tampa  " + comprimento_vao + "x" + larg_vao + "....";
                create_tampa("393397");
                lbl_informar_tampas.Text = "Criação Finalizada";

            }
            else
            {
                criar_tampa_2023();
            }
        }

        public void criar_tampa_2023()
        {


            dtg_tampas.Columns.Clear();
            dtg_tampas.Columns.Add("FAM", "FAM");
            dtg_tampas.Columns.Add("CODIGO", "CODIGO");

            lbl_informar_tampas.Text = "Verificando itens necessários....";
            System.Threading.Thread.Sleep(2000);

            double comprimento_vao = Convert.ToDouble(txt_comp_tp.Text);
            double larg_vao = Convert.ToDouble(txt_larg_tp.Text);


            double[] tolerancia = { 0.1, 0.1, 0.1 };
            string desc = "CH_AL_LAV_TAMPA_INSP_VAO_" + comprimento_vao + "x" + larg_vao;
            //MessageBox.Show(comprimento_vao + " " +larg_vao);
            //MessageBox.Show("213298");
            busca("541646", desc, new string[] { "COMP_VAO", "LARG_VAO" }, new double[] { comprimento_vao, larg_vao }, tolerancia);

            //aqui não chega

            comprimento_vao = Convert.ToDouble(txt_comp_tp.Text) - 25;
            larg_vao = Convert.ToDouble(txt_larg_tp.Text) - 25;
            desc = "CH_GALVALUME_CSA_ASTM-A792_" + comprimento_vao + "x" + larg_vao + "x0,75mm";
            busca("448618", desc, new string[] { "A", "B" }, new double[] { comprimento_vao, larg_vao }, tolerancia);

            comprimento_vao = Convert.ToDouble(txt_comp_tp.Text) - 128;
            larg_vao = Convert.ToDouble(txt_larg_tp.Text) - 128;

            desc = "PRF_AL_TAMPA_INSP_" + comprimento_vao + "mm";
            busca("448539", desc, new string[] { "COMP" }, new double[] { comprimento_vao }, tolerancia);

            desc = "PRF_AL_TAMPA_INSP_" + larg_vao + "mm";
            busca("448539", desc, new string[] { "COMP" }, new double[] { larg_vao }, tolerancia);

            larg_vao = Convert.ToDouble(txt_larg_tp.Text);

            desc = "PRF_C_50x20_REF_TAMPA_INSP_VAO_" + larg_vao + "mm";

            if (cmb_fixadores.Text != "4")
            {
                busca("213357", desc, new string[] { "VAO" }, new double[] { larg_vao }, tolerancia);
            }

            comprimento_vao = Convert.ToDouble(txt_comp_tp.Text);
            larg_vao = Convert.ToDouble(txt_larg_tp.Text);

            desc = "CH_COMPENSADO_TAMPA_INSP_VAO_" + comprimento_vao + "x" + larg_vao;
            busca("448579", desc, new string[] { "COMP_VAO", "LARG_VAO" }, new double[] { comprimento_vao, larg_vao }, tolerancia);

            if (cmb_trav_movel.Text == "SIM")
            {
                double comp = Convert.ToDouble(txt_comp_tp.Text) - 66;
                desc = "CH_BAND_QD_TAMPA_INSP_" + comp + "mm";
                busca("566418", desc, new string[] { "COMP" }, new double[] { comp }, tolerancia);

                comp = Convert.ToDouble(txt_larg_tp.Text) - 66;
                desc = "CH_BAND_QD_TAMPA_INSP_" + comp + "mm";
                busca("566418", desc, new string[] { "COMP" }, new double[] { comp }, tolerancia);

                desc = "CJ_CH_BAND_QD_TAMPA_INSP__" + comprimento_vao + "x" + larg_vao + "mm";
                busca("566411", desc, new string[] { "COMP_VAO", "LARG_VAO", "PINTURA" }, new double[] { comprimento_vao, larg_vao, 1 }, tolerancia);
            }

            lbl_informar_tampas.Text = "Criando CJ da tampa  " + comprimento_vao + "x" + larg_vao + "....";
            create_tampa_1("448508");
            lbl_informar_tampas.Text = "Criação Finalizada";


        }

        private void cmb_assoalho_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_assoalho.SelectedIndex == 0)
            {
                cmb_passadeira.SelectedIndex = 0;
                cmb_passadeira.Enabled = false;
            }
            else
            {
                cmb_passadeira.SelectedIndex = 1;
                cmb_passadeira.Enabled = true;
            }
        }

        private void cmb_passadeira_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_assoalho.SelectedIndex == 1 && cmb_passadeira.SelectedIndex == 0)
            {
                cmb_passadeira.SelectedIndex = 1;
            }
        }

        private void tabControlAdv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectadmin = conexao_bd.select_user_admin(Environment.UserName);
            if (selectadmin != "1")
            {
                btn_mcc.Visible = true;
                Tab_fams.TabPages.Remove(Ferramentas);
                tab_itens.TabPages.Remove(tab_admin);
                tab_itens.TabPages.Remove(tab_admin2);

            }
        }

        private void tabControlAdv1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void tabPageAdv1_TabIndexChanged(object sender, EventArgs e)
        {
            //  tabcontrol_principal_Selected
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            string teste = Listar_itens.Localizar_Membro_da_Familia("393397");// Localizar_Membro_da_Familia
        }
        public static int GetUnloadOption(string dummy) { return (int)Session.LibraryUnloadOption.Immediately; }
        private void btn_visualizar_bol_Click(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;

            string item = workPart.FullPath.Substring(0, 6);
            int rev = Convert.ToInt32(workPart.FullPath.Substring(7, 3));
            conexao_db_TeamCenter conect = new conexao_db_TeamCenter();
            string realese = conect.busca_verifica_status(item, rev.ToString());
          
            bool editar = false;
            if (realese == "0")
            {
                editar = true;
            }
            if (realese == "1")
            {
                editar = false;
            }
            if (editar == false)
            {
                MessageBox.Show("O Status atual do item não permite a inserção de boletim técnico.\nVerifique o status do Item", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                frm_select_boletim frm = new frm_select_boletim(item, rev.ToString(), 1, editar);
                frm.ShowDialog();
                List<string> _lista_selecionados = new List<string>();
                List<string> _lista_selecionado_pintura = new List<string>();

                conexao_bd db = new conexao_bd();
                DataSet _busca_itens_select = db.busca_boletim_selecionado(item, rev.ToString());

                foreach (DataRow row1 in _busca_itens_select.Tables[0].Rows)
                {

                    _lista_selecionados.Add(row1["boletim"].ToString());
                    if (row1["tag"].ToString() != "")
                    {
                        _lista_selecionado_pintura.Add(row1["boletim"].ToString());
                        _lista_selecionado_pintura.Add(row1["tag"].ToString());
                        _lista_selecionado_pintura.Add(row1["info_color"].ToString());
                    }

                }
                if (_lista_selecionados.Count < 5)
                {
                    for (int i = _lista_selecionados.Count; i < 5; i++)
                    {
                        _lista_selecionados.Add("-");
                    }
                }




                string[] lista_selecionados = _lista_selecionados.ToArray();
                string[] lista_selecionados_pintura = _lista_selecionado_pintura.ToArray();


                criar_atributos(lista_selecionados);
                if (lista_selecionados_pintura.Count() > 0)
                {
                    criar_attibutos_pintura(lista_selecionados_pintura);
                }

                deletar_tabela("BOLETIM_TECNICO");
                deletar_tabela("BOLETIM_PINTURA");

                Insertb_bt();
                if (lista_selecionados_pintura.Count() > 0)
                {
                    Insertb_bp();
                }
            }
            //inserir_tabela();
            //string formato = verificar_formato();
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
        public void inserir_tabela()
        {

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Preferences.LoadDraftingStandardBuilder loadDraftingStandardBuilder1;

            Part part1;
            try
            {
                part1 = (Part)theSession.Parts.FindObject("@DB/Boletim_Tecnico_metric/A");

            }
            catch (Exception)
            {

                BasePart basePart1;
                PartLoadStatus partLoadStatus1;
                basePart1 = theSession.Parts.OpenBaseDisplay("@DB/Boletim_Tecnico_metric/A", out partLoadStatus1);
                part1 = (Part)basePart1;
            }

            loadDraftingStandardBuilder1 = part1.Preferences.DraftingPreference.CreateLoadDraftingStandardBuilder();
            loadDraftingStandardBuilder1.WelcomeMode = false;

            loadDraftingStandardBuilder1.Level = NXOpen.Preferences.LoadDraftingStandardBuilder.LoadLevel.User;

            loadDraftingStandardBuilder1.Name = "ISO(MASCARELLO)";

            NXObject nXObject1;
            nXObject1 = loadDraftingStandardBuilder1.Commit();

            Part part2;
            part2 = theSession.Parts.Work;

            Part part3;
            part3 = theSession.Parts.Display;

            Part part4;
            part4 = theSession.Parts.Work;

            Part part5;
            part5 = theSession.Parts.Display;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXObject[] objects1 = new NXObject[1];
            objects1[0] = workPart;
            AttributePropertiesBuilder attributePropertiesBuilder1;
            attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            attributePropertiesBuilder1.Units = "MilliMeter";

            NXObject[] objects2 = new NXObject[1];
            objects2[0] = workPart;
            MassPropertiesBuilder massPropertiesBuilder1;
            massPropertiesBuilder1 = workPart.PropertiesManager.CreateMassPropertiesBuilder(objects2);

            SelectNXObjectList selectNXObjectList1;
            selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

            NXObject[] objects3;
            objects3 = selectNXObjectList1.GetArray();

            massPropertiesBuilder1.LoadPartialComponents = true;

            massPropertiesBuilder1.Accuracy = 0.99;

            NXObject[] objects4 = new NXObject[1];
            objects4[0] = workPart;
            PreviewPropertiesBuilder previewPropertiesBuilder1;
            previewPropertiesBuilder1 = workPart.PropertiesManager.CreatePreviewPropertiesBuilder(objects4);

            previewPropertiesBuilder1.StorePartPreview = true;

            previewPropertiesBuilder1.StoreModelViewPreview = true;

            previewPropertiesBuilder1.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

            DateBuilder dateBuilder1;
            dateBuilder1 = attributePropertiesBuilder1.DateValue;

            DateItemBuilder dateItemBuilder1;
            dateItemBuilder1 = dateBuilder1.FromDateItem;

            dateItemBuilder1.Year = "2018";

            DateBuilder dateBuilder2;
            dateBuilder2 = attributePropertiesBuilder1.DateValue;

            DateItemBuilder dateItemBuilder2;
            dateItemBuilder2 = dateBuilder2.ToDateItem;

            dateItemBuilder2.Year = "2018";

            NXObject[] objects5 = new NXObject[1];
            objects5[0] = workPart;
            attributePropertiesBuilder1.SetAttributeObjects(objects5);

            attributePropertiesBuilder1.Units = "MilliMeter";

            theSession.SetUndoMarkName(markId1, "Displayed Part Properties Dialog");

            DateBuilder dateBuilder3;
            dateBuilder3 = attributePropertiesBuilder1.DateValue;

            DateItemBuilder dateItemBuilder3;
            dateItemBuilder3 = dateBuilder3.DateItem;

            dateItemBuilder3.Day = NXOpen.DateItemBuilder.DayOfMonth.Day05;

            DateBuilder dateBuilder4;
            dateBuilder4 = attributePropertiesBuilder1.DateValue;

            DateItemBuilder dateItemBuilder4;
            dateItemBuilder4 = dateBuilder4.DateItem;

            dateItemBuilder4.Month = NXOpen.DateItemBuilder.MonthOfYear.Apr;

            DateBuilder dateBuilder5;
            dateBuilder5 = attributePropertiesBuilder1.DateValue;

            DateItemBuilder dateItemBuilder5;
            dateItemBuilder5 = dateBuilder5.DateItem;

            dateItemBuilder5.Year = "2019";

            DateBuilder dateBuilder6;
            dateBuilder6 = attributePropertiesBuilder1.DateValue;

            DateItemBuilder dateItemBuilder6;
            dateItemBuilder6 = dateBuilder6.DateItem;

            dateItemBuilder6.Time = "00:00:00";

            massPropertiesBuilder1.UpdateOnSave = NXOpen.MassPropertiesBuilder.UpdateOptions.No;

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Title = "DB_QTD";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Link to Expression");

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Launch Expression Editor");

            attributePropertiesBuilder1.IsReferenceType = false;



            // ----------------------------------------------
            //   Menu: Tools->Journal->Stop Recording
            // ----------------------------------------------

        }
        public void deletar_tabela(string name_tabela)
        {

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Delete");

            bool notifyOnDelete1;
            notifyOnDelete1 = theSession.Preferences.Modeling.NotifyOnDelete;

            theSession.UpdateManager.ClearErrorList();

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Delete");

            NXOpen.Annotations.TableSection[] tableSections = workPart.Annotations.TableSections.ToArray();

            NXObject[] objects1 = new NXObject[1];
            foreach (NXOpen.Annotations.TableSection tableSection in tableSections)
            {
                if (tableSection.Name == name_tabela)
                {
                    objects1[0] = tableSection;
                }

            }
            int nErrs1;
            nErrs1 = theSession.UpdateManager.AddToDeleteList(objects1);

            bool notifyOnDelete2;
            notifyOnDelete2 = theSession.Preferences.Modeling.NotifyOnDelete;

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId2);

            theSession.DeleteUndoMark(markId1, null);

        }
        public static void deletar_tabela_fam(string name_tabela, Part part)
        {

            Session theSession = Session.GetSession();
            // Part workPart = theSession.Parts.Work;
            //Part displayPart = theSession.Parts.Display;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Delete");

            bool notifyOnDelete1;
            notifyOnDelete1 = theSession.Preferences.Modeling.NotifyOnDelete;

            theSession.UpdateManager.ClearErrorList();

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Delete");

            NXOpen.Annotations.TableSection[] tableSections = part.Annotations.TableSections.ToArray();

            NXObject[] objects1 = new NXObject[1];
            foreach (NXOpen.Annotations.TableSection tableSection in tableSections)
            {
                if (tableSection.Name == name_tabela)
                {
                    objects1[0] = tableSection;
                }

            }
            int nErrs1;
            nErrs1 = theSession.UpdateManager.AddToDeleteList(objects1);

            bool notifyOnDelete2;
            notifyOnDelete2 = theSession.Preferences.Modeling.NotifyOnDelete;

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId2);

            theSession.DeleteUndoMark(markId1, null);

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

            //NXOpen.Session.UndoMarkId markId1;
            //markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Delete");

            //bool notifyOnDelete1;
            //notifyOnDelete1 = theSession.Preferences.Modeling.NotifyOnDelete;

            //theSession.UpdateManager.ClearErrorList();

            //NXOpen.Session.UndoMarkId markId2;
            //markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Delete");

            //NXOpen.Annotations.Note[] tableSections = workPart.Notes.ToArray();

            //NXObject[] objects1 = new NXObject[1];
            //foreach (NXOpen.Annotations.TableSection tableSection in tableSections)
            //{
            //    if (tableSection.Name == "BOLETIM_TECNICO")
            //    {
            //        objects1[0] = tableSection;
            //    }

            //}


        }

        public static string verificar_formato_novo()
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

                        if (item == "<C0.500><C0.250>-x,xx<C><C>")
                        {
                            formato = "novo";
                        }
                    }
                }

            }

            return formato;

            //NXOpen.Session.UndoMarkId markId1;
            //markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Delete");

            //bool notifyOnDelete1;
            //notifyOnDelete1 = theSession.Preferences.Modeling.NotifyOnDelete;

            //theSession.UpdateManager.ClearErrorList();

            //NXOpen.Session.UndoMarkId markId2;
            //markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Delete");

            //NXOpen.Annotations.Note[] tableSections = workPart.Notes.ToArray();

            //NXObject[] objects1 = new NXObject[1];
            //foreach (NXOpen.Annotations.TableSection tableSection in tableSections)
            //{
            //    if (tableSection.Name == "BOLETIM_TECNICO")
            //    {
            //        objects1[0] = tableSection;
            //    }

            //}


        }
        public static string verificar_formato_fam(Part workPart)
        {

            Session theSession = Session.GetSession();

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

        public static void Insert_tab_alteracao()//(string codigo, string rev)
        {
            double pos_x = 0.0;
            double pos_y = 0.0;
            string format = verificar_formato();
            if (format == "A3")
            {
                pos_x = 6;
                pos_y = 291;
            }
            if (format == "A4")
            {
                pos_x = 6;
                pos_y = 276;
            }
            //if (format == "A2")
            //{
            //    pos_x = 588;
            //    pos_y = 399;
            //}

            NXOpen.UF.UFSession ufSession = NXOpen.UF.UFSession.GetUFSession();
            double[] origem = { pos_x, pos_y, 0 };
            ufSession.Tabnot.CreateFromTemplate("@DB/Revisao-metric/A", origem, out NXOpen.Tag tabular_note);
        }
        public void criar_atributos(string[] lista_selecionados)
        {

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: File->Properties
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            string[] lista_atributos = { "B1", "B2", "B3", "B4", "B5" };
            for (int i = 0; i <= lista_atributos.Length - 1; i++)
            {

                NXObject[] objects1 = new NXObject[1];
                objects1[0] = workPart;
                AttributePropertiesBuilder attributePropertiesBuilder1;
                attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

                attributePropertiesBuilder1.Units = "MilliMeter";

                NXObject[] objects2 = new NXObject[1];
                objects2[0] = workPart;
                MassPropertiesBuilder massPropertiesBuilder1;
                massPropertiesBuilder1 = workPart.PropertiesManager.CreateMassPropertiesBuilder(objects2);

                SelectNXObjectList selectNXObjectList1;
                selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

                NXObject[] objects3;
                objects3 = selectNXObjectList1.GetArray();

                massPropertiesBuilder1.UpdateOnSave = NXOpen.MassPropertiesBuilder.UpdateOptions.No;

                massPropertiesBuilder1.LoadPartialComponents = true;

                massPropertiesBuilder1.Accuracy = 0.99;

                NXObject[] objects4 = new NXObject[1];
                objects4[0] = workPart;
                PreviewPropertiesBuilder previewPropertiesBuilder1;
                previewPropertiesBuilder1 = workPart.PropertiesManager.CreatePreviewPropertiesBuilder(objects4);

                attributePropertiesBuilder1.DateValue.FromDateItem.Year = "2023";

                attributePropertiesBuilder1.DateValue.ToDateItem.Year = "2023";

                previewPropertiesBuilder1.StorePartPreview = true;

                previewPropertiesBuilder1.StoreModelViewPreview = true;

                previewPropertiesBuilder1.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

                NXObject[] objects5 = new NXObject[1];
                objects5[0] = workPart;
                attributePropertiesBuilder1.SetAttributeObjects(objects5);

                attributePropertiesBuilder1.Units = "MilliMeter";

                theSession.SetUndoMarkName(markId1, "Displayed Part Properties Dialog");

                attributePropertiesBuilder1.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day12;

                attributePropertiesBuilder1.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Mar;

                attributePropertiesBuilder1.DateValue.DateItem.Year = "2024";

                attributePropertiesBuilder1.DateValue.DateItem.Time = "00:00:00";

                attributePropertiesBuilder1.Title = lista_atributos[i];

                NXOpen.Session.UndoMarkId markId2;
                markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

                string valor_att = lista_selecionados[i];

                attributePropertiesBuilder1.StringValue = valor_att;

                theSession.DeleteUndoMark(markId2, null);

                NXOpen.Session.UndoMarkId markId3;
                markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

                NXObject nXObject1;
                nXObject1 = attributePropertiesBuilder1.Commit();

                NXOpen.MassPropertiesBuilder.UpdateOptions updateoption1;
                updateoption1 = massPropertiesBuilder1.UpdateOnSave;

                NXObject nXObject2;
                nXObject2 = massPropertiesBuilder1.Commit();

                workPart.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

                NXObject nXObject3;
                nXObject3 = previewPropertiesBuilder1.Commit();

                NXOpen.Session.UndoMarkId id1;
                id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

                int nErrs1;
                nErrs1 = theSession.UpdateManager.DoUpdate(id1);

                theSession.DeleteUndoMark(markId3, null);

                theSession.SetUndoMarkName(id1, "Displayed Part Properties");

                attributePropertiesBuilder1.Destroy();

                massPropertiesBuilder1.Destroy();

                previewPropertiesBuilder1.Destroy();
            }

        }
        public void criar_atributos_fam(string[] lista_selecionados, Part part)
        {

            Session theSession = Session.GetSession();
            //Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: File->Properties
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            string[] lista_atributos = { "B1", "B2", "B3", "B4", "B5" };
            for (int i = 0; i <= lista_atributos.Length - 1; i++)
            {

                NXObject[] objects1 = new NXObject[1];
                objects1[0] = part;
                AttributePropertiesBuilder attributePropertiesBuilder1;
                attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(part, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

                attributePropertiesBuilder1.Units = "MilliMeter";

                NXObject[] objects2 = new NXObject[1];
                objects2[0] = part;
                MassPropertiesBuilder massPropertiesBuilder1;
                massPropertiesBuilder1 = part.PropertiesManager.CreateMassPropertiesBuilder(objects2);

                SelectNXObjectList selectNXObjectList1;
                selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

                NXObject[] objects3;
                objects3 = selectNXObjectList1.GetArray();

                massPropertiesBuilder1.UpdateOnSave = NXOpen.MassPropertiesBuilder.UpdateOptions.No;

                massPropertiesBuilder1.LoadPartialComponents = true;

                massPropertiesBuilder1.Accuracy = 0.99;

                NXObject[] objects4 = new NXObject[1];
                objects4[0] = part;
                PreviewPropertiesBuilder previewPropertiesBuilder1;
                previewPropertiesBuilder1 = part.PropertiesManager.CreatePreviewPropertiesBuilder(objects4);

                attributePropertiesBuilder1.DateValue.FromDateItem.Year = "2023";

                attributePropertiesBuilder1.DateValue.ToDateItem.Year = "2023";

                previewPropertiesBuilder1.StorePartPreview = true;

                previewPropertiesBuilder1.StoreModelViewPreview = true;

                previewPropertiesBuilder1.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

                NXObject[] objects5 = new NXObject[1];
                objects5[0] = part;
                attributePropertiesBuilder1.SetAttributeObjects(objects5);

                attributePropertiesBuilder1.Units = "MilliMeter";

                theSession.SetUndoMarkName(markId1, "Displayed Part Properties Dialog");

                attributePropertiesBuilder1.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day12;

                attributePropertiesBuilder1.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Mar;

                attributePropertiesBuilder1.DateValue.DateItem.Year = "2024";

                attributePropertiesBuilder1.DateValue.DateItem.Time = "00:00:00";

                attributePropertiesBuilder1.Title = lista_atributos[i];

                NXOpen.Session.UndoMarkId markId2;
                markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

                string valor_att = lista_selecionados[i];

                attributePropertiesBuilder1.StringValue = valor_att;

                theSession.DeleteUndoMark(markId2, null);

                NXOpen.Session.UndoMarkId markId3;
                markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

                NXObject nXObject1;
                nXObject1 = attributePropertiesBuilder1.Commit();

                NXOpen.MassPropertiesBuilder.UpdateOptions updateoption1;
                updateoption1 = massPropertiesBuilder1.UpdateOnSave;

                NXObject nXObject2;
                nXObject2 = massPropertiesBuilder1.Commit();

                part.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

                NXObject nXObject3;
                nXObject3 = previewPropertiesBuilder1.Commit();

                NXOpen.Session.UndoMarkId id1;
                id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

                int nErrs1;
                nErrs1 = theSession.UpdateManager.DoUpdate(id1);

                theSession.DeleteUndoMark(markId3, null);

                theSession.SetUndoMarkName(id1, "Displayed Part Properties");

                attributePropertiesBuilder1.Destroy();

                massPropertiesBuilder1.Destroy();

                previewPropertiesBuilder1.Destroy();
            }

        }
        public void criar_attibutos_pintura(string[] lista_selecionados)
        {

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: File->Properties
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            string[] lista_atributos = { "B1_P", "B1_P_ABREV", "B1_P_DESC" };
            for (int i = 0; i <= lista_atributos.Length - 1; i++)
            {

                NXObject[] objects1 = new NXObject[1];
                objects1[0] = workPart;
                AttributePropertiesBuilder attributePropertiesBuilder1;
                attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

                attributePropertiesBuilder1.Units = "MilliMeter";

                NXObject[] objects2 = new NXObject[1];
                objects2[0] = workPart;
                MassPropertiesBuilder massPropertiesBuilder1;
                massPropertiesBuilder1 = workPart.PropertiesManager.CreateMassPropertiesBuilder(objects2);

                SelectNXObjectList selectNXObjectList1;
                selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

                NXObject[] objects3;
                objects3 = selectNXObjectList1.GetArray();

                massPropertiesBuilder1.UpdateOnSave = NXOpen.MassPropertiesBuilder.UpdateOptions.No;

                massPropertiesBuilder1.LoadPartialComponents = true;

                massPropertiesBuilder1.Accuracy = 0.99;

                NXObject[] objects4 = new NXObject[1];
                objects4[0] = workPart;
                PreviewPropertiesBuilder previewPropertiesBuilder1;
                previewPropertiesBuilder1 = workPart.PropertiesManager.CreatePreviewPropertiesBuilder(objects4);

                attributePropertiesBuilder1.DateValue.FromDateItem.Year = "2023";

                attributePropertiesBuilder1.DateValue.ToDateItem.Year = "2023";

                previewPropertiesBuilder1.StorePartPreview = true;

                previewPropertiesBuilder1.StoreModelViewPreview = true;

                previewPropertiesBuilder1.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

                NXObject[] objects5 = new NXObject[1];
                objects5[0] = workPart;
                attributePropertiesBuilder1.SetAttributeObjects(objects5);

                attributePropertiesBuilder1.Units = "MilliMeter";

                theSession.SetUndoMarkName(markId1, "Displayed Part Properties Dialog");

                attributePropertiesBuilder1.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day12;

                attributePropertiesBuilder1.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Mar;

                attributePropertiesBuilder1.DateValue.DateItem.Year = "2024";

                attributePropertiesBuilder1.DateValue.DateItem.Time = "00:00:00";

                attributePropertiesBuilder1.Title = lista_atributos[i];

                NXOpen.Session.UndoMarkId markId2;
                markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

                string valor_att = lista_selecionados[i];

                attributePropertiesBuilder1.StringValue = valor_att;

                theSession.DeleteUndoMark(markId2, null);

                NXOpen.Session.UndoMarkId markId3;
                markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

                NXObject nXObject1;
                nXObject1 = attributePropertiesBuilder1.Commit();

                NXOpen.MassPropertiesBuilder.UpdateOptions updateoption1;
                updateoption1 = massPropertiesBuilder1.UpdateOnSave;

                NXObject nXObject2;
                nXObject2 = massPropertiesBuilder1.Commit();

                workPart.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

                NXObject nXObject3;
                nXObject3 = previewPropertiesBuilder1.Commit();

                NXOpen.Session.UndoMarkId id1;
                id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

                int nErrs1;
                nErrs1 = theSession.UpdateManager.DoUpdate(id1);

                theSession.DeleteUndoMark(markId3, null);

                theSession.SetUndoMarkName(id1, "Displayed Part Properties");

                attributePropertiesBuilder1.Destroy();

                massPropertiesBuilder1.Destroy();

                previewPropertiesBuilder1.Destroy();
            }

        }
        public void criar_attibutos_pintura_fam(string[] lista_selecionados, Part workPart)
        {

            Session theSession = Session.GetSession();
            //Part workPart = theSession.Parts.Work;
            //Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: File->Properties
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            string[] lista_atributos = { "B1_P", "B1_P_ABREV", "B1_P_DESC" };
            for (int i = 0; i <= lista_atributos.Length - 1; i++)
            {

                NXObject[] objects1 = new NXObject[1];
                objects1[0] = workPart;
                AttributePropertiesBuilder attributePropertiesBuilder1;
                attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.IsArray = false;

                attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

                attributePropertiesBuilder1.Units = "MilliMeter";

                NXObject[] objects2 = new NXObject[1];
                objects2[0] = workPart;
                MassPropertiesBuilder massPropertiesBuilder1;
                massPropertiesBuilder1 = workPart.PropertiesManager.CreateMassPropertiesBuilder(objects2);

                SelectNXObjectList selectNXObjectList1;
                selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

                NXObject[] objects3;
                objects3 = selectNXObjectList1.GetArray();

                massPropertiesBuilder1.UpdateOnSave = NXOpen.MassPropertiesBuilder.UpdateOptions.No;

                massPropertiesBuilder1.LoadPartialComponents = true;

                massPropertiesBuilder1.Accuracy = 0.99;

                NXObject[] objects4 = new NXObject[1];
                objects4[0] = workPart;
                PreviewPropertiesBuilder previewPropertiesBuilder1;
                previewPropertiesBuilder1 = workPart.PropertiesManager.CreatePreviewPropertiesBuilder(objects4);

                attributePropertiesBuilder1.DateValue.FromDateItem.Year = "2023";

                attributePropertiesBuilder1.DateValue.ToDateItem.Year = "2023";

                previewPropertiesBuilder1.StorePartPreview = true;

                previewPropertiesBuilder1.StoreModelViewPreview = true;

                previewPropertiesBuilder1.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

                NXObject[] objects5 = new NXObject[1];
                objects5[0] = workPart;
                attributePropertiesBuilder1.SetAttributeObjects(objects5);

                attributePropertiesBuilder1.Units = "MilliMeter";

                theSession.SetUndoMarkName(markId1, "Displayed Part Properties Dialog");

                attributePropertiesBuilder1.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day12;

                attributePropertiesBuilder1.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Mar;

                attributePropertiesBuilder1.DateValue.DateItem.Year = "2024";

                attributePropertiesBuilder1.DateValue.DateItem.Time = "00:00:00";

                attributePropertiesBuilder1.Title = lista_atributos[i];

                NXOpen.Session.UndoMarkId markId2;
                markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

                string valor_att = lista_selecionados[i];

                attributePropertiesBuilder1.StringValue = valor_att;

                theSession.DeleteUndoMark(markId2, null);

                NXOpen.Session.UndoMarkId markId3;
                markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

                NXObject nXObject1;
                nXObject1 = attributePropertiesBuilder1.Commit();

                NXOpen.MassPropertiesBuilder.UpdateOptions updateoption1;
                updateoption1 = massPropertiesBuilder1.UpdateOnSave;

                NXObject nXObject2;
                nXObject2 = massPropertiesBuilder1.Commit();

                workPart.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

                NXObject nXObject3;
                nXObject3 = previewPropertiesBuilder1.Commit();

                NXOpen.Session.UndoMarkId id1;
                id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

                int nErrs1;
                nErrs1 = theSession.UpdateManager.DoUpdate(id1);

                theSession.DeleteUndoMark(markId3, null);

                theSession.SetUndoMarkName(id1, "Displayed Part Properties");

                attributePropertiesBuilder1.Destroy();

                massPropertiesBuilder1.Destroy();

                previewPropertiesBuilder1.Destroy();
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void teste_Click(object sender, EventArgs e)
        {


            //Part workPart = theSession.Parts.Work;

            //// Definir dimensões do cubo (paralelepípedo)
            //double length = 10.0; // Comprimento
            //double width = 10.0;  // Largura
            //double height = 10.0; // Altura
            //Point3d origin = new Point3d(0.0, 0.0, 0.0); // Posição de origem do cubo

            //// Criar o Block (Cubo)
            //NXOpen.Features.BlockFeatureBuilder blockFeatureBuilder = workPart.Features.CreateBlockFeatureBuilder(null);

            //// Definir o tipo de origem (neste caso, um ponto específico)
            //blockFeatureBuilder.OriginPoint = workPart.Points.CreatePoint(origin);
            //blockFeatureBuilder.SetOriginAndLengths(origin,"100","100","100");

            //// Commit para criar o cubo (block)
            //NXOpen.Features.CustomFeature blockFeature = blockFeatureBuilder.CommitFeature() as NXOpen.Features.CustomFeature;
            //blockFeatureBuilder.Destroy(); // Limpeza do builder

            //// Criar o CustomFeatureBuilder
            //CustomFeatureBuilder customFeatureBuilder = workPart.Features.CreateCustomFeatureBuilder(null);

            //customFeatureBuilder. = "CUBO"; // Definir o tipo do CustomFeature
            //// Adicionar o cubo como entrada geométrica ao CustomFeature
            //customFeatureBuilder.AddGeometryInput(blockFeature);

            //// Commit para criar o CustomFeature
            //CustomFeature customFeature = customFeatureBuilder.CommitFeature() as CustomFeature;

            //// Limpeza do CustomFeatureBuilder
            //customFeatureBuilder.Destroy();


        }

        private void button5_Click(object sender, EventArgs e)
        {
            string folderPath = @"\\gm-siemens-prod\POOL";

            List<string> fileList = new List<string>();
            LOG_Message("Sistema de Geração de PDF e DXF iniciado");
            while (true)
            {
                LOG_Message("Buscando itens em " + folderPath);
                // Foreach para percorrer a pasta e adicionar os nomes dos arquivos na lista
                foreach (string file in Directory.GetFiles(folderPath))
                {
                    fileList.Add(Path.GetFileName(file)); // Adiciona apenas o nome do arquivo
                    LOG_Message(Path.GetFileName(file));
                }
                LOG_Message("Total de itens para geração " + fileList.Count);
                // Foreach para percorrer a lista de nomes de arquivos
                foreach (string fileName in fileList)
                {
                    lbl_pdf.Text = "Gerando pdf do item " + fileName;

                    string[] split = fileName.Split('_'); // Separar o nome do arquivo em partes

                    try
                    {
                        bool gerar_dxf = false;
                        gerar_dxf = Open_drawing(split[0], split[1]);

                        //if(gerar_dxf == true)
                        //{

                        //    LOG_Message("Gerar dxf do item " + fileName);
                        //    Open_gerar_dxf(split[0], split[1]);
                        //}
                    }
                    catch
                    {
                        // LOG_Message("Erro ao gerar pdf do item " + fileName + " - " + ex.Message);
                    }

                    string file = @"S:\Desenhos\PDF\" + split[0] + "-" + split[1] + ".pdf";
                    if (!File.Exists(file))
                    {
                        Open_drawing_fam(split[0], split[1]);
                    }

                    if (File.Exists(file))
                    {
                        File.Move(folderPath + "\\" + split[0] + "_" + split[1] + "_" + split[2] + "_" + split[3],
                            folderPath + "\\ITENS_GERADOS\\" + split[0] + "_" + split[1] + "_" + split[2] + "_" + split[3]);
                    }
                }
                fileList.Clear();
                DateTime proxima = DateTime.Now.AddMinutes(2);
                lbl_pdf.Text = "Próxima verificação as " + proxima.Hour + "+" + proxima.Minute + ":" + proxima.Second;
                System.Threading.Thread.Sleep(60000);
            }
        }
        public static bool Open_drawing(string codigo, string rev)
        {

            string item = "@DB/" + codigo + "/" + rev + "/specification/" + codigo + "-" + rev + "-dwg1";

            // ----------------------------------------------
            //   Menu: Tools->Automation->Journal->Stop Recording
            // ----------------------------------------------

            NXOpen.Session theSession = NXOpen.Session.GetSession();
            // ----------------------------------------------
            //   Menu: File->Open...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            theSession.SetUndoMarkName(markId1, "Open Part File Dialog");

            theSession.UndoToMark(markId1, null);

            theSession.DeleteUndoMark(markId1, null);

            theSession.DeleteUndoMark(markId1, null);

            // ----------------------------------------------
            //   Menu: File->Open...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            theSession.SetUndoMarkName(markId2, "Open Part File Dialog");

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

            theSession.DeleteUndoMark(markId3, null);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

            theSession.DeleteUndoMark(markId4, null);

            theSession.SetUndoMarkName(markId2, "Open Part File");

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.BasePart basePart1;
            NXOpen.PartLoadStatus partLoadStatus1;
            try
            {
                // File already exists
                basePart1 = theSession.Parts.OpenActiveDisplay(item, NXOpen.DisplayPartOption.AllowAdditional, out partLoadStatus1);
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(1020004);
            }

            NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.FindObject(item));
            NXOpen.BasePart basePart2;
            NXOpen.PartReopenReport partReopenReport1;
            basePart2 = part1.Reopen(NXOpen.BasePart.CloseWholeTree.False, NXOpen.BasePart.CloseModified.CloseModified, null, out partReopenReport1);

            partReopenReport1.Dispose();
            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Change Displayed Part");

            NXOpen.Part part2 = ((NXOpen.Part)basePart2);
            NXOpen.PartLoadStatus partLoadStatus2;
            NXOpen.PartCollection.SdpsStatus status1;
            status1 = theSession.Parts.SetActiveDisplay(part2, NXOpen.DisplayPartOption.AllowAdditional, NXOpen.PartDisplayPartWorkPartOption.UseLast, out partLoadStatus2);

            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();
            theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");

            workPart.Drafting.EnterDraftingApplication();

            workPart.Views.WorkView.UpdateCustomSymbols();

            workPart.Drafting.SetTemplateInstantiationIsComplete(true);

            bool gerar_dxf = export_pdf(codigo, rev);

            // gerar_dxf_non_master_2(codigo, rev);

            Save_Close(codigo, rev);
            return gerar_dxf;

        }
        public static void Open_gerar_dxf(string codigo, string rev)
        {

            string item = "@DB/" + codigo + "/" + rev;



            NXOpen.Session theSession = NXOpen.Session.GetSession();
            // ----------------------------------------------
            //   Menu: File->Open...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            theSession.SetUndoMarkName(markId1, "Open Part File Dialog");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

            theSession.DeleteUndoMark(markId3, null);

            theSession.SetUndoMarkName(markId1, "Open Part File");

            theSession.DeleteUndoMark(markId1, null);

            theSession.Parts.SetNonmasterSeedPartData(item);

            // NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.FindObject(item));
            // Potential journal callback detected. Pausing journal.
            // Journal callback ended. Unpausing
            NXOpen.BasePart basePart1;
            NXOpen.PartLoadStatus partLoadStatus1;
            try
            {
                // File already exists
                basePart1 = theSession.Parts.OpenActiveDisplay(item, NXOpen.DisplayPartOption.AllowAdditional, out partLoadStatus1);
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(1020004);
            }

            //basePart1 = theSession.Parts.OpenActiveDisplay(item, NXOpen.DisplayPartOption.AllowAdditional, out partLoadStatus1);
            NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.FindObject(item));
            // partLoadStatus1.Dispose();
            theSession.ApplicationSwitchImmediate("UG_APP_SBSM");
            string name_blank = "";
            foreach (Feature blank in part1.Features)
            {

                if (blank.GetFeatureName().Contains("Flat Pattern") && (blank.Name == "BLANK" || blank.Name == "blank"))
                {
                    //MessageBox.Show(item.GetFeatureName() + "  " + item.Name);
                    name_blank = blank.GetFeatureName().Replace(' ', '_').ToLower();
                }

            }
            name_blank = name_blank.Replace(' ', '_').ToUpper();
            // ----------------------------------------------
            //   Menu: Insert->Flat Pattern->Export Flat Pattern...
            // ----------------------------------------------
            if (name_blank != "")
            {
                string path = @"S:\Desenhos\DXF\" + codigo + "-" + rev + ".dxf";
                LOG_Message("BLANK encontrado: " + name_blank);
                NXOpen.Session.UndoMarkId markId4;
                markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

                NXOpen.Features.SheetMetal.ExportFlatPatternBuilder exportFlatPatternBuilder1;
                exportFlatPatternBuilder1 = part1.Features.SheetmetalManager.CreateExportFlatPatternBuilder();
                exportFlatPatternBuilder1.ExportLocation = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.ExportLocationOptions.Native;
                exportFlatPatternBuilder1.OutputFile = path;

                exportFlatPatternBuilder1.BendUp = false;

                exportFlatPatternBuilder1.BendDown = false;

                exportFlatPatternBuilder1.InteriorCutout = true;

                exportFlatPatternBuilder1.InteriorFeature = true;

                NXOpen.Part part2 = ((NXOpen.Part)part1);
                NXOpen.Features.FlatPattern flatPattern1 = ((NXOpen.Features.FlatPattern)part2.Features.FindObject(name_blank));
                exportFlatPatternBuilder1.FlatPattern.Value = flatPattern1;

                exportFlatPatternBuilder1.FlatPattern.Value = flatPattern1;

                theSession.SetUndoMarkName(markId4, "Export Flat Pattern Dialog");

                exportFlatPatternBuilder1.DxfRevision = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.DxfRevisionType.R12;

                NXOpen.Session.UndoMarkId markId5;
                markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export Flat Pattern");

                theSession.DeleteUndoMark(markId5, null);

                NXOpen.Session.UndoMarkId markId6;
                markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export Flat Pattern");

                // Potential journal callback detected. Pausing journal.
                // Journal callback ended. Unpausing
                NXOpen.NXObject nXObject1;
                nXObject1 = exportFlatPatternBuilder1.Commit();

                theSession.DeleteUndoMark(markId6, null);

                exportFlatPatternBuilder1.Destroy();

                // ----------------------------------------------
                //   Menu: File->Close->All Parts
                // ----------------------------------------------

                //theSession.Parts.CloseAll(NXOpen.BasePart.CloseModified.CloseModified, null);
                part1.Close(BasePart.CloseWholeTree.False, BasePart.CloseModified.CloseModified, null);
                theSession.ApplicationSwitchImmediate("UG_APP_NOPART");
                if (File.Exists(path))
                {
                    LOG_Message("BLANK Gerado: " + name_blank);
                }
                else
                {
                    LOG_Message("Erro ao gerar BLANK: " + name_blank);
                }

            }
            else
            {
                part1.Close(BasePart.CloseWholeTree.False, BasePart.CloseModified.CloseModified, null);
                theSession.ApplicationSwitchImmediate("UG_APP_NOPART");
                LOG_Message("SEM BLANK: " + name_blank);
            }
        }
        public static void Open_drawing_fam(string codigo, string rev)
        {

            string item = "@DB/" + codigo + "/" + rev;



            NXOpen.Session theSession = NXOpen.Session.GetSession();
            // ----------------------------------------------
            //   Menu: File->Open...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            theSession.SetUndoMarkName(markId1, "Open Part File Dialog");

            theSession.UndoToMark(markId1, null);

            theSession.DeleteUndoMark(markId1, null);

            theSession.DeleteUndoMark(markId1, null);

            // ----------------------------------------------
            //   Menu: File->Open...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            theSession.SetUndoMarkName(markId2, "Open Part File Dialog");

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

            theSession.DeleteUndoMark(markId3, null);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

            theSession.DeleteUndoMark(markId4, null);

            theSession.SetUndoMarkName(markId2, "Open Part File");

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.BasePart basePart1;
            NXOpen.PartLoadStatus partLoadStatus1;
            try
            {
                // File already exists
                basePart1 = theSession.Parts.OpenActiveDisplay(item, NXOpen.DisplayPartOption.AllowAdditional, out partLoadStatus1);
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(1020004);
            }

            NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.FindObject(item));
            NXOpen.BasePart basePart2;
            NXOpen.PartReopenReport partReopenReport1;
            basePart2 = part1.Reopen(NXOpen.BasePart.CloseWholeTree.False, NXOpen.BasePart.CloseModified.CloseModified, null, out partReopenReport1);

            partReopenReport1.Dispose();
            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Change Displayed Part");

            NXOpen.Part part2 = ((NXOpen.Part)basePart2);
            NXOpen.PartLoadStatus partLoadStatus2;
            NXOpen.PartCollection.SdpsStatus status1;
            status1 = theSession.Parts.SetActiveDisplay(part2, NXOpen.DisplayPartOption.AllowAdditional, NXOpen.PartDisplayPartWorkPartOption.UseLast, out partLoadStatus2);

            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            partLoadStatus2.Dispose();
            theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");

            // NXOpen.Part part2 = ((NXOpen.Part)basePart1);
            part2.Drafting.EnterDraftingApplication();

            part2.Views.WorkView.UpdateCustomSymbols();

            part2.Drafting.SetTemplateInstantiationIsComplete(true);

            // NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Open Sheet");

            try
            {
                NXOpen.Drawings.DraftingDrawingSheet draftingDrawingSheet1 = ((NXOpen.Drawings.DraftingDrawingSheet)part2.DraftingDrawingSheets.FindObject("Sheet 1"));
                if (draftingDrawingSheet1 != null)
                {
                    draftingDrawingSheet1.Open();
                    bool dxf = export_pdf(codigo, rev);
                    //gerar_dxf_familia(codigo, rev);


                }
            }
            catch
            {
                //string folderPath = @"\\gm-siemens-prod\POOL";
                //File.Move(folderPath + "\\" + codigo + "_" + rev + "_" + "PDF" + "_" + "2",
                //                folderPath + "\\ITENS_GERADOS\\" + codigo + "_" + rev + "_" + "PDF" + "_" + "2");

            }


            Close_Item_Fam();
        }


        public static void Save_Close(string codigo, string rev)
        {
            string item = codigo + "/" + rev + " 1";
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: File->Save
            // ----------------------------------------------
            NXOpen.Part part1;
            part1 = theSession.Parts.Work;

            NXOpen.Part part2;
            part2 = theSession.Parts.Display;

            NXOpen.Expression expression1;
            try
            {
                // No object found with this name
                expression1 = workPart.Expressions.FindObject("mp");
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(3520016);
            }

            NXOpen.Part part3;
            part3 = theSession.Parts.Work;

            NXOpen.Part part4;
            part4 = theSession.Parts.Display;

            NXOpen.Expression expression2;
            try
            {
                // No object found with this name
                expression2 = workPart.Expressions.FindObject("mp");
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(3520016);
            }

            NXOpen.Part part5;
            part5 = theSession.Parts.Work;

            NXOpen.Assemblies.ComponentAssembly componentAssembly1;
            componentAssembly1 = workPart.ComponentAssembly;

            NXOpen.Assemblies.Component component1;
            component1 = componentAssembly1.RootComponent;

            NXOpen.Assemblies.Component[] children1;
            children1 = component1.GetChildren();

            NXOpen.Assemblies.Component[] children2;
            children2 = component1.GetChildren();

            string name1;
            name1 = children1[0].Name;

            string name2;
            name2 = children1[0].DisplayName;

            string name3;
            name3 = children1[0].DisplayName;

            NXOpen.Assemblies.ComponentAssembly componentAssembly2;
            componentAssembly2 = workPart.ComponentAssembly;

            NXOpen.Assemblies.Component component2;
            component2 = componentAssembly2.RootComponent;

            NXOpen.INXObject iNXObject1;
            iNXObject1 = component2.FindObject("COMPONENT " + item);

            NXOpen.Assemblies.Component component3 = ((NXOpen.Assemblies.Component)iNXObject1);
            NXOpen.NXObject.AttributeInformation[] info1;
            info1 = component3.GetUserAttributes();

            NXOpen.Part part6;
            part6 = theSession.Parts.Work;

            NXOpen.Part part7;
            part7 = theSession.Parts.Display;

            string fullPath1;
            fullPath1 = workPart.FullPath;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Save");

            NXOpen.PDM.SmartSaveContext smartSaveContext1;
            smartSaveContext1 = theSession.PdmSession.CreateSmartSaveContext(NXOpen.PDM.SmartSaveBuilder.SaveType.Save);

            NXOpen.PDM.SmartSaveBuilder smartSaveBuilder1;
            smartSaveBuilder1 = theSession.PdmSession.CreateSmartSaveBuilderWithContext(smartSaveContext1);

            NXOpen.PDM.SmartSaveObject[] smartsaveobjects1;
            smartSaveBuilder1.GetSmartSaveObjects(out smartsaveobjects1);

            smartSaveBuilder1.ValidateSmartSaveObjects();

            NXOpen.PDM.ErrorMessageHandler errorMessageHandler1;
            errorMessageHandler1 = smartSaveBuilder1.GetErrorMessageHandler(true);

            NXOpen.NXObject nXObject1;
            nXObject1 = smartSaveBuilder1.Commit();

            NXOpen.PDM.ErrorMessageHandler errorMessageHandler2;
            errorMessageHandler2 = smartSaveBuilder1.GetErrorMessageHandler(true);

            smartSaveBuilder1.Destroy();

            smartSaveContext1.Dispose();
            // ----------------------------------------------
            //   Menu: File->Close->All Parts
            // ----------------------------------------------
            theSession.Parts.CloseAll(NXOpen.BasePart.CloseModified.CloseModified, null);

            workPart = null;
            displayPart = null;
            theSession.ApplicationSwitchImmediate("UG_APP_NOPART");

            // ----------------------------------------------
            //   Menu: Tools->Automation->Journal->Stop Recording
            // ----------------------------------------------

        }
        public static void Close_Item_Fam()
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: File->Close->All Parts
            // ----------------------------------------------
            theSession.Parts.CloseAll(NXOpen.BasePart.CloseModified.CloseModified, null);

            workPart = null;
            displayPart = null;


        }
        public static bool export_pdf(string codigo, string rev)
        {
            LOG_Message("Verificando necessidade de gerar pdf...");
            string formato = verificar_formato();
            double x = 210.0;
            double y = 297.0;


            if (formato != "A4")
            {
                x = 297.0;
                y = 210.0; // workPart.DraftingViews.WorkView.SetScale(0.5, true);
            }
            string item = codigo + "-" + rev;
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Enter Drafting");

            theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");

            workPart.Drafting.EnterDraftingApplication();

            workPart.Views.WorkView.UpdateCustomSymbols();

            theSession.CleanUpFacetedFacesAndEdges();

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            var shetts = workPart.DraftingDrawingSheets.ToArray();
            bool gerar_pdf = false;
            foreach (NXOpen.Drawings.DraftingDrawingSheet item1 in shetts)
            {
                if (item1.Name == "Sheet 1")
                {
                    gerar_pdf = true;
                }

            }
            if (gerar_pdf == true)
            {
                LOG_Message("Gerando PDF do item " + item);
                NXOpen.PrintPDFBuilder printPDFBuilder1;
                printPDFBuilder1 = workPart.PlotManager.CreatePrintPdfbuilder();

                printPDFBuilder1.Relation = NXOpen.PrintPDFBuilder.RelationOption.Manifestation;

                printPDFBuilder1.DeleteDatasets = true;

                printPDFBuilder1.DatasetType = "PDF";

                printPDFBuilder1.NamedReferenceType = "PDF_Reference";

                printPDFBuilder1.Scale = 1.0;

                printPDFBuilder1.Size = NXOpen.PrintPDFBuilder.SizeOption.ScaleFactor;

                printPDFBuilder1.OutputText = NXOpen.PrintPDFBuilder.OutputTextOption.Polylines;

                printPDFBuilder1.Units = NXOpen.PrintPDFBuilder.UnitsOption.English;

                printPDFBuilder1.XDimension = 8.5;

                printPDFBuilder1.YDimension = 11.0;

                printPDFBuilder1.RasterImages = true;

                NXOpen.PDM.PartBuilder.PartFileNameData partInfo1;

                printPDFBuilder1.Assign();

                theSession.SetUndoMarkName(markId2, "Export PDF Dialog");

                printPDFBuilder1.Action = NXOpen.PrintPDFBuilder.ActionOption.Native;

                printPDFBuilder1.Size = NXOpen.PrintPDFBuilder.SizeOption.Dimension;

                printPDFBuilder1.Units = NXOpen.PrintPDFBuilder.UnitsOption.Metric;

                printPDFBuilder1.XDimension = 215.89999999999998;

                printPDFBuilder1.YDimension = 279.39999999999998;

                printPDFBuilder1.XDimension = x;

                NXOpen.Session.UndoMarkId markId3;
                markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export PDF");

                printPDFBuilder1.YDimension = y;

                theSession.DeleteUndoMark(markId3, null);

                NXOpen.Session.UndoMarkId markId4;
                markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export PDF");

                printPDFBuilder1.Watermark = "";


                NXOpen.NXObject[] sheets1 = new NXOpen.NXObject[1];
                NXOpen.Drawings.DraftingDrawingSheet draftingDrawingSheet1 = ((NXOpen.Drawings.DraftingDrawingSheet)workPart.DraftingDrawingSheets.FindObject("Sheet 1"));
                sheets1[0] = draftingDrawingSheet1;
                printPDFBuilder1.SourceBuilder.SetSheets(sheets1);

                printPDFBuilder1.CreateNewFromUi = false;

                printPDFBuilder1.Filename = "S:\\Desenhos\\PDF\\" + item + ".pdf"; ;

                NXOpen.NXObject nXObject1;
                nXObject1 = printPDFBuilder1.Commit();

                theSession.DeleteUndoMark(markId4, null);

                theSession.SetUndoMarkName(markId2, "Export PDF");

                printPDFBuilder1.Destroy();

                theSession.DeleteUndoMark(markId2, null);

                theSession.CleanUpFacetedFacesAndEdges();
            }
            else
            {
                LOG_Message("Arquivo pdf não necessário");
                if (File.Exists(@"C:\POOL\" + codigo + "_" + rev))
                {
                    File.Delete(@"C:\POOL\" + codigo + "_" + rev);
                    LOG_Message("Deletando a solicitação: " + @"C:\POOL\" + codigo + "_" + rev);
                }
            }

            NXOpen.Drawings.DraftingDrawingSheet draftingDrawingSheet2 = ((NXOpen.Drawings.DraftingDrawingSheet)workPart.DraftingDrawingSheets.FindObject("Sheet 1"));
            bool gerar_dxf = false;

            foreach (var item1 in draftingDrawingSheet2.GetDraftingViews())
            {
                if (item1.Name.Contains("FLAT-PATTERN"))
                    gerar_dxf = true;
            }


            return gerar_dxf;
        }
        public static void gerar_dxf_familia(string codigo, string rev)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;



            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            string name_blank = "";
            foreach (Feature item in workPart.Features)
            {

                if (item.GetFeatureName().Contains("Flat Pattern") && (item.Name == "BLANK" || item.Name == "blank"))
                {
                    //MessageBox.Show(item.GetFeatureName() + "  " + item.Name);
                    name_blank = item.GetFeatureName().Replace(' ', '_').ToLower();
                }

            }
            name_blank = name_blank.Replace(' ', '_').ToUpper();

            if (name_blank != "")
            {
                NXOpen.Features.SheetMetal.ExportFlatPatternBuilder exportFlatPatternBuilder1;
                exportFlatPatternBuilder1 = workPart.Features.SheetmetalManager.CreateExportFlatPatternBuilder();
                exportFlatPatternBuilder1.ExportLocation = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.ExportLocationOptions.Native;
                exportFlatPatternBuilder1.OutputFile = @"S:\Desenhos\DXF\" + codigo + "-" + rev + ".dxf";

                exportFlatPatternBuilder1.BendUp = false;

                exportFlatPatternBuilder1.BendDown = false;

                exportFlatPatternBuilder1.InteriorCutout = false;

                exportFlatPatternBuilder1.DxfRevision = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.DxfRevisionType.R20102012;

                NXOpen.Features.FlatPattern flatPattern1 = ((NXOpen.Features.FlatPattern)workPart.Features.FindObject(name_blank));
                exportFlatPatternBuilder1.FlatPattern.Value = flatPattern1;

                exportFlatPatternBuilder1.FlatPattern.Value = flatPattern1;

                theSession.SetUndoMarkName(markId1, "Export Flat Pattern Dialog");

                exportFlatPatternBuilder1.DxfRevision = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.DxfRevisionType.R20102012;

                NXOpen.Session.UndoMarkId markId2;
                markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export Flat Pattern");

                theSession.DeleteUndoMark(markId2, null);

                NXOpen.Session.UndoMarkId markId3;
                markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export Flat Pattern");

                // Potential journal callback detected. Pausing journal.
                // Journal callback ended. Unpausing
                NXOpen.NXObject nXObject1;
                nXObject1 = exportFlatPatternBuilder1.Commit();

                theSession.DeleteUndoMark(markId3, null);

                exportFlatPatternBuilder1.Destroy();
            }
            //   LogFile("DXF gerado com sucesso");
        }

        private void button1_Click_3(object sender, EventArgs e)
        {

            export_pdf("testeddddd", "000");
        }
        public static void LOG_Message(string Mensagem)
        {

            string temp = @"C:\Temp\";


            string gLog_FileName = temp + DateTime.Now.ToString("yyyyMMdd") + "_pdfs_pool.log";
            StreamWriter vWriter = new StreamWriter(gLog_FileName, true, System.Text.Encoding.Unicode);
            vWriter.WriteLine(DateTime.Now.ToString() + " - " + Mensagem);
            vWriter.Close();

        }

        private void button1_Click_4(object sender, EventArgs e)
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

            NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.FindObject("@DB/TPL_CV/000"));
            // Potential journal callback detected. Pausing journal.
            // Journal callback ended. Unpausing
            //NXOpen.BasePart basePart1;
            //NXOpen.PartLoadStatus partLoadStatus1;
            //basePart1 = theSession.Parts.OpenBase("@DB/TPL_CV/000", out partLoadStatus1);

            //partLoadStatus1.Dispose();
            addComponentBuilder1.SetUseReferenceSetAndApplyInitialLocation(false);

            addComponentBuilder1.ReferenceSet = "Entire Part";

            addComponentBuilder1.Layer = -1;

            NXOpen.BasePart[] partstouse1 = new NXOpen.BasePart[1];
            NXOpen.Part part2 = ((NXOpen.Part)part1);
            partstouse1[0] = part2;
            addComponentBuilder1.SetPartsToAdd(partstouse1);

            NXOpen.Assemblies.ProductInterface.InterfaceObject[] productinterfaceobjects1;
            addComponentBuilder1.GetAllProductInterfaceObjects(out productinterfaceobjects1);

            NXOpen.Assemblies.Arrangement arrangement1 = ((NXOpen.Assemblies.Arrangement)workPart.ComponentAssembly.Arrangements.FindObject("Arrangement 1"));
            componentPositioner1.PrimaryArrangement = arrangement1;

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

            componentPositioner2.PrimaryArrangement = arrangement1;

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

        private void button1_Click_5(object sender, EventArgs e)
        {
            string folderPath = @"\\gm-siemens-prod\POOL\DXF";

            List<string> fileList = new List<string>();
            LOG_Message("DXF: Sistema de Geração DXF iniciado");
            while (true)
            {
                LOG_Message("Buscando itens em " + folderPath);
                // Foreach para percorrer a pasta e adicionar os nomes dos arquivos na lista
                foreach (string file in Directory.GetFiles(folderPath))
                {
                    fileList.Add(Path.GetFileName(file)); // Adiciona apenas o nome do arquivo
                    LOG_Message(Path.GetFileName(file));
                }
                LOG_Message("Total de itens para geração " + fileList.Count);
                // Foreach para percorrer a lista de nomes de arquivos
                foreach (string fileName in fileList)
                {
                    lbl_pdf.Text = "Gerando pdf do item " + fileName;

                    string[] split = fileName.Split('_'); // Separar o nome do arquivo em partes


                    LOG_Message("Gerar dxf do item " + fileName);
                    Open_gerar_dxf(split[0], split[1]);

                    string file = @"S:\Desenhos\DXF\" + split[0] + "-" + split[1] + ".pdf";
                    //if (!File.Exists(file))
                    //{
                    //    Open_drawing_fam(split[0], split[1]);
                    //}

                    //MessageBox.Show(folderPath + "\\" + split[0] + "_" + split[1] + "_" + split[2] + "_" + split[3]);
                    //MessageBox.Show(folderPath + "\\DXF_GERADOS\\" + split[0] + "_" + split[1] + "_" + split[2] + "_" + split[3]);

                    File.Move(folderPath + "\\" + split[0] + "_" + split[1] + "_" + split[2] + "_" + split[3],
                        folderPath + "\\DXF_GERADOS\\" + split[0] + "_" + split[1] + "_" + split[2] + "_" + split[3]);

                }
                fileList.Clear();
                DateTime proxima = DateTime.Now.AddMinutes(2);
                lbl_pdf.Text = "Próxima verificação as " + proxima.Hour + "+" + proxima.Minute + ":" + proxima.Second;
                System.Threading.Thread.Sleep(60000);
            }
        }
        static string PartFamilyTemplateFilename(Part thePart)
        {
            UFSession ufSession = UFSession.GetUFSession();
            string familyTemplate = "";
            ufSession.Part.AskTemplateFilename(thePart.Tag, out familyTemplate);
            return familyTemplate;
        }
        public static bool IsPartFamilyInstance(Part thePart)
        {
            bool isFamilyInstance = false;

            // Obtém a sessão UF (User Function)
            UFSession ufSession = UFSession.GetUFSession();

            // Verifica se a peça é um modelo de família usando o método IsFamilyTemplate
            ufSession.Part.IsFamilyInstance(thePart.Tag, out isFamilyInstance);


            return isFamilyInstance;
        }
        private void button6_Click(object sender, EventArgs e)
        {

            string[] items = new string[] {
"265760-003",
"265759-003"
            };
            foreach (string item in items)
            {
                string[] split = item.Split('-');

                int new_rev = Convert.ToInt32(split[1]);
                new_rev = new_rev + 1;
                string teste = gerar_rev_item_fam("421356", split[0], new_rev.ToString().PadLeft(3, '0'));
            }

            Registrar_no_LOG("Fim da execução");

        }
        public static string gerar_rev_item_fam(string Familia, string NomeMembro, string NovaRevisao)
        {
            //try
            //{
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            // Abrir a família
            Registrar_no_LOG($"Abrindo Família: {Familia}");
            PartLoadStatus loadStatus;
            Part objFamilia = theSession.Parts.Work;

            // Obter a primeira tabela de famílias
            UFSession theUfSession = UFSession.GetUFSession();
            Tag[] families;
            theUfSession.Part.AskFamilies(objFamilia.Tag, out int numFamilies, out families);

            if (numFamilies == 0)
            {
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

                if (memberData.values[0] == NomeMembro)
                {
                    memberIndex = i;
                    break;
                }
            }

            if (memberIndex == -1)
            {
                throw new Exception($"Membro '{NomeMembro}' não encontrado na família.");
            }

            // Revisar o membro
            Registrar_no_LOG($"Atualizando Revisão do Membro: {NomeMembro}");
            UFFam.MemberData existingMemberData;
            theUfSession.Fam.AskMemberRowData(families[0], memberIndex, out existingMemberData);

            // Modificar o valor da revisão no campo apropriado (ex.: índice 1 para revisão)
            existingMemberData.values[4] = NovaRevisao;

            // Atualizar o membro na tabela de famílias
            theUfSession.Fam.EditMember(families[0], memberIndex, ref existingMemberData);

            // Atualizar a instância do part
            Tag partTag, instTag;
            bool saved;
            theUfSession.Part.UpdateFamilyInstance(families[0], memberIndex, true, out partTag, out saved, out int count, out Tag[] partList, out int[] errorList, out string info);

            // Salvar alterações
            BasePart objFamiliaPart = (BasePart)NXOpen.Utilities.NXObjectManager.Get(partTag);
            objFamiliaPart.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);

            Registrar_no_LOG($"Revisão '{NovaRevisao}' criada para o membro '{NomeMembro}'.");
            return NomeMembro;
            //}
            //catch (Exception ex)
            //{
            //    Registrar_no_LOG($"Erro: {ex.Message}");
            //    return null;
            //}
        }

        public static void Registrar_no_LOG(string Mensagem)
        {
            string gLog_FileName = @"c:\temp\" + DateTime.Now.ToString("yyyyMMdd") + "_rev_fam.log";
            System.IO.StreamWriter vWriter = new System.IO.StreamWriter(gLog_FileName, true, System.Text.Encoding.Unicode);
            vWriter.WriteLine(DateTime.Now.ToString() + " " + Mensagem);
            vWriter.Close();
        }


        private Body Bruto()
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: Analysis->Measure...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.MeasurePrefsBuilder measurePrefsBuilder1;
            measurePrefsBuilder1 = theSession.Preferences.CreateMeasurePrefsBuilder();

            measurePrefsBuilder1.InfoUnits = NXOpen.MeasurePrefsBuilder.JaMeasurePrefsInfoUnit.CustomUnit;

            measurePrefsBuilder1.ShowValueOnlyToggle = false;

            measurePrefsBuilder1.ConsoleOutput = false;

            theSession.SetUndoMarkName(markId1, "Measure Dialog");

            workPart.MeasureManager.SetPartTransientModification();

            NXOpen.ScCollector scCollector1;
            scCollector1 = workPart.ScCollectors.CreateCollector();

            scCollector1.SetMultiComponent();

            workPart.MeasureManager.SetPartTransientModification();

            NXOpen.SelectionIntentRuleOptions selectionIntentRuleOptions1;
            selectionIntentRuleOptions1 = workPart.ScRuleFactory.CreateRuleOptions();

            selectionIntentRuleOptions1.SetSelectedFromInactive(false);
            Body body1 = null;
            NXOpen.Body[] bodies1 = new NXOpen.Body[1];
            foreach (Feature item in workPart.Features)
            {
                if (item.GetFeatureName().Contains("Bounding"))
                {
                    foreach (Body body in item.GetBodies())
                    {
                        bodies1[0] = body;
                        body1 = body;
                    }
                }
            }
            try
            {


                NXOpen.BodyDumbRule bodyDumbRule1;
                bodyDumbRule1 = workPart.ScRuleFactory.CreateRuleBodyDumb(bodies1, true, selectionIntentRuleOptions1);

                selectionIntentRuleOptions1.Dispose();
                NXOpen.SelectionIntentRule[] rules1 = new NXOpen.SelectionIntentRule[1];
                rules1[0] = bodyDumbRule1;
                scCollector1.ReplaceRules(rules1, false);

                workPart.MeasureManager.SetPartTransientModification();

                NXOpen.ScCollector scCollector2;
                scCollector2 = workPart.ScCollectors.CreateCollector();

                scCollector2.SetMultiComponent();

                NXOpen.Session.UndoMarkId markId2;
                markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Measure");

                theSession.DeleteUndoMark(markId2, null);

                NXOpen.Session.UndoMarkId markId3;
                markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Measure");

                NXOpen.Session.UndoMarkId markId4;
                markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Measurement Apply");

                workPart.MeasureManager.ClearPartTransientModification();

                NXOpen.Session.UndoMarkId markId5;
                markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Measurement Update");

                NXOpen.MeasureMaster measureMaster1;
                measureMaster1 = workPart.MeasureManager.MasterMeasurement();

                measureMaster1.SequenceType = NXOpen.MeasureMaster.Sequence.Free;

                measureMaster1.UpdateAtTimestamp = false;

                measureMaster1.SetNameSuffix("Body");

                NXOpen.Unit[] massUnits1 = new NXOpen.Unit[8];
                NXOpen.Unit unit1 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("MilliMeter"));
                massUnits1[0] = unit1;
                NXOpen.Unit unit2 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("SquareMilliMeter"));
                massUnits1[1] = unit2;
                NXOpen.Unit unit3 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("CubicMilliMeter"));
                massUnits1[2] = unit3;
                NXOpen.Unit unit4 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("KilogramPerCubicMilliMeter"));
                massUnits1[3] = unit4;
                NXOpen.Unit unit5 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("Kilogram"));
                massUnits1[4] = unit5;
                NXOpen.Unit unit6 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("KilogramMilliMeterSquared"));
                massUnits1[5] = unit6;
                NXOpen.Unit unit7 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("KilogramMilliMeter"));
                massUnits1[6] = unit7;
                NXOpen.Unit unit8 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("Newton"));
                massUnits1[7] = unit8;
                NXOpen.MeasureElement measureElement1;
                measureElement1 = workPart.MeasureManager.BodyElement(measureMaster1, massUnits1, 0.98999999999999999, scCollector1);

                measureElement1.MeasureObject1 = NXOpen.MeasureElement.Measure.Object;

                measureElement1.SingleSelect1 = true;

                measureElement1.SelectionIntent1 = 1;

                measureElement1.SetExpressionState(0, true);

                measureElement1.SetGeometryState(0, false);

                measureElement1.SetAnnotationState(0, false);

                measureElement1.SetApproximateState(0, false);

                measureElement1.SetExpressionState(1, true);

                measureElement1.SetGeometryState(1, false);

                measureElement1.SetAnnotationState(1, false);

                measureElement1.SetApproximateState(1, false);

                measureElement1.SetExpressionState(2, true);

                measureElement1.SetGeometryState(2, true);

                measureElement1.SetAnnotationState(2, false);

                measureElement1.SetApproximateState(2, false);

                measureElement1.SetExpressionState(3, true);

                measureElement1.SetGeometryState(3, false);

                measureElement1.SetAnnotationState(3, false);

                measureElement1.SetApproximateState(3, false);

                measureElement1.SetExpressionState(4, true);

                measureElement1.SetGeometryState(4, false);

                measureElement1.SetAnnotationState(4, false);

                measureElement1.SetApproximateState(4, false);

                measureElement1.SetExpressionState(5, false);

                measureElement1.SetGeometryState(5, false);

                measureElement1.SetAnnotationState(5, false);

                measureElement1.SetApproximateState(5, false);

                measureElement1.SetExpressionState(6, false);

                measureElement1.SetGeometryState(6, false);

                measureElement1.SetAnnotationState(6, false);

                measureElement1.SetApproximateState(6, false);

                measureElement1.SetExpressionState(7, true);

                measureElement1.SetGeometryState(7, false);

                measureElement1.SetAnnotationState(7, false);

                measureElement1.SetApproximateState(7, false);

                measureElement1.SetExpressionState(8, false);

                measureElement1.SetGeometryState(8, false);

                measureElement1.SetAnnotationState(8, false);

                measureElement1.SetApproximateState(8, false);

                measureElement1.SetExpressionState(9, false);

                measureElement1.SetGeometryState(9, false);

                measureElement1.SetAnnotationState(9, false);

                measureElement1.SetApproximateState(9, false);

                measureElement1.SetExpressionState(10, false);

                measureElement1.SetGeometryState(10, false);

                measureElement1.SetAnnotationState(10, false);

                measureElement1.SetApproximateState(10, false);

                measureElement1.SetExpressionState(11, false);

                measureElement1.SetGeometryState(11, false);

                measureElement1.SetAnnotationState(11, false);

                measureElement1.SetApproximateState(11, false);

                measureElement1.SetExpressionState(12, true);

                measureElement1.SetGeometryState(12, false);

                measureElement1.SetAnnotationState(12, false);

                measureElement1.SetApproximateState(12, false);

                measureElement1.SetExpressionState(13, false);

                measureElement1.SetGeometryState(13, false);

                measureElement1.SetAnnotationState(13, false);

                measureElement1.SetApproximateState(13, false);

                measureElement1.SetExpressionState(14, false);

                measureElement1.SetGeometryState(14, false);

                measureElement1.SetAnnotationState(14, false);

                measureElement1.SetApproximateState(14, false);

                measureElement1.SetExpressionState(15, true);

                measureElement1.SetGeometryState(15, true);

                measureElement1.SetAnnotationState(15, false);

                measureElement1.SetApproximateState(15, false);

                measureElement1.SetExpressionState(16, true);

                measureElement1.SetGeometryState(16, false);

                measureElement1.SetAnnotationState(16, false);

                measureElement1.SetApproximateState(16, false);

                measureElement1.SetExpressionState(17, true);

                measureElement1.SetGeometryState(17, false);

                measureElement1.SetAnnotationState(17, false);

                measureElement1.SetApproximateState(17, false);

                measureElement1.SetExpressionState(18, true);

                measureElement1.SetGeometryState(18, false);

                measureElement1.SetAnnotationState(18, false);

                measureElement1.SetApproximateState(18, false);

                measureElement1.SetExpressionState(19, false);

                measureElement1.SetGeometryState(19, false);

                measureElement1.SetAnnotationState(19, false);

                measureElement1.SetApproximateState(19, false);

                measureElement1.SetExpressionState(20, false);

                measureElement1.SetGeometryState(20, false);

                measureElement1.SetAnnotationState(20, false);

                measureElement1.SetApproximateState(20, false);

                measureElement1.SetExpressionState(21, false);

                measureElement1.SetGeometryState(21, false);

                measureElement1.SetAnnotationState(21, false);

                measureElement1.SetApproximateState(21, false);

                measureElement1.SetExpressionState(22, false);

                measureElement1.SetGeometryState(22, false);

                measureElement1.SetAnnotationState(22, false);

                measureElement1.SetApproximateState(22, false);

                measureElement1.SetExpressionState(23, false);

                measureElement1.SetGeometryState(23, false);

                measureElement1.SetAnnotationState(23, false);

                measureElement1.SetApproximateState(23, false);

                measureElement1.SetExpressionState(24, false);

                measureElement1.SetGeometryState(24, false);

                measureElement1.SetAnnotationState(24, false);

                measureElement1.SetApproximateState(24, false);

                measureElement1.SetExpressionState(25, false);

                measureElement1.SetGeometryState(25, false);

                measureElement1.SetAnnotationState(25, false);

                measureElement1.SetApproximateState(25, false);

                NXOpen.MeasureElement measureElement2;
                measureElement2 = measureMaster1.GetMeasureElement(0);

                measureElement2.CreateGeometry();

                NXOpen.Point3d position1 = new NXOpen.Point3d(512.14118185465497, 253.66843091911511, 40.027249064458978);
                measureElement2.SetGwifPosition(position1);

                NXOpen.MeasureElement measureElement3;
                measureElement3 = measureMaster1.GetMeasureElement(0);

                workPart.MeasureManager.SetPartTransientModification();

                int nErrs1;
                nErrs1 = theSession.UpdateManager.DoUpdate(markId5);

                theSession.DeleteUndoMark(markId5, "Measurement Update");

                theSession.DeleteUndoMark(markId4, "Measurement Apply");

                bool datadeleted1;
                datadeleted1 = theSession.DeleteTransientDynamicSectionCutData();

                theSession.DeleteUndoMark(markId3, null);

                theSession.SetUndoMarkName(markId1, "Measure");

                measurePrefsBuilder1.ConsoleOutput = false;

                scCollector2.Destroy();

                workPart.MeasureManager.ClearPartTransientModification();

                theSession.CleanUpFacetedFacesAndEdges();
            }
            catch
            {


            }

            return body1;
        }
        public void pesoliquido(Feature[] features)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: Analysis->Measure...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.MeasurePrefsBuilder measurePrefsBuilder1;
            measurePrefsBuilder1 = theSession.Preferences.CreateMeasurePrefsBuilder();

            measurePrefsBuilder1.InfoUnits = NXOpen.MeasurePrefsBuilder.JaMeasurePrefsInfoUnit.CustomUnit;

            measurePrefsBuilder1.ShowValueOnlyToggle = false;

            measurePrefsBuilder1.ConsoleOutput = false;

            theSession.SetUndoMarkName(markId1, "Measure Dialog");

            workPart.MeasureManager.SetPartTransientModification();

            NXOpen.ScCollector scCollector1;
            scCollector1 = workPart.ScCollectors.CreateCollector();

            scCollector1.SetMultiComponent();

            workPart.MeasureManager.SetPartTransientModification();

            NXOpen.SelectionIntentRuleOptions selectionIntentRuleOptions1;
            selectionIntentRuleOptions1 = workPart.ScRuleFactory.CreateRuleOptions();

            selectionIntentRuleOptions1.SetSelectedFromInactive(false);


            NXOpen.DisplayableObject nullNXOpen_DisplayableObject = null;
            NXOpen.BodyFeatureRule bodyFeatureRule1;
            bodyFeatureRule1 = workPart.ScRuleFactory.CreateRuleBodyFeature(features, true, nullNXOpen_DisplayableObject, selectionIntentRuleOptions1);

            selectionIntentRuleOptions1.Dispose();
            NXOpen.SelectionIntentRule[] rules1 = new NXOpen.SelectionIntentRule[1];
            rules1[0] = bodyFeatureRule1;
            scCollector1.ReplaceRules(rules1, false);

            workPart.MeasureManager.SetPartTransientModification();

            NXOpen.ScCollector scCollector2;
            scCollector2 = workPart.ScCollectors.CreateCollector();

            scCollector2.SetMultiComponent();

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Measure");

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Measure");

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Measurement Apply");

            workPart.MeasureManager.ClearPartTransientModification();

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Measurement Update");

            NXOpen.MeasureMaster measureMaster1;
            measureMaster1 = workPart.MeasureManager.MasterMeasurement();

            measureMaster1.SequenceType = NXOpen.MeasureMaster.Sequence.Free;

            measureMaster1.UpdateAtTimestamp = true;

            measureMaster1.SetNameSuffix("Body");

            NXOpen.Unit[] massUnits1 = new NXOpen.Unit[8];
            NXOpen.Unit unit1 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("MilliMeter"));
            massUnits1[0] = unit1;
            NXOpen.Unit unit2 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("SquareMilliMeter"));
            massUnits1[1] = unit2;
            NXOpen.Unit unit3 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("CubicMilliMeter"));
            massUnits1[2] = unit3;
            NXOpen.Unit unit4 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("KilogramPerCubicMilliMeter"));
            massUnits1[3] = unit4;
            NXOpen.Unit unit5 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("Kilogram"));
            massUnits1[4] = unit5;
            NXOpen.Unit unit6 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("KilogramMilliMeterSquared"));
            massUnits1[5] = unit6;
            NXOpen.Unit unit7 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("KilogramMilliMeter"));
            massUnits1[6] = unit7;
            NXOpen.Unit unit8 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("Newton"));
            massUnits1[7] = unit8;
            NXOpen.MeasureElement measureElement1;
            measureElement1 = workPart.MeasureManager.BodyElement(measureMaster1, massUnits1, 0.98999999999999999, scCollector1);

            measureElement1.MeasureObject1 = NXOpen.MeasureElement.Measure.Object;

            measureElement1.SingleSelect1 = true;

            measureElement1.SelectionIntent1 = 0;

            measureElement1.SetExpressionState(0, true);

            measureElement1.SetGeometryState(0, false);

            measureElement1.SetAnnotationState(0, false);

            measureElement1.SetApproximateState(0, false);

            measureElement1.SetExpressionState(1, true);

            measureElement1.SetGeometryState(1, false);

            measureElement1.SetAnnotationState(1, false);

            measureElement1.SetApproximateState(1, false);

            measureElement1.SetExpressionState(2, false);

            measureElement1.SetGeometryState(2, true);

            measureElement1.SetAnnotationState(2, false);

            measureElement1.SetApproximateState(2, false);

            measureElement1.SetExpressionState(3, true);

            measureElement1.SetGeometryState(3, false);

            measureElement1.SetAnnotationState(3, false);

            measureElement1.SetApproximateState(3, false);

            measureElement1.SetExpressionState(4, true);

            measureElement1.SetGeometryState(4, false);

            measureElement1.SetAnnotationState(4, false);

            measureElement1.SetApproximateState(4, false);

            measureElement1.SetExpressionState(5, false);

            measureElement1.SetGeometryState(5, false);

            measureElement1.SetAnnotationState(5, false);

            measureElement1.SetApproximateState(5, false);

            measureElement1.SetExpressionState(6, false);

            measureElement1.SetGeometryState(6, false);

            measureElement1.SetAnnotationState(6, false);

            measureElement1.SetApproximateState(6, false);

            measureElement1.SetExpressionState(7, false);

            measureElement1.SetGeometryState(7, false);

            measureElement1.SetAnnotationState(7, false);

            measureElement1.SetApproximateState(7, false);

            measureElement1.SetExpressionState(8, false);

            measureElement1.SetGeometryState(8, false);

            measureElement1.SetAnnotationState(8, false);

            measureElement1.SetApproximateState(8, false);

            measureElement1.SetExpressionState(9, false);

            measureElement1.SetGeometryState(9, false);

            measureElement1.SetAnnotationState(9, false);

            measureElement1.SetApproximateState(9, false);

            measureElement1.SetExpressionState(10, false);

            measureElement1.SetGeometryState(10, false);

            measureElement1.SetAnnotationState(10, false);

            measureElement1.SetApproximateState(10, false);

            measureElement1.SetExpressionState(11, false);

            measureElement1.SetGeometryState(11, false);

            measureElement1.SetAnnotationState(11, false);

            measureElement1.SetApproximateState(11, false);

            measureElement1.SetExpressionState(12, false);

            measureElement1.SetGeometryState(12, false);

            measureElement1.SetAnnotationState(12, false);

            measureElement1.SetApproximateState(12, false);

            measureElement1.SetExpressionState(13, false);

            measureElement1.SetGeometryState(13, false);

            measureElement1.SetAnnotationState(13, false);

            measureElement1.SetApproximateState(13, false);

            measureElement1.SetExpressionState(14, false);

            measureElement1.SetGeometryState(14, false);

            measureElement1.SetAnnotationState(14, false);

            measureElement1.SetApproximateState(14, false);

            measureElement1.SetExpressionState(15, false);

            measureElement1.SetGeometryState(15, true);

            measureElement1.SetAnnotationState(15, false);

            measureElement1.SetApproximateState(15, false);

            measureElement1.SetExpressionState(16, false);

            measureElement1.SetGeometryState(16, false);

            measureElement1.SetAnnotationState(16, false);

            measureElement1.SetApproximateState(16, false);

            measureElement1.SetExpressionState(17, false);

            measureElement1.SetGeometryState(17, false);

            measureElement1.SetAnnotationState(17, false);

            measureElement1.SetApproximateState(17, false);

            measureElement1.SetExpressionState(18, false);

            measureElement1.SetGeometryState(18, false);

            measureElement1.SetAnnotationState(18, false);

            measureElement1.SetApproximateState(18, false);

            measureElement1.SetExpressionState(19, false);

            measureElement1.SetGeometryState(19, false);

            measureElement1.SetAnnotationState(19, false);

            measureElement1.SetApproximateState(19, false);

            measureElement1.SetExpressionState(20, false);

            measureElement1.SetGeometryState(20, false);

            measureElement1.SetAnnotationState(20, false);

            measureElement1.SetApproximateState(20, false);

            measureElement1.SetExpressionState(21, false);

            measureElement1.SetGeometryState(21, false);

            measureElement1.SetAnnotationState(21, false);

            measureElement1.SetApproximateState(21, false);

            measureElement1.SetExpressionState(22, false);

            measureElement1.SetGeometryState(22, false);

            measureElement1.SetAnnotationState(22, false);

            measureElement1.SetApproximateState(22, false);

            measureElement1.SetExpressionState(23, false);

            measureElement1.SetGeometryState(23, false);

            measureElement1.SetAnnotationState(23, false);

            measureElement1.SetApproximateState(23, false);

            measureElement1.SetExpressionState(24, false);

            measureElement1.SetGeometryState(24, false);

            measureElement1.SetAnnotationState(24, false);

            measureElement1.SetApproximateState(24, false);

            measureElement1.SetExpressionState(25, false);

            measureElement1.SetGeometryState(25, false);

            measureElement1.SetAnnotationState(25, false);

            measureElement1.SetApproximateState(25, false);

            NXOpen.MeasureElement measureElement2;
            measureElement2 = measureMaster1.GetMeasureElement(0);

            measureElement2.CreateGeometry();

            NXOpen.Point3d position1 = new NXOpen.Point3d(514.33698246183189, 134.92854847383663, -23.238918182880155);
            measureElement2.SetGwifPosition(position1);

            NXOpen.MeasureElement measureElement3;
            measureElement3 = measureMaster1.GetMeasureElement(0);

            workPart.MeasureManager.SetPartTransientModification();

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId5);

            theSession.DeleteUndoMark(markId5, "Measurement Update");

            theSession.DeleteUndoMark(markId4, "Measurement Apply");

            bool datadeleted1;
            datadeleted1 = theSession.DeleteTransientDynamicSectionCutData();

            theSession.DeleteUndoMark(markId3, null);

            theSession.SetUndoMarkName(markId1, "Measure");

            measurePrefsBuilder1.ConsoleOutput = false;

            scCollector2.Destroy();

            workPart.MeasureManager.ClearPartTransientModification();
        }
        private static CartesianCoordinateSystem criar_cys(Part workPart, Face face)
        {

            NXOpen.Session theSession = NXOpen.Session.GetSession();

            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: Insert->Datum->Datum CSYS...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.Features.Feature nullNXOpen_Features_Feature = null;
            NXOpen.Features.DatumCsysBuilder datumCsysBuilder1;
            datumCsysBuilder1 = workPart.Features.CreateDatumCsysBuilder(nullNXOpen_Features_Feature);

            NXOpen.Point3d origin1 = new NXOpen.Point3d(0.0, 0.0, 0.0);
            NXOpen.Vector3d normal1 = new NXOpen.Vector3d(0.0, 0.0, 1.0);
            NXOpen.Plane plane1;
            plane1 = workPart.Planes.CreatePlane(origin1, normal1, NXOpen.SmartObject.UpdateOption.WithinModeling);

            NXOpen.Unit unit1 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("MilliMeter"));
            NXOpen.Expression expression1;
            expression1 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            NXOpen.Expression expression2;
            expression2 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            NXOpen.Point3d origin2 = new NXOpen.Point3d(0.0, 0.0, 0.0);
            NXOpen.Vector3d normal2 = new NXOpen.Vector3d(0.0, 0.0, 1.0);
            NXOpen.Plane plane2;
            plane2 = workPart.Planes.CreatePlane(origin2, normal2, NXOpen.SmartObject.UpdateOption.WithinModeling);

            NXOpen.Expression expression3;
            expression3 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            NXOpen.Expression expression4;
            expression4 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            NXOpen.Point3d origin3 = new NXOpen.Point3d(0.0, 0.0, 0.0);
            NXOpen.Vector3d normal3 = new NXOpen.Vector3d(0.0, 0.0, 1.0);
            NXOpen.Plane plane3;
            plane3 = workPart.Planes.CreatePlane(origin3, normal3, NXOpen.SmartObject.UpdateOption.WithinModeling);

            NXOpen.Expression expression5;
            expression5 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            NXOpen.Expression expression6;
            expression6 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            NXOpen.Point3d origin4 = new NXOpen.Point3d(0.0, 0.0, 0.0);
            NXOpen.Vector3d normal4 = new NXOpen.Vector3d(0.0, 0.0, 1.0);
            NXOpen.Plane plane4;
            plane4 = workPart.Planes.CreatePlane(origin4, normal4, NXOpen.SmartObject.UpdateOption.WithinModeling);

            NXOpen.Expression expression7;
            expression7 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            NXOpen.Expression expression8;
            expression8 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            NXOpen.Unit unit2 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("Degrees"));
            NXOpen.Expression expression9;
            expression9 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit2);

            NXOpen.Expression expression10;
            expression10 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit2);

            NXOpen.Expression expression11;
            expression11 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit2);

            NXOpen.Expression expression12;
            expression12 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            NXOpen.Expression expression13;
            expression13 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit2);

            NXOpen.Expression expression14;
            expression14 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            NXOpen.Expression expression15;
            expression15 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit2);

            NXOpen.Expression expression16;
            expression16 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            NXOpen.Expression expression17;
            expression17 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit2);

            expression9.SetFormula("0");

            expression10.SetFormula("0");

            expression11.SetFormula("0");

            expression12.SetFormula("0");

            expression14.SetFormula("0");

            expression16.SetFormula("0");

            expression13.SetFormula("0");

            expression15.SetFormula("0");

            expression17.SetFormula("0");

            expression12.RightHandSide = "0";

            expression14.RightHandSide = "0";

            expression16.RightHandSide = "0";

            expression13.RightHandSide = "0";

            expression15.RightHandSide = "0";

            expression17.RightHandSide = "0";

            expression9.SetFormula("0");

            expression10.SetFormula("0");

            expression11.SetFormula("0");

            expression12.SetFormula("0");

            expression14.SetFormula("0");

            expression16.SetFormula("0");

            expression13.SetFormula("0");

            expression15.SetFormula("0");

            expression17.SetFormula("0");

            theSession.SetUndoMarkName(markId1, "Datum CSYS Dialog");


            NXOpen.Xform xform1;
            xform1 = workPart.Xforms.CreateXform(face, NXOpen.SmartObject.UpdateOption.WithinModeling);

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Datum CSYS");

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Datum CSYS");

            NXOpen.CartesianCoordinateSystem cartesianCoordinateSystem1;
            cartesianCoordinateSystem1 = workPart.CoordinateSystems.CreateCoordinateSystem(xform1, NXOpen.SmartObject.UpdateOption.WithinModeling);

            theSession.DeleteUndoMark(markId3, null);

            theSession.SetUndoMarkName(markId1, "Datum CSYS");

            workPart.Expressions.Delete(expression12);

            workPart.Expressions.Delete(expression14);

            workPart.Expressions.Delete(expression16);

            workPart.Expressions.Delete(expression13);

            workPart.Expressions.Delete(expression15);

            workPart.Expressions.Delete(expression17);

            workPart.Expressions.Delete(expression9);

            workPart.Expressions.Delete(expression10);

            workPart.Expressions.Delete(expression11);

            try
            {
                // Expression is still in use.
                workPart.Expressions.Delete(expression2);
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(1050029);
            }

            try
            {
                // Expression is still in use.
                workPart.Expressions.Delete(expression4);
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(1050029);
            }

            try
            {
                // Expression is still in use.
                workPart.Expressions.Delete(expression6);
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(1050029);
            }

            try
            {
                // Expression is still in use.
                workPart.Expressions.Delete(expression8);
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(1050029);
            }

            try
            {
                // Expression is still in use.
                workPart.Expressions.Delete(expression1);
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(1050029);
            }

            plane1.DestroyPlane();

            try
            {
                // Expression is still in use.
                workPart.Expressions.Delete(expression3);
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(1050029);
            }

            plane2.DestroyPlane();

            try
            {
                // Expression is still in use.
                workPart.Expressions.Delete(expression5);
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(1050029);
            }

            plane3.DestroyPlane();

            try
            {
                // Expression is still in use.
                workPart.Expressions.Delete(expression7);
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(1050029);
            }

            plane4.DestroyPlane();

            datumCsysBuilder1.Csys = cartesianCoordinateSystem1;

            datumCsysBuilder1.DisplayScaleFactor = 1.25;

            NXOpen.NXObject nXObject1;
            nXObject1 = datumCsysBuilder1.Commit();

            datumCsysBuilder1.Destroy();

            theSession.CleanUpFacetedFacesAndEdges();
            return cartesianCoordinateSystem1;
            // Obter o primeiro body visível no modelo

        }
        public static void flatt_pattern(Face face)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: Insert->Flat Pattern->Flat Pattern...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.Features.Feature nullNXOpen_Features_Feature = null;
            NXOpen.Features.SheetMetal.FlatPatternBuilder flatPatternBuilder1;
            flatPatternBuilder1 = workPart.Features.SheetmetalManager.CreateFlatPatternBuilder(nullNXOpen_Features_Feature);

            flatPatternBuilder1.SetApplicationContext(NXOpen.Features.SheetMetal.ApplicationContext.NxSheetMetal);

            flatPatternBuilder1.TransformToAbsoluteCsys = false;

            flatPatternBuilder1.OuterCornerTreatment.Value.SetFormula("0.1");

            flatPatternBuilder1.InnerCornerTreatment.Value.SetFormula("0.1");

            flatPatternBuilder1.HoleTreatment.Diameter.SetFormula("0.1");

            flatPatternBuilder1.FixAtTimestamp = true;

            flatPatternBuilder1.SetApplicationContext(NXOpen.Features.SheetMetal.ApplicationContext.NxSheetMetal);

            flatPatternBuilder1.KeepFlatSolidExternal = false;


            theSession.SetUndoMarkName(markId1, "Flat Pattern Dialog");

            NXOpen.Features.SheetMetal.FeatureProperty insidecornertreatmenttype1;
            insidecornertreatmenttype1 = workPart.Preferences.SheetMetalPreferences.GetInsideCornerTreatmentType();

            NXOpen.Features.SheetMetal.CornerTreatmentBuilder cornerTreatmentBuilder1;
            cornerTreatmentBuilder1 = flatPatternBuilder1.InnerCornerTreatment;

            cornerTreatmentBuilder1.TreatmentType = NXOpen.Features.SheetMetal.CornerTreatmentBuilder.CornerTreatmentType.None;

            NXOpen.Expression expression1;
            expression1 = cornerTreatmentBuilder1.Value;

            NXOpen.Expression expression2;
            expression2 = workPart.Preferences.SheetMetalPreferences.GetFlatPatternInsideCornerTreatmentValue();

            string name1;
            name1 = expression2.Name;

            expression1.RightHandSide = name1;

            NXOpen.Features.SheetMetal.FeatureProperty outsidecornertreatmenttype1;
            outsidecornertreatmenttype1 = workPart.Preferences.SheetMetalPreferences.GetOutsideCornerTreatmentType();

            NXOpen.Features.SheetMetal.CornerTreatmentBuilder cornerTreatmentBuilder2;
            cornerTreatmentBuilder2 = flatPatternBuilder1.OuterCornerTreatment;

            cornerTreatmentBuilder2.TreatmentType = NXOpen.Features.SheetMetal.CornerTreatmentBuilder.CornerTreatmentType.None;

            NXOpen.Expression expression3;
            expression3 = cornerTreatmentBuilder2.Value;

            NXOpen.Expression expression4;
            expression4 = workPart.Preferences.SheetMetalPreferences.GetFlatPatternOutsideCornerTreatmentValue();

            string name2;
            name2 = expression4.Name;

            expression3.RightHandSide = name2;

            flatPatternBuilder1.UpwardFace.Value = face;

            expression3.SetFormula(name2);

            expression1.SetFormula(name1);

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Flat Pattern");

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Flat Pattern");

            flatPatternBuilder1.BendDirectionUpText = "Up";

            flatPatternBuilder1.BendDirectionDownText = "Down";

            theSession.Preferences.Sketch.CreateInferredConstraints = false;

            theSession.Preferences.Sketch.ContinuousAutoDimensioning = false;

            theSession.Preferences.Sketch.DimensionLabel = NXOpen.Preferences.SketchPreferences.DimensionLabelType.Expression;

            theSession.Preferences.Sketch.TextSizeFixed = false;

            theSession.Preferences.Sketch.FixedTextSize = 3.0;

            theSession.Preferences.Sketch.DisplayParenthesesOnReferenceDimensions = true;

            theSession.Preferences.Sketch.DisplayReferenceGeometry = false;

            theSession.Preferences.Sketch.DisplayShadedRegions = true;

            theSession.Preferences.Sketch.FindMovableObjects = true;

            theSession.Preferences.Sketch.ConstraintSymbolSize = 3.0;

            theSession.Preferences.Sketch.DisplayObjectColor = false;

            theSession.Preferences.Sketch.DisplayObjectName = true;

            theSession.Preferences.Sketch.EditDimensionOnCreation = true;

            theSession.Preferences.Sketch.CreateDimensionForTypedValues = true;

            theSession.Preferences.Sketch.AddRemoteFindingAfterSnap = true;


            NXOpen.Features.Feature feature1;
            feature1 = flatPatternBuilder1.CommitFeature();
            feature1.SetName("BLANK");
            theSession.DeleteUndoMark(markId3, null);

            theSession.SetUndoMarkName(markId1, "Flat Pattern BLANK");

            NXOpen.Expression expression5 = flatPatternBuilder1.HoleTreatment.Diameter;
            flatPatternBuilder1.Destroy();

            theSession.CleanUpFacetedFacesAndEdges();

            // ----------------------------------------------
            //   Menu: Tools->Automation->Journal->Stop Recording
            // ----------------------------------------------

        }
        private void button4_Click(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;


            NXOpen.CartesianCoordinateSystem cartesianCoordinateSystem1 = null;
            NXOpen.Features.Feature[] features1 = new NXOpen.Features.Feature[1];
            Body body1 = null;

            bool flat = false;
            var ret = criar_flat();
            features1[0] = ret.Item1;
            Face face = ret.Item2;
            pesoliquido(features1);
            flat = true;

            if (flat == true)
            {
                Feature item = features1[0];
                cartesianCoordinateSystem1 = criar_cys(workPart, face);

                NXOpen.Session.UndoMarkId markId1;
                markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

                NXOpen.Features.ToolingBox nullNXOpen_Features_ToolingBox = null;
                NXOpen.Features.ToolingBoxBuilder toolingBoxBuilder1;
                toolingBoxBuilder1 = workPart.Features.ToolingFeatureCollection.CreateToolingBoxBuilder(nullNXOpen_Features_ToolingBox);

                toolingBoxBuilder1.Type = NXOpen.Features.ToolingBoxBuilder.Types.BoundedBlock;

                toolingBoxBuilder1.ReferenceCsysType = NXOpen.Features.ToolingBoxBuilder.RefCsysType.SelectedCsys;

                toolingBoxBuilder1.XValue.SetFormula("10");

                toolingBoxBuilder1.YValue.SetFormula("10");

                toolingBoxBuilder1.ZValue.SetFormula("10");

                toolingBoxBuilder1.OffsetPositiveX.SetFormula("0");

                toolingBoxBuilder1.OffsetNegativeX.SetFormula("0");

                toolingBoxBuilder1.OffsetPositiveY.SetFormula("0");

                toolingBoxBuilder1.OffsetNegativeY.SetFormula("0");

                toolingBoxBuilder1.OffsetPositiveZ.SetFormula("0");

                toolingBoxBuilder1.OffsetNegativeZ.SetFormula("0");

                toolingBoxBuilder1.RadialOffset.SetFormula("0");

                toolingBoxBuilder1.Clearance.SetFormula("0");

                theSession.SetUndoMarkName(markId1, "Bounding Body Dialog");


                toolingBoxBuilder1.CsysSelection.Value = cartesianCoordinateSystem1;

                NXOpen.Matrix3x3 matrix1 = cartesianCoordinateSystem1.Orientation.Element;


                // 🔹 Obter a posição de origem do CartesianCoordinateSystem
                NXOpen.Point3d position1 = cartesianCoordinateSystem1.Origin;

                // 🔹 Aplicar ao ToolingBoxBuilder
                toolingBoxBuilder1.SetBoxMatrixAndPosition(matrix1, position1);
                toolingBoxBuilder1.SetBoxMatrixAndPosition(matrix1, position1);

                NXOpen.SelectionIntentRuleOptions selectionIntentRuleOptions1;
                selectionIntentRuleOptions1 = workPart.ScRuleFactory.CreateRuleOptions();

                selectionIntentRuleOptions1.SetSelectedFromInactive(false);


                NXOpen.DisplayableObject nullNXOpen_DisplayableObject = null;
                NXOpen.BodyFeatureRule bodyFeatureRule1;
                bodyFeatureRule1 = workPart.ScRuleFactory.CreateRuleBodyFeature(features1, true, nullNXOpen_DisplayableObject, selectionIntentRuleOptions1);

                selectionIntentRuleOptions1.Dispose();
                NXOpen.ScCollector scCollector1;
                scCollector1 = toolingBoxBuilder1.BoundedObject;

                NXOpen.SelectionIntentRule[] rules1 = new NXOpen.SelectionIntentRule[1];
                rules1[0] = bodyFeatureRule1;
                scCollector1.ReplaceRules(rules1, false);

                NXOpen.NXObject[] selections1 = new NXOpen.NXObject[1];
                selections1[0] = body1;
                NXOpen.NXObject[] deselections1 = new NXOpen.NXObject[0];
                toolingBoxBuilder1.SetSelectedOccurrences(selections1, deselections1);

                NXOpen.SelectNXObjectList selectNXObjectList1;
                selectNXObjectList1 = toolingBoxBuilder1.FacetBodies;

                NXOpen.NXObject[] objects1 = new NXOpen.NXObject[0];
                bool added1;
                added1 = selectNXObjectList1.Add(objects1);

                toolingBoxBuilder1.CalculateBoxSize();

                NXOpen.Point3d csysorigin1 = new NXOpen.Point3d(506.59728962793713, 332.83605520325125, 22.450648973221305);
                toolingBoxBuilder1.BoxPosition = csysorigin1;

                NXOpen.Session.UndoMarkId markId2;
                markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Bounding Body");

                theSession.DeleteUndoMark(markId2, null);

                NXOpen.Session.UndoMarkId markId3;
                markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Bounding Body");

                NXOpen.NXObject nXObject1;
                nXObject1 = toolingBoxBuilder1.Commit();

                theSession.DeleteUndoMark(markId3, null);

                theSession.SetUndoMarkName(markId1, "Bounding Body");

                NXOpen.Expression expression1 = toolingBoxBuilder1.OffsetPositiveZ;
                toolingBoxBuilder1.Destroy();

                theSession.CleanUpFacetedFacesAndEdges();
            }

            Body body_bruto = Bruto();

            string name_exp_mass_bruta = "";
            string v_exp_mass_bruta = "";
            string name_exp_mass_liq = "";
            string v_exp_mass_liq = "";
            string material_name = "";

            string type_feature_pb = "";
            string name_feature_pb = "";

            // Feature remove_ref = null;

            NXOpen.Body body_remove = null;
            foreach (Feature feature in workPart.Features)
            {
                string name = feature.GetFeatureName();
                if (name.Contains("Bounding Body"))
                {
                    Expression[] expressions = feature.GetExpressions();

                    foreach (Body body in feature.GetBodies())
                    {
                        body_remove = body;
                    }

                    Feature[] parents = feature.GetChildren();

                    foreach (Feature parent in parents)
                    {
                        type_feature_pb = parent.FeatureType.ToString();
                        name_feature_pb = parent.GetFeatureName();
                        Expression[] exp = parent.GetExpressions();
                        foreach (Expression exp2 in exp)
                        {
                            NXOpen.Unit unidades = exp2.Units;
                            try
                            {
                                if (unidades.TypeName == "Kilogram" && exp2.Value != 0)
                                {
                                    name_exp_mass_bruta = exp2.Name;
                                    v_exp_mass_bruta = exp2.Value.ToString();
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            Feature[] _PESO_LIQUIDO = features1[0].GetChildren();

            foreach (Feature feature in _PESO_LIQUIDO)
            {
                if (feature.FeatureType == "GENERIC MEASUREMENT")
                {
                    Expression[] exp = feature.GetExpressions();
                    foreach (Expression exp2 in exp)
                    {
                        NXOpen.Unit unidades = exp2.Units;
                        try
                        {
                            if (unidades.TypeName == "Kilogram" && exp2.Value != 0)
                            {
                                name_exp_mass_liq = exp2.Name;
                                v_exp_mass_liq = exp2.Value.ToString();

                                break;
                            }
                        }
                        catch
                        {
                        }
                    }
                }

            }
            foreach (Expression exp in workPart.Expressions)
            {
                if (exp.Name == "NX_Material")
                {

                    material_name = exp.StringValue;
                }
                if (exp.Name == "mp")
                {
                    material_name = exp.StringValue;
                }
            }
            aplicar_material(body_bruto, material_name);

            preenche_expression("QTD_BRUTO", name_exp_mass_bruta, workPart);
            preenche_expression("QTD", name_exp_mass_liq, workPart);

            theSession.ListingWindow.Open();
            theSession.ListingWindow.WriteLine($"PESO LIQUIDO: {v_exp_mass_liq.ToString()}");
            theSession.ListingWindow.WriteLine($"  PESO BRUTO: {v_exp_mass_bruta.ToString()}");
            NXOpen.ReferenceSet referenceSet1 = null;
            NXOpen.NXObject[] components1 = new NXOpen.NXObject[1];
            components1[0] = body_remove;
            foreach (ReferenceSet reference in workPart.GetAllReferenceSets())
            {

                if (reference.Name == "MODEL")
                {
                    referenceSet1 = reference;
                }
            }

            referenceSet1.RemoveObjectsFromReferenceSet(components1);

            NXOpen.DisplayableObject[] objects2 = new NXOpen.DisplayableObject[1];

            objects2[0] = body_remove;
            theSession.DisplayManager.BlankObjects(objects2);

            NXOpen.DisplayableObject[] objects3 = new NXOpen.DisplayableObject[1];

            objects3[0] = body1;
            theSession.DisplayManager.BlankObjects(objects3);

            NXOpen.DisplayableObject[] objects4 = new NXOpen.DisplayableObject[1];
            objects4[0] = cartesianCoordinateSystem1;
            theSession.DisplayManager.BlankObjects(objects4);
            NXOpen.DisplayableObject[] objects5 = new NXOpen.DisplayableObject[1];



            foreach (Feature feature in features1)
            {
                foreach (Body body in feature.GetBodies())
                {

                    objects5[0] = body;
                    theSession.DisplayManager.BlankObjects(objects5);
                }

            }

            foreach (Feature feature in workPart.Features)
            {

                if (feature.FeatureType == "GENERIC MEASUREMENT")
                {


                    List<NXOpen.DisplayableObject> objetosParaOcultar = new List<NXOpen.DisplayableObject>();

                    // Percorre os objetos associados à feature
                    foreach (NXOpen.NXObject child in feature.GetEntities())
                    {
                        if (child is NXOpen.DisplayableObject displayable)
                        {
                            objetosParaOcultar.Add(displayable);
                        }
                    }

                    if (objetosParaOcultar.Count > 0)
                    {
                        theSession.DisplayManager.BlankObjects(objetosParaOcultar.ToArray());
                    }
                }


            }
            List<Feature> featureList = new List<Feature>();

            // Exemplo: Filtra todas as features do tipo "SB_FLAT_SOLID"
            foreach (Feature feature in workPart.Features)
            {
                if (feature.FeatureType == "SB_FLAT_SOLID")
                {
                    featureList.Add(feature);
                }
            }

            flatt_pattern(face);

            //NXOpen.ReferenceSet referenceSet1 = ((NXOpen.ReferenceSet)workPart.FindObject("HANDLE R-649"));
            //NXOpen.NXObject[] components1 = new NXOpen.NXObject[1];
            //NXOpen.Body body1 = ((NXOpen.Body)workPart.Bodies.FindObject("Bounding Body(8)"));
            //components1[0] = body1;
            //referenceSet1.RemoveObjectsFromReferenceSet(components1);

            //NXOpen.Session theSession = NXOpen.Session.GetSession();
            //Part workPart = theSession.Parts.Work;
            //Feature[] allFeatures = workPart.Features.ToArray();
            //double targetLength = 60.88; // Comprimento desejado para comparação

            //foreach (Feature feature in allFeatures)
            //{
            //    if (feature is BodyFeature bodyFeature)
            //    {
            //        foreach (Body body in bodyFeature.GetBodies())
            //        {

            //          //  MessageBox.Show($"Analisando o body: {body.Name}");

            //            // Obter todas as edges do corpo
            //            Edge[] edges = body.GetEdges();
            //            foreach (Edge edge in edges)
            //            {
            //                // Obter o comprimento da edge
            //                double edgeLength = edge.GetLength();
            //              //MessageBox.Show($"Edge comprimento: {edgeLength}");

            //                // Comparar com o valor de entrada
            //                if (Math.Round(edgeLength,2) == targetLength)
            //                {
            //                    MessageBox.Show($"Edge encontrada com o comprimento alvo: {edgeLength}, body:{feature.Name}, comprimento: {edgeLength} ");
            //                    //Console.WriteLine($"Edge encontrada com o comprimento alvo: {edgeLength}");
            //                }
            //            }
            //        }
            //    }
            //}
        }


        public static void aplicar_material(Body body, string material_name)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: Tools->Materials->Assign Materials...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.PhysMat.PhysicalMaterialListBuilder physicalMaterialListBuilder1;
            physicalMaterialListBuilder1 = workPart.MaterialManager.PhysicalMaterials.CreateListBlockBuilder();

            NXOpen.PhysMat.PhysicalMaterialAssignBuilder physicalMaterialAssignBuilder1;
            physicalMaterialAssignBuilder1 = workPart.MaterialManager.PhysicalMaterials.CreateMaterialAssignBuilder();

            theSession.SetUndoMarkName(markId1, "Assign Material Dialog");

            NXOpen.Session.UndoMarkId id1;
            id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.AnyVisibility);

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Assign Material");

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Assign Material");
            string _material = "PhysicalMaterial[" + material_name + "]";
            MessageBox.Show(_material);
            NXOpen.PhysicalMaterial physicalMaterial1 = ((NXOpen.PhysicalMaterial)workPart.MaterialManager.PhysicalMaterials.FindObject(_material));
            NXOpen.NXObject[] objects1 = new NXOpen.NXObject[1];
            objects1[0] = body;
            physicalMaterial1.AssignObjects(objects1);

            theSession.DeleteUndoMark(markId3, null);

            theSession.SetUndoMarkName(id1, "Assign Material");

            physicalMaterialAssignBuilder1.Destroy();

            physicalMaterialListBuilder1.Destroy();

            theSession.CleanUpFacetedFacesAndEdges();

            // ----------------------------------------------
            //   Menu: Tools->Automation->Journal->Stop Recording
            // ----------------------------------------------

        }
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
        public static void Registrar_Dados(string itens)
        {
            string item_pai = @"C:\Temp\familias-pai-filho.csv";
            System.IO.StreamWriter vWriter = new System.IO.StreamWriter(item_pai, true);
            vWriter.WriteLine(itens);
            vWriter.Close();
        }
        private void criar_rev_item_no_familia(object sender, EventArgs e)
        {
            string[] items = new string[] {
"511440-000",
"511443-000",
"511445-000",
"511455-000",
"511458-000",
"511459-000",
"511461-000",
"511493-000",
"511523-000",
"511537-000",
"511629-000",
"511656-000",
"511679-000",
"511681-000",
"511682-000",
"511686-000",
"511688-000",
"511698-000",
"511701-000",
"511705-000",
"511709-000",
"511711-000",
"511712-000",
"511715-000",
"511722-000",
"511723-000",
"511743-000",
"511747-000",
"511759-000",
"511766-000",
"511767-000",
"511769-000",
"511770-000",
"511771-000",
"511776-000",
"511777-000",
"511779-000",
"511780-000",
"511869-000",
"511935-000",
"511991-000",
"511993-000",
"511994-000",
"512000-000",
"512005-000",
"512059-000",
"512111-000",
"512389-000",
"512390-000",
"512454-000",
"512576-000",
"512598-000",
"512604-000",
"512616-000",
"512618-000",
"512662-000",
"512672-000",
"512682-000",
"512734-000",
"512746-000",
"512771-000",
"512773-000",
"512775-000",
"512776-000",
"512777-000",
"512778-000",
"512781-000",
"512783-000",
"512802-000",
"512804-000",
"512807-000",
"512810-000",
"512819-000",
"512828-000",
"512829-000",
"512833-000",
"512842-000",
"512844-000",
"512846-000",
"512848-000",
"512851-000",
"512853-000",
"512863-000",
"512864-000",
"512865-000",
"512866-000",
"512867-000",
"512870-000",
"512871-000",
"512894-000",
"512948-000",
"512979-000",
"512980-000",
"512996-000",
"512999-000",
"513038-000",
"513072-000",
"513074-000",
"513088-000",
"513089-000",
"513102-000",
"513116-000",
"513123-000",
"513136-000",
"513155-000",
"513172-000",
"513173-000",
"513174-000",
"513179-000",
"513184-000",
"513187-000",
"513203-000",
"513207-000",
"513209-000",
"513213-000",
"513214-000",
"513215-000",
"513216-000",
"513219-000",
"513236-000",
"513251-000",
"513297-000",
"513299-000",
"513301-000",
"513303-000",
"513310-000",
"513353-000",
"513385-000",
"513386-000",
"513409-000",
"513411-000",
"513412-000",
"513414-000",
"513415-000",
"513423-000",
"513428-000",
"513438-000",
"513440-000",
"513454-000",
"513459-000",
"513463-000",
"513466-000",
"513484-000",
"513516-000",
"513531-000",
"513533-000",
"513535-000",
"513537-000",
"513541-000",
"513542-000",
"513549-000",
"513601-000",
"513603-000",
"513604-000",
"513632-000",
"513638-000",
"513639-000",
"513649-000",
"513657-000",
"513658-000",
"513659-000",
"513667-000",
"513675-000",
"513683-000",
"513692-000",
"513731-000",
"513742-000",
"513763-000",
"513766-000",
"513768-000",
"513783-000",
"513785-000",
"513786-000",
"513787-000",
"513803-000",
"513811-000",
"513812-000",
"513822-000",
"513830-000",
"513831-000",
"513834-000",
"513845-000",
"513849-000",
"513851-000",
"513858-000",
"513861-000",
"513863-000",
"513864-000",
"513867-000",
"513868-000",
"513869-000",
"513872-000",
"513878-000",
"513879-000",
"513881-000",
"513884-000",
"513885-000",
"513886-000",
"513887-000",
"513889-000",
"513890-000",
"513891-000",
"513892-000",
"513893-000",
"513894-000",
"513895-000",
"513896-000",
"513898-000",
"513901-000",
"513905-000",
"513908-000",
"513910-000",
"513939-000",
"513942-000",
"513955-000",
"513966-000",
"513969-000",
"513971-000",
"513997-000",
"514002-000",
"514004-000",
"514005-000",
"514036-000",
"514053-000",
"514064-000",
"514066-000",
"514070-000",
"514072-000",
"514073-000",
"514074-000",
"514076-000",
"514077-000",
"514078-000",
"514095-000",
"514101-000",
"514103-000",
"514106-000",
"514109-000",
"514111-000",
"514115-000",
"514117-000",
"514118-000",
"514124-000",
"514139-000",
"514143-000",
"514187-000",
"514188-000",
"514215-000",
"514216-000",
"514231-000",
"514234-000",
"514236-000",
"514245-000",
"514254-000",
"514255-000",
"514256-000",
"514258-000",
"514260-000",
"514261-000",
"514276-000",
"514289-000",
"514292-000",
"514298-000",
"514306-000",
"514363-000",
"514364-000",
"514367-000",
"514370-000",
"514372-000",
"514374-000",
"514376-000",
"514377-000",
"514378-000",
"514383-000",
"514384-000",
"514399-000",
"514401-000",
"514408-000",
"514409-000",
"514420-000",
"514437-000",
"514438-000",
"514440-000",
"514441-000",
"514442-000",
"514443-000",
"514457-000",
"514480-000",
"514531-000",
"514537-000",
"514548-000",
"514549-000",
"514569-000",
"514579-000",
"514580-000",
"514596-000",
};

            foreach (string item in items)
            {
                string[] split = item.Split('-');
                Open_gerar_dxf(split[0], split[1]);


                NXOpen.Session theSession = NXOpen.Session.GetSession();
                NXOpen.Session.UndoMarkId markId1;
                markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Load Part");

                NXOpen.BasePart basePart1;
                NXOpen.PartLoadStatus partLoadStatus1;
                basePart1 = theSession.Parts.OpenActiveDisplay("@DB/" + split[0] + "/" + split[1], NXOpen.DisplayPartOption.AllowAdditional, out partLoadStatus1);

                NXOpen.Part workPart = theSession.Parts.Work;
                NXOpen.Part displayPart = theSession.Parts.Display;

                Rotinas_Startup.Program.Verificar_Blank_Fam();
                Rotinas_Startup.Program.item_fam();

                workPart.Close(BasePart.CloseWholeTree.False, BasePart.CloseModified.CloseModified, null);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string[] items = new string[] {

"264164-002",
"265751-003",
"279247-003",
"282004-000",
"287859-000",
"291256-000",
"299881-000",
"312714-000",
"317406-000",
"320465-000",
"320705-001",
"322953-000",
"328278-000",
"328761-000",
"343083-001",
"347024-000",
"356008-000",
"358952-000",
"363667-000",
"364747-001",
"365088-000",
"381219-000",
"381909-000",
"382094-000",
"385915-000",
"387379-000",
"387545-000",
"390393-000",
"392728-000",
"395153-000",
"399296-000",
"399504-000",
"404364-000",
"404864-000",
"405801-000",
"406495-000",
"409172-000",
"412753-000",
"413714-000",
"414228-000",
"415212-000",
"416367-000",
"428086-000",
"429641-000",
"430264-000",
"433064-000",
"437677-000",
"438741-000",
"440276-000",
"441119-000",
"441387-000",
"445131-000",
"445911-000",
"447438-000",
"447899-000",
"449322-000",
"451588-000",
"455453-000",
"458308-000",
"459064-000",
"461780-000",
"462900-000",
"464308-000",
"464593-000",
"465339-000",
"466218-000",
"468025-000",
"468635-000",
"470362-000",
"472501-000",
"475521-000",
"478362-000",
"478890-000",
"479646-001",
"482899-000",
"486980-000",
"489948-000",
"490639-000",
"492303-000",
"497736-000",
"498602-000",
"499824-000",
"499988-000",
"503112-000",
"504894-001",
"509715-000",
"511542-000",
"513047-000",
"513646-000",
"514006-000",
"514686-000",
"515471-000",
"516381-000",
"516804-000",
"516983-001",
"517710-000"

            };
            Registrar_no_LOG("Registrar alteração");
            foreach (string item in items)
            {
                string[] split = item.Split('-');

                int new_rev = Convert.ToInt32(split[1]);
                new_rev = new_rev + 1;

                Registrar_no_LOG("Item" + split[0] + "-" + new_rev.ToString());

                DateTime d1 = DateTime.Now;
                string teste = conexao_bd.gravar_alteracao(split[0], new_rev.ToString(), d1.ToString("yyyy/MM/dd hh:mm:ss"), "107", "2",
                    "S", "ALTERADO CONFORME SAP 555", "Alteração sob demanda", "22", "0", "-", "2");
            }

            Registrar_no_LOG("Fim da execução");
        }
        public static void gerar_dxf_demanda(string codigo, string rev)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;



            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            string name_blank = "";
            foreach (Feature item in workPart.Features)
            {

                if (item.GetFeatureName().Contains("Flat Pattern") && (item.Name == "BLANK" || item.Name == "blank"))
                {
                    //MessageBox.Show(item.GetFeatureName() + "  " + item.Name);
                    name_blank = item.GetFeatureName().Replace(' ', '_').ToLower();
                }

            }
            name_blank = name_blank.Replace(' ', '_').ToUpper();

            if (name_blank != "")
            {
                NXOpen.Features.SheetMetal.ExportFlatPatternBuilder exportFlatPatternBuilder1;
                exportFlatPatternBuilder1 = workPart.Features.SheetmetalManager.CreateExportFlatPatternBuilder();
                exportFlatPatternBuilder1.ExportLocation = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.ExportLocationOptions.Native;
                exportFlatPatternBuilder1.OutputFile = @"S:\Desenhos\DXF\" + codigo + "-" + rev + ".dxf";

                exportFlatPatternBuilder1.BendUp = false;

                exportFlatPatternBuilder1.BendDown = false;

                exportFlatPatternBuilder1.InteriorCutout = false;

                exportFlatPatternBuilder1.DxfRevision = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.DxfRevisionType.R20102012;

                NXOpen.Features.FlatPattern flatPattern1 = ((NXOpen.Features.FlatPattern)workPart.Features.FindObject(name_blank));
                exportFlatPatternBuilder1.FlatPattern.Value = flatPattern1;

                exportFlatPatternBuilder1.FlatPattern.Value = flatPattern1;

                theSession.SetUndoMarkName(markId1, "Export Flat Pattern Dialog");

                exportFlatPatternBuilder1.DxfRevision = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.DxfRevisionType.R20102012;

                NXOpen.Session.UndoMarkId markId2;
                markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export Flat Pattern");

                theSession.DeleteUndoMark(markId2, null);

                NXOpen.Session.UndoMarkId markId3;
                markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export Flat Pattern");

                // Potential journal callback detected. Pausing journal.
                // Journal callback ended. Unpausing
                NXOpen.NXObject nXObject1;
                nXObject1 = exportFlatPatternBuilder1.Commit();

                theSession.DeleteUndoMark(markId3, null);

                exportFlatPatternBuilder1.Destroy();
            }
            //   LogFile("DXF gerado com sucesso");
        }
        public static void saveas(string codigo, string rev)
        {


            string item = "@DB/" + codigo + "/" + rev;
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            // ----------------------------------------------
            //   Menu: File->Open...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            theSession.SetUndoMarkName(markId1, "Open Part File Dialog");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

            theSession.DeleteUndoMark(markId3, null);

            theSession.SetUndoMarkName(markId1, "Open Part File");

            theSession.DeleteUndoMark(markId1, null);

            theSession.Parts.SetNonmasterSeedPartData("@DB/" + codigo + "/" + rev);


            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;

            theSession.ApplicationSwitchImmediate("UG_APP_MODELING");

            // ----------------------------------------------
            //   Menu: File->Save As...
            // ----------------------------------------------
            NXOpen.Part part1;
            part1 = theSession.Parts.Work;

            // User Function call - UF_PART_is_family_instance

            NXOpen.Part part2;
            part2 = theSession.Parts.Work;

            NXOpen.Part part3;
            part3 = theSession.Parts.Display;

            string name1;
            name1 = workPart.Name;

            string name2;
            name2 = workPart.Name;

            //NXOpen.Features.DatumCsys datumCsys1 = ((NXOpen.Features.DatumCsys)workPart.Features.FindObject("DATUM_CSYS(0)"));
            //string featureName1;
            //featureName1 = datumCsys1.GetFeatureName();

            NXOpen.Part part4;
            part4 = theSession.Parts.Work;

            NXOpen.Part part5;
            part5 = theSession.Parts.Display;

            NXOpen.Expression expression1;
            try
            {
                // No object found with this name
                expression1 = workPart.Expressions.FindObject("NX_Material");
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(3520016);
            }

            NXOpen.Part part6;
            part6 = theSession.Parts.Work;

            NXOpen.Part part7;
            part7 = theSession.Parts.Display;

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.NXObject[] objects1 = new NXOpen.NXObject[1];
            objects1[0] = workPart;
            NXOpen.AttributePropertiesBuilder attributePropertiesBuilder1;
            attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.Revise);

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            attributePropertiesBuilder1.Units = "MilliMeter";

            NXOpen.NXObject[] objects2 = new NXOpen.NXObject[1];
            objects2[0] = workPart;
            NXOpen.MassPropertiesBuilder massPropertiesBuilder1;
            massPropertiesBuilder1 = workPart.PropertiesManager.CreateMassPropertiesBuilder(objects2);

            NXOpen.SelectNXObjectList selectNXObjectList1;
            selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

            NXOpen.NXObject[] objects3;
            objects3 = selectNXObjectList1.GetArray();

            massPropertiesBuilder1.LoadPartialComponents = true;

            massPropertiesBuilder1.Accuracy = 0.98999999999999999;

            NXOpen.NXObject[] objects4 = new NXOpen.NXObject[1];
            objects4[0] = workPart;
            NXOpen.PreviewPropertiesBuilder previewPropertiesBuilder1;
            previewPropertiesBuilder1 = workPart.PropertiesManager.CreatePreviewPropertiesBuilder(objects4);

            previewPropertiesBuilder1.StorePartPreview = true;

            previewPropertiesBuilder1.StoreModelViewPreview = true;

            previewPropertiesBuilder1.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

            NXOpen.NXObject[] objects5 = new NXOpen.NXObject[1];
            objects5[0] = workPart;
            attributePropertiesBuilder1.SetAttributeObjects(objects5);

            attributePropertiesBuilder1.Units = "MilliMeter";

            theSession.SetUndoMarkName(markId4, "Displayed Part Properties Dialog");

            NXOpen.DateBuilder dateBuilder1;
            dateBuilder1 = attributePropertiesBuilder1.DateValue;

            NXOpen.DateItemBuilder dateItemBuilder1;
            dateItemBuilder1 = dateBuilder1.DateItem;

            dateItemBuilder1.Day = NXOpen.DateItemBuilder.DayOfMonth.Day11;

            NXOpen.DateBuilder dateBuilder2;
            dateBuilder2 = attributePropertiesBuilder1.DateValue;

            NXOpen.DateItemBuilder dateItemBuilder2;
            dateItemBuilder2 = dateBuilder2.DateItem;

            dateItemBuilder2.Month = NXOpen.DateItemBuilder.MonthOfYear.Nov;

            NXOpen.DateBuilder dateBuilder3;
            dateBuilder3 = attributePropertiesBuilder1.DateValue;

            NXOpen.DateItemBuilder dateItemBuilder3;
            dateItemBuilder3 = dateBuilder3.DateItem;

            dateItemBuilder3.Year = "2024";

            NXOpen.DateBuilder dateBuilder4;
            dateBuilder4 = attributePropertiesBuilder1.DateValue;

            NXOpen.DateItemBuilder dateItemBuilder4;
            dateItemBuilder4 = dateBuilder4.DateItem;

            dateItemBuilder4.Time = "00:00:00";

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Category = "M4_it_mascarelloRevisionMaster";

            attributePropertiesBuilder1.Title = "Material_Produto";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "p";

            //NXOpen.Expression expression2;
            //expression2 = workPart.Expressions.CreateSystemExpression("String", @"""""");

            //expression2.RightHandSide = "p11";

            //NXOpen.Session.UndoMarkId markId5;
            //markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Add New Attribute from Template");

            //attributePropertiesBuilder1.IsReferenceType = false;

            //attributePropertiesBuilder1.Expression = expression2;

            attributePropertiesBuilder1.ValueAlias = "";

            //bool changed1;
            //try
            //{
            //    // This is a reserved attribute title.
            //    changed1 = attributePropertiesBuilder1.CreateAttribute();
            //}
            //catch (NXException ex)
            //{
            //    ex.AssertErrorCode(512006);
            //}

            NXOpen.Session.UndoMarkId markId6;
            markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            NXOpen.PDM.PartOperationCopyBuilder partOperationCopyBuilder1;
            partOperationCopyBuilder1 = theSession.PdmSession.CreateCopyOperationBuilder(NXOpen.PDM.PartOperationBuilder.OperationType.Revise);

            partOperationCopyBuilder1.SetOperationSubType(NXOpen.PDM.PartOperationCopyBuilder.OperationSubType.Default);

            partOperationCopyBuilder1.DefaultDestinationFolder = ":TESTE_PRD";

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

            theSession.SetUndoMarkName(markId6, "Save Parts As Dialog");

            NXOpen.NXObject[] sourceobjects4;
            sourceobjects4 = logicalobjects1[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] sourceobjects5;
            sourceobjects5 = logicalobjects1[0].GetUserAttributeSourceObjects();

            partOperationCopyBuilder1.SetDialogOperation(NXOpen.PDM.PartOperationBuilder.OperationType.Revise);

            NXOpen.BasePart[] selectedparts2 = new NXOpen.BasePart[1];
            selectedparts2[0] = workPart;
            NXOpen.BasePart[] failedparts2;
            partOperationCopyBuilder1.SetSelectedPartsToCopy(selectedparts2, out failedparts2);

            NXOpen.PDM.LogicalObject[] logicalobjects2;
            partOperationCopyBuilder1.CreateLogicalObjects(out logicalobjects2);

            NXOpen.NXObject[] sourceobjects6;
            sourceobjects6 = logicalobjects1[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] sourceobjects7;
            sourceobjects7 = logicalobjects1[0].GetUserAttributeSourceObjects();

            NXOpen.NXObject[] sourceobjects8;
            sourceobjects8 = logicalobjects1[0].GetUserAttributeSourceObjects();

            string[] attributetitles1 = new string[1];
            attributetitles1[0] = "DB_PART_REV";
            string[] titlepatterns1 = new string[1];
            titlepatterns1[0] = "NNN";
            NXOpen.NXObject nXObject1;
            nXObject1 = partOperationCopyBuilder1.CreateAttributeTitleToNamingPatternMap(attributetitles1, titlepatterns1);

            NXOpen.NXObject[] objects6 = new NXOpen.NXObject[1];
            objects6[0] = logicalobjects1[0];
            NXOpen.NXObject[] properties1 = new NXOpen.NXObject[1];
            properties1[0] = nXObject1;
            NXOpen.ErrorList errorList1;
            errorList1 = partOperationCopyBuilder1.AutoAssignAttributesWithNamingPattern(objects6, properties1);

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

            // User Function call - UF_PART_is_family_instance

            NXOpen.NXObject nXObject2;
            nXObject2 = partOperationCopyBuilder1.Commit();

            NXOpen.PDM.ErrorMessageHandler errorMessageHandler4;
            errorMessageHandler4 = partOperationCopyBuilder1.GetErrorMessageHandler(true);

            theSession.DeleteUndoMark(markId8, null);

            partOperationCopyBuilder1.Destroy();

            theSession.DeleteUndoMark(markId6, null);

            // ----------------------------------------------
            //   Menu: Tools->Automation->Journal->Stop Recording
            // ----------------------------------------------

        }

        private void button8_Click(object sender, EventArgs e)
        {

            StreamReader str;
            if (System.IO.File.Exists(@"C:\temp\gerar_demanda_cagada.txt"))
            {

                str = new StreamReader(@"C:\temp\gerar_demanda_cagada.txt");
                using (str)
                {
                    int cont = 0;
                    string linha = "";
                    while ((linha = str.ReadLine()) != null)
                    {
                        string[] quebra = linha.Split(';');
                        string[] quebra2 = quebra[0].Split('-');
                        int rev = Convert.ToInt32(quebra2[1].TrimEnd());
                       // rev++;
                        Registrar_no_LOG("Abrindo Item PDF " + quebra2[0] + "-" + rev.ToString());
                        Open_model(quebra2[0], rev.ToString().PadLeft(3, '0'));
                        GerarPDF(quebra2[0], rev.ToString().PadLeft(3, '0'));
                        Registrar_no_LOG("GeraPDF " + quebra2[0] + "-" + rev.ToString());

                        Verificar_Blank_Fam(quebra2[0], rev.ToString().PadLeft(3, '0'));

                        Close_Item_Fam();

                    }
                }

                str.Close();
            }

        }
        public static void Verificar_Blank_Fam(string codigo, string rev)
        {
            Session theSession = Session.GetSession();
            Part part1 = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;


            // partLoadStatus1.Dispose();

            string name_blank = "";
            foreach (Feature blank in part1.Features)
            {

                if (blank.GetFeatureName().Contains("Flat Pattern") && (blank.Name == "BLANK" || blank.Name == "blank"))
                {
                    //MessageBox.Show(item.GetFeatureName() + "  " + item.Name);
                    name_blank = blank.GetFeatureName().Replace(' ', '_').ToLower();
                }

            }
            name_blank = name_blank.Replace(' ', '_').ToUpper();
            // ----------------------------------------------
            //   Menu: Insert->Flat Pattern->Export Flat Pattern...
            // ----------------------------------------------

            if (name_blank != "")
            {
                Registrar_no_LOG("Gerado DXF" + codigo + "-" + rev.ToString());
                theSession.ApplicationSwitchImmediate("UG_APP_SBSM");
                string path = @"S:\Desenhos\Temp_DXF\" + codigo + "-" + rev + ".dxf";
                NXOpen.Session.UndoMarkId markId4;
                markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

                NXOpen.Features.SheetMetal.ExportFlatPatternBuilder exportFlatPatternBuilder1;
                exportFlatPatternBuilder1 = part1.Features.SheetmetalManager.CreateExportFlatPatternBuilder();
                exportFlatPatternBuilder1.ExportLocation = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.ExportLocationOptions.Native;
                exportFlatPatternBuilder1.OutputFile = path;

                exportFlatPatternBuilder1.BendUp = false;

                exportFlatPatternBuilder1.BendDown = false;

                exportFlatPatternBuilder1.InteriorCutout = true;
                exportFlatPatternBuilder1.InteriorFeature = true;

                NXOpen.Part part2 = ((NXOpen.Part)part1);
                NXOpen.Features.FlatPattern flatPattern1 = ((NXOpen.Features.FlatPattern)part2.Features.FindObject(name_blank));
                exportFlatPatternBuilder1.FlatPattern.Value = flatPattern1;

                exportFlatPatternBuilder1.FlatPattern.Value = flatPattern1;

                theSession.SetUndoMarkName(markId4, "Export Flat Pattern Dialog");

                exportFlatPatternBuilder1.DxfRevision = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.DxfRevisionType.R12;

                NXOpen.Session.UndoMarkId markId5;
                markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export Flat Pattern");

                theSession.DeleteUndoMark(markId5, null);

                NXOpen.Session.UndoMarkId markId6;
                markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export Flat Pattern");

                NXOpen.NXObject nXObject1;
                nXObject1 = exportFlatPatternBuilder1.Commit();

                theSession.DeleteUndoMark(markId6, null);

                exportFlatPatternBuilder1.Destroy();


            }

        }

        public static void Verificar_Blank(string codigo, string rev)
        {
            Session theSession = Session.GetSession();
            Part part1 = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;


            // partLoadStatus1.Dispose();

            string name_blank = "";
            foreach (Feature blank in part1.Features)
            {

                if (blank.GetFeatureName().Contains("Flat Pattern"))
                {
                    //MessageBox.Show(item.GetFeatureName() + "  " + item.Name);
                    name_blank = blank.GetFeatureName().Replace(' ', '_').ToLower();
                }

            }
            name_blank = name_blank.Replace(' ', '_').ToUpper();
            // ----------------------------------------------
            //   Menu: Insert->Flat Pattern->Export Flat Pattern...
            // ----------------------------------------------

            if (name_blank != "")
            {
                Registrar_no_LOG("Gerado DXF" + codigo + "-" + rev.ToString());
                theSession.ApplicationSwitchImmediate("UG_APP_SBSM");
                string path = @"S:\Desenhos\Temp_DXF\" + codigo + "-" + rev + ".dxf";
                NXOpen.Session.UndoMarkId markId4;
                markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

                NXOpen.Features.SheetMetal.ExportFlatPatternBuilder exportFlatPatternBuilder1;
                exportFlatPatternBuilder1 = part1.Features.SheetmetalManager.CreateExportFlatPatternBuilder();
                exportFlatPatternBuilder1.ExportLocation = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.ExportLocationOptions.Native;
                exportFlatPatternBuilder1.OutputFile = path;

                exportFlatPatternBuilder1.BendUp = false;

                exportFlatPatternBuilder1.BendDown = false;

                exportFlatPatternBuilder1.InteriorCutout = true;
                exportFlatPatternBuilder1.InteriorFeature = true;

                NXOpen.Part part2 = ((NXOpen.Part)part1);
                NXOpen.Features.FlatPattern flatPattern1 = ((NXOpen.Features.FlatPattern)part2.Features.FindObject(name_blank));
                exportFlatPatternBuilder1.FlatPattern.Value = flatPattern1;

                exportFlatPatternBuilder1.FlatPattern.Value = flatPattern1;

                theSession.SetUndoMarkName(markId4, "Export Flat Pattern Dialog");

                exportFlatPatternBuilder1.DxfRevision = NXOpen.Features.SheetMetal.ExportFlatPatternBuilder.DxfRevisionType.R12;

                NXOpen.Session.UndoMarkId markId5;
                markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export Flat Pattern");

                theSession.DeleteUndoMark(markId5, null);

                NXOpen.Session.UndoMarkId markId6;
                markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export Flat Pattern");

                NXOpen.NXObject nXObject1;
                nXObject1 = exportFlatPatternBuilder1.Commit();

                theSession.DeleteUndoMark(markId6, null);

                exportFlatPatternBuilder1.Destroy();
            }

        }
        public static void GerarPDF(string codigo, string rev)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: File->Export->PDF...
            // ----------------------------------------------
            theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            NXOpen.Annotations.PartsList[] partsList1 = workPart.Annotations.PartsLists.ToArray();

            foreach (PartsList part in partsList1)
            {

                part.Update();
            }
            //NXOpen.Annotations.PartsList partsList1 = ((NXOpen.Annotations.PartsList)workPart.Annotations.Tables.FindObject("HANDLE R-348093"));
            //partsList1.Update();

            NXOpen.PrintPDFBuilder printPDFBuilder1;
            printPDFBuilder1 = workPart.PlotManager.CreatePrintPdfbuilder();

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

            theSession.SetUndoMarkName(markId1, "Export PDF Dialog");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export PDF");

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Export PDF");

            printPDFBuilder1.Watermark = "";

            NXOpen.NXObject[] sheets1 = new NXOpen.NXObject[1];
            NXOpen.Drawings.DraftingDrawingSheet draftingDrawingSheet1 = ((NXOpen.Drawings.DraftingDrawingSheet)workPart.DraftingDrawingSheets.FindObject("Sheet 1"));
            sheets1[0] = draftingDrawingSheet1;
            printPDFBuilder1.SourceBuilder.SetSheets(sheets1);

            printPDFBuilder1.CreateNewFromUi = false;

            printPDFBuilder1.DatasetName = codigo + "_" + rev + "-PDF-1";

            printPDFBuilder1.Commit();


            theSession.DeleteUndoMark(markId3, null);

            theSession.SetUndoMarkName(markId1, "Export PDF");

            printPDFBuilder1.Destroy();

            theSession.DeleteUndoMark(markId1, null);
        }
        public static void Open_model(string codigo, string rev)
        {
            string item = "@DB/" + codigo + "/" + rev;
            Registrar_no_LOG(" Item " + item);


            NXOpen.Session theSession = NXOpen.Session.GetSession();
            // ----------------------------------------------
            //   Menu: File->Open...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            theSession.SetUndoMarkName(markId1, "Open Part File Dialog");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

            theSession.DeleteUndoMark(markId3, null);

            theSession.SetUndoMarkName(markId1, "Open Part File");

            theSession.DeleteUndoMark(markId1, null);

            theSession.Parts.SetNonmasterSeedPartData(item);

            // NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.FindObject(item));
            // Potential journal callback detected. Pausing journal.
            // Journal callback ended. Unpausing
            NXOpen.BasePart basePart1;
            NXOpen.PartLoadStatus partLoadStatus1;
            try
            {
                // File already exists
                basePart1 = theSession.Parts.OpenActiveDisplay(item, NXOpen.DisplayPartOption.AllowAdditional, out partLoadStatus1);
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(1020004);
            }

            //basePart1 = theSession.Parts.OpenActiveDisplay(item, NXOpen.DisplayPartOption.AllowAdditional, out partLoadStatus1);

        }
        public static void Open_model_save(string codigo, string rev)
        {

            string item = "@DB/" + codigo + "/" + rev;



            NXOpen.Session theSession = NXOpen.Session.GetSession();
            // ----------------------------------------------
            //   Menu: File->Open...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            theSession.SetUndoMarkName(markId1, "Open Part File Dialog");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

            theSession.DeleteUndoMark(markId3, null);

            theSession.SetUndoMarkName(markId1, "Open Part File");

            theSession.DeleteUndoMark(markId1, null);


            // NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.FindObject(item));
            // Potential journal callback detected. Pausing journal.
            // Journal callback ended. Unpausing
            NXOpen.BasePart basePart1;
            NXOpen.PartLoadStatus partLoadStatus1;
            try
            {
                // File already exists
                basePart1 = theSession.Parts.OpenActiveDisplay(item, NXOpen.DisplayPartOption.AllowAdditional, out partLoadStatus1);
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(1020004);
            }
            MP_DEMANDA();

            NXOpen.Part workPart = theSession.Parts.Work;

            NXOpen.Part displayPart = theSession.Parts.Display;



            workPart.Save(NXOpen.BasePart.SaveComponents.True, NXOpen.BasePart.CloseAfterSave.True);
            //NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.FindObject(item));
            //NXOpen.BasePart basePart2;
            //NXOpen.PartReopenReport partReopenReport1;

            //NXOpen.PartLoadStatus partLoadStatus2;
            //NXOpen.PartCollection.SdpsStatus status1;
            //status1 = theSession.Parts.SetActiveDisplay(part1, NXOpen.DisplayPartOption.AllowAdditional, NXOpen.PartDisplayPartWorkPartOption.UseLast, out partLoadStatus2);

            //// theSession.Parts.SaveAll(BasePart.CloseModified.CloseModified, null);
            //basePart2.Save(NXOpen.BasePart.SaveComponents.True, NXOpen.BasePart.CloseAfterSave.True);
            //basePart1 = theSession.Parts.OpenActiveDisplay(item, NXOpen.DisplayPartOption.AllowAdditional, out partLoadStatus1);

        }
        public static void MP_DEMANDA()
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: File->Properties
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.NXObject[] objects1 = new NXOpen.NXObject[1];
            objects1[0] = workPart;
            NXOpen.AttributePropertiesBuilder attributePropertiesBuilder1;
            attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            attributePropertiesBuilder1.Units = "MilliMeter";

            NXOpen.NXObject[] objects2 = new NXOpen.NXObject[1];
            objects2[0] = workPart;
            NXOpen.MassPropertiesBuilder massPropertiesBuilder1;
            massPropertiesBuilder1 = workPart.PropertiesManager.CreateMassPropertiesBuilder(objects2);

            NXOpen.SelectNXObjectList selectNXObjectList1;
            selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

            NXOpen.NXObject[] objects3;
            objects3 = selectNXObjectList1.GetArray();

            massPropertiesBuilder1.LoadPartialComponents = true;

            massPropertiesBuilder1.Accuracy = 0.98999999999999999;

            NXOpen.NXObject[] objects4 = new NXOpen.NXObject[1];
            objects4[0] = workPart;
            NXOpen.PreviewPropertiesBuilder previewPropertiesBuilder1;
            previewPropertiesBuilder1 = workPart.PropertiesManager.CreatePreviewPropertiesBuilder(objects4);

            previewPropertiesBuilder1.StorePartPreview = true;

            previewPropertiesBuilder1.StoreModelViewPreview = true;

            previewPropertiesBuilder1.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

            NXOpen.NXObject[] objects5 = new NXOpen.NXObject[1];
            objects5[0] = workPart;
            attributePropertiesBuilder1.SetAttributeObjects(objects5);

            attributePropertiesBuilder1.Units = "MilliMeter";

            theSession.SetUndoMarkName(markId1, "Displayed Part Properties Dialog");

            attributePropertiesBuilder1.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day16;

            attributePropertiesBuilder1.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Jan;

            attributePropertiesBuilder1.DateValue.DateItem.Year = "2025";

            attributePropertiesBuilder1.DateValue.DateItem.Time = "00:00:00";

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Category = "M4_it_mascarelloRevisionMaster";

            attributePropertiesBuilder1.Title = "Material_Produto";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsReferenceType = false;



            NXOpen.Expression expression1;
            expression1 = workPart.Expressions.CreateSystemExpression("String", @"""""");
            //attributePropertiesBuilder1.Expression = expression1;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            NXOpen.NXObject[] objects6 = new NXOpen.NXObject[1];
            objects6[0] = workPart;
            NXOpen.AttributePropertiesBuilder attributePropertiesBuilder2;
            attributePropertiesBuilder2 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects6, NXOpen.AttributePropertiesBuilder.OperationType.None);

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            attributePropertiesBuilder2.Units = "MilliMeter";

            NXOpen.NXObject[] objects7 = new NXOpen.NXObject[1];
            objects7[0] = workPart;
            attributePropertiesBuilder2.SetAttributeObjects(objects7);

            attributePropertiesBuilder2.Units = "MilliMeter";

            theSession.SetUndoMarkName(markId2, "Attributes Dialog");

            attributePropertiesBuilder2.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day16;

            attributePropertiesBuilder2.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Jan;

            attributePropertiesBuilder2.DateValue.DateItem.Year = "2025";

            attributePropertiesBuilder2.DateValue.DateItem.Time = "00:00:00";

            // ----------------------------------------------
            //   Dialog Begin Attributes
            // ----------------------------------------------
            attributePropertiesBuilder2.Title = "";

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.StringValue = "";

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.StringValue = "";

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.Category = "Materials";

            attributePropertiesBuilder2.Title = "NX_Material";

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.StringValue = "097439 - CH_AL_LISO_3105_H16_NBR7556_3000x1300x2,00mm";

            attributePropertiesBuilder2.IsArray = false;

            attributePropertiesBuilder2.IsArray = false;

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Attributes");

            theSession.DeleteUndoMark(markId3, null);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Attributes");

            theSession.DeleteUndoMark(markId4, null);

            theSession.SetUndoMarkName(markId2, "Attributes");

            attributePropertiesBuilder2.Destroy();

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Expression expression2;
            expression2 = workPart.Expressions.GetAttributeExpression(workPart, "NX_Material", NXOpen.NXObject.AttributeType.String, -1);

            expression1.RightHandSide = "NX_Material";

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Accept Edit");

            expression1.RightHandSide = "NX_Material";

            attributePropertiesBuilder1.StringValue = "097439 - CH_AL_LISO_3105_H16_NBR7556_3000x1300x2,00mm";

            attributePropertiesBuilder1.ValueAlias = "";

            bool changed1;
            changed1 = attributePropertiesBuilder1.CreateAttribute();

            attributePropertiesBuilder1.Category = "";

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsReferenceType = false;

            NXOpen.Expression nullNXOpen_Expression = null;
            attributePropertiesBuilder1.Expression = nullNXOpen_Expression;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Category = "M4_it_mascarelloRevisionMaster";

            attributePropertiesBuilder1.Title = "Material_Produto";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "097439 - CH_AL_LISO_3105_H16_NBR7556_3000x1300x2,00mm";

            attributePropertiesBuilder1.IsReferenceType = false;

            attributePropertiesBuilder1.Expression = expression1;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            NXOpen.Session.UndoMarkId markId6;
            markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            theSession.DeleteUndoMark(markId6, null);

            NXOpen.Session.UndoMarkId markId7;
            markId7 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            NXOpen.Session.UndoMarkId markId8;
            markId8 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Attribute Properties");

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId8);

            NXOpen.MassPropertiesBuilder.UpdateOptions updateoption1;
            updateoption1 = massPropertiesBuilder1.UpdateOnSave;

            NXOpen.NXObject nXObject1;
            nXObject1 = massPropertiesBuilder1.Commit();

            workPart.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

            NXOpen.NXObject nXObject2;
            nXObject2 = previewPropertiesBuilder1.Commit();

            NXOpen.Session.UndoMarkId id1;
            id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(id1);

            theSession.DeleteUndoMark(markId7, null);

            theSession.SetUndoMarkName(markId1, "Displayed Part Properties");

            attributePropertiesBuilder1.Destroy();

            massPropertiesBuilder1.Destroy();

            previewPropertiesBuilder1.Destroy();

            theSession.DeleteUndoMark(id1, null);

            theSession.DeleteUndoMark(markId5, null);

            theSession.CleanUpFacetedFacesAndEdges();



            // ----------------------------------------------
            //   Menu: Tools->Automation->Journal->Stop Recording
            // ----------------------------------------------

        }
        public static void Save_Update()
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: File->Save
            // ----------------------------------------------
            NXOpen.Part part1;
            part1 = theSession.Parts.Work;

            // User Function call - UF_PART_is_family_instance

            NXOpen.Part part2;
            part2 = theSession.Parts.Work;

            NXOpen.Part part3;
            part3 = theSession.Parts.Display;

            string name1;
            name1 = workPart.Name;

            string name2;
            name2 = workPart.Name;

            NXOpen.Part part4;
            part4 = theSession.Parts.Work;

            NXOpen.Part part5;
            part5 = theSession.Parts.Display;

            string applicationname1;
            applicationname1 = theSession.ApplicationName;

            NXOpen.Part part6;
            part6 = theSession.Parts.Work;

            NXOpen.Part part7;
            part7 = theSession.Parts.Display;

            NXOpen.Expression expression1;
            try
            {
                // No object found with this name
                expression1 = workPart.Expressions.FindObject("NX_Material");
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(3520016);
            }

            NXOpen.Part part8;
            part8 = theSession.Parts.Work;

            NXOpen.Part part9;
            part9 = theSession.Parts.Display;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.NXObject[] objects1 = new NXOpen.NXObject[1];
            objects1[0] = workPart;
            NXOpen.AttributePropertiesBuilder attributePropertiesBuilder1;
            attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            attributePropertiesBuilder1.Units = "MilliMeter";

            NXOpen.NXObject[] objects2 = new NXOpen.NXObject[1];
            objects2[0] = workPart;
            NXOpen.MassPropertiesBuilder massPropertiesBuilder1;
            massPropertiesBuilder1 = workPart.PropertiesManager.CreateMassPropertiesBuilder(objects2);

            NXOpen.SelectNXObjectList selectNXObjectList1;
            selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

            NXOpen.NXObject[] objects3;
            objects3 = selectNXObjectList1.GetArray();

            massPropertiesBuilder1.LoadPartialComponents = true;

            massPropertiesBuilder1.Accuracy = 0.98999999999999999;

            NXOpen.NXObject[] objects4 = new NXOpen.NXObject[1];
            objects4[0] = workPart;
            NXOpen.PreviewPropertiesBuilder previewPropertiesBuilder1;
            previewPropertiesBuilder1 = workPart.PropertiesManager.CreatePreviewPropertiesBuilder(objects4);

            previewPropertiesBuilder1.StorePartPreview = true;

            previewPropertiesBuilder1.StoreModelViewPreview = true;

            previewPropertiesBuilder1.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

            NXOpen.NXObject[] objects5 = new NXOpen.NXObject[1];
            objects5[0] = workPart;
            attributePropertiesBuilder1.SetAttributeObjects(objects5);

            attributePropertiesBuilder1.Units = "MilliMeter";

            theSession.SetUndoMarkName(markId1, "Displayed Part Properties Dialog");

            NXOpen.DateBuilder dateBuilder1;
            dateBuilder1 = attributePropertiesBuilder1.DateValue;

            NXOpen.DateItemBuilder dateItemBuilder1;
            dateItemBuilder1 = dateBuilder1.DateItem;

            dateItemBuilder1.Day = NXOpen.DateItemBuilder.DayOfMonth.Day11;

            NXOpen.DateBuilder dateBuilder2;
            dateBuilder2 = attributePropertiesBuilder1.DateValue;

            NXOpen.DateItemBuilder dateItemBuilder2;
            dateItemBuilder2 = dateBuilder2.DateItem;

            dateItemBuilder2.Month = NXOpen.DateItemBuilder.MonthOfYear.Nov;

            NXOpen.DateBuilder dateBuilder3;
            dateBuilder3 = attributePropertiesBuilder1.DateValue;

            NXOpen.DateItemBuilder dateItemBuilder3;
            dateItemBuilder3 = dateBuilder3.DateItem;

            dateItemBuilder3.Year = "2024";

            NXOpen.DateBuilder dateBuilder4;
            dateBuilder4 = attributePropertiesBuilder1.DateValue;

            NXOpen.DateItemBuilder dateItemBuilder4;
            dateItemBuilder4 = dateBuilder4.DateItem;

            dateItemBuilder4.Time = "00:00:00";

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Category = "M4_it_mascarelloRevisionMaster";

            attributePropertiesBuilder1.Title = "Material_Produto";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "p";

            NXOpen.Expression expression2;
            expression2 = workPart.Expressions.CreateSystemExpression("String", @"""""");

            try
            {
                expression2.RightHandSide = "p11";
            }
            catch
            {
                try
                {
                    expression2.RightHandSide = "NX_Material";
                }
                catch
                {



                }
            }


            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Add New Attribute from Template");

            attributePropertiesBuilder1.IsReferenceType = false;

            attributePropertiesBuilder1.Expression = expression2;

            attributePropertiesBuilder1.ValueAlias = "";

            try
            {
                bool changed1;
                changed1 = attributePropertiesBuilder1.CreateAttribute();
            }
            catch
            {


            }


            attributePropertiesBuilder1.Category = "";

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsReferenceType = false;

            NXOpen.Expression nullNXOpen_Expression = null;
            attributePropertiesBuilder1.Expression = nullNXOpen_Expression;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Category = "M4_it_mascarelloRevisionMaster";

            attributePropertiesBuilder1.Title = "Material_Produto";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsReferenceType = false;

            attributePropertiesBuilder1.Expression = expression2;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            theSession.DeleteUndoMark(markId3, null);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Attribute Properties");

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId5);

            NXOpen.MassPropertiesBuilder.UpdateOptions updateoption1;
            updateoption1 = massPropertiesBuilder1.UpdateOnSave;

            NXOpen.NXObject nXObject1;
            nXObject1 = massPropertiesBuilder1.Commit();

            workPart.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

            // Potential journal callback detected. Pausing journal.
            // Journal callback ended. Unpausing
            NXOpen.NXObject nXObject2;
            nXObject2 = previewPropertiesBuilder1.Commit();

            NXOpen.Session.UndoMarkId id1;
            id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(id1);

            theSession.DeleteUndoMark(markId4, null);

            theSession.SetUndoMarkName(markId1, "Displayed Part Properties");

            attributePropertiesBuilder1.Destroy();

            massPropertiesBuilder1.Destroy();

            previewPropertiesBuilder1.Destroy();

            theSession.DeleteUndoMark(id1, null);

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId6;
            markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Save");

            NXOpen.PDM.SmartSaveContext smartSaveContext1;
            smartSaveContext1 = theSession.PdmSession.CreateSmartSaveContext(NXOpen.PDM.SmartSaveBuilder.SaveType.Save);

            NXOpen.PDM.SmartSaveBuilder smartSaveBuilder1;
            smartSaveBuilder1 = theSession.PdmSession.CreateSmartSaveBuilderWithContext(smartSaveContext1);

            NXOpen.PDM.SmartSaveObject[] smartsaveobjects1;
            smartSaveBuilder1.GetSmartSaveObjects(out smartsaveobjects1);

            NXOpen.NXObject[] objects6 = new NXOpen.NXObject[1];
            objects6[0] = smartsaveobjects1[0];
            NXOpen.ErrorList errorList1;
            errorList1 = smartSaveBuilder1.AutoAssignAttributes(objects6);

            smartSaveBuilder1.ValidateSmartSaveObjects();

            NXOpen.PDM.ErrorMessageHandler errorMessageHandler1;
            errorMessageHandler1 = smartSaveBuilder1.GetErrorMessageHandler(true);

            NXOpen.NXObject nXObject3;
            nXObject3 = smartSaveBuilder1.Commit();

            NXOpen.PDM.ErrorMessageHandler errorMessageHandler2;
            errorMessageHandler2 = smartSaveBuilder1.GetErrorMessageHandler(true);

            smartSaveBuilder1.Destroy();

            smartSaveContext1.Dispose();
            // ----------------------------------------------
            //   Menu: Tools->Automation->Journal->Stop Recording
            // ----------------------------------------------

        }

        private void button9_Click(object sender, EventArgs e)
        {
            StreamReader str;
            if (System.IO.File.Exists(@"C:\temp\itens_demanda.txt"))
            {

                str = new StreamReader(@"C:\temp\itens_demanda.txt");
                using (str)
                {
                    int cont = 0;
                    string linha = "";
                    while ((linha = str.ReadLine()) != null)
                    {
                        string[] quebra = linha.Split(';');
                        string[] quebra2 = quebra[0].Split('-');
                        //string peso_bruto = quebra[1];
                        //string peso_liquido = quebra[2];
                        Registrar_no_LOG("Abrindo item  " + quebra2[0] + "-" + quebra2[1]);
                        Open_model(quebra2[0], quebra2[1]);
                        remove_drawing_model();

                        int rev = Convert.ToInt32(quebra2[1].TrimEnd());
                        rev++;
                        Registrar_no_LOG("Criando Revisao Item " + quebra2[0] + "-" + rev.ToString());
                        saveas(quebra2[0], quebra2[1].PadLeft(3, '0'));
                        Registrar_no_LOG("Revisao Item Criada com sucesso " + quebra2[0] + "-" + rev.ToString());

                        NXOpen.Session theSession = NXOpen.Session.GetSession();
                        NXOpen.Part workPart = theSession.Parts.Work;
                        NXOpen.Part displayPart = theSession.Parts.Display;
                        alterar_mp(quebra[1]);

                        //preenche_expression("QTD_BRUTO", peso_bruto.Replace(',', '.'), workPart);
                        //preenche_expression("QTD", peso_liquido.Replace(',', '.'), workPart);

                        //preenche_expression("Fator_Perda", "IF(QTD != 0 && QTD_BRUTO != 0)((1 - (QTD / QTD_BRUTO)) * 100)ELSE(0)", workPart);
                        //IF(QTD != 0 && QTD_BRUTO != 0)((1 - (QTD / QTD_BRUTO)) * 100)ELSE(0)
                        //string mp_new = "097439 - CH_AL_LISO_3105_H16_NBR7556_3000x1300x2,00mm";
                        //alterar_mp(mp_new);
                        Registrar_no_LOG("ALterado peso liquido e peso bruto");

                        Registrar_no_LOG("Verificar blank ");

                        Verificar_Blank(quebra2[0], rev.ToString().PadLeft(3, '0'));
                        Registrar_no_LOG("Blank ok");

                        Save_Update();


                        Registrar_no_LOG("Salvando Model " + quebra2[0] + "-" + rev.ToString());
                        Registrar_no_LOG("Abrindo drawing " + quebra2[0] + "-" + rev.ToString());
                        try
                        {


                            Open_drawing_demanda(quebra2[0], rev.ToString().PadLeft(3, '0'));
                            deletar_alteracao();
                            Insertb_alt(workPart);

                            preenche_alteracao(quebra[2]);

                            Registrar_no_LOG("Fechar arquivo " + quebra2[0] + "-" + rev.ToString());
                        }
                        catch
                        {
                            Registrar_no_LOG("Sem Drawing " + quebra2[0] + "-" + rev.ToString());

                            //NXOpen.Session theSession = NXOpen.Session.GetSession();
                            //NXOpen.Part workPart = theSession.Parts.Work;
                            //NXOpen.Part displayPart = theSession.Parts.Display;

                            theSession.Parts.CloseAll(BasePart.CloseModified.CloseModified, null);
                        }
                        // Open_model_save(quebra2[0], rev.ToString().PadLeft(3, '0'));

                        Save_Update();

                        theSession.Parts.CloseAll(BasePart.CloseModified.CloseModified, null);

                        //conexao_bd conexao = new conexao_bd();
                        //conexao.insert_wf_auto(quebra2[0].Substring(0, 6), rev.ToString().PadLeft(3, '0'));
                    }
                }
                str.Close();
            }
        }

        public void preenche_alteracao(string desc_alt)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: File->Properties
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.NXObject[] objects1 = new NXOpen.NXObject[1];
            objects1[0] = workPart;
            NXOpen.AttributePropertiesBuilder attributePropertiesBuilder1;
            attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            attributePropertiesBuilder1.Units = "MilliMeter";

            NXOpen.NXObject[] objects2 = new NXOpen.NXObject[1];
            objects2[0] = workPart;
            NXOpen.MassPropertiesBuilder massPropertiesBuilder1;
            massPropertiesBuilder1 = workPart.PropertiesManager.CreateMassPropertiesBuilder(objects2);

            NXOpen.SelectNXObjectList selectNXObjectList1;
            selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

            NXOpen.NXObject[] objects3;
            objects3 = selectNXObjectList1.GetArray();

            massPropertiesBuilder1.LoadPartialComponents = true;

            massPropertiesBuilder1.Accuracy = 0.98999999999999999;

            NXOpen.NXObject[] objects4 = new NXOpen.NXObject[1];
            objects4[0] = workPart;
            NXOpen.PreviewPropertiesBuilder previewPropertiesBuilder1;
            previewPropertiesBuilder1 = workPart.PropertiesManager.CreatePreviewPropertiesBuilder(objects4);

            previewPropertiesBuilder1.StorePartPreview = true;

            previewPropertiesBuilder1.StoreModelViewPreview = true;

            previewPropertiesBuilder1.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

            NXOpen.NXObject[] objects5 = new NXOpen.NXObject[1];
            objects5[0] = workPart;
            attributePropertiesBuilder1.SetAttributeObjects(objects5);

            attributePropertiesBuilder1.Units = "MilliMeter";

            theSession.SetUndoMarkName(markId1, "Displayed Part Properties Dialog");

            attributePropertiesBuilder1.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day10;

            attributePropertiesBuilder1.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Jan;

            attributePropertiesBuilder1.DateValue.DateItem.Year = "2025";

            attributePropertiesBuilder1.DateValue.DateItem.Time = "00:00:00";

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Title = "ALTERACAO";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            attributePropertiesBuilder1.StringValue = desc_alt;

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            NXOpen.NXObject nXObject1;
            nXObject1 = attributePropertiesBuilder1.Commit();

            NXOpen.MassPropertiesBuilder.UpdateOptions updateoption1;
            updateoption1 = massPropertiesBuilder1.UpdateOnSave;

            NXOpen.NXObject nXObject2;
            nXObject2 = massPropertiesBuilder1.Commit();

            workPart.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

            NXOpen.NXObject nXObject3;
            nXObject3 = previewPropertiesBuilder1.Commit();

            NXOpen.Session.UndoMarkId id1;
            id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(id1);

            theSession.DeleteUndoMark(markId3, null);

            theSession.SetUndoMarkName(id1, "Displayed Part Properties");

            attributePropertiesBuilder1.Destroy();

            massPropertiesBuilder1.Destroy();

            previewPropertiesBuilder1.Destroy();


        }
        public static void Insertb_alt(Part part)//(string codigo, string rev)
        {
            double pos_x = 0.0;
            double pos_y = 0.0;
            string format = verificar_formato();
            if (format == "A3")
            {
                pos_x = 6.024486091;
                pos_y = 290.987036704;
            }
            if (format == "A4")
            {
                pos_x = 6.089779844;
                pos_y = 275.975126950;
            }
            if (format == "A4")
            {

                string novo = verificar_formato_novo();

                if (novo == "")
                {
                    pos_y = 290.987036704;
                }
            }

            NXOpen.UF.UFSession ufSession = NXOpen.UF.UFSession.GetUFSession();
            double[] origem = { pos_x, pos_y, 0 };
            ufSession.Tabnot.CreateFromTemplate("@DB/Revisao-metric/A", origem, out NXOpen.Tag tabular_note);

        }
        private void deletar_alteracao()
        {

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            NXOpen.TaggedObject[] objects1 = new NXOpen.TaggedObject[1];

            foreach (TableSection obj in workPart.Annotations.TableSections)
            {

                _nxTableSection = obj;

                UFSession.GetUFSession().Tabnot.AskTabularNoteOfSection(_nxTableSection.Tag, out Tag nxTableTag);

                _nxTable = Session.GetSession().GetObjectManager().GetTaggedObject(nxTableTag) as NXOpen.Annotations.Table;
                int test = GetNumRows();

                string text = GetSetText(1, 1);

                if (text == "Modificações")
                {
                    objects1[0] = obj;
                }


            }

            if (objects1.Length > 0)
            {
                int nErrs1;
                nErrs1 = theSession.UpdateManager.AddObjectsToDeleteList(objects1);

                NXOpen.Session.UndoMarkId id1;
                id1 = theSession.NewestVisibleUndoMark;

                int nErrs2;
                nErrs2 = theSession.UpdateManager.DoUpdate(id1);
            }

        }
        private TableSection _nxTableSection;
        private static NXOpen.Annotations.Table _nxTable;
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
        public static void Open_drawing_demanda(string codigo, string rev)
        {
            bool dwg_ok = false;
            try
            {
                dwg_ok = true;
                string item = "@DB/" + codigo + "/" + rev + "/specification/" + codigo + "-" + rev + "-dwg1";

                // ----------------------------------------------
                //   Menu: Tools->Automation->Journal->Stop Recording
                // ----------------------------------------------

                NXOpen.Session theSession = NXOpen.Session.GetSession();
                // ----------------------------------------------
                //   Menu: File->Open...
                // ----------------------------------------------
                NXOpen.Session.UndoMarkId markId1;
                markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

                theSession.SetUndoMarkName(markId1, "Open Part File Dialog");

                theSession.UndoToMark(markId1, null);

                theSession.DeleteUndoMark(markId1, null);

                theSession.DeleteUndoMark(markId1, null);

                // ----------------------------------------------
                //   Menu: File->Open...
                // ----------------------------------------------
                NXOpen.Session.UndoMarkId markId2;
                markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

                theSession.SetUndoMarkName(markId2, "Open Part File Dialog");

                NXOpen.Session.UndoMarkId markId3;
                markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

                theSession.DeleteUndoMark(markId3, null);

                NXOpen.Session.UndoMarkId markId4;
                markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

                theSession.DeleteUndoMark(markId4, null);

                theSession.SetUndoMarkName(markId2, "Open Part File");

                theSession.DeleteUndoMark(markId2, null);

                NXOpen.BasePart basePart1;
                NXOpen.PartLoadStatus partLoadStatus1;
                try
                {
                    // File already exists
                    basePart1 = theSession.Parts.OpenActiveDisplay(item, NXOpen.DisplayPartOption.AllowAdditional, out partLoadStatus1);
                }
                catch (NXException ex)
                {
                    ex.AssertErrorCode(1020004);
                }

                NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.FindObject(item));
                NXOpen.BasePart basePart2;
                NXOpen.PartReopenReport partReopenReport1;
                basePart2 = part1.Reopen(NXOpen.BasePart.CloseWholeTree.False, NXOpen.BasePart.CloseModified.CloseModified, null, out partReopenReport1);

                partReopenReport1.Dispose();
                NXOpen.Session.UndoMarkId markId5;
                markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Change Displayed Part");

                NXOpen.Part part2 = ((NXOpen.Part)basePart2);
                NXOpen.PartLoadStatus partLoadStatus2;
                NXOpen.PartCollection.SdpsStatus status1;
                status1 = theSession.Parts.SetActiveDisplay(part2, NXOpen.DisplayPartOption.AllowAdditional, NXOpen.PartDisplayPartWorkPartOption.UseLast, out partLoadStatus2);

                NXOpen.Part workPart = theSession.Parts.Work;
                NXOpen.Part displayPart = theSession.Parts.Display;
                partLoadStatus2.Dispose();
                theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");

                workPart.Drafting.EnterDraftingApplication();

                workPart.Views.WorkView.UpdateCustomSymbols();

                workPart.Drafting.SetTemplateInstantiationIsComplete(true);


                var shetts = workPart.DraftingDrawingSheets.ToArray();

                foreach (NXOpen.Drawings.DraftingDrawingSheet item1 in shetts)
                {
                    if (item1.Name == "BLANK")
                    {
                        NXOpen.TaggedObject[] objects1 = new NXOpen.TaggedObject[1];
                        NXOpen.View view1 = ((NXOpen.View)workPart.Views.FindObject("BLANK@0"));
                        objects1[0] = view1;
                        int nErrs1;
                        nErrs1 = theSession.UpdateManager.AddObjectsToDeleteList(objects1);

                    }

                }

            }
            catch
            {

                dwg_ok = false;
            }
            if (dwg_ok == false)
            {
                try
                {
                    dwg_ok = true;
                    string item = "@DB/" + codigo + "/" + rev + "/specification/" + codigo + "-" + rev + "-dwg2";

                    // ----------------------------------------------
                    //   Menu: Tools->Automation->Journal->Stop Recording
                    // ----------------------------------------------

                    NXOpen.Session theSession = NXOpen.Session.GetSession();
                    // ----------------------------------------------
                    //   Menu: File->Open...
                    // ----------------------------------------------
                    NXOpen.Session.UndoMarkId markId1;
                    markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

                    theSession.SetUndoMarkName(markId1, "Open Part File Dialog");

                    theSession.UndoToMark(markId1, null);

                    theSession.DeleteUndoMark(markId1, null);

                    theSession.DeleteUndoMark(markId1, null);

                    // ----------------------------------------------
                    //   Menu: File->Open...
                    // ----------------------------------------------
                    NXOpen.Session.UndoMarkId markId2;
                    markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

                    theSession.SetUndoMarkName(markId2, "Open Part File Dialog");

                    NXOpen.Session.UndoMarkId markId3;
                    markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

                    theSession.DeleteUndoMark(markId3, null);

                    NXOpen.Session.UndoMarkId markId4;
                    markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Open Part File");

                    theSession.DeleteUndoMark(markId4, null);

                    theSession.SetUndoMarkName(markId2, "Open Part File");

                    theSession.DeleteUndoMark(markId2, null);

                    NXOpen.BasePart basePart1;
                    NXOpen.PartLoadStatus partLoadStatus1;
                    try
                    {
                        // File already exists
                        basePart1 = theSession.Parts.OpenActiveDisplay(item, NXOpen.DisplayPartOption.AllowAdditional, out partLoadStatus1);
                    }
                    catch (NXException ex)
                    {
                        ex.AssertErrorCode(1020004);
                    }

                    NXOpen.Part part1 = ((NXOpen.Part)theSession.Parts.FindObject(item));
                    NXOpen.BasePart basePart2;
                    NXOpen.PartReopenReport partReopenReport1;
                    basePart2 = part1.Reopen(NXOpen.BasePart.CloseWholeTree.False, NXOpen.BasePart.CloseModified.CloseModified, null, out partReopenReport1);

                    partReopenReport1.Dispose();
                    NXOpen.Session.UndoMarkId markId5;
                    markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Change Displayed Part");

                    NXOpen.Part part2 = ((NXOpen.Part)basePart2);
                    NXOpen.PartLoadStatus partLoadStatus2;
                    NXOpen.PartCollection.SdpsStatus status1;
                    status1 = theSession.Parts.SetActiveDisplay(part2, NXOpen.DisplayPartOption.AllowAdditional, NXOpen.PartDisplayPartWorkPartOption.UseLast, out partLoadStatus2);

                    NXOpen.Part workPart = theSession.Parts.Work;
                    NXOpen.Part displayPart = theSession.Parts.Display;
                    partLoadStatus2.Dispose();
                    theSession.ApplicationSwitchImmediate("UG_APP_DRAFTING");

                    workPart.Drafting.EnterDraftingApplication();

                    workPart.Views.WorkView.UpdateCustomSymbols();

                    workPart.Drafting.SetTemplateInstantiationIsComplete(true);


                    var shetts = workPart.DraftingDrawingSheets.ToArray();

                    foreach (NXOpen.Drawings.DraftingDrawingSheet item1 in shetts)
                    {
                        if (item1.Name == "BLANK")
                        {
                            NXOpen.TaggedObject[] objects1 = new NXOpen.TaggedObject[1];
                            NXOpen.View view1 = ((NXOpen.View)workPart.Views.FindObject("BLANK@0"));
                            objects1[0] = view1;
                            int nErrs1;
                            nErrs1 = theSession.UpdateManager.AddObjectsToDeleteList(objects1);

                        }

                    }

                }
                catch
                {

                    dwg_ok = false;
                }
            }



        }
        public static void remove_drawing_model()
        {



            NXOpen.Session theSession = NXOpen.Session.GetSession();


            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;

            workPart.Drafting.EnterDraftingApplication();

            workPart.Views.WorkView.UpdateCustomSymbols();

            workPart.Drafting.SetTemplateInstantiationIsComplete(true);


            var shetts = workPart.DraftingDrawingSheets.ToArray();

            foreach (NXOpen.Drawings.DraftingDrawingSheet item1 in shetts)
            {

                NXOpen.TaggedObject[] objects1 = new NXOpen.TaggedObject[1];
                objects1[0] = item1;
                int nErrs1;
                nErrs1 = theSession.UpdateManager.AddObjectsToDeleteList(objects1);
            }



        }
        public static void alterar_mp_atributo()
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: File->Properties
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.NXObject[] objects1 = new NXOpen.NXObject[1];
            objects1[0] = workPart;
            NXOpen.AttributePropertiesBuilder attributePropertiesBuilder1;
            attributePropertiesBuilder1 = theSession.AttributeManager.CreateAttributePropertiesBuilder(workPart, objects1, NXOpen.AttributePropertiesBuilder.OperationType.None);

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.DataType = NXOpen.AttributePropertiesBaseBuilder.DataTypeOptions.String;

            attributePropertiesBuilder1.Units = "MilliMeter";

            NXOpen.NXObject[] objects2 = new NXOpen.NXObject[1];
            objects2[0] = workPart;
            NXOpen.MassPropertiesBuilder massPropertiesBuilder1;
            massPropertiesBuilder1 = workPart.PropertiesManager.CreateMassPropertiesBuilder(objects2);

            NXOpen.SelectNXObjectList selectNXObjectList1;
            selectNXObjectList1 = massPropertiesBuilder1.SelectedObjects;

            NXOpen.NXObject[] objects3;
            objects3 = selectNXObjectList1.GetArray();

            massPropertiesBuilder1.LoadPartialComponents = true;

            massPropertiesBuilder1.Accuracy = 0.98999999999999999;

            NXOpen.NXObject[] objects4 = new NXOpen.NXObject[1];
            objects4[0] = workPart;
            NXOpen.PreviewPropertiesBuilder previewPropertiesBuilder1;
            previewPropertiesBuilder1 = workPart.PropertiesManager.CreatePreviewPropertiesBuilder(objects4);

            previewPropertiesBuilder1.StorePartPreview = true;

            previewPropertiesBuilder1.StoreModelViewPreview = true;

            previewPropertiesBuilder1.ModelViewCreation = NXOpen.PreviewPropertiesBuilder.ModelViewCreationOptions.OnViewSave;

            NXOpen.NXObject[] objects5 = new NXOpen.NXObject[1];
            objects5[0] = workPart;
            attributePropertiesBuilder1.SetAttributeObjects(objects5);

            attributePropertiesBuilder1.Units = "MilliMeter";

            theSession.SetUndoMarkName(markId1, "Displayed Part Properties Dialog");

            attributePropertiesBuilder1.DateValue.DateItem.Day = NXOpen.DateItemBuilder.DayOfMonth.Day13;

            attributePropertiesBuilder1.DateValue.DateItem.Month = NXOpen.DateItemBuilder.MonthOfYear.Jan;

            attributePropertiesBuilder1.DateValue.DateItem.Year = "2025";

            attributePropertiesBuilder1.DateValue.DateItem.Time = "00:00:00";

            attributePropertiesBuilder1.Title = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = "";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.Category = "M4_it_mascarelloRevisionMaster";

            attributePropertiesBuilder1.Title = "Material_Produto";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.StringValue = @"""""";

            attributePropertiesBuilder1.IsArray = false;

            attributePropertiesBuilder1.IsArray = false;

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            attributePropertiesBuilder1.StringValue = "097439 - CH_AL_LISO_3105_H16_NBR7556_3000x1300x2,00mm";

            theSession.DeleteUndoMark(markId2, null);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Displayed Part Properties");

            NXOpen.NXObject nXObject1;
            nXObject1 = attributePropertiesBuilder1.Commit();

            NXOpen.MassPropertiesBuilder.UpdateOptions updateoption1;
            updateoption1 = massPropertiesBuilder1.UpdateOnSave;

            NXOpen.NXObject nXObject2;
            nXObject2 = massPropertiesBuilder1.Commit();

            workPart.PartPreviewMode = NXOpen.BasePart.PartPreview.OnSave;

            NXOpen.NXObject nXObject3;
            nXObject3 = previewPropertiesBuilder1.Commit();

            NXOpen.Session.UndoMarkId id1;
            id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.Visible);

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(id1);

            theSession.DeleteUndoMark(markId3, null);

            theSession.SetUndoMarkName(id1, "Displayed Part Properties");

            attributePropertiesBuilder1.Destroy();

            massPropertiesBuilder1.Destroy();

            previewPropertiesBuilder1.Destroy();

            theSession.CleanUpFacetedFacesAndEdges();

            // ----------------------------------------------
            //   Menu: Tools->Automation->Journal->Stop Recording
            // ----------------------------------------------

        }
        public void alterar_mp(string mp)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: Tools->Materials->Assign Materials...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.PhysMat.PhysicalMaterialListBuilder physicalMaterialListBuilder1;
            physicalMaterialListBuilder1 = workPart.MaterialManager.PhysicalMaterials.CreateListBlockBuilder();

            NXOpen.PhysMat.PhysicalMaterialAssignBuilder physicalMaterialAssignBuilder1;
            physicalMaterialAssignBuilder1 = workPart.MaterialManager.PhysicalMaterials.CreateMaterialAssignBuilder();

            theSession.SetUndoMarkName(markId1, "Assign Material Dialog");

            NXOpen.Session.UndoMarkId id1;
            id1 = theSession.GetNewestUndoMark(NXOpen.Session.MarkVisibility.AnyVisibility);

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Inspect");

            NXOpen.PhysicalMaterial physicalMaterial1;
            try
            {
                physicalMaterial1 = workPart.MaterialManager.PhysicalMaterials.LoadFromMatmlLibrary("X:\\Materials\\BibliotecaMascarello.xml", mp);
            }
            catch
            {

                physicalMaterial1 = ((NXOpen.PhysicalMaterial)workPart.MaterialManager.PhysicalMaterials.FindObject($"PhysicalMaterial[{mp}]"));
            }


            NXOpen.PhysicalMaterialBuilder physicalMaterialBuilder1;
            physicalMaterialBuilder1 = workPart.MaterialManager.PhysicalMaterials.CreatePhysicalMaterialInspectBuilder(physicalMaterial1);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            NXOpen.CAE.PropertyTable propertyTable1;
            propertyTable1 = physicalMaterialBuilder1.ItemPropertyTable;

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper1;
            scalarFieldWrapper1 = propertyTable1.GetScalarFieldWrapperByIndex(0, 0, "IsotropicSaturatedSeries");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper2;
            scalarFieldWrapper2 = propertyTable1.GetScalarFieldWrapperByIndex(0, 1, "IsotropicSaturatedSeries");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper3;
            scalarFieldWrapper3 = propertyTable1.GetScalarFieldWrapperByIndex(0, 0, "ChabocheKinematicHardening");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper4;
            scalarFieldWrapper4 = propertyTable1.GetScalarFieldWrapperByIndex(0, 1, "ChabocheKinematicHardening");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper5;
            scalarFieldWrapper5 = propertyTable1.GetScalarFieldWrapperByIndex(0, 0, "ChabocheNonlinearKinematicHardening");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper6;
            scalarFieldWrapper6 = propertyTable1.GetScalarFieldWrapperByIndex(0, 1, "ChabocheNonlinearKinematicHardening");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper7;
            scalarFieldWrapper7 = propertyTable1.GetScalarFieldWrapperByIndex(0, 2, "ChabocheNonlinearKinematicHardening");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper8;
            scalarFieldWrapper8 = propertyTable1.GetScalarFieldWrapperByIndex(0, 3, "ChabocheNonlinearKinematicHardening");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper9;
            scalarFieldWrapper9 = propertyTable1.GetScalarFieldWrapperByIndex(0, 0, "LatentHeatTable");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper10;
            scalarFieldWrapper10 = propertyTable1.GetScalarFieldWrapperByIndex(0, 1, "LatentHeatTable");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper11;
            scalarFieldWrapper11 = propertyTable1.GetScalarFieldWrapperByIndex(0, 2, "LatentHeatTable");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper12;
            scalarFieldWrapper12 = propertyTable1.GetScalarFieldWrapperByIndex(0, 3, "LatentHeatTable");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper13;
            scalarFieldWrapper13 = propertyTable1.GetScalarFieldWrapperByIndex(0, 0, "electricPoles");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper14;
            scalarFieldWrapper14 = propertyTable1.GetScalarFieldWrapperByIndex(0, 1, "electricPoles");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper15;
            scalarFieldWrapper15 = propertyTable1.GetScalarFieldWrapperByIndex(0, 2, "electricPoles");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper16;
            scalarFieldWrapper16 = propertyTable1.GetScalarFieldWrapperByIndex(0, 0, "NonlinearIsotropicPermeability");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper17;
            scalarFieldWrapper17 = propertyTable1.GetScalarFieldWrapperByIndex(0, 1, "NonlinearIsotropicPermeability");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper18;
            scalarFieldWrapper18 = propertyTable1.GetScalarFieldWrapperByIndex(0, 0, "NonlinearIsotropicPermanentMagnetPermeability");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper19;
            scalarFieldWrapper19 = propertyTable1.GetScalarFieldWrapperByIndex(0, 1, "NonlinearIsotropicPermanentMagnetPermeability");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper20;
            scalarFieldWrapper20 = propertyTable1.GetScalarFieldWrapperByIndex(0, 0, "IronLoss");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper21;
            scalarFieldWrapper21 = propertyTable1.GetScalarFieldWrapperByIndex(0, 1, "IronLoss");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper22;
            scalarFieldWrapper22 = propertyTable1.GetScalarFieldWrapperByIndex(0, 2, "IronLoss");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper23;
            scalarFieldWrapper23 = propertyTable1.GetScalarFieldWrapperByIndex(0, 0, "BHLoop");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper24;
            scalarFieldWrapper24 = propertyTable1.GetScalarFieldWrapperByIndex(0, 1, "BHLoop");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper25;
            scalarFieldWrapper25 = propertyTable1.GetScalarFieldWrapperByIndex(0, 0, "magneticPoles");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper26;
            scalarFieldWrapper26 = propertyTable1.GetScalarFieldWrapperByIndex(0, 1, "magneticPoles");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper27;
            scalarFieldWrapper27 = propertyTable1.GetScalarFieldWrapperByIndex(0, 2, "magneticPoles");

            NXOpen.Fields.ScalarFieldWrapper scalarFieldWrapper28;
            scalarFieldWrapper28 = propertyTable1.GetScalarFieldWrapperByIndex(0, 0, "AnsysUserCreepTable");

            theSession.SetUndoMarkName(markId3, "Isotropic Material Dialog");

            theSession.SetUndoMarkVisibility(markId3, null, NXOpen.Session.MarkVisibility.Invisible);

            propertyTable1.SetIntegerPropertyValue("MatlAppliedEnvironment", 19);

            string[] propertyValue1 = new string[1];
            propertyValue1[0] = "designViewIso";
            propertyTable1.SetTextPropertyValue("ModelingAppliedViews", propertyValue1);

            // ----------------------------------------------
            //   Dialog Begin Isotropic Material
            // ----------------------------------------------
            theSession.SetUndoMarkName(markId3, "Isotropic Material");

            theSession.DeleteUndoMark(markId3, null);

            physicalMaterialBuilder1.Destroy();

            theSession.UndoToMark(markId2, "Inspect");

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Assign Material");

            theSession.DeleteUndoMark(markId4, null);

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Assign Material");
            NXOpen.PhysicalMaterial physicalMaterial2;
            try
            {

                physicalMaterial2 = workPart.MaterialManager.PhysicalMaterials.LoadFromMatmlLibrary("X:\\Materials\\BibliotecaMascarello.xml", mp);
            }
            catch
            {

                physicalMaterial2 = ((NXOpen.PhysicalMaterial)workPart.MaterialManager.PhysicalMaterials.FindObject($"PhysicalMaterial[{mp}"));
            }

            foreach (Body body in workPart.Bodies)
            {

                try
                {
                    NXOpen.NXObject[] objects1 = new NXOpen.NXObject[1];
                    objects1[0] = body;
                    physicalMaterial2.AssignObjects(objects1);
                }

                catch
                {


                }
            }

            // NXOpen.NXObject[] objects1 = new NXOpen.NXObject[1];
            //NXOpen.Body body1 = ((NXOpen.Body)workPart.Bodies.FindObject("SB_CONTOUR_FLANGE_BASE(2)"));



            theSession.DeleteUndoMark(markId5, null);

            theSession.SetUndoMarkName(id1, "Assign Material");

            physicalMaterialAssignBuilder1.Destroy();

            physicalMaterialListBuilder1.Destroy();
        }

        private void button10_Click(object sender, EventArgs e)
        {

            //Session theSession = Session.GetSession();
            //Part workPart = theSession.Parts.Work;

            //if (workPart != null)
            //{
            //    try
            //    {
            //        // Define a peça como editável (somente se não estiver bloqueada pelo Teamcenter)
            //        workPart.SetReadOnly(false);
            //        theSession.ListingWindow.WriteLine($"Peça '{workPart.FullPath}' definida como editável.");
            //    }
            //    catch (Exception ex)
            //    {
            //        theSession.ListingWindow.WriteLine($"Erro ao definir como editável: {ex.Message}");
            //    }
            //}
            //StreamReader str;
            //if (System.IO.File.Exists(@"C:\temp\itens_demanda.txt"))
            //{

            //    str = new StreamReader(@"C:\temp\itens_demanda.txt");
            //    using (str)
            //    {
            //        int cont = 0;
            //        string linha = "";
            //        while ((linha = str.ReadLine()) != null)
            //        {

            //            string[] quebra2 = linha.Split('-');

            //            int rev = Convert.ToInt32(quebra2[1].TrimEnd());
            //            //rev++;
            //            Open_model_save(quebra2[0], rev.ToString().PadLeft(3, '0'));

            //            Registrar_no_LOG(quebra2[0] + "-" + rev.ToString().PadLeft(3, '0') + " >>> ADD MP");
            //            //theSession = NXOpen.Session.GetSession();
            //            //workPart = theSession.Parts.Work;
            //            //displayPart = theSession.Parts.Display;


            //            //workPart.Save(NXOpen.BasePart.SaveComponents.True, NXOpen.BasePart.CloseAfterSave.False);

            //        }
            //    }
            //    str.Close();
            //}
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string[] itens =
            {
"371348- Chapa em alumínio stucco de 0,8 mm, 1450×485 mm, utilizada como painel de revestimento ou acabamento estrutural.",
"398160- Chapa em alumínio stucco de 0,8 mm, 2870×485 mm, destinada ao fechamento lateral ou painéis de proteção.",
"283517- Chapa em alumínio stucco de 0,8 mm, 2870×595 mm, aplicada em grandes superfícies de revestimento.",
"302802- Revestimento interno em alumínio stucco semi sino, 0,80 mm, 1008×415 mm, lado esquerdo, usado para acabamento interno.",
"224118- Chapa para porta de vão 900 mm GRMicro S4 utilizada no fechamento estrutural do compartimento.",
"555423- Suporte direcional para cinto três pontos, lado direito, estruturado com encosto, montado na área do rodapé.",
"566071- Chapa de alumínio stucco 0,8 mm, 645×40 mm, usada como complemento ou faixa de acabamento.",
"566845- Vedação inferior do bagageiro lateral MS3 para impedir infiltrações e poeira.",
"520643- Rodapé com trilho de 2475 mm aplicado na fixação e acabamento inferior interno.",
"566898- Chapa inferior BT MS3 lado 0, com múltiplas medidas, usada na estrutura inferior.",
"359012- Chapa inferior traseira LD saia 530 MS3 2125 mm, usada como reforço traseiro.",
"394173- Fechamento de painel LO916 E6 MS3 frente usado para selagem frontal.",
"521158- Perfil LNE38 4,25 mm 100×95 mm utilizado como reforço estrutural.",
"230457- Revestimento interno stucco semi sino 881×220 mm LD aplicado como acabamento interno.",
"473961- Chapa inferior da saia 535 MS3 220 mm usada como proteção lateral inferior.",
"550194- Perfil ZAR230 1,95 mm 595×50 usado como reforço estrutural.",
"357011- Chapa externa dobrada da porta micro onurea S2 2021 usada em portas automotivas.",
"569320- Perfil ZAR230 1,55 mm 452×118 usado como reforço estrutural.",
"563772- Suporte do conjunto do para-choque dianteiro DCX utilizado na fixação frontal.",
"345123- Chapa do para-barro traseiro esquerdo LO915 MS3 usada como proteção contra detritos.",
"230986- Revestimento interno alumínio stucco semi sino 881×250 mm LD.",
"324744- Chapa aluminizada 2,00 mm 210×250 mm usada para reforço ou acabamento localizado.",
"542972- Chapa de dobra leve dianteira lado direito OF1519R usada no fechamento frontal.",
"566574- Revestimento da caixa de roda dianteira em alumínio 1087 mm.",
"516348- Chapa de saia GRMicro S4 218 mm usada como proteção lateral inferior.",
"560889- Chapa inferior da saia 535 MS3 820 mm aplicada na parte inferior lateral.",
"545167- Rodapé com trilho 1922 mm para encaixe e fixação interna.",
"327632- Acabamento ABS 2 da divisão frontal DPM MS2.",
"540677- Chapa alumínio stucco 965×455 mm para revestimentos.",
"538863- Perfil ZAR230 1,25 mm 1023×64 usado como reforço.",
"549089- Chapa de fechamento entre sino lateral 1558×128×20.",
"561562- Placa de amarração para preparação do chassi ELLO.",
"525925- Protetor de pedra dianteiro esquerdo VW 18330 MCC usado contra impactos."
//"234606- Revestimento interno stucco semi sino 1008×240 mm LD.",
//"480467- Chapa inferior da saia 535 MS3 460 mm.",
//"546816- Chapa traseira DPM Elevitta Phantom 1000 usada no fechamento posterior.",
//"535559- Fechamento lateral da caixa de roda esquerda.",
//"388176- Chapa alumínio stucco 1565×635 mm usada em grandes revestimentos.",
//"357075- Vedação lateral esquerda da porta sedan unorea micro S2.",
//"521327- Chapa externa da tampa traseira MS3 ORE2 sem luz elevada.",
//"535804- Perfil ZAR230 1,55 mm 640×425 mm.",
//"524165- Chapa alumínio stucco 1500×700 mm para revestimentos.",
//"531495- Rodapé com trilho 530 mm.",
//"530521- Chapa inferior saia 535 BT MS3 lado 0 com medidas ampliadas usada no fechamento inferior.",
//"548200- Perfil Z lateral da caixa de roda usado como reforço.",
//"492331- Chapa primária da base de rodagem M4 833×535 usada como estrutura base.",
//"544595- Rodapé com trilho 610 mm para encaixe interno.",
//"533100- Peça interna da trava central da porta de vão 800 MDU 25.",
//"566633- Perfil LNE38 9,52 mm 73×73 usado como reforço estrutural.",
//"539800- Chapa inferior complementar LD VW porta pantográfica MS3 com recorte para janelão.",
//"328686- Portinhola para vão 1500 padrão R4 1050 usada como tampa de acesso.",
//"356234- Chapa inferior traseira LD saia 530 MS3 2780 mm para reforço do bagageiro.",
//"541472- Fechamento inferior do tanque R6.",
//"503843- Chapa inferior BT MS3 1528×365 mm lado 0 usada na base estrutural.",
//"559819- Perfil ZAR230 1,95 mm 105×40 mm.",
//"494468- Chapa inferior saia 535 BT MS3 2685×470 mm lado 1.",
//"395073- Revestimento interno stucco semi sino 881×257 mm LD.",
//"490516- Chapa de amarração traseira inferior LD Volvo B360R MCC.",
//"562033- Perfil Z de fechamento da caixa de roda alumínio LD GRV 878.",
//"424508- Revestimento interno stucco semi sino 881×300 mm LD.",
//"421118- Chapa para porta de vão 500 mm GRMicro S4.",
//"539107- Perfil ZAR230 1,55 mm 2495×37 mm.",
//"542880- Suporte de fixação do apoio de braço urbano.",
//"505319- Chapa inferior da saia 535 MS3 190 mm.",
//"503980- Revestimento interno stucco semi sino 1008×395 mm LD.",
//"211316- Chapa alumínio stucco 3000×595 mm usada como revestimento de grande área.",
//"555439- Rodapé com trilho 1160 mm.",
//"540184- Chapa alumínio stucco 1427×512 mm.",
//"234570- Chapa alumínio stucco 1430×595 mm.",
//"494192- Chapa de amarração lateral direita Volvo.",
//"471723- Chapa lateral esquerda para fechamento da caixa de bateria Iveco E6.",
//"320830- Tampa de acesso de manutenção LD O500 RSD MCC.",
//"303570- Chapa alumínio stucco 950×485 mm.",
//"554386- Acabamento externo entre portas 350 saia 610 MS3 vidro colorido.",
//"451256- Revestimento interno stucco semi sino 1008×425 mm LD.",
//"420863- Revestimento interno stucco semi sino 1008×440 mm LD.",
//"422803- Perfil L de placa de amarração da roda traseira MC.",
//"480482- Chapa inferior saia 535 BT MS3 lado 0 em seções 420 mm e 30 mm.",
//"509579- Chapa inferior da saia 535 MS3 1282 mm.",
//"559208- Perfil ZAR230 1,55 mm 188×95 mm.",
//"475834- Chapa inferior saia 535 BT MS3 lado 0 2980×415 mm.",
//"312467- Chapa alumínio stucco 350×485 mm.",
//"253432- Revestimento interno stucco semi sino 1008×200 mm LE.",
//"262023- Chapa complementar LD VW9160 MS3.",
//"570571- Chapa inferior BT MS3 lado 0 com múltiplas dimensões estruturais.",
//"316620- Chapa inferior LD/LE da roda traseira O500 RSD MCC.",
//"214187- Acabamento do meio do sino 385 mm.",
//"234567- Revestimento interno stucco semi sino 1008×270 mm LE.",
//"448495- Chapa inferior saia 535 BT MS3 lado 0 2830×415 mm.",
//"444160- Chapa inferior saia 535 BT MS3 lado 1 1362×195 mm.",
//"473023- Chapa de acabamento lateral da porta onurea MS2 Agrale.",
//"573337- Perfil ZAR230 1,55 mm 1567×815 mm.",
//"550621- Placa L de amarração LD 6,35 mm 116×200×80 DCX.",
//"279386- Chapa complementar LD MA10 MS3.",
//"549374- Chapa dobrada de fechamento lateral.",
//"208498- Revestimento interno stucco semi sino 980×275 mm.",
//"162350- Chapa de amarração dianteira ROMA R4.",
//"563847- Suporte para pino de mola a gás.",
//"324826- Chapa flangada para fechamento da portinhola do bagageiro.",
//"549624- Chapa de fechamento da caixa de detritos M4.",
//"553590- Porta conduite VW 18320 MCC.",
//"534637- Chapa externa da janela do motorista DCX.",
//"550283- Chapa dobrada de fechamento da caixa de detritos/WC M4 1050.",
//"556010- Chapa aluminizada de 2,00 mm com dimensões de 1290×780 mm, utilizada como reforço estrutural ou painel de fechamento.",
//"500707- Revestimento interno alumínio stucco semi sino 1008×645 mm lado direito, usado como acabamento interno.",
//"524421- Chapa de alumínio stucco 1120×610 mm aplicada como painel de revestimento.",
//"382482- Chapa de reforço da caixa de bateria, usada para rigidez e proteção do compartimento.",
//"447412- Revestimento interno alumínio stucco semi sino 1008×445 mm LD.",
//"227272- Revestimento interno stucco semi sino 881×380 mm lado esquerdo.",
//"113901- Suporte da caixa do estepe reduzido MBB R6, usado na fixação do conjunto.",
//"555848- Chapa alumínio stucco 254×245 mm usada como peça complementar de revestimento.",
//"572622- Braço dobrado tipo 4 para tampa traseira FS3/FS4, usado na articulação do conjunto.",
//"223249- Revestimento interno stucco semi sino 1008×270 mm LD.",
//"447469- Revestimento interno stucco semi sino 1008×315 mm LE.",
//"556216- Perfil ZAR230 de 1,95 mm 150×210 mm usado como reforço estrutural.",
//"572621- Braço dobrado tipo 3 para tampa traseira FS3/FS4, usado na articulação do sistema.",
//"568514- Chapa interna LO916 MS3 1285 mm para peça elevada e afastada, usada como fechamento estrutural.",
//"534137- Fechamento leve traseiro R4 1050 utilizado na cobertura e proteção da extremidade traseira.",
//"235032- Revestimento interno stucco semi sino 881×320 mm LE.",
//"253629- Revestimento interno stucco semi sino 1008×335 mm LE.",
//"539817- Chapa alumínio stucco 1300×455 mm para revestimento.",
//"531226- Perfil ZAR230 1,95 mm 75×485 mm usado como reforço localizado.",
//"429040- Reforço do cockpit lado esquerdo VW E6/RR6 usado para rigidez estrutural.",
//"348492- Fechamento da base do podest ELLO VW17260, usado para cobrir e reforçar a estrutura.",
//"390794- Revestimento interno stucco semi sino 881×270 mm LE.",
//"533439- Estrutura superior de poltrona escolar para ensaio de ancoragem.",
//"223668- Revestimento interno stucco semi sino 1008×265 mm LD.",
//"547263- Amarração traseira 100×260×118 DCX usada como reforço traseiro.",
//"475262- Fechamento sobre o rodado dianteiro M4 1050 para MBB OF1726.",
//"451033- Revestimento interno stucco semi sino 881×390 mm LD.",
//"327325- Revestimento interno stucco semi sino 881×400 mm LD.",
//"462639- Revestimento interno stucco semi sino 1008×530 mm LD.",
//"365314- Revestimento interno stucco semi sino 881×470 mm LE.",
//"549790- Fechamento da caixa de detritos M4.",
//"543496- Fechamento de painel tipo 7 para MBB OF1519R MDE-25.",
//"545124- Fechamento lateral superior do estante traseiro DCX.",
//"486136- Protetor de pedra do rodado traseiro LE/LD M4 1050 Iveco 17-280F E6.",
//"508408- Reforço em L da tampa da caixa de porta delimada.",
//"523845- Revestimento interno stucco semi sino 1008×450 mm LE.",
//"303564- Revestimento interno stucco semi sino 1008×375 mm LD.",
//"423579- Fechamento de painel MCC VW 18330.",
//"455923- Suporte de puxador da tampa GRV 2023.",
//"560806- Lado direito da peça estampada de apoio de poltrona rodoviária MLX.",
//"253801- Revestimento interno stucco semi sino 1008×365 mm LD.",
//"253613- Revestimento interno stucco semi sino 881×160 mm LE.",
//"550064- Chapa dobrada estrutural da grade dianteira.",
//"567346- Chapa da câmara LO916/48 pod avançado E6 MS2.",
//"553891- Chapa frontal do duto de ar-condicionado.",
//"235033- Revestimento interno stucco semi sino 881×190 mm LE.",
//"445660- Revestimento interno stucco semi sino 1008×480 mm LD.",
//"217784- Revestimento interno stucco semi sino 1008×230 mm LD.",
//"528680- Fixação do trinco da porta sedan MDE-25.",
//"541217- Peça inferior de apoio inclinado da poltrona rodoviária MLX.",
//"568891- Chapa inferior BT MS3 lado 1 com múltiplas dimensões estruturais.",
//"234989- Revestimento interno stucco semi sino 1008×250 mm LE.",
//"278448- Revestimento interno stucco semi sino 1008×255 mm LE.",
//"550579- Placa em L 6,35 mm 255×56×40 DCX usada como reforço.",
//"210002- Revestimento interno stucco semi sino 1008×280 mm LE.",
//"469447- Revestimento interno stucco semi sino 1008×510 mm LD.",
//"315414- Revestimento interno stucco semi sino 1008×365 mm LE.",
//"390798- Revestimento interno stucco semi sino 881×492 mm LD.",
//"262921- Revestimento interno stucco semi sino 881×260 mm LE.",
//"564861- Revestimento interno BPPlus DCX.",
//"552690- Acabamento superior PU 700×1190 usado como peça estética superior.",
//"543015- Contorno de fechamento mecânico da porta MDU.",
//"533815- Revestimento interno stucco semi sino 1008×295 mm LE.",
//"382684- Revestimento interno stucco semi sino 1008×300 mm LE.",
//"540395- Revestimento interno stucco semi sino 1008×385 mm LD.",
//"562326- Fechamento superior da caixa de tomada de ar OF1726L elétrico.",
//"514700- Direcionador de captação de ar MBB O500R R4.",
//"326294- Suporte de fixação da bengala da portinhola ELLO.",
//"556250- Perfil LNE38 9,52 mm 80×95 usado como reforço estrutural.",
//"533232- Chapa inferior do duto de ar-condicionado ROMA.",
//"520623- Chapa inferior da saia 535 BT MS3 lado 1 1690×525 mm.",
//"531606- Conjunto 02 soldado para ar injetado GRV 23.",
//"559040- Chapa dobrada superior da tampa usada na articulação e reforço.",
//"551566- Travessa externa superior/inferior da porta de vão 950 DCX.",
//"338573- Perfil L 301×135 para placa de amarração da roda traseira Volvo B430R MCC.",
//"564170- Chapa inferior R4/M4 285 mm usada como reforço inferior.",
//"559022- Chapa dobrada de fixação do varão.",
//"337166- Lado direito superior do batente da tampa traseira MCC.",
//"540223- Chapa inferior MS3 1587 mm.",
//"541728- Chapa inferior da base da poltrona do motorista.",
//"545643- Perfil ZAR230 1,95 mm 40×155 usado como reforço.",
//"539319- Chapa alumínio stucco 600×485 mm.",
//"524235- Fechamento superior da caixa central elétrica.",
//"520343- Perfil ZAR230 1,55 mm 860×890 mm usado como reforço estrutural ampliado.",
//"536406- Chapa de reforço da caixa de rodas esquerda DCX BYD.",
//"563249- Fixação retrátil 2 lado direito MDU-25.",
//"547080- Perfil ZAR230 1,95 mm 700×80 mm.",
//"529658- Chapa inferior saia 535 BT MS3 lado 0 2880×1920 mm.",
//"349914- Chapa externa da porta pantográfica micro S2.",
//"558998- Rodapé com trilho 830 mm.",
//"452947- Fixação da guia da porta traseira ML.",
//"460773- Chapa PSD MS3 ORE2 de fechamento inferior.",
//"459328- Suporte lateral do para-barro GRV lado direito escolar.",
//"570036- Amarração dianteira de fixação do podest Iveco ELLO.",
//"556425- Perfil ZAR230 2,70 mm 64×40 mm.",
//"571990- Perfil ZAR230 1,25 mm 765×380 mm usado como reforço ampliado.",

            };

            foreach (string item in itens)
            {
                string [] quebra_item = item.Split('-');
                NXFeatureExtractor nXFeature = new NXFeatureExtractor();
                nXFeature.ExtractAllPartsToJson(quebra_item[0], quebra_item[1]);
            }
           
          

            //theSession = Session.GetSession();
            //theUI = UI.GetUI();
            //theUfSession = UFSession.GetUFSession();
            //string fam = "212947";

            //string encoded_familia = Create.TCSearchItem(fam);

            //PartLoadStatus LdSt;
            //Part obj_familia;
            //try
            //{
            //    obj_familia = (Part)theSession.Parts.FindObject(encoded_familia);
            //}
            //catch
            //{
            //    obj_familia = theSession.Parts.Open(encoded_familia, out LdSt);
            //}

            //List<string> btt = new List<string>();
            //List<string> btp = new List<string>();
            //btt.Add("BT 1");

            //btp.Add("BT 1");
            //btp.Add("PGAP");
            //btp.Add("ZINCAGEM ELETROLITICA FRIO BRANCO");

            //if (obj_familia != null)
            //{
            //    theSession.Parts.SetDisplay(obj_familia, true, true, out LdSt);

            //}
            //insert_boletin_criacao(btt, btp, obj_familia);

            //Session theSession = Session.GetSession();
            //Part workPart = theSession.Parts.Work;

            //if (workPart != null)
            //{
            //    bool isReadOnly = workPart.IsReadOnly;

            //    if (isReadOnly)
            //    {
            //        theSession.ListingWindow.Open();
            //        theSession.ListingWindow.WriteLine($"A peça '{workPart.FullPath}' está em modo Read-Only.");
            //    }
            //    else
            //    {
            //        theSession.ListingWindow.WriteLine($"A peça '{workPart.FullPath}' está editável.");
            //    }
            //}

            //StreamReader str;
            //if (System.IO.File.Exists(@"C:\temp\itens_demanda.txt"))
            //{

            //    str = new StreamReader(@"C:\temp\itens_demanda.txt");
            //    using (str)
            //    {
            //        int cont = 0;
            //        string linha = "";
            //        while ((linha = str.ReadLine()) != null)
            //        {
            //            string[] quebra = linha.Split(';');
            //            string[] quebra2 = quebra[0].Split('-');

            //            int rev = Convert.ToInt32(quebra2[1].TrimEnd());
            //            rev++;

            //            string alt = conexao_bd.verifica_alteracao(quebra2[0], rev.ToString());

            //            if (alt == "1")
            //            {
            //                LOG_Message(quebra2[0] + "-" + rev.ToString().PadLeft(3, '0'));
            //            }
            //        }
            //    }
            //    str.Close();
            //}
        }

        private void button12_Click(object sender, EventArgs e)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            StreamReader str;
            if (System.IO.File.Exists(@"C:\temp\itens_demanda.txt"))
            {

                str = new StreamReader(@"C:\temp\itens_demanda.txt");
                using (str)
                {
                    int cont = 0;
                    string linha = "";
                    while ((linha = str.ReadLine()) != null)
                    {
                         string[] quebra = linha.Split(';');
                        string[] quebra2 = quebra[0].Split('-');
                        Registrar_no_LOG("Abrindo item  " + quebra2[0] + "-" + quebra2[1].PadLeft(3, '0'));
                        Open_model(quebra2[0], quebra2[1].PadLeft(3, '0'));
                        remove_drawing_model();
                        int rev = Convert.ToInt32(quebra2[1].TrimEnd());
                        rev++;
                        Registrar_no_LOG("Criando Revisao Item " + quebra2[0] + "-" + rev.ToString());
                        saveas(quebra2[0], quebra2[1].PadLeft(3, '0'));
                        Registrar_no_LOG("Revisao Item Criada com sucesso " + quebra2[0] + "-" + rev.ToString());
                        //string mp_new = "097439 - CH_AL_LISO_3105_H16_NBR7556_3000x1300x2,00mm";
                        //alterar_mp(mp_new);
                        //Registrar_no_LOG("Materia Prima Alterada para " + mp_new);

                        //Registrar_no_LOG("Verificar blank ");

                        //Verificar_Blank(quebra2[0], rev.ToString().PadLeft(3, '0'));
                        //Registrar_no_LOG("Blank ok");


                        //Save_Update();
                        alterar_mp_componentes_all();

                        Registrar_no_LOG("Salvando Model " + quebra2[0] + "-" + rev.ToString());
                        Registrar_no_LOG("Abrindo drawing " + quebra2[0] + "-" + rev.ToString().PadLeft(3, '0'));
                        //try
                        //{


                        Open_drawing_demanda(quebra2[0], rev.ToString().PadLeft(3, '0'));
                        deletar_alteracao();
                        //Insertb_alt();

                        preenche_alteracao("M - ALTERADO ITEM 000831 PARA 554339");

                        // NXOpen.Session theSession = NXOpen.Session.GetSession();
                        NXOpen.Part workPart = theSession.Parts.Work;
                        NXOpen.Part displayPart = theSession.Parts.Display;


                        Insertb_alt(workPart);


                        workPart.Save(NXOpen.BasePart.SaveComponents.True, NXOpen.BasePart.CloseAfterSave.True);
                        Registrar_no_LOG("Drawing salvo " + quebra2[0] + "-" + rev.ToString());
                        Registrar_no_LOG("Fechar arquivo " + quebra2[0] + "-" + rev.ToString());
                        //}
                        //catch
                        //{
                        //    Registrar_no_LOG("Sem Drawing " + quebra2[0] + "-" + rev.ToString());

                        //  //  NXOpen.Session theSession = NXOpen.Session.GetSession();
                        //    NXOpen.Part workPart = theSession.Parts.Work;
                        //    NXOpen.Part displayPart = theSession.Parts.Display;

                        //    theSession.Parts.CloseAll(BasePart.CloseModified.CloseModified, null);
                        //}

                        registrar_alteracao(quebra2[0], rev.ToString());
                        // Open_model_save(quebra2[0], rev.ToString().PadLeft(3, '0'));




                        //  workPart.Save(NXOpen.BasePart.SaveComponents.True, NXOpen.BasePart.CloseAfterSave.True);

                    }
                }
                str.Close();
            }

        }

        public void registrar_alteracao(string codigo, string rev)
        {
            DateTime d1 = DateTime.Now;
            string teste = conexao_bd.gravar_alteracao(codigo, rev, d1.ToString("yyyy/MM/dd hh:mm:ss"), "2", "32",
                "S", "ALTERADO ITEM 000831 PARA 554339", "Alteração sob demanda", "22", "0", "-", "3");
        }
        public static void alterar_mp_componente()
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.Assemblies.ReplaceComponentBuilder replaceComponentBuilder1;
            replaceComponentBuilder1 = workPart.AssemblyManager.CreateReplaceComponentBuilder();

            replaceComponentBuilder1.ComponentNameType = NXOpen.Assemblies.ReplaceComponentBuilder.ComponentNameOption.AsSpecified;

            replaceComponentBuilder1.ComponentName = "213298";

            NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 1"));
            bool added1;
            added1 = replaceComponentBuilder1.ComponentsToReplace.Add(component1);

            theSession.SetUndoMarkName(markId1, "Replace Component Dialog");

            replaceComponentBuilder1.ComponentName = "000831";

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            theSession.SetUndoMarkName(markId2, "Part file name Dialog");

            // ----------------------------------------------
            //   Dialog Begin Part file name
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Part file name");

            theSession.DeleteUndoMark(markId3, null);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Part file name");

            theSession.DeleteUndoMark(markId4, null);

            theSession.SetUndoMarkName(markId2, "Part file name");

            theSession.DeleteUndoMark(markId2, null);

            theSession.Parts.SetNonmasterSeedPartData("@DB/554339/000");

            replaceComponentBuilder1.ComponentName = "554339";

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Replace Component");

            theSession.DeleteUndoMark(markId5, null);

            NXOpen.Session.UndoMarkId markId6;
            markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Replace Component");

            replaceComponentBuilder1.ReplacementPart = "@DB/554339/000";

            replaceComponentBuilder1.SetComponentReferenceSetType(NXOpen.Assemblies.ReplaceComponentBuilder.ComponentReferenceSet.Maintain, null);

            NXOpen.PartLoadStatus partLoadStatus1;
            partLoadStatus1 = replaceComponentBuilder1.RegisterReplacePartLoadStatus();

            NXOpen.NXObject nXObject1;
            nXObject1 = replaceComponentBuilder1.Commit();

            partLoadStatus1.Dispose();
            NXOpen.ErrorList errorList1;
            errorList1 = replaceComponentBuilder1.GetErrorList();

            int length1;
            length1 = errorList1.Length;

            errorList1.Dispose();
            NXOpen.Tooling.AddReusablePart addReusablePart1;
            addReusablePart1 = workPart.ReusableParts.CreateBuilder();

            addReusablePart1.DestroyReusableBuilder();

            theSession.DeleteUndoMark(markId6, null);

            theSession.SetUndoMarkName(markId1, "Replace Component");

            replaceComponentBuilder1.Destroy();

            theSession.CleanUpFacetedFacesAndEdges();

            // ----------------------------------------------
            //   Menu: Tools->Automation->Journal->Stop Recording
            // ----------------------------------------------

        }
        public static void alterar_mp_componentes_all()
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();

            NXOpen.Assemblies.Component cmp1 = workPart.ComponentAssembly.RootComponent;
            Componentes.Add(cmp1);
            NXOpen.Assemblies.Component[] cmp_agrupador = Componentes.ToArray()[0].GetChildren();




            NXOpen.Assemblies.ReplaceComponentBuilder replaceComponentBuilder1;
            replaceComponentBuilder1 = workPart.AssemblyManager.CreateReplaceComponentBuilder();

            replaceComponentBuilder1.ComponentNameType = NXOpen.Assemblies.ReplaceComponentBuilder.ComponentNameOption.AsSpecified;

            foreach (NXOpen.Assemblies.Component comp in cmp_agrupador)
            {
                if (comp.Name.Contains("000831"))
                {
                    replaceComponentBuilder1.ComponentName = "000831";
                    bool added1;
                    added1 = replaceComponentBuilder1.ComponentsToReplace.Add(comp);
                }

            }
            //replaceComponentBuilder1.ComponentName = "566411";

            //NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 37"));
            //bool added1;
            //added1 = replaceComponentBuilder1.ComponentsToReplace.Add(component1);

            //NXOpen.Assemblies.Component component2 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 46"));
            //bool added2;
            //added2 = replaceComponentBuilder1.ComponentsToReplace.Add(component2);

            //NXOpen.Assemblies.Component component3 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 35"));
            //bool added3;
            //added3 = replaceComponentBuilder1.ComponentsToReplace.Add(component3);

            //NXOpen.Assemblies.Component component4 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 12"));
            //bool added4;
            //added4 = replaceComponentBuilder1.ComponentsToReplace.Add(component4);

            //NXOpen.Assemblies.Component component5 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 11"));
            //bool added5;
            //added5 = replaceComponentBuilder1.ComponentsToReplace.Add(component5);

            //NXOpen.Assemblies.Component component6 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 15"));
            //bool added6;
            //added6 = replaceComponentBuilder1.ComponentsToReplace.Add(component6);

            //NXOpen.Assemblies.Component component7 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 26"));
            //bool added7;
            //added7 = replaceComponentBuilder1.ComponentsToReplace.Add(component7);

            //NXOpen.Assemblies.Component component8 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 31"));
            //bool added8;
            //added8 = replaceComponentBuilder1.ComponentsToReplace.Add(component8);

            //NXOpen.Assemblies.Component component9 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 32"));
            //bool added9;
            //added9 = replaceComponentBuilder1.ComponentsToReplace.Add(component9);

            //NXOpen.Assemblies.Component component10 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 28"));
            //bool added10;
            //added10 = replaceComponentBuilder1.ComponentsToReplace.Add(component10);

            //NXOpen.Assemblies.Component component11 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 6"));
            //bool added11;
            //added11 = replaceComponentBuilder1.ComponentsToReplace.Add(component11);

            //NXOpen.Assemblies.Component component12 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 42"));
            //bool added12;
            //added12 = replaceComponentBuilder1.ComponentsToReplace.Add(component12);

            //NXOpen.Assemblies.Component component13 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 36"));
            //bool added13;
            //added13 = replaceComponentBuilder1.ComponentsToReplace.Add(component13);

            //NXOpen.Assemblies.Component component14 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 27"));
            //bool added14;
            //added14 = replaceComponentBuilder1.ComponentsToReplace.Add(component14);

            //NXOpen.Assemblies.Component component15 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 22"));
            //bool added15;
            //added15 = replaceComponentBuilder1.ComponentsToReplace.Add(component15);

            //NXOpen.Assemblies.Component component16 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 25"));
            //bool added16;
            //added16 = replaceComponentBuilder1.ComponentsToReplace.Add(component16);

            //NXOpen.Assemblies.Component component17 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 24"));
            //bool added17;
            //added17 = replaceComponentBuilder1.ComponentsToReplace.Add(component17);

            //NXOpen.Assemblies.Component component18 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 34"));
            //bool added18;
            //added18 = replaceComponentBuilder1.ComponentsToReplace.Add(component18);

            //NXOpen.Assemblies.Component component19 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 4"));
            //bool added19;
            //added19 = replaceComponentBuilder1.ComponentsToReplace.Add(component19);

            //NXOpen.Assemblies.Component component20 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 30"));
            //bool added20;
            //added20 = replaceComponentBuilder1.ComponentsToReplace.Add(component20);

            //NXOpen.Assemblies.Component component21 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 45"));
            //bool added21;
            //added21 = replaceComponentBuilder1.ComponentsToReplace.Add(component21);

            //NXOpen.Assemblies.Component component22 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 21"));
            //bool added22;
            //added22 = replaceComponentBuilder1.ComponentsToReplace.Add(component22);

            //NXOpen.Assemblies.Component component23 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 1"));
            //bool added23;
            //added23 = replaceComponentBuilder1.ComponentsToReplace.Add(component23);

            //NXOpen.Assemblies.Component component24 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 2"));
            //bool added24;
            //added24 = replaceComponentBuilder1.ComponentsToReplace.Add(component24);

            //NXOpen.Assemblies.Component component25 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 14"));
            //bool added25;
            //added25 = replaceComponentBuilder1.ComponentsToReplace.Add(component25);

            //NXOpen.Assemblies.Component component26 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 3"));
            //bool added26;
            //added26 = replaceComponentBuilder1.ComponentsToReplace.Add(component26);

            //NXOpen.Assemblies.Component component27 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 29"));
            //bool added27;
            //added27 = replaceComponentBuilder1.ComponentsToReplace.Add(component27);

            //NXOpen.Assemblies.Component component28 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 8"));
            //bool added28;
            //added28 = replaceComponentBuilder1.ComponentsToReplace.Add(component28);

            //NXOpen.Assemblies.Component component29 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 13"));
            //bool added29;
            //added29 = replaceComponentBuilder1.ComponentsToReplace.Add(component29);

            //NXOpen.Assemblies.Component component30 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 39"));
            //bool added30;
            //added30 = replaceComponentBuilder1.ComponentsToReplace.Add(component30);

            //NXOpen.Assemblies.Component component31 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 44"));
            //bool added31;
            //added31 = replaceComponentBuilder1.ComponentsToReplace.Add(component31);

            //NXOpen.Assemblies.Component component32 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 18"));
            //bool added32;
            //added32 = replaceComponentBuilder1.ComponentsToReplace.Add(component32);

            //NXOpen.Assemblies.Component component33 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 41"));
            //bool added33;
            //added33 = replaceComponentBuilder1.ComponentsToReplace.Add(component33);

            //NXOpen.Assemblies.Component component34 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 10"));
            //bool added34;
            //added34 = replaceComponentBuilder1.ComponentsToReplace.Add(component34);

            //NXOpen.Assemblies.Component component35 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 23"));
            //bool added35;
            //added35 = replaceComponentBuilder1.ComponentsToReplace.Add(component35);

            //NXOpen.Assemblies.Component component36 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 38"));
            //bool added36;
            //added36 = replaceComponentBuilder1.ComponentsToReplace.Add(component36);

            //NXOpen.Assemblies.Component component37 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 16"));
            //bool added37;
            //added37 = replaceComponentBuilder1.ComponentsToReplace.Add(component37);

            //NXOpen.Assemblies.Component component38 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 33"));
            //bool added38;
            //added38 = replaceComponentBuilder1.ComponentsToReplace.Add(component38);

            //NXOpen.Assemblies.Component component39 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 9"));
            //bool added39;
            //added39 = replaceComponentBuilder1.ComponentsToReplace.Add(component39);

            //NXOpen.Assemblies.Component component40 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 40"));
            //bool added40;
            //added40 = replaceComponentBuilder1.ComponentsToReplace.Add(component40);

            //NXOpen.Assemblies.Component component41 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 43"));
            //bool added41;
            //added41 = replaceComponentBuilder1.ComponentsToReplace.Add(component41);

            //NXOpen.Assemblies.Component component42 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 17"));
            //bool added42;
            //added42 = replaceComponentBuilder1.ComponentsToReplace.Add(component42);

            //NXOpen.Assemblies.Component component43 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 19"));
            //bool added43;
            //added43 = replaceComponentBuilder1.ComponentsToReplace.Add(component43);

            //NXOpen.Assemblies.Component component44 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 5"));
            //bool added44;
            //added44 = replaceComponentBuilder1.ComponentsToReplace.Add(component44);

            //NXOpen.Assemblies.Component component45 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 20"));
            //bool added45;
            //added45 = replaceComponentBuilder1.ComponentsToReplace.Add(component45);

            //NXOpen.Assemblies.Component component46 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT 000831/000 7"));
            //bool added46;
            //added46 = replaceComponentBuilder1.ComponentsToReplace.Add(component46);

            theSession.SetUndoMarkName(markId1, "Replace Component Dialog");

            replaceComponentBuilder1.ComponentName = "";

            // ----------------------------------------------
            //   Dialog Begin Replace Component
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Start");

            theSession.SetUndoMarkName(markId2, "Part file name Dialog");

            // ----------------------------------------------
            //   Dialog Begin Part file name
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Part file name");

            theSession.DeleteUndoMark(markId3, null);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Part file name");

            theSession.DeleteUndoMark(markId4, null);

            theSession.SetUndoMarkName(markId2, "Part file name");

            theSession.DeleteUndoMark(markId2, null);

            theSession.Parts.SetNonmasterSeedPartData("@DB/554339/000");

            replaceComponentBuilder1.ComponentName = "554339";

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Replace Component");

            theSession.DeleteUndoMark(markId5, null);

            NXOpen.Session.UndoMarkId markId6;
            markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Replace Component");

            replaceComponentBuilder1.ReplacementPart = "@DB/554339/000";

            replaceComponentBuilder1.SetComponentReferenceSetType(NXOpen.Assemblies.ReplaceComponentBuilder.ComponentReferenceSet.Maintain, null);

            NXOpen.PartLoadStatus partLoadStatus1;
            partLoadStatus1 = replaceComponentBuilder1.RegisterReplacePartLoadStatus();

            NXOpen.NXObject nXObject1;
            nXObject1 = replaceComponentBuilder1.Commit();

            partLoadStatus1.Dispose();
            NXOpen.ErrorList errorList1;
            errorList1 = replaceComponentBuilder1.GetErrorList();

            int length1;
            length1 = errorList1.Length;

            errorList1.Dispose();
            NXOpen.Tooling.AddReusablePart addReusablePart1;
            addReusablePart1 = workPart.ReusableParts.CreateBuilder();

            addReusablePart1.DestroyReusableBuilder();

            theSession.DeleteUndoMark(markId6, null);

            theSession.SetUndoMarkName(markId1, "Replace Component");

            replaceComponentBuilder1.Destroy();
            // ----------------------------------------------
            //   Menu: Tools->Automation->Journal->Stop Recording
            // ----------------------------------------------

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btn_lista_itens_Click(object sender, EventArgs e)
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
        Part obj_familia;
        string Familia;

        Dictionary<string, List<string>> atributosDinamicos = new Dictionary<string, List<string>>();

        DataSet DataFamily = new DataSet("DataFamily");
        public string limites = "";

        string name_atb = "";
        List<string> valores_att = new List<string>();
        List<string> att_Atributo = new List<string>();
        List<string> att_AtributoBase = new List<string>();
        List<string> att_Valores = new List<string>();
        private void carregar_dados_fam_teamcenter()
        {
            limites = "";
            lista_atributos.Clear();
            DataFamily.Clear();
            dtg_resultado.DataSource = null;
            dtg_resultado.Rows.Clear();
            dtg_resultado.Columns.Clear();
            string cod = cmb_familia_pc.Text.Substring(0, 6);

            if (cmb_tipo_mp.Text != "Chapa AXB" && cmb_tipo_mp.Text != "Chapa AXB Aluminio" && cmb_tipo_mp.Text != "Tubo CV")
            {
                if (System.IO.File.Exists(@"X:\Xml\Imagens\" + cod + ".JPG"))
                {
                    Image image = Image.FromFile(@"X:\Xml\Imagens\" + cod + ".JPG");
                    this.picbox_fams.Image = image;
                }
            }
            grp_geral.Controls.Clear();
            atributosDinamicos.Clear();
            limites = conexao_bd.busca_limites(cod);
            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theUfSession = UFSession.GetUFSession();


            string encoded_familia = TCSearchItem(cmb_familia_pc.Text.Substring(0, 6), theSession, theUI, theUfSession);
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

            //var fonteDeDados = new AutoCompleteStringCollection();
            //fonteDeDados.AddRange(new string[]
            //{
            //"51",
            //"525",
            //"5600",
            //"580",
            //"123",
            //"456",
            //"5100",
            //"51234"
            //});


            string jsonStringAuto = conexao_bd.busca_atributos_autos(cod);
          
           // Atributos_Auto jsonDataAuto = JsonConvert.DeserializeObject<Atributos_Auto>(jsonStringAuto);

            Atributos_Auto obj = JsonConvert.DeserializeObject<Atributos_Auto>(jsonStringAuto);


          
            if (obj != null)
            {
                foreach (var item in obj.Lista_Atributos_Auto)
                {

                    att_Atributo.Add(item.Atributo);
                    foreach (var item1 in item.AtributoBase)
                    {
                        att_AtributoBase.Add(item1);
                    }
                    foreach (var item2 in item.Valor)
                    {
                        att_Valores.Add(item2);
                    }
                }
            }


            string jsonString = conexao_bd.busca_atributos_json(cod);
           
            Atributos jsonData = JsonConvert.DeserializeObject<Atributos>(jsonString);

            List<string> name_atb =new List<string>();
            valores_att.Clear();
            if (jsonData != null)
            {
                foreach (var item in jsonData.Lista_atributos)
                {
                    name_atb.Add(item.Atributo);
                    valores_att.Add(item.Atributo);
                }
            }
           

            Var_fam_tubos.Visible = false;
            Var_fam_chapas.Visible = false;
            grp_geral.Visible = true;
            int i = 0;
            for (j = 5; j <= attr.Length - 1; j++)
            {
                int pox_x = 0;
                theUfSession.Obj.AskName(attr[j], out atb);

                if (atb != "MATERIAL")
                {
                    int pos_y = 25 + (i * 30); ///  aqui está o calculo posição dentro groupbox (descrição da variavel)
                    string lbl = "aut_lbl_" + atb;
                    System.Windows.Forms.Label lbl_desc_add = new System.Windows.Forms.Label();
                    lbl_desc_add.Name = lbl;
                    lbl_desc_add.Text = atb;
                    lbl_desc_add.Font = new System.Drawing.Font("Calibri Light", 11);
                    lbl_desc_add.Location = new System.Drawing.Point(10, pos_y);
                   
                    lbl_desc_add.Visible = true;
                    //lbl_desc_add.AutoSize = true;
                    lbl_desc_add.BackColor = System.Drawing.Color.AliceBlue;
                    lbl_desc_add.Size = new Size(100, 20);
                    

                    pox_x = lbl_desc_add.Size.Width+5+lbl_desc_add.Location.X;/// posição do label baseado no calculo
                    lbl_desc_add.TextAlign = ContentAlignment.MiddleRight;
                    grp_geral.Controls.Add(lbl_desc_add);// adiciona a label ao groupbox( troque o nome grp_geral pelo nome que vc tem criado)

                    lbl_desc_add.BringToFront();

                    // codigo abaixo está duplicando o combox inserido  

                    int pos_x = 0;
                    int width = 0;
                    if ( name_atb.Contains(atb))
                    {

                        var listaLocal = new List<string>();

                        foreach (var item in jsonData.Lista_atributos)
                        {
                            if (item.Atributo == atb)
                            {
                                foreach (var val in item.Valor)
                                {
                                    listaLocal.Add(val);
                                }
                            }
                        }
                        ComboBox cmb_att = new ComboBox();
                        string txt = atb;
                        cmb_att.Name = txt; // nome do textbox igual a ao nome da variavel, para usar na criação ou pesquisa de itens 
                    //  txt_att.Multiline = false;
                        cmb_att.Font = new System.Drawing.Font("Calibri Light", 10);
                        cmb_att.Size = new Size(100, 15);
                        cmb_att.DataSource = listaLocal;
                        cmb_att.DropDownWidth = 200;
                       
                        cmb_att.Location = new System.Drawing.Point(pox_x, pos_y - 3);

                        /// posição do textbox baseado no calculo
                        /// 
                        cmb_att.Visible = true;
                        
                        grp_geral.Controls.Add(cmb_att); // adiciona a textbox ao groupbox( troque o nome grp_geral pelo nome que vc tem criado)
                        cmb_att.BringToFront();
                        pos_x = cmb_att.Location.X;
                        width = cmb_att.Size.Width;
                    }
                    else
                    {
                        
                       
                        TextBox txt_att = new TextBox();
                       // txt_att.
                        /// cria textbox para prenchimento dos valores 
                        string txt = atb;
                        txt_att.Name = txt; // nome do textbox igual a ao nome da variavel, para usar na criação ou pesquisa de itens 
                        txt_att.Multiline = false;
                        txt_att.Font = new System.Drawing.Font("Calibri Light", 10);
                        txt_att.Size = new Size(55, 15);
                        //txt_att.TextChanged += TextBox1_TextChanged;
                        txt_att.AutoCompleteSource = AutoCompleteSource.CustomSource;

                        txt_att.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        txt_att.Enter += txt_entrar;
                        txt_att.Leave += txt_Leave;
                        txt_att.Location = new System.Drawing.Point(pox_x, pos_y - 3);

                        /// posição do textbox baseado no calculo
                        txt_att.Visible = true;
                        if (att_Atributo.Contains(atb))
                        {
                            txt_att.Enabled = false;
                        }
                        grp_geral.Controls.Add(txt_att); // adiciona a textbox ao groupbox( troque o nome grp_geral pelo nome que vc tem criado)
                        txt_att.BringToFront();

                        pos_x = txt_att.Location.X;
                        width = txt_att.Size.Width;
                    }

                   


                    ///  aqui está o calculo posição dentro groupbox (descrição da variavel)
                    string lbl_tol = "tol_lbl_" + atb;
                    System.Windows.Forms.Label lbl_tol_add = new System.Windows.Forms.Label();
                    lbl_tol_add.Name = lbl_tol;
                    lbl_tol_add.Text = "Tol ";
                    lbl_tol_add.Font = new System.Drawing.Font("Calibri Light", 11);
                    lbl_tol_add.Location = new System.Drawing.Point(pos_x + width, pos_y);
                    lbl_tol_add.Visible = true;
                    lbl_tol_add.Size = new Size(30, 20); /// posição do label baseado no calculo
                    lbl_tol_add.TextAlign = ContentAlignment.MiddleRight;
                    grp_geral.Controls.Add(lbl_tol_add);// adiciona a label ao groupbox( troque o nome grp_geral pelo nome que vc tem criado)

                    TextBox txt_tol = new TextBox(); /// cria textbox para prenchimento dos valores 
                    string toltxt = "tol" + atb;
                    txt_tol.Name = toltxt; // nome do textbox igual a ao nome da variavel, para usar na criação ou pesquisa de itens 
                    txt_tol.Text = "0";
                    txt_tol.Multiline = false;
                    txt_tol.Font = new System.Drawing.Font("Calibri Light", 10);
                    txt_tol.Size = new Size(20, 15);
                    txt_tol.Location = new System.Drawing.Point(lbl_tol_add.Location.X + lbl_tol_add.Width + 5, pos_y - 3); /// posição do textbox baseado no calculo
                    txt_tol.Visible = true;

                    grp_geral.Controls.Add(txt_tol); // adiciona a textbox ao groupbox( troque o nome grp_geral pelo nome que vc tem criado)
                    txt_tol.BringToFront();

                    string[] lim = limites.Split(';');

                    for (int k = 0; k <= lim.Length - 1; k++)
                    {
                        if (lim[k] == atb)
                        {
                            string lbl_min = "min_lbl" + atb;
                            System.Windows.Forms.Label lbl_min_add = new System.Windows.Forms.Label();
                            lbl_min_add.Name = lbl_min;
                            string valor = "";
                            if (lim[k + 1] != "0")
                            {
                                valor = valor + "min:" + lim[k + 1] + " ";
                            }
                            if (lim[k + 2] != "0")
                            {
                                valor = valor + "max:" + lim[k + 2];
                            }
                            lbl_min_add.Text = valor;
                            lbl_min_add.Font = new System.Drawing.Font("Calibri Light", 11);
                            lbl_min_add.Location = new System.Drawing.Point(txt_tol.Location.X + txt_tol.Size.Width + 5, pos_y);
                            lbl_min_add.Visible = true;
                            lbl_min_add.Size = new Size(150, 20); /// posição do label baseado no calculo
                            lbl_min_add.TextAlign = ContentAlignment.MiddleLeft;
                            grp_geral.Controls.Add(lbl_min_add);
                        }
                    }

                    i++;
                }

                //dtg_resultado.Columns.Add(atb, atb);
            }

       
            DataSet ds = new DataSet();
            ds.Tables.Add("Familia");

            string[] att_lista = new string[attr.Length - 5];
            // MessageBox.Show(attr.Length.ToString() + "  " + (attr.Length - 5).ToString());
            // Adicionando as colunas à tabela
            for (j = 0; j <= attr.Length - 1; j++)
            {
                theUfSession.Obj.AskName(attr[j], out atb);
                ds.Tables["Familia"].Columns.Add(atb);
                if (j > 4)
                {

                    att_lista[j - 5] = atb;
                    lista_atributos.Add(atb);
                }
                //DataGridViewTextBoxColumn coluna = new DataGridViewTextBoxColumn();
                //coluna.HeaderText = atb;
                //coluna.Name = atb;
                //coluna.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                dtg_resultado.Columns.Add(atb, atb);


            }

            dtg_resultado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dtg_resultado.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg_resultado.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            for (int member_idx = 0; member_idx < family_data.member_count; member_idx++)
            {
                UFFam.MemberData member_data;
                theUfSession.Fam.AskMemberRowData(families[0], member_idx, out member_data);

                DataRow newRow = ds.Tables["Familia"].NewRow();
                newRow[0] = member_data.values[0];
                for (int k = 1; k < member_data.values.Length; k++)
                {
                    newRow[k] = member_data.values[k];
                }

                ds.Tables["Familia"].Rows.Add(newRow);
            }
            DataFamily = ds;
            DataTable tabela = DataFamily.Tables["Familia"];
            foreach (DataRow dr in tabela.Rows)
            {
                string[] valores = dr.ItemArray.Select(field => field.ToString()).ToArray();
                dtg_resultado.Rows.Add(valores);
            }

            for (int m = 0; m < att_lista.Length; m++)
            {
                string nomeColuna = att_lista[m];
                List<string> list_temp = new List<string>();
                foreach (DataRow dr in ds.Tables["Familia"].Rows)
                {
                    if (nomeColuna != "MATERIAL")
                    {
                        // Pegue o valor da coluna desejada
                        string valor = dr[nomeColuna].ToString();

                        list_temp.Add(valor);
                    }
                }
                if (nomeColuna != "MATERIAL")
                {
                    atributosDinamicos.Add(nomeColuna, list_temp);
                    string[] valores = list_temp.ToArray();
                }
            }

            foreach (var item in atributosDinamicos)
            {
                LOG_Message(item.Key + "  " + item.Value);
            }


            obj_familia.Close(NXOpen.BasePart.CloseWholeTree.False, NXOpen.BasePart.CloseModified.CloseModified, null);
        }
        
        private DataSet carregar_dados_fam_teamcenter_autocad(Dictionary<string, double> _dados)
        {
            LOG_Message("Carregando dados da familia " + _dados["FAM"].ToString() + " do Teamcenter...");

            lista_atributos.Clear();
            DataFamily.Clear();

            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theUfSession = UFSession.GetUFSession();

            // string cod_fam = _dados["FAM"].ToString();
            string encoded_familia = TCSearchItem(_dados["FAM"].ToString(), theSession, theUI, theUfSession);
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


            Var_fam_tubos.Visible = false;
            Var_fam_chapas.Visible = false;
            grp_geral.Visible = true;
            int i = 0;


            DataSet ds = new DataSet();
            ds.Tables.Add("Familia");

            string[] att_lista = new string[attr.Length - 5];

            for (j = 0; j <= attr.Length - 1; j++)
            {
                theUfSession.Obj.AskName(attr[j], out atb);
                ds.Tables["Familia"].Columns.Add(atb);
            }


            for (int member_idx = 0; member_idx < family_data.member_count; member_idx++)
            {
                UFFam.MemberData member_data;
                theUfSession.Fam.AskMemberRowData(families[0], member_idx, out member_data);

                DataRow newRow = ds.Tables["Familia"].NewRow();
                newRow[0] = member_data.values[0];
                for (int k = 1; k < member_data.values.Length; k++)
                {
                    newRow[k] = member_data.values[k];
                }

                ds.Tables["Familia"].Rows.Add(newRow);
            }
            DataFamily = ds;

            obj_familia.Close(NXOpen.BasePart.CloseWholeTree.False, NXOpen.BasePart.CloseModified.CloseModified, null);

            return ds;
        }
        private void carregar_dados_fam_cv_teamcenter()
        {

            lista_atributos.Clear();
            DataFamily.Clear();
            dtg_resultado.DataSource = null;
            dtg_resultado.Rows.Clear();
            dtg_resultado.Columns.Clear();
            string cod = cmb_familia_pc.Text.Substring(0, 6);

            if (cmb_tipo_mp.Text != "Chapa AXB" && cmb_tipo_mp.Text != "Chapa AXB Aluminio" && cmb_tipo_mp.Text != "Tubo CV")
            {
                if (System.IO.File.Exists(@"X:\Xml\Imagens\" + cod + ".JPG"))
                {
                    Image image = Image.FromFile(@"X:\Xml\Imagens\" + cod + ".JPG");
                    this.picbox_fams.Image = image;
                }
            }
            grp_geral.Controls.Clear();
            atributosDinamicos.Clear();
            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theUfSession = UFSession.GetUFSession();


            string encoded_familia = TCSearchItem("FAM_CV_" + cmb_familia_pc.Text, theSession, theUI, theUfSession);
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


         

            
            Var_fam_tubos.Visible = false;
            Var_fam_chapas.Visible = false;
            grp_geral.Visible = true;
            int i = 0;
            for (j = 5; j <= attr.Length - 1; j++)
            {
                theUfSession.Obj.AskName(attr[j], out atb);
                if (atb != "MATERIAL")
                {
                    int pos_y = 25 + (i * 30); ///  aqui está o calculo posição dentro groupbox (descrição da variavel)
                    string lbl = "aut_lbl_" + atb;
                    System.Windows.Forms.Label lbl_desc_add = new System.Windows.Forms.Label();
                    lbl_desc_add.Name = lbl;
                    lbl_desc_add.Text = atb;
                    lbl_desc_add.Font = new System.Drawing.Font("Calibri Light", 11);
                    lbl_desc_add.Location = new System.Drawing.Point(5, pos_y);
                    lbl_desc_add.Visible = true;
                    lbl_desc_add.Size = new Size(80, 20); /// posição do label baseado no calculo
                    lbl_desc_add.TextAlign = ContentAlignment.MiddleRight;
                    grp_geral.Controls.Add(lbl_desc_add);// adiciona a label ao groupbox( troque o nome grp_geral pelo nome que vc tem criado)

                    lbl_desc_add.BringToFront();


                    TextBox txt_att = new TextBox(); /// cria textbox para prenchimento dos valores 
                    string txt = atb;
                    txt_att.Name = txt; // nome do textbox igual a ao nome da variavel, para usar na criação ou pesquisa de itens 
                    txt_att.Multiline = false;
                    txt_att.Font = new System.Drawing.Font("Calibri Light", 10);
                    txt_att.Size = new Size(55, 15);
                    txt_att.TextChanged += TextBox1_TextChanged;
                    // txt_att.Leave += txt_Leave;
                    txt_att.Location = new System.Drawing.Point(80 + 15, pos_y - 3); /// posição do textbox baseado no calculo
                    txt_att.Visible = true;

                    grp_geral.Controls.Add(txt_att); // adiciona a textbox ao groupbox( troque o nome grp_geral pelo nome que vc tem criado)
                    txt_att.BringToFront();


                    ///  aqui está o calculo posição dentro groupbox (descrição da variavel)
                    string lbl_tol = "tol_lbl_" + atb;
                    System.Windows.Forms.Label lbl_tol_add = new System.Windows.Forms.Label();
                    lbl_tol_add.Name = lbl_tol;
                    lbl_tol_add.Text = "Tol ";
                    lbl_tol_add.Font = new System.Drawing.Font("Calibri Light", 11);
                    lbl_tol_add.Location = new System.Drawing.Point(txt_att.Location.X + txt_att.Size.Width, pos_y);
                    lbl_tol_add.Visible = true;
                    lbl_tol_add.Size = new Size(30, 20); /// posição do label baseado no calculo
                    lbl_tol_add.TextAlign = ContentAlignment.MiddleRight;
                    grp_geral.Controls.Add(lbl_tol_add);// adiciona a label ao groupbox( troque o nome grp_geral pelo nome que vc tem criado)

                    TextBox txt_tol = new TextBox(); /// cria textbox para prenchimento dos valores 
                    string toltxt = "tol" + atb;
                    txt_tol.Name = toltxt; // nome do textbox igual a ao nome da variavel, para usar na criação ou pesquisa de itens 
                    txt_tol.Text = "0";
                    txt_tol.Multiline = false;
                    txt_tol.Font = new System.Drawing.Font("Calibri Light", 10);
                    txt_tol.Size = new Size(20, 15);
                    txt_tol.Location = new System.Drawing.Point(lbl_tol_add.Location.X + lbl_tol_add.Width + 5, pos_y - 3); /// posição do textbox baseado no calculo
                    txt_tol.Visible = true;

                    grp_geral.Controls.Add(txt_tol); // adiciona a textbox ao groupbox( troque o nome grp_geral pelo nome que vc tem criado)
                    txt_tol.BringToFront();

                    ckd_vao.Location = new System.Drawing.Point(175, 198);

                    grp_geral.Controls.Add(ckd_vao);
                    i++;
                }
            }

            DataSet ds = new DataSet();
            ds.Tables.Add("Familia");

            string[] att_lista = new string[attr.Length - 5];
            // MessageBox.Show(attr.Length.ToString() + "  " + (attr.Length - 5).ToString());
            // Adicionando as colunas à tabela
            for (j = 0; j <= attr.Length - 1; j++)
            {
                theUfSession.Obj.AskName(attr[j], out atb);
                ds.Tables["Familia"].Columns.Add(atb);
                if (j > 4)
                {
                    att_lista[j - 5] = atb;
                    lista_atributos.Add(atb);
                }
                //DataGridViewTextBoxColumn coluna = new DataGridViewTextBoxColumn();
                //coluna.HeaderText = atb;
                //coluna.Name = atb;
                //coluna.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                dtg_resultado.Columns.Add(atb, atb);


            }

            dtg_resultado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dtg_resultado.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg_resultado.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Adicionando os dados das linhas
            for (int member_idx = 0; member_idx < family_data.member_count; member_idx++)
            {
                UFFam.MemberData member_data;
                theUfSession.Fam.AskMemberRowData(families[0], member_idx, out member_data);


                // Crie uma nova linha para a tabela
                DataRow newRow = ds.Tables["Familia"].NewRow();

                // Preenchendo a linha com os valores
                newRow[0] = member_data.values[0];
                for (int k = 1; k < member_data.values.Length; k++)
                {
                    newRow[k] = member_data.values[k];
                }

                // Adicionando a linha à tabela
                ds.Tables["Familia"].Rows.Add(newRow);
            }
            DataFamily = ds;

            DataTable tabela = DataFamily.Tables["Familia"];
            foreach (DataRow dr in tabela.Rows)
            {
                string[] valores = dr.ItemArray.Select(field => field.ToString()).ToArray();
                dtg_resultado.Rows.Add(valores);
            }
            // Exibindo os valores da tabela
            // Percorra todas as linhas da tabela e pegue os valores da coluna específica


            for (int m = 0; m < att_lista.Length; m++)
            {
                string nomeColuna = att_lista[m];
                List<string> list_temp = new List<string>();
                foreach (DataRow dr in ds.Tables["Familia"].Rows)
                {
                    if (nomeColuna != "MATERIAL")
                    {
                        // Pegue o valor da coluna desejada
                        string valor = dr[nomeColuna].ToString();

                        list_temp.Add(valor);
                    }
                }
                if (nomeColuna != "MATERIAL")
                {
                    atributosDinamicos.Add(nomeColuna, list_temp);
                    string[] valores = list_temp.ToArray();
                }
            }


            obj_familia.Close(NXOpen.BasePart.CloseWholeTree.False, NXOpen.BasePart.CloseModified.CloseModified, null);
        }

        private void carregar_dados_fam_teamcenter_tubos(string cod)
        {

            ////lista_atributos.Clear();
            ////DataFamily.Clear();
            ////dtg_resultado.DataSource = null;
            ////dtg_resultado.Rows.Clear();
            ////dtg_resultado.Columns.Clear();
            ////string cod = cmb_familia_pc.Text.Substring(0, 6);

            ////if (cmb_tipo_mp.Text != "Chapa AXB" && cmb_tipo_mp.Text != "Chapa AXB Aluminio" && cmb_tipo_mp.Text != "Tubo CV")
            ////{
            ////    if (System.IO.File.Exists(@"X:\Xml\Imagens\" + cod + ".JPG"))
            ////    {
            ////        Image image = Image.FromFile(@"X:\Xml\Imagens\" + cod + ".JPG");
            ////        this.picbox_fams.Image = image;
            ////    }
            ////}
            ////grp_geral.Controls.Clear();
            ////atributosDinamicos.Clear();

            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theUfSession = UFSession.GetUFSession();


            string encoded_familia = TCSearchItem(cmb_familia_pc.Text.Substring(0, 6), theSession, theUI, theUfSession);
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


            Var_fam_tubos.Visible = false;
            Var_fam_chapas.Visible = false;
            grp_geral.Visible = true;
            int i = 0;
            for (j = 5; j <= attr.Length - 1; j++)
            {
                theUfSession.Obj.AskName(attr[j], out atb);
                if (atb != "MATERIAL")
                {
                    int pos_y = 25 + (i * 30); ///  aqui está o calculo posição dentro groupbox (descrição da variavel)
                    string lbl = "aut_lbl_" + atb;
                    System.Windows.Forms.Label lbl_desc_add = new System.Windows.Forms.Label();
                    lbl_desc_add.Name = lbl;
                    lbl_desc_add.Text = atb;
                    lbl_desc_add.Font = new System.Drawing.Font("Calibri Light", 11);
                    lbl_desc_add.Location = new System.Drawing.Point(10, pos_y);
                    lbl_desc_add.Visible = true;
                    lbl_desc_add.Size = new Size(60, 20); /// posição do label baseado no calculo
                    lbl_desc_add.TextAlign = ContentAlignment.MiddleRight;
                    grp_geral.Controls.Add(lbl_desc_add);// adiciona a label ao groupbox( troque o nome grp_geral pelo nome que vc tem criado)

                    lbl_desc_add.BringToFront();


                    TextBox txt_att = new TextBox(); /// cria textbox para prenchimento dos valores 
                    string txt = atb;
                    txt_att.Name = txt; // nome do textbox igual a ao nome da variavel, para usar na criação ou pesquisa de itens 
                    txt_att.Multiline = false;
                    txt_att.Font = new System.Drawing.Font("Calibri Light", 10);
                    txt_att.Size = new Size(55, 15);
                    txt_att.TextChanged += TextBox1_TextChanged;
                    //txt_att.Leave += txt_Leave;
                    txt_att.Location = new System.Drawing.Point(80, pos_y - 3); /// posição do textbox baseado no calculo
                    txt_att.Visible = true;

                    grp_geral.Controls.Add(txt_att); // adiciona a textbox ao groupbox( troque o nome grp_geral pelo nome que vc tem criado)
                    txt_att.BringToFront();


                    ///  aqui está o calculo posição dentro groupbox (descrição da variavel)
                    string lbl_tol = "tol_lbl_" + atb;
                    System.Windows.Forms.Label lbl_tol_add = new System.Windows.Forms.Label();
                    lbl_tol_add.Name = lbl_tol;
                    lbl_tol_add.Text = "Tol ";
                    lbl_tol_add.Font = new System.Drawing.Font("Calibri Light", 11);
                    lbl_tol_add.Location = new System.Drawing.Point(txt_att.Location.X + txt_att.Size.Width, pos_y);
                    lbl_tol_add.Visible = true;
                    lbl_tol_add.Size = new Size(30, 20); /// posição do label baseado no calculo
                    lbl_tol_add.TextAlign = ContentAlignment.MiddleRight;
                    grp_geral.Controls.Add(lbl_tol_add);// adiciona a label ao groupbox( troque o nome grp_geral pelo nome que vc tem criado)

                    TextBox txt_tol = new TextBox(); /// cria textbox para prenchimento dos valores 
                    string toltxt = "tol" + atb;
                    txt_tol.Name = toltxt; // nome do textbox igual a ao nome da variavel, para usar na criação ou pesquisa de itens 
                    txt_tol.Text = "0";
                    txt_tol.Multiline = false;
                    txt_tol.Font = new System.Drawing.Font("Calibri Light", 10);
                    txt_tol.Size = new Size(20, 15);
                    txt_tol.Location = new System.Drawing.Point(lbl_tol_add.Location.X + lbl_tol_add.Width + 5, pos_y - 3); /// posição do textbox baseado no calculo
                    txt_tol.Visible = true;

                    grp_geral.Controls.Add(txt_tol); // adiciona a textbox ao groupbox( troque o nome grp_geral pelo nome que vc tem criado)
                    txt_tol.BringToFront();
                    i++;
                }

                //dtg_resultado.Columns.Add(atb, atb);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add("Familia");

            string[] att_lista = new string[attr.Length - 5];
            // MessageBox.Show(attr.Length.ToString() + "  " + (attr.Length - 5).ToString());
            // Adicionando as colunas à tabela
            for (j = 0; j <= attr.Length - 1; j++)
            {
                theUfSession.Obj.AskName(attr[j], out atb);
                ds.Tables["Familia"].Columns.Add(atb);
                if (j > 4)
                {

                    att_lista[j - 5] = atb;
                    lista_atributos.Add(atb);
                }
                //DataGridViewTextBoxColumn coluna = new DataGridViewTextBoxColumn();
                //coluna.HeaderText = atb;
                //coluna.Name = atb;
                //coluna.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                dtg_resultado.Columns.Add(atb, atb);


            }

            dtg_resultado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dtg_resultado.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg_resultado.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            for (int member_idx = 0; member_idx < family_data.member_count; member_idx++)
            {
                UFFam.MemberData member_data;
                theUfSession.Fam.AskMemberRowData(families[0], member_idx, out member_data);

                DataRow newRow = ds.Tables["Familia"].NewRow();
                newRow[0] = member_data.values[0];
                for (int k = 1; k < member_data.values.Length; k++)
                {
                    newRow[k] = member_data.values[k];
                }

                ds.Tables["Familia"].Rows.Add(newRow);
            }
            DataFamily = ds;
            DataTable tabela = DataFamily.Tables["Familia"];
            foreach (DataRow dr in tabela.Rows)
            {
                string[] valores = dr.ItemArray.Select(field => field.ToString()).ToArray();
                dtg_resultado.Rows.Add(valores);
            }

            for (int m = 0; m < att_lista.Length; m++)
            {
                string nomeColuna = att_lista[m];
                List<string> list_temp = new List<string>();
                foreach (DataRow dr in ds.Tables["Familia"].Rows)
                {
                    if (nomeColuna != "MATERIAL")
                    {
                        // Pegue o valor da coluna desejada
                        string valor = dr[nomeColuna].ToString();

                        list_temp.Add(valor);
                    }
                }
                if (nomeColuna != "MATERIAL")
                {
                    atributosDinamicos.Add(nomeColuna, list_temp);
                    string[] valores = list_temp.ToArray();
                }
            }
            obj_familia.Close(NXOpen.BasePart.CloseWholeTree.False, NXOpen.BasePart.CloseModified.CloseModified, null);
        }
        private void cmb_familia_pc_Validating(object sender, CancelEventArgs e)
        {
        }
        private ContextMenuStrip menuSugestoes = new ContextMenuStrip();
        private TextBox ultimoTextBox = null;
        ComboBox cmb_att = new ComboBox();

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        bool select_cmb = false;
        private void Cmb_att_SelectedIndexChanged(object sender, EventArgs e)
        {
            select_cmb = true;
            ultimoTextBox.Text = cmb_att.SelectedItem.ToString();
            ultimoTextBox.Focus();



        }
        private void txt_Leave(object sender, EventArgs e)
        {

            TextBox txt_att = (TextBox)sender;

            if (txt_att.Text != "")
            {
                string[] lim = limites.Split(';');
                double valor_atb = Convert.ToDouble(txt_att.Text);
                double min = 0;
                double max = 0;
                for (int k = 0; k <= lim.Length - 1; k++)
                {
                    if (lim[k] == txt_att.Name)
                    {
                        min = Convert.ToDouble(lim[k + 1]);
                        max = Convert.ToDouble(lim[k + 2]);
                        if (valor_atb < min)
                        {
                            MessageBox.Show("Valor mínimo para o atributo " + txt_att.Name + " é " + min.ToString(), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txt_att.Focus();
                        }
                        else if (valor_atb > max && max != 0)
                        {
                            MessageBox.Show("Valor máximo para o atributo " + txt_att.Name + " é " + max.ToString(), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txt_att.Focus();
                        }
                    }
                }
                int i = 0;
                foreach (string item in att_AtributoBase)
                {

                    if (txt_att.Name == item)
                    {
                        double valor_tol = Convert.ToDouble(txt_att.Text) - Convert.ToDouble(att_Valores[i]);
                        string nome_txt_tol = att_Atributo[0];
                        TextBox txt_tol = (TextBox)grp_geral.Controls.Find(nome_txt_tol, true)[0];
                        txt_tol.Text = valor_tol.ToString("F0");
                    }
                    i++;
                }
                





            }
        }
        private void txt_entrar(object sender, EventArgs e)
        {
            TextBox txt_att = (TextBox)sender;

            //LogMessage_fam(txt_att.Name);

            var fonteDeDados = new AutoCompleteStringCollection();

            foreach (var item in atributosDinamicos)
            {
                // LogMessage_fam("nome:" + txt_att.Name + "   "+ "KEy:" + item.Key);
                if (item.Key == txt_att.Name)
                {
                    //LogMessage_fam(item.Key);
                    string[] valores = item.Value.ToArray();
                    //LogMessage_fam(valores)
                    fonteDeDados.AddRange(valores);
                }
            }
            //txt_att.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_att.AutoCompleteCustomSource = fonteDeDados;
            //txt_att.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

        }
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

            //       cmb_att.SelectedIndexChanged -= Cmb_att_SelectedIndexChanged;
            //       cmb_att.Items.Clear();
            //       cmb_att.SelectedIndex = -1;

            //       grp_geral.Controls.Remove(cmb_att);
            //       TextBox txt_att = (TextBox)sender;


            //       var fonteDeDados = new AutoCompleteStringCollection();

            //       txt_att.AutoCompleteSource = AutoCompleteSource.CustomSource;
            //       txt_att.AutoCompleteCustomSource = fonteDeDados;
            //       txt_att.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            //       //cmb_att.Location = new System.Drawing.Point(txt_att.Location.X+ txt_att.Width+10+30+20, txt_att.Location.Y);
            //       //cmb_att.Size = new Size(70, 20);
            //       //if (select_cmb == false)
            //       //{
            //       //    grp_geral.Controls.Add(cmb_att);
            //       //}


            //       ultimoTextBox = txt_att;
            //       string chave = txt_att.Name;
            //       string textoDigitado = txt_att.Text.Trim().ToLower();


            //       //if (string.IsNullOrEmpty(textoDigitado))
            //       //    return;

            //       var valoresFiltrados = atributosDinamicos[chave]
            //.Where(valor => valor.ToLower().StartsWith(textoDigitado))
            //.ToList();
            //       if (select_cmb == false)
            //       {

            //           if (valoresFiltrados.Count == 0)
            //               return;
            //           foreach (string valor in valoresFiltrados)
            //           {
            //               if (cmb_att.Items.Contains(valor) == false)

            //               {
            //                   cmb_att.Items.Add(valor);// menuSugestoes.Items.Add(valor);
            //               }

            //           }
            //           if (cmb_att.Items.Count > 0)
            //           {
            //               cmb_att.SelectedIndex = 0;
            //               cmb_att.DroppedDown = true;
            //           }
            //           else
            //           {
            //               grp_geral.Controls.Remove(cmb_att);
            //               cmb_att.Items.Clear();
            //           }
            //           cmb_att.SelectedIndexChanged += Cmb_att_SelectedIndexChanged;
            //       }
            //       select_cmb = false;
            //       txt_att.BeginInvoke((MethodInvoker)delegate { txt_att.Focus(); });
        }
        private void btn_pesquisar_fam_Click(object sender, EventArgs e)
        {
            frm_pesquisa_fam frm_Pesquisa_Fam = new frm_pesquisa_fam();
            frm_Pesquisa_Fam.StartPosition = FormStartPosition.CenterParent;
            frm_Pesquisa_Fam.ShowDialog();

            string codigo = frm_Pesquisa_Fam.Codigo_ret;
            string desc = frm_Pesquisa_Fam.Desc_ret;

            if (codigo != "")
            {
                cmb_familia_pc.Text = codigo + " - " + desc;
            }
        }

        private void lista_mp_Opened(object sender, EventArgs e)
        {
            //if (menuSugestoes.Visible == true)
            //{
            //    this.ultimoTextBox.Focus();

            //} 
        }

        private void lista_mp_Opening(object sender, CancelEventArgs e)
        {
            this.ultimoTextBox.Focus();
        }

        private void txt_alt_tubo_Leave(object sender, EventArgs e)
        {

        }

        private void dtg_resultado_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dtg_resultado_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hit = dtg_resultado.HitTest(e.X, e.Y);
                
                // Verifica se o clique foi na primeira coluna e em uma linha válida
                if (hit.RowIndex >= 0 && hit.ColumnIndex == 0)
                {
                    // Seleciona a célula clicada
                    // dtg_resultado.ClearSelection();
                    
                    dtg_resultado.Rows[hit.RowIndex].Cells[hit.ColumnIndex].Selected = true;

                    string selectadmin = conexao_bd.select_user_admin(Environment.UserName);
                    if (selectadmin != "1")
                    {
                        menu_acoes.Items.Remove(criarrev00ToolStripMenuItem);
                        menu_acoes.Items.Remove(gERARToolStripMenuItem);
                        //btn_mcc.Visible = true;
                        //Tab_fams.TabPages.Remove(Ferramentas);
                        //tab_itens.TabPages.Remove(tab_admin);
                    }
                    // Exibe o menu de contexto na posição do mouse
                    menu_acoes.Show(dtg_resultado, e.Location);
                }
            }
        }

        private void inserirbolToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in dtg_resultado.SelectedRows)
            {
                string[] item = new string[dtg_resultado.ColumnCount];
                int i = 0;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    item[i] = cell.Value.ToString();
                    i++;
                }

                string descricao_final = item[1];
                int rev = Convert.ToInt32(item[4]) + 1;
                int index_col = 0;
                lista_boletins.Items.Clear();
                List<string> listaPintura = new List<string>();
                List<string> listaSelecionados = new List<string>();
                bool boletim = false;

                foreach (DataGridViewColumn col in dtg_resultado.Columns)
                {
                    if (col.HeaderText == "PINTURA")
                    {

                        DialogResult pintura = MessageBox.Show("Inserir boletim de pintura?", "Pintura", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (pintura == DialogResult.Yes)
                        {
                            index_col = col.Index;

                            string ver_boletim = conexao_bd.verifica_boletim_fam(cmb_familia_pc.Text.Substring(0, 6));

                            if (ver_boletim != "")
                            {

                                boletim = true;
                                (listaSelecionados, listaPintura) = insert_boletin_fam_sincrona(ver_boletim);

                                if (boletim == true)
                                {
                                    registro_boletim_fam(listaSelecionados, listaPintura, item[0], rev.ToString());
                                }
                            }
                            item[index_col] = "1";
                            descricao_final = descricao_final + "_C/_PINTURA";
                        }
                        else
                        {
                            item[index_col] = "0";
                            descricao_final = descricao_final.Replace("_C/_PINTURA", "");
                        }
                    }
                }
                item[1] = descricao_final;
                item[2] = descricao_final;
                string dados = string.Join(";", item);


                frmPrincipal.Registrar_no_LOG("Atualizando Item: " + item[0] + " - " + dados);

                conexao_db_TeamCenter conn = new conexao_db_TeamCenter();

                var (pitem, pitemrev) = conn.busca_dados_alt_desc(item[0]);


                string up = conn.update_desc(descricao_final, pitem, pitemrev);
                atualizacao_fam_2312.update_item_fam(Familia, item[0], obj_familia, item, false);
            }
        }
        private void upadateToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in dtg_resultado.SelectedRows)
            {
                string[] item = new string[dtg_resultado.ColumnCount];
                int i = 0;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    item[i] = cell.Value.ToString();
                    i++;
                }

                string descricao_final = item[1];
               // int rev = Convert.ToInt32(item[4]) + 1;
                
                //string desc_padrao = conexao_bd.select_desc(cmb_familia_pc.Text.Substring(0, 6));

                //string[] descricao = desc_padrao.Split(';');
                
                //int k = 0;
                //for (int j = 5; j <= item.Length - 1; j++)
                //{
                //    descricao_final = descricao_final + item[j] + descricao[k + 1];

                //    k++;
                //}
                item[1] = descricao_final;
                item[2] = descricao_final;
                string dados = string.Join(";", item);


                frmPrincipal.Registrar_no_LOG("Atualizando Item: " + item[0] + " - " + dados);

                conexao_db_TeamCenter conn = new conexao_db_TeamCenter();

                var (pitem, pitemrev) = conn.busca_dados_alt_desc(item[0]);

                string rev = item[4];

                string up = conn.update_desc(descricao_final, pitem, rev.PadLeft(3,'0'));

                bool boletim = false;

                int index_col = 0;
                foreach (DataGridViewColumn col in dtg_resultado.Columns)
                {
                    if (col.HeaderText == "PINTURA")
                    {
                        index_col = col.Index;
                    }
                }
                if (item[index_col] == "1")
                {
                    boletim = true;
                }
                else
                {
                    boletim = false;
                }

                atualizacao_fam_2312.update_item_fam(Familia, item[0], obj_familia, item, boletim);
            }
        }
        private static string criar_rev(string codigo, int rev)
        {
            //Session theSession = Session.GetSession();
            //Part workPart = theSession.Parts.Work;
            //Part displayPart = theSession.Parts.Display;
            //UFSession theUfSession = UFSession.GetUFSession();

            //NXObject[] objects1 = new NXObject[1];
            //objects1[0] = workPart;

            //string codigo = objects1[0].GetStringAttribute("DB_PART_NO");
            //int revisao = Convert.ToInt16(objects1[0].GetStringAttribute("DB_PART_REV"));

            if (rev > 0)
            {
                frm_motivo_alteracao abrir_mot = new frm_motivo_alteracao();
             //   WindowState = FormWindowState.Minimized;
                abrir_mot.ShowDialog();

                string tipo = abrir_mot.Tipo;
                string desc = abrir_mot.Desc;
                string decisao = abrir_mot.Decisao;

                if (decisao == "1")
                {
                    string data = DateTime.Now.ToString("dd/MM/yyyy");
                    string usuario = Environment.UserName;

                    Registrar_alteracao(codigo + ";" + rev + ";" + data + ";" + usuario + ";" + tipo.Substring(0, 1) + ";" + desc);

                    using (var p = System.Diagnostics.Process.Start(@"C:\app_projeto\Sistema_Controle_Alteracao.exe"))
                    {
                        p.WaitForExit(); // aguarda o usuário fechar o programa
                                         // se precisar: int exitCode = p.ExitCode;
                    }
                  
                }
                string retorno = tipo.Substring(0, 1) + "-" + desc;
                return retorno;
            }
            else
            {
                MessageBox.Show("A revisão deve ser diferente de \"000\"", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return "";
        }

        private void criarRevisãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogMessage_fam($"Total de itens para gerar {dtg_resultado.SelectedRows.Count.ToString()}");
            int cont = dtg_resultado.SelectedRows.Count;
            foreach (DataGridViewRow row in dtg_resultado.SelectedRows)
            {
                string[] item = new string[dtg_resultado.ColumnCount];
                int i = 0;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    item[i] = cell.Value.ToString();
                    i++;
                }
              
                string dados = string.Join(";", item);

                int rev = Convert.ToInt32(item[4])+1;
                frmPrincipal.Registrar_no_LOG("Item que será revisado: " + item[0] + " - " + dados);
                int index_col = 0;
                lista_boletins.Items.Clear();
                List<string> listaPintura = new List<string>();
                List<string> listaSelecionados = new List<string>();
                bool boletim = false;
                

                //if(boletim == true)
                //{
                //    string boletins = string.Join(";", listaSelecionados);

                //    // MessageBox.Show(boletins + "  " + index_col.ToString());
                //    //conexao_bd.atualiza_boletim_fam(cmb_familia_pc.Text.Substring(0, 6), boletins);
                //}
                string selectadmin = conexao_bd.select_user_admin(Environment.UserName);
                if (selectadmin == "1")
                {

                    foreach (DataGridViewColumn col in dtg_resultado.Columns)
                    {
                        if (col.HeaderText == "PINTURA")
                        { 
                                index_col = col.Index;
                        }
                    }
                    
                    if (item[index_col] == "1")
                    {
                        boletim = true;
                    }
                    else
                    {
                        boletim = false;
                    }
                    bool revisar = false;
                    //if (item[7] == "2")
                    //{
                        atualizacao_fam_2312.criar_rev_item_fam_demanda(Familia, item[0], obj_familia, item, "M - ATUALIZADO VISTAS", boletim, listaSelecionados, listaPintura, index_col);
                        cont--;
                        LogMessage_fam(tag + $" - Item {item[0]} atualizado com motivo de alteração por admin. Itens restantes para gerar revisão: {cont.ToString()}");
                    //}
                    //else
                    //{
                      
                    //}
                   
                }
                else
                {
                    foreach (DataGridViewColumn col in dtg_resultado.Columns)
                    {
                        if (col.HeaderText == "PINTURA")
                        {

                            DialogResult pintura = MessageBox.Show("Inserir boletim de pintura?", "Pintura", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (pintura == DialogResult.Yes)
                            {
                                index_col = col.Index;

                                string ver_boletim = conexao_bd.verifica_boletim_fam(cmb_familia_pc.Text.Substring(0, 6));

                                if (ver_boletim != "")
                                {

                                    boletim = true;
                                    (listaSelecionados, listaPintura) = insert_boletin_fam_sincrona(ver_boletim);

                                    if (boletim == true)
                                    {
                                        registro_boletim_fam(listaSelecionados, listaPintura, item[0], rev.ToString());
                                    }
                                }
                            }
                        }
                    }

                    DialogResult resp = System.Windows.Forms.MessageBox.Show($"Será criada a Revisão {rev.ToString().PadLeft(3, '0')} para o item {item[0]}\n" +
                     "Deseja continuar?", "Atencão", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                    if (DialogResult.Yes == resp)
                    {
                        string desc_rev = criar_rev(item[0], rev);

                        if (desc_rev != "")
                        {
                            // MessageBox.Show(desc_rev);
                            atualizacao_fam_2312.criar_rev_item_fam(Familia, item[0], obj_familia, item, desc_rev, boletim, listaSelecionados, listaPintura, index_col);
                        }
                        else
                        {
                            MessageBox.Show("Revisão não criada. Falta a descrição repita o processo");
                        }

                    }
                }


                //atualizacao_fam_2312.update_item_fam(Familia, item[0], obj_familia, item);
            }
            
        }

        public static void SelectCurves(ref NXObject[] selectedObjects)
        {


            UI ui = NXOpen.UI.GetUI();

            string message = "Selecione a superfície estacionária para o Flat Solid";
            string title = "Secione uma face";

            Selection.SelectionScope scope = Selection.SelectionScope.WorkPart;
            bool keepHighlighted = false;
            bool includeFeatures = false;
            Selection.Response response = default(Selection.Response);

            Selection.SelectionAction selectionAction = Selection.SelectionAction.ClearAndEnableSpecific;

            Selection.MaskTriple[] selectionMask_array = new Selection.MaskTriple[100];
            {
                selectionMask_array[0].Type = UFConstants.UF_face_type;
                selectionMask_array[0].Subtype = 0;
                selectionMask_array[0].SolidBodySubtype = 0;
            }



            response = ui.SelectionManager.SelectObjects(message, title, scope, selectionAction, includeFeatures, keepHighlighted, selectionMask_array, out selectedObjects);

            if (response == Selection.Response.Cancel | response == Selection.Response.Back)
            {
                return;
            }
        }
        private static (Feature, Face) criar_flat()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            NXObject[] objects1 = null;
            SelectCurves(ref objects1);
            Face face_flat = (Face)objects1[0];


            // ----------------------------------------------
            //   Menu: Application->Design->Sheet Metal
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Enter Sheet Metal");

            theSession.ApplicationSwitchImmediate("UG_APP_SBSM");

            theSession.CleanUpFacetedFacesAndEdges();

            // ----------------------------------------------
            //   Menu: Insert->Flat Pattern->Flat Solid...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.Features.Feature nullNXOpen_Features_Feature = null;
            NXOpen.Features.SheetMetal.FlatSolidBuilder flatSolidBuilder1;
            flatSolidBuilder1 = workPart.Features.SheetmetalManager.CreateFlatSolidFeatureBuilder(nullNXOpen_Features_Feature);

            flatSolidBuilder1.SetApplicationContext(NXOpen.Features.SheetMetal.ApplicationContext.NxSheetMetal);

            flatSolidBuilder1.LayerSetting = NXOpen.Features.SheetMetal.FlatSolidBuilder.LayerSettingOption.Preference;

            flatSolidBuilder1.FlatBodyLayer = 1;

            flatSolidBuilder1.FlatBodyColor = 189;

            flatSolidBuilder1.Orientation = NXOpen.Features.SheetMetal.FlatSolidBuilder.OrientationType.Csys;

            flatSolidBuilder1.OuterCornerTreatment.Value.SetFormula("0.1");

            flatSolidBuilder1.InnerCornerTreatment.Value.SetFormula("0.1");

            flatSolidBuilder1.FixAtTimestamp = true;

            theSession.SetUndoMarkName(markId2, "Flat Solid Dialog");

            NXOpen.Features.SheetMetal.FeatureProperty insidecornertreatmenttype1;
            insidecornertreatmenttype1 = workPart.Preferences.SheetMetalPreferences.GetInsideCornerTreatmentType();

            NXOpen.Features.SheetMetal.CornerTreatmentBuilder cornerTreatmentBuilder1;
            cornerTreatmentBuilder1 = flatSolidBuilder1.InnerCornerTreatment;

            cornerTreatmentBuilder1.TreatmentType = NXOpen.Features.SheetMetal.CornerTreatmentBuilder.CornerTreatmentType.None;

            NXOpen.Expression expression1;
            expression1 = cornerTreatmentBuilder1.Value;

            NXOpen.Expression expression2;
            expression2 = workPart.Preferences.SheetMetalPreferences.GetFlatPatternInsideCornerTreatmentValue();

            string name1;
            name1 = expression2.Name;

            expression1.RightHandSide = name1;

            NXOpen.Features.SheetMetal.FeatureProperty outsidecornertreatmenttype1;
            outsidecornertreatmenttype1 = workPart.Preferences.SheetMetalPreferences.GetOutsideCornerTreatmentType();

            NXOpen.Features.SheetMetal.CornerTreatmentBuilder cornerTreatmentBuilder2;
            cornerTreatmentBuilder2 = flatSolidBuilder1.OuterCornerTreatment;

            cornerTreatmentBuilder2.TreatmentType = NXOpen.Features.SheetMetal.CornerTreatmentBuilder.CornerTreatmentType.None;

            NXOpen.Expression expression3;
            expression3 = cornerTreatmentBuilder2.Value;

            NXOpen.Expression expression4;
            expression4 = workPart.Preferences.SheetMetalPreferences.GetFlatPatternOutsideCornerTreatmentValue();

            string name2;
            name2 = expression4.Name;

            expression3.RightHandSide = name2;


            flatSolidBuilder1.StationaryFace.Value = face_flat;

            NXOpen.SelectFace selectFace1;
            selectFace1 = flatSolidBuilder1.StationaryFace;

            selectFace1.Value = face_flat;

            flatSolidBuilder1.Orientation = default;

            expression3.SetFormula(name2);

            expression1.SetFormula(name1);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Flat Solid");

            theSession.DeleteUndoMark(markId3, null);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Flat Solid");

            NXOpen.Features.Feature feature1;
            feature1 = flatSolidBuilder1.CommitFeature();

            theSession.DeleteUndoMark(markId4, null);

            theSession.SetUndoMarkName(markId2, "Flat Solid");

            flatSolidBuilder1.Destroy();

            theSession.CleanUpFacetedFacesAndEdges();

            return (feature1, face_flat);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            NXObject[] objects1 = null;
            SelectCurves(ref objects1);
            Face face_flat = (Face)objects1[0];


            // ----------------------------------------------
            //   Menu: Application->Design->Sheet Metal
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Enter Sheet Metal");

            theSession.ApplicationSwitchImmediate("UG_APP_SBSM");

            theSession.CleanUpFacetedFacesAndEdges();

            // ----------------------------------------------
            //   Menu: Insert->Flat Pattern->Flat Solid...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.Features.Feature nullNXOpen_Features_Feature = null;
            NXOpen.Features.SheetMetal.FlatSolidBuilder flatSolidBuilder1;
            flatSolidBuilder1 = workPart.Features.SheetmetalManager.CreateFlatSolidFeatureBuilder(nullNXOpen_Features_Feature);

            flatSolidBuilder1.SetApplicationContext(NXOpen.Features.SheetMetal.ApplicationContext.NxSheetMetal);

            flatSolidBuilder1.LayerSetting = NXOpen.Features.SheetMetal.FlatSolidBuilder.LayerSettingOption.Preference;

            flatSolidBuilder1.FlatBodyLayer = 1;

            flatSolidBuilder1.FlatBodyColor = 189;

            flatSolidBuilder1.Orientation = NXOpen.Features.SheetMetal.FlatSolidBuilder.OrientationType.Csys;

            flatSolidBuilder1.OuterCornerTreatment.Value.SetFormula("0.1");

            flatSolidBuilder1.InnerCornerTreatment.Value.SetFormula("0.1");

            flatSolidBuilder1.FixAtTimestamp = true;

            theSession.SetUndoMarkName(markId2, "Flat Solid Dialog");

            NXOpen.Features.SheetMetal.FeatureProperty insidecornertreatmenttype1;
            insidecornertreatmenttype1 = workPart.Preferences.SheetMetalPreferences.GetInsideCornerTreatmentType();

            NXOpen.Features.SheetMetal.CornerTreatmentBuilder cornerTreatmentBuilder1;
            cornerTreatmentBuilder1 = flatSolidBuilder1.InnerCornerTreatment;

            cornerTreatmentBuilder1.TreatmentType = NXOpen.Features.SheetMetal.CornerTreatmentBuilder.CornerTreatmentType.None;

            NXOpen.Expression expression1;
            expression1 = cornerTreatmentBuilder1.Value;

            NXOpen.Expression expression2;
            expression2 = workPart.Preferences.SheetMetalPreferences.GetFlatPatternInsideCornerTreatmentValue();

            string name1;
            name1 = expression2.Name;

            expression1.RightHandSide = name1;

            NXOpen.Features.SheetMetal.FeatureProperty outsidecornertreatmenttype1;
            outsidecornertreatmenttype1 = workPart.Preferences.SheetMetalPreferences.GetOutsideCornerTreatmentType();

            NXOpen.Features.SheetMetal.CornerTreatmentBuilder cornerTreatmentBuilder2;
            cornerTreatmentBuilder2 = flatSolidBuilder1.OuterCornerTreatment;

            cornerTreatmentBuilder2.TreatmentType = NXOpen.Features.SheetMetal.CornerTreatmentBuilder.CornerTreatmentType.None;

            NXOpen.Expression expression3;
            expression3 = cornerTreatmentBuilder2.Value;

            NXOpen.Expression expression4;
            expression4 = workPart.Preferences.SheetMetalPreferences.GetFlatPatternOutsideCornerTreatmentValue();

            string name2;
            name2 = expression4.Name;

            expression3.RightHandSide = name2;


            flatSolidBuilder1.StationaryFace.Value = face_flat;

            NXOpen.SelectFace selectFace1;
            selectFace1 = flatSolidBuilder1.StationaryFace;

            selectFace1.Value = face_flat;

            flatSolidBuilder1.Orientation = default;

            expression3.SetFormula(name2);

            expression1.SetFormula(name1);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Flat Solid");

            theSession.DeleteUndoMark(markId3, null);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Flat Solid");

            NXOpen.Features.Feature feature1;
            feature1 = flatSolidBuilder1.CommitFeature();

            theSession.DeleteUndoMark(markId4, null);

            theSession.SetUndoMarkName(markId2, "Flat Solid");

            flatSolidBuilder1.Destroy();

            theSession.CleanUpFacetedFacesAndEdges();
        }

        private static object ExtractPartData(Part part)
        {
            var data = new
            {
                FileName = part.FullPath,
                Description = GetPartDescription(part),
                //Type = part.IsComponent ? "assembly" : "part", // Verifica se é uma montagem ou peça
                Features = ExtractFeatures(part),
                //Components = ExtractComponents(part) // Se for uma montagem
            };
            return data;
        }
        private static string GetPartDescription(Part part)
        {

            NXObject[] objects1 = new NXObject[1];
            objects1[0] = part;


            string description = part.GetStringAttribute("DB_PART_NAME"); // Exemplo de atributo

            return description;
        }

        private static List<object> ExtractFeatures(Part part)
        {
            var featuresList = new List<object>();
            foreach (NXOpen.Features.Feature feature in part.Features)
            {
                // Aqui você precisará de lógica para extrair parâmetros específicos para cada tipo de feature
                // Isso pode ser feito com um switch case ou if/else if para os tipos de feature mais comuns.
                // Exemplo para Extrude:
                //if (feature is NXOpen.Features.Extrude extrude)
                //{
                //    Expression[] exp = extrude.GetExpressions(); 
                //    featuresList.Add(new
                //    {
                //        Id = feature.Tag.ToString(), // Tag é um identificador único
                //        Type = feature.FeatureType,
                //        Name = feature.Name,

                //        // Exemplo de como acessar parâmetros
                //        //Direction = extrude.Direction.Vector.ToString(),
                //        // ... outros parâmetros relevantes
                //    });
                //}
                //else if (feature is Features.HolePackage hole)
                //{
                //    featuresList.Add(new
                //    {
                //        Id = feature.Tag.ToString(),
                //        Type = feature.FeatureType,
                //        Name = feature.Name,
                //        Diameter = hole.Diameter.Value,
                //        Depth = hole.Depth.Value,
                //        // ...
                //    });
                //}
                //else if (feature is NXOpen.Features.SketchFeature sketch)
                //{
                //    // Extrair dados do esboço é complexo.
                //    // Você precisará iterar sobre as SketchGeometries e SketchConstraints dentro do esboço.
                //    // Para cada linha, arco, círculo, etc., extrair seus pontos, raios, etc.
                //    // E para cada restrição, extrair os elementos que ela restringe.
                //    foreach (SketchCurves cruves in feature)
                //    {

                //    }
                //    featuresList.Add(new
                //    {
                //        Id = feature.Tag.ToString(),
                //        Type = feature.FeatureType,
                //        Name = feature.Name, 


                //        // SketchGeometry e SketchConstraints seriam listas de objetos mais detalhados
                //        //SketchGeometry = Extra(sketch),
                //        //SketchConstraints = ExtractSketchConstraints(sketch)
                //    });
                //}
                //else
                //{
                // Para outros tipos de feature, apenas o tipo e nome
                featuresList.Add(new
                {
                    Type = feature.FeatureType,
                    Name = feature.Name
                });
                //}
            }
            return featuresList;
        }
        private void teste_busca_feature()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            NXOpen.Features.Feature[] feature = workPart.Features.GetFeatures();


            Sketch[] skct = workPart.Sketches.ToArray();


            foreach (NXOpen.Sketch sk in skct)
            {
                NXOpen.Line[] line = sk.GetAllGeometry().OfType<NXOpen.Line>().ToArray();

                foreach (NXOpen.Line ln in line)
                {
                    // Aqui você pode acessar os pontos de início e fim da linha
                    Point3d startPoint = ln.StartPoint;
                    Point3d endPoint = ln.EndPoint;
                    // Exemplo de como exibir as coordenadas

                    MessageBox.Show($"Linha: {ln.GetLength().ToString()}");
                    //MessageBox.Show($"Linha: {ln.Name} - Início: {startPoint.X}, {startPoint.Y}, {startPoint.Z} - Fim: {endPoint.X}, {endPoint.Y}, {endPoint.Z}");
                }

                NXOpen.Arc[] arc = sk.GetAllGeometry().OfType<NXOpen.Arc>().ToArray();

                foreach (NXOpen.Arc ar in arc)
                {
                    // Aqui você pode acessar o centro, raio e ângulos do arco
                    Point3d centerPoint = ar.CenterPoint;
                    double radius = ar.Radius;
                    double startAngle = ar.StartAngle;
                    double endAngle = ar.EndAngle;
                    // Exemplo de como exibir as informações do arco
                    MessageBox.Show($"Arco: {ar.GetLength().ToString()} - Centro: {centerPoint.X}, {centerPoint.Y}, {centerPoint.Z} - Raio: {radius} - Ângulo Inicial: {startAngle} - Ângulo Final: {endAngle}");
                }

                //else if (curve is NXOpen.Arc)
                //{
                //    NXOpen.Arc arc = (NXOpen.Arc)curve;
                //    elementData["tipo"] = "arco";
                //    elementData["parametros"] = new Dictionary<string, double>
                //    {
                //        ["cx"] = arc.CenterPoint.X,
                //        ["cy"] = arc.CenterPoint.Y,
                //        ["raio"] = arc.Radius,
                //        ["angInicial"] = arc.StartAngle,
                //        ["angFinal"] = arc.EndAngle
                //    };
                //}
                //for (int i = 0; i < length; i++)
                //{

                //}
                //MessageBox.Show("Sketch: " + sk.Name + " - " + sk.Tag.ToString());
            }




            //foreach (NXOpen.Features.Feature feat in feature)
            //{


            //    if (feat is NXOpen.Features.SketchFeature)
            //    {

            //            NXOpen.Features.SketchFeature sketchFeature = (NXOpen.Features.SketchFeature)feat;

            //            MessageBox.Show("Sketch Feature: " + sketchFeature.Name + " - " + sketchFeature.Tag.ToString());


            //        //Face[] faces = feat.GetFaces();

            //        //foreach (Face face in faces)
            //        //{
            //        //    NXOpen.Edge[] edgess = face.GetEdges();



            //        //     //Expression[] exp = face.GetExpressions();


            //        //    //MessageBox.Show("Face: " + face.Tag.ToString() + " - " + face.Name);
            //        //    //MessageBox.Show("Face Area: " + face.GetEdges().ToString());
            //        //    //MessageBox.Show("Face Center: " + face.GetEdges().ToString());
            //        //}

            //        //NXOpen.Edge [] edges = feat.GetEdges();

            //        ////foreach (NXOpen.Edge e in edges)
            //        ////{

            //        ////    if (e is NXOpen.Edge)   



            //        ////    MessageBox.Show("Edge: " + e.Tag.ToString() + " - " + e.Name);
            //        ////    MessageBox.Show("Edge: " + e.GetLength().ToString());
            //        ////}

            //        //Expression[] exp = feat.GetExpressions();


            //    }
            //}




        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            string[] list =
            {

"492333-1",
"492951-1",
"492955-1",
"492966-1",
"492977-1",
"494378-1",
"494380-1",
"495323-1",
"495324-1",
"495327-1",
"495328-1",
"495342-1",
"495343-1",
"495345-1",
"495346-1",
"496611-1",
"499224-1",
"499875-1",
"499877-1",
"499881-1",
"499884-1",
"501916-1",
"501924-1",
"501927-1",
"503351-1",
"503385-1",
"503389-1",
"503395-1",
"503398-1",
"503405-1",
"503692-1",
"504239-1",
"504257-1",
"504262-1",
"504267-1",
"504268-1",
"505278-1",
"508282-1",
"510870-1",
"517937-1",
"517940-1",
"524161-1",
"524165-1",
"524168-1",
"528100-1",
"528101-1",
"528934-1",
"537806-1",
"537834-1",
"539444-1",
"539449-1",
"539455-1",
"539808-1",
"539817-1",
"540179-1",
"540180-1",
"540183-1",
"540184-1",
"541202-1",
"541272-1",
"541286-1",
"541291-1",
"543782-1",
"543872-1",
"546725-1",
"546728-1",
"548522-1",
"548747-1",
"548780-1",
"552728-1",
"552739-1",
"552758-1",
"552820-1",
"211316-1",
"211318-1",
"211319-1",
"211320-1",
"211306-1",
"234570-1",
"234571-1",
"234594-1",
"234610-1",
"234994-1",
"234995-1",
"237793-1",
"237794-1",
"240834-1",
"240835-1",
"241570-1",
"241571-1",
"244290-1",
"244291-1",
"251640-1",
"259362-1",
"265069-1",
"271845-1",
"271846-1",
"271847-1",
"272053-1",
"272054-1",
"277240-1",
"277243-1",
"277245-1",
"277993-1",
"277995-1",
"277997-1",
"278000-1",
"278001-1",
"278002-1",
"279812-1",
"279817-1",
"279818-1",
"279821-1",
"281619-1",
"281621-1",
"283515-1",
"283517-1",
"313737-1",
"314572-1",
"314575-1",
"364981-1",
"387250-1",
"387252-1",
"387253-1",
"390334-1",
"390338-1",
"406880-1",
"406882-1",
"406885-1",
"412913-1",
"460190-1",
"471785-1",
"475794-1",
"476704-1",
"429368-1",
"432165-1",
"423584-1",
"235914-1",
"364971-1",
"364977-1",
"364980-1",
"364982-1",
"418691-1",
"418692-1",
"355068-1",
"359877-1",
"391209-1",
"391210-1",
"403299-1",
"404352-1",
"428005-1",
"428006-1",
"537303-1",
"537306-1",
"442764-1",
"456218-1",
"456220-1",
"502981-1",
"505731-1",
"505733-1",
"506404-1",
"506427-1",
"506429-1",
"521548-1",
"521549-1",
"527485-1",
"527488-1",
"536550-1",
"539264-1",
"539923-1",
"540700-1",
"543902-1",
"210061-2",
"210062-2",
"210063-2",
"210065-2",
"212500-2",
"212501-2",
"217325-2",
"217326-2",
"223274-2",
"223670-2",
"223673-2",
"225353-2",
"226264-2",
"226265-2",
"226266-2",
"226267-2",
"226268-2",
"226852-2",
"226855-2",
"249501-2",
"254214-2",
"258199-2",
"258200-2",
"271268-2",
"272519-2",
"273215-2",
"273216-2",
"273217-2",
"278451-2",
"278459-2",
"278460-2",
"282884-2",
"282885-2",
"282886-2",
"282887-2",
"301241-2",
"301243-2",
"302319-2",
"302323-2",
"302785-2",
"302786-2",
"302796-2",
"302797-2",
"303570-2",
"303575-2",
"307192-2",
"307196-2",
"309865-2",
"310340-2",
"310344-2",
"312466-2",
"312467-2",
"315023-2",
"315027-2",
"315035-2",
"315038-2",
"315046-2",
"315049-2",
"315261-2",
"315409-2",
"315412-2",
"315413-2",
"316815-2",
"316816-2",
"316818-2",
"316819-2",
"323761-2",
"323971-2",
"323981-2",
"324691-2",
"324699-2",
"324708-2",
"326231-2",
"326236-2",
"329088-2",
"329095-2",
"329098-2",
"336688-2",
"336692-2",
"336693-2",
"337261-2",
"355069-2",
"359874-2",
"359875-2",
"359881-2",
"359883-2",
"359885-2",
"359886-2",
"359902-2",
"359905-2",
"367016-2",
"367019-2",
"371348-2",
"375879-2",
"382697-2",
"391211-2",
"398160-2",
"399075-2",
"400298-2",
"482362-2",
"525086-2",
"525140-2",
"525142-2",
"525144-2",
"525145-2",
"525147-2",
"537282-2",
"537328-2",
"539319-2",
"539322-2",
"539878-2",
"277239-1",
"277242-1",
"278005-1",
"278007-1",
"279815-1",
"390340-1",
"406889-1",
"423583-1",
"503969-1",
"503970-1",
"503971-1",
"395130-1",
"395133-1",
"395135-1",
"395138-1",
"397499-1",
"457533-1",
"457534-1",
"457535-1",
"545263-1",
"545275-1",
"545289-1",
"545313-1",
"395129-1",
"362169-1",
"512895-1",
"512897-1",
"515570-1",
"522062-1",
"515577-1",
"388176-1",
"471359-1",
"500088-1",
"388871-1",
"424725-1",
"424727-1",
"424729-1",
"424730-1",
"429359-1",
"432173-1",
"438233-1",
"438236-1",
"438881-1",
"438925-1",
"438926-1",
"438949-1",
"438988-1",
"439297-1",
"439298-1",
"438900-1",
"442327-1",
"442776-1",
"442779-1",
"442782-1",
"446602-1",
"446604-1",
"446614-1",
"447733-1",
"447740-1",
"447849-1",
"447860-1",
"450612-1",
"450622-1",
"451261-1",
"457296-1",
"457408-1",
"457412-1",
"470769-1",
"470776-1",
"470821-1",
"470823-1",
"478912-1",
"478914-1",
"488666-1",
"488672-1",
"500189-1",
"501801-1",
"501802-1",
"502712-1",
"502986-1",
"503966-1",
"503968-1",
"503974-1",
"503976-1",
"504130-1",
"504131-1",
"506394-1",
"506396-1",
"506405-1",
"506410-1",
"506417-1",
"506419-1",
"510995-1",
"510997-1",
"510999-1",
"511695-1",
"511699-1",
"512887-1",
"512915-1",
"512920-1",
"512923-1",
"512924-1",
"515574-1",
"516792-1",
"516794-1",
"519579-1",
"524051-1",
"524052-1",
"524053-1",
"524421-1",
"525569-1",
"527475-1",
"527491-1",
"527493-1",
"527476-1",
"528960-1",
"533513-1",
"533517-1",
"533656-1",
"533786-1",
"533795-1",
"534305-1",
"534308-1",
"534360-1",
"534895-1",
"535812-1",
"536485-1",
"536486-1",
"536487-1",
"536560-1",
"540407-1",
"540413-1",
"540417-1",
"540693-1",
"542460-1",
"543075-1",
"543076-1",
"543084-1",
"543086-1",
"544613-1",
"544617-1",
"544622-1",
"544716-1",
"545957-1",
"310345-1",
"312469-1",
"315029-1",
"323760-1",
"323762-1",
"323763-1",
"336690-1",
"336691-1",
"359878-1",
"359884-1",
"359888-1",
"396138-1",
"401853-1",
"422650-1",
"422654-1",
"363322-1",
"363324-1",
"407844-1",
"459119-1",
"459120-1",
"488260-1",
"488262-1",
"488264-1",
"521160-1",
"521161-1",
"523079-1",
"555053-1",
"555069-1",
"555083-1",
"555435-1",
"555449-1",
"553172-1",
"553162-1",
"553533-1",
"555503-1",
"555505-1",
"555507-1",
"555508-1",
"555520-1",
"555798-1",
"555848-1",
"555926-1",
"556085-1",
"556087-1",
"556088-1",
"557133-1",
"557137-1",
"557142-1",
"557145-1",
"557149-1",
"560977-1",
"560980-1",
"560992-1",
"561003-1"};
            foreach (string codigo in list)
            {
                string[] _codigo = codigo.Split('-');
                atualizacao_fam_2312.gerar_pdf_criacao_familia(_codigo[0], _codigo[1].PadLeft(3, '0'));
            }
        }
        public string bt;
        string tag;
        string info_color;
        List<string> _lista_selecionados = new List<string>();
        List<string> _lista_selecionado_pintura = new List<string>();
        private void MenuItem_Click(object sender, EventArgs e)
        {
            _lista_selecionados.Clear();
            _lista_selecionado_pintura.Clear();

            bool color = false;
            if (sender is ToolStripMenuItem clickedItem)
            {
                // Verifica se é um subitem (tem um OwnerItem)
                bool isSubitem = clickedItem.OwnerItem != null;

                if (isSubitem)
                {
                    string parentText = clickedItem.OwnerItem.Text;
                    //MessageBox.Show($"Subitem Clicado\nItem: {parentText}\nSubitem: {clickedItem.Text}");

                    string[] array = parentText.Split('-');
                    string[] array_color = clickedItem.Text.Split('-');
                    bt = array[0].TrimEnd();
                    _lista_selecionados.Add(bt);
                    if (tag != "")
                    {
                        tag = array_color[0].TrimEnd();
                        info_color = array_color[1].TrimEnd();
                        _lista_selecionado_pintura.Add(bt);
                        _lista_selecionado_pintura.Add(tag);
                        _lista_selecionado_pintura.Add(info_color);
                    }
                    color = true;
                }
                else
                {
                    string itemText = clickedItem.Text;
                    string[] array = itemText.Split('-');
                    bt = array[0].TrimEnd();
                    _lista_selecionados.Add(bt);
                    color = false;
                }
            }

            _insercaoConcluida?.TrySetResult(true);

        }
        public void insert_boletin_criacao(List<string> btt, List<string> btp, Part part)
        {
            deletar_tabela_fam("BOLETIM_TECNICO", part);
            deletar_tabela_fam("BOLETIM_PINTURA", part);
            bt = btt[0];
            tag = btp[1].TrimEnd();
            info_color = btp[2].TrimEnd();


            if (btt.Count < 5)
            {
                for (int i = btt.Count; i < 5; i++)
                {
                    btt.Add("-");
                }
            }
            string[] lista_selecionados = btt.ToArray();
            string[] lista_selecionados_pintura = btp.ToArray();

            criar_atributos_fam(lista_selecionados, part);
            Insertb_bt();

            if (btp.Count > 0)
            {
                criar_attibutos_pintura_fam(lista_selecionados_pintura, part);
                Insertb_bp();
            }


        }
        public void insert_boletin_criacao_demanda(List<string> btt, List<string> btp, Part part)
        {
            deletar_tabela_fam("BOLETIM_TECNICO", part);
            deletar_tabela_fam("BOLETIM_PINTURA", part);
            //bt = btt[0];
            //tag = btp[1].TrimEnd();
            //info_color = btp[2].TrimEnd();


            //if (btt.Count < 5)
            //{
            //    for (int i = btt.Count; i < 5; i++)
            //    {
            //        btt.Add("-");
            //    }
            //}
            //string[] lista_selecionados = btt.ToArray();
            //string[] lista_selecionados_pintura = btp.ToArray();

            //criar_atributos_fam(lista_selecionados, part);
            Insertb_bt();

            //if (btp.Count > 0)
            //{
            //    criar_attibutos_pintura_fam(lista_selecionados_pintura, part);
                Insertb_bp();
           // }


        }

        public void deletar_tabelas(Part part)
        {
            deletar_tabela_fam("BOLETIM_TECNICO", part);
            deletar_tabela_fam("BOLETIM_PINTURA", part);
        }

        public void registro_boletim_fam(List<string> btt, List<string> btp, string codigo, string rev)
        {

            bt = btt[0];
            tag = btp[1].TrimEnd();
            info_color = btp[2].TrimEnd();



            _lista_boletins = new DataSet();
            _lista_boletins_cores = new DataSet();
            conexao_bd db = new conexao_bd();
            _lista_boletins = db.busca_boletim();
            _lista_boletins_cores = db.lista_bol_cores();

            string id_bt = "";

            foreach (DataRow row1 in _lista_boletins.Tables[0].Rows)
            {
                if (row1["boletim"].ToString().TrimEnd() == bt.TrimEnd())
                {
                    id_bt = row1["id"].ToString();
                }
            }
            string id_bt_pintura = "";
            foreach (DataRow row1 in _lista_boletins_cores.Tables[0].Rows)
            {
                if (row1["tag"].ToString().TrimEnd() == tag)
                {
                    id_bt_pintura = row1["id"].ToString();
                }
            }

            // MessageBox.Show(id_bt + "  " + id_bt_pintura);
            if (id_bt_pintura == "")
            {
                db.insert_boletim(codigo, rev, id_bt, null);
            }
            else
            {
                db.insert_boletim(codigo, rev, id_bt, id_bt_pintura);
            }
        }
        DataSet _lista_boletins;
        DataSet _lista_boletins_cores;

        private void button14_Click(object sender, EventArgs e)
        {
            lista_boletins.Items.Clear();

            string boletim = conexao_bd.verifica_boletim_fam(cmb_familia_pc.Text.Substring(0, 6));

            //  MessageBox.Show(boletim);
            if (boletim != "")
            {
                _lista_boletins = new DataSet();
                _lista_boletins_cores = new DataSet();
                string[] array_bol = JsonConvert.DeserializeObject<string[]>(boletim);
                conexao_bd db = new conexao_bd();
                _lista_boletins = db.busca_boletim();
                _lista_boletins_cores = db.lista_bol_cores();

                foreach (DataRow row1 in _lista_boletins.Tables[0].Rows)
                {
                    if (array_bol.Contains(row1["id"].ToString()))
                    {
                        ToolStripMenuItem item1 = new ToolStripMenuItem(row1["boletim"].ToString() + " - " + row1["descricao"].ToString());

                        foreach (DataRow row in _lista_boletins_cores.Tables[0].Rows)
                        {
                            if (row1["id"].ToString() == row["tipo"].ToString())
                            {
                                ToolStripMenuItem subItem = new ToolStripMenuItem(row["tag"].ToString() + " - " + row["info_color"].ToString());
                                subItem.Click += MenuItem_Click;
                                item1.DropDownItems.Add(subItem);

                            }

                            // ToolStripMenuItem subItem1 = new ToolStripMenuItem("Subitem 1.1");

                            //item1.DropDownItems.Add(subItem1);
                        }
                        lista_boletins.Items.Add(item1);
                        item1.Click += MenuItem_Click;
                    }
                }
                lista_boletins.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }
        private TaskCompletionSource<bool> _insercaoConcluida;
        private async Task<(List<string> listaSelecionados, List<string> listaPintura)> insert_boletin_fam(string boletim)
        {



            _insercaoConcluida = new TaskCompletionSource<bool>();
            var listaSelecionados = new List<string>();
            var listaPintura = new List<string>();
            if (boletim != "")
            {
                _insercaoConcluida = new TaskCompletionSource<bool>();
                _lista_boletins = new DataSet();
                _lista_boletins_cores = new DataSet();
                string[] array_bol = JsonConvert.DeserializeObject<string[]>(boletim);
                conexao_bd db = new conexao_bd();
                _lista_boletins = db.busca_boletim();
                _lista_boletins_cores = db.lista_bol_cores();

                foreach (DataRow row1 in _lista_boletins.Tables[0].Rows)
                {
                    if (array_bol.Contains(row1["id"].ToString()))
                    {
                        ToolStripMenuItem item1 = new ToolStripMenuItem(row1["boletim"].ToString() + " - " + row1["descricao"].ToString());

                        foreach (DataRow row in _lista_boletins_cores.Tables[0].Rows)
                        {
                            if (row1["id"].ToString() == row["tipo"].ToString())
                            {
                                ToolStripMenuItem subItem = new ToolStripMenuItem(row["tag"].ToString() + " - " + row["info_color"].ToString());
                                subItem.Click += MenuItem_Click;
                                item1.DropDownItems.Add(subItem);

                            }
                        }
                        lista_boletins.Items.Add(item1);
                        item1.Click += MenuItem_Click;
                    }
                }
                lista_boletins.Show(Cursor.Position.X, Cursor.Position.Y);

            }

            await _insercaoConcluida.Task;

            listaSelecionados = new List<string>(_lista_selecionados);
            listaPintura = new List<string>(_lista_selecionado_pintura);

            return (listaSelecionados, listaPintura);
        }

        private (List<string> listaSelecionados, List<string> listaPintura) insert_boletin_fam_sincrona(string boletim)
        {

            var listaSelecionados = new List<string>();
            var listaPintura = new List<string>();
            if (boletim != "")
            {

                _lista_boletins = new DataSet();
                _lista_boletins_cores = new DataSet();
                string[] array_bol = JsonConvert.DeserializeObject<string[]>(boletim);
                conexao_bd db = new conexao_bd();
                _lista_boletins = db.busca_boletim();
                _lista_boletins_cores = db.lista_bol_cores();

                bool color = false;

                Form selectionForm = new Form();
                selectionForm.Text = "Selecione um Boletim de Pintura";
                selectionForm.Size = new Size(400, 300);
                selectionForm.Icon = Properties.Resources.Mascarello;
                selectionForm.StartPosition = FormStartPosition.CenterScreen;
                TreeView treeView = new TreeView();
                treeView.Dock = DockStyle.Fill;

                treeView.Font = new Font("Arial", 10, FontStyle.Regular);
                treeView.BackColor = Color.White;

                // Adiciona os nós (pai e filhos) no TreeView
                foreach (DataRow row1 in _lista_boletins.Tables[0].Rows)
                {
                    if (array_bol.Contains(row1["id"].ToString()))
                    {
                        // Nó pai com informações do boletim
                        TreeNode parentNode = new TreeNode($"{row1["boletim"]} - {row1["descricao"]}");
                        foreach (DataRow row in _lista_boletins_cores.Tables[0].Rows)
                        {
                            if (row1["id"].ToString() == row["tipo"].ToString())
                            {

                                TreeNode childNode = new TreeNode($"{row["tag"]} - {row["info_color"]}");
                                parentNode.Nodes.Add(childNode);
                            }
                        }
                        treeView.Nodes.Add(parentNode);
                    }
                }

                selectionForm.Controls.Add(treeView);
                treeView.ExpandAll();

                _lista_selecionados.Clear();
                _lista_selecionado_pintura.Clear();
                // Tratamento para evento de duplo clique (seleção)
                treeView.NodeMouseDoubleClick += (sender, e) =>
                {
                    // Se o nó for filho, registra o pai e o filho na seleção
                    if (e.Node.Parent != null)
                    {

                        string[] array = e.Node.Parent.Text.Split('-');
                        string[] array_color = e.Node.Text.Split('-');
                        bt = array[0].TrimEnd();
                        _lista_selecionados.Add(bt);
                        if (tag != "")
                        {
                            tag = array_color[0].TrimEnd();
                            info_color = array_color[1].TrimEnd();
                            _lista_selecionado_pintura.Add(bt);
                            _lista_selecionado_pintura.Add(tag);
                            _lista_selecionado_pintura.Add(info_color);
                        }
                        selectionForm.DialogResult = DialogResult.OK;
                        selectionForm.Close();
                        color = true;
                    }

                };
                selectionForm.ShowDialog();
            }


            listaSelecionados = new List<string>(_lista_selecionados);
            listaPintura = new List<string>(_lista_selecionado_pintura);

            return (listaSelecionados, listaPintura);
        }
        private (List<string> listaSelecionados, List<string> listaPintura) insert_boletin_fam_sincrona_auto(string bt, string btp, string rev)
        {

            var listaSelecionados = new List<string>();
            var listaPintura = new List<string>();
            conexao_bd db = new conexao_bd();

            DataSet _lista_boletins = db.busca_boletim_auto(bt, btp);
            foreach (DataRow row1 in _lista_boletins.Tables[0].Rows)
            {
                _lista_selecionados.Add(row1["boletim"].ToString());
                _lista_selecionado_pintura.Add(row1["boletim"].ToString());
                _lista_selecionado_pintura.Add(row1["tag"].ToString());
                _lista_selecionado_pintura.Add(row1["info_color"].ToString());
            }
            listaSelecionados = new List<string>(_lista_selecionados);
            listaPintura = new List<string>(_lista_selecionado_pintura);

            return (listaSelecionados, listaPintura);
        }

        private void ckd_fam_tb_cod_CheckedChanged(object sender, EventArgs e)
        {

        }


        public static void SelectCurves_1(ref NXObject[] selectedObjects)
        {

            UI ui = NXOpen.UI.GetUI();

            string message = "Selecione as duas faces de medir o vão";
            string title = "Selecione";

            Selection.SelectionScope scope = Selection.SelectionScope.WorkPartAndWorkPartOccurrence;
            bool keepHighlighted = false;
            bool includeFeatures = false;
            Selection.Response response = default(Selection.Response);

            Selection.SelectionAction selectionAction = Selection.SelectionAction.ClearAndEnableSpecific;

            Selection.MaskTriple[] selectionMask_array = new Selection.MaskTriple[100];
            {
                selectionMask_array[0].Type = UFConstants.UF_face_type;
                //selectionMask_array[0].Subtype = UFConstants.uf_face;
                //selectionMask_array[0].SolidBodySubtype = UFConstants.UF_solid_type;
            }



            response = ui.SelectionManager.SelectObjects(message, title, scope, selectionAction, includeFeatures, keepHighlighted, selectionMask_array, out selectedObjects);

            if (response == Selection.Response.Cancel | response == Selection.Response.Back)
            {
                return;
            }
        }

        private Task _processamentoTask;
        private CancellationTokenSource _cts;
        // Helper para gerenciar o estado dos botões


        private TcpListener tcpListener;
        private bool isListening = false;
        private CancellationTokenSource cancellationTokenSource;
        private readonly ConcurrentQueue<ClientRequest> clientQueue = new ConcurrentQueue<ClientRequest>();
        private readonly SemaphoreSlim processingSemaphore = new SemaphoreSlim(1, 1);


        private async void btnStartServer_Click(object sender, EventArgs e)
        {
            try
            {
                txt_ip.Text = IPAddress.Any.ToString();
                tcpListener = new TcpListener(IPAddress.Any, Convert.ToInt32(txtPort.Text));
                tcpListener.Start();
                isListening = true;
                cancellationTokenSource = new CancellationTokenSource();

                UpdateStatus("Servidor iniciado na porta 8080", true);
                LogMessage($"Servidor iniciado na porta: {txtPort.Text}");

                // Inicia o processamento da fila
                _ = Task.Run(() => ProcessClientQueue(cancellationTokenSource.Token));

                // Aceita conexões de clientes
                _ = Task.Run(() => AcceptClients(cancellationTokenSource.Token));



            }
            catch (Exception ex)
            {
                LogMessage($"Erro ao iniciar servidor: {ex.Message}");
            }
        }
        private void btnStopServer_Click(object sender, EventArgs e)
        {
            try
            {
                isListening = false;
                cancellationTokenSource?.Cancel();
                tcpListener?.Stop();
                UpdateStatus("Servidor parado", false);
            }
            catch (Exception ex)
            {
                LogMessage($"Erro ao parar servidor: {ex.Message}");
            }
        }

        private async Task AcceptClients(CancellationToken cancellationToken)
        {
            while (isListening && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var tcpClient = await tcpListener.AcceptTcpClientAsync();
                    LogMessage($"Cliente conectado: {tcpClient.Client.RemoteEndPoint}");

                    // Adiciona o cliente na fila
                    var clientRequest = new ClientRequest
                    {
                        TcpClient = tcpClient,
                        RequestTime = DateTime.Now
                    };

                    clientQueue.Enqueue(clientRequest);
                    UpdateQueueStats();
                }
                catch (ObjectDisposedException)
                {
                    // Listener foi fechado
                    break;
                }
                catch (Exception ex)
                {
                    LogMessage($"Erro ao aceitar cliente: {ex.Message}");
                }
            }
        }
        private async Task ProcessClientQueue(CancellationToken cancellationToken)
        {
            int processedCount = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                if (clientQueue.TryDequeue(out ClientRequest clientRequest))
                {
                    await processingSemaphore.WaitAsync(cancellationToken);

                    try
                    {
                        await ProcessClient(clientRequest.TcpClient);
                        processedCount++;
                        UpdateProcessedStats(processedCount);
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"Erro ao processar cliente: {ex.Message}");
                    }
                    finally
                    {
                        processingSemaphore.Release();
                        UpdateQueueStats();
                    }
                }
                else
                {
                    await Task.Delay(100, cancellationToken); // Aguarda novos clientes
                }
            }
        }

        private async Task ProcessClient(TcpClient client)
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();

                // Lê os dados do cliente
                var buffer = new byte[4096];
                var data = new StringBuilder();

                do
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;
                    data.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                } while (stream.DataAvailable);

                var receivedData = data.ToString();
                LogMessage($"Dados recebidos: {receivedData}");
                Dictionary<string, double> lista = new Dictionary<string, double>();

                // Deserializa a lista recebida
                var inputList = System.Text.Json.JsonSerializer.Deserialize<List<string>>(receivedData);

                // Processa a lista (exemplo: converte para maiúsculo e adiciona prefixo)
                var processedList = ProcessList(inputList);
                int acao = 1;
                string user_create = "";
                // string[] lista_selecionados = processedList.ToString().Split(';');
                foreach (string item in processedList)
                {
                    string[] lista_selecionados = item.Split(';');
                    if (lista_selecionados.Length > 1)
                    {
                        foreach (string item1 in lista_selecionados)
                        {
                            string[] lista_dados = item1.Split('=');

                            LogMessage($"Attr: {lista_dados[0]} value: {lista_dados[1]}");
                            if (lista_dados[0] == "ACAO" && Convert.ToDouble(lista_dados[1]) == 0)
                            {
                                acao = 0;
                            }
                            //else
                            //{
                            //    acao = 1;

                            //}
                            if (lista_dados[0] != "USER")
                            {
                                lista.Add(lista_dados[0], Convert.ToDouble(lista_dados[1]));
                            }
                            else
                            {
                                user_create = lista_dados[1];
                            }

                        }
                    }

                }
                LogMessage($"Ação =={acao.ToString()}");
                if (acao == 1)
                {
                    string codigo = "";
                    this.Invoke((Action)(() =>
                    {
                        codigo = Create_fam_geral_Sincrona_AutoCAD(lista, user_create);
                    }));

                    // Serializa a resposta
                    var response = System.Text.Json.JsonSerializer.Serialize(codigo);
                    var responseBytes = Encoding.UTF8.GetBytes(response);

                    // Envia a resposta
                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    await stream.FlushAsync();

                    LogMessage($"Item  {codigo}");
                }
                else if (acao == 0)
                {
                    DataSet dataSet = new DataSet();
                    this.Invoke((Action)(() =>
                    {
                        dataSet = carregar_dados_fam_teamcenter_autocad(lista);
                    }));
                    LogMessage($"Busca_Concluida.");
                    dataSet.Tables[0].Columns.Remove("DB_PART_NAME");
                    dataSet.Tables[0].Columns.Remove("DB_PART_TYPE");


                    if (dataSet != null && dataSet.Tables.Count > 0)
                    {
                        //foreach (DataRow item in dataSet.Tables[0].Rows)
                        //{

                        //}
                        var response = JsonConvert.SerializeObject(dataSet);

                        // salvar o arquivo json
                        //string filePath = Path.Combine(@"C:\temp", "dados.json");
                        //File.WriteAllText(filePath, response);



                        // LogMessage($"Dados retornados: {response}");

                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        LogMessage($" Retorno T {responseBytes.Length} bytes");
                        // E então para um array de caracteres, se necessário

                        // Envia a resposta
                        await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                        await stream.FlushAsync();
                    }

                    else
                    {
                        LogMessage("DataSet retornado está vazio ou não contém tabelas.");
                        // Opcional: Enviar uma resposta vazia ou de erro
                        var emptyResponse = "[]"; // JSON para um array vazio
                        var responseBytes = Encoding.UTF8.GetBytes(emptyResponse);
                        await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                        await stream.FlushAsync();
                    }
                    //var response = System.Text.Json.JsonSerializer.Serialize(dataSet);
                    //LogMessage(response);
                    //var responseBytes = Encoding.UTF8.GetBytes(response);

                    //// Envia a resposta
                    //await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    //await stream.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Erro ao processar cliente: {ex.Message}");
            }
            finally
            {
                stream?.Close();
                client?.Close();
            }
        }
        public List<Dictionary<string, object>> ConvertDataTableToList(DataTable dt)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in dt.Rows)
            {
                // Cria um dicionário para representar a linha atual
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in dt.Columns)
                {
                    // Adiciona a coluna (chave) e o valor da célula (valor) ao dicionário
                    // DBNull.Value não é serializável para JSON, então o convertemos para null
                    dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                }
                list.Add(dict);
            }

            return list;
        }
        private List<string> ProcessList(List<string> inputList)
        {
            // Exemplo de processamento: converte para maiúsculo e adiciona timestamp
            var processedList = new List<string>();
            var timestamp = DateTime.Now.ToString("HH:mm:ss");

            foreach (var item in inputList)
            {
                processedList.Add($"{item.ToUpper()}");
            }

            // Adiciona um item de confirmação
            processedList.Add($"Processado em {timestamp} - Total de itens: {inputList.Count}");

            return processedList;
        }

        private void UpdateStatus(string message, bool isRunning)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateStatus(message, isRunning)));
                return;
            }

            var lblStatus = Controls.Find("lblStatus", true).FirstOrDefault() as Label;
            var btnIniciar = Controls.Find("btnIniciar", true).FirstOrDefault() as Button;
            var btnParar = Controls.Find("btnParar", true).FirstOrDefault() as Button;

            if (lblStatus != null) lblStatus.Text = $"Status: {message}";
            if (btnIniciar != null) btnIniciar.Enabled = !isRunning;
            if (btnParar != null) btnParar.Enabled = isRunning;
        }

        private void UpdateQueueStats()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateQueueStats));
                return;
            }

            var lblStats = Controls.Find("lblStats", true).FirstOrDefault() as Label;
            if (lblStats != null)
            {
                lblStats.Text = $"Clientes na fila: {clientQueue.Count}";
            }
        }

        private void UpdateProcessedStats(int processedCount)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateProcessedStats(processedCount)));
                return;
            }

            var lblStats = Controls.Find("lblStats", true).FirstOrDefault() as Label;
            if (lblStats != null)
            {
                lblStats.Text = $"Clientes na fila: {clientQueue.Count} | Processados: {processedCount}";
            }
        }

        private void LogMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => LogMessage(message)));
                return;
            }

            var txtLogs = Controls.Find("txt_log_server", true).FirstOrDefault() as TextBox;
            if (txtLogs != null)
            {
                txtLogs.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
                txtLogs.ScrollToCaret();
            }
        }
        private void LogMessage_fam(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => LogMessage_fam(message)));
                return;
            }

            var txtLogs = Controls.Find("txt_logfam", true).FirstOrDefault() as TextBox;
            if (txtLogs != null)
            {
                txtLogs.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
                txtLogs.ScrollToCaret();
            }
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isListening)
            {
                btnStopServer_Click(null, null);
            }
            base.OnFormClosing(e);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            LogMessage($"Total de Itens {txt_list.Lines.Length}");
            int cont = txt_list.Lines.Length;

            foreach (string item in txt_list.Lines)
            {
                Dictionary<string, double> lista = new Dictionary<string, double>();
                string codigo = "";
                string[] lista_selecionados = item.Split(';');
                if (lista_selecionados.Length > 1)
                {
                    foreach (string item1 in lista_selecionados)
                    {
                        string[] lista_dados = item1.Split('=');

                        LogMessage($"Attr: {lista_dados[0]} value: {lista_dados[1]}");
                        lista.Add(lista_dados[0], Convert.ToDouble(lista_dados[1]));
                    }
                }
                cont--;
                Create_fam_geral_Sincrona_AutoCAD_create(lista);
                LogMessage($"Itens restantes: {cont}");
            }

            //PartLoadStatus LdSt;
            //Part obj_familia;
            //Part obj_atual;
            //try
            //{
            //    obj_familia = (Part)theSession.Parts.FindObject(Familia);
            //}
            //catch
            //{
            //    obj_familia = theSession.Parts.Open(Familia, out LdSt);
            //}
            //obj_familia.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
        }

        private void txt_list_TextChanged(object sender, EventArgs e)
        {

        }
        public void cadastrar_atributos(string codigorev, string familia)
        {
            string[] attr = conexao_bd.dados_atributos(familia).Split(';');

            string ge = attr[0];
            string fam = attr[1];
            string linha = attr[2];
            string deposito = attr[3];
            string tr = attr[4];
            string forma = attr[5];
            conexao_db_TeamCenter conn = new conexao_db_TeamCenter();

            string puid = conn.busca_puid_form(codigorev);

            LogMessage($"Codigo: {codigorev} Puid: {puid}");
            if (puid != "")
            {
                LogMessage($"ge: {ge} fam: {fam} linha: {linha} deposito: {deposito} tr: {tr} forma: {forma}");
                conn.update_attr_fam(puid, ge, fam, linha, deposito, tr, forma);
                LogMessage($"Atributos atualizados para o Puid: {puid} Codigo{codigorev}");

            }

        }
        public void cadastrar_atributos_ge_fam(string codigorev, string familia)
        {
            string[] attr = conexao_bd.dados_atributos_1(familia).Split(';');

            string ge = attr[0];
            string fam = attr[1];

            conexao_db_TeamCenter conn = new conexao_db_TeamCenter();

            string puid = conn.busca_puid_form(codigorev);


            if (puid != "")
            {
                conn.update_attr_ge_fam(puid, ge, fam);
            }
        }
        private void button16_Click(object sender, EventArgs e)
        {
            foreach (string item in lista_atualizar.Lines)
            {

                atualizacao_fam_2312.gerar_pdf_criacao_familia(item.Substring(0, 6), "001");
            }

            //UFSession ufSession = null;
            ////try
            ////{
            //    ufSession = UFSession.GetUFSession();

            //    // Set login arguments
            //    string[] loginArgs = new string[] { "-pim=yes", "-u=renato.ferreira", "-p=150897" };

            //    // Initialize Ugmgr for login
            //    ufSession.Ugmgr.Initialize(loginArgs.Length, loginArgs);

            //    // Check if connected
            //    bool isConnected;
            //    ufSession.UF.IsUgmanagerActive(out isConnected);

            //    if (isConnected)
            //    {
            //        // Connection successful, proceed with Teamcenter operations
            //        // For example: ufSession.Ugmgr.OpenPartInTeamcenter(...)
            //        LogMessage("Successfully connected to Teamcenter.");
            //    }
            //    else
            //    {
            //        LogMessage("Failed to connect to Teamcenter.");
            //    }
            ////}
            ////catch (NXException ex)
            ////{
            ////    System.Console.WriteLine("NXOpen Error: " + ex.Message);
            ////}
            ////finally
            ////{
            ////    if (ufSession != null)
            ////    {
            ////        // Optionally terminate the session when done
            ////        // ufSession.Ugmgr.Terminate();
            ////    }
            ////}


        }

        private void button18_Click(object sender, EventArgs e)
        {
            //foreach (string item in lista_atualizar.Lines)
            //{
            //    string[] array = item.Split(';');
            //    string codigorev = array[0];

            //    string ge = array[1];
            //    string fam = array[2];
            //    string linha = array[3];
            //    string deposito = array[4];
            //    string tr = array[5];
            //    if (codigorev != "")
            //    {
            //        try
            //        {
            //            conexao_db_TeamCenter conn = new conexao_db_TeamCenter();
            //            string puid = conn.busca_puid_form(codigorev);
            //            LogMessage($"Codigo: {codigorev} Puid: {puid}");
            //            if (puid != "")
            //            {
            //                LogMessage($" linha: {linha} deposito: {deposito} tr: {tr}");
            //                conn.update_attr_fam_update(puid, ge, fam, linha, deposito, tr);
            //                LogMessage($"Atributos atualizados para o Puid: {puid} Codigo{codigorev}");

            //            }
            //            LogMessage($"Atualizando atributos para o codigo: {codigorev} linha: {linha} deposito: {deposito} tr: {tr}");
            //        }
            //        catch
            //        {


            //        }
            //    }
            //}

            atualizar_obtencao();
        }

        private void atualizar_obtencao()
        {
            foreach (string item in lista_atualizar.Lines)
            {
                string[] array = item.Split(';');
                string codigorev = array[0];

                string ge = array[1];
                string fam = array[2];
                string linha = array[3];
                string deposito = array[4];
                string tr = array[5];
                string obtencao = "FABRICADO";
                if (codigorev != "")
                {
                    try
                    {
                        conexao_db_TeamCenter conn = new conexao_db_TeamCenter();
                        string puid = conn.busca_puid_form(codigorev);
                        LogMessage($"Codigo: {codigorev} Puid: {puid}");
                        if (puid != "")
                        {
                            LogMessage($" linha: {linha} deposito: {deposito} tr: {tr}");
                            conn.update_attr_fam(puid, ge, fam, linha, deposito, tr, obtencao);
                            LogMessage($"Atributos atualizados para o Puid: {puid} Codigo{codigorev}");

                        }
                        LogMessage($"Atualizando atributos para o codigo: {codigorev} linha: {linha} deposito: {deposito} tr: {tr} obtencao: {obtencao}");
                    }
                    catch
                    {


                    }
                }
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {

        }

        private void txt_logfam_Enter(object sender, EventArgs e)
        {

        }
        public static string GetEncondedPartName_REV(string item_id)
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

            return revision_id;
        }
        public static string TCSearchItem_REV(string item_id)
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

                encodedname = GetEncondedPartName_REV(results[0]);

                Registrar_no_LOG(" DEBUG -> (TCSearchItem) encodedname:" + encodedname);

                return encodedname;

            }

            else return "";
        }

        private void button20_Click(object sender, EventArgs e)
        {
            frm_Configurador_2025 abrir = new frm_Configurador_2025();
            abrir.Show();
            this.Close();
        }
      
        private void button21_Click(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();



            //Session theSession = Session.GetSession();
            //UFSession theUfSession = UFSession.GetUFSession();

            //theUfSession.Clone.Terminate();

            //theUfSession.Clone.Initialise(UFClone.OperationClass.CloneOperation);


            //string currentFolder;
            //theUfSession.Clone.AskDefDirectory(out currentFolder);
            //MessageBox.Show("Current Folder: " + currentFolder);

            //  Tag families;
            //Tag rootFolderTag;
            //theUFSession.Ugmgr.AskRootFolder(out rootFolderTag);

            //Tag[] rootFolderTags;

            //int cont;
            //theUFSession.Ugmgr.ListFolderContents(rootFolderTag,out cont, out rootFolderTags);


            //theUFSession.Ugmgr.SetDefaultFolder(rootFolderTag);



            //            foreach ( Tag tag in rootFolderTags )
            //{
            //    theUFSession.Ugmgr.AskFolderName(tag, out string folderName);
            //    txt_entrada_dados.AppendText(folderName);

            //}


            //MessageBox.Show("Root Folder Tag: " + rootFolderTag.ToString());
            //NXOpen.UF.UFUgmgr.(workPart.Tag, 1, NXOpen.Tag[])
            //theUfSession.UF..ListFolderContents
            // ----------------------------------------------
            //   Menu: File->Save As...
            // ----------------------------------------------
            //NXOpen.PDM.PartOperationCopyBuilder partOperationCopyBuilder1;
            //partOperationCopyBuilder1 = theSession.PdmSession.CreateCopyOperationBuilder(NXOpen.PDM.PartOperationBuilder.OperationType.SaveAs);

            //partOperationCopyBuilder1.SetOperationSubType(NXOpen.PDM.PartOperationCopyBuilder.OperationSubType.Default);

            //partOperationCopyBuilder1.DefaultDestinationFolder = ":Newstuff";

            //partOperationCopyBuilder1.Destroy();

        }

        private void button22_Click(object sender, EventArgs e)
        {
            string[] demanda = {
           "431184/002",
"413551/003",
"350270/002",
"401073/001",
"232930/002",
"232931/002",
"355256/002",
"355257/002",
"355977/001",
"281907/002",
"347038/003",
"281908/002",
"401075/001",
"380296/001",
"380298/001",
"395219/001",
"395220/001",
"356378/004",
"442358/001",
"426370/001",
"378242/002",
"433893/002",
"315294/002",
"406216/003",
"369565/002",
"369582/002",
"393940/001",
"402900/001",
"381047/002",
"381048/002",
"381050/004",
"400008/002",
"400014/002",
"382148/001",
"382149/001",
"418613/001",
"418615/002",
"301825/001",
"441968/001",
"393827/003",
"229082/002",
"442037/002",
"382854/002",
"381245/001",
"232510/004",
"232511/004",
"457920/001",
"391982/001",
"391992/001",
"442061/001",
"442116/001",
"262877/002",
"346017/001",
"352144/001",
"347232/001",
"323976/001",
"323987/001",
"228673/002",
"405469/001",
"437780/002",
"222577/002",
"229088/002",
"229822/002",
"380484/001",
"372558/001",
"372562/001",
"372563/001",
"364895/001",
"226579/002",
"226576/004",
"228642/002",
"228643/002",
"372560/001",
"363924/001",
"364059/001",
"364060/001",
"364902/003",
"368207/001",
"368208/001",
"368361/001",
"364258/001",
"364410/001",
"381056/001",
"382141/001",
"364261/001",
"222582/002",
"244384/002",
"227624/004",
"222578/002",
"364162/002",
"363922/002",
"389810/001",
"385338/001",
"385631/001",
"291754/001",
"295076/003",
"316793/001",
"291755/002",
"421367/001",
"291757/001",
"294179/001",
"295238/001",
"407807/002",
"293794/001",
"293795/002",
"347244/001",
"361885/003",
"361891/003",
"352160/001",
"357747/001",
"346925/001",
"358940/001",
"450618/001",
"358818/002",
"378628/001",
"232259/003",
"407804/002",
"381126/001",
"391281/001",
"514197/001",
"514199/001",
"371465/002",
"442059/001",
"355020/003",
"355021/005",
"370735/001",
"371161/001",
"371163/002",
"370739/001",
"370740/001",
"280051/002",
"280079/001",
"263639/001",
"343930/001",
"343908/001",
"343949/002",
"422863/001",
"422864/001",
"315337/003",
"313776/001",
"314390/001",
"315327/002",
"312017/002",
"480189/001",
"335959/001",
"356802/001",
"226078/003",
"335961/001",
"263116/003",
"356800/001",
"336945/001",
"336961/001",
"439431/002",
"346961/001",
"472973/002",
"473905/003",
"473901/003",
"325158/001",
"291222/003",
"444182/002",
"357152/001",
"343946/002",
"323811/001",
"293357/002",
"393778/001",
"458348/002",
"458561/001",
"393777/001",
"411627/001",
"349213/001",
"346018/002",
"312611/001",
"312612/002",
"255874/002",
"312613/002",
"288201/002",
"386324/001",
"335955/001",
"335951/001",
"269447/003",
"269524/002",
"455618/001",
"441417/002",
"376086/001",
"481540/002",
"481552/003",
"455617/002",
"226381/006",
"236891/002",
"291226/003",
"318374/003",
"327632/002",
"336585/002",
"350757/001",
"392900/001",
"451432/001",
"451438/001",
"458082/001",
"504149/001"
            };
            foreach (string item in demanda)
            {
                string[] quebra2 = item.Split('/');
                int rev = Convert.ToInt32(quebra2[1]);

                Open_drawing_demanda(quebra2[0], rev.ToString().PadLeft(3, '0'));



                deletar_alteracao();

                NXOpen.Session theSession = NXOpen.Session.GetSession();
                NXOpen.Part workPart = theSession.Parts.Work;
                NXOpen.Part displayPart = theSession.Parts.Display;

                Insertb_alt(workPart);

                Save_Update();

                theSession.Parts.CloseAll(BasePart.CloseModified.CloseModified, null);
            }
        }

        private void ckd_2023_CheckedChanged(object sender, EventArgs e)
        {
            if (ckd_2023.Checked)
            {
                ckd_antigo.Checked = false;
            }
            else
            {
                ckd_antigo.Checked = true;

            }
        }
        public class Modelo
        {
            // O atributo mapeia a propriedade "itens" do JSON para a propriedade "Itens" do C#
            [JsonPropertyName("itens")] // para System.Text.Json
                                        // [JsonProperty("itens")]    // para Newtonsoft.Json
            public List<Item_Json_fam> Itens { get; set; }
        }
        public class Item_Json_fam
        {
            [JsonPropertyName("item")]
            // [JsonProperty("item")]
            public string Item { get; set; }

            [JsonPropertyName("atributos")]
            // [JsonProperty("codigo")]
            public List<string> Atributos { get; set; }
            [JsonPropertyName("valor")]
            // [JsonProperty("codigo")]
            public List<string> Valor { get; set; }
            [JsonPropertyName("ajuste")]
            // [JsonProperty("codigo")]
            public List<double> Ajuste { get; set; }

        }

        public class Atributos
        {
            // O atributo mapeia a propriedade "itens" do JSON para a propriedade "Itens" do C#
            [JsonPropertyName("lista_atributos")] // para System.Text.Json
                                        // [JsonProperty("itens")]    // para Newtonsoft.Json
            public List<Item_Json_Atributos> Lista_atributos { get; set; }
        }
        public class Item_Json_Atributos
        {
            [JsonPropertyName("atributo")]
            // [JsonProperty("item")]
            public string Atributo { get; set; }

            [JsonPropertyName("valor")]
            // [JsonProperty("codigo")]
            public List<string> Valor { get; set; }

        }
        
        public class Atributos_Auto
        {
            [JsonProperty("lista_att_auto")]
            public List<Item_Json_Atributos_Auto> Lista_Atributos_Auto { get; set; }
        }

        public class Item_Json_Atributos_Auto
        {
            [JsonProperty("atributo")]
            public string Atributo { get; set; }

            [JsonProperty("atributobase")]  // Exato do JSON (minúscula 'b')
            public List<string> AtributoBase { get; set; }

            [JsonProperty("valor")]
            public List<string> Valor { get; set; }
        }

        private void ckd_antigo_CheckedChanged(object sender, EventArgs e)
        {
            if (!ckd_antigo.Checked)
            {
                ckd_2023.Checked = true;
            }
            else
            {
                ckd_2023.Checked = false;
            }
        }

        private void tab_admin_Click(object sender, EventArgs e)
        {

        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btn_executar_Click(object sender, EventArgs e)
        {
            foreach(string cod_fam in txt_entrada_dados.Lines)
            {
                try
                {
                    string[] att = carregar_dados(cod_fam.PadLeft(6, '0'));
                    txt_saida_dados.AppendText(cod_fam + ";" + string.Join(";", att) + "\n");
                }
                catch (Exception)
                {

                    txt_saida_dados.AppendText(cod_fam + "\n");
                }
                

                 
            }
        }

        private string[] carregar_dados(string fam)
        {
            //limites = "";
            lista_atributos.Clear();
            //DataFamily.Clear();
            //dtg_resultado.DataSource = null;
            //dtg_resultado.Rows.Clear();
            //dtg_resultado.Columns.Clear();
            //string cod = cmb_familia_pc.Text.Substring(0, 6);

            //if (cmb_tipo_mp.Text != "Chapa AXB" && cmb_tipo_mp.Text != "Chapa AXB Aluminio" && cmb_tipo_mp.Text != "Tubo CV")
            //{
            //    if (System.IO.File.Exists(@"X:\Xml\Imagens\" + cod + ".JPG"))
            //    {
            //        Image image = Image.FromFile(@"X:\Xml\Imagens\" + cod + ".JPG");
            //        this.picbox_fams.Image = image;
            //    }
            //}
            //grp_geral.Controls.Clear();
            //atributosDinamicos.Clear();
            //limites = conexao_bd.busca_limites(cod);
            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theUfSession = UFSession.GetUFSession();


            string encoded_familia = TCSearchItem(fam, theSession, theUI, theUfSession);
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

            string[] atributos_fam = new string[attr.Length - 5];
            for (j = 5; j <= attr.Length - 1; j++)
            {
                theUfSession.Obj.AskName(attr[j], out atb);

                if (atb != "MATERIAL")
                {
                    atributos_fam[j - 5] = atb;

                }

            }

                obj_familia.Close(NXOpen.BasePart.CloseWholeTree.False, NXOpen.BasePart.CloseModified.CloseModified, null);
            return atributos_fam;
        }

        private void grp_ferramentas_Enter(object sender, EventArgs e)
        {

        }

        private void criarrev00ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //StreamReader str;
            //if (System.IO.File.Exists(@"C:\temp\itens_demanda_fam.txt"))
            //{

            //    str = new StreamReader(@"C:\temp\itens_demanda_fam.txt");
            //    using (str)
            //    {
            //        int cont = 0;
            //        string linha = "";
            //        while ((linha = str.ReadLine()) != null)
            //        {
            //            string[] quebra = linha.Split(';');
            //            string[] quebra2 = quebra[0].Split('-');

            //            atualizacao_fam_2312.criar_rev_item_fam(Familia, quebra[0], obj_familia, quebra[0], "M - ALTERADO MP 002468 P/ 070673", boletim, listaSelecionados, listaPintura, index_col);
            //        }
            //    }
            //}
                        //string[] lista_revisar =
                        //{
                        //    "505689"
                        //};

                        //foreach (var cod in lista_revisar)
                        //{

                        //foreach (DataGridViewRow row in dtg_resultado.SelectedRows)

                        //    {

                        //        //if (row.Cells[0].Value.ToString() == cod)
                        //        //{

                        //                    string[] item = new string[dtg_resultado.ColumnCount];
                        //            int i = 0;
                        //            foreach (DataGridViewCell cell in row.Cells)
                        //            {
                        //                item[i] = cell.Value.ToString();
                        //                i++;
                        //            }
                        //            string dados = string.Join(";", item);
                        //          //  MessageBox.Show($"Dados do item: {dados}");

                        //            int rev = Convert.ToInt32(item[4]) + 1;
                        //            frmPrincipal.Registrar_no_LOG("Item que será revisado: " + item[0] + " - " + dados);
                        //            int index_col = 0;
                        //            lista_boletins.Items.Clear();
                        //            List<string> listaPintura = new List<string>();
                        //            List<string> listaSelecionados = new List<string>();
                        //            bool boletim = false;
                        //            foreach (DataGridViewColumn col in dtg_resultado.Columns)
                        //            {
                        //                if (col.HeaderText == "PINTURA"  && row.Cells[col.Index].Value =="1")
                        //                {
                        //                      frmPrincipal.Registrar_no_LOG("Item com pintura: " + item[0] + " - " + dados);
                        //                     boletim = true;
                        //                    listaSelecionados.Add("BT 1");
                        //                    listaPintura.Add("BT 1");
                        //                    listaPintura.Add("PPPT");
                        //                    listaPintura.Add("PINTURA PÓ PRETO TEXTURIZADO");
                        //                }
                        //            }

                        //            string selectadmin = conexao_bd.select_user_admin(Environment.UserName);
                        //            if (selectadmin == "1")
                        //            {

                        //                atualizacao_fam_2312.criar_rev_item_fam(Familia, item[0], obj_familia, item, "M - ALTERADO MP 002468 P/ 070673", boletim, listaSelecionados, listaPintura, index_col);
                        //            }

                        //           // atualizacao_fam_2312.update_item_fam(Familia, item[0], obj_familia, item);

                        //        }

                        //   // }
                        ////}

                    }

        private void button23_Click(object sender, EventArgs e)
        {
            StreamReader str;
            if (System.IO.File.Exists(@"C:\temp\itens_pdf_fam.txt"))
            {

                str = new StreamReader(@"C:\temp\itens_pdf_fam.txt");
                using (str)
                {
                    int cont = 0;
                    string linha = "";
                    while ((linha = str.ReadLine()) != null)
                    {
                        string[] quebra = linha.Split('-');

                        atualizacao_fam_2312.gerar_pdf_criacao_familia(quebra[0], quebra[1]);
                    }
                }
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
          
            foreach (string item in txt_entrada_dados.Lines)
            {

                string file = $"S:\\Desenhos\\DXF\\{item}.dxf";
                txt_saida_dados.AppendText($"{file}- item procurado" + "\n");
                if (File.Exists(file))
                {
                    txt_saida_dados.AppendText($"{item}- ok" + "\n");
                }
                 else
                {
                    string[] _item = item.Split('-');
                    int rev = Convert.ToInt32(_item[1]) - 1;
                    string file_origem = $"S:\\Desenhos\\DXF\\{_item[0]}-{rev.ToString().PadLeft(3,'0')}.dxf";
                    //
                    //file = $"S:\\Desenhos\\Temp_DXF\\{item}.dxf";
                    if (File.Exists(file_origem))
                    {
                        File.Copy(file_origem, file, true);
                        txt_saida_dados.AppendText($"{item}- movido/criado" + "\n");
                    }
                    {
                         file_origem = $"S:\\Desenhos\\DXF\\{_item[0]}-{rev.ToString().PadLeft(3, '0')}.dxf";
                        if (File.Exists(file_origem))
                        {
                            File.Copy(file_origem, file, true);
                            txt_saida_dados.AppendText($"{item}- movido/criado" + "\n");
                        }
                        else
                        {
                            txt_saida_dados.AppendText($"{item}- não encontrado" + "\n");
                        }
                           
                       
                    }
                    //else
                    //{
                    //    file = $"S:\\Desenhos\\Temp_\\{item}.dxf";
                    //}

                  


                }
                //string codigo = item.Trim();

                //conexao_db_TeamCenter conn = new conexao_db_TeamCenter();

                //var (pitem, pitemrev) = conn.busca_dados_rev_demanda(codigo);

                //txt_saida_dados.AppendText(pitem + "-" + pitemrev + "\n");
            }

        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void gERARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogMessage_fam($"Total de itens para gerar {dtg_resultado.SelectedRows.Count.ToString()}");
            int cont = dtg_resultado.SelectedRows.Count;
            foreach (DataGridViewRow row in dtg_resultado.SelectedRows)
            {
                string[] item = new string[dtg_resultado.ColumnCount];
                int i = 0;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    item[i] = cell.Value.ToString();
                    i++;
                }
                atualizacao_fam_2312.gerar_pdf_criacao_familia(item[0], item[4]);
                LogMessage_fam($"Gerando PDF para o item: {item[0]} - {item[4]}");
                
                cont--;
                LogMessage_fam($"Itens restantes para gerar PDF: {cont}");
            }

        }

        private void button25_Click(object sender, EventArgs e)
        {
            foreach (string input in txt_entrada.Lines)
            {
                Session theSession = Session.GetSession();
                Part workPart = theSession.Parts.Work;
                Part displayPart = theSession.Parts.Display;


                Part part1 = null;
                try
                {
                    part1 = (Part)theSession.Parts.FindObject("@DB/" + input);
                    if (part1 != null)
                    {
                        // MessageBox.Show("Part encontrada no display. Exibindo part.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        PartLoadStatus loadStatus;
                        theSession.Parts.SetDisplay(part1, true, true, out loadStatus);
                    }


                }
                catch
                {

                    //MessageBox.Show("Part não encontrada no display. Abrindo part para exibição.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (part1 == null)
                {
                    BasePart basePart1;
                    PartLoadStatus partLoadStatus1;
                    basePart1 = theSession.Parts.OpenBaseDisplay("@DB/" + input, out partLoadStatus1);
                    part1 = (Part)basePart1;
                }

                try
                {
                    Expression expression2 = (Expression)part1.Expressions.FindObject("ALT_VAO");
                    Expression expression3 = (Expression)part1.Expressions.FindObject("LARGURA_VAO");

                    string name_tubo = "";
                    NXOpen.Assemblies.Component[] cmps = part1.ComponentAssembly.RootComponent.GetChildren();

                    foreach (NXOpen.Assemblies.Component comp in cmps)
                    {
                        if (comp.Name.Contains("CV"))
                        {
                            name_tubo = comp.GetStringAttribute("DB_PART_NAME");

                        }
                    }

                    txt_saida.AppendText($"{input}:{expression2.Value.ToString()}:{expression3.Value.ToString()}:{name_tubo}"+ Environment.NewLine);


                }
                catch
                {
                    txt_saida.AppendText($"{input} - Expressões não encontradas" + Environment.NewLine);
                }

               
                



                part1.Close(BasePart.CloseWholeTree.False, BasePart.CloseModified.CloseModified, null);
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
         

            Part part1 = null;
            try
            {
                part1 = (Part)theSession.Parts.FindObject("@DB/TPL_CJ_X/000");
                if (part1!= null)
                {
                    MessageBox.Show("Part encontrada no display. Exibindo part.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PartLoadStatus loadStatus;
                    theSession.Parts.SetDisplay(part1, true, true, out loadStatus);
                }
               
              
            }
            catch
            {

                MessageBox.Show("Part não encontrada no display. Abrindo part para exibição.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (part1 == null)
            {
                BasePart basePart1;
                PartLoadStatus partLoadStatus1;
                basePart1 = theSession.Parts.OpenBaseDisplay("@DB/TPL_CJ_X/000", out partLoadStatus1);
                part1 = (Part)basePart1;
            }
           
            // preencher expression 
            
            string[] dims = cmb_dim_x.Text.Split('x');

            string altura = dims[0].Trim();
            string largura = dims[1].Trim();
            string espessura = dims[2].Trim();


            preenche_expression("VAO_LARGURA", txt_larg_x.Text, part1);
            preenche_expression("VAO_ALTURA", txt_alt_x.Text, part1);
            preenche_expression("FOLGA", txt_folga_x.Text, part1);

            preenche_expression("ESP", espessura.Replace(",", "."), part1);
            preenche_expression("TUBO_ALTURA", altura.Replace(",", "."), part1);
            preenche_expression("TUBO_LARGURA", largura.Replace(",", "."), part1);

            NXOpen.Assemblies.Component [] cmp = part1.ComponentAssembly.RootComponent.GetChildren();
            foreach (NXOpen.Assemblies.Component comp in cmp)
            {
                NXOpen.PartLoadStatus partLoadStatus;
                NXOpen.Assemblies.ComponentAssembly.OpenComponentStatus[] openStatus1;
                NXOpen.Assemblies.Component[] componentsToOpen1 = new NXOpen.Assemblies.Component[1];
                componentsToOpen1[0] = comp;
                partLoadStatus = part1.ComponentAssembly.OpenComponents(NXOpen.Assemblies.ComponentAssembly.OpenOption.ComponentOnly, componentsToOpen1, out openStatus1);

                partLoadStatus.Dispose();

                NXOpen.PartLoadStatus partLoadStatus2;
                theSession.Parts.SetWorkComponent(comp, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus2);

                workPart = theSession.Parts.Work; // CV018119/000;1-TP_CV1_CJ_X
                partLoadStatus2.Dispose();

                NXOpen.Session.UndoMarkId markId6;
                markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

                NXOpen.Assemblies.Component nullNXOpen_Assemblies_Component = null;
                NXOpen.PartLoadStatus partLoadStatus3;
                theSession.Parts.SetWorkComponent(nullNXOpen_Assemblies_Component, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus3);

                workPart = theSession.Parts.Work; // TPL_CJ_X/000;1
                partLoadStatus3.Dispose();
                theSession.SetUndoMarkName(markId6, "Make Work Part");

            }

        }

        private void button27_Click(object sender, EventArgs e)
        {


            Session theSession = Session.GetSession();


            Part displayAnterior = theSession.Parts.Display;
            Part wp_pai = theSession.Parts.Work;
            Part wp = theSession.Parts.Work;
            NXOpen.Assemblies.Component[] compts = new NXOpen.Assemblies.Component[2];
            //NXOpen.Assemblies.Component component2 = (NXOpen.Assemblies.Component)wp.ComponentAssembly.RootComponent.FindObject("COMPONENT " + "CV018120/000" + " " + "2");
            compts[0] = (NXOpen.Assemblies.Component)wp.ComponentAssembly.RootComponent.FindObject("COMPONENT " + "CV018119/000" + " " + "1");
            compts[1] = (NXOpen.Assemblies.Component)wp.ComponentAssembly.RootComponent.FindObject("COMPONENT " + "CV018120/000" + " " + "2");

            string familias = "";
            string desc_padrao = "";

            Dictionary<string, Dictionary<string, double>> dic = new Dictionary<string, Dictionary<string, double>>();

            foreach (NXOpen.Assemblies.Component item in compts)
            {


                PartLoadStatus partLoadStatus2;

                theSession.Parts.SetWorkComponent(item, out partLoadStatus2);


                wp = theSession.Parts.Work;


                Expression expression2 = (Expression)wp.Expressions.FindObject("COMP");
                Expression expression3 = (Expression)wp.Expressions.FindObject("ANG_A_V");
                Expression expression4 = (Expression)wp.Expressions.FindObject("ANG_B_V");
                Expression expression8 = (Expression)wp.Expressions.FindObject("ANG_A_H");
                Expression expression9 = (Expression)wp.Expressions.FindObject("ANG_B_H");

                double comp = Convert.ToDouble(expression2.Value);
                double ang_a_v = Convert.ToDouble(expression3.Value);
                double ang_b_v = Convert.ToDouble(expression4.Value);
                double ang_a_h = Convert.ToDouble(expression8.Value);
                double ang_b_h = Convert.ToDouble(expression9.Value);

                Dictionary<string, double> atributos = new Dictionary<string, double>();
                atributos.Add("COMP", comp);
                atributos.Add("ANG_A_V", ang_a_v);
                atributos.Add("ANG_B_V", ang_b_v);
                atributos.Add("ANG_A_H", ang_a_h);
                atributos.Add("ANG_B_H", ang_b_h);

                dic.Add(item.DisplayName, atributos);


                LogMessage_x($"COMP: {comp} | ANG_A_V: {ang_a_v} | ANG_B_V: {ang_b_v} | ANG_A_H: {ang_a_h} | ANG_B_H: {ang_b_h}");

                familias = "FAM_CV_" + cmb_dim_x.Text;

                DataSet dados_fam = new DataSet();
                conexao_bd conn = new conexao_bd();
                dados_fam = conn.busca_fam_tubos(familias);

                foreach (DataRow dRow in dados_fam.Tables["tab_fam_nx"].Rows)
                {

                    desc_padrao = dRow["descricao_padrao"].ToString();
                }

                // itens_replace.Add(item, "");
            }
            theSession.Parts.SetWork(wp_pai);


            Dictionary<NXOpen.Assemblies.Component, string> itens_replace = new Dictionary<NXOpen.Assemblies.Component, string>();
            foreach (var item2 in dic)
            {
                LogMessage_x($"Item: {item2.Key}");
                foreach (var att in item2.Value)
                {
                    // string cv = Create_fam_cv_2312_x(familias, desc_padrao, item2.Value, displayAnterior);
                    LogMessage_x($"Atributo: {att.Key} - Valor: {att.Value}");

                }
                string cv = global_class.Create_fam_cv_2312_x(familias, desc_padrao, item2.Value, displayAnterior);

                foreach (var item in compts)
                {
                    if (item.DisplayName == item2.Key)
                    {
                        itens_replace.Add(item, cv);
                    }

                }
            }
            foreach (var item in itens_replace)
            {
                LogMessage_x($"Item para substituir: {item.Key.DisplayName} : novo item{item.Value}");
            }




            foreach (var item in itens_replace)
            {
                NXOpen.Assemblies.ReplaceComponentBuilder replaceComponentBuilder1;
                replaceComponentBuilder1 = wp_pai.AssemblyManager.CreateReplaceComponentBuilder();

                replaceComponentBuilder1.ReplaceAllOccurrences = true;

                replaceComponentBuilder1.ComponentNameType = NXOpen.Assemblies.ReplaceComponentBuilder.ComponentNameOption.AsSpecified;
                LogMessage_x($"Substituindo item: {item.Key.DisplayName} por {item.Value}");
                string _item = item.Value;
                if (_item.Contains("/000"))
                {
                    _item = _item.Remove(_item.Length - 4, 4);
                }
                bool added1;
                added1 = replaceComponentBuilder1.ComponentsToReplace.Add(item.Key);

                try
                {
                    LogMessage_x($"Part @DB/{_item}/000 encontrada para substituição.");
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + _item + "/000");
                  
                }
                catch
                {
                    LogMessage_x($"Part @DB/{_item}/000 não encontrada no display. Tentando abrir para substituição.");
                    theSession.Parts.SetNonmasterSeedPartData("@DB/" + _item + "/000");
                   
                }

                try
                {
                    replaceComponentBuilder1.ReplacementPart = "@DB/" + _item + "/000";
                    LogMessage_x($"Part @DB/{_item}/000 definida para substituição.");
                }
                catch
                {
                    replaceComponentBuilder1.ReplacementPart = "@DB/" + _item + "/000";
                    LogMessage_x($"Part @DB/{_item}/000 não encontrada para substituição. Verifique se o item foi criado corretamente.");
                }

                replaceComponentBuilder1.SetComponentReferenceSetType(NXOpen.Assemblies.ReplaceComponentBuilder.ComponentReferenceSet.Maintain, null);
                replaceComponentBuilder1.Commit();

            }
            // agora vamos fazer um saveas do item pai para um novo item    
            // gere codigo
            // saveas_x("teste_saveas");
            //wp_pai.SaveAs("@DB/" + "teste_saveas" + "/000");
            string name = $"CJ_X_TB_{cmb_dim_x.Text.Replace(" ", "")}_VAO_{txt_larg_x.Text}x{txt_alt_x.Text}mm";
            string id_new = saveas_x(name);


            if (id_new != null)
            {
                LogMessage_x($"Item salvo com sucesso: {id_new} - {name}");
            }

            frm_contraventamentos frm_Contraventamentos = new frm_contraventamentos();

            frm_Contraventamentos.criacao_externa();


          //  wp_pai.Save();

        }


        private void LogMessage_x(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => LogMessage_fam(message)));
                return;
            }

            var txtLogs = Controls.Find("txt_log_x", true).FirstOrDefault() as TextBox;
            if (txtLogs != null)
            {
                txtLogs.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
                txtLogs.ScrollToCaret();
            }
        }
        public static string saveas_x(string db_name)
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

            // Potential journal callback detected. Pausing journal.
            // Journal callback ended. Unpausing
            NXOpen.NXObject nXObject4;
            nXObject4 = partOperationCopyBuilder1.Commit();

            NXOpen.PDM.ErrorMessageHandler errorMessageHandler4;
            errorMessageHandler4 = partOperationCopyBuilder1.GetErrorMessageHandler(true);

            theSession.DeleteUndoMark(markId8, null);

            partOperationCopyBuilder1.Destroy();

            attributePropertiesBuilder2.Destroy();

           

            theSession.CleanUpFacetedFacesAndEdges();

            return new_id;
        }


        public string Create_fam_cv_2312_x(string cod_fam, string desc_padrao, Dictionary<string, double> Dict, Part displayAnterior)
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
                LogMessage_x(descricao_final);
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


                LogMessage_x("Criando o item....aguarde");

                   


                     string item_type = "M4_it_cv";
                        descricao_final = "CV_" + descricao_final;



              //  codigo_novo = "este item for criado - teste";
                    (codigo_novo, existe) = Custom_Mascarello.Create.CreateMember_new(cod_fam, descricao_final, expressao, valor, 0, tolerancia, "", descricao_final, item_type, "", false, null, null);


                    
                    if (existe == false)
                    {
                    LogMessage_x($"Item CV {codigo_novo} criado");
                    }
                    if (existe == true)
                    {
                    LogMessage_x($"Item CV {codigo_novo} existente");
                    }
                return codigo_novo;
            }

                theSession.Parts.SetDisplay(displayAnterior, true, true, out loadStatus);

            return codigo_novo;
        }
    }
}

