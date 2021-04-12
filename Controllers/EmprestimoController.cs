using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Biblioteca.Controllers
{

    public class EmprestimoController : Controller
    {
        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);

            if (HttpContext.Session.GetString("Nivel") == "Usuario")
            {
                return RedirectToAction("Login", "Home");
            }

            LivroService livroService = new LivroService();
            EmprestimoService emprestimoService = new EmprestimoService();

            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();
            cadModel.Livros = livroService.ListarTodos();
            return View(cadModel);
        }

        [HttpPost]
        public IActionResult Cadastro(CadEmprestimoViewModel viewModel)
        {
            EmprestimoService emprestimoService = new EmprestimoService();

            if (viewModel.Emprestimo.Id == 0)
            {
                emprestimoService.Inserir(viewModel.Emprestimo);
            }
            else
            {
                emprestimoService.Atualizar(viewModel.Emprestimo);
            }
            return RedirectToAction("Listagem");
        }


        [HttpGet]

        public IActionResult Listagem(int p = 1)
        {

            Autenticacao.CheckLogin(this);

            if (HttpContext.Session.GetString("Nivel") == "Usuario")
            {
                return RedirectToAction("Login", "Home");
            }

            int quantidadePorPagina = 10;

            EmprestimoService em = new EmprestimoService();

            ICollection<Emprestimo> emprestimo = em.Lista(p, quantidadePorPagina);

            int quantidadeRegistros = em.CountEmprestimo();

            ViewData["Paginas"] = (int)Math.Ceiling((double)quantidadeRegistros / quantidadePorPagina);


            return View(emprestimo);
        }

        [HttpPost]

        public IActionResult Listagem(string tipoFiltro, string filtro)
        {
            Autenticacao.CheckLogin(this);

            if (HttpContext.Session.GetString("Nivel") == "Usuario")
            {
                return RedirectToAction("Login", "Home");
            }

            FiltrosEmprestimos objFiltro = null;
            if (!string.IsNullOrEmpty(filtro))
            {
                objFiltro = new FiltrosEmprestimos();
                objFiltro.Filtro = filtro;
                objFiltro.TipoFiltro = tipoFiltro;
            }
            EmprestimoService emprestimoService = new EmprestimoService();
            ICollection<Emprestimo> emprestimo = emprestimoService.ListarTodos(objFiltro);

            if (emprestimo.Count == 0)
            {
                ViewData["MensagemE"] = "Nenhum registro encontrado";
            }

            return View(emprestimo);
        }

        public IActionResult Edicao(int id)
        {
            Autenticacao.CheckLogin(this);

            if (HttpContext.Session.GetString("Nivel") == "Usuario")
            {
                return RedirectToAction("Login", "Home");
            }

            LivroService livroService = new LivroService();
            EmprestimoService em = new EmprestimoService();
            Emprestimo e = em.ObterPorId(id);

            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();
            cadModel.Livros = livroService.ListarTodos();
            cadModel.Emprestimo = e;

            return View(cadModel);
        }
    }
}