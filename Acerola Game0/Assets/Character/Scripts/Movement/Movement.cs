using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    public float speed = 2f;              // Movement speed
    public float rotationSpeed = 90f;    // Base rotation speed
    public float maxRotationSpeed = 540f;   // Maximum rotation speed
    public float lerpExponent = 2f;
    private Rigidbody rb;

    void Start()
    {
        // Ensure we have a Rigidbody component
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on the ship!");
            enabled = false;
        }

        // Freeze rotation in the Y axis
        rb.freezeRotation = true;
    }

    float Exerp(float a, float b, float factor, float exponent)
    {
        float expFactor = Mathf.Pow(factor, exponent);
        float result = a + (b - a) * expFactor;
        return result;
    }


    void Update()
    {
        // Get input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Move the ship
        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);

        // Rotate the ship
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            float angle = Quaternion.Angle(rb.rotation, targetRotation);
            float adjustedRotationSpeed = Exerp(maxRotationSpeed, rotationSpeed, 1 - (angle / 180f), lerpExponent); // Normalize angle to be between 0 and 1

            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, adjustedRotationSpeed * Time.deltaTime);
        }
    }
}