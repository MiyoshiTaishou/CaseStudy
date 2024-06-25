using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_Debug : MonoBehaviour
{
    private const int MaxLogLines = 50; // �\�����郍�O�̍ő�s��
    private Dictionary<string, LogEntry> logEntries = new Dictionary<string, LogEntry>();
    private GUIStyle guiStyle = new GUIStyle();
    [Header("���O�\���H�H�H�H"), SerializeField]
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

        // �Ō�Ƀ��O��\�������������X�V
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

        // �\�����郍�O�̍s����MaxLogLines�𒴂�����A�Â����O���폜
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