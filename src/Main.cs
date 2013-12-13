using System;
using System.Windows.Forms;

namespace MangaReader
{
  static class Program
  {
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MangaReaderForm());
      // Application.Run(new Form1());
    }
  }
}