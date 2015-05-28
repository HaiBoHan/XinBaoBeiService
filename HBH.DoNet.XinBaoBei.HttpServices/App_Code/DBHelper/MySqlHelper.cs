using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Configuration;
using MySql.Data.MySqlClient;

public class MySqlHelper : DatabaseHelper
{
    private int _commandTimeout = 3000;
    private string _sqlConnStr;
    private SetOfBookType setOfBook = SetOfBookType.Test;
    private string character = string.Empty;
    private string character_set_client = string.Empty;
    private string character_set_results = string.Empty;
    private string character_set_connection = string.Empty;

    public MySqlHelper(SetOfBookType sobType)
    {
        setOfBook = sobType;

        //this._sqlConnStr = ConfigurationSettings.AppSettings["SqlConnStr"].ToString();
        //if (ConfigurationSettings.AppSettings["CommandTimeout"] != null)
        //{
        //    this._commandTimeout = int.Parse(ConfigurationSettings.AppSettings["CommandTimeout"].ToString());
        //}

        //this._sqlConnStr = ConfigurationManager.AppSettings["LocalMySqlServer"].ToString(); ;
        //this._commandTimeout = int.Parse(ConfigurationSettings.AppSettings["CommandTimeout"].ToString());

        //this._sqlConnStr = ConfigurationManager.ConnectionStrings["LocalMySqlServer"].ToString();
        this._sqlConnStr = ConfigurationManager.ConnectionStrings[setOfBook.ToString()].ToString();

        this.character = UIHelper.GetString(ConfigurationManager.AppSettings["charset"]);
        this.character_set_client = UIHelper.GetString(ConfigurationManager.AppSettings["character_set_client"]);
        this.character_set_results = UIHelper.GetString(ConfigurationManager.AppSettings["character_set_results"]);
        this.character_set_connection = UIHelper.GetString(ConfigurationManager.AppSettings["character_set_connection"]);

        if (setOfBook == SetOfBookType.HBHBaby)
        {
            this.character = "";
            this.character_set_client = "utf8";
            this.character_set_results = "utf8";
            this.character_set_connection = "utf8";
        }
       
    }

    public MySqlHelper(string SqlConnStr)
    {
        this._sqlConnStr = SqlConnStr;
    }

    public TableTypeParameter CreateParameter(string p_paramName, object p_paramValue)
    {
        TableTypeParameter param =  new TableTypeParameter() ;
        {
            param.ParameterName = p_paramName;
            param.ParameterValue = p_paramValue; 
        };

        return param;
    }

    public MySqlParameter CreateParameter(string p_paramName, SqlDbType p_paramType, object p_paramSize, object p_paramValue, ParameterDirection p_paramDirection)
    {
        MySqlParameter parameter = new MySqlParameter(p_paramName, p_paramType);
        if (p_paramSize != null)
        {
            parameter.Size = int.Parse(p_paramSize.ToString());
        }
        if (p_paramValue != null)
        {
            parameter.Value = p_paramValue;
        }
        parameter.Direction = p_paramDirection;
        return parameter;
    }

    public int ExecuteNonQuery(CommandType p_CommandType, string p_CommandText, params TableTypeParameter[] p_Parameters)
    {
        int result = 0;
        MySqlConnection connection = null;
        MySqlCommand command = null;
        try
        {
            connection = new MySqlConnection();
            {
                connection.ConnectionString = this._sqlConnStr;
                OpenConnection(connection);
            };
            command = new MySqlCommand(p_CommandText, connection);
            {
                command.CommandType = p_CommandType;
                command.CommandTimeout = this._commandTimeout;
            };
            if ((p_Parameters != null) && (p_Parameters.Length != 0))
            {
                foreach (TableTypeParameter parameter in p_Parameters)
                {
                    if (parameter != null)
                    {
                        command.Parameters.AddWithValue(parameter.ParameterName, parameter.ParameterValue);
                    }
                }
            }
            result = command.ExecuteNonQuery();
        }
        catch (Exception exception)
        {
            throw exception;
        }
        finally
        {
            if (command != null)
            {
                command.Dispose();
            }
            CloseConnection(connection);
        }
        return result;
    }

    public int ExecuteNonQuery(CommandType p_CommandType, string p_CommandText, params MySqlParameter[] p_Parameters)
    {
        int result = 0;
        MySqlConnection connection = null;
        MySqlCommand command = null;
        try
        {
            connection = new MySqlConnection();
            {
                connection.ConnectionString = this._sqlConnStr;
            };

            OpenConnection(connection);
            command = new MySqlCommand(p_CommandText, connection);
            {
                command.CommandType = p_CommandType;
                command.CommandTimeout = this._commandTimeout;
            };
            if ((p_Parameters != null) && (p_Parameters.Length != 0))
            {
                foreach (MySqlParameter parameter in p_Parameters)
                {
                    if (parameter != null)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
            }
            result = command.ExecuteNonQuery();
        }
        catch (Exception exception)
        {
            throw exception;
        }
        finally
        {
            CloseConnection(connection);
            if (command != null)
            {
                command.Dispose();
            }
        }
        return result;
    }

    private static void OpenConnection(MySqlConnection connection)
    {
        if (connection.State != ConnectionState.Open)
        {
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("数据库连接失败,请联系管理员! MySql:{0}", connection.ConnectionString));
            }
        }
    }

    private static void CloseConnection(MySqlConnection connection)
    {
        if (connection != null)
        {
            connection.Close();
        }
    }

    public int ExecuteNonQuery(CommandType p_CommandType, string p_CommandText, MySqlParameter[][] p_Parameters)
    {
        int result = 0;
        MySqlConnection connection = null;
        MySqlCommand command = null;
        MySqlTransaction transaction = null;
        try
        {
            connection = new MySqlConnection();
            {
                connection.ConnectionString = this._sqlConnStr;
            };

            OpenConnection(connection);
            transaction = connection.BeginTransaction();
            for (int i = 0; i < p_Parameters.Length; i++)
            {
                command = new MySqlCommand(p_CommandText, connection, transaction);
                {
                    command.CommandType = p_CommandType;
                    command.CommandTimeout = this._commandTimeout;
                };
                if (p_Parameters[i] != null)
                {
                    if (p_Parameters[i].Length != 0)
                    {
                        foreach (MySqlParameter parameter in p_Parameters[i])
                        {
                            if (parameter != null)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }
                    }
                    result = command.ExecuteNonQuery();
                }
            }
            transaction.Commit();
        }
        catch (Exception exception)
        {
            if (command != null
                && command.Transaction != null
                )
            {
                command.Transaction.Rollback();
            }
            throw exception;
        }
        finally
        {
            CloseConnection(connection);
            if (command != null)
            {
                command.Dispose();
            }
            if (transaction != null)
            {
                transaction.Dispose();
            }
        }
        return result;
    }

    public int ExecuteNonQuery(CommandType p_CommandType, string p_CommandText, TableTypeParameter[][] p_Parameters)
    {
        int result = 0;
        MySqlConnection connection = null;
        MySqlCommand command = null;
        MySqlTransaction transaction = null;
        try
        {
            connection = new MySqlConnection();
            {
                connection.ConnectionString = this._sqlConnStr;
            };

            OpenConnection(connection);
            transaction = connection.BeginTransaction();
            for (int i = 0; i < p_Parameters.Length; i++)
            {
                command = new MySqlCommand(p_CommandText, connection, transaction);
                {
                    command.CommandType = p_CommandType;
                    command.CommandTimeout = this._commandTimeout;
                };
                if (p_Parameters[i] != null)
                {
                    if (p_Parameters[i].Length != 0)
                    {
                        //foreach (MySqlParameter parameter in p_Parameters[i])
                        //{
                        //    command.Parameters.Add(parameter);
                        //}
                        foreach (TableTypeParameter parameter in p_Parameters[i])
                        {
                            if (parameter != null)
                            {
                                command.Parameters.AddWithValue(parameter.ParameterName, parameter.ParameterValue);
                            }
                        }
                    }
                    result = command.ExecuteNonQuery();
                }
            }
            transaction.Commit();
        }
        catch (Exception exception)
        {
            if (command != null
                && command.Transaction != null
                )
            {
                command.Transaction.Rollback();
            }
            throw exception;
        }
        finally
        {
            CloseConnection(connection);
            if (command != null)
            {
                command.Dispose();
            }
            if (transaction != null)
            {
                transaction.Dispose();
            }
        }
        return result;
    }


    public object ExecuteScalar(CommandType p_CommandType, string p_CommandText, params TableTypeParameter[] p_Parameters)
    {
        object result = null;
        MySqlConnection connection = null;
        MySqlCommand command = null;
        try
        {
            connection = new MySqlConnection();
            {
                connection.ConnectionString = this._sqlConnStr;
            };

            OpenConnection(connection);
            command = new MySqlCommand(p_CommandText, connection);
            {
                command.CommandType = p_CommandType;
                command.CommandTimeout = this._commandTimeout;
            };
            if ((p_Parameters != null) && (p_Parameters.Length != 0))
            {
                foreach (TableTypeParameter parameter in p_Parameters)
                {
                    if (parameter != null)
                    {
                        command.Parameters.AddWithValue(parameter.ParameterName, parameter.ParameterValue);
                    }
                }
            }
            result = command.ExecuteScalar();
        }
        catch (Exception exception)
        {
            throw exception;
        }
        finally
        {
            CloseConnection(connection);
            if (command != null)
            {
                command.Dispose();
            }
        }
        return result;
    }

    public object ExecuteScalar(CommandType p_CommandType, string p_CommandText, params MySqlParameter[] p_Parameters)
    {
        object result = null;
        MySqlConnection connection = null;
        MySqlCommand command = null;
        try
        {
            connection = new MySqlConnection();
            {
                connection.ConnectionString = this._sqlConnStr;
            };

            OpenConnection(connection);
            command = new MySqlCommand(p_CommandText, connection);
            {
                command.CommandType = p_CommandType;
                command.CommandTimeout = this._commandTimeout;
            };
            if ((p_Parameters != null) && (p_Parameters.Length != 0))
            {
                foreach (MySqlParameter parameter in p_Parameters)
                {
                    if (parameter != null)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
            }
            result = command.ExecuteScalar();
        }
        catch (Exception exception)
        {
            throw exception;
        }
        finally
        {
            CloseConnection(connection);
            if (command != null)
            {
                command.Dispose();
            }
        }
        return result;
    }

    public void Fill(DataSet ds, CommandType p_CommandType, string p_CommandText, params TableTypeParameter[] p_Parameters)
    {
        MySqlConnection connection = null;
        MySqlCommand command = null;
        MySqlDataAdapter adapter = null;
        try
        {
            connection = new MySqlConnection();
            {
                connection.ConnectionString = this._sqlConnStr;
            };

            OpenConnection(connection);

            TableTypeParameter param = null;

            //if (!UIHelper.IsNull(character))
            //{
            //    string strCharType = character;     //"utf8"  //  "latin1"    // 'gbk'  //   "gb2312";
            //    string sqlChar = "SET character_set_client=" + strCharType;
            //    ExecuteNonQuery(CommandType.Text, sqlChar, param);
            //    sqlChar = "SET character_set_results=" + strCharType;
            //    ExecuteNonQuery(CommandType.Text, sqlChar, param);
            //    sqlChar = "SET character_set_connection=" + strCharType;
            //    ExecuteNonQuery(CommandType.Text, sqlChar, param);
            //}

            // string strCharType = string.Empty;  //   character_set_client;     //"utf8"  //  "latin1"    // 'gbk'  //   "gb2312";
            StringBuilder sbCharacter = new StringBuilder();

            if (!UIHelper.IsNull(character_set_connection))
            {
                sbCharacter.Append(string.Format("SET {0}={1};", "character_set_connection", character_set_connection));
            }

            if (!UIHelper.IsNull(character_set_results))
            {
                sbCharacter.Append(string.Format("SET {0}={1};", "character_set_results", character_set_results));
            }

            if (!UIHelper.IsNull(character_set_client))
            {
                sbCharacter.Append(string.Format("SET {0}={1};", "character_set_client", character_set_client));
            }

            if (sbCharacter.Length > 0)
            {
                ExecuteNonQuery(CommandType.Text, sbCharacter.ToString(), param);
            }

            // myCommand.CommandType = CommandType.Text;
            // myCommand.CommandText = "SET character_set_client =gbk";
            // conn.Open();
            // myCommand.ExecuteNonQuery();
            // myCommand.CommandText ="SET character_set_results = gbk";
            // myCommand.ExecuteNonQuery();
            // myCommand.CommandText ="SET character_set_connection  = gbk";
            // myCommand.ExecuteNonQuery();

            command = new MySqlCommand();
            {
                command.Connection = connection;
                command.CommandText = p_CommandText;
                command.CommandType = p_CommandType;
                command.CommandTimeout = this._commandTimeout;
                command.Transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
               
            };
            if ((p_Parameters != null) && (p_Parameters.Length > 0))
            {
                foreach (TableTypeParameter parameter in p_Parameters)
                {
                    if (parameter != null)
                    {
                        command.Parameters.AddWithValue(parameter.ParameterName, parameter.ParameterValue);
                    }
                }
            }
            adapter = new MySqlDataAdapter();
            {
                adapter.SelectCommand = command;
            };
            adapter.Fill(ds);
            


        }
        catch (Exception exception)
        {
            if(command != null
                && command.Transaction != null
                )
            {
                command.Transaction.Rollback();
            }
            throw exception;
        }
        finally
        {
            CloseConnection(connection);
            if (command != null)
            {
                command.Dispose();
            }
            if (adapter != null)
            {
                adapter.Dispose();
            }
        }
    }

    public int Fill(DataSet ds, CommandType p_CommandType, string p_CommandText, TableTypeParameter[][] p_Parameters)
    {
        int result = 0;
        MySqlConnection connection = null;
        MySqlCommand command = null;
        MySqlTransaction transaction = null;
        try
        {
            connection = new MySqlConnection();
            {
                connection.ConnectionString = this._sqlConnStr;
            };

            OpenConnection(connection);
            transaction = connection.BeginTransaction();
            for (int i = 0; i < p_Parameters.Length; i++)
            {
                command = new MySqlCommand(p_CommandText, connection, transaction);
                {
                    command.CommandType = p_CommandType;
                    command.CommandTimeout = this._commandTimeout;
                };
                if (p_Parameters[i] != null)
                {
                    if (p_Parameters[i].Length != 0)
                    {
                        //foreach (MySqlParameter parameter in p_Parameters[i])
                        //{
                        //    command.Parameters.Add(parameter);
                        //}
                        foreach (TableTypeParameter parameter in p_Parameters[i])
                        {
                            if (parameter != null)
                            {
                                command.Parameters.AddWithValue(parameter.ParameterName, parameter.ParameterValue);
                            }
                        }
                    }
                    result = command.ExecuteNonQuery();
                }
            }
            transaction.Commit();
        }
        catch (Exception exception)
        {
            if (command != null
                && command.Transaction != null
                )
            {
                command.Transaction.Rollback();
            }
            throw exception;
        }
        finally
        {
            CloseConnection(connection);
            if (command != null)
            {
                command.Dispose();
            }
            if (transaction != null)
            {
                transaction.Dispose();
            }
        }
        return result;
    }

    public void Fill(DataTable dt, CommandType p_CommandType, string p_CommandText, params TableTypeParameter[] p_Parameters)
    {
        MySqlConnection connection = null;
        MySqlCommand command = null;
        MySqlDataAdapter adapter = null;
        try
        {
            connection = new MySqlConnection();
            {
                connection.ConnectionString = this._sqlConnStr;
            };

            OpenConnection(connection);
            command = new MySqlCommand();
            {
                command.Connection = connection;
                command.CommandText = p_CommandText;
                command.CommandType = p_CommandType;
                command.CommandTimeout = this._commandTimeout;
                command.Transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            };
            if ((p_Parameters != null) && (p_Parameters.Length > 0))
            {
                foreach (TableTypeParameter parameter in p_Parameters)
                {
                    if (parameter != null)
                    {
                        command.Parameters.AddWithValue(parameter.ParameterName, parameter.ParameterValue);
                    }
                }
            }
            adapter = new MySqlDataAdapter();
            {
                adapter.SelectCommand = command;
            };
            adapter.Fill(dt);
        }
        catch (Exception exception)
        {
            if (command != null
                && command.Transaction != null
                )
            {
                command.Transaction.Rollback();
            }
            throw exception;
        }
        finally
        {
            CloseConnection(connection);
            if (command != null)
            {
                command.Dispose();
            }
            if (adapter != null)
            {
                adapter.Dispose();
            }
        }
    }

    public void Fill(DataTable dt, CommandType p_CommandType, string p_CommandText, MySqlParameter[] p_Parameters)
    {
        MySqlConnection connection = null;
        MySqlCommand command = null;
        MySqlDataAdapter adapter = null;
        try
        {
            connection = new MySqlConnection();
            {
                connection.ConnectionString = this._sqlConnStr;
            };

            OpenConnection(connection);
            command = new MySqlCommand();
            {
                command.Connection = connection;
                command.CommandText = p_CommandText;
                command.CommandType = p_CommandType;
                command.CommandTimeout = this._commandTimeout;
                command.Transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            };
            if ((p_Parameters != null) && (p_Parameters.Length > 0))
            {
                foreach (MySqlParameter parameter in p_Parameters)
                {
                    if (parameter != null)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
            }
            adapter = new MySqlDataAdapter();
            {
                adapter.SelectCommand = command;
            };
            adapter.Fill(dt);
        }
        catch (Exception exception)
        {
            if (command != null
                && command.Transaction != null
                )
            {
                command.Transaction.Rollback();
            }
            throw exception;
        }
        finally
        {
            CloseConnection(connection);
            if (command != null)
            {
                command.Dispose();
            }
            if (adapter != null)
            {
                adapter.Dispose();
            }
        }
    }

    public void Fill(DataSet ds, string[] p_TableNames, CommandType p_CommandType, string p_CommandText, MySqlParameter[] p_Parameters)
    {
        MySqlConnection connection = null;
        MySqlCommand command = null;
        MySqlDataAdapter adapter = null;
        try
        {
            connection = new MySqlConnection();
            {
                connection.ConnectionString = this._sqlConnStr;
            };

            OpenConnection(connection);
            command = new MySqlCommand();
            {
                command.Connection = connection;
                command.CommandText = p_CommandText;
                command.CommandType = p_CommandType;
                command.CommandTimeout = this._commandTimeout;
                command.Transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            };
            if ((p_Parameters != null) && (p_Parameters.Length > 0))
            {
                foreach (MySqlParameter parameter in p_Parameters)
                {
                    if (parameter != null)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
            }
            adapter = new MySqlDataAdapter();
            {
                adapter.SelectCommand = command;
            };
            if ((p_TableNames != null) && (p_TableNames.Length > 0))
            {
                for (int i = 0; i < p_TableNames.Length; i++)
                {
                    adapter.TableMappings.Add("Table" + ((i == 0) ? string.Empty : i.ToString()), p_TableNames[i]);
                }
            }
            adapter.Fill(ds);
        }
        catch (Exception exception)
        {
            if (command != null
                && command.Transaction != null
                )
            {
                command.Transaction.Rollback();
            }
            throw exception;
        }
        finally
        {
            CloseConnection(connection);
            if (command != null)
            {
                command.Dispose();
            }
            if (adapter != null)
            {
                adapter.Dispose();
            }
        }
    }

    public void Fill(DataTable dt, CommandType p_CommandType, string p_CommandText, TableTypeParameter[] p_TableParams, MySqlParameter[] p_SqlParams)
    {
        MySqlConnection connection = null;
        MySqlCommand command = null;
        MySqlDataAdapter adapter = null;
        try
        {
            connection = new MySqlConnection();
            {
                connection.ConnectionString = this._sqlConnStr;
            };

            OpenConnection(connection);
            command = new MySqlCommand();
            {
                command.Connection = connection;
                command.CommandText = p_CommandText;
                command.CommandType = p_CommandType;
                command.CommandTimeout = this._commandTimeout;
                command.Transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            };
            if ((p_TableParams != null) && (p_TableParams.Length > 0))
            {
                foreach (TableTypeParameter parameter in p_TableParams)
                {
                    if (parameter != null)
                    {
                        command.Parameters.AddWithValue(parameter.ParameterName, parameter.ParameterValue);
                    }
                }
            }
            if ((p_SqlParams != null) && (p_SqlParams.Length > 0))
            {
                foreach (MySqlParameter parameter in p_SqlParams)
                {
                    if (parameter != null)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
            }
            adapter = new MySqlDataAdapter();
            {
                adapter.SelectCommand = command;
            };
            adapter.Fill(dt);
        }
        catch (Exception exception)
        {
            if (command != null
                && command.Transaction != null
                )
            {
                command.Transaction.Rollback();
            }
            throw exception;
        }
        finally
        {
            CloseConnection(connection);
            if (command != null)
            {
                command.Dispose();
            }
            if (adapter != null)
            {
                adapter.Dispose();
            }
        }
    }

    public int GetRowsCount(string sql, SetOfBookType sobType)
    {
        MySqlHelper mysqlHelper = new MySqlHelper(sobType);

        DataSet ds = new DataSet();

       mysqlHelper.Fill(ds, CommandType.Text, sql);
       if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
       {
           return ds.Tables[0].Rows.Count;
       }
       return 0;
    }

    //public void SqlBulkCopy(DataTable dtSource, string tableName)
    //{
    //    MySqlConnection connection = null;
    //    MySql.Data.MySqlClient.MySqlBulkLoader copy = null;

    //    try
    //    {
    //        connection = new MySqlConnection
    //        {
    //            ConnectionString = this._sqlConnStr
    //        };
    //        copy = new MySql.Data.MySqlClient.MySqlBulkLoader(connection)
    //        {
    //            //BatchSize = dtSource.Rows.Count,
    //            Timeout = 30,
    //            TableName = tableName
    //        };
    //        //copy.WriteToServer(dtSource);
    //        copy.
    //    }
    //    catch (Exception exception)
    //    {
    //        throw exception;
    //    }
    //    finally
    //    {
    //        CloseConnection(connection);
    //        if (copy != null)
    //        {
    //            copy.Close();
    //        }
    //    }
    //}
}
