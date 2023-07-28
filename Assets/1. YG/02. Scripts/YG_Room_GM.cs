using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YG_Room_GM : MonoBehaviour
{
    public GameObject eye_Up, eye_Down,
        letter_Fall1, letter_Fall2, letter_Fall3,
        audio_Voice, letter_BG, collider_Dowan;

    [Header("첫번째 편지 떨어지기 전까지 시간")]
    public float time_BeforeLetter1 = 20;

    [Header("첫번째 후 두번째 편지 떨어지기 전까지 시간")]
    public float time_BeforeLetter2 = 30;

    [Header("두번째 후 세번째 편지 떨어지기 전까지 시간")]
    public float time_BeforeLetter3 = 30;

    int eyeState;

    private void Start()
    {
        StartCoroutine("EyesOpenCtrl");
        StartCoroutine("LetterFall");
    }

    public IEnumerator EyesOpenCtrl()
    {
        // 눈 살짝 뜨기
        yield return new WaitForSeconds(0);
        eyeState = 1;

        // 살짝 뜬 후에 감기
        yield return new WaitForSeconds(2);
        eyeState = 2;

        // 다 감기면 눈 다 뜨기
        yield return new WaitForSeconds(1.6f);
        eyeState = 3;

        // 눈 다 뜬 상태
        yield return new WaitForSeconds(1f);
        eyeState = 4;

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
                    new Vector3(0, 0.28f, 0.1001f), 1.2f * Time.deltaTime);
                eye_Down.transform.localPosition = Vector3.Lerp(eye_Down.transform.localPosition,
                    new Vector3(0, -0.28f, 0.1001f), 1.2f * Time.deltaTime);
                break;

            // 눈 감기
            case 2:
                eye_Up.transform.localPosition = Vector3.Lerp(eye_Up.transform.localPosition,
                    new Vector3(0, 0.2f, 0.1001f), 1f * Time.deltaTime);
                eye_Down.transform.localPosition = Vector3.Lerp(eye_Down.transform.localPosition,
                    new Vector3(0, -0.2f, 0.1001f), 1f * Time.deltaTime);              
                break;

            // 눈 다 뜨기
            case 3:
                eye_Up.transform.localPosition = Vector3.Lerp(eye_Up.transform.localPosition,
                    new Vector3(0, 0.5f, 0.1001f), 0.8f * Time.deltaTime);
                eye_Down.transform.localPosition = Vector3.Lerp(eye_Down.transform.localPosition,
                    new Vector3(0, -0.5f, 0.1001f), 0.8f * Time.deltaTime);
                break;

            // 눈 다 뜬 상태
            case 4:
                // 편지가 눈 앞에 멈추면 (0.5까지 떨어짐)
                if (letter_Fall3.transform.localPosition.y <= 0.7f)
                {
                    // 다른 편지 소리 다 끄기
                    GameObject[] btmLetters = GameObject.FindGameObjectsWithTag("BOTTOMLETTER");

                    for (int i = 0; i < btmLetters.Length; i++)
                    {
                        btmLetters[i].GetComponent<AudioSource>().enabled = false;
                    }

                    // 중섭목소리 재생
                    audio_Voice.SetActive(true);

                    // 그림 방향에 콜라이더 생성
                    collider_Dowan.SetActive(true);

                    // 한번만 호출
                    eyeState = 0;
                }
                break;
        }
    }

    public IEnumerator LetterFall()
    {
        // 첫번째 편지 떨어지기
        yield return new WaitForSeconds(time_BeforeLetter1);
        letter_Fall1.GetComponent<Rigidbody>().isKinematic = false;

        // 두번째 편지 떨어지기
        yield return new WaitForSeconds(time_BeforeLetter2);
        letter_Fall2.GetComponent<Rigidbody>().isKinematic = false;

        // 세번째 편지 떨어지기 (베지어커브 실행)
        yield return new WaitForSeconds(time_BeforeLetter3);
        letter_BG.SetActive(true);
    }
}
