using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WiredBrainCoffee.CustomersApp.DataAccess;
using WiredBrainCoffee.CustomersApp.Model;

namespace WiredBrainCoffee.CustomersApp
{
    public sealed partial class MainPage : Page
    {
        private CustomerDataProvider _customerDataProvider;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
            App.Current.Suspending += CurrentOnSuspending;
            _customerDataProvider = new CustomerDataProvider();
        }

        private async void CurrentOnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral(); //opóźniamy zamknięcie aplikacji

            await _customerDataProvider.SaveCustomersAsync(customerListView.Items.OfType<Customer>());

            deferral.Complete(); //poinformuj, że można już zamknąć
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            customerListView?.Items?.Clear();

            var customers = await _customerDataProvider.LoadCustomersAsync();
            foreach (var customer in customers)
            {
                customerListView.Items.Add(customer);
            }
        }

        private void ButtonAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customer = new Customer{FirstName = "New"};
            customerListView.Items.Add(customer);
            customerListView.SelectedItem = customer;
        }

        private void ButtonRemoveCustomer_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = customerListView.SelectedItem as Customer;
            if (selectedCustomer != null)
            {
                customerListView.Items.Remove(selectedCustomer);
            }
        }

        private void ButtonMoveCustomer_Click(object sender, RoutedEventArgs e)
        {
            int column = Grid.GetColumn(customerListGrid); //pobierz kolumnę do której należy Grid
            int newColumn = column == 0 ? 2 : 0;

            Grid.SetColumn(customerListGrid, newColumn); // ustaw kolumnę dla grida

            SymbolIcon.Symbol = newColumn == 0 ? Symbol.Forward : Symbol.Back;
        }

        private void CustomerListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCustomer = customerListView.SelectedItem as Customer;
            CustomerDetailControl.Customer = selectedCustomer;

        }
        
    }
}
