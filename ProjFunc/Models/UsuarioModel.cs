﻿using System;
using ProjFunc.Helper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace ProjFunc.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe o login")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Informe o senha")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Informe o nome")]
        public string Nome { get; set; }

        public static UsuarioModel ValidarUsuario(string login, string senha)
        {
            UsuarioModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from usuario where login=@login and senha=@senha";

                    comando.Parameters.Add("@login", SqlDbType.VarChar).Value = login;
                    comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(senha);

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new UsuarioModel
                        {
                            Id = (int)reader["id"],
                            Login = (string)reader["login"],
                            Senha = (string)reader["senha"],
                            Nome = (string)reader["nome"]
                        };
                    }
                }
            }

            return ret;
        }

        public static List<UsuarioModel> RecuperarLista()
        {
            var ret = new List<UsuarioModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from usuario order by nome";
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new UsuarioModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Login = (string)reader["login"]
                        });
                    }
                }
            }

            return ret;
        }

        public static UsuarioModel RecuperarPeloId(int id)
        {
            UsuarioModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from usuario where (id = @id)";

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new UsuarioModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Login = (string)reader["login"]
                        };
                    }
                }
            }

            return ret;
        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            if (RecuperarPeloId(id) != null)
            {
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;
                        comando.CommandText = "delete from usuario where (id = @id)";

                        comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                        ret = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }

            return ret;
        }

        public int Salvar()
        {
            var ret = 0;

            var model = RecuperarPeloId(this.Id);

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    if (model == null)
                    {
                        comando.CommandText = "insert into usuario (nome, login, senha) values (@nome, @login, @senha); select convert(int, scope_identity())";

                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@login", SqlDbType.VarChar).Value = this.Login;
                        comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(this.Senha);

                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText =
                            "update usuario set nome=@nome, login=@login" +
                            (!string.IsNullOrEmpty(this.Senha) ? ", senha=@senha" : "") +
                            " where id = @id";

                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@login", SqlDbType.VarChar).Value = this.Login;

                        if (!string.IsNullOrEmpty(this.Senha))
                        {
                            comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(this.Senha);
                        }

                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;

                        if (comando.ExecuteNonQuery() > 0)
                        {
                            ret = this.Id;
                        }
                    }
                }
            }

            return ret;
        }
    }
}