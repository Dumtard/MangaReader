using System;
using System.Collections;
using System.Drawing;
using System.Net;

namespace MangaReader
{
  public class Manga
  {
    WebClient Client;
    public ArrayList Images {get; private set;}

    public bool Loaded {get; private set;}

    public int NumberOfPages {get; private set;}

    private string Name;
    private int Chapter;

    public Manga()
    {
      Client = new WebClient();
      Images = new ArrayList();
      Loaded = false;
      NumberOfPages = 0;

      Name = "one-piece";
      Chapter = 1;
    }

    public void LoadManga()
    {
      while(!Loaded)
      {
        Loaded = !ReadWebPage("http://www.mangareader.net/" +
                              Name + "/" + Chapter +
                              "/" + (Images.Count+1));          
      }
    }

    private bool ReadWebPage(string url)
    {
      string htmlCode;
      try
      {
        htmlCode = Client.DownloadString(url);
      }
      catch (WebException)
      {
        Console.WriteLine("Web Exception");
        return false;
      }

      int start = htmlCode.IndexOf("id=\"img\"", StringComparison.Ordinal);
      int end = htmlCode.IndexOf("alt=", StringComparison.Ordinal);

      string temp = htmlCode.Substring(start, end-start);
      start = temp.IndexOf("src=", StringComparison.Ordinal) + 5;
      temp = temp.Substring(start, temp.Length-start-2);

      var request = WebRequest.Create(temp);

      using (var response = request.GetResponse())
      using (var stream = response.GetResponseStream())
      {
        Images.Add(Bitmap.FromStream(stream));
      }

      return true;
    }
  }
}