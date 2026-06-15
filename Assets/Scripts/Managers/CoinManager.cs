using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textCoin;
    [SerializeField] public int currentCoins = 0;

    void Start()
    {
        currentCoins = 0;
    }

    void Update()
    {
        
        textCoin.text = currentCoins.ToString();

    }

    public void AddCoin()
    {
        currentCoins++;
    }
}
