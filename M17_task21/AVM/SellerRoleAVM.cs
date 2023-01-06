using M17Library1.AVM;
using M17Library1.AVM.Command;
using M17Library1.AVM.DBObject;
using M17Library1.AVM.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace M17_task21.AVM
{

    public class SellerRoleAVM : UserRoleAVM, ITableSelect<Row>
    {
        SellerWindow window;                   // окно

        TableAVM workTable;                    // таблица в работе
        TableAVM bTable;
        TableAVM pTable;

        //  ___________________________________счет
        BuyersRow buyer;                              // попупатель 
        ProductsRow product;                          // товар, который будет удален из списка
        ObservableCollection<ProductsRow> products;   // спосок товаров


        /// <summary>
        /// продавец
        /// </summary>
        public User Seller { get { return base.user; } }


        /// <summary>
        /// отображаемая таблица
        /// </summary>
        public TableAVM WorkTable { get { return workTable; } set { workTable = value; OnPropertyChanged("WorkTable"); } }

        /// <summary>
        /// покупатель, сведения о котором отразятся в счете
        /// </summary>
        public BuyersRow Buyer { get { return buyer; } set { buyer = value; OnPropertyChanged("Buyer"); } }

        /// <summary>
        /// список выбранных товаров
        /// </summary>
        public ObservableCollection<ProductsRow> Products { get { return products; } }


        WeirdCommand exit;   // выход
        public WeirdCommand Exit { get { return exit; } }


        WeirdCommand deleteProduct;   // удалить товар из списка в чеке
        public WeirdCommand DeleteProduct { get { return deleteProduct; } }

        WeirdCommand saveNewBuyer;   // запомнить нового покупателя
        public WeirdCommand SaveNewBuyer { get { return saveNewBuyer; } }

        /// <summary>
        /// создает модель для продавца
        /// </summary>
        /// <param name="user">информация о правах пользователя</param>
        /// <param name="strCon">строка подключения</param>
        /// <param name="window">интерфейс пользователя</param>
        public SellerRoleAVM(User user, SqlConnectionStringBuilder strCon, SellerWindow window) :
            base(user, strCon)
        {
            this.window = window;

            foreach (Role r in user.Roles) 
                if(r.RoleName == "seller") 
                {
                    foreach (Permission p in r.Permissions) BObjects.Add(p.BObject);
                    break;
                }
            
            // модели для уплавлеия таблицами
            bTable = new TableAVM(strCon, BObjects[0]);  // tables[0] == Buyers
            pTable = new TableAVM(strCon, BObjects[1]);  // tables[1] == Products
            // подключить обработчик для получения выбранного элемента таблицы
            (bTable.Show as BuyersShowTableAVM).RowChoiceNotify += this.SetRow;
            (pTable.Show as ProductsShowTableAVM).RowChoiceNotify += this.SetRow;
            // подключить модель таблицы для отображения
            workTable = pTable;
            // смена отображаемой таблицы
            this.window.objList.SelectionChanged += WorkTableChange;
            // подключить обработчик выбора условия для выборки данных
            this.window.dataRequest.SelectedCellsChanged += WorkTable.Select.AddNewCondition;
            // подключить обработчик выбара строки в таблице
            this.window.dataShow.SelectedCellsChanged += WorkTable.Show.SelectRow;            
            // выбрать элемент в списке выбранных продуктов (для последующего удаления)
            this.window.productList.SelectedCellsChanged += ChoiceProduct;
            // элементы счета, который составляет продавец
            buyer = new BuyersRow();
            products = new ObservableCollection<ProductsRow>();
            product = new ProductsRow();
            // удаление товара из счета
            deleteProduct = new WeirdCommand(o =>
            {
                if (Products != null) Products.Remove(product);
            });

            exit = new WeirdCommand(o =>
            {
                this.window.ElderWindow.Show();
                this.window.Close();
            });

            saveNewBuyer = new WeirdCommand(o => 
            {
                BuyersRow b = new BuyersRow();
                b.FamilyName = window.familyname.Text;
                b.FirstName = window.firstname.Text;
                b.Patronymic = window.patronymic.Text;
                b.Phone = window.phone.Text;
                b.Email = window.email.Text;
                Buyer = b;
            });

        }


        /// <summary>
        /// смена таблиц
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void WorkTableChange(object sender, SelectionChangedEventArgs args)
        {
            // отключить обработчики событий для сменяемой теблицы
            this.window.dataRequest.SelectedCellsChanged -= WorkTable.Select.AddNewCondition;
            this.window.dataShow.SelectedCellsChanged -= WorkTable.Show.SelectRow;

            // подключить другую таблицу
            switch (args.AddedItems[0].ToString())
            {
                case "Buyers":
                    WorkTable = bTable;
                    break;
                case "Products":
                    WorkTable = pTable;
                    break;

            }
            // подключить обработчики событий для текущей теблицы
            this.window.dataRequest.SelectedCellsChanged += WorkTable.Select.AddNewCondition;
            this.window.dataShow.SelectedCellsChanged += WorkTable.Show.SelectRow;

        }

        /// <summary>
        /// обработает событие RowChoiceNotify объкта BuyersShowTableAVM и ProductsShowTableAVM
        /// выбранные в таблицах строки отобразятся в счете, который составляет продавец
        /// контрвариантный интерфейс ITableSelect<Row>
        /// </summary>
        /// <param name="row"></param>
        public void SetRow(Row row)
        {
            if (row.GetType() == typeof(BuyersRow)) Buyer = row as BuyersRow;
            if (row.GetType() == typeof(ProductsRow))
                Products.Add(row as ProductsRow);
        }

        /// <summary>
        /// выбор элемента в списке подуктов в счете, который составляет продавец
        /// (элемент будет удален из списка при вызове команды DeleteProduct)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ChoiceProduct(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count > 0)
                product = e.AddedCells[0].Item as ProductsRow;

        }

    }
}
