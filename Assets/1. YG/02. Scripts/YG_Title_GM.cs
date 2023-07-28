using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class YG_Title_GM : MonoBehaviour
{
    public GameObject video;
    VideoPlayer videoPlyar;

    private void Start()
    {
        videoPlyar = video.GetComponent<VideoPlayer>();
        StartCoroutine("Ending");
    }

    public IEnumerator Ending()
    {
        yield return new WaitForSeconds((float)videoPlyar.length);
        // 페이트 아웃
        if (GameObject.FindWithTag("MainCamera"))
        {
            GameObject.FindWithTag("MainCamera").GetComponent<OVRScreenFade>().FadeOut();
        }

        yield return new WaitForSeconds(2); // 페이드아웃 걸리는 시간
        // 씬전환
        SceneManager.LoadScene(2);

    }
}
