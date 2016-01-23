using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {

    public GameObject LoadingImage;

	public void LoadScene(int level)
    {
        LoadingImage.SetActive(true);
        Application.LoadLevel(level);
    }
}
