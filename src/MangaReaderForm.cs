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
    private System.ComponentModel.IContainer components = null;

    private Manga CurrentManga;

    private PictureBox PictureBox1;

    int CurrentPage = 0;

    Thread LoadingThread;

    public MangaReaderForm()
    {
      CurrentManga = new Manga();

      PictureBox1 = new PictureBox();

      LoadingThread = new Thread(new ThreadStart(CurrentManga.LoadManga));
      LoadingThread.Start();
      
      InitializeComponent();

    }

    protected override void Dispose(bool disposing)
    {
      LoadingThread.Abort();
      LoadingThread.Join();
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    protected override void OnResize(EventArgs e)
    {
      PictureBox1.Size = this.ClientSize;
    }

    private void NextImage(object sender, EventArgs e)
    {
      Console.WriteLine("Start");
      CurrentPage++;
      // ReadWebPage(Client.DownloadString("http://www.mangareader.net/one-piece/1/" + CurrentPage));


      // Change to CurrentPage
    }

    private void KeyPressed(object sender, KeyEventArgs e)
    {
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


      if (CurrentManga.Loaded)
      {
        LoadingThread.Abort();
        LoadingThread.Join();
      }
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();

      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 600);
      this.Name = "MangaReader";
      this.Text = "MangaReader";
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyPressed);

      while(CurrentManga.Images.Count < 1);

      PictureBox1.Location = new Point(0, 0); //20 from left and 100 from top
      PictureBox1.Size = this.ClientSize;
      PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
      PictureBox1.Image = (Image)CurrentManga.Images[CurrentPage];
      this.Controls.Add(PictureBox1);

      this.ResumeLayout(false);
    }
  }
}