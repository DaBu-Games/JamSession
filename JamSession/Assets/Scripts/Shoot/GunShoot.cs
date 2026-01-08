using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShoot : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float range;
    [SerializeField] private int bullets = 3;
    [SerializeField] private float coolDown = 2f;
    [SerializeField] private LayerMask hitLayers = ~0;
    
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Material notTargetMaterial;
    [SerializeField] private Material hasTargetMaterial;
    
    private float coolDownTimer = 0f;
    private Transform target;

    void Start()
    {
        line.positionCount = 2;
        line.startWidth = 0.02f;
        line.endWidth = 0.02f;
        line.useWorldSpace = true;
        line.material = notTargetMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 start = transform.position;
        Vector3 direction = transform.forward;
        Vector3 end = start + direction * range;

        if (Physics.Raycast(start, direction, out RaycastHit hit, range, hitLayers))
        {
            end = hit.point;
            line.material = hasTargetMaterial;
            target = hit.transform;
        }
        else
        {
            line.material = notTargetMaterial;
            target = null;
        }

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (target != null && context.performed && (Time.time - coolDownTimer) >= coolDown && bullets > 0)
        {
            Destroy(target.gameObject);
            bullets--;
            coolDownTimer = Time.time;
        }
    }
}
