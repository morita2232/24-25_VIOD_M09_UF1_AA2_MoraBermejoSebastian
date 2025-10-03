using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{

    public GameObject baseTorreta; //Base de la torreta
    public GameObject torsoTorreta; //Torso de la torreta

    //Control de rotacion de la base
    public bool baseT;
    public bool torsoT;

    //Distancia de rotacion minima y maxima de la base y el torso
    public float minBaseY = -60f;
    public float maxBaseY = 60f;

    public float minTorsoX = -20f;
    public float maxTorsoX = 45f;

    //Variables internas para seguimiento de rotacion
    private float baseRotationY = 0f;
    private float torsoRotationX = 0f;

    //Referencias a los transforms
    private Transform baseTransform;
    private Transform torsoTransform;

    void Start()
    {
        //Obtener referencias a los transforms
        baseTransform = baseTorreta != null ? baseTorreta.transform : null;
        torsoTransform = torsoTorreta != null ? torsoTorreta.transform : null;

        if (baseTransform != null)
            baseRotationY = baseTransform.localEulerAngles.y;

        if (torsoTransform != null)
            torsoRotationX = torsoTransform.localEulerAngles.x;
    }

    void Update()
    {
        //Obtener delta del mouse
        Vector2 mouseDelta = Mouse.current.delta.value;

        //Actualizar rotacion de la base y el torso segun el input del mouse
        if (baseT && baseTransform != null)
        {
            //Actualizar y limitar la rotacion Y de la base
            baseRotationY += mouseDelta.x;

            //Clamp entre los valores minimos y maximos
            baseRotationY = Mathf.Clamp(baseRotationY, minBaseY, maxBaseY);

            //Aplicar la rotacion al transform
            Vector3 euler = baseTransform.localEulerAngles;
            euler.y = baseRotationY;
            baseTransform.localEulerAngles = euler;
        }

        //Actualizar rotacion del torso
        if (torsoT && torsoTransform != null)
        {
            //Actualizar y limitar la rotacion X del torso
            torsoRotationX += mouseDelta.y;

            //Clamp entre los valores minimos y maximos
            torsoRotationX = Mathf.Clamp(torsoRotationX, minTorsoX, maxTorsoX);

            //Aplicar la rotacion al transform
            Vector3 euler = torsoTransform.localEulerAngles;
            euler.x = torsoRotationX;
            torsoTransform.localEulerAngles = euler;
        }
    }
}
