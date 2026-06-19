using Unity.VisualScripting;
using UnityEngine;

public class CharacterRollInmune : MonoBehaviour
{
    CustomCharacterController characterController;
    Life life;
    private void Awake()
    {
        characterController = GetComponent<CustomCharacterController>();
        life = GetComponent<Life>();
    }

    private void OnEnable()
    {
        characterController.OnRoll += OnRoll;
        characterController.OnFinishRoll += OnFinishRoll;
    }

    private void OnRoll()
    {
        life.SetInmunity(true);
    }

    private void OnFinishRoll()
    {
        life.SetInmunity(false);
    }


    private void OnDisable()
    {
        characterController.OnRoll -= OnRoll;
        characterController.OnFinishRoll -= OnFinishRoll;
    }

}
