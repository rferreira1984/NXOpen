using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_Mascarello
{
    internal class conexao_sim
    {
        public static string connString = @"Server=10.1.1.134;Database=sim;User ID=renato_bd;Password=QHXqr0LldujNHDEz;SslMode=none;Connect Timeout=30000";

        public DataSet busca_dados_fm(string fm)
        {


            DataSet ds = new DataSet();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                string query = "SELECT com_titulo as v_ct, ent_titulo as v_ee, ocar_pk_carroceria as carroceria, ocar_pk_chassi as chassi, carr_titulo, cha_titulo FROM tab_pedidos p " +
                 "left join tab_orcamentos a on a.orc_pk_orcamento = p.ped_pk_orcamento " +
                 "left join tab_orcamentos_carro b on b.ocar_id = a.orc_pk_carro " +
                 "left join eng_entreeixos c on c.ent_id = b.ocar_chassi_entreeixo " +
                 "left join eng_comprimentos d on d.com_id = b.ocar_comprimento " +
                 "left join tab_carroceria e on b.ocar_pk_carroceria = e.carr_id " +
                 "left join tab_chassi f on b.ocar_pk_chassi = f.cha_id " +
                 "where ped_id = '" + fm + "' AND a.orc_versao = p.ped_versao_orcamento";
                MySqlDataAdapter ad = new MySqlDataAdapter(query, connection);
                ad.Fill(ds, "tab_pedidos");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            return ds;
        }
    }
}
