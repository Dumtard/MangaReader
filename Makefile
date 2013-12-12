all:
	mcs src/Main.cs src/MangaReaderForm.cs src/Manga.cs \
	-target:winexe -out:MangaReader.exe \
	-r:System.Windows.Forms.dll \
	-r:System.Data.dll -r:System.Drawing.dll

run: all
	MangaReader.exe