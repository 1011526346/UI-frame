/***
 * 插件：UIManager
 * 功能：1：解析JSON，管理UI界面的加载和切换。
 *       2：分别用两个字典保存从JSON读取的面板信息和已动态加载实例化的面板。
 *       3：通过栈来管理场景中所有面板之间的切换。
 *       ui面板与该类做成单例
 * 作者：李平
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using LitJson;

public class UIManager
{
    #region 将该类做成单例模式
    // 单例模式：定义一个静态的对象，构造方法私有化，内部构造，用于外部访问
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }
    #endregion

    //字典存储所有面板的Prefabs路径
    private Dictionary<UIPanelType, string> panelPathDict = new Dictionary<UIPanelType, string>();
    //保存所有已实例化面板的游戏物体身上的BasePanel组件
    private Dictionary<UIPanelType, BasePanel> panelDict = new Dictionary<UIPanelType, BasePanel>();
    //存储当前场景中的界面
    private Stack<BasePanel> panelStack = new Stack<BasePanel>();
    #region 将CanVas做成单例
    private Transform canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }
    #endregion
    private UIManager()//私有
    {
        ParseUIPanelTypeJson();
    }

    /// <summary>
    /// 解析JSON，获取所有面板的路径信息
    /// </summary>
    private void ParseUIPanelTypeJson()
    {
        TextAsset ta = Resources.Load<TextAsset>("UIPanelType");//读取josn文件
        JsonData jsonDataArray = JsonMapper.ToObject(ta.text);//json对象
        foreach (JsonData item in jsonDataArray)
        {
            UIPanelType panelType = (UIPanelType)Enum.Parse(typeof(UIPanelType), item["panelType"].ToString());//将字符串转换成枚举
            string path = item["path"].ToString();//路径
            panelPathDict.Add(panelType, path);
        }
    }

    /// <summary>
    /// 根据面板类型，返回对应的BasePanel组件
    /// </summary>
    /// <param name="panelType">需要返回的面板类型</param>
    /// <returns>返回该面板组件</returns>
    private BasePanel GetPanel(UIPanelType panelType)
    {//根据枚举找到路径，克隆
        BasePanel basePanel = panelDict.GetValue(panelType);
        //如果panel为空，根据该面板prefab的路径，实例化该面板
        if (basePanel == null)
        {
            string path = panelPathDict.GetValue(panelType);
            GameObject newPanel = GameObject.Instantiate(Resources.Load<GameObject>(path)) as GameObject;
            newPanel.transform.SetParent(CanvasTransform, false); //放在Canvas下
            //第一次实例化的面板需要保存在字典中
            panelDict.Add(panelType, newPanel.GetComponent<BasePanel>());
            return newPanel.GetComponent<BasePanel>();
        }
        else
        {
            return basePanel;
        }
    }

    /// <summary>
    /// 设置默认的栈顶元素
    /// </summary>
    /// <param name="panelType">界面类型</param>
    /// <param name="basePanel">组件</param>
    public void SetDefaultPopPanel(UIPanelType panelType,BasePanel basePanel)  //枚举，面板
    {
        panelDict.Add(panelType, basePanel);//
        panelStack.Push(basePanel);//入栈
    }

    /// <summary>
    /// 把该页面显示在场景中
    /// </summary>
    /// <param name="panelType">需要显示界面的类型</param>
    public void PushPanel(UIPanelType panelType)
    {
        //判断一下栈里面是否有页面
        if (panelStack.Count > 0)
        {
            panelStack.Peek().OnPause();//原栈顶界面暂停，指针指向栈顶面板
        }
        BasePanel panel = GetPanel(panelType);//克隆面板
        panel.OnEnter();//调用进入动作
        panelStack.Push(panel);//页面入栈
    }

    /// <summary>
    /// 关闭栈顶界面显示
    /// </summary>
    public void PopPanel()
    {
        //当前栈内为空，则直接返回
        if (panelStack.Count <= 0) return;//不在删除
        panelStack.Pop().OnExit();//Pop删除栈顶元素，并关闭栈顶界面的显示，
        if (panelStack.Count <= 0) return;//不在显示
        panelStack.Peek().OnResume();//获取现在栈顶界面，并调用界面恢复动作
    }
}