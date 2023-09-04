using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using HelloRedis.Contextos.Produtos.Domain;
using HelloRedis.Data;

namespace HelloRedis.Contextos.Produtos.Services
{
    public class ProdutosServices
    {
        private readonly DbAppContext _bancoDeDados;

        public ProdutosServices(DbAppContext banco)
        {
            _bancoDeDados = banco;
        }

        public void NovoProduto(string descricao, decimal valor)
        {
            var produto = new Produto(descricao, valor);
            _bancoDeDados.Produtos.Add(produto);
            _bancoDeDados.SaveChanges();
        }

        public void ApagarProdutos()
        {
            var produtos = _bancoDeDados.Produtos.ToList();
            _bancoDeDados.Produtos.RemoveRange(produtos);
            _bancoDeDados.SaveChanges();
        }

        public void ApagarProduto(Guid id)
        {
            var produto = _bancoDeDados.Produtos.FirstOrDefault(x => x.Id == id);
            if (produto == null)
                throw new Exception("Produto não encontrado");

            _bancoDeDados.Produtos.Remove(produto);
            _bancoDeDados.SaveChanges();
        }

        public List<Produto> ListarProdutos()
        {
            return _bancoDeDados.Produtos.ToList();
        }
    }
}