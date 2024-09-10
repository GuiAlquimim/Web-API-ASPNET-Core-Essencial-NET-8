using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasFiltroNomeParameters)
        {
            var categorias = await GetAllAsync();

            var categoriasQuery = categorias.AsQueryable();

            if (!string.IsNullOrEmpty(categoriasFiltroNomeParameters.Nome))
            {
                categoriasQuery = categoriasQuery.Where(c => c.Nome.Contains(categoriasFiltroNomeParameters.Nome));
            }

            //var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categoriasQuery, categoriasFiltroNomeParameters.PageNumber, categoriasFiltroNomeParameters.PageSize);

            var categoriasFiltradas = await categorias.ToPagedListAsync(categoriasFiltroNomeParameters.PageNumber, categoriasFiltroNomeParameters.PageSize);

            return categoriasFiltradas;
        }

        public async Task<IPagedList<Categoria>> GetCategoriasPaginationAsync(CategoriasParameters categoriasParams)
        {
            var categorias = await GetAllAsync();

            var categoriasOrderBy = categorias.OrderBy(c => c.CategoriaId).AsQueryable();

            //var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categoriasOrderBy, categoriasParams.PageNumber, categoriasParams.PageSize);

            var categoriasOrdenadas = await categoriasOrderBy.ToPagedListAsync(categoriasParams.PageNumber, categoriasParams.PageSize);

            return categoriasOrdenadas;
        }
    }
}