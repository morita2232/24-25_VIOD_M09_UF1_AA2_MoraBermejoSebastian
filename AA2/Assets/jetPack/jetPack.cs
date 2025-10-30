using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class jetPack : MonoBehaviour
{
    InputSystem_Actions inputs;
    public float fuel = 1f;
    public Image visualFuel;
    public Rigidbody rb;
    public GroundDetection gd;

    public float fuelUseRate = 0.1f;
    public float refuelRate = 0.05f;
    public float jumpForce = 5f;

    void Start()
    {
        inputs = new InputSystem_Actions();
        inputs.Enable();
    }

    void Update()
    {
        // Si se mantiene pulsado el salto y hay combustible
        if (inputs.Player.Jump.IsPressed() && fuel > 0)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
            fuel -= fuelUseRate * Time.deltaTime;
            fuel = Mathf.Clamp01(fuel);
            visualFuel.fillAmount = fuel;
        }

        // Si está en el suelo, comienza a recargar
        if (gd.grounded && fuel < 1f)
        {
            // Solo iniciar una corrutina si no hay otra recargando
            if (!isRefueling)
                StartCoroutine(Refuel());
        }
    }

    bool isRefueling = false;

    IEnumerator Refuel()
    {
        isRefueling = true;

        while (fuel < 1f && gd.grounded)
        {
            fuel += refuelRate * Time.deltaTime;
            fuel = Mathf.Clamp01(fuel);
            visualFuel.fillAmount = fuel;
            yield return null; // Esperar al siguiente frame
        }

        isRefueling = false;
    }
}

