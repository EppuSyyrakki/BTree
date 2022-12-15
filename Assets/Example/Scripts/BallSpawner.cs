using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private Ball ballPrefab = null;

    private List<Player> players;
    private event Action<Ball> OnRemoveBall;
    private event Action<Ball> OnSpawnBall;

    private void Awake()
    {
        players = new List<Player>(FindObjectsOfType<Player>());

        foreach (var player in players)
        {
            OnRemoveBall += (ball) =>
            {
                player.Ball = null;
                player.RemoveContext(ball);
            };
            OnSpawnBall += (ball) => player.Ball = ball;
        }

        SpawnBall();
    }

    public void RemoveBall(Ball ball)
    {
        Invoke(nameof(SpawnBall), 0);
        OnRemoveBall?.Invoke(ball);
    }

    private void SpawnBall()
    {
        var newBall = Instantiate(ballPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
        var rb = newBall.GetComponent<Rigidbody>();       
        rb.AddForce(Random.insideUnitSphere + Vector3.up, ForceMode.Impulse);
        OnSpawnBall?.Invoke(newBall);
    }
}
