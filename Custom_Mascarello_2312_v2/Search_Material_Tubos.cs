using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.Windows.Forms;


namespace Custom_Mascarello
{
    class Search_Material_Tubos
    {
        public static string Busca_Material(string fam, string comp_solicitado)
        {
            string valor_material = "Erro!!! ";
            int valor = Convert.ToInt32(comp_solicitado);
            
            XmlDocument xmlBiblioteca = new XmlDocument();
            xmlBiblioteca.Load(@"X:\Xml\Lista_Familia\Selecao_Material.xml");
         
            XmlNodeList Lista_Familia = default(XmlNodeList);


            Lista_Familia = xmlBiblioteca.SelectNodes("/Selecao_Material/Familia");
            List<string> list = new List<string>(new string[] { });

            
            foreach (XmlNode busca_fam in Lista_Familia)
            {
                if (busca_fam.ChildNodes.Item(0).InnerText.Substring(0, 6) == fam)
                {
                    int contador1 = busca_fam.ChildNodes.Count;

                    for (int i = 1; i <= contador1 - 1; i++)
                    {
                        list.Add(busca_fam.ChildNodes.Item(i).InnerText);
                    }
                }
            }
            int[] comps = new int[list.Count];

            for (int i = 0; i <= list.Count-1; i++)
            {
                int lenght = list[i].Length - 8;
                string [] comp_mp = list[i].Substring(lenght,8).Split('x');

                string x = comp_mp[1].Remove(comp_mp[1].Length - 2, 2);
                
                comps[i] = Convert.ToInt32(x);
              
            }

            var comp_uteis = new List<Double> {};
            for (int j = 0; j <= list.Count - 1; j++)
            {
               
                if (comps[j] >= valor)
                {
                    comp_uteis.Add(comps[j]);
                }
            }
            double comparar = comp_uteis[0];

            for (int j = 0; j <= comp_uteis.Count - 1; j++)
            {

                if (comp_uteis[j] < comparar)
                {
                    comparar = comp_uteis[j];
                }
            }

            
            for (int i = 0; i <= list.Count - 1; i++)
            {
                int lenght = list[i].Length - 8;
                string[] comp_mp = list[i].Substring(lenght, 8).Split('x');

                string x = comp_mp[1].Remove(comp_mp[1].Length - 2, 2);

                comps[i] = Convert.ToInt32(x);

                if (comps[i] == comparar)
                {
                    valor_material = list[i];
                }
            }
            return valor_material;
        }

        public static string Busca_Material_Contra(string fam, string comp_solicitado)
        {
           
            string valor_material = "Erro!!! ";

            int valor = Convert.ToInt32(comp_solicitado);

            XmlDocument xmlBiblioteca = new XmlDocument();
            xmlBiblioteca.Load(@"X:\Xml\Lista_Familia\Selecao_Material.xml");

            XmlNodeList Lista_Familia = default(XmlNodeList);


            Lista_Familia = xmlBiblioteca.SelectNodes("/Selecao_Material/Familia_Contra");
            List<string> list = new List<string>(new string[] { });


            foreach (XmlNode busca_fam in Lista_Familia)
            {
                if (busca_fam.ChildNodes.Item(0).InnerText.Substring(0, 6) == fam)
                {
                    int contador1 = busca_fam.ChildNodes.Count;

                    for (int i = 1; i <= contador1 - 1; i++)
                    {
                        list.Add(busca_fam.ChildNodes.Item(i).InnerText);
                    }
                }
            }
            valor_material = list[0];
 
            return valor_material;
        }
        public static string Busca_Descricao(string fam)
        {
            string name = "Erro!!! ";

            string name_total = "";

            XmlDocument xmlBiblioteca = new XmlDocument();
            xmlBiblioteca.Load(@"X:\Xml\Lista_Familia\Selecao_Material.xml");

            XmlNodeList Lista_Familia = default(XmlNodeList);


            Lista_Familia = xmlBiblioteca.SelectNodes("/Selecao_Material/Familia");
            List<string> list = new List<string>(new string[] { });


            foreach (XmlNode busca_fam in Lista_Familia)
            {
                if (busca_fam.ChildNodes.Item(0).InnerText.Substring(0, 6) == fam)
                {
                    name_total = busca_fam.ChildNodes.Item(0).InnerText;
                }
            }
            name = name_total.Substring(7, name_total.Length - 7);

            return name;
        }
    }
}
