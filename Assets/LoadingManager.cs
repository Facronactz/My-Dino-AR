using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar;
    public TMPro.TMP_Text loadingText;
    private EventManager ev;
    [SerializeField] private float val;

    void Start()
    {
        ev = gameObject.GetComponent<EventManager>();
    }
    internal IEnumerator LoadScene(int i)
    {
        yield return null;

        loadingText = ev.LoadingPanel.transform.Find("loadingText").GetComponent<TMPro.TMP_Text>();
        progressBar = ev.LoadingPanel.transform.Find("progressBar").GetComponent<Slider>();

        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(i);
        op.allowSceneActivation = false;
        val = op.progress;

        while (!op.isDone)
        {
            Debug.Log(op.progress);
            loadingText.text = "Loading: " + (op.progress * 100) + "%"; 
            progressBar.value = Mathf.Clamp(op.progress, 0.12f, 1f);

            if(op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
