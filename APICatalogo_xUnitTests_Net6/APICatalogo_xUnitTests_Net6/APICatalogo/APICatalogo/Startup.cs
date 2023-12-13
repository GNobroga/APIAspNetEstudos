using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Logging;
using ApiCatalogo.Repository;
using APICatalogo.Context;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace APICatalogo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. 
        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors();
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build();
                });
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //services.AddScoped<ApiLoggingFilter>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //ajuste da string de conexão para o provedor pomelo
            string mySqlConnection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                 options.UseMySql(mySqlConnection,
                 ServerVersion.AutoDetect(mySqlConnection)));

            services.AddIdentity<IdentityUser, IdentityRole>()
                  .AddEntityFrameworkStores<AppDbContext>()
                  .AddDefaultTokenProviders();

            //JWT
            //adiciona o manipulador de autenticacao e define o 
            //esquema de autenticacao usado : Bearer
            //valida o emissor, a audiencia e a chave
            //usando a chave secreta valida a assinatura
            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidAudience = Configuration["TokenConfiguration:Audience"],
                     ValidIssuer = Configuration["TokenConfiguration:Issuer"],
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes(Configuration["Jwt:key"]))
                 });

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "APICatalogo",
                    Description = "Catálogo de Produtos e Categorias",
                    TermsOfService = new Uri("https://macoratti.net/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "macoratti",
                        Email = "macoratti@yahoo.com",
                        Url = new Uri("https://www.macoratti.net"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Usar sobre LICX",
                        Url = new Uri("https://macoratti.net/license"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                 options.SerializerSettings.ReferenceLoopHandling
                   = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. 
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //,ILoggerFactory loggerFactory)           
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //The default HSTS value is 30 days. 
                //You may want to change this for production
                //scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
            //{
            //    LogLevel = LogLevel.Information
            //})); ;

            //adiciona o middleware de tratamento de erros
            //app.ConfigureExceptionHandler();

            //adiciona o middleware para redirecionar para https
            app.UseHttpsRedirection();

            //adiciona o middleware de roteamento 
            app.UseRouting();

            //adiciona o middleware de autenticacao
            app.UseAuthentication();

            //adiciona o middleware que habilita a autorizacao
            app.UseAuthorization();

            app.UseCors("EnableCORS");

            //Swagger
            app.UseSwagger();

            //SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                     "APICatalogo");
            });

            //app.UseMiddleware<TesteGraphQLMiddleware>();

            //Adiciona o middleware que executa o endpoint 
            //do request atual
            app.UseEndpoints(endpoints =>
            {
                // adiciona os endpoints para as Actions
                // dos controladores sem especificar rotas
                endpoints.MapControllers();
            });
        }
    }
}
