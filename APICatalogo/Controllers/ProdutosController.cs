﻿using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        //private readonly IRepository<Produto> _repository;
        private readonly IProdutoRepository _produtoRepository;

        public ProdutosController(/*IRepository<Produto> repository,*/ IProdutoRepository produtoRepository)
        {
            //_repository = repository;
            _produtoRepository = produtoRepository;
        }

        [HttpGet("ProdutosPorCategoria/{id:int}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPorCategoria(int id)
        { 
            var produtos = _produtoRepository.GetProdutosPorCategoria(id);

            if (produtos is null)
                return NotFound();

            return Ok(produtos);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetAll()
        {
            var produtos = _produtoRepository.GetAll();

            if (produtos is null)
                return NotFound();

            return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _produtoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound($"Produto com Id {id} não encontrado...");

            return Ok(produto);
        }

        [HttpPost]
        public ActionResult<Produto> Insert(Produto produto)
        {
            // Http 201: Created

            if (produto is null)
                return BadRequest();

            var novoProduto = _produtoRepository.Create(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProduto.ProdutoId }, novoProduto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Produto> Update(int id, Produto produto)
        {
            // Http 204: No content

            if (id != produto.ProdutoId)
                return BadRequest();

            var produtoAtualizado = _produtoRepository.Update(produto);

            return Ok(produtoAtualizado);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _produtoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound($"Produto com Id {id} não encontrado...");

            _produtoRepository.Delete(produto);

            return Ok(produto);
        }
    }
}