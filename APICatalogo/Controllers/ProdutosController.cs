﻿using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        //private readonly IRepository<Produto> _repository;
        //private readonly IProdutoRepository _produtoRepository;
        private readonly IUnitOfWork _uof;

        public ProdutosController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet("ProdutosPorCategoria/{id:int}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPorCategoria(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

            if (produtos is null)
                return NotFound();

            var produtosDto = new List<ProdutoDTO>();
            foreach (var produto in produtos)
            {
                var produtoDto = new ProdutoDTO();

                produtoDto.ProdutoId = produto.ProdutoId;
                produtoDto.Nome = produto.Nome;
                produtoDto.Descricao = produto.Descricao;
                produtoDto.Preco = produto.Preco;
                produtoDto.ImagemUrl = produto.ImagemUrl;
                produtoDto.Estoque = produto.Estoque;
                produtoDto.CategoriaId = produto.CategoriaId;

                produtosDto.Add(produtoDto);
            }

            return Ok(produtosDto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> GetAll()
        {
            var produtos = _uof.ProdutoRepository.GetAll();

            if (produtos is null)
                return NotFound();

            var produtosDto = new List<ProdutoDTO>();
            foreach (var produto in produtos)
            {
                var produtoDto = new ProdutoDTO();

                produtoDto.ProdutoId = produto.ProdutoId;
                produtoDto.Nome = produto.Nome;
                produtoDto.Descricao = produto.Descricao;
                produtoDto.Preco = produto.Preco;
                produtoDto.ImagemUrl = produto.ImagemUrl;
                produtoDto.Estoque = produto.Estoque;
                produtoDto.CategoriaId = produto.CategoriaId;

                produtosDto.Add(produtoDto);
            }

            return Ok(produtosDto);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound($"Produto com Id {id} não encontrado...");

            var produtoDto = new ProdutoDTO()
            {
                ProdutoId = produto.ProdutoId,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                ImagemUrl = produto.ImagemUrl,
                Estoque = produto.Estoque,
                CategoriaId = produto.CategoriaId
            };

            return Ok(produtoDto);
        }

        [HttpPost]
        public ActionResult<ProdutoDTO> Insert(ProdutoDTO produtoDto)
        {
            // Http 201: Created

            var produto = new Produto()
            {
                ProdutoId = produtoDto.ProdutoId,
                Nome = produtoDto.Nome,
                Descricao = produtoDto.Descricao,
                Preco = produtoDto.Preco,
                ImagemUrl = produtoDto.ImagemUrl,
                Estoque = produtoDto.Estoque,
                CategoriaId = produtoDto.CategoriaId
            };

            if (produto is null)
                return BadRequest();

            _uof.ProdutoRepository.Create(produto);
            _uof.Commit();

            var novoProdutoDto = new ProdutoDTO()
            {
                ProdutoId = produto.ProdutoId,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                ImagemUrl = produto.ImagemUrl,
                Estoque = produto.Estoque,
                CategoriaId = produto.CategoriaId
            };

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Update(int id, ProdutoDTO produtoDto)
        {
            // Http 204: No content

            var produto = new Produto()
            {
                ProdutoId = produtoDto.ProdutoId,
                Nome = produtoDto.Nome,
                Descricao = produtoDto.Descricao,
                Preco = produtoDto.Preco,
                ImagemUrl = produtoDto.ImagemUrl,
                Estoque = produtoDto.Estoque,
                CategoriaId = produtoDto.CategoriaId
            };

            if (id != produto.ProdutoId)
                return BadRequest();

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            var produtoAtualizadoDto = new ProdutoDTO()
            {
                ProdutoId = produto.ProdutoId,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                ImagemUrl = produto.ImagemUrl,
                Estoque = produto.Estoque,
                CategoriaId = produto.CategoriaId
            };

            return Ok(produtoAtualizadoDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound($"Produto com Id {id} não encontrado...");

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            var produtoDeletadoDto = new ProdutoDTO()
            {
                ProdutoId = produto.ProdutoId,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                ImagemUrl = produto.ImagemUrl,
                Estoque = produto.Estoque,
                CategoriaId = produto.CategoriaId
            };

            return Ok(produtoDeletadoDto);
        }
    }
}