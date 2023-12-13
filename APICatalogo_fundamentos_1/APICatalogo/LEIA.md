# Fundamentos

## Roteamento

Ele recebe a requisição e delega ela para o controlador responsável.

```cs
    // Atributo para especificar Route
    [Route("[controller]")]


    // Attributos HTTP
    [HttpPost]
    [HttpGet]
    [HttpPut]
    [HttpDelete]

    // Middleware para habilitar a leitura dos atributos [Route()]
    app.MapControllers()

    // Habilitar explicitamente o roteamento

        app.UseRouting() //é responsável por configurar o roteamento, que determina como as solicitações HTTP são encaminhadas para os endpoints 

        app.UseEndpoints() // e define como os endpoints da aplicação devem ser mapeados.
    
```


## Padrões de Rotas

```cs
    [Route("[controller]")]
    public class NameController {}

    [HttpGet("products")] // Output :  http://localhost:8080/controller/products
    public void Action() {}

    [HttpGet("/lanches")] // Output : http://localhost:8080/lanches
    public void Action() {}

    // Valores fixos

    [HttpGet("{id}/{param2=Teste}")]
    public void Action() {}

    // Multiplos Endpoints
    [HttpGet("/path1")]
    [HttpGet("/path2")]
    public void Action() {}
``` 

## Restricões de rotas - Permite filtrar ou impedir valores indejados atinjam os métodos da Action.

```cs

    /*
        Algumas restricoes

        int
        alpha
        bool
        datetime
        decimal
        double
        float
        guid
        length
        maxlength
        minlength
        range
        min
        max

    */
    [HttpGet("{id:int:min(1)}")] // Duas retricoes
    [HttpGet("{value:alpha}")] // Duas retricoes
```

## Tipos de Retorno de metódos actions

Tipo específico - string, um tipo criado por mim.

IActionResult - É apropriado quando vários tipos de retorno podem ser retornados na Action.


ActionResult<T> - É o mais indicado a ser utilizado

## Métodos de Actions assíncronos

```cs
    public async Task<ActionResult<Type>> Action() {}
```

## Model Binding

```cs

    http://localhost:xxx/api/products/4?nome=Suco&ativo=true

    [HttpGet("{id:int}")]
    public ActionResult<Product> Get(int id, string nome, bool ativo = false) 
    {

    }

```

**BindRequired** - Esse attributo obriga que o paramêtro seja informado.

**BindNever** - Informa ao Model Binder para não vincular a informação ao parâmetro.

**FromForm** - Dados provindos de formulário

**FromRoute** - Dados providos da rota 

**FromQuery** - Dados providos da query string

**FromHeader** - Dados providos do header

**FromBody** - Dados providos do Body

**FromServices** - Semelhante ao Autowired do Spring Boot, permite injetar uma dependencia.

## Validações

O attribute [ApiController] faz verificar automaticamente se o ModelState é válido e responde com um HTTP 400 com detalhes dos problemas. Para fazer a validação manual usa-se o TryValidateModel.

Criar validações customizadas 1. (Basicamente isso é um Attribute)

```cs
    public class ValidateCPF : ValidationAttribute 
    {
        protected override ValidationResult IsValid(
            object value, // Valor vindo
            ValidationContext context // Traz informacao da classe onde o esta sendo feito a validacao
        )
        {

        }
    }


    // Aplicando o ValidateCPF no modelo

    public class Person 
    {   
        [ValidateCPF]
        public string CPF { get; set;}
    }
```

Criar validações customizadas 2. (Basicamente isso é um Attribute)

```cs

    public class Person : IValidatableObject
    {
        public string CPF { get; set;}

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            if (this.CPF.StartsWith("C")) 
            {
                return yield new ValidationResult("sdsdsdsd");
            }
        }
    }
```

## Middleware

Pipeline é uma cadeia de métodos interligados. Cada método dentro de do pipeline são middlewares.


## Configurações

A leitura das informações dos arquivos de configurações é fornecido pelo framework através de serviço IConfiguration.
E podemos utilizar a injeção de dependência para solicitgar o serviço IConfiguration.

## Filtros

Os filtros servem para interceptar requisições, manipular respostas, etc. Semelhante ao que existe no Java OncePerRequest, UsernamePasswordAutenticationFilter etc.

Tipos de filtros

**Authorization (IAuthorizationFilter):**
    Propósito: Executa lógica antes ou após a execução de uma ação para verificar e autorizar o acesso com base em permissões.
    Métodos:
        OnAuthorization: Executado antes da execução da ação, permitindo a verificação e autorização.

**Resource (IResourceFilter):**
    Propósito: Personaliza o comportamento antes e após a execução do resultado (geralmente após a execução de uma ação).
    Métodos:
        OnResourceExecuting: Executado antes da execução do resultado.
        OnResourceExecuted: Executado após a execução do resultado.

**Action (IActionFilter):**
    Propósito: Executa lógica antes ou após a execução de uma ação.
    Métodos:
        OnActionExecuting: Executado antes da execução da ação.
        OnActionExecuted: Executado após a execução da ação.

**Exception (IExceptionFilter):**
    Propósito: Manipula exceções que ocorrem durante a execução de uma ação.
    Métodos:
        OnException: Executado quando ocorre uma exceção durante a execução da ação.

**Result (IResultFilter):**

    Propósito: Personaliza o comportamento antes e após a execução do resultado.
    Métodos:
        OnResultExecuting: Executado antes da execução do resultado.
        OnResultExecuted: Executado após a execução do resultado.
        Implementado um filtro sincrono, tem sua forma assincrona tambem que e so usar IAsyncActionFilter.

```cs

    public class CustomActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
                // Antes da action for executada
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Depois da action for executada
        }
    }

    // Applicando

    builder.Services.AddScoped<CustomActionFilter>();

    public class Teste
    {
        [ServiceFilter(typeof(CustomActionFilter))]
    }
```

1. O filtro global é aplicado primeiro

2. Depois o filtro de nível de classe é aplicado

3. Depois o filtro de nível de método é aplicado.

## Tratamento de erros

Podemos usar o middleware UseExceptionHandler para realizar o tratamento de execeções. Ele pode ser usado para manipular exceções globalmente.

```cs
    app.UseExceptionHandler(appError => {
        appError.Run(async context => {
            // Implementacao do tratamento de erros
            context.Features.Get<IExceptionHandlerFeature>() // Serve pra obter o erro
        })
    })
```

## Logging

Útil para identificar problemas

Níveis de Log no Asp .Net Core

1. Critical

2. Debug

3. Error

4. Information

5. None

6. Trace

7. Warning

Interfaces

. ILoggingFactory

. ILoggingProvider

. ILogger