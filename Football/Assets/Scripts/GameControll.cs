using System;
using UnityEngine;

public class GameControll : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Ball ball;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Setup();   
    }

    private void Setup()
    {
        ball.Reset();
    }
    public void GoalShoot(string goalId)
    {
        if (goalId == "RedGoal")
        {
            player.GoalShot(1.0f);
        }
        else
        {
            player.GoalShot(-1.0f);
        }
        Setup();
        player.EndEpisode();
    }
}
