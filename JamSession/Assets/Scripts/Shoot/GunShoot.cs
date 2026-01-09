using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GunShoot : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float range;
    [SerializeField] private int maxAmmo = 3;
    [SerializeField] private float coolDown = 2f;
    [SerializeField] private LayerMask hitLayers = ~0;
    
    [Header("References")]
    [SerializeField] private LineRenderer line;
    [SerializeField] private Material notTargetMaterial;
    [SerializeField] private Material hasTargetMaterial;
    
    private float _coolDownTimer = 0f;
    private Transform _target;
    private UIManager _UIManager;
    private int _ammo;

    void Start()
    {
        line.positionCount = 2;
        line.startWidth = 0.02f;
        line.endWidth = 0.02f;
        line.useWorldSpace = true;
        line.material = notTargetMaterial;
        
        _UIManager = UIManager.Instance;
        _UIManager.BulletsUI.SetAmmo(maxAmmo);
        _ammo = maxAmmo;
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
            _target = hit.transform;
        }
        else
        {
            line.material = notTargetMaterial;
            _target = null;
        }

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (_target != null && context.performed && (Time.time - _coolDownTimer) >= coolDown && maxAmmo > 0)
        {
            GameObject obj = _target.gameObject;
            Destroy(obj);
            
            if (obj.layer == LayerMask.NameToLayer("BuzzKill"))
            {
                _UIManager.CorrectTransition.Play();
                _ammo = maxAmmo;
                GameEvents.BuzzKillDestroyed?.Invoke();
            }
            else
            {
                _UIManager.WrongTransition.Play();
                _ammo--;

                if (_ammo == 0)
                {
                    GameEvents.GameOver?.Invoke();
                    return;
                }
            }
            
            _UIManager.BulletsUI.SetAmmo(_ammo);
            _coolDownTimer = Time.time;
        }
    }
}
