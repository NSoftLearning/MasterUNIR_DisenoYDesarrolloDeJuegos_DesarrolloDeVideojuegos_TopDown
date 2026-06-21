using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsCanvas : MonoBehaviour
{
    [SerializeField] Life life;
    [SerializeField] CustomCharacterController stamina;
    [SerializeField] Slider healthBar;
    [SerializeField] Slider staminaBar;

    private void OnEnable()
    {
        life.onLifeChanged.AddListener(OnLifechanged);
        life.onLifeDepleted.AddListener(OnLifeDepleted);

        stamina.onStaminaChanged.AddListener(OnStaminaChange);
    }

    private void Awake()
    {
        if (life == null || stamina == null)
        {
            Debug.LogWarning("No se asignaron las referencias en PlayerStatesCanvas");
            
        }
        
      
    }

    private void OnLifeDepleted(float arg0)
    {
        
    }

    private void OnLifechanged(float currentLife, float startingLife)
    {
        healthBar.maxValue = startingLife;
        healthBar.wholeNumbers = true; //baja por segmentos enteros
        healthBar.value = currentLife;
    }

    private void OnStaminaChange(float currentStamina, float startingStamina)
    {
        staminaBar.maxValue = startingStamina;
        staminaBar.wholeNumbers = false; //NO baja por segmentos enteros
        staminaBar.value = currentStamina;
    }

    private void OnDisable()
    {
        life.onLifeChanged.RemoveListener(OnLifechanged);
        life.onLifeDepleted.RemoveListener(OnLifeDepleted);

        stamina.onStaminaChanged.RemoveListener(OnStaminaChange);
    }
}
