using System;
using System.Collections;
using System.Net;

namespace MangaReader
{
  public class MangaList
  {
    WebClient Client;
    public ArrayList Mangas {get; private set;}

    string URL;

    public MangaList(string list)
    {
      Client = new WebClient();
      Mangas = new ArrayList();

      URL = list;
      LoadList();
    }

    public void LoadList()
    {
      string htmlCode = Client.DownloadString(URL);

      string[] letters = htmlCode.Split(new string[]
                                      { "<ul class=\"series_alpha\">" } ,
                                      100,
                                      StringSplitOptions.RemoveEmptyEntries);

      // for (int i = 0; i < letters.Length; i++)
      for (int i = 1; i < letters.Length; i++)
      {
        string[] lists = letters[i].Split(new string[] { "</ul>" },
                                        StringSplitOptions.None);

        string[] lines = lists[0].Split(new string[] { "\r\n", "\n" },
                                        StringSplitOptions.None);

        for (int j = 0; j < lines.Length; j++)
        {
          if (lines[j].IndexOf("<li>", StringComparison.Ordinal) != -1)
          {
            string[] names = lines[j].Split(new string[] { "<li>" },
                                      StringSplitOptions.RemoveEmptyEntries);
            for (int k = 0; k < names.Length; k++)
            {
              int start = names[k].IndexOf(">", StringComparison.Ordinal);
              int end = names[k].IndexOf("</a>", StringComparison.Ordinal); 

              string temp = names[k].Substring(start+1, end-start-1);
              Mangas.Add(temp);
            } 
          }
        }
      }
    }
  }
}