using Unity.Netcode;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    [SerializeField] private Camera mainCamera;
    void Awake()
    {
        if(IsOwner)
        {
            mainCamera.gameObject.SetActive(true);
        } else {
            mainCamera.gameObject.SetActive(false);
        }
    }
     
    void Update()
    {
        if(!IsOwner)
        {
            mainCamera.gameObject.SetActive(false);
        } else
        {
            mainCamera.gameObject.SetActive(true);
        }
    }
}
