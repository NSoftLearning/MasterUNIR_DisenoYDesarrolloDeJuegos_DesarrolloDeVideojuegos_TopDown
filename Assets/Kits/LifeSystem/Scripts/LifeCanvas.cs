using System;
using UnityEngine;
using UnityEngine.UI;

public class LifeCanvas : MonoBehaviour
{
    [SerializeField] Life life;
    [SerializeField] Image mask;

    private void OnEnable()
    {
        life.onLifeChanged.AddListener(OnLifechanged);
        life.onLifeDepleted.AddListener(OnLifeDepleted);
    }

    private void OnLifeDepleted(float arg0)
    {
        
    }

    private void OnLifechanged(float currentLife, float startingLife)
    {
        mask.fillAmount = currentLife / startingLife;
    }
}
