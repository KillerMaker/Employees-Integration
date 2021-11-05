using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text.Json.Serialization;

#nullable enable

namespace DirectDesk.Models
{
    public class Employee
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
        public string? managerName { get; set; }
        public int? departmentID { get; set; }
        public string? departmentName { get; set; }
        public DateTime modifiedDate { get; set; }
        public int? addressId { get; set; }
        public string cityName { get; set; }
        public string direction { get; set; }

        public Employee
            (
                int id,
                string names,
                string lastName,
                string cardId,
                string telephone,
                DateTime birthdate,
                string status,
                DateTime hireDate,
                int? managerId,
                string? managerName,
                int? departmentID,
                string? departmentName,
                DateTime modifiedDate,
                int? addressId,
                string cityName,
                string direction
            )
        {
            this.id = id;
            this.names = names;
            this.lastName = lastName;
            this.cardId = cardId;
            this.telephone = telephone;
            this.birthdate = birthdate;
            this.status = status;
            this.hireDate = hireDate;
            this.managerId = managerId;
            this.departmentID = departmentID;
            this.departmentName = departmentName;
            this.managerName = managerName;
            this.modifiedDate = modifiedDate;
            this.addressId = addressId;
            this.cityName = cityName;
            this.direction = direction;
        }

        public List<SqlParameter> GetParameters()
        {
            SqlParameter names = new SqlParameter("@NAMES", this.names);
            SqlParameter lastName = new SqlParameter("@LAST_NAME", this.lastName);
            SqlParameter cardId = new SqlParameter("@CARD_ID", this.cardId);
            SqlParameter telephone = new SqlParameter("@TELEPHONE", this.telephone);

            SqlParameter birthdate = new SqlParameter("@BIRTHDATE", this.birthdate);
            birthdate.SqlDbType = System.Data.SqlDbType.Date;

            SqlParameter status = new SqlParameter("@STATUS", this.status);
            status.SqlDbType = System.Data.SqlDbType.Bit;

            SqlParameter hireDate = new SqlParameter("@HIRE_DATE", this.hireDate);
            hireDate.SqlDbType = System.Data.SqlDbType.Date;

            //EMPLOYEE_MANAGER puede ser nulo por lo que en caso de que departmentID
            //no tenga valor el valor del sqlparameter debe ser tipo SQLInt32.Null para evitar errores
            SqlParameter managerId = new SqlParameter("@MANAGER_ID", (this.managerId.HasValue) ? this.managerId : SqlInt32.Null);
            managerId.SqlDbType = System.Data.SqlDbType.Int;

            //EMPLOYEE_DEPARTMENT puede ser nulo por lo que en caso de que departmentID
            //no tenga valor el valor del sqlparameter debe ser tipo SQLInt32.Null para evitar errores 
            SqlParameter departmentID = new SqlParameter("@DEPARTMENT_ID", (this.departmentID.HasValue) ? this.departmentID : SqlInt32.Null);
            departmentID.SqlDbType = System.Data.SqlDbType.Int;

            SqlParameter modifiedDate = new SqlParameter("@MODIFIED_DATE", this.modifiedDate);
            modifiedDate.SqlDbType = System.Data.SqlDbType.DateTime;

            SqlParameter addressId = new SqlParameter("@ADDRESS_ID", this.addressId);

            return new List<SqlParameter>() { names, lastName, cardId, telephone, birthdate, status, hireDate, managerId, departmentID, modifiedDate, addressId };
        }

        public string GetInsertInsertString()=>
                @"INSERT INTO EMPLOYEE
                (
                    EMPLOYEE_NAME,
                    EMPLOYEE_LAST_NAME,
                    EMPLOYEE_CARD_ID,
                    EMPLOYEE_TELEPHONE,
                    EMPLOYEE_BIRTHDATE,
                    EMPLOYEE_HIRE_DATE,
                    EMPLOYEE_STATUS,
                    EMPLOYEE_MANAGER,
                    EMPLOYEE_DEPARTMENT,
                    MODIFIED_DATE,
                    ADDRESS_ID
                ) 
                VALUES
                (
                    @NAMES,
                    @LAST_NAME,
                    @CARD_ID,
                    @TELEPHONE,
                    @BIRTHDATE,
                    @HIRE_DATE,
                    @STATUS,
                    @MANAGER_ID,
                    @DEPARTMENT_ID,
                    @MODIFIED_DATE,
                    @ADDRESS_ID
                ); ";
        
    }
}
