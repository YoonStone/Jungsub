using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class YG_Ending_GM : MonoBehaviour
{
    public GameObject sprite_Logo, sprite_Animation, audio_Last, audio_Background, video_Ending;

    [Header("로고 올라가는 속도")]
    public float speed_LogoUp;

    bool isLogoUp, isSoundDown;

    private void Start()
    {
        StartCoroutine("EndingCtrl");
    }

    private void Update()
    {
        if (isLogoUp)
        {
            sprite_Logo.transform.position = Vector3.Lerp(sprite_Logo.transform.position,
               new Vector3(sprite_Logo.transform.position.x, 18, sprite_Logo.transform.position.z),
               speed_LogoUp * Time.deltaTime);
        }

        if (isSoundDown)
        {
            audio_Background.GetComponent<AudioSource>().volume = Mathf.Lerp(audio_Background.GetComponent<AudioSource>().volume, 0, 0.8f * Time.deltaTime);
        }
    }

    public IEnumerator EndingCtrl()
    {
        // 1초 후 대사 시작
        yield return new WaitForSeconds(1);
        audio_Last.SetActive(true);

        // 대사가 끝난 후 엔딩사운드 + 로고
        yield return new WaitForSeconds(audio_Last.GetComponent<AudioSource>().clip.length); // 6초
        audio_Background.SetActive(true);
        sprite_Logo.SetActive(true);

        // 로고 생긴 후 로고 살짝 위로 올라가기
        yield return new WaitForSeconds(2);
        isLogoUp = true;

        // 움직이는 스프라이트
        sprite_Animation.SetActive(true);

        // 엔딩크레딧 영상이 끝나면 소리와 화면 페이드 아웃
        yield return new WaitForSeconds((float)video_Ending.GetComponent<VideoPlayer>().length);
        isSoundDown = true;

        // 페이트 아웃
        if (GameObject.FindWithTag("MainCamera"))
        {
            GameObject.FindWithTag("MainCamera").GetComponent<OVRScreenFade>().FadeOut();
        }

        yield return new WaitForSeconds(2); // 페이드아웃 걸리는 시간

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }
}
