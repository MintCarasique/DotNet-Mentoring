using System;
using Gtk;
using HelloWorldStandard;

public partial class MainWindow : Gtk.Window
{
    private HelloWorld hello;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        hello = new HelloWorld();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnApplyButtonClicked(object sender, EventArgs e)
    {
        var name = inputEntry.Text;
        resultEntry.Text = hello.ReturnHelloMessage(name);
    }
}
