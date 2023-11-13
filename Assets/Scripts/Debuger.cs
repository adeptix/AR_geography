using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Debuger
{
    private const string LOG_PREFIX = "ADEPT LOG";

    public static void Log(object message) {
        Debug.LogFormat("{0} - {1}", LOG_PREFIX, message);
    }

    public static void LogFormat(string format, params object[] args) {
        Debug.LogFormat(LOG_PREFIX + " - " + format, args);
    }
}
