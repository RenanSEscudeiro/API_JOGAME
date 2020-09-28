using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EfCore_Produtos.Domains;
using EfCore_Produtos.Interfaces;
using EfCore_Produtos.Repositories;
using EfCore_Produtos.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EfCore_Produtos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutosController()
        {
            _produtoRepository = new ProdutoRepository();
        }

        /// <summary>
        /// Mostra todos os produtos cadastrados
        /// </summary>
        /// <returns>Lista com todos os produtos</returns>

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var produtos = _produtoRepository.Listar();

                if (produtos.Count == 0)
                    return NoContent();

                return Ok(new
                {
                    totalCount = produtos.Count,
                    data = produtos

                });

                return Ok(produtos);
            }
            catch (Exception ex)
            {

                return BadRequest(new {
                 StatusCode = 400,  
                 error = "Ocoorreu um erro no endpoint Get/produtos, envie um e-mail para miniqui10@gmail.com"
                });
            }
        }

        // GET api/produtos/5
        /// <summary>
        /// Mostra um único produto
        /// </summary>
        /// <param name="id">Id do produto</param>
        /// <returns>Um produto</returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                Produto produto = _produtoRepository.BuscarPorId(id);
                if (produto == null)
                    return NotFound();


                Moeda dolar = new Moeda();

                return Ok(new { 
                    produto, valorDolar = dolar.GetDolarValue() * produto.Preco 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/produtos
        /// <summary>
        /// Cadastra um produto
        /// </summary>
        /// <param name="produto">Objeto completo de produto</param>
        /// <returns>Produto cadastrado</returns>
        [HttpPost]
        public IActionResult Post([FromForm]Produto produto)
        {
            try
            {
                if(produto.Imagem != null)
                {
                    var urlImagem = Upload.Local(produto.Imagem);

                    produto.UrlImagem = urlImagem;
                    ;

                }



                _produtoRepository.Adicionar(produto);

                return Ok(produto);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


        }

        // PUT api/produtos/5
        /// <summary>
        /// Altera determinado produto    
        /// </summary>
        /// <param name="id">Id do produto</param>
        /// <param name="produto">Objeto do produto com alterações</param>
        /// <returns>Produto alterado</returns>
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, Produto produto)
        {
            try
            {
                _produtoRepository.Editar(produto);

                return Ok(produto);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // DELETE api/produtos/5
        /// <summary>
        /// Exclui um produto
        /// </summary>
        /// <param name="id">Id do produto</param>
        /// <returns>Id excluido</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var produto = _produtoRepository.BuscarPorId(id);

                if (produto == null)
                    return NotFound();

                _produtoRepository.Remover(id);

                return Ok(id);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
