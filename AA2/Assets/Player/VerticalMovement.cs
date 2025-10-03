using UnityEngine;
using UnityEngine.InputSystem;

public class VerticalMovement : MonoBehaviour
{
    public GameObject cam;
    public float minX = -20f;
    public float maxX = 45f;

    private float RotationX = 0f;
    private Transform transformX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transformX = cam != null ? cam.transform : null;
        RotationX = transformX.localEulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.value;

        RotationX += mouseDelta.y;

        //Clamp entre los valores minimos y maximos
        RotationX = Mathf.Clamp(RotationX, minX, maxX);

        //Aplicar la rotacion al transform
        Vector3 euler = transformX.localEulerAngles;
        euler.x = RotationX;
        transformX.localEulerAngles = euler;

    }
}
