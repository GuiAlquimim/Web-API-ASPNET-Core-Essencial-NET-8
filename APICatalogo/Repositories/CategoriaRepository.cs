using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasFiltroNomeParameters)
        {
            var categorias = GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(categoriasFiltroNomeParameters.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasFiltroNomeParameters.Nome));
            }

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriasFiltroNomeParameters.PageNumber, categoriasFiltroNomeParameters.PageSize);

            return categoriasFiltradas;
        }

        public PagedList<Categoria> GetCategoriasPagination(CategoriasParameters categoriasParams)
        {
            var categorias = GetAll().OrderBy(c => c.CategoriaId).AsQueryable();

            var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias, categoriasParams.PageNumber, categoriasParams.PageSize);

            return categoriasOrdenadas;
        }
    }
}