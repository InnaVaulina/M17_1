using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace M17_Task31.VM.Obj
{

    public class Cell : INotifyPropertyChanged
    {
        string columnName;
        string compare;
        string value;

        public string DBColumnName 
        { 
            get { return columnName; } 
            set { columnName = value; }
        }



        public string Compare
        {
            get { return compare; }
            set
            {
                compare = value;
                OnPropertyChanged("Compare");
            }
        }
        public string Value
        {
            get { return this.value; }
            set
            {
                string[] s = value.Split(' ');
                if (compare == "между") this.value = $"{s[0]} {s[1]}";
                else this.value = $"{s[0]}";
                OnPropertyChanged("Value");
            }
        }

        public Cell(string columnName, string compare, string value)
        {
            this.columnName = columnName;
            this.compare = compare;
            this.value = value;

        }


        public Cell(string columnName)
        {
            this.columnName = columnName;
            compare = "";
            Value = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }


    public class CompareItem 
    {
        public string Compare { get; set; }
    }

    public class CellS : Cell
    {
        public string ColumnType { get; set; }
        public ObservableCollection<CompareItem> CompareList { get; set; }

        ObservableCollection<CompareItem> compareIntList = new ObservableCollection<CompareItem>() 
        { 
            new CompareItem(){ Compare = "="},
            new CompareItem(){ Compare = "<"},
            new CompareItem(){ Compare = ">"},
            new CompareItem(){ Compare = "между"},
            new CompareItem(){ Compare = ""}
        };

        ObservableCollection<CompareItem> compareStringList = new ObservableCollection<CompareItem>() 
        { 
            new CompareItem(){ Compare = "="},
            new CompareItem(){ Compare = "начинается с"},
            new CompareItem(){ Compare = ""}
        };


        public CellS(string columnName, string columnType): base(columnName) 
        {
            ColumnType = columnType;
            switch (columnType) 
            {
                case "System.Int32":
                    CompareList = compareIntList;
                    break;
                case "System.Nullable`1[System.Int32]":
                    CompareList = compareIntList;
                    break;
                case "System.String":
                    CompareList = compareStringList;
                    break;
            }
            
        }

    }

}
