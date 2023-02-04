using M17_Task31.VM.Delegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace M17_Task31
{
    /// <summary>
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        Window elderwindow;
        public Window ElderWindow { get { return elderwindow; } set { elderwindow = value; } }

        public event MyDataGridSelectedCellsChanged SelectedNotify;

        public ManagerWindow()
        {
            InitializeComponent();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            ElderWindow.Show();
        }

        

        private void BuyersDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            SelectedNotify?.Invoke(sender, e);
        }

        private void ProductsDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            SelectedNotify?.Invoke(sender, e);
        }
    }
}
