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
    bool isRefueling = false;

    void Start()
    {
        inputs = new InputSystem_Actions();
        inputs.Enable();
    }

    void Update()
    {
        
        if (inputs.Player.Jump.IsPressed() && fuel > 0)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
            fuel -= fuelUseRate * Time.deltaTime;
            fuel = Mathf.Clamp01(fuel);
            visualFuel.fillAmount = fuel;
        }

        
        if (gd.grounded && fuel < 1f)
        {
            
            if (!isRefueling)
                StartCoroutine(Refuel());
        }
    }


    IEnumerator Refuel()
    {
        isRefueling = true;

        while (fuel < 1f && gd.grounded)
        {
            fuel += refuelRate * Time.deltaTime;
            fuel = Mathf.Clamp01(fuel);
            visualFuel.fillAmount = fuel;
            yield return null;
        }

        isRefueling = false;
    }
}

