using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 시계소리 Pitch 0.72가 딱 맞음
public class YG_Hospital_GM : MonoBehaviour
{
    public GameObject eye_Up, eye_Down,
        audio_People, audio_Me, audio_Breath, audio_Clock, audio_ClockEnd, audio_Neon,
        particle_Paint, light_Clock, anim_BedCover;
    public Transform minutesTransform, secondsTransform;

    [Header("첫번째 대사 시작 (에휴)")]
    public float time_Start = 1;

    [Header("초바늘, 분바늘 시작 시간")]
    public int second = 0, minute = 44;

    bool isClockEnd, isStartClock, isBreathDown, isEyeClose, isNeonDown;
    int eyeState;
    float time;

    const float
        degreesPerMinute = 6f,
        degreesPerSecond = 6f;

    private void Start()
    {
        StartCoroutine("EyesOpenCtrl");
    }

    public IEnumerator EyesOpenCtrl()
    {
        yield return new WaitForSeconds(time_Start);
        audio_People.SetActive(true);

        // 첫번째 대사 끝나면 눈 살짝 뜨기
        yield return new WaitForSeconds(audio_People.GetComponent<AudioSource>().clip.length);
        eyeState = 1;

        // 숨소리, 시계소리 시작
        audio_Breath.SetActive(true);
        audio_Clock.SetActive(true);

        // 시계 움직임 시작
        isStartClock = true;

        // 살짝 뜬 후에 감기
        yield return new WaitForSeconds(4);
        eyeState = 2;

        // 다 감기면 눈 살짝 더 뜨기
        yield return new WaitForSeconds(1);
        eyeState = 3;

        // 살짝 더 뜬 후에 감기
        yield return new WaitForSeconds(3f);
        eyeState = 4;

        // 다 감기면 눈 다 뜨기
        yield return new WaitForSeconds(2);
        eyeState = 5;

        // 눈 다 뜨고 나면 40초 후 죽음시작
        yield return new WaitForSeconds(5);
        eyeState = 0;
    }

    private void Update()
    {
        switch (eyeState)
        {
            // 아무것도 x
            case 0:
                break;
            // 눈 살짝 뜨기
            case 1:
                eye_Up.transform.localPosition = Vector3.Lerp(eye_Up.transform.localPosition,
                    new Vector3(0, 0.24f, 0.1001f), 1.2f * Time.deltaTime);
                eye_Down.transform.localPosition = Vector3.Lerp(eye_Down.transform.localPosition,
                    new Vector3(0, -0.24f, 0.1001f), 1.2f * Time.deltaTime);

                break;
            // 눈 감기
            case 2:
                eye_Up.transform.localPosition = Vector3.Lerp(eye_Up.transform.localPosition, 
                    new Vector3(0, 0.2f, 0.1001f), 0.3f * Time.deltaTime);
                eye_Down.transform.localPosition = Vector3.Lerp(eye_Down.transform.localPosition,
                    new Vector3(0, -0.2f, 0.1001f), 0.3f * Time.deltaTime);
                break;
            // 눈 살짝 더 뜨기
            case 3:
                eye_Up.transform.localPosition = Vector3.Lerp(eye_Up.transform.localPosition,
                    new Vector3(0, 0.28f, 0.1001f), 1 * Time.deltaTime);
                eye_Down.transform.localPosition = Vector3.Lerp(eye_Down.transform.localPosition,
                    new Vector3(0, -0.28f, 0.1001f), 1 * Time.deltaTime);
                break;
            // 눈 감기
            case 4:
                eye_Up.transform.localPosition = Vector3.Lerp(eye_Up.transform.localPosition,
                    new Vector3(0, 0.2f, 0.1001f), 1 * Time.deltaTime);
                eye_Down.transform.localPosition = Vector3.Lerp(eye_Down.transform.localPosition,
                    new Vector3(0, -0.2f, 0.1001f), 1 * Time.deltaTime);
                break;
            // 눈 다 뜨기
            case 5:
                eye_Up.transform.localPosition = Vector3.Lerp(eye_Up.transform.localPosition,
                    new Vector3(0, 0.35f, 0.1001f), 0.4f * Time.deltaTime);
                eye_Down.transform.localPosition = Vector3.Lerp(eye_Down.transform.localPosition,
                    new Vector3(0, -0.35f, 0.1001f), 0.4f * Time.deltaTime);
                break;
        }

        // 시계 움직임 시작
        if (isStartClock)
        {
            ClockCtrl();
        }

        // 숨소리 줄어들기
        if (isBreathDown)
        {
            audio_Breath.GetComponent<AudioSource>().volume = Mathf.Lerp(audio_Breath.GetComponent<AudioSource>().volume, 0, 1 * Time.deltaTime);
        }

        if (isNeonDown)
        {
            audio_Neon.GetComponent<AudioSource>().volume = Mathf.Lerp(audio_Neon.GetComponent<AudioSource>().volume, 0, 0.6f * Time.deltaTime);
        }
    }

    private void ClockCtrl()
    {
        if (!isClockEnd)
        {
            time += Time.deltaTime;

            if (time >= 1)
            {
                second += 1;
                if (second == 60)
                {
                    second = 0;
                    minute += 1;
                }

                time = 0;
            }

            UpdateDiscrete();

            // 55초가 되면 숨소리 작아지고, 시계 핀조명 켜고, 똑딱소리 크게
            if(second == 55)
            {
                isBreathDown = true;
                light_Clock.SetActive(true);
                audio_Clock.GetComponent<AudioSource>().volume = 1;
            }
            if (second == 59)
            {
                // 멈추는 시계소리 켜기
                StartCoroutine(ClockEndAudio());
                // 이불 숨쉬는 애니메이션 멈추기
                anim_BedCover.GetComponent<Animator>().enabled = false;
            }
        }

        if (isEyeClose)
        {
            eye_Up.transform.localPosition = Vector3.Lerp(eye_Up.transform.localPosition,
                new Vector3(0, 0.2f, 0.1001f), 1f * Time.deltaTime);
            eye_Down.transform.localPosition = Vector3.Lerp(eye_Down.transform.localPosition,
                new Vector3(0, -0.2f, 0.1001f), 1f * Time.deltaTime);

            // 눈이 다 감겼다면
            if (eye_Up.transform.localPosition.y <= 0.21f)
            {
                StartCoroutine("Ending");
            }
        }
    }

    public IEnumerator ClockEndAudio()
    {
        // 시계소리, 심장소리 멈추고
        yield return new WaitForSeconds(0.4f);
        audio_Clock.GetComponent<AudioSource>().Stop();

        // 멈추는 시계소리 켜기
        yield return new WaitForSeconds(0.6f);
        audio_ClockEnd.SetActive(true);
        isClockEnd = false;

        // 탕, 1초 후 나에게도 가족이 있소 
        yield return new WaitForSeconds(1f);
        audio_Me.SetActive(true);

        // 대사가 끝나면 파티클 실행 + 효과음
        yield return new WaitForSeconds(audio_Me.GetComponent<AudioSource>().clip.length);
        particle_Paint.SetActive(true);
        audio_Neon.SetActive(true);

        // 눈 감기 시작
        yield return new WaitForSeconds(5);
        isEyeClose = true;
        isNeonDown = true;
    }

    // 시계 기능
    void UpdateDiscrete()
    {
        minutesTransform.localRotation =
            Quaternion.Euler(0f, 0f, -minute * degreesPerMinute);
        secondsTransform.localRotation =
            Quaternion.Euler(0f, 0f, -second * degreesPerSecond);
    }

    // 끝나기 + 씬전환
    public IEnumerator Ending()
    {
        // 페이트 아웃
        yield return new WaitForSeconds(0);
        if (GameObject.FindWithTag("MainCamera"))
        {
            GameObject.FindWithTag("MainCamera").GetComponent<OVRScreenFade>().FadeOut();
        }

        // 씬전환
        yield return new WaitForSeconds(2); // 페이드아웃 걸리는 시간
        SceneManager.LoadScene(1);
    }
}
