﻿using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class BoatMovement : MonoBehaviour
{
    public float boatSpeed = 6f;
    public float boatStrafeSpeed = 3f;
    public float boatTurnSpeed = 0.2f;

    public float drunkEffectStrength = 1f;

    [Range(0, 1)]
    public float drunkLevel;

    internal bool isPaused;

    private PlayerInput input;

    private Vector3 startingPos;
    private float leftBound;
    private float rightBound;

    void Start()
    {
        input = GetComponent<PlayerInput>();

        startingPos = transform.position;

        var river = GameObject.FindGameObjectWithTag("River").GetComponent<Renderer>();
        var boatHalfWidth = transform.localScale.x / 2;

        leftBound = river.bounds.min.x + boatHalfWidth;
        rightBound = river.bounds.max.x - boatHalfWidth;
    }

    private float currentMovement;
    private float currentDampingVelocity;

    private float drunkMovement;

    private Vector3 rotation;

    public void Reset()
    {
        rotation.Set(0, 0, 0);
        var model = transform.GetChild(0);
        model.transform.rotation = Quaternion.Euler(rotation);

        transform.position = startingPos;
        drunkMovement = 0;

        isPaused = true;
    }

    void FixedUpdate()
    {
        if (isPaused) return;

        var newPos = transform.position;
        newPos.z += boatSpeed * Time.deltaTime;

        drunkMovement += Random.Range(-0.05f, 0.05f);

        var target = input.Horizontal + drunkMovement * drunkLevel;

        currentMovement = Mathf.SmoothDamp(currentMovement, target, ref currentDampingVelocity, boatTurnSpeed + drunkLevel);
        newPos.x += currentMovement * boatStrafeSpeed * Time.deltaTime;

        newPos.x = Mathf.Clamp(newPos.x, leftBound, rightBound);

        rotation.x = -boatSpeed;
        rotation.y = currentMovement * 45;

        transform.position = newPos;

        var model = transform.GetChild(0);
        model.transform.rotation = Quaternion.Euler(rotation);
    }
}
