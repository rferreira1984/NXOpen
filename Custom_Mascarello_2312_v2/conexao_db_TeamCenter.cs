using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;

namespace Custom_Mascarello
{
    public class conexao_db_TeamCenter
    {

        private string server = "gm-siemens-db";
        private string database = "TCDBPRD";
        private string username = "infodba";
        private string password = "$iemensPLM";

        public DataSet buscar_pdf(string nome_arquivo)
        {

            // Substitua "sua_string_de_conexao" pela string de conexão real do seu banco de dados
            //  string connectionString = "Sua_String_De_Conexao";

            string connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + password + ";";
            DataSet ds = new DataSet();

            // Cria uma instância de SqlConnection com a string de conexão
            var connection = new SqlConnection(connectionString);
            var command = connection.CreateCommand();

            string codigo = nome_arquivo + "%.pdf%";
            // MessageBox.Show(codigo);
            string query = "SELECT TOP 1 pfile_name, psd_path_name, ptime_last_modified FROM PIMANFILE WHERE poriginal_file_name like '" + codigo + "' ORDER BY ptime_last_modified DESC";

            SqlDataAdapter ad = new SqlDataAdapter(query, connection);
            ad.Fill(ds, "PIMANFILE");


            if (connection.State == ConnectionState.Open)
                connection.Close(); SqlConnection.ClearAllPools();

            connection.Close(); SqlConnection.ClearAllPools();
            SqlConnection.ClearAllPools();
            return ds;
        }
        public DataSet buscaItensProcesso()
        {


            string connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + password + ";";
            DataSet ds = new DataSet();


            var connection = new SqlConnection(connectionString);
            var command = connection.CreateCommand();
            string query = "SELECT item.pitem_id, itemRevision.pitem_revision_id, workspaceobject.pobject_name FROM PITEMREVISION itemRevision" +
                " inner join PITEM item on itemRevision.ritems_tagu = item.puid" +
                " inner join PRELEASE_STATUS_LIST releaseStatusList on releaseStatusList.puid = itemRevision.puid" +
                " inner join PRELEASESTATUS releaseStatus on releaseStatus.puid = releaseStatusList.pvalu_0" +
                " inner join PWORKSPACEOBJECT workspaceobject on itemRevision.ritems_tagu = workspaceobject.puid" +
                " WHERE releaseStatus.pname = 'M4_EmProcesso'";

            SqlDataAdapter ad = new SqlDataAdapter(query, connection);
            ad.Fill(ds, "PITEMREVISION");

            if (connection.State == ConnectionState.Open)
                connection.Close(); SqlConnection.ClearAllPools();

            connection.Close(); SqlConnection.ClearAllPools();
            SqlConnection.ClearAllPools();
            return ds;
        }
      
        public string busca_verifica_status(string codigo, string rev)
        {

            string connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + password + ";";
            DataSet ds = new DataSet();


            var connection = new SqlConnection(connectionString);
            var command = connection.CreateCommand();
           
            try
            {
                connection.Open();
                command.CommandText = "SELECT item.puid FROM PITEM item WHERE item.pitem_id = @codigo";
                command.Parameters.AddWithValue("@codigo", codigo);
               
                object boletin = command.ExecuteScalar();


               
                command.CommandText = "SELECT itemRevision.puid FROM PITEMREVISION itemRevision WHERE itemRevision.ritems_tagu = @tag AND itemRevision.pitem_revision_id = @rev";
                command.Parameters.AddWithValue("@tag", boletin.ToString());
                command.Parameters.AddWithValue("@rev", rev.PadLeft(3,'0'));

               
                object rev_puid = command.ExecuteScalar();

                command.CommandText = "SELECT pvalu_0 FROM PRELEASE_STATUS_LIST releaseStatusList WHERE releaseStatusList.puid = @puid";
                command.Parameters.AddWithValue("@puid", rev_puid.ToString());
               


                object realese = command.ExecuteScalar();

                if (realese != null)
                {
                    command.CommandText = "SELECT releaseStatus.pname FROM PRELEASESTATUS releaseStatus WHERE releaseStatus.puid = @realese";
                    command.Parameters.AddWithValue("@realese", realese.ToString());

                    object p_name = command.ExecuteScalar();
                    
                    if (p_name != null)
                    {
                        if (p_name.ToString() == "M4_Retornado")
                        {
                            
                            return "0";
                        }
                        else
                        {

                            return "1";
                        }
                    }
                    else
                    {
                        return "0";
                    }
                   
                 
                }
                else
                {
                    return "0";
                    
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); SqlConnection.ClearAllPools();
            }
            
        }

        public string busca_rev(string codigo)
        {

            string connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + password + ";";
            DataSet ds = new DataSet();

         
            var connection = new SqlConnection(connectionString);
            var command = connection.CreateCommand();

            try
            {
                connection.Open();
                command.CommandText = "SELECT item.puid FROM PITEM item WHERE item.pitem_id = @codigo";
                command.Parameters.AddWithValue("@codigo", codigo);

                object puid = command.ExecuteScalar();

                

                command.CommandText = "SELECT TOP 1 itemRevision.pitem_revision_id FROM PITEMREVISION itemRevision WHERE itemRevision.ritems_tagu = @tag ORDER BY pitem_revision_id DESC";
                command.Parameters.AddWithValue("@tag", puid.ToString());
               


                object pitem_revision_id = command.ExecuteScalar();




                return pitem_revision_id.ToString();


            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); SqlConnection.ClearAllPools();
            }

        }
        public string busca_fam(string _puid)
        {

            string connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + password + ";";
            DataSet ds = new DataSet();


            var connection = new SqlConnection(connectionString);
            var command = connection.CreateCommand();

            try
            {
                connection.Open();
                command.CommandText = "SELECT item.pitem_id FROM PITEM item WHERE item.puid = @codigo";
                command.Parameters.AddWithValue("@codigo", _puid);

                object id = command.ExecuteScalar();



                command.CommandText = "SELECT TOP 1 itemRevision.pitem_revision_id FROM PITEMREVISION itemRevision WHERE itemRevision.ritems_tagu = @tag ORDER BY pitem_revision_id DESC";
                command.Parameters.AddWithValue("@tag", _puid.ToString());



                object pitem_revision_id = command.ExecuteScalar();




                return id +"-"+pitem_revision_id.ToString();


            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); SqlConnection.ClearAllPools();
            }

        }

        public (string, string) busca_dados_alt_desc_fam(string codigo, string rev)
        {

            string iditem = "";
            string iditemrev = "";
            string connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + password + ";";



            using (var connection = new SqlConnection(connectionString))
            {
                string query = @"
        SELECT item.puid AS iditem, itemrev.puid AS iditemrev 
        FROM PITEM item
        INNER JOIN PITEMREVISION itemrev ON itemrev.ritems_tagu = item.puid AND itemrev.pitem_revision_id = @rev
        WHERE item.pitem_id = @codigo";

                using (var command = new SqlCommand(query, connection))
                {
                    // Uso de parâmetros para evitar SQL Injection
                    command.Parameters.AddWithValue("@codigo", codigo);
                    command.Parameters.AddWithValue("@rev", rev);

                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            iditem = dr["iditem"].ToString();
                            iditemrev = dr["iditemrev"].ToString();
                        }
                    }
                }
            }

            // Retorna os valores como uma tupla
            return (iditem, iditemrev);
        }
        public (string, string) busca_dados_alt_desc(string codigo)
        {

            string iditem = "";
            string iditemrev = "";
            string connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + password + ";";



            using (var connection = new SqlConnection(connectionString))
            {
                string query = @"
        SELECT item.puid AS iditem, itemrev.puid AS iditemrev 
        FROM PITEM item
        INNER JOIN PITEMREVISION itemrev ON itemrev.ritems_tagu = item.puid
        WHERE item.pitem_id = @codigo";

                using (var command = new SqlCommand(query, connection))
                {
                    // Uso de parâmetros para evitar SQL Injection
                    command.Parameters.AddWithValue("@codigo", codigo);

                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            iditem = dr["iditem"].ToString();
                            iditemrev = dr["iditemrev"].ToString();
                        }
                    }
                }
            }

            // Retorna os valores como uma tupla
            return (iditem, iditemrev);
        }
        public (string, string) busca_dados_rev_demanda(string codigo)
        {

            string iditem = "";
            string iditemrev = "";
            string connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + password + ";";



            using (var connection = new SqlConnection(connectionString))
            {
                string query = @"
        SELECT TOP 1 pitem_id, pitem_revision_id
        FROM PITEM item
        INNER JOIN PITEMREVISION itemrev ON itemrev.ritems_tagu = item.puid
        WHERE item.pitem_id = @codigo ORDER BY pitem_revision_id DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    // Uso de parâmetros para evitar SQL Injection
                    command.Parameters.AddWithValue("@codigo", codigo);

                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            iditem = dr["pitem_id"].ToString();
                            iditemrev = dr["pitem_revision_id"].ToString();
                        }
                    }
                }
            }

            // Retorna os valores como uma tupla
            return (iditem, iditemrev);
        }

        public string update_desc(string descricao, string pitem, string pitemrev)
        {

            string connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + password + ";";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE PWORKSPACEOBJECT SET pobject_name = @descricao WHERE puid = @puid";

                using (var command = new SqlCommand(query, connection))
                {
                    // Adiciona parâmetros para evitar SQL Injection
                    command.Parameters.AddWithValue("@descricao", descricao);

                    // Atualiza o primeiro objeto
                    command.Parameters.AddWithValue("@puid", pitem);
                    command.ExecuteNonQuery();

                    // Atualiza o segundo objeto, reaproveitando o comando e atualizando o parâmetro
                    command.Parameters["@puid"].Value = pitemrev;
                    command.ExecuteNonQuery();
                }
            }

            return "Edição concluída!";
        }

        public string busca_puid_form( string codigo_rev)
        {

            string connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + password + ";";
            DataSet ds = new DataSet();


            var connection = new SqlConnection(connectionString);
            var command = connection.CreateCommand();

            try
            {
                connection.Open();
                command.CommandText = "SELECT form.rdata_fileu FROM PWORKSPACEOBJECT wso " +
                    " INNER JOIN PFORM form ON wso.puid = form.puid" +
                    " WHERE wso.pobject_name = @codigorev";
                command.Parameters.AddWithValue("@codigorev", codigo_rev);

                object puid = command.ExecuteScalar();

                return puid.ToString();


            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); SqlConnection.ClearAllPools();
            }
        }

        public string update_attr_fam(string puid, string ge, string fam, string linha, string dep, string tr, string form_obtencao)
        {
            string connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + password + ";";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE PM4_IT_MASCARELLOREVMASTERS SET pm4_grupo_estoque = @ge," +
                    "pm4_familia = @fam, " +
                    "pm4_linha_producao = @linha, " +
                    "pm4_deposito = @dep, " +
                    "pm4_tr = @tr, " +
                    "pm4_forma_obtencao = @form_obtencao " +
                    "WHERE puid = @puid";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@puid", puid);
                    command.Parameters.AddWithValue("@ge", ge);
                    command.Parameters.AddWithValue("@fam", fam);
                    command.Parameters.AddWithValue("@linha", linha);
                    command.Parameters.AddWithValue("@dep", dep);
                    command.Parameters.AddWithValue("@tr", tr);
                    command.Parameters.AddWithValue("@form_obtencao", form_obtencao);
                    command.ExecuteNonQuery();
                }
            }

            return "Edição concluída!";
        }
        public string update_attr_ge_fam(string puid, string ge, string fam)
        {
            string connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + password + ";";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE PM4_IT_MASCARELLOREVMASTERS SET pm4_grupo_estoque = @ge," +
                    "pm4_familia = @fam " +
                    "WHERE puid = @puid";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@puid", puid);
                    command.Parameters.AddWithValue("@ge", ge);
                    command.Parameters.AddWithValue("@fam", fam);

                    command.ExecuteNonQuery();
                    // Atualiza o segundo objeto, reaproveitando o comando e atualizando o parâmetro
                }
            }

            return "Edição concluída!";
        }
        public string update_attr_fam_update(string puid, string ge, string fam, string linha, string dep, string tr)
        {
            string connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + username + ";Password=" + password + ";";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE PM4_IT_MASCARELLOREVMASTERS SET "+
                    "pm4_grupo_estoque = @ge," +
                    "pm4_familia = @fam, " +
                    "pm4_linha_producao = @linha, " +
                    "pm4_deposito = @dep, " +
                    "pm4_tr = @tr " +
                    "WHERE puid = @puid";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@puid", puid);
                    command.Parameters.AddWithValue("@ge", ge);
                    command.Parameters.AddWithValue("@fam", fam);
                    command.Parameters.AddWithValue("@linha", linha);
                    command.Parameters.AddWithValue("@dep", dep);
                    command.Parameters.AddWithValue("@tr", tr);


                    command.ExecuteNonQuery();


                }
            }

            return "Edição concluída!";
        }
    }
}
