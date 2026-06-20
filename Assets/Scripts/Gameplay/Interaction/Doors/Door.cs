using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Linq;
using System;

public class Door : MonoBehaviour, IPathFindingBlocker
{
    [Header("Door Settings")]
    [SerializeField] Animator anim;
    [SerializeField] public bool isOpen = false;
    [SerializeField] int requiredLevel = 0;
    GameManager gameManager;

    [Header("Lever Conections")]
    [SerializeField] List<Lever> leversToActivate;

    [Header("Collider")]
    [SerializeField] Collider2D doorCollider;

    public event Action BlockerStatusChanged;
    public event Action DoorOpen;
    public event Action DoorClose;

    void Awake()
    {
        anim = GetComponent<Animator>();
        gameManager = FindAnyObjectByType<GameManager>();

        if (doorCollider == null)
        {
            Debug.LogWarning("No se ha asignado un Collider2D a la puerta");
        }

    }

    void Update()
    {
        if (leversToActivate == null && !isOpen && requiredLevel <= 0)
        {
            OpenDoor();
        }

        if (leversToActivate.All(lever => lever.activated) && !isOpen && requiredLevel <= 0)
        {
            OpenDoor(); 

        }
        
        if (leversToActivate.Any(lever => !lever.activated) && isOpen && requiredLevel <= 0)
        {
            CloseDoor();

        }

        if (requiredLevel > 0 && gameManager.IsLevelCompleted(requiredLevel) && !isOpen)
        {
            OpenDoor();
        }

    }

    void OpenDoor()
    {
        doorCollider.enabled = false;
        anim.SetTrigger("Open");
        isOpen = true;

        DoorOpen?.Invoke();
        BlockerStatusChanged?.Invoke();
    }

    void CloseDoor()
    {
        doorCollider.enabled = true;
        anim.SetTrigger("Close");
        isOpen = false;

        DoorClose?.Invoke();
        BlockerStatusChanged?.Invoke();
    }


}
