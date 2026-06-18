using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Level Progression")]
    public bool level1Completed = false;
    public bool level2Completed = false;
    public bool level3Completed = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CompleteLevel(int levelNumber)
    {
        switch (levelNumber)
        {
            case 1:
                level1Completed = true;
                Debug.Log("Nivel 1 completado, valor guardado");
                break;
            case 2:
                level2Completed = true;
                Debug.Log("Nivel 2 completado, valor guardado");
                break;
            case 3:
                level3Completed = true;
                Debug.Log("Nivel 3 completado, valor guardado");
                break;
            default:
                Debug.LogWarning("Número de nivel no válido");
                break;
        }
    }

}
