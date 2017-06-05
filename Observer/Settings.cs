namespace ShadowWatcher
{
    public static class Settings
    {
        public const int SHOW_SUMMON_CARD = 1;

        public static bool ShowSummonCard { get; set; } = false;

        public static void Parse(string data)
        {
            int flag = int.Parse(data);
            ShowSummonCard = (flag & SHOW_SUMMON_CARD) > 0;
        }

        public static new string ToString()
        {
            int flag = 0;
            flag |= ShowSummonCard ? SHOW_SUMMON_CARD : 0;
            return flag.ToString();
        }
    }
}
