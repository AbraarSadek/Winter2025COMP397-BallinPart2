using UnityEngine;

public class MobileController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        #if UNITY_ANDROID
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        #endif
    }
}
