using ApiCatalogo.DTOs;
using ApiCatalogo.Repository;
using APICatalogo.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiCatalogo.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CategoriasController(IUnitOfWork contexto,IMapper mapper)
        {
            _context = contexto;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("teste")]
        public string GetTeste()
        {
            return $"CategoriasController - {DateTime.Now.ToLongDateString().ToString()}";
        }

        /// <summary>
        /// Obtém os produtos relacionados para cada categoria
        /// </summary>
        /// <returns>Objetos Categoria e respectivo Objetos Produtos</returns>
        [HttpGet("produtos")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos()
        {
           // _logger.LogInformation("================GET api/categorias/produtos ======================");

            var categorias = _context.CategoriaRepository.GetCategoriasProdutos().ToList();
            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDto;
        }

        /// <summary>
        /// Retorna uma coleção de objetos Categoria
        /// </summary>
        /// <returns>Lista de Categorias</returns>
        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            try
            {
                var categorias = _context.CategoriaRepository.Get().ToList();
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriasDto;
            }
            catch (Exception)
            {
                return BadRequest();
            }           
        }

        [HttpGet("paginacao")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetPaginacao(int pag=1, int reg=5)
        {
            if (reg > 99)
                reg = 5;

            var categorias = _context.CategoriaRepository
                .LocalizaPagina<Categoria>(pag,reg)
                .ToList();

            var totalDeRegistros = _context.CategoriaRepository.GetTotalRegistros();
            var numeroPaginas = ((int)Math.Ceiling((double)totalDeRegistros / reg));

            Response.Headers["X-Total-Registros"] = totalDeRegistros.ToString();
            Response.Headers["X-Numero-Paginas"] = numeroPaginas.ToString();

            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDto;
        }

        /// <summary>
        /// Obtem uma Categoria pelo seu Id
        /// </summary>
        /// <param name="id">codigo do categoria</param>
        /// <returns>Objetos Categoria</returns>
        [HttpGet("{id}", Name = "ObterCategoria")]
        //[EnableCors("PermitirApiRequest")]
        [ProducesResponseType(typeof(ProdutoDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Permite informar para o OpenAPI o que a action pode retornar.
        public ActionResult<CategoriaDTO> Get(int? id)
        {
            var categoria = _context.CategoriaRepository.GetById(p => p.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound();
            }
            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return categoriaDto;
        }

        /// <summary>
        /// Inclui uma nova categoria
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     POST api/categorias
        ///     {
        ///        "categoriaId": 1,
        ///        "nome": "categoria1",
        ///        "imagemUrl": "http://teste.net/1.jpg"
        ///     }
        /// </remarks>
        /// <param name="categoriaDto">objeto Categoria</param>
        /// <returns>O objeto Categoria incluida</returns>
        /// <remarks>Retorna um objeto Categoria incluído</remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Post([FromBody]CategoriaDTO categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _context.CategoriaRepository.Add(categoria);
            _context.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoriaDTO);
        }

        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), // Esse attribute e um shorthand do ProducesResponseType, ele  consegue inferir tudo.
                     nameof(DefaultApiConventions.Put))]
        public ActionResult Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                return BadRequest();
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _context.CategoriaRepository.Update(categoria);
            _context.Commit();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<CategoriaDTO> Delete(int? id)
        {
            var categoria = _context.CategoriaRepository.GetById(p => p.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound();
            }
            _context.CategoriaRepository.Delete(categoria);
            _context.Commit();

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

            return categoriaDto;
        }
    }
}

