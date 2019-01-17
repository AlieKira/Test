using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraManager : BaseManager {
    public CameraManager(GameFacade gameFacade) : base(gameFacade) { }

    private GameObject cameraGO;
    private GameObject TimeLine;
    private GameObject currentRole;
    private FollowTarget followTarget;
    private Quaternion targetQuaternion;
    private Vector3 originalPosition;
    private Vector3 originalRotation;

    public override void OnInit()
    {
        cameraGO = Camera.main.gameObject;
        TimeLine=GameObject.Find("TimeLine");
        cameraGO.SetActive(false);
        TimeLine.SetActive(true);
        followTarget = cameraGO.GetComponent<FollowTarget>();
    }

    public void ShowTimeLine()
    {
        cameraGO.SetActive(false);
        TimeLine.SetActive(true);
    }

    public void ShowMainCamera()
    {
        TimeLine.SetActive(false);
        cameraGO.SetActive(true);
    }

    public void FollowRole()
    {
        originalPosition = cameraGO.transform.position;
        originalRotation = cameraGO.transform.eulerAngles;
        followTarget.target = gameFacade.GetCurrentRole();
        targetQuaternion=Quaternion.LookRotation(Vector3.zero-new Vector3(0,7.2f,8.5f));
        cameraGO.transform.DORotateQuaternion(targetQuaternion,2);
        cameraGO.transform.DOMove((followTarget.target.transform.position + followTarget.offset),2).OnComplete(
            delegate()
            {
                followTarget.enabled = true;
                gameFacade.StartPlaying();
            });
    //    cameraGO.transform.DORotateQuaternion(targetQuaternion, 1f).OnComplete(delegate()
    //    {
    //        followTarget.enabled = true;
    //    });
    }

    public void StopFollowRole()
    {
        followTarget.enabled=false;
        cameraGO.transform.DOMove(originalPosition, 1f);
    }
}
