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

        private async void ButtonAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("Customer Added");
            await messageDialog.ShowAsync();
        }

        private async void ButtonRemoveCustomer_Click(object sender, RoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("Customer Removed");
            await messageDialog.ShowAsync();
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
            var customer = customerListView.SelectedItem as Customer;

            txtFirstName.Text = customer?.FirstName ?? "";
            txtLastName.Text = customer?.LastName ?? "";
            isDeveloperBox.IsChecked = customer?.IsDeveloper ?? false;
        }

        private void TxtFirstName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCustomer();
        }

        private void TxtLastName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
           UpdateCustomer();
        }

        private void IsDeveloperBox_OnCheckedChanged(object sender, RoutedEventArgs e)
        {
            UpdateCustomer();
        }

        private void UpdateCustomer()
        {
            var selectedCustomer = customerListView.SelectedItem as Customer;
            if (selectedCustomer != null)
            {
                selectedCustomer.FirstName = txtFirstName.Text;
                selectedCustomer.LastName = txtLastName.Text;
                selectedCustomer.IsDeveloper = isDeveloperBox.IsChecked.GetValueOrDefault();
            }
        }
    }
}
