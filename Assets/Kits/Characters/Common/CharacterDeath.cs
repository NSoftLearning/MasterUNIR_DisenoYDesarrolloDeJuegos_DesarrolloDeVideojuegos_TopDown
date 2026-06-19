using UnityEngine;

public class CharacterDeath : MonoBehaviour
{

    Life life;
    CustomCharacterController characterController;
    private void Awake()
    {
        life = GetComponent<Life>();
        characterController = GetComponent<CustomCharacterController>();
    }

    private void OnEnable()
    {
        life.Died += OnDie;
    }

    private void OnDie()
    {
        characterController.Die();
    }

    private void OnDisable()
    {
        life.Died -= OnDie;
    }
}
