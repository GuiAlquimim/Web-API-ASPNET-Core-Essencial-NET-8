using APICatalogo.DTOs;
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
            var categoriasDto = new List<CategoriaDTO>();

            foreach (var categoria in categorias)
            {
                CategoriaDTO categoriaDto = new CategoriaDTO();
                categoriaDto.CategoriaId = categoria.CategoriaId;
                categoriaDto.Nome = categoria.Nome;
                categoriaDto.ImagemUrl = categoria.ImagemUrl;

                categoriasDto.Add(categoriaDto);
            }

            return Ok(categoriasDto);
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

            var categoriaDto = new CategoriaDTO()
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl,
            };

            return Ok(categoriaDto);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Insert(CategoriaDTO categoriaDto)
        {
            var categoria = new Categoria()
            {
                CategoriaId = categoriaDto.CategoriaId,
                Nome = categoriaDto.Nome,
                ImagemUrl = categoriaDto.ImagemUrl,
            };

            if (categoria is null)
                return BadRequest();

            _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            var novaCategoriaDto = new CategoriaDTO()
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl,
            };

            // retornar http 201 - Created
            return new CreatedAtRouteResult("ObterCategoria",
               new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Update(int id, CategoriaDTO categoriaDto)
        {
            var categoria = new Categoria()
            {
                CategoriaId = categoriaDto.CategoriaId,
                Nome = categoriaDto.Nome,
                ImagemUrl = categoriaDto.ImagemUrl,
            };

            if (id != categoria.CategoriaId)
                return BadRequest();

            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            var CategoriaAtualizadaDto = new CategoriaDTO()
            {
                CategoriaId = categoriaDto.CategoriaId,
                Nome = categoriaDto.Nome,
                ImagemUrl = categoriaDto.ImagemUrl,
            };

            return Ok(CategoriaAtualizadaDto);
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

            var CategoriaDeletadaDto = new CategoriaDTO()
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl,
            };

            return Ok(CategoriaDeletadaDto);
        }
    }
}