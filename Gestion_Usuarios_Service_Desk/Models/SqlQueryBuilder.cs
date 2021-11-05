using System;
using System.Collections.Generic;
using Dapper;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;

namespace Gestion_Usuarios_Service_Desk.Models
{
     public class SqlQueryBuilder 
    {
        private readonly string connectionString;     
        public readonly DataProvider provider;

     /// <summary>
     /// Metodo Constructor de la clase SqlQueryBuilder
     /// </summary>
     /// <param name="provider">Proveedor de acceso a datos</param>
     /// <param name="connectionString">String de conexion con la informacion 
     /// necesaria para conectarse a una base de datos</param>
        public SqlQueryBuilder(DataProvider provider,string connectionString)
        {
            this.provider = provider;
            this.connectionString= connectionString;
        }

        /// <summary>
        /// Obtiene una instancia de una clase hija de DbConnection segun el DataProvider
        /// </summary>
        /// <returns>Una instancia de una clase que hija de DbConnection con su respectivo connection string</returns>
        private DbConnection GetConnectionByDataProvider() =>
            provider switch
            {
                DataProvider.SqlServer => new SqlConnection(connectionString),
                DataProvider.MySql => new MySqlConnection(connectionString),
                DataProvider.Oracle => new OracleConnection(connectionString),
                _ => throw new NotImplementedException("Este tipo de proveedor aun no ha sido configurado")
            };

        /// <summary>
        /// Realiza un Query a la base de datos y realiza un 
        /// binding a un objeto con lo que el query retorne
        /// </summary>
        /// <typeparam name="T">Tipo del objeto a bindear</typeparam>
        /// <param name="query">Query a ejecutar</param>
        /// <param name="parameter">Parametro opcional del query</param>
        /// <returns>Un IEnurable de T</returns>
        public async Task<IEnumerable<T>> GetObjectFromDB<T>(string query, DynamicParameters parameters = null)
        {
            //Le asigna a connection una instancia definida por el metodo GetConnectionByDataProvider()
            using (DbConnection connection = GetConnectionByDataProvider())
            {
                //Si parameter no es null realiza el query con el parametro y de ser null no
               return (parameters is not null) ?
                    await connection.QueryAsync<T>(query, param: parameters) : await connection.QueryAsync<T>(query);
            } 
        }

        /// <summary>
        /// Ejecuta comandos SQL en una base de datos
        /// </summary>
        /// <param name="command">Comando a ejecutar</param>
        /// <param name="parameters">listado de parametros del comando</param>
        /// <returns>Numero de filas afectadas por el comando</returns>
        public async Task<int>ExcuteCommand(string command, DynamicParameters parameters= null)
        {
            //Le asigna a connection una instancia definida por el metodo GetConnectionByDataProvider()
            using (DbConnection connection = GetConnectionByDataProvider())
            {
                return(parameters is not null)? 
                    await connection.ExecuteAsync(command,parameters,commandType:CommandType.StoredProcedure):
                    await connection.ExecuteAsync(command, commandType: CommandType.StoredProcedure);
            }
        }
    }

}
