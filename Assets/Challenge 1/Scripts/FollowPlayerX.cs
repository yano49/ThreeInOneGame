using UnityEngine;

public class FollowPlayerX : MonoBehaviour
{
    public GameObject plane;

    private Vector3 offset = new Vector3(30, 0, -10);

    void LateUpdate()
    {
        transform.position = plane.transform.position + offset;

        // Keep the camera looking at the plane
        transform.LookAt(plane.transform);
    }
}