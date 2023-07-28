using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YG_House_GM : MonoBehaviour
{
    [Header("시작 후 편지오기 전까지 시간")]
    public float time_BeforeLetter = 8;

    //[Header("집이 점점 보이는 시간")]
    //public float time_HouseUp = 0.2f;

    [Header("편지가 날아오고 눈감기 전까지 시간")]
    public float time_BeforeClose;

    public GameObject letter, letter_BG, eye_Up, eye_Down, audio_Background;
    //public Light[] light_Pin;

    bool isSoundDown;
    int eyeState;

    private void Start()
    {
        StartCoroutine("SequenceCtrl");
    }    

    private void Update()
    {
        if (isSoundDown)
        {
            audio_Background.GetComponent<AudioSource>().volume = Mathf.Lerp(audio_Background.GetComponent<AudioSource>().volume, 0, 1 * Time.deltaTime);
        }

        switch (eyeState)
        {
            // 아무것도 x
            case 0:
                break;
            // 눈 감기
            case 1:
                eye_Up.transform.localPosition = Vector3.Lerp(eye_Up.transform.localPosition,
                    new Vector3(0, 0.2f, 0.1001f), 0.8f * Time.deltaTime);
                eye_Down.transform.localPosition = Vector3.Lerp(eye_Down.transform.localPosition,
                    new Vector3(0, -0.2f, 0.1001f), 0.8f * Time.deltaTime);
                break;
            // 눈 살짝 뜨기
            case 2:
                eye_Up.transform.localPosition = Vector3.Lerp(eye_Up.transform.localPosition,
                    new Vector3(0, 0.25f, 0.1001f), 1 * Time.deltaTime);
                eye_Down.transform.localPosition = Vector3.Lerp(eye_Down.transform.localPosition,
                    new Vector3(0, -0.25f, 0.1001f), 1 * Time.deltaTime);
                break;
            // 눈 감기
            case 3:
                eye_Up.transform.localPosition = Vector3.Lerp(eye_Up.transform.localPosition,
                    new Vector3(0, 0.2f, 0.1001f), 1 * Time.deltaTime);
                eye_Down.transform.localPosition = Vector3.Lerp(eye_Down.transform.localPosition,
                    new Vector3(0, -0.2f, 0.1001f), 1 * Time.deltaTime);
                break;
        }

    }

    public IEnumerator SequenceCtrl()
    {
        // 편지 날아오기
        yield return new WaitForSeconds(time_BeforeLetter);
        letter.SetActive(true);
        letter_BG.SetActive(true);

        //// 순서대로 스프라이트 핀조명 On
        //yield return new WaitForSeconds(5);
        //for (int i = 0; i < light_Pin.Length; i++)
        //{
        //    light_Pin[i].gameObject.SetActive(true);
        //    yield return new WaitForSeconds(2);
        //}

        // 눈 감기
        yield return new WaitForSeconds(time_BeforeClose);
        eyeState = 1;

        // 살짝 뜨기
        yield return new WaitForSeconds(2f);
        eyeState = 2;

        // 다시 감기
        yield return new WaitForSeconds(1);
        eyeState = 3;

        // 페이트 아웃
        yield return new WaitForSeconds(2f);

        // 배경음악 페이드아웃
        isSoundDown = true;

        if (GameObject.FindWithTag("MainCamera"))
        {
            GameObject.FindWithTag("MainCamera").GetComponent<OVRScreenFade>().FadeOut();
        }

        // 씬전환
        yield return new WaitForSeconds(3); // 페이드아웃 걸리는 시간
        SceneManager.LoadScene(5);
    }
}
