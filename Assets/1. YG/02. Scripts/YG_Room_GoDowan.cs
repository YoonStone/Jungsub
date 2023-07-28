using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BansheeGz.BGSpline.Curve;

public class YG_Room_GoDowan : MonoBehaviour
{
    [Header("라이팅이 어두워지는 속도")]
    public float speed_LightDown;

    public Light[] lights; // 0: 도원그림 비추는 빛
                           // 1 ~ : 도원그림 비추는 빛 제외한 빛
    public GameObject frame, bg_Map, audio_GoDowan, audio_Background;

    bool isEnding, isBackDown;

    // 도원그림이 다른 물체와 닿았을 때 호출됨
    private void OnTriggerEnter(Collider other)
    {
        // 만약 닿은 물건의 태그가 날아온 편지("LETTER")라면
        if(other.gameObject.tag == "GRABLETTER3")
        {
            // 엔딩 시작
            StartCoroutine("Ending");
        }
    }

    private void Update()
    {
        // 도원그림 부분을 제외한 다른 라이트 페이드 아웃
        if (isEnding)
        {
            for (int i = 1; i < lights.Length; i++)
            {
                lights[i].intensity = Mathf.Lerp(lights[i].intensity, 0, speed_LightDown * Time.deltaTime);
            }
        }

        // 배경음악 페이드 아웃
        if (isBackDown)
        {
            audio_Background.GetComponent<AudioSource>().volume = Mathf.Lerp(audio_Background.GetComponent<AudioSource>().volume, 0, speed_LightDown * Time.deltaTime);
        }
    }

    // 엔딩 관리
    public IEnumerator Ending()
    {
        // 일렁이는 효과 + 효과음 재생 + 라이팅 조절
        yield return new WaitForSeconds(0);
        // 헤이트맵 -> 쉐이더일거같음*

        // 배경음악 끄고 효과음*
        isBackDown = true;
        audio_GoDowan.SetActive(true);

        // 라이팅 조절
        isEnding = true;

        // 플레이어 이동
        yield return new WaitForSeconds(1);
        bg_Map.SetActive(true);

        // 페이드아웃
        yield return new WaitForSeconds(3);
        if (GameObject.FindWithTag("MainCamera"))
        {
            GameObject.FindWithTag("MainCamera").GetComponent<OVRScreenFade>().FadeOut();
        }

        // 씬전환
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(3);
    }
}
