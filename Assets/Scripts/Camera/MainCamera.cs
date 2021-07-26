using Cinemachine;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private CinemachineBrain brain;


    private void Start()
    {
        CameraManager.Instance.SetMainCamera(this);

    }
}
