using UnityEngine;

public class FlyingPlayerController : MonoBehaviour
{
    public float speed = 20f;
    public float rotationSpeed = 70f;

    // Crash settings
    public float propellerBreakForce = 5f;
    public float crashTumbleTorque = 3f;

    private float verticalInput;
    private bool crashed = false;

    void Update()
    {
        // Stop controlling the plane once it has crashed
        if (crashed) return;

        // Get player input
        verticalInput = Input.GetAxis("Vertical");

        // Move the plane forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Tilt the plane
        transform.Rotate(Vector3.left * rotationSpeed * verticalInput * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (crashed || !collision.gameObject.CompareTag("Obstacle")) return;

        crashed = true;
        Debug.Log("Plane Crashed!");

        speed = 0;
        rotationSpeed = 0;

        // Break the propeller off: stop it spinning, detach it and let it fly off
        SpinPropellerX propeller = GetComponentInChildren<SpinPropellerX>();
        if (propeller != null)
        {
            propeller.enabled = false;
            propeller.transform.SetParent(null);

            if (propeller.GetComponent<Collider>() == null) propeller.gameObject.AddComponent<BoxCollider>();
            Rigidbody propRb = propeller.gameObject.AddComponent<Rigidbody>();
            propRb.AddForce((Vector3.up - transform.forward) * propellerBreakForce, ForceMode.Impulse);
            propRb.AddTorque(Random.insideUnitSphere * propellerBreakForce, ForceMode.Impulse);
        }

        // Let the plane body tumble and fall
        Rigidbody planeRb = GetComponent<Rigidbody>();
        if (planeRb != null)
        {
            planeRb.useGravity = true;
            planeRb.AddTorque(Random.insideUnitSphere * crashTumbleTorque, ForceMode.Impulse);
        }
    }
}
