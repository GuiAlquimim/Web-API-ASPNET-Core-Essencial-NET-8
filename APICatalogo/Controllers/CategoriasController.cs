using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        //private readonly ICategoriaRepository _repository;
        //private readonly IRepository<Categoria> _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger, IMapper mapper)
        {
            _uof = uof;
            _logger = logger;
            _mapper = mapper;
        }

        /*[HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation("================= GET api/categorias/produtos =================");

            return _context.Categorias.Include(p => p.Produtos).ToList();
        }
        */

        [HttpGet("Pagination")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasPagination([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasPaginationAsync(categoriasParameters);

            var categoriasDTO = ObterCategoriasPagination(categorias);

            return Ok(categoriasDTO);
        } 

        [HttpGet("Filter/Nome/Pagination")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasFilterNomePagination([FromQuery] CategoriasFiltroNome categoriasFiltroNome)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasFiltroNomeAsync(categoriasFiltroNome);

            var categoriasFiltradasDTO = ObterCategoriasPagination(categorias);

            return Ok(categoriasFiltradasDTO);
        }

        [HttpGet]
        [Authorize]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAll()
        {
            _logger.LogInformation("================= GET api/categorias/produtos =================");

            var categorias = await _uof.CategoriaRepository.GetAllAsync();

            if (categorias is null || !categorias.Any())
                return NotFound("Nenhuma categoria encontrada.");

            // var destino = _mapper.Map<destino>(origem);
            var categoriasDTO = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            return Ok(categoriasDTO);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            _logger.LogInformation($"================= GET api/categorias/id = {id} =================");

            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogInformation($"================= GET api/categorias/id = {id} NOT FOUND =================");
                return NotFound($"Categoria com Id {id} não encontrada");
            }

            // var destino = _mapper.Map<destino>(origem);
            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDTO);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Insert(CategoriaDTO categoriaDTO)
        {
            // var destino = _mapper.Map<destino>(origem);
            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            if (categoria is null)
                return BadRequest();

            _uof.CategoriaRepository.Create(categoria);
            await _uof.CommitAsync();

            // var destino = _mapper.Map<destino>(origem);
            var novaCategoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            // retornar http 201 - Created
            return new CreatedAtRouteResult("ObterCategoria",
               new { id = novaCategoriaDTO.CategoriaId }, novaCategoriaDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Update(int id, CategoriaDTO categoriaDTO)
        {
            // var destino = _mapper.Map<destino>(origem);
            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            if (categoria is null || id != categoria.CategoriaId)
                return BadRequest();

            _uof.CategoriaRepository.Update(categoria);
            await _uof.CommitAsync();

            // var destino = _mapper.Map<destino>(origem);
            var CategoriaAtualizadaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(CategoriaAtualizadaDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com o Id {id} não encontrada...");
                return NotFound($"Categoria com o Id {id} não encontrada...");
            }

            _uof.CategoriaRepository.Delete(categoria);
            await _uof.CommitAsync();

            // var destino = _mapper.Map<destino>(origem);
            var CategoriaDeletadaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(CategoriaDeletadaDTO);
        }

        private IEnumerable<CategoriaDTO>? ObterCategoriasPagination(IPagedList<Categoria> categorias)
        {
            var metadados = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadados));

            var categoriasDTO = categorias.ToCategoriaDTOList();
            return categoriasDTO;
        }

    }
}