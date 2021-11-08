using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;



public class CameraManager : ManagerBase<CameraManager>
{
    private const int CAMERA_PRIORITY_DEFAULT = 10;
    private const int CAMERA_PRIORITY_PLAYER = 100;
    private const int CAMERA_PRIORITY_SCENE = 500;
    private const int CAMERA_PRIORITY_CINEMATIC = 1000;



    /// <summary>
    /// 메인 카메라
    /// </summary>
    private Camera mainCam;

    private CinemachineBrain brain;


    /// <summary>
    /// 현재 활성화되어있는 카메라 컨트롤러
    /// </summary>
    private CameraController curCameraCtrl = null;

    /// <summary>
    /// 바로 이전에 활성화됐었던 카메라 컨트롤러
    /// </summary>
    private CameraController preCameraCtrl = null;

    /// <summary>
    /// 자유시점 카메라 컨트롤러
    /// </summary>
    private FreeLookCamController freeLookCam = null;

    /// <summary>
    /// 숄더뷰시점 카메라 컨트롤러
    /// </summary>
    private ShoulderCamController shoulderCam = null;

    /// <summary>
    /// 카메라 컨트롤러 리스트
    /// </summary>
    private Dictionary<int, CameraController> customCameraDic = new Dictionary<int, CameraController>();


    /// <summary>
    /// 현재 활성화되어있는 씬카메라
    /// </summary>
    private ICinemachineCamera curSceneCamera;




    [SerializeField]
    private ForwardRendererData rendererData;



    private RadialBlurFilter radialBlurFilter = new RadialBlurFilter();



    [Tooltip("마우스 잠금")]
    [SerializeField]
    private bool lockCursor;



    public Camera MainCam { get => mainCam; }

    public CameraController CurrentCameraCtrl { get => curCameraCtrl; }

    public bool IsBlending { get => brain.IsBlending; }

    public ForwardRendererData RendererData { get => rendererData; }

    public RadialBlurFilter RadialBlurFilter { get => radialBlurFilter; }



    protected override void Awake()
    {
        base.Awake();

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void LateUpdate()
    {
        if (curCameraCtrl != null)
        {
            curCameraCtrl.CameraUpdate();
        }
        if (preCameraCtrl != null && brain.IsBlending)
        {
            preCameraCtrl.CameraUpdate();
        }

    }

    /// <summary>
    /// 초기화. 필요한 카메라들 있는지 체크
    /// </summary>
    public void Initialize()
    {
        mainCam = Camera.main;
        brain = mainCam.GetComponent<CinemachineBrain>();

        if (freeLookCam == null)
        {
            freeLookCam = ResourceManager.Instance.CreatePrefab<FreeLookCamController>(
                "FreeLookCam", PrefabPath.Camera, null);

            freeLookCam.Target = PlayerManager.Instance.PlayerCtrl.transform;
        }

        if (shoulderCam == null)
        {
            shoulderCam = ResourceManager.Instance.CreatePrefab<ShoulderCamController>(
                "ShoulderCam", PrefabPath.Camera, null);

            shoulderCam.Target = PlayerManager.Instance.PlayerCtrl.followTarget;
        }

        ActivateCustomCamera(0);

        lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// 커스텀 카메라 리스트에 카메라를 추가합니다
    /// </summary>
    /// <param name="idx">인덱스</param>
    /// <param name="camera">추가할 카메라</param>
    public void AddCustomCamera(int idx, CameraController camera)
    {
        if (camera == null)
            return;

        if (customCameraDic.ContainsKey(idx))
        {
            customCameraDic[idx] = camera;
        }
        else
        {
            customCameraDic.Add(idx, camera);
        }
    }

    /// <summary>
    /// 커스텀 카메라를 활성화합니다
    /// </summary>
    /// <param name="idx">인덱스</param>
    public void ActivateCustomCamera(int idx)
    {
        if (customCameraDic.ContainsKey(idx))
        {
            if (curCameraCtrl != null)
            {
                preCameraCtrl = curCameraCtrl;
                preCameraCtrl.Priority = CAMERA_PRIORITY_DEFAULT;
            }

            curCameraCtrl = customCameraDic[idx];
            curCameraCtrl.Priority = CAMERA_PRIORITY_PLAYER;
            curCameraCtrl.ApplyPreMovement(mainCam.transform.rotation);
            PlayerManager.Instance.PlayerCtrl.onRotationAxisInput = curCameraCtrl.Rotate;
        }
    }

    /// <summary>
    /// 씬카메라를 활성화합니다
    /// </summary>
    /// <param name="camera">활성화할 카메라</param>
    public void ActivateSceneCamera(ICinemachineCamera camera)
    {
        if (curSceneCamera != null)
            curSceneCamera.Priority = CAMERA_PRIORITY_DEFAULT;

        camera.Priority = CAMERA_PRIORITY_SCENE;
        curSceneCamera = camera;
    }

    /// <summary>
    /// 씬카메라를 비활성화합니다
    /// </summary>
    public void DeactivateSceneCamera()
    {
        if (curSceneCamera == null)
            return;

        curSceneCamera.Priority = CAMERA_PRIORITY_DEFAULT;
        curSceneCamera = null;
    }



    // 임시
    private void TempInputCamera()
    {
        //if (Input.GetKey(KeyCode.LeftControl))
        {
            for (int i = 0; i < 10; i++)
            {
                if (Input.GetKeyDown(KeyCode.F1 + i))
                {
                    ActivateCustomCamera(i);
                    return;
                }
            }
        }
    }
}
