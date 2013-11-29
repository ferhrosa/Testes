using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesteMvc.Models;

namespace TesteMvc.Areas.Cadastros.Controllers
{
    public class ProdutoController : Controller
    {
        //
        // GET: /Cadastros/Produto/

        public ActionResult Index()
        {
            ViewBag.Produtos = Produto.Produtos;

            return View();
        }

        [HttpGet]
        public ActionResult Cadastro(long? id = null)
        {
            return View(id.HasValue ? Produto.Carregar(id.Value) : new Produto());
        }

        [HttpPost]
        public ActionResult Cadastro(long? id, Produto produto)
        {
            if ( id.HasValue && id.Value > 0 )
                Produto.Alterar(produto);
            else
                Produto.Criar(produto);

            return RedirectToAction("Index");
        }



    }
}
