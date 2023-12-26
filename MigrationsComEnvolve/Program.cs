
using System.Data.SQLite;
using EvolveDb;

try 
{
    var connection = new SQLiteConnection("Data source=app.db");

    var evolve = new Evolve(connection)
    {
        Locations = new List<string>() { "Db/Migrations", "Db/Populate" },
    };

    evolve.Migrate();
}
catch 
{
    throw;
}