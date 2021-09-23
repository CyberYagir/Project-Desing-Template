using MultiScreenshotCaptureNamespace;
using UnityEditor;

// ensure class initializer is called whenever scripts recompile
[InitializeOnLoadAttribute]
public static class PlayModeStateListener
{
    // register an event handler when the class is initialized
    static PlayModeStateListener()
    {
        EditorApplication.playModeStateChanged += LogPlayModeState;
    }

    private static void LogPlayModeState(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode && EditorWindow.HasOpenInstances<MultiScreenshotCapture>())
        {
            MultiScreenshotCapture window = EditorWindow.GetWindow<MultiScreenshotCapture>();
            window.CreateScreenshoterInput();
        }
    }
}