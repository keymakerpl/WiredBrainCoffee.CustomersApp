using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using WiredBrainCoffee.CustomersApp.Model;

namespace WiredBrainCoffee.CustomersApp.DataAccess
{
    public class CustomerDataProvider
    {
        private static readonly string _customerFileName = "customers.json";
        private static readonly StorageFolder _localFolder = ApplicationData.Current.LocalFolder;

        public async Task<IEnumerable<Customer>> LoadCustomersAsync()
        {
            var storageFile = await _localFolder.TryGetItemAsync(_customerFileName) as StorageFile;
            List<Customer> customerList = null;

            if (storageFile == null) // jeśli brak pliku to utwórz nową listę
            {
                customerList = new List<Customer>
                {
                    new Customer{FirstName = "Jan", LastName = "Nowak", IsDeveloper = true},
                    new Customer{FirstName = "Michał", LastName = "Kwiatek", IsDeveloper = true},
                    new Customer{FirstName = "Alina", LastName = "Halny", IsDeveloper = true},
                    new Customer{FirstName = "Wojtek", LastName = "Gawron", IsDeveloper = false},
                    new Customer{FirstName = "Anna", LastName = "Wróbel", IsDeveloper = true},
                    new Customer{FirstName = "Mirosław", LastName = "Lis", IsDeveloper = false}
                };
            }
            else //inaczej czytaj z dżejsona
            {
                using (var stream = await storageFile.OpenAsync(FileAccessMode.Read)) //stream
                {
                    using (var dataReader = new DataReader(stream))
                    {
                        await dataReader.LoadAsync((uint) stream.Size); //czytaj dane ze stream
                        var json = dataReader.ReadString((uint) stream.Size); //czytaj string
                        customerList = JsonConvert.DeserializeObject<List<Customer>>(json); //deserializuj string jsona
                    }
                }
            }

            return customerList;
        }

        public async Task SaveCustomersAsync(IEnumerable<Customer> customers)
        {
            var storageFile =
                await _localFolder.CreateFileAsync(_customerFileName, CreationCollisionOption.ReplaceExisting);

            using (var stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite)) //otwórz stream
            {
                using (var dataWriter = new DataWriter(stream))
                {
                    var json = JsonConvert.SerializeObject(customers, Formatting.Indented);
                    dataWriter.WriteString(json);
                    await dataWriter.StoreAsync();
                }
            }
        }
    }
}
