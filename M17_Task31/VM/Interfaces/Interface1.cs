using System;
using System.ComponentModel;

namespace M17_Task31.VM.Interfaces
{
    internal interface IAVMEditSelect<out T>
    {
        T SelectEditAVM(string query);    
    }

    internal interface IAVMShowSelect<out T>
    {
        T SelectShowAVM();
    }

    public interface ITableSelect<in T>
    {
        void SetRow(T row);
    }

    


}
