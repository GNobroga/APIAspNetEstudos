using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters);
    Task<IEnumerable<Produto>> GetProdutosPorPreco();
}
