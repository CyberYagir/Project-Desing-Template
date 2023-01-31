using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;

namespace YagirDev
{
    public static class BackupperPrefs
    {
        public enum EditorPrefsValues
        {
            BackupNumber,
            MaxBackupsNumber,
            BackupTime,
            StartTime
        }

        public static DateTime ConvertedStartTime => DateTime.Parse(EditorPrefs.GetString(Pref(EditorPrefsValues.StartTime), DateTime.Now.ToString("G")));

        public static Dictionary<EditorPrefsValues, object> StartValues = new Dictionary<EditorPrefsValues, object>()
        {
            {EditorPrefsValues.BackupNumber, 0},
            {EditorPrefsValues.MaxBackupsNumber, 10},
            {EditorPrefsValues.BackupTime, 300},
            {EditorPrefsValues.StartTime, ConvertedStartTime}
        };

        public static string Pref(EditorPrefsValues prefValues)
        {
            return prefValues.ToString();
        }
    }
}