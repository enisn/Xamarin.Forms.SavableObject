using Sample.SavableObject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sample.SavableObject
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{

			InitializeComponent();
		}
        private void SingleDataClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SimplePage());
        }
        private void ListDataClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListPage());
        }

    }
}
