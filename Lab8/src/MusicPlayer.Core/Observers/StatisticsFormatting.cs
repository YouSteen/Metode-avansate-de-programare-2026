namespace MusicPlayer.Core.Observers;

internal static class StatisticsFormatting
{
    public static string FormatMinutes(double totalMinutes)
    {
        var hours = (int)(totalMinutes / 60);
        var minutes = (int)Math.Round(totalMinutes % 60);
        return hours > 0 ? $"{hours}h {minutes}m" : $"{minutes}m";
    }
}
