/***
*  功能：
*  作者：
*  版本：
*  日期：
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LoginPanel : BasePanel
{
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void OnEnter()
    {
        //显示
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        Vector3 temp = transform.localPosition;
        temp.x = -800;
        transform.localPosition = temp;
        transform.DOLocalMoveX(0, 0.5f);
    }

    public override void OnPause()
    {
        //切换到其他界面的时候
        canvasGroup.blocksRaycasts = false;
    }

    public override void OnResume()
    {
        //重新回到界面的时候
        canvasGroup.blocksRaycasts = true;
    }

    public override void OnExit()
    {
        //不显示
        transform.DOLocalMoveX(-800, .5f).OnComplete(() => canvasGroup.alpha = 0);
    }

    public void OnClosePanel()
    {
        //关闭面板
        UIManager.Instance.PopPanel();
        OnPause();//切换到其他界面的时候，射线点击false
    }
}
