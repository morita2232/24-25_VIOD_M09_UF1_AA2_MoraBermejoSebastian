using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    InputSystem_Actions inputs;
    public float speed;
    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputs = new InputSystem_Actions();
        inputs.Enable();
        cam = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 dir = inputs.Player.Move.ReadValue<Vector2>();


        if(dir.y < 0)
        {

            transform.position +=
            Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized
            * dir.y * (speed * 0.5f) * Time.fixedDeltaTime;
        }
        else
        {
            if(inputs.Player.Sprint.IsPressed())
            {
                transform.position +=
            Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized
            * dir.y * (speed * 2f) * Time.fixedDeltaTime;
            }
            else
            {
                transform.position +=
                    Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized
                    * dir.y * speed * Time.fixedDeltaTime;

            }

        }

        transform.position +=
            Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized
            * dir.x * (speed * 0.75f) * Time.fixedDeltaTime;
    }
}
