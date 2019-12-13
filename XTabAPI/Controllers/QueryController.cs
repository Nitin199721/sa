using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using XTabAPI.Domain;
using XTabAPI.Repository;

namespace XTabAPI.Controllers
{
    public class QueryController : ApiController
    {
        /// <summary>
        /// Returns DB Schema - Tables, Columns, DataTypes
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns>  </returns>
        [Authorize]
        [HttpGet]
        [Route("api/Query/GetDatabaseSchema")]
        public Database GetDatabaseSchema(string dbName)
        {
            try
            {
                RepositoryService rs = new RepositoryService();
                dbName = dbName.Replace("\\", "").Replace("\"", "");

                // Get Connection String for the DB
                string connString = GetDBConnectionString(dbName);

                Database dbObject = new Database();
                if (!string.IsNullOrEmpty(connString))
                {
                    //string query = "select '['+ s.SCHEMA_NAME +'].['+ t.name +']' as TableName, c.name as ColumnName, c.column_id as ColumnId, ty.name as DataType, case when c.is_identity = 1 then 1  else  0 end as IsPrimaryKey from  " + dbName + ".sys.tables t " +
                    //                "inner Join sys.columns c on t.object_id = c.object_id " +
                    //                "left outer Join INFORMATION_SCHEMA.SCHEMATA s on s.SCHEMA_NAME = SCHEMA_NAME(t.SCHEMA_ID) " +
                    //                "left outer Join sys.types ty on c.user_type_id = ty.user_type_id " +
                    //                "where t.type = 'U' and t.name not like 'stag%' " +
                    //                "order by TableName , ColumnId";

                  string query = "select '['+ s.SCHEMA_NAME +'].['+ t.name +']' as TableName, c.name as ColumnName, c.column_id as ColumnId, ty.name as DataType, case when c.is_identity = 1 then 1  else  0 end as IsPrimaryKey from  " + dbName + ".sys.tables t " +
                                        "inner Join sys.columns c on t.object_id = c.object_id " +
                                        "left outer Join INFORMATION_SCHEMA.SCHEMATA s on s.SCHEMA_NAME = SCHEMA_NAME(t.SCHEMA_ID) " +
                                        "left outer Join sys.types ty on c.user_type_id = ty.user_type_id " +
                                        "where t.type = 'U' and t.name not like 'stag%' " +
                                    "UNION ALL "+
                                        "select '['+ s.SCHEMA_NAME +'].['+ t.name +']' as TableName, c.name as ColumnName, c.column_id as ColumnId, ty.name as DataType, case when c.is_identity = 1 then 1  else  0 end as IsPrimaryKey from  " + dbName + ".sys.views t " +
                                    "inner Join sys.columns c on t.object_id = c.object_id " +
                                    "left outer Join INFORMATION_SCHEMA.SCHEMATA s on s.SCHEMA_NAME = SCHEMA_NAME(t.SCHEMA_ID) " +
                                    "left outer Join sys.types ty on c.user_type_id = ty.user_type_id " +
                                    "where t.type = 'V' and t.name not like 'stag%' " +
                                    "order by TableName , ColumnId";

                    // Get DB Schema for the selected DB
                    dbObject = Utilities.ConvertDataTableToDatabaseObject(rs.ExecuteQuery(query, connString));
                    return dbObject;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get Connection String for the DB
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        private string GetDBConnectionString(string dbName)
        {
            RepositoryService rs = new RepositoryService();
            string query = "Select * from XTab.DBDetails where DBName = '" + dbName + "'";

            DataSet ds = rs.ExecuteQuery(query, "Server=NICOSQLC02A\\FAM10_DEV; DataBase = Finance; Integrated Security=true;");

            if (ds != null && ds.Tables != null)
            {
                List<DBDetails> dbDetails = Utilities.ConvertDataTable<DBDetails>(ds.Tables[0]);

                if (dbDetails != null && dbDetails.Any())
                {
                    return dbDetails.FirstOrDefault().ConnectionString;
                }
            }

            return null;
        }

        [HttpGet]
        [Authorize]
        [Route("api/Query/GetTableData")]
        public HttpResponseMessage GetTableData(string query)
        {
            try
            {
                RepositoryService rs = new RepositoryService();
                DataSet ds = new DataSet();

                if (query.ToLower().StartsWith("select"))
                {
                    ds = (rs.ExecuteQuery(query, "Server=NICOSQLC02A\\FAM10_DEV; DataBase = Finance;Integrated Security=true;"));
                }

                if (ds == null || ds.Tables == null)
                {
                    var response = Request.CreateResponse(HttpStatusCode.NotFound);
                    return response;
                }
                else
                {
                    string JSONresult;

                    JSONresult = JsonConvert.SerializeObject(ds.Tables[0] /*records*/);

                    var response = Request.CreateResponse(HttpStatusCode.OK);

                    response.Content = new StringContent(JSONresult, Encoding.UTF8, "application/JSON");

                    return response;
                }
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/Query/GetPreferredQueries")]
        public HttpResponseMessage GetPreferredQueries(string userName)
        {
            try
            {
                RepositoryService rs = new RepositoryService();
                string query = "Select * from XTab.PreferredQueries";

                DataSet ds = rs.ExecuteQuery(query, "Server=NICOSQLC02A\\FAM10_DEV; DataBase = Finance;Integrated Security=true;");

                if (ds != null && ds.Tables != null)
                {
                    string JSONresult;

                    JSONresult = JsonConvert.SerializeObject(ds.Tables[0] /*records*/);

                    var response = Request.CreateResponse(HttpStatusCode.OK);

                    response.Content = new StringContent(JSONresult, Encoding.UTF8, "application/JSON");

                    return response;
                }
                else
                {
                    var response = Request.CreateResponse(HttpStatusCode.NotFound);
                    return response;
                }
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }

        /// <summary>
        /// Save Preferred Query
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/Query/SavePreferredQuery")]
        public HttpResponseMessage SavePreferredQuery(PreferredQueries queryDetails)
        {
            try
            {
                RepositoryService rs = new RepositoryService();
                string Insertquery =
                    "INSERT INTO XTAB.PREFERREDQUERIES " +
                            " (WindowsUserName, QueryName, DBDetailsId, Query, IsActive, CreatedDate) " +
                            " VALUES('Finance', '" + queryDetails.QueryName + "', 1, '" + queryDetails.Query + "', 1, GETDATE())";

                rs.ExecuteQuery(Insertquery, "Server=NICOSQLC02A\\FAM10_DEV; DataBase = Finance;Integrated Security=true;");

                //if (ds != null && ds.Tables != null)
                //{
                //    string JSONresult;

                //    JSONresult = JsonConvert.SerializeObject(ds.Tables[0] /*records*/);

                //    var response = Request.CreateResponse(HttpStatusCode.OK);

                //    response.Content = new StringContent(JSONresult, Encoding.UTF8, "application/JSON");

                //    return response;
                //}
                //else
                //{
                //    var response = Request.CreateResponse(HttpStatusCode.NotFound);
                //    return response;
                //}

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }
    }
}
