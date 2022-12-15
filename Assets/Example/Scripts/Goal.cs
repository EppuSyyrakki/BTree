using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTree;
using System;

public class Goal : MonoBehaviour, ITreeContext
{
    [SerializeField]
    private Player.Team team;

    [SerializeField]
    private LayerMask ballLayer;

    private Light goalLight;

    private BallSpawner spawner = null;
    private List<Player> players = null;

    private void Awake()
    {
        goalLight = GetComponent<Light>();
        goalLight.enabled = false;
        spawner = FindObjectOfType<BallSpawner>();
        players = new List<Player>(FindObjectsOfType<Player>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ball b) && !b.Scored)
        {
            b.Scored = true;
            StartCoroutine(FlashLight(b));
        }
    }

    private IEnumerator FlashLight(Ball ball)
    {
        spawner.RemoveBall(ball);

        for (int i = 0; i < 4; i++)
        {
            goalLight.enabled = true;
            yield return new WaitForSeconds(0.25f);
            goalLight.enabled = false;
            yield return new WaitForSeconds(0.25f);
        }

        Destroy(ball.gameObject);
    }
}
