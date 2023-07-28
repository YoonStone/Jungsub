using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// #도원씬 전체 관리 스크립트
public class YG_Dowan_GM : MonoBehaviour
{
    public GameObject wholeMap, rotIsland, children, letter, bottom,
        letterCome_BG, letterLeave_BG, fish1,
        audio_Background1, audio_Me1, audio_Me2, audio_Wave, audio_Wind, audio_UhUh, audio_FlowerRain,
        letter_GoalPos,
        anim_island;
    public GameObject[] smallIsland, hands;
    public ParticleSystem[] particle_Leaves, particle_Flowers, particle_FlowerEachs, particle_Snow;

    [Header("미들섬 상승시간")]
    public float time_MidlandUp;

    [Header("미들섬 목표높이")]
    public float high_Midland;
    
    [Header("미들섬 회전속도")]
    public float speed_MidIslandRot;

    [Header("미들섬 도착 후 편지 떨어지기 전까지 시간")]
    public float time_BeforeLetterCome = 60;

    [Header("편지가 날아갈 때부터 엔딩 전까지의 시간")]
    public float time_BeforeEnding = 3;

    [Header("엔딩 때 사용될 라이팅")]
    public Light light_Dowon;

    [Header("라이팅의 최소 밝기")]
    public float intensity_Light;

    [Header("라이팅이 어두워지는 속도")]
    public float speed_LightDown;

    [Header("나뭇잎 떨어질 때부터 씬전환까지의 시간")]
    public float time_ChangeScene = 10;

    [Header("파티클이 사라지는 속도")]
    public float speed_ParticleDown;

    private bool isEnding, isAudioDown, isFirstGoal;

    // 미들섬 상태관리
    enum MidIslandState
    {
        m_Under,
        m_UpNRotate,
        m_End
    }

    MidIslandState midState = MidIslandState.m_UpNRotate;

    private void Start()
    {
        Invoke("Next", audio_Background1.GetComponent<AudioSource>().clip.length);
    }
    private void Update()
    {
        // 전체 맵이 0에 도착했을 때 시작
        if (wholeMap.transform.position.z == 0)
        {
            // 도착하면 미들섬 움직임 시작
            MiddleIslandCtrl();
        }

        // 엔딩 때, 라이팅 조절
        if (isEnding)
        {
            light_Dowon.intensity = Mathf.Lerp(light_Dowon.intensity, intensity_Light, speed_LightDown * Time.deltaTime);
        }

        if (isFirstGoal)
        {
            letter.transform.position = Vector3.Lerp(letter.transform.position, letter_GoalPos.transform.position,
                    (1 / (letter_GoalPos.transform.position.y - letter.transform.position.y) * 0.3f * Time.deltaTime));
            letter.transform.rotation = Quaternion.Slerp(letter.transform.rotation, Quaternion.Euler(0, 0, 0),
                                (1 / (letter_GoalPos.transform.position.y - letter.transform.position.y) * 0.3f * Time.deltaTime));

            // 목표에 가까워지면 베지어커브 켜기
            if (letter.transform.position.y >= letter_GoalPos.transform.position.y - 0.05f)
            {
                letterLeave_BG.SetActive(true);
                isFirstGoal = false;
            }
        }

        // 편지가 거의 다 도착하면
        if (letter.transform.localPosition.y <= 0.2f)
        {
            // 바람 소리 끄기
            audio_Wind.SetActive(false);

            // 중섭목소리 재생
            audio_Me2.SetActive(true);

            letter.GetComponent<Rigidbody>().isKinematic = false;

            // 곧 편지 날아가기
            StartCoroutine("LetterLeave");
            midState = MidIslandState.m_Under;
        }

    }

    // 중간 섬 관리
    private void MiddleIslandCtrl()
    {
        switch (midState)
        {
            // 미들섬이 바다아래에 있는 상태
            case MidIslandState.m_Under: break;

            // 미들섬이 수면 위로 상승+회전 중
            case MidIslandState.m_UpNRotate:

                // 미들섬을 목표높이까지 상승시간만큼 이동 (74.2 -> 82.7)
                rotIsland.transform.localPosition = Vector3.MoveTowards(rotIsland.transform.localPosition, 
                    new Vector3(rotIsland.transform.localPosition.x, high_Midland, rotIsland.transform.localPosition.z), 
                    time_MidlandUp * Time.deltaTime);

                // 아이들섬이 앞으로 나올 때까지 회전
                rotIsland.transform.localRotation 
                    = Quaternion.Lerp(rotIsland.transform.localRotation, Quaternion.Euler(0, 0, 0), speed_MidIslandRot * Time.deltaTime);

                // 미들섬이 목표에 도달하면 상태바꾸기
                if (rotIsland.transform.eulerAngles.y >= 359f)
                {
                    midState = MidIslandState.m_End;
                }
                break;

            // 미들섬의 움직임이 모두 끝난 상태
            case MidIslandState.m_End:
                MidIslandEnd();
                break;
        }
    }

    // 미들 섬이 도착했을 때
    private void MidIslandEnd()
    {
        // 작은 섬 애니메이션 시작
        for (int i = 0; i < smallIsland.Length; i++)
        {
            smallIsland[i].GetComponent<Animator>().enabled = true;
        }

        // 아이들 애니메이션 시작
        children.GetComponent<Animator>().enabled = true;

        // 섬 애니메이션 시작
        anim_island.GetComponent<Animator>().enabled = true;

        fish1.SetActive(true);
    }

    // 첫번째 배경음악이 끝나면
    public void Next()
    {
        // 작은 섬 애니메이션 끝
        for (int i = 0; i < smallIsland.Length; i++)
        {
            smallIsland[i].GetComponent<Animator>().enabled = false;
        }


        // 배경음악 바꾸기 (Me1 + Wave)
        audio_Me1.SetActive(true);
        audio_Wave.SetActive(true);
        // 섬 애니메이션 끝
        anim_island.GetComponent<Animator>().enabled = false;

        // 아내와~ 목소리 끝난 후 편지 날아오기
        Invoke("LetterCome", time_BeforeLetterCome);
    }

    // 편지 날아오기
    public void LetterCome()
    {
        letter.SetActive(true);
        letterCome_BG.SetActive(true);

        // 바람 소리 켜기
        audio_Wind.SetActive(true);
    }

    // 편지 날아갈 때
    public IEnumerator LetterLeave()
    {
        yield return new WaitForSeconds(3);

        // 편지가 손에 있다면 바로
        if (letter.tag == "COMELETTERINHAND")
        {
            // 바람소리 켜기
            audio_Wind.SetActive(true);

            // 복숭아에게 AddForce
            GameObject[] peaches = GameObject.FindGameObjectsWithTag("PEACH");
            for (int i = 0; i < peaches.Length; i++)
            {
                peaches[i].GetComponent<Rigidbody>().isKinematic = true;
                peaches[i].GetComponent<Rigidbody>().AddForce(new Vector3(-100, 0, 100));
            }

            // 날아가는 편지 잡지 못하도록
            hands[0].GetComponent<OVRGrabber>().m_grabbedObj = null;
            hands[1].GetComponent<OVRGrabber>().m_grabbedObj = null;
            letter.GetComponent<BoxCollider>().enabled = false;
            letter.GetComponent<Rigidbody>().isKinematic = true;

            // 목표주고 이동시키기
            isFirstGoal = true;

            // 조금 후에 어어소리 켜기
            yield return new WaitForSeconds(1f);
            audio_UhUh.SetActive(true);
        }

        // 편지가 손에 있지 않다면
        else
        {
            // 3초 더 있다가
            yield return new WaitForSeconds(3f);

            // 바람소리 켜기
            audio_Wind.SetActive(true);

            // 복숭아에게 AddForce
            GameObject[] peaches = GameObject.FindGameObjectsWithTag("PEACH");
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("PEACH").Length; i++)
            {
                peaches[i].GetComponent<Rigidbody>().isKinematic = true;
                peaches[i].GetComponent<Rigidbody>().AddForce(new Vector3(-100, 0, 100));
            }

            // 목표주고 이동시키기
            letter.GetComponent<BoxCollider>().enabled = false;
            letter.GetComponent<Rigidbody>().isKinematic = true;
            isFirstGoal = true;

            // 조금 후에 어어소리 켜기
            yield return new WaitForSeconds(1f);
            audio_UhUh.SetActive(true);
        }

        // 3초 더 있다가
        yield return new WaitForSeconds(time_BeforeEnding);
        audio_Wind.SetActive(true);
        StartCoroutine(EndingCtrl());
    }

    // 씬 엔딩 관리
    public IEnumerator EndingCtrl()
    {
        yield return new WaitForSeconds(1);
        // 화면 어두워지기
        isEnding = true;

        yield return new WaitForSeconds(2);
        // 꽃비 효과음 실행
        audio_FlowerRain.SetActive(true);

        // 나뭇잎 , 꽃잎
        particle_Leaves[0].Play();
        particle_Flowers[0].Play();
        particle_FlowerEachs[0].Play();
        
        yield return new WaitForSeconds(4);

        // 나뭇잎 , 꽃잎 하나씩 더 추가
        particle_Leaves[1].Play();
        particle_Flowers[1].Play();
        particle_FlowerEachs[1].Play();

        yield return new WaitForSeconds(2);

        // 나뭇잎 , 꽃잎 하나씩 더 추가
        particle_Leaves[2].Play();
        particle_Flowers[2].Play();
        particle_FlowerEachs[2].Play();

        yield return new WaitForSeconds(4);

        // 눈 파티클 켜기
        particle_Snow[0].Play();
        particle_Snow[1].Play();

        yield return new WaitForSeconds(time_ChangeScene);
        // 페이트 아웃
        if (GameObject.FindWithTag("MainCamera"))
        {
            GameObject.FindWithTag("MainCamera").GetComponent<OVRScreenFade>().FadeOut();
        }

        yield return new WaitForSeconds(2); // 페이드아웃 걸리는 시간
        // 씬전환
        SceneManager.LoadScene(4);
    }
}
