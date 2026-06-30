using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using MySqlX.XDevAPI;
using Newtonsoft.Json.Linq;
using NXOpen;
using NXOpen.Annotations;
using NXOpen.BlockStyler;
using NXOpen.UF;
using NXOpen.UIStyler;
using Syncfusion.Windows.Forms.Tools;
using static NXOpen.UF.UFCurve;
using Session = NXOpen.Session;

namespace Custom_Mascarello
{
    public partial class frm_MCC : Form
    {
        string CHASSI = "";
        string COMP = "";
        string EE = "0";
        string BD = "0";
        string BT = "0";
        string EE_P0 = "0";
        string EET = "0";
        string POS_TQ = "0";
        string VAO_TQ = "0";
        string POS_PORTA_DPM = "0";
        string CAMA_MOTORISTA = "0";
        string POS_CAMA = "0";
        string POS_DIANT_BASE_INF = "";
        string POS_TRASEIRA_BASE_INF = "";
        string FINAL_LONG_LE_BASE = "";
        string FINAL_LONG_LD_BASE = "";
        string POS_TB_ARO_TRAS = "";
        string POS_INICIO_AC = "";
        string AC = "";
        string MARCA_AC = "";

        Dictionary<string, string> Dados = new Dictionary<string, string>();

        public frm_MCC()
        {
            InitializeComponent();
        }
        private void frm_MCC_Load(object sender, EventArgs e)
        {

        }
        // criar estilos para os botões do form
        private void btn_importar_MouseEnter(object sender, EventArgs e)
        {
            btn_importar.BackColor = Color.FromArgb(0, 0, 0);
            btn_importar.ForeColor = Color.FromArgb(255, 255, 255);
        }
        Veiculo veiculo = new Veiculo();
        List<item_tarefas.Item> lista_items = new List<item_tarefas.Item>();
        private void btn_importar_Click(object sender, EventArgs e)
        {
            string fm = txt_FM.Text;
            string fm_retorno = busca_itens_tarefa.busca_tarefas(fm);

            // dynamic objeto = Newtonsoft.Json.Linq.JObject.Parse(fm);
            JObject objeto_json = JObject.Parse(fm_retorno);
            string caminhoArquivo = "C:\\temp\\" + fm + ".json";

            // Salvar o objeto JSON em um arquivo
            File.WriteAllText(caminhoArquivo, objeto_json.ToString());
           
            string id_orcamento = "";
            foreach (var item in objeto_json["tarefas"])
            {
                lista_items.Add(new item_tarefas.Item
                {
                    oitem_pk_orcamento = item["oitem_pk_orcamento"].ToString(),
                    oitem_pk_grupo = item["oitem_pk_grupo"].ToString(),
                    gru_titulo = item["gru_titulo"].ToString(),
                    sub_grupo = item["sub_grupo"].ToString(),
                    sub_referencia = item["sub_referencia"].ToString(),
                    sub_titulo = item["sub_titulo"].ToString(),
                    item_grupo = item["item_grupo"].ToString(),
                    item_referencia = item["item_referencia"].ToString(),
                    item_titulo = item["item_titulo"].ToString(),
                    item_id = item["item_id"].ToString(),
                    nome_variavel = "v_"+item["sub_grupo"].ToString()+ item["sub_referencia"].ToString() + item["item_referencia"].ToString(),
                    valor_variavel = item["sub_grupo"].ToString() + "." + item["sub_referencia"].ToString() + "." + item["item_referencia"].ToString() + "." + item["item_titulo"].ToString()
                });
                
            }

            //foreach (var item in lista)
            //{
            //    MessageBox.Show(item.nome_variavel + " = " +item.valor_variavel);
               
            //}   

            tree2.Nodes.Clear();
            TreeNode n1 = new TreeNode();
            string grupo = "";
            foreach (var item in objeto_json["tarefas"])
            {

                TreeNode sub_item = new TreeNode();
                id_orcamento = item["oitem_pk_orcamento"].ToString();
                TreeNode item_referencia = new TreeNode();
                if (grupo == item["oitem_pk_grupo"].ToString())
                {
                    sub_item = n1.Nodes.Add(item["sub_grupo"].ToString() + "." + item["sub_referencia"].ToString() + " - " + item["sub_titulo"].ToString());
                    item_referencia = sub_item.Nodes.Add(item["item_grupo"].ToString() + "." + item["sub_referencia"].ToString() + "." + item["item_referencia"].ToString() + " - " + item["item_titulo"].ToString());
                }
                else
                {
                    n1 = tree2.Nodes.Add(item["oitem_pk_grupo"].ToString() + " - " + item["gru_titulo"].ToString());
                    sub_item = n1.Nodes.Add(item["sub_grupo"].ToString() + "." + item["sub_referencia"].ToString() + " - " + item["sub_titulo"].ToString());
                    item_referencia = sub_item.Nodes.Add(item["item_grupo"].ToString() + "." + item["sub_referencia"].ToString() + "." + item["item_referencia"].ToString() + " - " + item["item_titulo"].ToString());
                }
                grupo = item["oitem_pk_grupo"].ToString();

            }


            conexao_bd _busca_dados = new conexao_bd();


            DataSet busca_dados_nodes = _busca_dados.dados_pedido(id_orcamento);

            DataTable dataTable = busca_dados_nodes.Tables["Resultado"];
            

            foreach (DataRow row in dataTable.Rows)
            {

                // Exemplo de como acessar e modificar propriedades
                veiculo.Chassi = row["cha_titulo"].ToString();
                veiculo.CT = row["com_titulo"].ToString();
                veiculo.EE = row["ent_titulo"].ToString();
                veiculo.BD = row["bd"].ToString();
                veiculo.p0_BD = row["p0_bd"].ToString();
                lbl_carroceria.Text = row["carr_titulo"].ToString();
                lbl_chassi.Text = row["cha_titulo"].ToString();
                lbl_comprimento.Text = row["com_titulo"].ToString();
                lbl_ee.Text = row["ent_titulo"].ToString();
                lbl_norma.Text = row["nor_titulo"].ToString();
            }
           
                //string detalhes = busca_itens_tarefa.busca_detalhes("3704");

                ////JObject detalhes_json = JObject.Parse(detalhes);
                //caminhoArquivo = "C:\\temp\\3107.txt";

                //// Salvar o objeto JSON em um arquivo
                //File.WriteAllText(caminhoArquivo, detalhes);
                // treeView
                //XmlDocument doc = new XmlDocument();
                //doc.Load(@"http://extranet.mascarello.com.br/webservicesimples/pegatarefapedido.php?id=" + txt_FM.Text);
                // caminhoArquivo = "C:\\temp\\" + fm + ".xml";
                //doc.Save(caminhoArquivo);
                //XmlNodeList Lista_Postos = default(XmlNodeList);
                //Lista_Postos = doc.SelectNodes(" /webpedidotarefa");

                // TreeNode rootNode = new TreeNode();
                // string[] lista = { "1 - PORTAS", "2 - TANQUE DE COMBUSTÍVEL", "3 - CLIMATIZAÇÃO", "4 - PORTA - PACOTE", "5 - SANITÁRIO", "6 - BAR", "7 - DIVISÓRIAS", "8 - POLTRONAS", "9 - JANELAS","10 - ITINERÁRIOS", "11 - BAGAGEIRO", "12 - CABINA",
                //"13 - DIVERSOS", "14 - FERRAMENTAS", "15 - INSTALAÇÃO ELÉTRICA", "16 - ILUMINAÇÃO", "17 - SINALIZAÇÃO", "18 - REVESTIMENTO", "19 - PINTURA"};
                // for (int i = 1; i <= lista.Length; i++)
                // {
                //     rootNode = tree2.Nodes.Add(lista[i - 1]);
                //     string valor_node = i.ToString();
                //     TreeNode states1 = new TreeNode();
                //     foreach (XmlNode posto in Lista_Postos)
                //     {
                //         int cont = posto.ChildNodes.Count;
                //         for (int j = 0; j < cont; j++)
                //         {
                //             string opcao = posto.ChildNodes.Item(j).InnerText;
                //             if (opcao != "1")
                //             {
                //                 string[] quebra_valor = opcao.Split('.');
                //                 try
                //                 {
                //                     int num = Convert.ToInt32(quebra_valor[0]);

                //                     if (valor_node == num.ToString())
                //                     {

                //                         states1 = rootNode.Nodes.Add(posto.ChildNodes.Item(j).InnerText);
                //                         if (posto.ChildNodes.Item(j).InnerText.Substring(0, 5) == "13.24")
                //                         {

                //                             CAMA_MOTORISTA = posto.ChildNodes.Item(j).InnerText.Substring(0,7);
                //                             Dados.Add("CAMA_MOTORISTA", posto.ChildNodes.Item(j).InnerText.Substring(0, 7));
                //                         }
                //                         if (posto.ChildNodes.Item(j).InnerText.Substring(0, 4) == "3.11")
                //                         {
                //                             string[] quebra = posto.ChildNodes.Item(j).InnerText.Split('-');

                //                             AC = quebra[0].TrimEnd();
                //                             MARCA_AC = quebra[1].TrimStart().TrimEnd();
                //                             Dados.Add("AC", quebra[0].TrimEnd());
                //                             Dados.Add("MARCA_AC", quebra[1].TrimStart().TrimEnd());
                //                         }
                //                     }
                //                 }
                //                 catch
                //                 {

                //                 }
                //             }

                //         }

                //     }
                // }
                //foreach (XmlNode posto in Lista_Postos)
                //{

                //    int cont = posto.ChildNodes.Count;

                //    for (int j = 0; j < cont; j++)
                //    {
                //        if (posto.ChildNodes.Item(j).Name == "CHASSI")
                //        {
                //            lbl_chassi.Text = posto.ChildNodes.Item(j).InnerText;
                //            Dados.Add("CHASSI", posto.ChildNodes.Item(j).InnerText);
                //        }

                //        if (posto.ChildNodes.Item(j).Name == "CARROCERIA")
                //        {
                //            lbl_carroceria.Text = posto.ChildNodes.Item(j).InnerText;

                //        }
                //        if (posto.ChildNodes.Item(j).Name == "COMPRIMENTO")
                //        {
                //            lbl_comprimento.Text = posto.ChildNodes.Item(j).InnerText;
                //            Dados.Add("COMP", posto.ChildNodes.Item(j).InnerText);
                //        }
                //        if (posto.ChildNodes.Item(j).Name == "ENTREEIXO")
                //        {
                //            lbl_ee.Text = posto.ChildNodes.Item(j).InnerText;

                //        }
                //        if (posto.ChildNodes.Item(j).Name == "NORMA")
                //        {
                //            lbl_norma.Text = posto.ChildNodes.Item(j).InnerText;
                //        }
                //    }
                //}
            }
       
        private void btn_aplicar_mcc_Click(object sender, EventArgs e)
        {
            

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            theSession.Preferences.Modeling.UpdatePending = false;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Expression");

            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            
            foreach(PropertyInfo prop in veiculo.GetType().GetProperties())
             {
                string name = prop.Name;
                object value = prop.GetValue(veiculo);
               
                Expression expression1 = (Expression)workPart.Expressions.FindObject(name);

                if(expression1 != null)
                {
                    if (expression1.Type.ToString() == "String" || expression1.Type.ToString() == "string")
                    {
                        workPart.Expressions.EditWithUnits(expression1, null, "\"" + value.ToString() + "\"");
                    }
                    else
                    {
                        workPart.Expressions.EditWithUnits(expression1, unit1, value.ToString());
                    }
                }
            }

            foreach (var item in lista_items)
            {
               
                try
                {
                   
                    Expression expression1 = (Expression)workPart.Expressions.FindObject(item.nome_variavel);
                    if (expression1.Type.ToString() == "String" || expression1.Type.ToString() == "string")
                    {
                        workPart.Expressions.EditWithUnits(expression1, null, "\"" + item.valor_variavel.ToString() + "\"");
                    }
                    else
                    {
                        workPart.Expressions.EditWithUnits(expression1, unit1, item.valor_variavel.ToString());
                    }
                }
                catch 
                {

                    
                }
               
            }
            lista_items.Clear();
            theSession.Preferences.Modeling.UpdatePending = true;

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Expression Data");

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId3);
            theSession.DeleteUndoMark(markId3, "Update Expression Data");

            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

            MessageBox.Show("Configurações aplicadas");
        }

        //private void busca_fm(string fm, string idditem_pk_item, string sol_carroceria)
        //{
        //    conexao_bd _busca_dados = new conexao_bd();


        //    string fm_retorno = busca_itens_tarefa.busca_tarefas(fm);

        //    JObject objeto_json = JObject.Parse(fm_retorno);
        //    JToken status = objeto_json["status"];

        //    List<item_tarefas.Item> lista = new List<item_tarefas.Item>();
        //    if (status.ToString() != "404")
        //    {

        //        foreach (var item in objeto_json["tarefas"])
        //        {
        //            lista.Add(new item_tarefas.Item
        //            {
        //                oitem_pk_grupo = item["oitem_pk_grupo"].ToString(),
        //                gru_titulo = item["gru_titulo"].ToString(),
        //                sub_grupo = item["sub_grupo"].ToString(),
        //                sub_referencia = item["sub_referencia"].ToString(),
        //                sub_titulo = item["sub_titulo"].ToString(),
        //                item_grupo = item["item_grupo"].ToString(),
        //                item_referencia = item["item_referencia"].ToString(),
        //                item_titulo = item["item_titulo"].ToString(),
        //                item_id = item["item_id"].ToString(),
        //            });
        //        }



        //        tree2.Nodes.Clear();
        //        TreeNode n1 = new TreeNode();

        //        DataSet busca_dados_nodes = _busca_dados.postos_tarefas(idditem_pk_item, sol_carroceria);

        //        DataTable dataTable = busca_dados_nodes.Tables["Resultado"];


        //        foreach (DataRow row in dataTable.Rows)
        //        {

        //            TreeNode pai = new TreeNode(row["oitem_pk_grupo"].ToString() + " - " + row["gru_titulo"].ToString());
        //            string node_name_pai = row["oitem_pk_grupo"].ToString() + row["gru_titulo"].ToString();
        //            pai.Name = node_name_pai;

        //            TreeNode n1_analise = new TreeNode(row["oitem_pk_grupo"].ToString() + "." + row["sub_referencia"].ToString() + "." + row["sub_titulo"].ToString());

        //            string node_name_n1_analise = row["oitem_pk_grupo"].ToString() + row["gru_titulo"].ToString();
        //            n1_analise.Name = node_name_n1_analise;

        //            TreeNode n2_analise = new TreeNode(row["oitem_pk_grupo"].ToString() + "." + row["sub_referencia"].ToString() + "." + row["pk_sub_referencia"].ToString() + " - " + row["item_titulo"].ToString());

        //            string node_name_n2_analise = row["oitem_pk_grupo"].ToString() + row["sub_referencia"].ToString() + row["pk_sub_referencia"].ToString() + row["item_titulo"].ToString();
        //            n2_analise.Name = node_name_n2_analise;

        //            TreeNode n3_analise = new TreeNode(row["observacao"].ToString());
        //            n3_analise.BackColor = System.Drawing.Color.Yellow;
        //            n2_analise.Nodes.Add(n3_analise);

        //            n1_analise.Nodes.Add(n2_analise);


        //            TreeNodeCollection nodes_pai = tree2.Nodes;

        //            if (nodes_pai.ContainsKey(pai.Name))
        //            {
        //                pai = nodes_pai[node_name_pai];
        //                pai.Nodes.Add(n1_analise);
        //            }
        //            else
        //            {
        //                pai.Nodes.Add(n1_analise);
        //                tree2.Nodes.Add(pai);
        //            }
        //        }
        //        tree2.ExpandAll();

        //        tree2.Nodes.Clear();
        //        n1.Nodes.Clear();
        //        string grupo = "";
        //        TreeNodeAdv n1_2 = new TreeNodeAdv();
        //        foreach (var item in objeto_json["tarefas"])
        //        {
        //            TreeNode sub_item = new TreeNode();

        //            TreeNode item_referencia = new TreeNode();
        //            if (grupo == item["oitem_pk_grupo"].ToString())
        //            {


        //                sub_item = n1.Nodes.Add(item["sub_grupo"].ToString() + "." + item["sub_referencia"].ToString() + " - " + item["sub_titulo"].ToString());
        //                sub_item.BackColor = System.Drawing.Color.FromArgb(201, 201, 201);
        //                item_referencia = sub_item.Nodes.Add(item["item_grupo"].ToString() + "." + item["sub_referencia"].ToString() + "." + item["item_referencia"].ToString() + " - " + item["item_titulo"].ToString());
        //                item_referencia.BackColor = System.Drawing.Color.FromArgb(122, 122, 122);
        //                item_referencia.ForeColor = System.Drawing.Color.WhiteSmoke;
        //            }
        //            else
        //            {

        //                n1 = tree2.Nodes.Add(item["oitem_pk_grupo"].ToString() + " - " + item["gru_titulo"].ToString());
        //                n1.BackColor = System.Drawing.Color.FromArgb(189, 189, 189);
        //                sub_item = n1.Nodes.Add(item["sub_grupo"].ToString() + "." + item["sub_referencia"].ToString() + " - " + item["sub_titulo"].ToString());
        //                sub_item.BackColor = System.Drawing.Color.FromArgb(201, 201, 201);
        //                item_referencia = sub_item.Nodes.Add(item["item_grupo"].ToString() + "." + item["sub_referencia"].ToString() + "." + item["item_referencia"].ToString() + " - " + item["item_titulo"].ToString());
        //                item_referencia.BackColor = System.Drawing.Color.FromArgb(122, 122, 122);
        //                item_referencia.ForeColor = System.Drawing.Color.WhiteSmoke;
        //            }
        //            grupo = item["oitem_pk_grupo"].ToString();

        //        }
        //    }

        //}
    }

    
}
