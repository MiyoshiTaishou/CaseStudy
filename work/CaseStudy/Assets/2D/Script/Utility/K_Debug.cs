using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_Debug : MonoBehaviour
{
    private const int MaxLogLines = 50; // 表示するログの最大行数
    private Dictionary<string, LogEntry> logEntries = new Dictionary<string, LogEntry>();
    private GUIStyle guiStyle = new GUIStyle();
    [Header("ログ表示？？？？"), SerializeField]
    private bool showLogInGame = false;

    private float lastLogTime = 0f;

    private void Start()
    {
        guiStyle.fontSize = 20;
        guiStyle.normal.textColor = Color.white;
    }

    private void OnGUI()
    {
#if UNITY_STANDALONE_WIN
        if (showLogInGame && Input.GetKeyDown(KeyCode.Alpha0))
        {
            showLogInGame = !showLogInGame;
        }

        if (showLogInGame)
        {
            string combinedLogText = GetCombinedLogText();
            GUI.Label(new Rect(10, 10, Screen.width, Screen.height), combinedLogText, guiStyle);
        }
#endif
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void Update()
    {
        if (showLogInGame && Time.time - lastLogTime > 3f)
        {
            logEntries.Clear();
        }
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string logKey = stackTrace;

        if (logEntries.ContainsKey(logKey))
        {
            logEntries[logKey].Count++;
        }
        else
        {
            logEntries[logKey] = new LogEntry
            {
                Message = logString,
                Count = 1
            };
        }

        // 最後にログを表示した時刻を更新
        lastLogTime = Time.time;
    }

    private string GetCombinedLogText()
    {
        List<string> combinedLogLines = new List<string>();

        foreach (var entry in logEntries)
        {
            string logLine = entry.Value.Message;
            if (entry.Value.Count > 1)
            {
                logLine += $" (x{entry.Value.Count})";
            }
            combinedLogLines.Add(logLine);
        }

        // 表示するログの行数がMaxLogLinesを超えたら、古いログを削除
        if (combinedLogLines.Count > MaxLogLines)
        {
            combinedLogLines = combinedLogLines.GetRange(combinedLogLines.Count - MaxLogLines, MaxLogLines);
        }

        return string.Join("\n", combinedLogLines);
    }

    private class LogEntry
    {
        public string Message;
        public int Count;
    }
}