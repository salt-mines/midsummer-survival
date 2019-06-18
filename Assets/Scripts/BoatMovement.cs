using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class BoatMovement : MonoBehaviour
{
    public float boatSpeed = 6f;
    public float boatStrafeSpeed = 3f;

    private PlayerInput input;

    private float startingPos;
    private float leftBound;
    private float rightBound;

    void Start()
    {
        input = GetComponent<PlayerInput>();

        startingPos = transform.position.z;

        var river = GameObject.FindGameObjectWithTag("River").GetComponent<Renderer>();
        var boatHalfWidth = transform.localScale.x / 2;

        leftBound = river.bounds.min.x + boatHalfWidth;
        rightBound = river.bounds.max.x - boatHalfWidth;
    }

    void FixedUpdate()
    {
        var newPos = transform.position;
        newPos.z += boatSpeed * Time.deltaTime;

        newPos.x += input.Horizontal * boatStrafeSpeed * Time.deltaTime;
        newPos.x = Mathf.Clamp(newPos.x, leftBound, rightBound);

        transform.position = newPos;
    }
}
