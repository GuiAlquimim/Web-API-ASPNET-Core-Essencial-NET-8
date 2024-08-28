using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

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

        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        /*[HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation("================= GET api/categorias/produtos =================");

            return _context.Categorias.Include(p => p.Produtos).ToList();
        }
        */

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> GetAll()
        {
            _logger.LogInformation("================= GET api/categorias/produtos =================");

            var categorias = _uof.CategoriaRepository.GetAll();

            var categoriasDTO = categorias.ToCategoriaDTOList();

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

            var categoriaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaDTO);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Insert(CategoriaDTO categoriaDTO)
        {
            var categoria = categoriaDTO.ToCategoria();

            if (categoria is null)
                return BadRequest();

            _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            var novaCategoriaDTO = categoria.ToCategoriaDTO();

            // retornar http 201 - Created
            return new CreatedAtRouteResult("ObterCategoria",
               new { id = novaCategoriaDTO.CategoriaId }, novaCategoriaDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Update(int id, CategoriaDTO categoriaDTO)
        {
            var categoria = categoriaDTO.ToCategoria();

            if (categoria is null || id != categoria.CategoriaId)
                return BadRequest();

            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            var CategoriaAtualizadaDTO = categoria.ToCategoriaDTO();

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

            var CategoriaDeletadaDTO = categoria.ToCategoriaDTO();

            return Ok(CategoriaDeletadaDTO);
        }
    }
}