using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject currentPanel;
    private Transform canvas;
    private GameObject goodsPanel;
    private GameObject levelPanel;
    private GameObject editorPanel;

    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Transform>();
        goodsPanel = GameObject.Find("GoodsPanel");
        levelPanel = GameObject.Find("LevelPanel");
        editorPanel = GameObject.Find("EditorPanel");
    }

    public void ShowGoodsPanel()
    {
        ShowAnim(goodsPanel);
    }

    public void ShowTaskPanel()
    {
        ShowAnim(levelPanel);
    }

    public void ShowEditorPanel()
    {
        ShowAnim(editorPanel);
    }

    public void OnCloseClick()
    {
        HideAnim(currentPanel);
    }

    private void ShowAnim(GameObject panel)
    {
        if (currentPanel!=null)
        {
            HideAnim(currentPanel);
        }
        panel.SetActive(true);
        panel.transform.DOLocalMoveX(0,0.5f);
        panel.transform.DOScale(Vector3.one, 0.5f);
        currentPanel = panel;
    }

    private void HideAnim(GameObject panel)
    {
        panel.transform.DOLocalMoveX(-720, 0.5f);
        panel.transform.DOScale(Vector3.zero, 0.5f).OnComplete(()=>panel.SetActive(false));
        currentPanel = null;
    }

}
