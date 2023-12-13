# Minamal API

## Criar os endpoints da API

- Map, MapGet, MapPost, MapPut, MapDelete, MapMethods, MapFallback.

## Classe Results

Essa classe permite produzir respostas http status code.

```cs
    Results.Ok(new { Done = true });
```

## Definir metadados para o mapeamento de endpoints

Por padrão o Asp net infere esses metadados, mas podemos especificar.

Os métodos RouteHandlerBuilder são usados para personalizar o comportamento de um manipulador de rota. Eles podem ser usados para configurar o tipo de conteúdo aceito, o tipo de conteúdo retornado, a autenticação e autorização necessárias, e muito mais.

```cs
    app.MapPost("/", () => return Results.Ok()) // Retorna um RouteHandlerBuilder
        .Accepts<string>("application/json")
        .Returns<string>("application/json");
        .Produces
        .WithName
        .WithTags // Permite agrupar endpoints, muito util pra agrupar eles no swagger
```

## Documentar com C#

- #region e #endregion

Permite criar uma região (um grupo de código)

```cs
    #region 
        Console.WriteLine("Hello World");
    #endregion 
```