﻿using ApiCatalogo.Pagination;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {
        }

        public async Task<PagedList<Categoria>> GetCategorias(CategoriasParameters categoriaParameters)
        {
            return await PagedList<Categoria>.ToPagedList(Get().OrderBy(on => on.Nome),
                               categoriaParameters.PageNumber,
                               categoriaParameters.PageSize);
        }
        public async Task<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return await Get().Include(x => x.Produtos).ToListAsync();
        }
    }
}
