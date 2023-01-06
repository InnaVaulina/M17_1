using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace M17Library1.AVM
{


//    Запретите использование следующих символов в вводе
//; Разделитель запросов
//' Разделитель строк символьных данных
//-- Разделитель однострочных комментариев.
/// * ... * / Разделители комментариев.
//xp_ Расширенные в каталоге хранимые процедуры, такие как xp_cmdshell.
//Мы не рекомендуем использовать xp_cmdshell в среде SQL Server.Используйте вместо этой процедуры SQLCLR или постарайтесь найти другие варианты, поскольку xp_cmdshell может представлять определенные риски.
    public class BoolCompare 
    {
        List<Cell> cells;
        List<string> compare;
        int count;

        public int Count { get { return cells.Count; } }

        public BoolCompare() 
        {
            cells = new List<Cell>();
            compare = new List<string>();
            count = 0;
        }

        public void Add(Cell cell) 
        {
            if (cells.Count == 0) { cells.Add(cell); count++; }
        }

        public void Add(Cell cell, string c) 
        {
            if (cells.Count > 0) { cells.Add(cell); compare.Add(c); count++; }
        }

        public void Delete(Cell cell) 
        {
            if (cells.Count > 0)
                if (cells[0].DBColumnName == cell.DBColumnName)
                { 
                    cells.RemoveAt(0); 
                    if(compare.Count>0) compare.RemoveAt(0);
                    count--;
                }
            if (cells.Count > 1)
            for (int i=1; i < cells.Count; i++)
            {
                if (cells[i].DBColumnName == cell.DBColumnName) 
                { cells.RemoveAt(i); compare.RemoveAt(i - 1); count--; }
            }             
        }

        public void DeleteAll() 
        {
            cells.Clear();
            compare.Clear();
        }

        public string BoolCompareToString() 
        {
            string s = "";
            if (cells.Count > 0) 
            { s = $"{cells[0].DBColumnName} {cells[0].Compare} {cells[0].Value}"; }
            if (cells.Count > 1)
               for(int i = 1;i<cells.Count;i++) 
                { s += $" {compare[i - 1]} {cells[i].DBColumnName} {cells[i].Compare} {cells[i].Value}"; }
            return s;
        }
    }
    public class Cell : INotifyPropertyChanged
    {
        string columnName;
        string columnType;
        string compare;
        string value;
        public string DBColumnName { get { return columnName; } }

        public string Compare
        {
            get { return compare; }
            set
            {
                string[] s = value.Split(' ');
                compare = s[s.Length - 1];
                OnPropertyChanged("Compare");
            }
        }
        public string Value
        {
            get { return this.value; }
            set
            {
                string[] s = value.Split(' ');
                if (columnType == "nchar") this.value = $"\'{s[0]}\'";
                else this.value = s[0];
                OnPropertyChanged("Value");
            }
        }

        public Cell(string columnName, string compare, string value)
        {
            this.columnName = columnName;
            this.columnType = "";
            this.compare = compare;
            this.value = value;

        }

        public Cell(string columnName, string columnType)
        {
            this.columnName = columnName;
            this.columnType = columnType;
            Value = "";

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

}
