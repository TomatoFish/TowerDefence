using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Weapon : MonoBehaviour
{
    public LineRenderer Line;

    private TowerConfig config;
    private float damage;
    private float fireRate;
    private List<IHittable> targets;
    private Coroutine attackCoroutine;

    public void Initialize(TowerConfig config)
    {
        this.config = config;
        targets = new List<IHittable>();
        StartCoroutine(AttackCoroutine());
    }

    public void StatsUpdate(int level)
    {
        damage = config.Damage + config.DamageLevelMultiplier * level;
        fireRate = Mathf.Max(config.FireRate - config.FireRateMultiplier * level, config.FireRateMin);
    }

    private void RemoveTarget(IHittable hittable)
    {
        targets.Remove(hittable);
    }

    private void OnTriggerEnter(Collider other)
    {
        var hittable = other.gameObject.GetComponent<IHittable>();
        if (hittable != null)
        {
            if (!targets.Contains(hittable))
                targets.Add(hittable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var hittable = other.gameObject.GetComponent<IHittable>();
        if (hittable != null && targets.Contains(hittable))
            RemoveTarget(hittable);
    }

    private IEnumerator AttackCoroutine()
    {
        while (true)
        {
            targets.RemoveAll(t => t == null || t.IsDead);
            if (attackCoroutine != null) StopCoroutine(attackCoroutine);

            if (targets.Count > 0)
            {
                var target = targets.FirstOrDefault();
                if (target != null)
                {
                    attackCoroutine = StartCoroutine(AttackAnimation(target.GameObject.transform));
                    if (target.ApplyHit(damage))
                        RemoveTarget(target);
                    yield return new WaitForSeconds(fireRate);
                }
            }
            yield return null;
        }
    }

    private IEnumerator AttackAnimation(Transform target)
    {
        Line.SetPosition(1, target.position - transform.position);
        Line.gameObject.SetActive(true);
        yield return new WaitForSeconds(config.FireRateMin);
        Line.gameObject.SetActive(false);
    }
}
