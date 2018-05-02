using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sample.SavableObject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public ListPage()
        {
            InitializeComponent();            
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
