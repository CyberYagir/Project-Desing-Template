using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TODO : MonoBehaviour
{
    [System.Serializable]
    public class TODORow
    {
        public enum TaskPriority {
            None, Middle, High
            
        }
        public string text;
        public bool isComplited;
        public TaskPriority priority;
    }
    public List<TODORow> tasks = new List<TODORow>();
    
}
