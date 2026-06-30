using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Custom_Mascarello
{
    public partial class frm_pesquisa_fam : Form// Syncfusion.WinForms.Controls.SfForm
    {
        string _codigo = "";
        string _desc = "";
        public String Codigo_ret
        {
            get { return _codigo; }
            set { _codigo = value; }
        }
        public String Desc_ret
        {
            get { return _desc; }
            set { _desc = value; }
        }
        public frm_pesquisa_fam()
        {
            InitializeComponent();
        }

        private void frm_pesquisa_fam_Load(object sender, EventArgs e)
        {
            string caminhoXml = @"X:\Xml\Lista_Familia\Lista_Familias_busca.xml";

            Itens = LerItensDoXml(caminhoXml);
            textBox1.Focus();
            textBox1.Text = "Digite algo relacionado a familia que procura...";
            textBox1.ForeColor = Color.Gray;
        }

        public static List<Item> LerItensDoXml(string caminhoXml)
        {
            XDocument xml = XDocument.Load(caminhoXml);

            return xml.Descendants("item")
                      .Select(x => new Item(
                          (int)x.Attribute("codigo"),
                          (string)x.Attribute("descricao"),
                          (string)x.Attribute("nl_descricao"),
                          (string)x.Attribute("classificacao"),
                          (string)x.Attribute("descricao_padrao")))
                      .ToList();
        }
        public static List<Item> Itens = new List<Item>();

        private void button1_Click(object sender, EventArgs e)
        { 
            listBox1.Items.Clear(); 


            string termoBusca = textBox1.Text.TrimEnd();
            var resultados = BuscarItens(termoBusca.ToLower());

            if (resultados.Any())
            {
                foreach (var item in resultados)
                {
                    listBox1.Items.Add($"{item.Codigo}-{item.CodigoFamilia}");
                }
            }
            else
            {
                listBox1.Items.Add("Nenhum item encontrado.");
            }
        }
        public static List<Item> BuscarItens(string termos)
        {
            // Divide a entrada do usuário em palavras individuais, ignorando espaços extras
            //var palavras = termos.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);

            //return Itens
            //    .Where(item => palavras.All(palavra => item.Descricao.Contains(palavra)))
            //    .ToList();

            var palavras = termos.Split(new[] { ' ', '_', '-', ',', '.', '/' }, StringSplitOptions.RemoveEmptyEntries);

            return Itens
                .Where(item =>
                {
                    var palavrasDescricao = item.Descricao.Split(new[] { ' ', '_', '-', '.', '/' }, StringSplitOptions.RemoveEmptyEntries);

                    // Conta quantas palavras da pesquisa existem na descrição
                    int palavrasEncontradas = palavras.Count(palavra => palavrasDescricao.Contains(palavra, StringComparer.OrdinalIgnoreCase));

                    // Calcula a porcentagem de palavras encontradas
                    double percentual = (double)palavrasEncontradas / palavras.Length * 100;

                    // Retorna itens com pelo menos 60% de correspondência
                    return percentual >= 60;
                })
                .ToList();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            string [] valor = listBox1.SelectedItem.ToString().Split('-');
            _codigo = valor[0];
            _desc = valor[1];
            this.Close();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Digite algo relacionado a familia que procura...")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black; // Muda a cor para preto quando o usuário começa a digitar
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                button1_Click(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataSet dadosXML = new DataSet();
            // referencia ao arquivo XML            
            dadosXML.ReadXml(@"X:\Xml\Lista_Familia\Lista_Familias.xml");
           
           
                foreach (DataRow dRow in dadosXML.Tables["Tubos_1"].Rows)
                {

                string up = conexao_bd.up_fam_cv(dRow["Fam_item"].ToString(), dRow["Fam_cv"].ToString(), "Tubos CV");
                
                }
            

        }


    }
    public class Item
    {
        public int Codigo { get; set; }
        public string CodigoFamilia { get; set; }
        public string Descricao { get; set; }
        public string Classificacao { get; set; }
        public string Descricao_padrao { get; set; }

        public Item(int codigo, string codigoFamilia, string descricao, string classificacao, string descricao_padrao)
        {
            Codigo = codigo;
            CodigoFamilia = codigoFamilia;
            Descricao = descricao;
            Classificacao = classificacao;
            Descricao_padrao = descricao_padrao;



        }
    }
}
