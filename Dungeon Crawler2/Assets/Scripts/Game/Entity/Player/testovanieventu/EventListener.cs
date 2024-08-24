using UnityEngine;

public class EventListener : MonoBehaviour
{
    // Method to be called when the event is triggered
    public void OnSpacePressed()
    {
        Debug.Log("Space key was pressed!");
    }
}
