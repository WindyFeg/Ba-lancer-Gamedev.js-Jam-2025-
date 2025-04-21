namespace Base
{
    [System.Serializable]
    public class StatConfig
    {
        public StatType BaseStat;
        public StatType[] LinkedStat;
        public float Ratio = 1f; // e.g. 1 Attack = 2 HP => ratio = 2
        // When source increases by +X, target changes by -X * ratio
    }
}