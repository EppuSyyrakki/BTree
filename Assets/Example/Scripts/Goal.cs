using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTree;
using TMPro;

public class Goal : MonoBehaviour, ITreeContext
{
    [SerializeField]
    private Player.Team team;

    [SerializeField]
    private LayerMask ballLayer;

    [SerializeField]
    private TMP_Text opponentScoreLabel;

    private Light goalLight;
    private BallSpawner spawner = null;
    private int scoredToThis = 0;

    public Vector3 Position
    {
        get
        {
            var p = transform.position;
            return new Vector3(p.x, 0, p.z);
        }
    }

    private void Awake()
    {
        goalLight = GetComponent<Light>();
        goalLight.enabled = false;
        spawner = FindObjectOfType<BallSpawner>();
        var players = new List<Player>(FindObjectsOfType<Player>());

        foreach (var p in players)
        {
            AssignToPlayer(p);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ball b) && !b.Scored)
        {
            scoredToThis++;
            opponentScoreLabel.text = scoredToThis.ToString();
            b.Scored = true;
            StartCoroutine(FlashLight(b));
        }
    }

    private IEnumerator FlashLight(Ball ball)
    {
        var go = ball.gameObject;
        spawner.RemoveBall(ball);

        for (int i = 0; i < 4; i++)
        {
            goalLight.enabled = true;
            yield return new WaitForSeconds(0.25f);
            goalLight.enabled = false;
            yield return new WaitForSeconds(0.25f);
        }

        Destroy(go);
    }

    private void AssignToPlayer(Player p)
    {
        if (p.Side == team)
        {
            p.OwnGoal = this;
            return;
        }

        p.OpponentGoal = this;
    }
}
