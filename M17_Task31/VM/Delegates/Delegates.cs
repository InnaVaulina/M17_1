using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using M17_Task31.VM.Obj;

namespace M17_Task31.VM.Delegates
{
    // для события RowChoiceNotify в BuyersRow и ProductsRow
    public delegate void RowChoiceHendler(Row row);

    // для события QueryNotify в RequestAVM и ProductsRow
    public delegate void QueryListHendler(ObservableCollection<CellS> list);

    // для передачи элемента по дереву
    public delegate void MyDataGridSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e);

}
