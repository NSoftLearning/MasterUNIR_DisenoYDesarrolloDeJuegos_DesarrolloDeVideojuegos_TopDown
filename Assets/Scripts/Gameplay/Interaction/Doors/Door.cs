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
    [SerializeField] public bool isOpen = false;
    [SerializeField] DoorLevel doorLevel = DoorLevel.NotLevelDoor;

    GameManager gameManager;

    [Header("Lever Conections")]
    [SerializeField] List<Lever> leversToActivate;

    [Header("Collider")]
    [SerializeField] Collider2D doorCollider;

    public enum DoorLevel
    {
        NotLevelDoor,
        Level1,
        Level2,
        Level3
    }

    void Awake()
    {
        anim = GetComponent<Animator>();

        if (doorCollider == null)
        {
            Debug.LogWarning("No se ha asignado un Collider2D a la puerta");
        }

    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (leversToActivate == null && !isOpen && doorLevel == DoorLevel.NotLevelDoor)
        {
            doorCollider.enabled = false;
            anim.SetTrigger("Open");
            isOpen = true;
            Debug.Log("Puerta abierta");
        }

        if (leversToActivate.All(lever => lever.activated) && !isOpen && doorLevel == DoorLevel.NotLevelDoor)
        {
            doorCollider.enabled = false;
            anim.SetTrigger("Open");
            isOpen = true;
            Debug.Log("Puerta abierta");

        }
        
        if (leversToActivate.Any(lever => !lever.activated) && isOpen && doorLevel == DoorLevel.NotLevelDoor)
        {
            doorCollider.enabled = true;
            anim.SetTrigger("Close");
            isOpen = false;
            Debug.Log("Puerta cerrada");

        }

        switch (doorLevel)
        {
            case DoorLevel.Level1:
                if (gameManager.level1Completed && !isOpen)
                {
                    doorCollider.enabled = false;
                    anim.SetTrigger("Open");
                    isOpen = true;
                    Debug.Log("Puerta de nivel 1 abierta");
                }
                    break;
            case DoorLevel.Level2:
                if (gameManager.level2Completed && !isOpen)
                {
                    doorCollider.enabled = false;
                    anim.SetTrigger("Open");
                    isOpen = true;
                    Debug.Log("Puerta de nivel 2 abierta");
                }
                break;
            case DoorLevel.Level3:
                if (gameManager.level3Completed && !isOpen)
                {
                    doorCollider.enabled = false;
                    anim.SetTrigger("Open");
                    isOpen = true;
                    Debug.Log("Puerta de nivel 3 abierta");
                }
                break;
        }

    }


}
