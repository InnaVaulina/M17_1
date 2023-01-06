using System;


namespace M17Library1.AVM.Interfaces
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
