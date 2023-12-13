# Versionamento

Podemos identificar a versao da API pela 

1. **QueryString** : http://dominio/clientes/lista?v=1

2. **URI** : http://dominio/clientes/v1

3. **Headers**

## Package

```bash
    Microsoft.AspNetCore.Mvc.Versioning // Serve pra ajudar a versionar a API
    Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer // Serve pra mapear a documentacao do versionamento pro swagger.


    builder.Services.AddApiVersioning(o => {
        o.AssumeDefaultVersionWhenUnspecified = true;
        o.DefaultApiVersion = new ApiVersion(1, 0);
        o.ReportApiVersions = true;
        //");
    });

    Services.AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");
    builder.Services.SubstituteApiVersionInUrl = true;

    [ApiVersion("1.0")] // Baseado em QueryString = api/home?api-version=1.0
    public class Controller : ControllerBase 
    {

    }

    [ApiVersion("1.0")] // na URL /api/1.0/home
    [Route("/api/{version:apiVersion}"/users)] 
    public class Controller : ControllerBase 
    {

    }

    // Se a classe tiver dois ApiVersion da pra especificar em qual versao a action vai ser respondida.
    [MapToApiVersion("2.0")]
    public ActionResult Get() 
    {

    }
```

## Swagger

O Swagger conhecido como OpenAPI é um projeto composto por recursos que ajudam os desenvolvedores a realizar a documentação, consumir, etc.

Para isso, o Swagger especifica o OpenAPI, uma linguagem para descrição de cotratos de APIs REST.

A implementação do Swagger para o Asp net se encontra no package,

```cs
    Swashbuckle.AspNetCore

    // Existem 3 principais components no Swashbuckle

    // Swashbuckle.AspNetCore.Swagger - Gera automaticamente documentos Swagger JSON para APIs ASP.NET Core.
    app.UseSwagger();

    // Swashbuckle.AspNetCore.SwaggerGen - Fornece geradores para processar controladores e criar informações para o documento Swagger JSON.
    app.UseSwaggerGen( c => {
        c.SwaggerDoc("v1", new OpenApiInfo {
            Version = "v1"
        })
    });

    // Swashbuckle.AspNetCore.SwaggerUI -  Oferece uma interface web interativa para visualizar e testar a documentação Swagger.
    app.UseSwaggerUI();

```