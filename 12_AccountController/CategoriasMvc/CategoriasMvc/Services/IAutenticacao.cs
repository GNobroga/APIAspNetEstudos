using CategoriasMvc.Models;

namespace CategoriasMvc.Services;

public interface IAutenticacao
{
    Task<TokenViewModel> AutenticaUsuario(UsuarioViewModel usuarioVM);
}
