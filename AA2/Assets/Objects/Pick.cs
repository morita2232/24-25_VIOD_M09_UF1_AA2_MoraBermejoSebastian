using UnityEngine;

public class Pick : MonoBehaviour
{
    private Rigidbody rb;

    public bool IsHeld { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnPickUp()
    {
        IsHeld = true;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void OnDrop()
    {
        IsHeld = false;
        rb.useGravity = true;
    }
}
