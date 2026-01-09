using UnityEngine;

public class BulletsUI : MonoBehaviour
{
    [SerializeField] private Transform bulletsParent;

    private int _ammo = 0;

    public void SetAmmo(int newAmmo)
    {
        _ammo = Mathf.Clamp(newAmmo, 0, bulletsParent.childCount);

        for (int i = 0; i < bulletsParent.childCount; i++)
        {
            bulletsParent.GetChild(i).gameObject.SetActive(i < _ammo);
        }
    }
}