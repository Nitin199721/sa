using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTabAPI.Domain;
using XTabAPI.Repository.IRepository;

namespace XTabAPI.Repository
{
    public class RepositoryService : IRepository.IRepository
    {
        /// <summary>
        /// Executes Non Query passing the parameters sent in ParamList
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public DataSet ExecuteStoredProcedure(string spName, List<SQLParameter> paramList, string databaseConnection)
        {
            // using (ApplicationConstants.GetImpersonatedUser())

            DataSet ds = null;

            using (SqlConnection connection = new SqlConnection(databaseConnection))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(spName, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    if (paramList != null)
                    {
                        foreach (var param in paramList)
                        {
                            if (param.size != null)
                            {
                                cmd.Parameters.Add(new SqlParameter(param.ParamName, param.SqlDBType, Convert.ToInt32(param.size)));
                                cmd.Parameters[param.ParamName].Value = param.ParamValue;
                            }
                            else
                            {
                                cmd.Parameters.Add(new SqlParameter(param.ParamName, param.size));
                                cmd.Parameters[param.ParamName].Value = param.ParamValue;
                            }

                            if (param.ParamDirection != null)
                            {
                                cmd.Parameters[param.ParamName].Direction = param.ParamDirection.Value;
                            }
                        }
                    }

                    ds = new DataSet();
                    cmd.Connection.Open();
                    var reader = cmd.ExecuteReader();

                    do
                    {
                        var tb = new DataTable();
                        tb.Load(reader);
                        ds.Tables.Add(tb);

                    } while (!reader.IsClosed);
                }
                catch (Exception ex)
                {
                    // _logService.Error(ex);
                    return null;
                }
                finally
                {
                    connection.Close();
                }
                return ds;
            }
        }

        /// <summary>
        /// Executes Non Query passing the parameters sent in ParamList
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public DataSet ExecuteQuery(string query, string databaseConnection)
        {
            DataSet ds = null;

            //using (ApplicationConstants.GetImpersonatedUser())
            try
            {
                databaseConnection = (databaseConnection).Replace(@"\\", @"\");
                using (SqlConnection connection = new SqlConnection(databaseConnection))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);

                    ds = new DataSet();
                    cmd.Connection.Open();
                    var reader = cmd.ExecuteReader();

                    do
                    {
                        var tb = new DataTable();
                        tb.Load(reader);
                        ds.Tables.Add(tb);

                    } while (!reader.IsClosed);
                }
            }
            catch (Exception ex)
            {
                // _logService.Error(ex);
                return null;
            }
            return ds;
        }
    }
}
