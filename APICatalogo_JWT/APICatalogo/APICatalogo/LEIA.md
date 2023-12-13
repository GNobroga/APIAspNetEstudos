# JWT

E formado por 3 partes: 

- Header

    Contém informações relacionados aos metadados, algoritmo usado, etc.

- Payload 

    Contém metadados associados ao usuário do Token.

- Signature

    Assinatura usada na validação do token.


```bash
    System.IdentityModel.Tokens.Jwt
    Microsoft.AspNetCore.Authentication.JwtBearer
```

```cs
    // Isso e como se fosse um servico de filtro pra autenticar o cabecalho Authorization Bearer Token
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true
        }
    })

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
```

#  Cors - Cross Origin Resource Sharing

```cs
    Microsoft.AspNetCore.Cors

    // Isso permite habilitar o cors globalmente
    app.UseCors(builder => builder.WithOrigins("http://localhost:5000"));
    // WithOrigins, WithMethod, WithHeaders, AllowAnyOrigin, AllowAnyMethod, AllowAnyHeader

    // Habilitar para apenas um controlador ou action
    builder.Services.AddCors(options => {
        options.AddPolicy("nomeDaPolitica", builder => builder.AllowAnyOrigins())
    });
    
    [EnableCors("nomeDaPolitica")]

```