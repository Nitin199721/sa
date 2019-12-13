using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XTabAPI.Domain;

namespace XTabAPI.Repository
{
    public static class Utilities
    {
        public static Database ConvertDataTableToDatabaseObject(DataSet ds)
        {
            if (ds == null || ds.Tables == null || ds.Tables[0] == null)
            {
                return null;
            }
            else
            {
                Database dbSchemaObject = new Database() { DBName = "Finance", Id = 1, Tables = new List<Table>() };

                List<DBSchema> dbDetailsObject = ConvertDataTable<DBSchema>(ds.Tables[0]);

                // Loop on dbDetailsObject and load Table and Columns to dbSchemaObject
                int tableID = 1;
                foreach (var tblRow in dbDetailsObject)
                {
                    // Create Table Object by picking unique
                    if (dbSchemaObject.Tables != null && dbSchemaObject.Tables.FirstOrDefault(t => t.tableName == tblRow.TableName) == null)
                    {
                        dbSchemaObject.Tables.Add(new Table() { Id = tableID, tableName = tblRow.TableName, selectAllFields = true, tableAlias= "", columns = new List<Column>() });
                        tableID++;
                    }

                    if (dbSchemaObject.Tables != null && dbSchemaObject.Tables.FirstOrDefault(t => t.tableName == tblRow.TableName) != null)
                    {
                        dbSchemaObject.Tables.FirstOrDefault(t => t.tableName == tblRow.TableName).columns.Add(new Column() {
                            Id = (dbSchemaObject.Tables.FirstOrDefault(t => t.tableName == tblRow.TableName).columns != null && dbSchemaObject.Tables.FirstOrDefault(t => t.tableName == tblRow.TableName).columns.Any()) ? dbSchemaObject.Tables.FirstOrDefault(t => t.tableName == tblRow.TableName).columns.Max(c => c.Id) + 1 : 1,
                            columnName = tblRow.ColumnName,
                            dataType = tblRow.DataType,
                            isPrimaryKey = tblRow.IsPrimaryKey,
                            selectField = true,
                            grpcolumnselected = false});
                    }
                }

                return dbSchemaObject;
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            if (items != null)
            {
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
            }

            return dataTable;
        }
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                var value = dr[column.ColumnName];
                if (obj.GetType().GetProperty(column.ColumnName).PropertyType == typeof(Nullable<System.DateTime>) || obj.GetType().GetProperty(column.ColumnName).PropertyType.Name == "DateTime")
                {
                    obj.GetType().GetProperty(column.ColumnName).SetValue(obj, value != DBNull.Value ? value : System.DateTime.Now, null);
                }
                else if (obj.GetType().GetProperty(column.ColumnName).PropertyType == typeof(Nullable<System.Int32>))
                {
                    obj.GetType().GetProperty(column.ColumnName).SetValue(obj, value != DBNull.Value ? value : null, null);
                }
                else if (obj.GetType().GetProperty(column.ColumnName).PropertyType == typeof(System.String))
                {
                    obj.GetType().GetProperty(column.ColumnName).SetValue(obj, value != DBNull.Value ? Convert.ToString(value) : string.Empty, null);
                }
                else if (obj.GetType().GetProperty(column.ColumnName).PropertyType == typeof(System.Boolean))
                {
                    obj.GetType().GetProperty(column.ColumnName).SetValue(obj, value != DBNull.Value ? Convert.ToBoolean(value) : false, null);
                }
                else
                {
                    obj.GetType().GetProperty(column.ColumnName).SetValue(obj, value != DBNull.Value ? value : string.Empty, null);
                }
            }
            return obj;
        }
    }
}
