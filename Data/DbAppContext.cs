using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloRedis.Contextos.Produtos.Domain;
using Microsoft.EntityFrameworkCore;

namespace HelloRedis.Data
{
    public class DbAppContext : DbContext
    {

        public DbSet<Produto> Produtos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
                            => options.UseInMemoryDatabase("ProdutosBD");




    }


}