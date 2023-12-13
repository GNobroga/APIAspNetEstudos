using CategoriasMvc.Models;

namespace CategoriasMvc.Services;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoViewModel>> GetProdutos(string token);
    Task<ProdutoViewModel> GetProdutoPorId(int id, string token);
    Task<ProdutoViewModel> CriaProduto(ProdutoViewModel produtoVM, string token);
    Task<bool> AtualizaProduto(int id, ProdutoViewModel produtoVM, string token);
    Task<bool> DeletaProduto(int id, string token);
}
