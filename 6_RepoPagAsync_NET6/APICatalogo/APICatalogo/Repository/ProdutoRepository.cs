using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext contexto) : base(contexto)
    {
    }

    public async Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters)
    {

        return await PagedList<Produto>.ToPagedList(Get().OrderBy(on => on.ProdutoId),
            produtosParameters.PageNumber, produtosParameters.PageSize);
    }

    public async Task<IEnumerable<Produto>> GetProdutosPorPreco()
    {
        return await Get().OrderBy(c => c.Preco).ToListAsync();
    }
}
