using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MyORM
{
    // Класс, который занимается разборкой XML
    public class ParserXML
    {
        public static List<SomeRecord> Parse(string path = null)
        {
            List<SomeRecord> someList = new List<SomeRecord>();
            if (String.IsNullOrEmpty(path))
            {
                path = @"Example.xml";
            }
            
            TextReader reader = new StreamReader(path);
            string xmlText = reader.ReadToEnd();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlText);
            XmlElement root = xmlDoc.DocumentElement;

            foreach (XmlNode table in root.ChildNodes)
            {
                var attr = table.Attributes;
                string tableName = attr["Name"].Value;
                string className = attr["TargetClass"].Value;

                foreach (XmlNode column in table.ChildNodes)
                {
                    someList.Add(new SomeRecord(
                        className,    
                        column.Attributes["FieldName"].Value,    
                        tableName,  
                        column.Attributes["ColumnName"].Value    
                    ));
                }
            }
            
            return someList;
        }
    }
}