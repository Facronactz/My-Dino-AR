using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arEventManager : MonoBehaviour
{
    public bool isTracked;
    public bool isHidden;
    public float RotationSpeed;
    public GameObject currentObj;

    public GameObject OptionPanel;

    [SerializeField]
    public bool ShouldVibrate { get; set; }

    private Vector3 asdf;

    private float initialFingersDistance;

    private Vector3 initialScale;

    private Canvas currentCanvas;

    public Image OptionButton;
    private Sprite OldButton;
    public Sprite NewButton;

    public GameObject HideToggle;
    public GameObject MainUI;
    [SerializeField] private bool Optioning;
    public Slider opacity;
    [SerializeField] private bool isSounding;
    public bool shouldSounding { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        isSounding = false;
        shouldSounding = true;
        ShouldVibrate = true;
        isTracked = false;
        OptionPanel.SetActive(false);
        Optioning = false;
        OldButton = OptionButton.sprite;
        asdf = new Vector3(270, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTracked)
        {
            transformObj();
            if (shouldSounding)
            {
                if (!isSounding)
                {
                    isSounding = true;
                    try
                    {
                        currentObj.GetComponent<AudioSource>().Play();
                    }
                    catch
                    {
                        throw new System.Exception("Object has no Sound");
                    }
                }
            }
            else
            {
                try
                {
                    currentObj.GetComponent<AudioSource>().Stop();
                }
                catch
                {
                    throw new System.Exception("Cant Stop Audio");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Back();
        checkHidden();
    }

    public void AlwaysHIde(bool b)
    {
        if (b)
        {
            opacity.gameObject.SetActive(true);
            HideToggle.SetActive(false);
            isHidden = true;
            MainUI.GetComponent<CanvasGroup>().alpha = opacity.value ;
            ShowOption();
        }
        else if (!b)
        {
            opacity.gameObject.SetActive(false);
            HideToggle.SetActive(true);
            isHidden = false;
            MainUI.GetComponent<CanvasGroup>().alpha = 1 ;
        }
    }

    public void ChangeOpacity(float f)
    {
            MainUI.GetComponent<CanvasGroup>().alpha = f ;
    }

    public void ShowOption()
    {
        if (!Optioning)
        {
            OptionButton.sprite = NewButton;
            OptionPanel.SetActive(true);
            Optioning = true;
        }
            
        else if (Optioning)
        {
            OptionPanel.SetActive(false);
            OptionButton.sprite = OldButton;
            Optioning = false;
        }
    }

    private void transformObj()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AR Model"))
        {
            if (obj.GetComponent<MeshRenderer>().enabled)
            {
                currentObj = obj;
                //Debug.Log(obj);
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    obj.transform.Rotate((Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime), (Input.GetAxis("Mouse Y") * RotationSpeed * Time.deltaTime), 0, Space.Self);
                }
                else
                {
                    if (Input.touchCount > 0)
                    {
                        Touch touch = Input.GetTouch(0);
                        if (Input.touchCount == 1)
                        {
                            if (touch.phase == TouchPhase.Moved)
                            {
                                Quaternion rotate = Quaternion.Euler(
                                    touch.deltaPosition.y * RotationSpeed,
                                    -touch.deltaPosition.x * RotationSpeed,
                                    0f);
                                obj.transform.rotation = rotate * obj.transform.rotation;
                            }
                        }
                        else if (Input.touchCount == 2)
                        {
                            Touch t1 = Input.touches[0];
                            Touch t2 = Input.touches[1];
                            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
                            {
                                initialFingersDistance = Vector2.Distance(t1.position, t2.position);
                                initialScale = currentObj.transform.localScale;
                            }
                            else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
                            {

                                float currentFingersDistance = Vector2.Distance(t1.position, t2.position);
                                var scaleFactor = currentFingersDistance / initialFingersDistance;

                                Vector3 m_scale = initialScale * scaleFactor;

                                //m_scale.x = Mathf.Clamp(m_scale.x, m_minScale, m_maxScale);
                                //m_scale.y = Mathf.Clamp(m_scale.y, m_minScale, m_maxScale);
                                //m_scale.z = Mathf.Clamp(m_scale.z, m_minScale, m_maxScale);
                                currentObj.transform.localScale = m_scale;
                            }
                        }
                    }
                }
            }
        }
    }

    public void getState(bool _)
    {
        isHidden = _;
    }

    void checkHidden()
    {
        if (isHidden)
            HideUI(isHidden);
        else if (isTracked && !isHidden) {
            try
            {
                currentCanvas.enabled = true;
            }
            catch { }
        }
        //HideUI(false);
    }

    public void HideUI(bool isHiding)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AR UI"))
        {
            if (obj.GetComponent<Canvas>().enabled)
                currentCanvas = obj.GetComponent<Canvas>();
            obj.GetComponent<Canvas>().enabled = !isHiding;
        }
    }

    public void ResetView()
    {
        try
        {
            currentObj.transform.rotation = Quaternion.Euler(asdf);
            currentObj.transform.localScale = Vector3.one;
        }
        catch
        {
            throw new System.Exception("No Object Found");
        }
        finally
        {
            ShowOption();
        }
    }

    public void Back()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void TargetFound()
    {
        isTracked = true;
        if (ShouldVibrate)
            Handheld.Vibrate();
    }
    public void TargetLost()
    {
        isTracked = false;
        try
        {
            currentObj.GetComponent<AudioSource>().Stop();
        }
        catch
        {
            throw new System.Exception("Cant Stop Audio");
        }
        finally
        {
            isSounding = false;
            currentCanvas = null;
            currentObj = null;
        }
    }
}
