using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using NXOpen;
using NXOpen.UF;
using NXOpen.UIStyler;

namespace Custom_Mascarello
{
    public partial class frm_Gerenciador_NX : Form
    {
        public static string lista_busca = "";
        private static Session theSession;
        private static UI theUI;
        private static UFSession theUfSession;

        public frm_Gerenciador_NX()
        {
            InitializeComponent();
        }

        private void Gerenciador_NX_Load(object sender, EventArgs e)
        {
            lbl_inf.Text = "Aguarde buscadon informações....";
            busca_itens_datasul();
           // abrir_inf.Close();
            verificar_obsoletos();
            verificar_itens_gerais();
        }
        public  void busca_itens_datasul()
        {
            try
            {
                File.Delete(@"C:\temp\Desc.txt");
            }
            catch
            {

               
            }

            string descricao = "-";
            string cod_complementar = "-";
            string ge = "-";
            string obsoleto = "-";
            string ver_situacao = "-";
            string familia = "-";
            string unidade = "-";

            List<String> Lista_itens = new List<String>();
            List<string> lista_codigo = new List<string>();
            try
            {

            
            theSession = Session.GetSession();
            Part wp_m = theSession.Parts.Work;
            
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

                    NXObject[] objects1 = new NXObject[1];


                    NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)(wp_m.ComponentAssembly.RootComponent.FindObject("COMPONENT " + cod + " " + total_itens)));
                    objects1[0] = component1;
                    AttributePropertiesBuilder attributePropertiesBuilder1;


                    NXObject.AttributeInformation[] XXX = component1.GetUserAttributes();

                    // UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, cod.Remove(6,4));
                    string codigo = cod.Remove(6, 4);
                    bool codigoValido = Regex.IsMatch(codigo, @"^\d{6}$");
                    //UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, codigoValido.ToString());


                    if (!component1.IsSuppressed && !Lista_itens.Contains(codigo) && codigoValido == true)
                    {
                        Lista_itens.Add(codigo);
                    }
                }
            }
            }
            catch
            {

               
            }
            if (Lista_itens.Count > 0)
            {
                string path_busca = "";
                string path_xml = @"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\CUSTOMIZACOES\xml_conexoes_ds.xml";
                if (System.IO.File.Exists(path_xml))
                {
                    DataSet conexoes_XML = new DataSet();
                    conexoes_XML.ReadXml(path_xml);
                    string table = "conexoes";
                    path_busca = conexoes_XML.Tables[table].Rows[0]["consulta"].ToString();
                }
                List<String> lista_paralela = new List<String>();

                string path = @"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\CUSTOMIZACOES\xml_obsoletos.xml";
                DataSet dadosXML = new DataSet();

                dadosXML.ReadXml(path);
                foreach (DataRow dRow in dadosXML.Tables["item"].Rows)
                {
                    /// Console.Write(dRow["codigo"].ToString() + "\n");

                    lista_paralela.Add(dRow["codigo"].ToString());

                }

                for (int j = 0; j <= Lista_itens.Count - 1; j++)
                {
                    lista_busca += Lista_itens[j] + ",";
                }

                if (lista_busca != "")
                {
                    string codigo_busca = "\"" + lista_busca.Remove(lista_busca.Length - 1, 1) + "\"";

                    System.Diagnostics.Process abrir_teste = new System.Diagnostics.Process();
                    abrir_teste.StartInfo.FileName = @"C:\\Windows\system32\cmd.exe";
                    abrir_teste.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    string caminho = path_busca + codigo_busca;
                    abrir_teste.StartInfo.Arguments = "/c " + caminho;
                    abrir_teste.Start();
                    abrir_teste.WaitForExit();
                }

                StreamReader str_cont;
                str_cont = new StreamReader(@"C:\temp\Desc.txt");
                int n_linhas = 0;
                using (str_cont)
                {


                    while ((str_cont.ReadLine()) != null)
                    {
                        n_linhas++;
                    }
                    str_cont.Close();
                }

                StreamReader str;
                str = new StreamReader(@"C:\temp\Desc.txt");
                int linha_ini = n_linhas - (Lista_itens.Count - 1);
                int cont = 1;
                int cont_1 = 0;
                using (str)
                {

                    string linha;
                    while ((linha = str.ReadLine()) != null)
                    {
                        if (cont >= linha_ini)
                        {

                            string[] separa_desc = linha.Split(';');

                            descricao = separa_desc[0].Trim();
                            ge = separa_desc[1].Trim();
                            obsoleto = separa_desc[2].Trim();
                            familia = separa_desc[7].Trim();
                            unidade = separa_desc[4].Trim();
                            if (cod_complementar == "")
                            {
                                cod_complementar = "-";
                            }

                            ver_situacao = "Ativo";

                            if (lista_paralela.Contains(Lista_itens[cont_1]))
                            {
                                ver_situacao = "Obsoleto";
                            }
                                //if (obsoleto == "Totalmente Obsoleto")
                                //{
                                //    ver_situacao = "Obsoleto";
                                //}
                                //else
                                //{
                                //    ver_situacao = "Ativo";
                                //}
                                string[] rowi = { Lista_itens[cont_1], descricao, ge, unidade, familia, ver_situacao };

                            dtg_estrutura.Rows.Add(rowi);
                            dtg_estrutura.Rows[dtg_estrutura.RowCount - 1].ReadOnly = true;
                            cont_1++;
                        }
                        cont++;
                    }
                    str.Close();



                    lbl_inf.SendToBack();

                }
            }

            else
            {
                List<String> Lista_mp = new List<String>();
                theSession = Session.GetSession();
                Part wp_m = theSession.Parts.Work;

                NXObject.AttributeInformation[] attr_pai = wp_m.GetUserAttributes();

                foreach (var item in attr_pai)
                {

                    if (item.Title == "Material")
                    {
                        Lista_mp.Add(item.StringValue.Substring(0, 6));
                       // MessageBox.Show(item.StringValue.Substring(0, 6));
                    }
                    

                }
                if (Lista_mp.Count > 0)
                {

                    for (int j = 0; j <= Lista_mp.Count - 1; j++)
                    {
                        lista_busca += Lista_mp[j] + ",";
                    }

                    if (lista_busca != "")
                    {
                        string codigo_busca = "\"" + lista_busca.Remove(lista_busca.Length - 1, 1) + "\"";

                        System.Diagnostics.Process abrir_teste = new System.Diagnostics.Process();
                        abrir_teste.StartInfo.FileName = @"C:\\Windows\system32\cmd.exe";
                        abrir_teste.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        string caminho = "\\\\10.1.1.114\\datasul\\oe11\\bin\\prowin32.exe -basekey \"ini\" -ininame \\\\10.1.1.114\\erp\\scripts-8580\\datasul-progress.ini  -pf \\\\10.1.1.114\\ERP\\scripts-8580\\PfEng.pf -p U:\\ems2\\_Custom\\es0233.r -param " + codigo_busca;
                        abrir_teste.StartInfo.Arguments = "/c " + caminho;
                        abrir_teste.Start();
                        abrir_teste.WaitForExit();
                    }

                    StreamReader str_cont;
                    str_cont = new StreamReader(@"C:\temp\Desc.txt");
                    int n_linhas = 0;
                    using (str_cont)
                    {


                        while ((str_cont.ReadLine()) != null)
                        {
                            n_linhas++;
                        }
                        str_cont.Close();
                    }

                    StreamReader str;
                    str = new StreamReader(@"C:\temp\Desc.txt");
                    int linha_ini = n_linhas - (Lista_mp.Count - 1);
                    int cont = 1;
                    int cont_1 = 0;
                    using (str)
                    {

                        string linha;
                        while ((linha = str.ReadLine()) != null)
                        {
                            if (cont >= linha_ini)
                            {
                               
                                string[] separa_desc = linha.Split(';');

                                descricao = separa_desc[0].Trim();
                                ge = separa_desc[1].Trim();
                                obsoleto = separa_desc[2].Trim();
                                familia = separa_desc[7].Trim();
                                unidade = separa_desc[4].Trim();
                                if (cod_complementar == "")
                                {
                                    cod_complementar = "-";
                                }

                                if (obsoleto == "Totalmente Obsoleto")
                                {
                                    ver_situacao = "Obsoleto";
                                }
                                else
                                {
                                    ver_situacao = "Ativo";
                                }
                                string[] rowi = { Lista_mp[cont_1], descricao, ge, unidade, familia, ver_situacao };

                                dtg_estrutura.Rows.Add(rowi);
                                dtg_estrutura.Rows[dtg_estrutura.RowCount - 1].ReadOnly = true;
                                cont_1++;
                            }
                            cont++;
                        }
                        str.Close();



                        lbl_inf.SendToBack();

                    }
                }
            }
            


        }
        public void verificar_itens_gerais()// verificação no produto
        {
            string inf_ggf_ini = "Item GGF não chamar na estrutura----> ";
            string inf_ggf_final = "";
            string familia = "";


            for (int i = 0; i <= dtg_estrutura.RowCount - 1; i++) //verificar filhos 
            {
                familia = dtg_estrutura.Rows[i].Cells[5].Value.ToString();




                if (familia == "48" || familia == "50")
                {



                    dtg_estrutura.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LemonChiffon;
                    inf_ggf_final += dtg_estrutura.Rows[i].Cells[1].Value.ToString() + "; ";


                }

            }
      
        }
        public void verificar_obsoletos()// verificação no produto
        {
            string obsoleto = "";

            for (int x = 0; x <= dtg_estrutura.Rows.Count - 1; x++)
            {
                if (dtg_estrutura.Rows[x].Cells[5].Value.ToString() == "Obsoleto")
                {
                    dtg_estrutura.Rows[x].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    obsoleto += dtg_estrutura.Rows[x].Cells[1].Value.ToString() + "; ";
                    
                }
            }
            
        }
        private void dtg_estrutura_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void frm_Gerenciador_NX_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                File.Delete(@"C:\temp\Desc.txt");

            }
            catch
            {


            }
        }
    }
}
