using System;
using Gtk;

public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnApplyButtonClicked(object sender, EventArgs e)
    {
        var name = inputEntry.Text;
        resultEntry.Text = $"Hello, {name}!";
    }
}
