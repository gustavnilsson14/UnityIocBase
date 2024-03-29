﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraShakeType { 
	NONE, DEFAULT, HIT
}

public class CameraShake : MonoBehaviour
{
	public float shakeAmount = 0.7f;
	public float hitAmount = 0.7f;
	public float hitRotationAmount = 3f;
	private CameraShakeType shakeType;
	private float duration;

	private Vector3 originalPos;
	private Quaternion originalRotation;
	private Vector3 hitDirection;
	private float hitCurrentDistance;
	private float hitDistance;
	
	public bool test;
	public CameraShakeType testType;

    private void Awake()
	{
		originalPos = transform.localPosition;
		originalRotation = transform.rotation;
	}

	void Update()
	{
		HandleDebug();
		if (duration == 0)
			return;
		HandleShakeType();
		
	}
	public void Shake(CameraShakeType shakeType = CameraShakeType.NONE, float duration = 0.3f, float amount = 0.1f)
	{
		if (shakeType == CameraShakeType.NONE)
			return;
		this.shakeType = shakeType;
		this.duration = duration;
		this.shakeAmount = amount;
		this.hitAmount = amount;
		hitDistance = duration/2;
		hitCurrentDistance = 0;
		transform.localPosition = originalPos;
		transform.rotation = originalRotation;
		hitDirection = Random.insideUnitSphere;
	}
	private void HandleShakeType()
    {
		switch (shakeType)
		{
			case CameraShakeType.DEFAULT:
				ShakeUpdate();
				break;
			case CameraShakeType.HIT:
				HitUpdate();
				break;
			default:
				break;
		}
		duration -= Time.deltaTime;
		if (duration > 0)
			return;
		duration = 0f;
		transform.localPosition = originalPos;
		transform.rotation = originalRotation;
	}

    private void HandleDebug()
    {
		if (!test)
			return;
		test = false;
		Shake(testType);
	}

	private void ShakeUpdate()
	{
		transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
	}

	private void HitUpdate()
	{
		hitCurrentDistance += Time.deltaTime;
		float currentDistance = Mathf.PingPong(hitCurrentDistance, hitDistance) * hitAmount;
		transform.rotation = originalRotation;
		transform.Rotate(hitDirection * currentDistance * hitRotationAmount);
		transform.localPosition = originalPos + (hitDirection * currentDistance);
	}

}