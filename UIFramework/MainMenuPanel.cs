using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MainMenuPanel : BasePanel
{
    //传入UI面板
    public void OnPushPanel(string panelTypeString)
    {
        UIPanelType panelType = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeString);//根据字符串，转换成枚举
        UIManager.Instance.PushPanel(panelType);//出栈
    }
}
