using UnityEngine;

public class PickUp : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Distancia máxima para recoger un objeto")]
    public float pickupRange = 2f;

    [Tooltip("Distancia a la que se mantiene el objeto al recogerlo")]
    public float holdDistance = 2f;

    [Tooltip("Máscara de capa que define qué objetos se pueden recoger")]
    public LayerMask pickupMask;

    [Tooltip("Velocidad de interpolación al mantener el objeto")]
    public float holdSmoothness = 10f;

    private Camera mainCamera;
    private Rigidbody heldObject;
    private float originalDistance;

    private InputSystem_Actions inputs;
    private bool isHoldingInput;

    private void Awake()
    {
        mainCamera = Camera.main;
        inputs = new InputSystem_Actions();
        inputs.Enable();
    }

    private void Update()
    {
        // Leer el estado del clic derecho del nuevo Input System
        isHoldingInput = inputs.Player.Interact.ReadValue<float>() > 0f; // Asegúrate que "Interact" esté vinculado al botón derecho

        if (heldObject == null)
        {
            // Si no tenemos objeto en la mano, intentamos coger uno
            TryPickup();
        }
        else
        {
            // Si tenemos uno cogido, lo mantenemos o lo soltamos
            if (isHoldingInput)
                HoldObject();
            else
                DropObject();
        }
    }

    private void TryPickup()
    {
        if (!isHoldingInput) return;

        RaycastHit hit;
        Vector3 origin = mainCamera.transform.position;
        Vector3 direction = mainCamera.transform.forward;

        if (Physics.Raycast(origin, direction, out hit, pickupRange, pickupMask))
        {
            Rigidbody rb = hit.rigidbody;
            if (rb != null)
            {
                heldObject = rb;
                originalDistance = hit.distance;

                // Desactivar la física del objeto
                rb.useGravity = false;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    private void HoldObject()
    {
        if (heldObject == null) return;

        Vector3 targetPosition = mainCamera.transform.position + mainCamera.transform.forward * originalDistance;
        heldObject.MovePosition(Vector3.Lerp(heldObject.position, targetPosition, Time.deltaTime * holdSmoothness));
    }

    private void DropObject()
    {
        if (heldObject == null) return;

        heldObject.useGravity = true;
        heldObject = null;
    }

    private void OnDrawGizmos()
    {
        if (mainCamera == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(mainCamera.transform.position,
                        mainCamera.transform.position + mainCamera.transform.forward * pickupRange);
    }
}

