using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FeContadoNew.Shared
{
    public class DataMapper
    {
        #region Crear instancia

        /// <summary>
        /// Atributo utilizado para evitar problemas con multithreading en el singleton.
        /// </summary>
        private static object syncRoot = new Object();

        private static volatile DataMapper instancia;

        private DataMapper()
        {
        }

        public static DataMapper Instancia
        {
            get
            {
                if (instancia == null)
                {
                    lock (syncRoot)
                    {
                        if (instancia == null)
                        {
                            instancia = new DataMapper();
                        }
                    }
                }
                return instancia;
            }
        }

        #endregion Crear instancia

        /// <summary>
        /// Ejecuta un procedimiento almacenado de consulta, actualizacion, creacion o eliminacion
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="executeType"></param>
        /// <param name="conection"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public async Task<object> ExecuteProcedure(string procedureName, ExecuteType executeType, SqlConnection conection, SqlParameterCollection sqlParamsCollection = null)
        {
            SqlParameter[] sqlParams = sqlParameterCollectionToSqlParameterArray(sqlParamsCollection);
            object returnObject = null;
            using (var cmd = conection.CreateCommand())
            {
                cmd.CommandText = procedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                // get parameters procedure
                SqlCommandBuilder.DeriveParameters(cmd);
                foreach (SqlParameter parm in cmd.Parameters)
                {
                    parm.IsNullable = true;

                    foreach (SqlParameter parmeter in sqlParams)
                    {
                        if (parm.ParameterName.Replace("@", "").ToUpper() == parmeter.ParameterName.Replace("@", "").ToUpper())
                        {
                            parm.Value = parmeter.Value ?? DBNull.Value;
                        }
                    }
                }

                switch (executeType)
                {
                    case ExecuteType.ExecuteReader:
                        returnObject = await cmd.ExecuteReaderAsync();
                        break;

                    case ExecuteType.ExecuteNonQuery:
                        returnObject = await cmd.ExecuteNonQueryAsync();
                        break;

                    case ExecuteType.ExecuteScalar:
                        returnObject = await cmd.ExecuteScalarAsync();
                        break;

                    default:
                        break;
                }
            }

            return returnObject;
        }

        /// <summary>
        /// Mapea el datareader a la entidad y devuelve una coleccion
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public ICollection<TEntity> mapResultsToEntities<TEntity>(ref SqlDataReader reader)
        {
            ICollection<TEntity> lEntities = new Collection<TEntity>();
            while (reader.Read())
            {
                TEntity entity;
                //TODO find a constructor that has zero parameters on TEntity then, use the Activator.CreateInstance method.
                //Otherwise, you use the Factory<T>.CreateNew() method
                //var constructors = typeof(TEntity).GetConstructors();
                entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
                entity = MapColumnsToEntity<TEntity>(ref entity, ref reader);
                lEntities.Add(entity);
            }
            return lEntities;
        }

        /// <summary>
        /// Mapea el datareader a la entidad
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        private TEntity MapColumnsToEntity<TEntity>(ref TEntity entity, ref SqlDataReader reader)
        {
            Byte[] buffer = null;
            long len = 0L;
            int columns = reader.FieldCount;
            for (int i = 0; i < columns; i++)
            {
                String attributeName = reader.GetName(i);
                PropertyInfo attr = propertyFromCustomAttribute<TEntity>(attributeName);
                if (attr != null)
                {
                    Type dataType = reader.GetFieldType(i);
                    bool isNull = reader.IsDBNull(i);
                    if (isNull) { attr.SetValue(entity, null); }
                    else
                    {
                        switch (dataType.ToString())
                        {
                            case "System.Int64":
                                attr.SetValue(entity, reader.GetInt64(i));
                                break;

                            case "System.Int32":
                                attr.SetValue(entity, reader.GetInt32(i));
                                break;

                            case "System.Int16":

                                attr.SetValue(entity, reader.GetInt16(i));
                                break;

                            case "System.String":
                                attr.SetValue(entity, reader.GetString(i));
                                break;

                            case "System.DateTime":
                                attr.SetValue(entity, reader.GetDateTime(i));
                                break;

                            case "System.Decimal":
                                attr.SetValue(entity, reader.GetDecimal(i));
                                break;

                            case "System.Double":
                                attr.SetValue(entity, reader.GetDouble(i));
                                break;

                            case "System.Byte":
                                attr.SetValue(entity, reader.GetByte(i));
                                break;

                            case "System.Byte[]":
                                len = reader.GetBytes(i, 0, null, 0, 0);
                                buffer = new Byte[len];
                                reader.GetBytes(i, 0, buffer, 0, (int)len);
                                attr.SetValue(entity, buffer);
                                break;

                            case "System.Single":
                                attr.SetValue(entity, reader.GetFloat(i));
                                break;

                            case "System.Boolean":
                                attr.SetValue(entity, reader.GetBoolean(i));
                                break;

                            case "System.Guid":
                                attr.SetValue(entity, reader.GetGuid(i));
                                break;
                        }
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// Retorna la propiedad correspondiente al alias(ColumnAttribute) filtrado
        /// </summary>
        /// <param name="customAttribute"></param>
        /// <returns></returns>
        private PropertyInfo propertyFromCustomAttribute<TEntity>(String customAttribute)
        {
            var properties = typeof(TEntity).GetProperties();
            String columnMapping = properties.FirstOrDefault(a => a.Name == customAttribute).Name;
            PropertyInfo attr = typeof(TEntity).GetProperty(columnMapping);
            return attr;
        }

        #region Utils

        public string getConnectionString(IConfiguration _config)
        {
            string ProjectStage = _config.GetSection("PROJECT_STAGE").Value;
            string connectionString = null;
            switch (ProjectStage.ToUpper())
            {
                case "DESARROLLO":
                    connectionString = _config.GetConnectionString("Desarrollo").ToString();
                    break;

                case "PRUEBAS":
                    connectionString = _config.GetConnectionString("Pruebas").ToString();
                    break;

                case "PRODUCCION":
                    connectionString = _config.GetConnectionString("Produccion").ToString();
                    break;
            }
            if (connectionString == null)
            {
                throw new ArgumentNullException("Connection String no encontrada (PROJECT_STAGE) No encontrado" + ProjectStage);
            }
            return connectionString;
        }

        public string getConnectionStringNOVA(IConfiguration _config)
        {
            string ProjectStage = _config.GetSection("PROJECT_STAGE").Value;
            string connectionString = null;
            switch (ProjectStage.ToUpper())
            {
                case "DESARROLLO":
                    connectionString = _config.GetConnectionString("NovaDesarrollo").ToString();
                    break;

                case "PRUEBAS":
                    connectionString = _config.GetConnectionString("NovaPruebas").ToString();
                    break;

                case "PRODUCCION":
                    connectionString = _config.GetConnectionString("NovaProduccion").ToString();
                    break;
            }
            if (connectionString == null)
            {
                throw new ArgumentNullException("Connection String no encontrada (PROJECT_STAGE) No encontrado" + ProjectStage);
            }
            return connectionString;
        }

        private static SqlParameter[] sqlParameterCollectionToSqlParameterArray(SqlParameterCollection sqlParamsCollection)
        {
            SqlParameter[] parameters = new SqlParameter[sqlParamsCollection.Count];

            for (int i = 0; i < sqlParamsCollection.Count; i++)
            {
                SqlParameter p = sqlParamsCollection[i];
                parameters[i] = p;
            }

            return parameters;
        }

        #endregion Utils
    }
}
