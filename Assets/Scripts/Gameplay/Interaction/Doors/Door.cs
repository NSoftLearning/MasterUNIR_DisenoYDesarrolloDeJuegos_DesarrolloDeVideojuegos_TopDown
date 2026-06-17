using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Linq;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] Animator anim;
    public bool isOpen = false;

    [Header("Lever Conections")]
    [SerializeField] List<Lever> leversToActivate;

    [Header("Collider")]
    [SerializeField] Collider2D doorCollider;

    void Awake()
    {
        anim = GetComponent<Animator>();
        

        if (doorCollider == null)
        {
            Debug.LogWarning("No se ha asignado un Collider2D a la puerta");
        }

    }   

    void Update()
    {
        if (leversToActivate.All(lever => lever.activated) && !isOpen)
        {
            doorCollider.enabled = false;
            anim.SetTrigger("Open");
            isOpen = true;
            Debug.Log("Puerta abierta");

        }
        
        if (leversToActivate.Any(lever => !lever.activated) && isOpen)
        {
            doorCollider.enabled = true;
            anim.SetTrigger("Close");
            isOpen = false;
            Debug.Log("Puerta cerrada");
        }
    }


}
