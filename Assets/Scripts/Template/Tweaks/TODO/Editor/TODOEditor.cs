using System.Linq;
using Template.Editor;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(TODO))]
public class TODOEditor : EditorTweaks
{
    private TODO script;
    public static bool InfoMode;
    private void OnEnable()
    {
        script = target as TODO;

        InfoMode = EditorPrefs.GetBool("InfoMode");
    }

    private void OnDisable()
    {
        EditorPrefs.SetBool("InfoMode", InfoMode);
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Tasks:");
            if (GUILayout.Button("Move Mode"))
            {
                InfoMode = !InfoMode;
            }
        }
        GUILayout.EndHorizontal();
        
        GUIStyle style = new GUIStyle(EditorStyles.textArea);
        style.wordWrap = true;

        foreach (var task in script.tasks)
        {
            var oldEnabled = GUI.enabled;
            
            int index = script.tasks.FindIndex(x => x == task);
            if (index != 0)
            {
                if (!script.tasks[index - 1].isComplited && script.tasks[index].isComplited)
                {
                    HorizontalLine(Color.gray);
                }
            }

            var oldColor = GUI.backgroundColor;
            if (!task.isComplited)
            {
                switch (task.priority)
                {
                    case TODO.TODORow.TaskPriority.Middle:
                        GUI.backgroundColor = Color.yellow;
                        break;

                    case TODO.TODORow.TaskPriority.High:
                        GUI.backgroundColor = Color.red;
                        break;
                }
            }

            GUILayout.BeginHorizontal();
            {
                GUI.enabled = !task.isComplited;
                task.text = EditorGUILayout.TextArea(task.text,style, GUILayout.MinWidth(0), GUILayout.ExpandHeight(true));
                GUILayout.BeginVertical(GUILayout.MaxWidth(60));
                {
                    if (InfoMode)
                    {
                        GUI.enabled = true;

                        GUILayout.BeginHorizontal();
                        {
                            if (script.tasks[0] != task)
                            {
                                if (!script.tasks[index].isComplited)
                                {
                                    if (GUILayout.Button("↑"))
                                    {
                                        TODO.TODORow row = script.tasks[index - 1];
                                        script.tasks[index - 1] = task;
                                        script.tasks[index] = row;
                                        break;
                                    }
                                }
                            }

                            if (script.tasks[script.tasks.Count - 1] != task)
                            {
                                if (!script.tasks[index + 1].isComplited)
                                {
                                    if (GUILayout.Button("↓"))
                                    {
                                        TODO.TODORow row = script.tasks[index + 1];
                                        script.tasks[index + 1] = task;
                                        script.tasks[index] = row;
                                        break;
                                    }
                                }
                            }
                        }
                        GUILayout.EndHorizontal();
                    }

                    GUI.enabled = oldEnabled;
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(index.ToString());
                        
                        var mode = EditorGUILayout.Toggle(task.isComplited);
                        if (mode != task.isComplited)
                        {
                            task.isComplited = mode;
                            script.tasks = script.tasks.OrderBy(x => x.isComplited).ToList();
                        }
                        
                        if (GUILayout.Button("X"))
                        {
                            script.tasks.Remove(task);
                            break;
                        }
                    }
                    GUILayout.EndHorizontal();
                    

                    task.priority = (TODO.TODORow.TaskPriority)EditorGUILayout.EnumPopup(task.priority);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUI.backgroundColor = oldColor;
        }
        
        if (GUILayout.Button("Add"))
        {
            script.tasks.Insert(0, new TODO.TODORow());
        }

        EditorUtility.SetDirty(script);
    }
}
