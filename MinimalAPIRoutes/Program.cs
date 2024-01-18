using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MinimalAPIRoutes.RouteConstraints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<RouteOptions>(options => {
    options.ConstraintMap.Add("customConstraint", typeof(CheckName));
});

var app = builder.Build();

// Rota convencional
app.MapGet("/route1", async context => {
    await context.Response.WriteAsJsonAsync(new { Message = "Welcome to the route test" });
});

// Rota com parâmetros dinamicos
app.MapGet("/route2/{param1}/{param2}", (HttpContext context, [FromRoute] string param1, [FromRoute] string param2) => {
    return Results.Ok($"Param 1 = {param1} Param 2 = {param2}");
});

// Rota com parâmetro padrão
app.Map("/route3/{userId=1}", (HttpContext context, string userId) => {
    return Results.Ok($$""" {{ userId }}""");
});

// Rota com parâmetro opcional
app.Map("/route4/{productId?}", (HttpContext context, string? productId) => {
    return Results.Ok($$""" Optional {{ productId  ?? "Nothing" }}""");
});

// Rota com * pra pegar uma sequência por exemplo, http://localhost:5252/route5/pedro/123/134
// o paramêtro user vai pegar tudo que vier depois. Output: pedro/123/134
app.Map("/route5/{*user}", (HttpContext context, string user) => {
    return Results.Ok($$""" Route with * {{ user }}""");
});

// Rota com constraint
app.Map("/route6/{price:double}", (HttpContext context, double price) => {
    return Results.Ok($$""" Preço * {{ price }}""");
});

// Rota com custom constraint
app.Map("/route7/{name:customConstraint}", (HttpContext context, string name) => {
    return Results.Ok($$""" Preço * {{ name }}""");
});

// Adicionar ordem a rota com parâmetros iguais, a rota com constraint double vai ter prioridade
app.Map("/route8/{id:int}", (HttpContext context, int id) => {
    return Results.Ok($$""" Preço int* {{ id }}""");
}).Add(builder => {
    (builder as RouteEndpointBuilder)!.Order = 2;
});

app.Map("/route8/{id:double}", (HttpContext context, double id) => {
    return Results.Ok($$""" Preço  double* {{ id }}""");
}).Add(builder => {
    (builder as RouteEndpointBuilder)!.Order = 1;
});



app.Run();


