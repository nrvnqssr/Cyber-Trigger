using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchWeapon : MonoBehaviour
{
    public static Action<Gun> OnWeaponChanged;

    private WeaponControll weaponControll = null;
    private int weaponIndex = 0;

    [SerializeField] private GameObject[] _weapons = new GameObject[5];

    void Awake()
    {
        weaponControll = new WeaponControll();
    }

    private void Start()
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            DisableWeapon(i);
        }
        EnableWeapon(weaponIndex);
    }

    private void OnEnable()
    {
        weaponControll.OnWeaponControll.Enable();
        weaponControll.OnWeaponControll.SwitchWeapon.performed += Switch;
    }

    private void OnDisable()
    {
        weaponControll.Disable();
        weaponControll.OnWeaponControll.SwitchWeapon.performed -= Switch;
    }

    private void Switch(InputAction.CallbackContext callback)
    {
        DisableWeapon(weaponIndex);
        if (weaponIndex < _weapons.Length - 1)
        {
            if (_weapons[weaponIndex + 1] != null)
                weaponIndex++;
            else 
                weaponIndex = 0;
        }
        else
        {
            weaponIndex = 0;
        }
        EnableWeapon(weaponIndex);

        OnWeaponChanged.Invoke(_weapons[weaponIndex]?.GetComponent<Gun>());
    }

    private void EnableWeapon(int index) 
    {
        if (_weapons[index] != null)
            _weapons[index].SetActive(true);
    }

    private void DisableWeapon(int index)
    {
        _weapons[index].SetActive(false);
    }
}