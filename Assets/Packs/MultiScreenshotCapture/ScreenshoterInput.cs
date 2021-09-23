using UnityEngine;
using UnityEngine.Events;

public class ScreenshoterInput : MonoBehaviour
{
    public UnityEvent keyDownEvent;

    public static ScreenshoterInput Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        DontDestroyOnLoad(gameObject);
        
        keyDownEvent = new UnityEvent();
        
    }

    private void OnDestroy()
    {
        keyDownEvent.RemoveAllListeners();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            keyDownEvent?.Invoke();
        }
    }
}