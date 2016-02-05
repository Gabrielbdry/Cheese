using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickToLoadASync : MonoBehaviour {

    public Slider LoadingBar;
    public GameObject LoadingImage;

    private AsyncOperation async;

    public void ClickASync(int level)
    {
        LoadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(level));
    }

    IEnumerator LoadLevelWithBar(int level)
    {
        async = Application.LoadLevelAsync(level);
        while (!async.isDone)
        {
            LoadingBar.value = async.progress;
            yield return null;
        }
    }
}
