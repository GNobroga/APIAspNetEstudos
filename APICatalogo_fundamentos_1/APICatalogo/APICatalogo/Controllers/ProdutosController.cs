using ApiCatalogo.Filters;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext contexto)
        {
            _context = contexto;
        }

        // api/produtos
        [HttpGet]
        //[ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }   
        
        // api/produtos/1
        [HttpGet("{id}", Name ="ObterProduto")]
        public async Task<ActionResult<Produto>> Get(int id )
        {

            //throw new Exception("Exception ao retornar produto pelo id");
            //string[] teste = null;
            //if (teste.Length > 0)
            //{ }

            var produto = await _context.Produtos.AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProdutoId == id);

            if(produto == null)
            {
                return NotFound();
            }
            return produto;
        }

        //  api/produtos
        [HttpPost]
        public ActionResult Post([FromBody]Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }

        // api/produtos/1
        [HttpPut("{id}")]
        public ActionResult Put(int id,[FromBody] Produto produto)
        {

            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if( id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        //  api/produtos/1
        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            //var produto = _context.Produtos.Find(id);

            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return produto;
        }
    }
}