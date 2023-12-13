using ApiCatalogo.Pagination;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ApiCatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {
        }

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriaParameters)
        {
            return PagedList<Categoria>.ToPagedList(Get().OrderBy(on => on.Nome),
                               categoriaParameters.PageNumber,
                               categoriaParameters.PageSize);
        }
    

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(x => x.Produtos);
        }
    }
}
