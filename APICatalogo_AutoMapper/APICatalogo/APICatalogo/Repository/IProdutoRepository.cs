using APICatalogo.Models;
using System.Collections.Generic;

namespace ApiCatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorPreco();
    }
}
