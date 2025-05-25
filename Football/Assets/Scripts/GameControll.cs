using System;
using UnityEngine;

public class GameControll : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Ball ball;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Setup()
    {
        ball.Reset();
        player.Reset();
    }
    public void GoalShoot(string goalId)
    {
        Debug.Log(goalId);
        Setup();
    }
}
