using System.Diagnostics.CodeAnalysis;

namespace RwNotificator.Console;

[SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
public static class BeepPlayer
{
    private static System.Timers.Timer? _timer;
    
    public static void PlayAudio()
    {
        if (_timer is not null) return;
        
        _timer = new System.Timers.Timer(3000);
        _timer.Elapsed += (_, _) =>
        {
            System.Console.Beep(659, 125);
            System.Console.Beep(659, 125);
            Thread.Sleep(125);
            System.Console.Beep(659, 125);
            Thread.Sleep(167);
            System.Console.Beep(523, 125);
            System.Console.Beep(659, 125);
            Thread.Sleep(125);
            System.Console.Beep(784, 125);
            Thread.Sleep(375);
            System.Console.Beep(392, 125);
        };
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }
    
    public static void StopAudio()
    {
        if (_timer is null) return;
        
        _timer.Stop();
        _timer.Dispose();
        _timer = null;
    }
}