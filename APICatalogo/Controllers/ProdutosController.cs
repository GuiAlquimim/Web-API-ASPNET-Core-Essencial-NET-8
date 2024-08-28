using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
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

            if (!produtos.Any())
                return NotFound();

            var produtosDto = produtos.ToProdutoDTOList();

            return Ok(produtosDto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> GetAll()
        {
            var produtos = _uof.ProdutoRepository.GetAll();

            if (produtos is null)
                return NotFound();

            var produtosDto = produtos.ToProdutoDTOList();

            return Ok(produtosDto);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound($"Produto com Id {id} não encontrado...");

            var produtoDTO = produto.ToProdutoDTO();

            return Ok(produtoDTO);
        }

        [HttpPost]
        public ActionResult<ProdutoDTO> Insert(ProdutoDTO produtoDTO)
        {
            // Http 201: Created

            var produto = produtoDTO.ToProduto();

            if (produto is null)
                return BadRequest();

            _uof.ProdutoRepository.Create(produto);
            _uof.Commit();

            var novoProdutoDTO = produto.ToProdutoDTO();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProdutoDTO.ProdutoId }, novoProdutoDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Update(int id, ProdutoDTO produtoDTO)
        {
            // Http 204: No content

            var produto = produtoDTO.ToProduto();

            if (produto is null || id != produto.ProdutoId)
                return BadRequest();

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            var produtoAtualizadoDto = produto.ToProdutoDTO();

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

            var produtoDeletadoDto = produto.ToProdutoDTO();

            return Ok(produtoDeletadoDto);
        }
    }
}