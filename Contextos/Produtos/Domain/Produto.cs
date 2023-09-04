using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelloRedis.Contextos.Produtos.Domain
{
    public class Produto
    {
        [Key]
        public Guid Id { get; set; }
        public string Descricao { get; private set; } = null!;
        public decimal Valor { get; private set; }

        public Produto(string descricao, decimal valor)
        {
            Id = Guid.NewGuid();
            Descricao = descricao;
            Valor = valor;
        }
    }
}