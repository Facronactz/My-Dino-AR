using System;
using System.Collections;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    private static bool AllowQuit;
    private static EventManager ev;

    void Awake()
    {
        AllowQuit = false;
    }

    void Start()
    {
        ev = gameObject.GetComponent<EventManager>();
    }

    //if Unity Quit Interceptor is failed
    internal void Quit()
    {
        try
        {
            ev.ShowQuitAlert();
        }
        catch
        {
            Application.Quit();
        }
    }

    public void YesOrNo(string s)
    {
        if ( s == "Yes" )
        {
            AllowQuit = true;
            Application.Quit();
        }
        else if ( s == "No")
        {
            ev.ChangePanel(0);
        }
    }

    [SerializeField]
    static bool WantsToQuit()
    {
        Debug.Log("Quiting...");
        try
        {
            ev.ShowQuitAlert();
        }
        catch
        {
            Application.Quit();
        }
        if (AllowQuit)
            return true;
        else return false;
    }

    [RuntimeInitializeOnLoadMethod]
    static void RunOnStart()
    {
        Application.wantsToQuit += WantsToQuit;
    }

    //void OnApplicationQuit()
    //{
    //    Debug.Log("Quiting...");
    //    ev.ShowQuitAlert();
    //    if (!AllowQuit)
    //        Application.CancelQuit();
    //}
}
