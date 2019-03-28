using System;
using System.Windows.Forms;

namespace wtf
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      var config = new Configuration();

      var form = new MainForm();
      form.Configure(config);

      Application.Run(form);
    }
  }
}
