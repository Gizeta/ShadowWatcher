namespace ShadowWatcher.Battle
{
    public static class BattleAgentHelper
    {
        public static int ToInt(this object o)
        {
            return int.Parse(o.ToString());
        }
    }
}
