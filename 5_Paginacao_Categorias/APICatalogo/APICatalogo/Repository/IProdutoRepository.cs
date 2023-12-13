using ApiCatalogo.Pagination;
using APICatalogo.Models;
using System.Collections.Generic;

namespace ApiCatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);

        IEnumerable<Produto> GetProdutosPorPreco();
    }
}
