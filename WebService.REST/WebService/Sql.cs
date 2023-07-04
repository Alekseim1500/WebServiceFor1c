using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Dapper;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Collections.Generic;

public class Sql
{
    private string connectionString;
    private string table;

    public Sql(string server, string database, string table)
    {
        connectionString = $"Server={server};Database={database};Trusted_Connection=True;TrustServerCertificate=true;";
        this.table = table;
    }

    public Sql(List<string> config)
    {
        connectionString = $"Server={config[0]};Database={config[1]};Trusted_Connection=True;TrustServerCertificate=true;";
        table = config[2];
    }

    public void insert(string jsonString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            dynamic jsonObj = JsonConvert.DeserializeObject(jsonString);

            JObject obj = jsonObj;
            var columns = string.Join(",", obj.Properties().Select(p => p.Name).ToList());//получаем ключи
            var values = string.Join(",", obj.Values().ToList());//получаем значения

            var query = $"INSERT INTO {table} ({columns}) VALUES ({values})";
            connection.Execute(query);

            connection.Close();
        }
    }
}