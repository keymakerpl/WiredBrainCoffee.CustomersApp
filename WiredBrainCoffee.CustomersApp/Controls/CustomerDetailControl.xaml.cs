using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WiredBrainCoffee.CustomersApp.Model;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WiredBrainCoffee.CustomersApp.Controls
{
    public sealed partial class CustomerDetailControl : UserControl
    {
        private Customer _customer;

        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                txtFirstName.Text = Customer?.FirstName ?? "";
                txtLastName.Text = Customer?.LastName ?? "";
                isDeveloperBox.IsChecked = Customer?.IsDeveloper ?? false;
            }
        }

        public CustomerDetailControl()
        {
            this.InitializeComponent();
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
            if (Customer != null)
            {
                Customer.FirstName = txtFirstName.Text;
                Customer.LastName = txtLastName.Text;
                Customer.IsDeveloper = isDeveloperBox.IsChecked.GetValueOrDefault();
            }
        }
    }
}
