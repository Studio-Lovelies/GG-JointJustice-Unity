namespace SaveFiles
{
    public static class ChapterExtensions
    {
        public static void AddChapter(ref this SaveData.Progression.Chapters currentChapters, SaveData.Progression.Chapters newChapterToAdd)
        {
            currentChapters |= newChapterToAdd;
        }
    }
}