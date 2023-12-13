using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<PagedList<Categoria>> GetCategorias(CategoriasParameters categoriaParameters);
    Task<IEnumerable<Categoria>> GetCategoriasProdutos();
}
