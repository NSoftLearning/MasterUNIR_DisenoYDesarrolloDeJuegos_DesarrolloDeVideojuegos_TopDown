using UnityEngine;

public class CinematicScene : MonoBehaviour
{
    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void Start()
    {
        gameManager.ResetProgress(); //Los nivelesa completados se reinician, empiezas de 0
    }
}
