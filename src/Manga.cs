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

      Name = "";
      Chapter = 0;
    }

    public delegate void Reportback(bool loaded);

    public void LoadManga(Reportback callback, string mangaName, int chapter)
    {
    Name = mangaName;
    Chapter = chapter;

    Console.WriteLine("Loading " + Name + " Chapter " + Chapter);

    while(!Loaded)
      {
        Loaded = !ReadWebPage(Name + "/" + Chapter +
                              "/" + (Images.Count+1));          
      }
      callback(true);
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