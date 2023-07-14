using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace CarroBemGuardado.Web.Models
{
    public class EstacionamentoModel
    {
        public int Id { get; set; }
        public string Placa { get; set; }
        public DateTime HoraChegada { get; set; }
        public DateTime HoraSaida { get; set; }
        public DateTime Duracao { get; set; }
        public int TempoCobrado { get; set; }
        public decimal Preco { get; set; }
        public decimal ValorTotal { get; set; }

        public static List<EstacionamentoModel> RecuperarLista()
        {
            var ret = new List<EstacionamentoModel>();

            using (var conexao = new SQLiteConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SQLiteCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from tb_controle_vagas order by ID";
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new EstacionamentoModel
                        {
                            Id = (int)reader["ID"],
                            Placa = (string)reader["PLACA"],
                            HoraChegada = (DateTime)reader["HORA_CHEGADA"],
                            HoraSaida = (DateTime)reader["HORA_SAIDA"],
                            Duracao = (DateTime)reader["DURACAO"],
                            TempoCobrado = (int)reader["TEMPO_CCOBRADO"],
                            Preco = (decimal)reader["PRECO"],
                            ValorTotal = (decimal)reader["VALOR_TOTAL"]
                        });
                    }
                }
            }

            return ret;
        }

        public static EstacionamentoModel RecuperarPeloId(int id)
        {
            EstacionamentoModel ret = null;

            using (var conexao = new SQLiteConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SQLiteCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from tb_controle_vagas where (ID = {0})", id);
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new EstacionamentoModel
                        {
                            Id = (int)reader["ID"],
                            Placa = (string)reader["PLACA"],
                            HoraChegada = (DateTime)reader["HORA_CHEGADA"],
                            HoraSaida = (DateTime)reader["HORA_SAIDA"],
                            Duracao = (DateTime)reader["DURACAO"],
                            TempoCobrado = (int)reader["TEMPO_CCOBRADO"],
                            Preco = (decimal)reader["PRECO"],
                            ValorTotal = (decimal)reader["VALOR_TOTAL"]
                        };
                    }
                }
            }

            return ret;
        }

        public int Salvar()
        {
            var ret = 0;

            var model = RecuperarPeloId(this.Id);

            using (var conexao = new SQLiteConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SQLiteCommand())
                {
                    comando.Connection = conexao;

                    if (model == null)
                    {
                        comando.CommandText = string.Format("insert into tb_controle_vagas (PLACA, HORA_CHEGADA, HORA_SAIDA, DURACAO, TEMPO_COBRADO, PRECO, VALOR_TOTAL) values ('{0}', {1}, {2}, {3}, {4}, {5}, {6}, {7}); select convert(int, scope_identity())", this.Placa, this.HoraChegada, this.HoraSaida, this.Duracao, this.TempoCobrado, this.Preco, this.ValorTotal);
                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = string.Format(
                            "update tb_controle_vagas set PLACA='{1}', HORA_CHEGADA={2},  HORA_SAIDA={3},  DURACAO={4},  TEMPO_COBRADO={5}, PRECO={6}, VALOR_TOTAL={7} where ID = {0}",
                            this.Id, this.Placa, this.HoraChegada, this.HoraSaida, this.Duracao, this.TempoCobrado, this.Preco, this.ValorTotal );
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