using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [HideInInspector]
    public Camera mainCam;
    [HideInInspector]
    public Transform mainCamTransform;
    public float moveSpeed = 5;
    public float bulletSpeed = 200;
    public float RateOfFire = 0.05f;
    private void Awake()
    {
        mainCam = Camera.main;
        mainCamTransform = mainCam.transform;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
