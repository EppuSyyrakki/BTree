using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private Ball ballPrefab = null;

    private List<Player> players;
    private event Action OnRemoveBall;
    private event Action<Ball> OnSpawnBall;

    private void Awake()
    {
        players = new List<Player>(FindObjectsOfType<Player>());

        foreach (var player in players)
        {
            OnRemoveBall += () => player.Ball = null;
            OnSpawnBall += (ball) => player.Ball = ball;
        }

        SpawnBall();
    }

    public void RemoveBall(Ball ball)
    {
        Invoke(nameof(SpawnBall), 0);
        OnRemoveBall?.Invoke();
    }

    private void SpawnBall()
    {
        Vector2 rnd = Random.insideUnitCircle;
        Vector3 offset = new Vector3(rnd.x, 0, rnd.y) * 2f;
        var newBall = Instantiate(ballPrefab, transform.position + Vector3.up * 2f + offset, Quaternion.identity);
        var rb = newBall.GetComponent<Rigidbody>();
        rb.AddForce(Random.insideUnitSphere + Vector3.up, ForceMode.Impulse);
        OnSpawnBall?.Invoke(newBall);
    }
}
