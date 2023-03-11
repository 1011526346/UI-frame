/***
 * 插件：UI基类BasePanel
 * 功能：1：UI基类BasePanel场景中的界面继承该基类，并将四种状态写成虚方法，
 * 依次分别为OnEnter()，OnPause()，OnResume()，OnExit()，提供给子类重写。
 * 作者：李平
 */
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    /// <summary>
    /// 界面显示出来
    /// </summary>
    public virtual void OnEnter() { }

    /// <summary>
    /// 界面暂停(弹出了其他界面)
    /// </summary>
    public virtual void OnPause() { }

    /// <summary>
    /// 界面继续(其他界面移除，回复本来的界面交互)
    /// </summary>
    public virtual void OnResume() { }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关闭
    /// </summary>
    public virtual void OnExit() { }
}
