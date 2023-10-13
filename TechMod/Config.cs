namespace TechMod;

public static class Config
{
    public struct Range
    {
        public float min;
        public float max;

        public Range(float min, float max)
        {
            this.min = min;
            this.max = max;
            if (min > max)
                this.min = max;
        }
    }
    public static string DatabasePath = "C:\\Users\\super\\source\\repos\\TechMod\\TechMod\\data.db";
    public static float MinutesToVote = 0.55f;
    public static int BarLength = 27;

    public static Range MuteUserRange = new Range(0.5f, 30.0f);
    public static Range ClearRange = new Range(10, 200);
    public static Range LockChannelRange = new Range(0.5f, 30.0f);
}