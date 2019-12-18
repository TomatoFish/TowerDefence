using UnityEngine;
using System;

public interface IHittable
{
    int GetHitCount { get; }
    int GetRewardCount { get; }
    bool IsDead { get; }
    GameObject GameObject { get; }

    bool ApplyHit(float ammount);
}
