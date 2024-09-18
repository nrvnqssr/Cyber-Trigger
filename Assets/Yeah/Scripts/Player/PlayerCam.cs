using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _camHolder;
    [SerializeField] private float _xRotation;
    [SerializeField] private float _yRotation;
    [SerializeField] private float _minClamp;
    [SerializeField] private float _maxClamp;
    [SerializeField] private float _mouseX;
    [SerializeField] private float _mouseY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _mouseX = Input.GetAxisRaw("Mouse X") * _sensX * Time.unscaledDeltaTime;
        _mouseY = Input.GetAxisRaw("Mouse Y") * _sensY * Time.unscaledDeltaTime;
        _yRotation += _mouseX;
        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -_maxClamp, -_minClamp);

        _orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
        _camHolder.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
    }
}