using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HelloWorldApp
{
    public partial class MainPage : ContentPage
    {
        Label textLabel;
        Entry nameEntry;
        public MainPage()
        {
            StackLayout stackLayout = new StackLayout();

            nameEntry = new Entry { Placeholder = "Login" };
            nameEntry.TextChanged += nameEntry_TextChanged;

            textLabel = new Label { FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) };

            stackLayout.Children.Add(nameEntry);
            stackLayout.Children.Add(textLabel);

            this.Content = stackLayout;
        }

        private void nameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(nameEntry.Text.Length > 0)
            {
                textLabel.Text = $"Hello, {nameEntry.Text}!";
            }
            else
            {
                textLabel.Text = string.Empty;
            }
            
        }
    }
}
