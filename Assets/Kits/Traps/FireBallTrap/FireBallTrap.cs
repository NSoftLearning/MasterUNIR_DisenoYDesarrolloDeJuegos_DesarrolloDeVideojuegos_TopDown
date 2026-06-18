using System;
using System.Collections;
using UnityEngine;

public class FireBallTrap : MonoBehaviour, ITrap
{
    [SerializeField] GameObject _fireBall;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] int _numOfBalls = 1;
    [SerializeField] float _timeBetweenBalls = 0.5f;
    [SerializeField] Direction _direction;

    public event Action OnActivate;
    public event Action OnDeactivate;

    public void Activate()
    {
        StopCoroutine(SpawnBalls());

        SpawnBall();

        if (_numOfBalls > 1)
        {
            StartCoroutine(SpawnBalls());
        }
    }

    private void SpawnBall()
    {
        GameObject spawned = Instantiate(_fireBall, _spawnPoint.position, Quaternion.identity);
        Projectile proj = spawned.GetComponent<Projectile>();
        proj.SetDirection(_direction);

        OnActivate?.Invoke();
    }

    IEnumerator SpawnBalls()
    {
        int counter = _numOfBalls - 1;

        while (counter > 0)
        {
            yield return new WaitForSeconds(_timeBetweenBalls);
            SpawnBall();
            counter--;
        }
    }

    public void Deactivate()
    {
        StopCoroutine(SpawnBalls());

        OnDeactivate?.Invoke();
    }
}
