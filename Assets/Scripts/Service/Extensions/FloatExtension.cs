public static class FloatExtension
{
    public static int ToMilliseconds(this float seconds)
    {
        return (int)(seconds * 1000);
    }
}