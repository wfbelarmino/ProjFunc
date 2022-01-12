using System.Data.SqlClient;


namespace ProjFunc.Models
{
    public class UsuarioModel
    {
        public static bool ValidarUsuario(string login, string senha) 
        {
            var ret = false;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = @"Data Source=DESKTOP-O75EG1C\OLAP;Initial Catalog=CadastroBD;User Id=admin;Password=123admin";
                conexao.Open();
                using (var comando = new SqlCommand()) //comando de conexão ao banco de dados
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                        "select count(*) from usuario where login='{0}' and senha='{1}'", login, senha);
                    ret = ((int)comando.ExecuteScalar() > 0);
                }
            }

            return ret;
        }
    }
}