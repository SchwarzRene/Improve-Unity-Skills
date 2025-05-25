using System;
using UnityEngine;

public class GameControll : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Ball ball;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void Update()
    {
        if (player.stepCount > 1000)
        {
            Debug.Log("Epoch End");
            player.EndEpisode();
            Setup();
        }
    }
    private void Setup()
    {
        ball.Reset();
        player.Reset();
        player.OnEpisodeBegin();
    }
    public void GoalShoot(string goalId)
    {
        if (goalId == "RedGoal")
        {
            player.SetReward( 1.0f - player.StepCount / 1000 );
        }
        else
        {
            player.SetReward(-1.0f);
        }
        
        player.EndEpisode();
        Debug.Log("Goal Shot");
        Setup();
    }
}
