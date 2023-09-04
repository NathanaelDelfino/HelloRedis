using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloRedis.Contextos.Produtos.Domain;
using HelloRedis.Contextos.Produtos.Services;
using HelloRedis.Data;
using HelloRedis.Infrastructure.Caching;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HelloRedis.Controllers
{
    [ApiController]
    [Route("v1")]
    public class ProdutosController : ControllerBase
    {
        [HttpPost]
        [Route("api/cadastrar-produtos")]
        public IActionResult NovoProduto([FromServices] DbAppContext banco)
        {
            var produtosServices = new ProdutosServices(banco);
            produtosServices.NovoProduto("Produto 1", 10);
            produtosServices.NovoProduto("Produto 2", 20);
            produtosServices.NovoProduto("Produto 3", 30);
            produtosServices.NovoProduto("Produto 4", 40);
            produtosServices.NovoProduto("Produto 5", 50);
            produtosServices.NovoProduto("Produto 6", 60);
            produtosServices.NovoProduto("Produto 7", 70);
            return Ok("Sucesso!");
        }

        [HttpDelete]
        [Route("api/apagar-produto/{id}")]
        public IActionResult ApagarProdutos([FromServices] DbAppContext banco, [FromQuery] Guid id)
        {
            try
            {
                var produtosServices = new ProdutosServices(banco);
                produtosServices.ApagarProduto(id);
                return Ok("Sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("api/obter-produto/{id}")]
        public IActionResult ObterProduto([FromServices] DbAppContext banco,
                                           [FromServices] ICachingService cache,
                                          [FromQuery] Guid id)
        {
            try
            {
                var produtoCache = cache.GetAsync(id.ToString()).Result;
                if (!string.IsNullOrEmpty(produtoCache))
                    return Ok(produtoCache);

                var produtosServices = new ProdutosServices(banco);
                var produto = produtosServices.ListarProdutos().FirstOrDefault(x => x.Id == id);

                cache.SetAsync(id.ToString(), JsonConvert.SerializeObject(produto)).Wait();

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("api/listar-produtos")]
        public IActionResult ListarProdutos([FromServices] DbAppContext banco,
                                            [FromServices] ICachingService cache)
        {
            try
            {
                var produtoCache = cache.GetAsync("produtos").Result;

                if (!string.IsNullOrEmpty(produtoCache))
                    return Ok(produtoCache);

                var produtosServices = new ProdutosServices(banco);
                var produtos = produtosServices.ListarProdutos();
                cache.SetAsync("produtos", JsonConvert.SerializeObject(produtos)).Wait();
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}