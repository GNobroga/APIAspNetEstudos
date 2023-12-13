using Dapper.Contrib.Extensions;
using TarefasApi.Data;
using static TarefasApi.Data.TarefaContext;

namespace TarefasApi.Endpoints
{
    public static class TarefasEndpoints
    {
        public static void MapTarefasEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => $"Bem-Vindo a API Tarefas {DateTime.Now}");

            app.MapGet("/tarefas", async(GetConnection connectionGetter) =>
            {
                using var con = await connectionGetter();
                var tarefas = con.GetAll<Tarefa>().ToList();
                
                if(tarefas is null)
                    return Results.NotFound();

                return Results.Ok(tarefas);
            });

            app.MapGet("/tarefas/{id}", async (GetConnection connectionGetter, int id) =>
            {
                using var con = await connectionGetter();
                //var tarefa = con.Get<Tarefa>(id);
                //if (tarefa is null)
                //    return Results.NotFound();
                //return Results.Ok(tarefa);

                return con.Get<Tarefa>(id) is Tarefa tarefa ? Results.Ok(tarefa) : Results.NotFound();
            });

            app.MapPost("/tarefas", async (GetConnection connectionGetter, Tarefa Tarefa) =>
             {
                 using var con = await connectionGetter();
                 var id = con.Insert(Tarefa);
                 return Results.Created($"/tarefas/{id}", Tarefa);
             });

            app.MapPut("/tarefas", async (GetConnection connectionGetter, Tarefa Tarefa) =>
            {
                using var con = await connectionGetter();
                var id = con.Update(Tarefa);
                return Results.Ok();
            });

            app.MapDelete("/tarefas/{id}", async (GetConnection connectionGetter, int id) =>
            {
                using var con = await connectionGetter();

                var deleted = con.Get<Tarefa>(id);

                if (deleted is null)
                    return Results.NotFound();

                con.Delete(deleted);
                return Results.Ok(deleted);
            });
        }
    }
}
