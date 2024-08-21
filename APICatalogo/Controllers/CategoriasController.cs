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
        private readonly ICategoriaRepository _categoriaRepository;
        //private readonly IRepository<Categoria> _repository;
        private readonly ILogger _logger;

        public CategoriasController(ICategoriaRepository repository, ILogger<CategoriasController> logger)
        {
            _categoriaRepository = repository;
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
        public ActionResult<IEnumerable<Categoria>> GetAll()
        {
            _logger.LogInformation("================= GET api/categorias/produtos =================");

            var categorias = _categoriaRepository.GetAll();

            return Ok(categorias);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult Get(int id)
        {
            _logger.LogInformation($"================= GET api/categorias/id = {id} =================");

            var categoria = _categoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogInformation($"================= GET api/categorias/id = {id} NOT FOUND =================");
                return NotFound($"Categoria com Id {id} não encontrada");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Insert(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            var novaCategoria = _categoriaRepository.Create(categoria);

            // retornar http 201 - Created
            return new CreatedAtRouteResult("ObterCategoria",
               new { id = novaCategoria.CategoriaId }, novaCategoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Update(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
                return BadRequest();

            _categoriaRepository.Update(categoria);

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _categoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com o Id {id} não encontrada...");
                return NotFound($"Categoria com o Id {id} não encontrada...");
            }

            _categoriaRepository.Delete(categoria);

            return Ok(categoria);
        }
    }
}