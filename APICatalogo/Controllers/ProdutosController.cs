using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("ProdutosPorCategoria/{id:int}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPorCategoria(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

            if (!produtos.Any())
                return NotFound($"Nenhum produto da categoria com id {id} encontrado.");

            // var destino = _mapper.Map<destino>(origem);
            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> GetAll()
        {
            var produtos = _uof.ProdutoRepository.GetAll();

            if (produtos is null)
                return NotFound("Nenhum produto encontrado.");

            // var destino = _mapper.Map<destino>(origem);
            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound($"Produto com Id {id} não encontrado...");

            // var destino = _mapper.Map<destino>(origem);
            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
        }

        [HttpPost]
        public ActionResult<ProdutoDTO> Insert(ProdutoDTO produtoDTO)
        {
            // Http 201: Created

            // var destino = _mapper.Map<destino>(origem);
            var produto = _mapper.Map<Produto>(produtoDTO);

            if (produto is null)
                return BadRequest();

            _uof.ProdutoRepository.Create(produto);
            _uof.Commit();

            // var destino = _mapper.Map<destino>(origem);
            var novoProdutoDTO = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProdutoDTO.ProdutoId }, novoProdutoDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Update(int id, ProdutoDTO produtoDTO)
        {
            // Http 204: No content

            // var destino = _mapper.Map<destino>(origem);
            var produto = _mapper.Map<Produto>(produtoDTO);

            if (produto is null || id != produto.ProdutoId)
                return BadRequest();

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            // var destino = _mapper.Map<destino>(origem);
            var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produto);

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

            // var destino = _mapper.Map<destino>(origem);
            var produtoDeletadoDto = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDeletadoDto);
        }
    }
}