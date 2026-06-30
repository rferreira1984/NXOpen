using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Teamcenter.Soa.Client.Model.Strong;
using static NXOpen.GeometricUtilities.CurveFitData;

namespace Custom_Mascarello
{   /// <summary>
/// 4.0
/// </summary>
    public class conexao_bd
    {

      //public static string connString = @"Server=10.1.1.111;Database=gerenciadorprojeto;User ID=renato_bd;Password=renatomasca;SslMode=none";
        public static string connString = @"Server=10.1.1.134;Database=gerenciadorprojeto;User ID=renato_bd;Password=QHXqr0LldujNHDEz;SslMode=none;Connect Timeout=30000";
        public static string verifica_alteracao(string codigo, string rev)
        {
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                connection.Open();
                command.CommandText = "SELECT * FROM tab_alteracoes where codigo ='" + codigo + "'and rev ='" + rev + "'"; 
                object existe = command.ExecuteScalar();

                if (existe != null)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
        public static string pos_wc(string carroceria, string chassi)
        {
            string pos_wc = "";


            List<int> rev = new List<int>();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                command.CommandText = "SELECT posicao_wc FROM plm_atributos " +
                     "where id_carroceria ='" + carroceria + "'and id_chassi ='" + chassi + "'";

                connection.Open();

                MySqlDataReader dr;
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    pos_wc = dr["posicao_wc"].ToString();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            return pos_wc;
        }
        public static string busca_ac(string marca)
        {
            string vao_recorte_madeira = "";


            List<int> rev = new List<int>();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                command.CommandText = "SELECT vao_recorte_pp_ac FROM plm_atributos " +
                "where marca_ac ='"+ marca  +"'";
                
                connection.Open();

                MySqlDataReader dr;
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    vao_recorte_madeira = dr["vao_recorte_pp_ac"].ToString();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            return vao_recorte_madeira;
        }
        public void insert_dados_fam(string _codigo, string _descricao, string _nl_descricao, string _classificacao, string _descricao_padrao)
        {
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                connection.Open();
                command.CommandText = "INSERT INTO tab_fam_nx(codigo,descricao, nl_descricao, classificacao, descricao_padrao) " +
                    "VALUES (@codigo,@descricao, @nl_descricao, @classificacao, @descricao_padrao)";
                command.Parameters.AddWithValue("@codigo", _codigo);
                command.Parameters.AddWithValue("@descricao", _descricao);
                command.Parameters.AddWithValue("@nl_descricao", _nl_descricao);
                command.Parameters.AddWithValue("@classificacao", _classificacao);
                command.Parameters.AddWithValue("@descricao_padrao", _descricao_padrao);
                command.ExecuteNonQuery();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
        }
        public void insert_wf_auto(string _codigo, string _rev)
        {
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                connection.Open();
                command.CommandText = "INSERT INTO tab_workflow_nx (codigo, rev) " +
                    "VALUES (@codigo, @rev)";
                command.Parameters.AddWithValue("@codigo", _codigo);
                command.Parameters.AddWithValue("@rev", _rev);
                command.ExecuteNonQuery();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
        }
        public DataSet busca_categorias_fam(string categoria)
        {
            DataSet ds = new DataSet();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                string query = "SELECT DISTINCT classificacao FROM tab_fam_nx";
                MySqlDataAdapter ad = new MySqlDataAdapter(query, connection);
                ad.Fill(ds, "tab_fam_nx");
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            connection.Close(); MySqlConnection.ClearAllPools();
            return ds;
        }
        public DataSet busca_cv_x()
        {
            DataSet ds = new DataSet();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                string query = "SELECT fam_cv FROM tab_fam_nx WHERE cj_x ='1'";
                MySqlDataAdapter ad = new MySqlDataAdapter(query, connection);
                ad.Fill(ds, "tab_fam_nx");
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            connection.Close(); MySqlConnection.ClearAllPools();
            return ds;
        }
        public DataSet ExecuteSelectQuery(string query, Dictionary<string, object> parameters)
        {

            DataSet ds = new DataSet();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                   
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }
        public DataSet busca_x(string _alt, string _larg, string _larg_tubo, string _alt_tubo, string _esp_tubo, string folga)
        {
           
            DataSet ds = new DataSet();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                string query = "SELECT * FROM tab_classificacao_cj_x WHERE alt =@alt AND larg = @larg AND larg_tubo = @larg_tubo AND alt_tubo = @alt_tubo AND esp_tubo = @esp_tubo AND folga = @folga";

                var paramsBusca = new Dictionary<string, object>
                {
                    { "@alt", _alt },
                    { "@larg", _larg },
                    { "@larg_tubo", _larg_tubo },
                    { "@alt_tubo", _alt_tubo },
                    { "@esp_tubo", _esp_tubo },
                    { "@folga", folga }
                };

                ds = ExecuteSelectQuery(query, paramsBusca);
               
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            connection.Close(); MySqlConnection.ClearAllPools();
            return ds;
        }
        public DataSet busca_fam(string categoria)
        {
            DataSet ds = new DataSet();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                string query = "SELECT * FROM tab_fam_nx WHERE classificacao ='" + categoria + "' AND status = '1' ORDER BY classificacao ASC";
                MySqlDataAdapter ad = new MySqlDataAdapter(query, connection);
                ad.Fill(ds, "tab_fam_nx");
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            connection.Close(); MySqlConnection.ClearAllPools();
            return ds;
        }
        public DataSet busca_fam_tubos(string dimensao)
        {
            DataSet ds = new DataSet();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                string query = "SELECT * FROM tab_fam_nx WHERE fam_cv ='" + dimensao + "' AND status = '1'";
                MySqlDataAdapter ad = new MySqlDataAdapter(query, connection);
                ad.Fill(ds, "tab_fam_nx");
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            connection.Close(); MySqlConnection.ClearAllPools();
            return ds;
        }
        public void insert_boletim(string _codigo, string _rev, string _id_boletim, string id_pintura)
        {
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                connection.Open();
                command.CommandText = "INSERT INTO tab_boletim_projeto(codigo_item,revisao_item, id_boletim, id_pintura) " +
                    "VALUES (@codigo_item,@revisao_item, @id_boletim, @id_pintura)";
                command.Parameters.AddWithValue("@codigo_item", _codigo);
                command.Parameters.AddWithValue("@revisao_item", _rev);
                command.Parameters.AddWithValue("@id_boletim", _id_boletim);
                command.Parameters.AddWithValue("@id_pintura", id_pintura);
                command.ExecuteNonQuery();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
        }

        public void insert_cj_x(string _codigo, string _alt, string _larg, string _larg_tubo, string _alt_tubo, string _esp_tubo, string folga)
        {
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                connection.Open();
                command.CommandText = "INSERT INTO tab_classificacao_cj_x(codigo,alt, larg, larg_tubo, alt_tubo, esp_tubo, folga) " +
                    "VALUES (@codigo,@alt, @larg, @larg_tubo, @alt_tubo, @esp_tubo, @folga)";
                command.Parameters.AddWithValue("@codigo", _codigo);
                command.Parameters.AddWithValue("@alt", _alt);
                command.Parameters.AddWithValue("@larg", _larg);
                command.Parameters.AddWithValue("@larg_tubo", _larg_tubo);
                command.Parameters.AddWithValue("@alt_tubo", _alt_tubo);
                command.Parameters.AddWithValue("@esp_tubo", _esp_tubo);
                command.Parameters.AddWithValue("@folga", folga);

                command.ExecuteNonQuery();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
        }
        public void delete_boletim(string _codigo, string _rev, string _id_boletim)
        {


            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                connection.Open();

                command.CommandText = "DELETE FROM tab_boletim_projeto WHERE codigo_item = @codigo_item AND revisao_item = @revisao_item AND id_boletim = @id_boletim";
                command.Parameters.AddWithValue("@codigo_item", _codigo);
                command.Parameters.AddWithValue("@revisao_item", _rev);
                command.Parameters.AddWithValue("@id_boletim", _id_boletim);
                command.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }

        }
        public void delete_boletim_todos(string _codigo, string _rev)
        {


            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                connection.Open();

                command.CommandText = "DELETE FROM tab_boletim_projeto WHERE codigo_item = @codigo_item AND revisao_item = @revisao_item";
                command.Parameters.AddWithValue("@codigo_item", _codigo);
                command.Parameters.AddWithValue("@revisao_item", _rev);
                command.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }

        }
        public static string verifica_boletim(string codigo, string revisao)
        {
            // string connString = @"Server=192.168.1.223;Database=gerenciadorprojeto;User ID=root;Password=;SslMode=none";
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                connection.Open();
                command.CommandText = "SELECT id FROM tab_boletim_projeto WHERE codigo_item = @codigo_item AND revisao_item = @revisao_item";

                command.Parameters.AddWithValue("@codigo_item", codigo);
                command.Parameters.AddWithValue("@revisao_item", revisao);

                object boletin = command.ExecuteScalar();

                if (boletin != null)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
        }
        public static string verifica_boletim_fam(string fam)
        {
            // string connString = @"Server=192.168.1.223;Database=gerenciadorprojeto;User ID=root;Password=;SslMode=none";
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                connection.Open();
                command.CommandText = "SELECT id_boletim FROM tab_fam_nx WHERE codigo = @fam ";

                command.Parameters.AddWithValue("@fam", fam);

                string boletim = command.ExecuteScalar().ToString();

                return boletim;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
        }
        public static string busca_mp_fam(string fam)
        {
            // string connString = @"Server=192.168.1.223;Database=gerenciadorprojeto;User ID=root;Password=;SslMode=none";
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                connection.Open();
                command.CommandText = "SELECT lista_mp_auto FROM tab_fam_nx WHERE codigo = @fam ";

                command.Parameters.AddWithValue("@fam", fam);

                string boletim = command.ExecuteScalar().ToString();

                return boletim;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
        }
        public DataSet busca_boletim()
        {
            string status = "1";
            DataSet ds = new DataSet();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                string query = "SELECT * FROM tab_boletim_tecnico";
                MySqlDataAdapter ad = new MySqlDataAdapter(query, connection);
                ad.Fill(ds, "tab_boletim_tecnico");
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            connection.Close(); MySqlConnection.ClearAllPools();
            return ds;
        }
        public DataSet busca_boletim_auto(string bt, string btp)
        {

            DataSet ds = new DataSet();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                string query = "SELECT * FROM tab_boletim_tecnico as bt " +
             "left join tab_boletim_cores cores on ( cores.id ='"+btp+"')" +
             " WHERE bt.id = '" + bt + "'";
                MySqlDataAdapter ad = new MySqlDataAdapter(query, connection);
                ad.Fill(ds, "tab_boletim_tecnico");
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            connection.Close(); MySqlConnection.ClearAllPools();
            return ds;
        }

        public DataSet busca_boletim_selecionado(string codigo, string revisao)
        {
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                connection.Open();
                DataSet ds = new DataSet();

                string query = "SELECT * FROM tab_boletim_projeto " +
                "inner join tab_boletim_tecnico boletim on (tab_boletim_projeto.id_boletim = boletim.id)" +
                "left join tab_boletim_cores cores on (tab_boletim_projeto.id_pintura = cores.id)" +
                " WHERE codigo_item = '" + codigo + "' AND revisao_item = '" + revisao + "'";
                MySqlDataAdapter ad = new MySqlDataAdapter(query, connection);
                ad.Fill(ds, "tab_boletim_projeto");
                return ds;

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
        }
        
        public static string select_desc(string codigo)
        {

            string desc = "";


            List<int> rev = new List<int>();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                command.CommandText = "SELECT descricao_padrao FROM tab_fam_nx " +
                     "where codigo ='" + codigo + "'";

                connection.Open();

                MySqlDataReader dr;
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    desc = dr["descricao_padrao"].ToString();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            return desc;
        }
        public static string select_rev(string codigo)
        {

            string id_ret = "";


            List<int> rev = new List<int>();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                command.CommandText = "SELECT cad.revisao FROM cadastroitem cad " +
                     "where codigo ='" + codigo + "'ORDER BY revisao desc LIMIT 0, 1";

                connection.Open();

                MySqlDataReader dr;
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    id_ret = dr["revisao"].ToString();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            return id_ret;
        }
        public DataSet dados_conexao()
        {
            string status = "1";
            DataSet ds = new DataSet();

            List<int> rev = new List<int>();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                string query = "SELECT * FROM tab_checkin_exportacao exp" +
                    " ORDER BY exp.id DESC LIMIT 1";
                MySqlDataAdapter ad = new MySqlDataAdapter(query, connection);
                ad.Fill(ds, "tab_checkin_exportacao");
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            connection.Close(); MySqlConnection.ClearAllPools();
            return ds;
        }
        public DataSet dados_pedido(string id_orcamento)
        {
            string connectionStringDb1 = connString;
            string connectionStringDb2 = connString.Replace("gerenciadorprojeto", "sim");// @"Server=10.1.1.134;Database=sim;User ID=renato_bd;Password=QHXqr0LldujNHDEz;SslMode=none;Connect Timeout=30000";
            DataSet dataSet = new DataSet();
            // Criar as conexões para os dois bancos de dados
            using (MySqlConnection connectionDb1 = new MySqlConnection(connString))
            using (MySqlConnection connectionDb2 = new MySqlConnection(connectionStringDb2))
            {
                try
                {
                    // Abrir as conexões
                    connectionDb1.Open();
                    connectionDb2.Open();
                    string query = "SELECT car.carr_titulo, cha.cha_titulo, comp.com_titulo, ee.ent_titulo, norma.nor_titulo, dados.bd, dados.p0_bd FROM `tab_orcamentos` " +
                                   "Inner JOIN tab_orcamentos_carro On orc_pk_carro = tab_orcamentos_carro.ocar_id "+
                                   "Inner JOIN tab_carroceria car On car.carr_id = tab_orcamentos_carro.ocar_pk_carroceria "+
                                   "Inner JOIN tab_chassi cha On cha.cha_id = tab_orcamentos_carro.ocar_pk_chassi "+
                                   "Inner JOIN eng_comprimentos comp On comp.com_id = tab_orcamentos_carro.ocar_comprimento "+
                                   "Inner JOIN eng_entreeixos ee On ee.ent_id = tab_orcamentos_carro.ocar_chassi_entreeixo "+
                                   "Inner JOIN tab_normas norma On norma.nor_id = tab_orcamentos_carro.ocar_pk_norma "+
                                   "Inner JOIN gerenciadorprojeto.tab_pb_dados_chassi AS dados On dados.pk_carroceria = tab_orcamentos_carro.ocar_pk_carroceria " +
                                   "AND  dados.pk_chassi = tab_orcamentos_carro.ocar_pk_chassi " +
                                   "where orc_id = '" + id_orcamento + "'";

                  

                    // Criar o adaptador de dados para a consulta
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connectionDb2);

                    // Criar um conjunto de dados para armazenar os resultados


                    // Preencher o conjunto de dados com os resultados da consulta
                    dataAdapter.Fill(dataSet, "Resultado");

                }
                finally
                {
                    if (connectionDb2.State == ConnectionState.Open)
                        connectionDb2.Close(); MySqlConnection.ClearAllPools();
                    if (connectionDb1.State == ConnectionState.Open)
                        connectionDb1.Close(); MySqlConnection.ClearAllPools();
                }
            }
            return dataSet;
        }
        public static string ReturnIDColor(string NameColor)
        {
            var Conexão = new MySqlConnection(connString);
            var ComandoSQL = Conexão.CreateCommand();
            string RetornoRevisão = "";

            try
            {
                Conexão.Open();
                ComandoSQL.CommandText = "SELECT id FROM tab_boletim_cores WHERE info_color = '" + NameColor + "'";
                ComandoSQL.ExecuteNonQuery();
                MySqlDataReader dr;
                dr = ComandoSQL.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    RetornoRevisão = dr["id"].ToString();
                }
            }
            finally
            {
                if (Conexão.State == ConnectionState.Open)
                    Conexão.Close(); MySqlConnection.ClearAllPools();
            }
            return RetornoRevisão;
        }

        public DataSet ColorSearch(string ColorType)
        {
            var Conexão = new MySqlConnection(connString);
            var ComandoSQL = Conexão.CreateCommand();
            DataSet CoresEncontradas = new DataSet();

            try
            {
                Conexão.Open();
                string query = "SELECT * FROM tab_boletim_cores WHERE tipo = '" + ColorType + "'";
                MySqlDataAdapter adpter = new MySqlDataAdapter(query, Conexão);
                adpter.Fill(CoresEncontradas, "cadastroitem");
            }
            finally
            {
                if (Conexão.State == ConnectionState.Open)
                    Conexão.Close(); MySqlConnection.ClearAllPools();
            }
            return CoresEncontradas;
        }
        public DataSet lista_bol_cores()
        {
            var Conexão = new MySqlConnection(connString);
            var ComandoSQL = Conexão.CreateCommand();
            DataSet CoresEncontradas = new DataSet();

            try
            {
                Conexão.Open();
                string query = "SELECT * FROM tab_boletim_cores";
                MySqlDataAdapter adpter = new MySqlDataAdapter(query, Conexão);
                adpter.Fill(CoresEncontradas, "tab_boletim_cores");
            }
            finally
            {
                if (Conexão.State == ConnectionState.Open)
                    Conexão.Close(); MySqlConnection.ClearAllPools();
            }
            return CoresEncontradas;
        }
        public static string UpdateColor(string Código, string Revisão, string Boletim, string Pintura)
        {
            var Conexão = new MySqlConnection(connString);
            var ComandoSQL = Conexão.CreateCommand();

            try
            {
                Conexão.Open();
                string query = "UPDATE tab_boletim_projeto SET id_pintura = '" + Pintura + "' WHERE codigo_item = '" + Código + "' AND revisao_item = '" + Revisão + "' AND id_boletim = '" + Boletim + "'";
                ComandoSQL.CommandText = query;
                ComandoSQL.ExecuteNonQuery();
            }
            finally
            {
                if (Conexão.State == ConnectionState.Open)
                    Conexão.Close(); MySqlConnection.ClearAllPools();
            }
            return "OK";
        }
        public static string up_fam_cv(string Código, string fam_cv, string classificacao)
        {
            var Conexão = new MySqlConnection(connString);
            var ComandoSQL = Conexão.CreateCommand();

            try
            {
                Conexão.Open();
                string query = "UPDATE tab_fam_nx SET fam_cv = '" + fam_cv + "', classificacao= '" + classificacao + "' WHERE codigo = '" + Código  + "'";
                ComandoSQL.CommandText = query;
                ComandoSQL.ExecuteNonQuery();
            }
            finally
            {
                if (Conexão.State == ConnectionState.Open)
                    Conexão.Close(); MySqlConnection.ClearAllPools();
            }
            return "OK";
        }
        
        public static string gravar_alteracao(string codigo, string rev, string data, string resp_rev, string resp_proj, string tipo, string desc_rev, string coment, string motivo, string inf_pcp, string gravidade, string equipe)
        {
            string alt_r66 = "Não";
            //       string connString = @"Server=192.168.1.223;Database=gerenciadorprojeto;User ID=root;Password=;SslMode=none";
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();


            connection.Open();
            //
            ///   MessageBox.Show(codigo);

            //try
            //{
            command.CommandText = "INSERT INTO tab_alteracoes (codigo, rev,data, resp_rev, resp_proj, tipo, desc_rev,coment, motivo, inf_pcp, gravidade, equipe , alt_r66) " +
                "VALUES ('" + codigo + "','" + rev + "','" + data + "','" + resp_rev + "','" + resp_proj + "','" + tipo + "','" + desc_rev + "','" + coment + "','" + motivo + "','" + inf_pcp + "','" + gravidade + "','" + equipe + "','" + alt_r66 + "')";

            command.ExecuteNonQuery();



            connection.Close(); MySqlConnection.ClearAllPools();

            return "ok";

            //command.CommandText = "INSERT INTO historicoworkflow (id_cadastrowf,revisao,acao,usuario_resp,	data,coment) " +
            //    "VALUES ('" + codigo + "','" + revisao + "','" + acao + "','" + user + "','" + data_acao + "','" + coment + "')";
        }
        public DataSet select_sol_fam()
        {
            string status = "1";
            DataSet ds = new DataSet();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                string query = "SELECT * FROM tab_solicitacao_item_fam_nx WHERE status = '1'";
                MySqlDataAdapter ad = new MySqlDataAdapter(query, connection);
                ad.Fill(ds, "tab_solicitacao_item_fam_nx");
            }
            catch
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            connection.Close(); MySqlConnection.ClearAllPools();
            return ds;
        }
        public static string update_criacao(string id)
        {
            var Conexão = new MySqlConnection(connString);
            var ComandoSQL = Conexão.CreateCommand();

            try
            {
                Conexão.Open();
                string query = "UPDATE tab_solicitacao_item_fam_nx SET status = '0' WHERE id = '" + id + "'";
                ComandoSQL.CommandText = query;
                ComandoSQL.ExecuteNonQuery();
            }
            finally
            {
                if (Conexão.State == ConnectionState.Open)
                    Conexão.Close(); MySqlConnection.ClearAllPools();
            }
            return "OK";
        }

        public static string dados_atributos(string fam)
        {
            // string connString = @"Server=192.168.1.223;Database=gerenciadorprojeto;User ID=root;Password=;SslMode=none";
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                connection.Open();
                command.CommandText = "SELECT atributos_teamcenter FROM tab_fam_nx WHERE codigo = @fam ";

                command.Parameters.AddWithValue("@fam", fam);

                string attr = command.ExecuteScalar().ToString();

                return attr;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
        }
        public static string dados_atributos_1(string fam)
        {
            // string connString = @"Server=192.168.1.223;Database=gerenciadorprojeto;User ID=root;Password=;SslMode=none";
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                connection.Open();
                command.CommandText = "SELECT atributos_teamcenter FROM tab_fam_nx WHERE codigo = @fam ";

                command.Parameters.AddWithValue("@fam", fam);

                string attr = command.ExecuteScalar().ToString();

                return attr;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
        }

        public static string busca_limites(string codigo)
        {
            // string connString = @"Server=192.168.1.223;Database=gerenciadorprojeto;User ID=root;Password=;SslMode=none";
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();

            try
            {
                connection.Open();
                command.CommandText = "SELECT limites FROM tab_fam_nx WHERE codigo=@codigo";

                command.Parameters.AddWithValue("@codigo", codigo);

                object limites = command.ExecuteScalar();

               
                if (limites != null)
                {
                    return limites.ToString();
                }
                else
                {
                    return "0";
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
        }

        public static string select_user_admin(string nome)
        {

            string id_ret = "";

            // 

            List<int> rev = new List<int>();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                command.CommandText = "SELECT * FROM tab_usuarios where user ='" + nome + "'ORDER BY user ASC"; //sql += "Select Nome, Endereco from Clientes ";

                connection.Open();

                MySqlDataReader dr;
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    id_ret = dr["admin"].ToString();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            return id_ret;
        }
        public static string busca_bd(string chassi)
        {

            string bd = "";

            // 

            List<int> rev = new List<int>();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                connection.Open();
                command.CommandText = "SELECT bd FROM tab_nx_templates where chassi = @chassi"; //sql += "Select Nome, Endereco from Clientes ";
                command.Parameters.AddWithValue("@chassi", chassi);

                object _id = command.ExecuteScalar();

                if (_id != null)
                {
                    bd = _id.ToString();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            return bd;
        }
        public static string busca_json(string chassi)
        {

            string bd = "";

            // 

            List<int> rev = new List<int>();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                connection.Open();
                command.CommandText = "SELECT itens FROM tab_nx_templates where chassi = @chassi"; //sql += "Select Nome, Endereco from Clientes ";
                command.Parameters.AddWithValue("@chassi", chassi);

                object _id = command.ExecuteScalar();

                if (_id != null)
                {
                    bd = _id.ToString();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            return bd;
        }
        public static string busca_atributos_autos(string fam)
        {

            string bd = "";

            List<int> rev = new List<int>();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                connection.Open();
                command.CommandText = "SELECT atributos_autos FROM tab_fam_nx where codigo=@codigo"; //sql += "Select Nome, Endereco from Clientes ";
                command.Parameters.AddWithValue("@codigo", fam);

                object _id = command.ExecuteScalar();

                if (_id != null)
                {
                    bd = _id.ToString();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            return bd;
        }
        public static string busca_subfam_json(string chassi)
        {

            string bd = "";

            // 

            List<int> rev = new List<int>();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                connection.Open();
                command.CommandText = "SELECT sub_fam FROM tab_fam_nx where codigo=@codigo"; //sql += "Select Nome, Endereco from Clientes ";
                command.Parameters.AddWithValue("@codigo", chassi);

                object _id = command.ExecuteScalar();

                if (_id != null)
                {
                    bd = _id.ToString();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            return bd;
        }
        public static string busca_atributos_json(string fam)
        {

            string bd = "";

            // 

            List<int> rev = new List<int>();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                connection.Open();
                command.CommandText = "SELECT atributos_selecao FROM tab_fam_nx where codigo=@codigo"; //sql += "Select Nome, Endereco from Clientes ";
                command.Parameters.AddWithValue("@codigo", fam);

                object _id = command.ExecuteScalar();

                if (_id != null)
                {
                    bd = _id.ToString();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            return bd;
        }
        public static string itens_add(string template)
        {

            string itens = "";

            // 

            List<int> rev = new List<int>();
            var connection = new MySqlConnection(connString);
            var command = connection.CreateCommand();
            try
            {
                connection.Open();
                command.CommandText = "SELECT lista_itens FROM tab_nx_templates where Template = @template"; //sql += "Select Nome, Endereco from Clientes ";
                command.Parameters.AddWithValue("@template", template);

                object _id = command.ExecuteScalar();

                if (_id != null)
                {
                    itens = _id.ToString();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); MySqlConnection.ClearAllPools();
            }
            return itens;
        }
        
    }
}
