using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public ActionResult<IEnumerable<Categoria>> GetProdutosPagination([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = _uof.CategoriaRepository.GetCategoriasPagination(categoriasParameters);

            var metadados = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadados));

            var categoriasDTO = categorias.ToCategoriaDTOList();

            return Ok(categoriasDTO);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> GetAll()
        {
            _logger.LogInformation("================= GET api/categorias/produtos =================");

            var categorias = _uof.CategoriaRepository.GetAll();

            if (categorias is null || !categorias.Any())
                return NotFound("Nenhuma categoria encontrada.");

            // var destino = _mapper.Map<destino>(origem);
            var categoriasDTO = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            return Ok(categoriasDTO);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            _logger.LogInformation($"================= GET api/categorias/id = {id} =================");

            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

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
        public ActionResult<CategoriaDTO> Insert(CategoriaDTO categoriaDTO)
        {
            // var destino = _mapper.Map<destino>(origem);
            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            if (categoria is null)
                return BadRequest();

            _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            // var destino = _mapper.Map<destino>(origem);
            var novaCategoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            // retornar http 201 - Created
            return new CreatedAtRouteResult("ObterCategoria",
               new { id = novaCategoriaDTO.CategoriaId }, novaCategoriaDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Update(int id, CategoriaDTO categoriaDTO)
        {
            // var destino = _mapper.Map<destino>(origem);
            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            if (categoria is null || id != categoria.CategoriaId)
                return BadRequest();

            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            // var destino = _mapper.Map<destino>(origem);
            var CategoriaAtualizadaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(CategoriaAtualizadaDTO);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com o Id {id} não encontrada...");
                return NotFound($"Categoria com o Id {id} não encontrada...");
            }

            _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            // var destino = _mapper.Map<destino>(origem);
            var CategoriaDeletadaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(CategoriaDeletadaDTO);
        }
    }
}