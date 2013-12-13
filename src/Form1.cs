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
  public class Form1 : Form
  {
    private System.ComponentModel.IContainer components = null;

    private ListBox ListBox1;

    private MangaList List;

    public Form1()
    {
      List = new MangaList("http://www.mangareader.net/alphabetical");

      InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    protected override void OnResize(EventArgs e)
    {
      this.ListBox1.Size = this.ClientSize;
    }

    private void InitializeComponent()
    {
      this.ListBox1 = new System.Windows.Forms.ListBox();

      this.SuspendLayout();

      // 
      // ListBox1
      // 
      this.ListBox1.FormattingEnabled = true;
      this.ListBox1.Location = new System.Drawing.Point(0, 0);
      this.ListBox1.Name = "ListBox1";
      this.ListBox1.Size = new System.Drawing.Size(800, 600);
      this.ListBox1.TabIndex = 0;

      ListBox1.BeginUpdate();
      for (int i = 1; i < List.Mangas.Count; i++)
      {
        ListBox1.Items.Add(List.Mangas[i]);
      }
      ListBox1.EndUpdate();

      //
      // Form
      //
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 600);
      this.Controls.Add(this.ListBox1);
      this.Name = "MangaReader";
      this.Text = "MangaReader";

      this.ResumeLayout(false);
    }
  }
}
