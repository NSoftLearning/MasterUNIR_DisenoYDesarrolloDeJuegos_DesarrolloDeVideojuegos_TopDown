using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class OnTrigger : MonoBehaviour
{
    [Header("Event")]
    public UnityEvent onTriggerEvent;

    [Header("Trigger Settings")]
    [SerializeField] bool triggerOnce = true;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onTriggerEvent.Invoke();

            if (triggerOnce)
            {
                GetComponent<Collider2D>().enabled = false;
            }
        }

        
    }
}
