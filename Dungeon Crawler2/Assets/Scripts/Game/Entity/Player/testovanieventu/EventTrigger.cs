using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    // Define a UnityEvent field
    public UnityEvent spacePressedEvent;

    void Update()
    {
        // Check if the Space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Invoke the event
            spacePressedEvent.Invoke();
        }
    }
}
