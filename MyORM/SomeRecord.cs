namespace MyORM
{
    // Отдельная запись в XML - документе
    public class SomeRecord
    {
        // Fields
        private string className;            // Имя класс
        private string propName;             // Имя свойства   
        private string tableName;            // Название таблицы
        private string columnName;           // Название колонки
        
        // Construtor
        public SomeRecord(string className, string propName, string tableName, string columnName)
        {
            this.ClassName = className;
            this.PropName = propName;
            this.TableName = tableName;
            this.ColumnName = columnName;
        }
        
        // Properties
        public string ClassName
        {
            get { return className; }
            set { this.className = value; }
        }

        public string PropName
        {
            get => propName;
            set => propName = value;
        }

        public string TableName
        {
            get => tableName;
            set => this.tableName = value;
        }

        public string ColumnName
        {
            get => columnName;
            set => columnName = value;
        }

        public override string ToString()
        {
            return "Имя класса " + ClassName + ", Имя свойства " + PropName
                   + ", Название таблицы " + TableName + ", Название колонки " + ColumnName;
        }
    }
}