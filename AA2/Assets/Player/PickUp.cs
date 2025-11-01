using UnityEngine;

public class PickUp : MonoBehaviour
{    
    public float pickupRange = 10f;

    
    public float holdDistance = 5f;

    
    public LayerMask pickupMask;

    
    public float holdSmoothness = 10f;

    private Camera mainCamera;
    private Rigidbody heldObject;
    private float originalDistance;

    private InputSystem_Actions inputs;
    private bool isHoldingInput;
    private Vector2 isScrollingInput;
    Vector3 targetPosition;
    RaycastHit hit;
    private Vector3 lastHeldPosition;

    private Vector3 lastHeldVelocity;




    private void Awake()
    {
        mainCamera = Camera.main;
        inputs = new InputSystem_Actions();
        inputs.Enable();
        

    }

    private void Update()
    {
        isHoldingInput = inputs.Player.Interact.ReadValue<float>() > 0f;

        if (heldObject == null)
        {
            TryPickup();
        }
        else
        {
            if (isHoldingInput) {
                targetPosition = mainCamera.transform.position + mainCamera.transform.forward * originalDistance;
                lastHeldPosition = heldObject.position;

                HoldObject();

                Vector3 currentPos = heldObject.position;
                lastHeldVelocity = (currentPos - lastHeldPosition) / Time.deltaTime;
                lastHeldPosition = currentPos;
            }
            else
                DropObject();
        }
    }

    private void TryPickup()
    {
        if (!isHoldingInput) return;

        
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

        Debug.Log("COGIDO");
        isScrollingInput = inputs.UI.ScrollWheel.ReadValue<Vector2>();

        if (heldObject == null) return;

            Debug.Log(isScrollingInput);
        
        heldObject.MovePosition(Vector3.Lerp(heldObject.position, targetPosition, Time.deltaTime * holdSmoothness));

        if (isScrollingInput.y > 0)
        {
            originalDistance += 1;
            heldObject.MovePosition(Vector3.Lerp(heldObject.position, targetPosition, Time.deltaTime * holdSmoothness));
            Debug.Log("scroll +");
        }
        else if (isScrollingInput.y < 0)
        {
            Debug.Log("scroll -");
            
            originalDistance -= 1;
            if (originalDistance < hit.distance)
            {
                originalDistance = hit.distance;
            }
            heldObject.MovePosition(Vector3.Lerp(heldObject.position, targetPosition, Time.deltaTime * holdSmoothness));
        }
    }

    private void DropObject()
    {
        if (heldObject == null) return;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();

        heldObject.useGravity = true;
        rb.linearVelocity = lastHeldVelocity;

        heldObject = null;
    }


    private void OnDrawGizmos()
    {
        if (mainCamera == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(mainCamera.transform.position,
                        mainCamera.transform.position + mainCamera.transform.forward * pickupRange);
        Gizmos.DrawWireSphere(targetPosition, 1);
    }
}

