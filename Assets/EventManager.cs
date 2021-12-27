using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] public Canvas Canvas;
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject HelpPanel;
    [SerializeField] private GameObject AboutPanel;
    [SerializeField] private GameObject AlertPanel;
    [SerializeField] internal GameObject LoadingPanel;

    [SerializeField] private ExitManager ex;
    [SerializeField] private LoadingManager lm;

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        Canvas.gameObject.SetActive(true);
    }

    private void Start()
    {
        ex = gameObject.GetComponent<ExitManager>();
        lm = gameObject.GetComponent<LoadingManager>();
        InitPanel();
        ChangePanel(0);
        Input.backButtonLeavesApp = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!MainPanel.activeSelf)
                ChangePanel(0);
            else
                Quit();
        }
    }

    public void StartApp(int s)
    {
        ChangePanel(98);
        StartCoroutine(lm.LoadScene(s));
    }

    //0 = Main
    //1 = Help
    //2 = About
    //98= Loading...
    //99= Exit Alert
    public void ChangePanel(int p)
    {
        SwicthPanel(p);
    }

    private void InitPanel()
    {
        MainPanel = Canvas.transform.Find("MainPanel").gameObject;
        HelpPanel = Canvas.transform.Find("HelpPanel").gameObject;
        AboutPanel = Canvas.transform.Find("AboutPanel").gameObject;
        AlertPanel = Canvas.transform.Find("AlertPanel").gameObject;
        LoadingPanel = Canvas.transform.Find("LoadingPanel").gameObject;
    }

    private void SwicthPanel(int p)
    {
        MainPanel.SetActive(false);
        HelpPanel.SetActive(false);
        AboutPanel.SetActive(false);
        AlertPanel.SetActive(false);
        LoadingPanel.SetActive(false);

        switch (p)
        {
            case 0:
                MainPanel.SetActive(true);
                break;
            case 1:
                HelpPanel.SetActive(true);
                break;
            case 2:
                AboutPanel.SetActive(true);
                break;
            case 98:
                LoadingPanel.SetActive(true);
                break;
            case 99:
                AlertPanel.SetActive(true);
                break;
            default:
                break;
        } 
    }

    public void ExitApp()
    {
        Quit();
    }

    public void ShowQuitAlert()
    {
        ChangePanel(99);
    }

    private void Quit()
    {
        ex.Quit();
    }

}
