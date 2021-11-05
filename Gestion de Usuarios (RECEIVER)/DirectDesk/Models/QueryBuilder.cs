using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
#nullable enable

namespace DirectDesk.Models
{
    public class QueryBuilder
    {
        public async static Task<int> ExecuteQuery(string[] querys, List<SqlParameter[]> parameters = null)
        {
            //Se obtiene el string de conexion mediante el archivo de configuracion xml
            string connectionString = ConfigurationManager.ConnectionStrings["Employees"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                //Se crea una transaccion en la base de datos para asegurar la integridad de los datos
                using SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync();

                //Se crea una lista de SQLCommands que se ejecutaran uno tras otro 
                List<SqlCommand> commands = new List<SqlCommand>();
                try
                {
                    //Variable que devolvera el numero de filas afectadas por la lista de comandos
                    int rowsAffected = 0;

                    //Se itera por cada comando de la lista de comandos
                    for (int i = 0; i < querys.Length; i++)
                    {
                        /*Se instancia a cada elemento y se le asigna su correspondiente query,
                         Conexion y transaccion*/
                        commands.Add(new SqlCommand(querys[i], connection, transaction));

                        if (parameters!=null)
                            commands[i].Parameters.AddRange(parameters[i]);

                        //Se Ejecuta el comando de manera asincrona y se le suma la cantidad de filas afectadas a rowsAffected
                        rowsAffected += await commands[i].ExecuteNonQueryAsync();
                    }
                    //Ejecuta los cambios hechos por todos los comandos en la base de datos
                    await transaction.CommitAsync();

                    //Devuelve la cantidad de filas afectadas por todos los comandos 
                    return rowsAffected;

                }
                catch (Exception e)
                {
                    //En caso de error se le hace un RollBack a la transaccion y se revierten los cambios hechos
                    await transaction.RollbackAsync();
                    throw new Exception(e.Message);
                }

            }
        }
    }
}
