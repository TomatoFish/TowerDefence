using UnityEngine;
using System;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHittable
{
    public Action<int, IHittable> OnEnemyDied;

    public NavMeshAgent Agent;

    private EnemyConfig config;
    private float health;

    public int GetHitCount => config.Damage;
    public int GetRewardCount => config.Reward;
    public bool IsDead => health <= 0;
    public GameObject GameObject => gameObject;

    public void Initialize(EnemyConfig config, int level, Vector3 position, Transform target)
    {
        this.config = config;
        health = config.Health + config.HealthLevelMultiplier * level;

        Agent.enabled = true;
        Agent.speed = config.Speed + config.SpeedLevelMultiplier * level;
        Agent.Warp(position);
        Agent.destination = target.position;
    }

    public bool ApplyHit(float ammount)
    {
        health = Mathf.Max(health - ammount, 0);
        if (health <= 0)
        {
            OnEnemyDied?.Invoke(config.Reward, this);
            return true;
        }
        return false;
    }

    private void OnDisable()
    {
        OnEnemyDied = null;
        Agent.enabled = false;
    }
}
