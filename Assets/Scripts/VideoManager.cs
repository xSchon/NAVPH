using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoManager : MonoBehaviour
{
    public void SkipVideo()
    {
        SceneManager.LoadScene("OfficeScene");
    }
}
