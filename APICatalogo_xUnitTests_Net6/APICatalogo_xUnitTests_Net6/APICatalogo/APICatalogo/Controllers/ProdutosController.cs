using ApiCatalogo.DTOs;
using ApiCatalogo.Repository;
using APICatalogo.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ApiCatalogo.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiConventionType(typeof(DefaultApiConventions))] // Aplica o ApiConventionMethod globalmente para todos os Actions, o tipo de retorno (Bad request, etc.)
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork contexto, IMapper mapper)
        {
            _uof = contexto;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtém os produtos ordenados por preço na ordem ascendente
        /// </summary>
        /// <returns>Lista de objetos Produtos ordenados por preço</returns>
        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPrecos()
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDto;
        }

        /// <summary>
        /// Exibe uma relação dos produtos
        /// </summary>
        /// <returns>Retorna uma lista de objetos Produto</returns>
        // api/produtos
        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = _uof.ProdutoRepository.Get().ToList();

            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;
        }

        /// <summary>
        /// Obtem um produto pelo seu identificador produtoId
        /// </summary>
        /// <param name="id">Código do produto</param>
        /// <returns>Um objeto Produto</returns>
        // api/produtos/1
        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound();
            }

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);
            return produtoDto;
        }

        //  api/produtos
        [HttpPost]
        public ActionResult Post([FromBody]ProdutoDTO produtoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto);

            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produtoDTO);
        }

       /// <summary>
       /// Atualiza um produto pelo id
       /// </summary>
       /// <param name="id"></param>
       /// <param name="produtoDto"></param>
       /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto);

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok();
        }

        //  api/produtos/1
        [HttpDelete("{id}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound();
            }
    
            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return produtoDto;
        }
    }
}