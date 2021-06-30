using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraManager : BaseManager<CameraManager>
{

    private CameraController mainCamCtrl = null;
    private CameraController curCameraCtrl = null;

    public CameraController CurrentCameraCtrl { get => curCameraCtrl; }


    private Dictionary<int, CameraController> camerDic = new Dictionary<int, CameraController>();

    protected override void Awake()
    {
        base.Awake();
    }


    public void SetMainCamera(CameraController cam)
    {
        mainCamCtrl = cam;
        curCameraCtrl = mainCamCtrl;
    }

    public void AddCamera(int idx, CameraController cam)
    {
        if (cam == null)
            return;

        camerDic.Add(idx, cam);
        cam.gameObject.SetActive(false);
    }

    // TODO 임시
    private void LateUpdate()
    {
        if(mainCamCtrl != null && mainCamCtrl.gameObject.activeSelf == false)
        {
            mainCamCtrl.CameraCtrl();
        }

        TempInputCamera(10);
    }

    private void TempInputCamera(int count)
    {
        //if (Input.GetKey(KeyCode.LeftControl))
        {
            int tempIdx = (int)KeyCode.F1;
            if (Input.GetKeyDown(KeyCode.F1))
            {
                ActiveCamera(0);
            }
            else
            {
                foreach (var item in camerDic)
                {
                    if (Input.GetKeyDown((KeyCode)(tempIdx + item.Key)))
                    {
                        ActiveCamera(item.Key);
                    }
                }
            }
        }
    }

    public void ActiveCamera(int idx)
    {
        if (idx == 0)
        {
            if (mainCamCtrl != null)
            {
                DisableAllCamera();
                mainCamCtrl.gameObject.SetActive(true);
                curCameraCtrl = mainCamCtrl;
            }
        }
        else
        {
            if(camerDic.ContainsKey(idx))
            {
                DisableAllCamera();
                camerDic[idx].gameObject.SetActive(true);
                curCameraCtrl = camerDic[idx];
            }
        }
    }

    public void DisableAllCamera()
    {
        mainCamCtrl.gameObject.SetActive(false);

        foreach (var item in camerDic)
        {
            item.Value.gameObject.SetActive(false);
        }
    }

    public Transform GetMoveDirCamera()
    {
        if(mainCamCtrl.gameObject.activeSelf)
        {
            return mainCamCtrl.transform;
        }
        else
        {
            if (curCameraCtrl != null && curCameraCtrl.IsTargetPlayerCamera)
                return curCameraCtrl.transform;

            return mainCamCtrl.transform;
        }
    }
}
