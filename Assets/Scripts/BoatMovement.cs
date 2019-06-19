using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class BoatMovement : MonoBehaviour
{
    public float boatSpeed = 6f;
    public float boatStrafeSpeed = 3f;
    public float boatTurnSpeed = 0.2f;

    [Range(0, 2)]
    public float drunkEffectStrength = 1f;

    [Range(0, 1)]
    public float drunkLevel;

    internal bool isPaused;

    private PlayerInput input;

    private Vector3 startingPos;
    private float leftBound;
    private float rightBound;

    void Awake()
    {
        input = GetComponent<PlayerInput>();

        startingPos = transform.position;

        var river = GameObject.FindGameObjectWithTag("River").GetComponent<Renderer>();
        var boatHalfWidth = transform.localScale.x / 2;

        leftBound = river.bounds.min.x + boatHalfWidth;
        rightBound = river.bounds.max.x - boatHalfWidth;

        Reset();
    }

    private float currentMovement;
    private float currentDampingVelocity;

    private float drunkMovement;
    private float drunkTarget;
    private float drunkDampingSpeed;
    private float currentDrunkDamping;

    private Vector3 rotation;

    public void Reset()
    {
        rotation.Set(0, 0, 0);
        var model = transform.GetChild(0);
        model.transform.rotation = Quaternion.Euler(rotation);

        transform.position = startingPos;
        currentMovement = 0;
        drunkMovement = 0;
        NewDrunkTarget();

        isPaused = true;
    }

    void NewDrunkTarget()
    {
        currentDrunkDamping = 0;
        drunkTarget = Random.Range(-2f, 2f) * drunkLevel * drunkEffectStrength;
        drunkDampingSpeed = Random.Range(0.5f, 2f);
    }

    void FixedUpdate()
    {
        if (isPaused) return;

        var newPos = transform.position;
        newPos.z += boatSpeed * Time.deltaTime;

        if (drunkLevel > 0 && Mathf.Abs(drunkTarget - drunkMovement) < 0.2f)
        {
            NewDrunkTarget();
        }

        drunkMovement = Mathf.SmoothDamp(drunkMovement, drunkTarget, ref currentDrunkDamping, drunkDampingSpeed);

        var target = input.Horizontal + drunkMovement;

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
