all:
	mcs src/Main.cs src/MangaReaderForm.cs src/Manga.cs src/Form1.cs \
	src/MangaList.cs \
	-target:winexe -out:MangaReader.exe \
	-r:System.Windows.Forms.dll \
	-r:System.Data.dll -r:System.Drawing.dll

run: all
	MangaReader.exe