using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using HelloWorldStandard;

namespace HelloWorldApp
{
    public partial class MainPage : ContentPage
    {
        Label textLabel;
        Entry nameEntry;

        private HelloWorld hello;
        public MainPage()
        {
            hello = new HelloWorld();
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
                textLabel.Text = hello.ReturnHelloMessage(nameEntry.Text);
            }
            else
            {
                textLabel.Text = string.Empty;
            }
            
        }
    }
}
