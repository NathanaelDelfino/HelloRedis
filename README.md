# Hello Redis

Projeto de estudo para o entendimento da mecanica do Redis como sistema de cache.

## O que é o Redis?

O Redis é um sistema de armazenamento em cache de dados de código aberto e em memória, frequentemente utilizado como banco de dados em memória, armazenando dados na RAM para acesso rápido. O termo "cache" refere-se à prática de armazenar temporariamente dados frequentemente acessados em locais de fácil recuperação, a fim de acelerar o acesso subsequente a esses dados.

## Para o que serve o Redis?

O Redis, especificamente, é projetado para ser extremamente rápido e eficiente no armazenamento e recuperação de dados na memória. Ele oferece várias estruturas de dados, como strings, listas, conjuntos, hashes e muito mais, tornando-o versátil para diferentes casos de uso.


### Primeiro passo - Instalando pacote no projeto
---

No caso do dotnet, fazer a instalação do seguinte pacote. [URL do pacote no nuget](https://www.nuget.org/packages/Microsoft.Extensions.Caching.StackExchangeRedis) 

Comando para instalar o pacote via nuget.

```
  dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis --version 7.0.10
```


### Segundo passo - Configurando ambiente
-------
No caso de projetos feitos utilizando Minimal API em C#. 
Adicionar o seguinte trecho de código.

```
  builder.Services.AddScoped<ICachingService, CachingService>();
  
  builder.Services.AddStackExchangeRedisCache(options =>
  {
      options.Configuration = "localhost:6379";
      options.InstanceName = "HelloRedis";
  });
```

#### Configuration

  
Na váriavel *configuration* deve ser informado a url de conexão no local a onde será hospedado o cache do redis
  options.Configuration = "localhost:6379";


#### InstanceName
  
Na váriavel *InstanceName* deve ser informado o nome da instancia na qual a conexão do redis vai estar disposto a aplicação. 
  options.Configuration = "localhost:6379";

Na criação dessa documentação (13/11/2023) o redis conta um plano free de cache com até 30MB.

![Preços e planos redis cloud](https://github.com/NathanaelDelfino/HelloRedis/assets/7662248/b43095f7-9c83-4a96-9377-f5c41e3725c4)

### Terceiro passo - Criando elemetos de cache

```
[HttpGet]
        [Route("api/listar-produtos")]
        public IActionResult ListarProdutos([FromServices] DbAppContext banco,
                                            [FromServices] ICachingService cache)
        {
            try
            {
                var produtoCache = cache.GetAsync("produtos").Result;

                //convert cache to object

                if (!string.IsNullOrEmpty(produtoCache))
                    return Ok(JsonConvert.DeserializeObject<List<Produto>>(produtoCache));

                var produtosServices = new ProdutosServices(banco);
                var produtos = produtosServices.ListarProdutos();
                if (produtos.Any())
                    cache.SetAsync("produtos", JsonConvert.SerializeObject(produtos)).Wait();

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
```

#### Criando uma lista de cache
  
Para adicionar elementos a uma lista de cache, você precisa basicamente de dois elementos. 
 - Chave :  Será o elemento que utilizaremos posteriormente para realizar buscas no cache 
 - Valor :  Será o elemento que vamos armazenar os dados para cache

```
 cache.SetAsync("produtos", JsonConvert.SerializeObject(produtos)).Wait();
```



### Quarto passo - Obtendo elementos do cache


```
[HttpGet]
        [Route("api/listar-produtos")]
        public IActionResult ListarProdutos([FromServices] DbAppContext banco,
                                            [FromServices] ICachingService cache)
        {
            try
            {
                var produtoCache = cache.GetAsync("produtos").Result;

                //convert cache to object

                if (!string.IsNullOrEmpty(produtoCache))
                    return Ok(JsonConvert.DeserializeObject<List<Produto>>(produtoCache));

                var produtosServices = new ProdutosServices(banco);
                var produtos = produtosServices.ListarProdutos();
                if (produtos.Any())
                    cache.SetAsync("produtos", JsonConvert.SerializeObject(produtos)).Wait();

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
```


#### Obtendo elemento de uma lista de cache

```
 var produtoCache = cache.GetAsync("produtos").Result;
```

Com a chave que definimos anteriormente podemos realizar consulta na base de cache, que estamos obtendo da services, na qual configuramos no segundo passo.
