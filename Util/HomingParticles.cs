using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingParticles : MonoBehaviour
{
    public Transform target;
    private ParticleSystem system;

    private void Awake()
    {
        system = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (!HandleTarget(out target))
            return;
        transform.parent.LookAt(target);
        float distance = Vector3.Distance(transform.parent.position, target.position);
        var shape = system.shape;
        shape.radius = distance;
        transform.localPosition = new Vector3(0,0, distance);
    }

    private bool HandleTarget(out Transform target)
    {
        target = this.target;
        if (target != null)
            return true;
        if (EnemyLogic.I.enemies.Count == 0)
            return false;

        target = MovementLogic.I.GetTrackMoverCart(EnemyLogic.I.enemies[0] as ITrackMover).transform;
        return target != null;
    }
}
