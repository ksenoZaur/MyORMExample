using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;

namespace MyORM
{
    // Создает запросы к БД
    // используя объекты SomeRecord 
    // и возвращает результат ввиде...
    public class HubORM
    {
        public List<SomeRecord> Rules { get; }
        public string ConnectionString { get; }

        public HubORM(List<SomeRecord> Rules, string connectionString)
        {
            this.Rules = Rules;
        }

        public HubORM(string pathToRuleXML, string connectionString)
        {
            if( string.IsNullOrEmpty(pathToRuleXML))
                throw new NullReferenceException("Объект pathToRuleXML должен содержать путь " +
                                                 "к XML-файлу содержащему список правил");
            try
            {
                Rules = ParserXML.Parse(pathToRuleXML);
                ConnectionString = connectionString;
            }
            catch (Exception exc)
            {
                Console.Error.WriteLine( exc.Message + '\n' + '\r' + exc.StackTrace);
                throw;
            }
        }

        public List<ClassName> GetAll<ClassName>()
        { 
            List<ClassName> result = new List<ClassName>();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    
                        // Получаем правила, относящиеся к данному классу    
                    var Rules = this.Rules.Where(rul => rul.ClassName == typeof(ClassName).Name);
                    
                    if(Rules.Count() <= 0)
                        throw new Exception("Не указано правил для класса " + typeof(ClassName).Name);

                    string tableName = Rules.First().TableName;
                    
                    if( String.IsNullOrEmpty(tableName) )
                        throw new Exception("Правило не содержит имя целевой таблицы");
                    
                    #region Подготовка запроса
                    
                    string query = "SELECT * FROM " + tableName;
                    
                    foreach (var currColumn in Rules)
                    {
                        query = query.Replace("*", currColumn.ColumnName + ",*");
                    }

                    query = query.Replace(",*", "");
                    
                    #endregion
                    
                    SqlCommand cmd = new SqlCommand(query);
                    cmd.Connection = connection;

                    #region  Обработка полученной информации
                    
                    SqlDataReader dataRider = cmd.ExecuteReader();
                    Dictionary<String, object> props = new Dictionary<string, object>();
                    
                    while (dataRider.Read())
                    {
                        props.Clear();
                        
                        foreach (var col in Rules)
                        {
                            // Пары вида: Название свойства - значение свойства
                            props.Add(col.PropName, dataRider[col.ColumnName]);
                        }

                        Type a = typeof(ClassName);
                        var constructor = a.GetConstructors().FirstOrDefault();
                        
                        if( constructor == null )
                            throw new Exception("Не удалось найти конструктор класса " + a.Name);
                        
                        // Создаем очередной объект
                        object resObject = constructor.Invoke(new object[0]);
                        
                        // Заполняем параметрами
                        foreach (var prop in props)
                        {
                            resObject.GetType().GetProperty(prop.Key).SetValue(resObject,prop.Value);
                        }
                        
                        result.Add((ClassName)resObject);
                    }
                    
                    #endregion
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
        }
    }
}