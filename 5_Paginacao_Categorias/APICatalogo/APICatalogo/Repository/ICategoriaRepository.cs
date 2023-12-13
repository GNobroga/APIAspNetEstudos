using ApiCatalogo.Pagination;
using APICatalogo.Models;
using System.Collections.Generic;

namespace ApiCatalogo.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> 
            GetCategorias(CategoriasParameters categoriaParameters);
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
}
