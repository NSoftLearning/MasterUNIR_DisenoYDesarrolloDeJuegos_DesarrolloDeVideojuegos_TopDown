using UnityEngine;

public abstract class ItemEffectSO : ScriptableObject
{
    [Header("Effect VFX")]
    [SerializeField] private GameObject useVfxPrefab;
    [SerializeField] private bool attachVfxToUser = true;
    [SerializeField] private Vector3 vfxOffset = Vector3.zero;
    [SerializeField] private float destroyVfxAfterSeconds = 2f;

    public bool Use(GameObject user)
    {
        bool usedSuccessfully = ApplyEffect(user);

        if (usedSuccessfully)
        {
            PlayUseVfx(user);
        }

        return usedSuccessfully;
    }

    public abstract bool ApplyEffect(GameObject user);

    private void PlayUseVfx(GameObject user)
    {
        if (useVfxPrefab == null || user == null)
            return;

        GameObject vfxInstance;

        if (attachVfxToUser)
        {
            vfxInstance = Instantiate(useVfxPrefab, user.transform);
            vfxInstance.transform.localPosition = vfxOffset;
        }
        else
        {
            Vector3 spawnPosition = user.transform.position + vfxOffset;
            vfxInstance = Instantiate(useVfxPrefab, spawnPosition, Quaternion.identity);
        }

        if (destroyVfxAfterSeconds > 0f)
        {
            Destroy(vfxInstance, destroyVfxAfterSeconds);
        }
    }
}