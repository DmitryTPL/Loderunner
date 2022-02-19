using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Debug = UnityEngine.Debug;

public static class Console
{
    [Conditional("ENABLE_LOGS")]
    public static void Log(this object origin, string message, Func<bool> condition = null, [CallerMemberName] string caller = "")
    {
        Log(message, condition, caller);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogWarning(this object origin, string message, Func<bool> condition = null, [CallerMemberName] string caller = "")
    {
        LogWarning(message, condition, caller);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogError(this object origin, string message, Func<bool> condition = null, [CallerMemberName] string caller = "")
    {
        LogError(message, condition, caller);
    }

    [Conditional("ENABLE_LOGS")]
    public static void Log(string message, Func<bool> condition = null, [CallerMemberName] string caller = "")
    {
        if (condition?.Invoke() ?? true)
        {
            Debug.Log(message);
        }
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogWarning(string message, Func<bool> condition = null, [CallerMemberName] string caller = "")
    {
        if (condition?.Invoke() ?? false)
        {
            Debug.LogWarning(message);
        }
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogError(string message, Func<bool> condition = null, [CallerMemberName] string caller = "")
    {
        if (condition?.Invoke() ?? false)
        {
            Debug.LogError(message);
        }
    }
}