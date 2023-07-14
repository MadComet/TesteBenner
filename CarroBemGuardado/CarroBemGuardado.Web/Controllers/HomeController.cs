using CarroBemGuardado.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarroBemGuardado.Web.Controllers
{
    public class HomeController : Controller
    {

        private static List<EstacionamentoModel> _listaVagas = new List<EstacionamentoModel>()
        {
            new EstacionamentoModel() { Id=1, Placa="AAA1234", HoraChegada=DateTime.MinValue, HoraSaida=DateTime.MaxValue, Duracao=DateTime.Now, TempoCobrado=1, Preco=2.00m, ValorTotal=2.00m }
        };

        public ActionResult Index()
        {
            return View(_listaVagas);
            //return View(EstacionamentoModel.RecuperarLista());
        }

        [HttpPost]
        public ActionResult RecuperarVagaEstacionamento(int id)
        {
            return Json(EstacionamentoModel.RecuperarPeloId(id));
        }

        [HttpPost]
        public ActionResult SalvarVagaEstacionamento(EstacionamentoModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var id = model.Salvar();
                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                }
                catch (Exception err)
                {
                    resultado = "ERRO";
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}