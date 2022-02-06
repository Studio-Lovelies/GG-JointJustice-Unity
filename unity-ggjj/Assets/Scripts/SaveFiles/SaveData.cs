using System;

namespace SaveFiles
{
    /// <summary>
    /// Passive data structure of all available settings of the game
    /// that are actually serialized and stored, so they can persist
    /// even when the game is closed
    /// </summary>
    public sealed class SaveData
    {
        public int Version;

        /// !!!!! IMPORTANT !!!!!
        /// Increase the value below by one and update <see cref="PlayerPrefsProxy.CreateUpgradedSaveData"/>
        /// to ensure an upgrade path from the previous version to the new version exists,
        /// if you make ANY changes to this class
        /// !!!!! IMPORTANT !!!!!
        public static readonly int LatestVersion = 1;

        public sealed class Progression
        {
            [Flags]
            public enum Chapters
            {
                None = 0,
                Chapter1 = 1 << 0,
                Chapter2 = 1 << 1,
                Chapter3 = 1 << 2,
                BonusChapter1 = 1 << 3
            }

            public Chapters UnlockedChapters;
        }

        public Progression GameProgression;

        /// <summary>
        /// Creates SaveData with default settings
        /// </summary>
        /// <param name="version">Version of this SaveData instance</param>
        public SaveData(int version)
        {
            Version = version;
            GameProgression = new Progression() {
                UnlockedChapters = Progression.Chapters.None
            };
        }
    }
}