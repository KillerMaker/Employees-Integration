using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Gestion_Usuarios_Service_Desk.Models
{
    public class EmployeeView
    {
        public int id { get; set; }
        public string names { get; set; }
        public string lastName { get; set; }
        public string cardId { get; set; }
        public string telephone { get; set; }
        public DateTime birthdate { get; set; }
        public string status { get; set; }
        public DateTime hireDate { get; set; }
        public int? managerId { get; set; }
        public string managerName { get; set; }
        public int? departmentID { get; set; }
        public string departmentName { get; set; }
        public DateTime modifiedDate { get; set; }
        public int? addressId { get; set; }
        public string cityName { get; set; }
        public string direction { get; set; }

        /// <summary>
        /// Ejecuta un Select En la Vista EMPLOYEES_VIEW en la base de datos
        /// </summary>
        /// <param name="queryBuilder">Instancia de la clase QueryBuilder utilizada para reqlizar el query</param>
        /// <param name="where">Parametro opcional que contiene un filtro Where para el query a realizar</param>
        /// <param name="parameter">Parametro opcional el cual sera llamado dentro del where</param>
        /// <returns></returns>
        public static async Task<IEnumerable<EmployeeView>>Select
            (SqlQueryBuilder queryBuilder, string where= null, SqlParameter parameter=null)=>
                await queryBuilder.GetObjectFromDB<EmployeeView>($"SELECT * FROM EMPLOYEES_VIEW {where}");

    }
}
