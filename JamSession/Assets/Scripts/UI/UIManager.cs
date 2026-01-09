using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [SerializeField] private BulletsUI bulletsUI;
    [SerializeField] private UIColorTransition wrongTransition;
    [SerializeField] private UIColorTransition correctTransition;
    
    public BulletsUI BulletsUI => bulletsUI;
    public UIColorTransition WrongTransition => wrongTransition;
    public UIColorTransition CorrectTransition => correctTransition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
