using ApiCatalogo.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogo.Controllers
{
    [Produces("application/json")] // Avisando que todas as actions produzem application/json
    [Route("api/[Controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AutorizaController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "AutorizaController ::  Acessado em  : "
               + DateTime.Now.ToLongDateString();
        }

        /// <summary>
        /// Registra um novo usuário
        /// </summary>
        /// <param name="model">Um objeto UsuarioDTO</param>
        /// <returns>Status 200 e o token para o cliente</returns>
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody]UsuarioDTO model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            //}

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, false);
            return Ok(GeraToken(model));
        }

        /// <summary>
        /// Verifica as credenciais de um usuário
        /// </summary>
        /// <param name="userInfo">Um objeot do tipo UsuarioDTO</param>
        /// <returns>Status 200 e o token para o cliente</returns>
        /// <remarks>retorna o Status 200 e o token para  novo</remarks>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
        {
            //verifica se o modelo é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }

            //verifica as credenciais do usuário e retorna um valor
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email,
                userInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(GeraToken(userInfo));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Inválido....");
                return BadRequest(ModelState);
            }
        }

        private UsuarioToken GeraToken(UsuarioDTO userInfo)
        {
            //define declarações do usuário
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                 new Claim("meuPet", "pipoca"),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            //gera uma chave com base em um algoritmo simetrico
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            //gera a assinatura digital do token usando o algoritmo Hmac e a chave privada
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Tempo de expiracão do token.
            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            // classe que representa um token JWT e gera o token
             JwtSecurityToken token = new JwtSecurityToken(
               issuer: _configuration["TokenConfiguration:Issuer"],
               audience: _configuration["TokenConfiguration:Audience"],
               claims: claims,
               expires: expiration,
               signingCredentials: credenciais);

            //retorna os dados com o token e informacoes
            return new UsuarioToken()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT OK"
            };
        }
    }
}
