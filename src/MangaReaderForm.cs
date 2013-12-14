using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MangaReader
{
  public class MangaReaderForm : Form
  {
    enum CurrentForm
    {
      None,
      ListManga,
      ChaptersManga,
      ReadManga
    };

    private System.ComponentModel.IContainer components = null;

    private Manga CurrentManga;
    private MangaList List;

    private PictureBox PictureBox1;
    private ListBox ListBox1;
    private ListBox ChaptersList;

    private int CurrentPage = 0;
    private string CurrentMangaURL = "";
    private int CurrentChapter = 1;
    private CurrentForm CurrentScreen;

    Thread LoadingThread;

    public MangaReaderForm()
    {
      CurrentManga = new Manga();
      List = new MangaList("http://www.mangareader.net/alphabetical");

      PictureBox1 = new PictureBox();
      ListBox1 = new ListBox();
      ChaptersList = new ListBox();

      InitializeComponent();
      InitializeMangaList();
    }

    protected override void Dispose(bool disposing)
    {
      if (LoadingThread != null)
      {
        LoadingThread.Abort();
        LoadingThread.Join();
      }
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    protected override void OnResize(EventArgs e)
    {
      PictureBox1.Size = this.ClientSize;
      ListBox1.Size = this.ClientSize;
    }

    private void MangaLoaded(bool loaded)
    {
      if (loaded)
      {
        Console.WriteLine("Manga Loaded");
        LoadingThread.Abort();
        LoadingThread.Join();
      }
      else
      {
        Console.WriteLine("Failed To Load Manga");        

        InitializeMangaList();

        LoadingThread.Abort();
        LoadingThread.Join();
      }
    }

    private void KeyPressed(object sender, KeyEventArgs e)
    {
      if (CurrentScreen == CurrentForm.ListManga)
      {
        if (e.KeyCode == Keys.Enter)
        {
          InitializeChapters((string)ListBox1.SelectedItem);
        }
        else if (e.KeyCode == Keys.Escape)
        {
          this.Close();
        }
      }
      else if (CurrentScreen == CurrentForm.ChaptersManga)
      {
        if (e.KeyCode == Keys.Enter)
        {
          CurrentChapter = ChaptersList.Items.Count - ChaptersList.SelectedIndex;
          InitializeManga();
        }
        else if (e.KeyCode == Keys.Escape)
        {
          InitializeMangaList();
        }
      }
      else if (CurrentScreen == CurrentForm.ReadManga)
      { 
        if (e.KeyCode == Keys.Escape)
        {
          LoadingThread.Abort();
          LoadingThread.Join();
          InitializeChapters();
        }
        if (e.KeyCode == Keys.Right)
        {
          if (CurrentPage < CurrentManga.Images.Count - 1)
          {
            CurrentPage++;
          }
        }
        else if (e.KeyCode == Keys.Left)
        {
          if (CurrentPage > 0)
          {
            CurrentPage--;
          }
        }

        PictureBox1.Image = (Image)CurrentManga.Images[CurrentPage];
        PictureBox1.Refresh();
      }
    }

    private void DoubleClicked(object sender, EventArgs e)
    {
      if (CurrentScreen == CurrentForm.ListManga)
      {
        InitializeManga();
      }
      else if (CurrentScreen == CurrentForm.ChaptersManga)
      {
        CurrentChapter = ChaptersList.Items.Count - ChaptersList.SelectedIndex;
        InitializeManga();
      }
    }

    private void InitializeMangaList()
    {
      this.SuspendLayout();
      CurrentScreen = CurrentForm.ListManga;

      this.Controls.RemoveByKey("PictureBox1");
      this.Controls.RemoveByKey("ChaptersList");

      // 
      // ListBox1
      // 
      this.ListBox1.FormattingEnabled = true;
      this.ListBox1.Location = new System.Drawing.Point(0, 0);
      this.ListBox1.Name = "ListBox1";
      this.ListBox1.Size = this.ClientSize;
      this.ListBox1.TabIndex = 0;
      this.ListBox1.DoubleClick += new EventHandler(this.DoubleClicked);
      this.ListBox1.KeyDown += new KeyEventHandler(this.KeyPressed);

      if (ListBox1.Items.Count < 1) {
        ListBox1.BeginUpdate();
        for (int i = 1; i < List.Mangas.Count; i++)
        {
          ListBox1.Items.Add(List.Mangas[i]);
        }
        ListBox1.EndUpdate();
      }
      this.Controls.Add(ListBox1);

      this.ResumeLayout();
    }

    private void InitializeChapters(string url)
    {
      CurrentMangaURL = url.ToLower();
      CurrentMangaURL = CurrentMangaURL.Replace(" ", "-");
      CurrentMangaURL = CurrentMangaURL.Replace("/", "");
      CurrentMangaURL = CurrentMangaURL.Replace(",", "");
      CurrentMangaURL = CurrentMangaURL.Replace(".", "");
      CurrentMangaURL = CurrentMangaURL.Replace("+", "");
      CurrentMangaURL = CurrentMangaURL.Replace("!", "");
      CurrentMangaURL = CurrentMangaURL.Replace("'", "");
      CurrentMangaURL = CurrentMangaURL.Replace(";", "");
      CurrentMangaURL = CurrentMangaURL.Replace(":", "");
      CurrentMangaURL = CurrentMangaURL.Replace("(", "");
      CurrentMangaURL = CurrentMangaURL.Replace(")", "");
      CurrentMangaURL = CurrentMangaURL.Replace("@", "");
      CurrentMangaURL = CurrentMangaURL.Replace("#", "");
      CurrentMangaURL = CurrentMangaURL.Replace("%", "");
      CurrentMangaURL = CurrentMangaURL.Replace("&", "");
      CurrentMangaURL = CurrentMangaURL.Replace("*", "");
      CurrentMangaURL = CurrentMangaURL.Replace("?", "");
      CurrentMangaURL = "http://www.mangareader.net/" + CurrentMangaURL;

      this.SuspendLayout();
      CurrentScreen = CurrentForm.ChaptersManga;

      this.Controls.RemoveByKey("ListBox1");
      this.Controls.RemoveByKey("PictureBox1");

      // 
      // ChaptersList
      // 
      this.ChaptersList.FormattingEnabled = true;
      this.ChaptersList.Location = new System.Drawing.Point(0, 0);
      this.ChaptersList.Name = "ChaptersList";
      this.ChaptersList.Size = this.ClientSize;
      this.ChaptersList.TabIndex = 0;
      this.ChaptersList.DoubleClick += new EventHandler(this.DoubleClicked);
      this.ChaptersList.KeyDown += new KeyEventHandler(this.KeyPressed);

      // Read in number of chapters
      int NumberOfChapters = 0;

      WebClient Client = new WebClient();

      string htmlCode = Client.DownloadString(CurrentMangaURL);

      string[] halves = htmlCode.Split(new string[] { "LATEST CHAPTERS" },
                                       StringSplitOptions.None);
      halves = halves[1].Split(new string[] { "</a>" },
                            StringSplitOptions.None);

      int start = halves[0].LastIndexOf(" ", StringComparison.Ordinal);

      NumberOfChapters = int.Parse(halves[0].Substring(
                         start+1, halves[0].Length - (start+1)));

      ChaptersList.Items.Clear();

      ChaptersList.BeginUpdate();
      for (int i = NumberOfChapters; i > 0; i--)
      {
        ChaptersList.Items.Add("Chapter " + i);
      }
      ChaptersList.EndUpdate();

      this.Controls.Add(ChaptersList);

      this.ResumeLayout();

    }

    private void InitializeManga()
    {
      CurrentManga = new Manga();
      CurrentPage = 0;

      this.SuspendLayout();
      CurrentScreen = CurrentForm.ReadManga;

      this.Controls.RemoveByKey("ListBox1");
      this.Controls.RemoveByKey("ChaptersList");
      //
      // Picture Box
      //
      LoadingThread = new Thread(() =>
                          CurrentManga.LoadManga(MangaLoaded,
                                                 CurrentMangaURL,
                                                 CurrentChapter));
      LoadingThread.Start();

      while(CurrentManga.Images.Count < 1);

      PictureBox1.Location = new Point(0, 0);
      PictureBox1.Name = "PictureBox1";
      PictureBox1.Size = this.ClientSize;
      PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
      PictureBox1.Image = (Image)CurrentManga.Images[CurrentPage];
      this.Controls.Add(PictureBox1);

      this.ResumeLayout();
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();

      //
      // Form
      //
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 600);
      this.Name = "MangaReader";
      this.Text = "MangaReader";
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyPressed);

      //
      // Picture Box
      //
      // while(CurrentManga.Images.Count < 1);

      // PictureBox1.Location = new Point(0, 0); //20 from left and 100 from top
      // PictureBox1.Size = this.ClientSize;
      // PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
      // PictureBox1.Image = (Image)CurrentManga.Images[CurrentPage];
      // this.Controls.Add(PictureBox1);

      this.ResumeLayout(false);
    }
  }
}
