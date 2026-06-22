using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class SplashItem : MonoBehaviour
{
    [Header("Splash Settings")]
    [SerializeField] Transform splashPoint;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] List<GameObject> potionsPrefabs;
    [SerializeField] List<GameObject> spellsPrefabs;

    [Header("Items Settings")]
    [SerializeField] int minCoinAmount = 2;
    [SerializeField] int maxCoinAmount = 3;
    [SerializeField] float potionChance = 0.05f;
    [SerializeField] float spellChance = 0.05f;

    [Header("Force")]
    [SerializeField] float minForce = 2f;
    [SerializeField] float maxForce = 5f;

    [Header("Spread")]
    [SerializeField] float randomAngle = 360f;

    private Life life;

    void Awake()
    {
        life = GetComponentInParent<Life>();
     
    }

    void OnEnable()
    {
        if (life != null)
        {
            life.Died += SpawnSplash;
        }
    }

    void OnDisable()
    {
        if (life != null)
        {
            life.Died -= SpawnSplash;
        }
    }

    void Start()
    {
        if (splashPoint == null)
        {
            splashPoint = transform;
        }
    }

    public void SpawnSplash()
    {
        SpawnCoins();
        SpawnExtraLoot();
    }

    void SpawnCoins()
    {
        int coinAmount = Random.Range(minCoinAmount, maxCoinAmount + 1);

        for (int i = 0; i < coinAmount; i++)
        {
            SpawnItem(coinPrefab);
        }
    }

    void SpawnExtraLoot()
    {
        float roll = Random.value;

        if (roll <= potionChance)
        {
            SpawnRandomPotion();
        }
        else if (roll <= potionChance + spellChance)
        {
            SpawnRandomSpell();
        }
        // si no entra aquí, no sale nada extra, solo monedas
    }

    void SpawnRandomPotion()
    {
        if (potionsPrefabs.Count == 0) return;

        GameObject potion = potionsPrefabs[Random.Range(0, potionsPrefabs.Count)];

        SpawnItem(potion);

        Debug.Log("Spawned Extra Loot --> Potion: " + potion.name);
    }

    void SpawnRandomSpell()
    {
        if (spellsPrefabs.Count == 0) return;

        GameObject spell = spellsPrefabs[Random.Range(0, spellsPrefabs.Count)];

        SpawnItem(spell);

        Debug.Log("Spawned Extra Loot --> Spell: " + spell.name);
    }

    void SpawnItem(GameObject prefab)
    {
        GameObject item = Instantiate(prefab, splashPoint.position, Quaternion.identity);
     
        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            float angle = Random.Range(0f, randomAngle);
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;

            float force = Random.Range(minForce, maxForce);

            rb.linearVelocity = direction * force;
        }
    }


}
