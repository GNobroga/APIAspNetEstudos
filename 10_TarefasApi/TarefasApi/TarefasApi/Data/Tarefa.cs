using System.ComponentModel.DataAnnotations.Schema;

namespace TarefasApi.Data;

[Table("Tarefas")]
public record Tarefa(int Id, string Atividade, string Status);
