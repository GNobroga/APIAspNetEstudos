using ApiCatalogo.Controllers;
using ApiCatalogo.DTOs;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Repository;
using APICatalogo.Context;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ApiCatalogoxUnitTests
{
    public class CategoriasUnitTestController
    {
        private IMapper mapper;
        private IUnitOfWork repository;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString =
           "Server=localhost;DataBase=CatalogoDB;Uid=root;Pwd=gpxpst";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(connectionString,
                 ServerVersion.AutoDetect(connectionString))
                .Options;
        }

        public CategoriasUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            //DBUnitTestsMockInitializer db = new DBUnitTestsMockInitializer();
            //db.Seed(context);

            repository = new UnitOfWork(context);
        }

        //=======================================================================
        // testes unitários
        // Inicio dos testes : método GET

        [Fact]
        public void GetCategorias_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);

            //Act  
            var data = controller.Get();

            //Assert  
            Assert.IsType<List<CategoriaDTO>>(data.Value);
        }

        [Fact]
        public void GetCategorias_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);

            //Act  
            var data = controller.Get();

            //Assert  
            Assert.IsType<BadRequestResult>(data.Result);
        }

        //GET retorna uma lista de objetos categoria
        //objetivo verificar se os dados esperados estão corretos
        [Fact]
        public void GetCategorias_MatchResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);

            //Act  
            var data = controller.Get();

            //Assert  
            Assert.IsType<List<CategoriaDTO>>(data.Value);
            var cat = data.Value.Should().BeAssignableTo<List<CategoriaDTO>>().Subject;

            Assert.Equal("Bebidas alterada", cat[0].Nome);
            Assert.Equal("bebidas21.jpg", cat[0].ImagemUrl);

            Assert.Equal("Sobremesas", cat[2].Nome);
            Assert.Equal("sobremesas.jpg", cat[2].ImagemUrl);
        }

        //-------------------------------------------------------------
        //GET - Retorna categoria por Id : Retorna objeto CategoriaDTO
        [Fact]
        public void GetCategoriaById_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 2;

            //Act  
            var data = controller.Get(catId);

            //Assert  
            Assert.IsType<CategoriaDTO>(data.Value);
        }

        //-------------------------------------------------------------
        //GET - Retorna Categoria por Id : NotFound
        [Fact]
        public void GetCategoriaById_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 9999;

            //Act  
            var data = controller.Get(catId);

            //Assert  
            Assert.IsType<NotFoundResult>(data.Result);
        }

        //-------------------------------------------------------------
        // POST - Incluir nova categoria - Obter CreatedResult
        [Fact]
        public void Post_Categoria_AddValidData_Return_CreatedResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);

            var cat = new CategoriaDTO() { Nome = "Teste Unitario Inclusao", ImagemUrl = "testecatInclusao.jpg" };

            //Act  
            var data = controller.Post(cat);

            //Assert  
            Assert.IsType<CreatedAtRouteResult>(data);
        }

        //-------------------------------------------------------------
        //PUT - Atualizar uma categoria existente com sucesso
        [Fact]
        public void Put_Categoria_Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 3;

            //Act  
            var existingPost = controller.Get(catId);
            var result = existingPost.Value.Should().BeAssignableTo<CategoriaDTO>().Subject;

            var catDto = new CategoriaDTO();
            catDto.CategoriaId = catId;
            catDto.Nome = "Categoria Atualizada - Testes 1";
            catDto.ImagemUrl = result.ImagemUrl;

            var updatedData = controller.Put(catId, catDto);

            //Assert  
            Assert.IsType<OkResult>(updatedData);
        }

        //-------------------------------------------------------------
        //Delete - Deleta categoria por id - Retorna CategoriaDTO
        [Fact]
        public void Delete_Categoria_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 4;

            //Act  
            var data = controller.Delete(catId);

            //Assert  
            Assert.IsType<CategoriaDTO>(data.Value);
        }
    }
}
