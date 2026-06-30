using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Custom_Mascarello
{
    public class BuscaDadosFicha
    {

        public static string busca_dna(string id_sol)
        {
            string messagem = "";
            using (var client = new WebClient())
            {
                string gera_tokem1 = GerarMD5("SPOL+SIM" + System.DateTime.Now.ToString("yyyy-MM-dd"));
                string gera_tokem = SHA1(gera_tokem1);

                var values = new NameValueCollection();
                values["token"] = gera_tokem;
                values["fm"] = id_sol;

                var response = client.UploadValues("http://extranet.mascarello.com.br/sim/webhook/spol/getCodigosDNA", values);

                var responseString = Encoding.Default.GetString(response);
                messagem = responseString;
            }
            return messagem;
        }
        private static string SHA1(string strPlain)
        {
            ASCIIEncoding UE = new ASCIIEncoding();

            byte[] HashValue, MessageBytes = UE.GetBytes(strPlain);

            SHA1Managed SHhash = new SHA1Managed();

            string strHex = "";


            HashValue = SHhash.ComputeHash(MessageBytes);

            foreach (byte b in HashValue)

            {
                strHex += String.Format("{0:x2}", b);
            }

            return strHex;
        }
        public static string GerarMD5(string valor)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] valorCriptografado = md5Hasher.ComputeHash(Encoding.Default.GetBytes(valor));

            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < valorCriptografado.Length; i++)

            {
                strBuilder.Append(valorCriptografado[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }
    }
}
