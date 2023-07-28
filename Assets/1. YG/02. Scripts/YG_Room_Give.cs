using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Curve;

public class YG_Room_Give : MonoBehaviour
{
    public GameObject[] letter_GoalPos, hands;

    [HideInInspector]
    public int flyState;

    GameObject letter;


    // 도원그림이 다른 물체와 닿았을 때 호출됨
    private void OnTriggerEnter(Collider other)
    {
        // 만약 닿은 물건의 태그가 잡고있는 편지("GRABLETTER3")라면 + 손에서 놔진 상태라면
        if(other.gameObject.tag == "GRABLETTER3")
        {
            // 편지 날아감
            flyState = 1;

            // 편지 오브젝트 저장
            letter = other.gameObject;

            // 날아가는 편지 잡지 못하도록
            hands[0].GetComponent<OVRGrabber>().m_grabbedObj = null;
            hands[1].GetComponent<OVRGrabber>().m_grabbedObj = null;

            letter.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void Update()
    {
        switch (flyState)
        {
            // 아무것도 아님
            case 0:
                break;

            // 첫번째 목표
            case 1:
                // 물리작용 끄기
                letter.GetComponent<Rigidbody>().isKinematic = true;
                letter.transform.position = Vector3.Lerp(letter.transform.position, letter_GoalPos[0].transform.position,
                                    (1 / (letter_GoalPos[0].transform.position.y - letter.transform.position.y) * 0.1f * Time.deltaTime));
                letter.transform.rotation = Quaternion.Slerp(letter.transform.rotation, Quaternion.Euler(200, -30, 20),
                                    (1 / (letter_GoalPos[0].transform.position.y - letter.transform.position.y) * 0.1f * Time.deltaTime));

                // 목표에 가까워지면 목표 바꾸기
                if (letter.transform.position.y >= letter_GoalPos[0].transform.position.y - 0.1f)
                {
                    flyState = 2;
                }
                break;

            // 두번째 목표
            case 2:
                letter.transform.position = Vector3.Lerp(letter.transform.position, letter_GoalPos[1].transform.position,
                                    (1 / (letter_GoalPos[1].transform.position.y - letter.transform.position.y) * 0.1f * Time.deltaTime));
                letter.transform.rotation = Quaternion.Slerp(letter.transform.rotation, Quaternion.Euler(215, 45, -50),
                                    (1 / (letter_GoalPos[1].transform.position.y - letter.transform.position.y) * 0.1f * Time.deltaTime));

                // 목표에 가까워지면 목표 바꾸기
                if (letter.transform.position.y >= letter_GoalPos[1].transform.position.y - 0.1f)
                {
                    flyState = 3;
                }
                break;

            // 최종 목표
            case 3:
                letter.transform.position = Vector3.Lerp(letter.transform.position, letter_GoalPos[2].transform.position,
                                    (1 / (letter_GoalPos[2].transform.position.y - letter.transform.position.y) * 0.1f * Time.deltaTime));
                letter.transform.rotation = Quaternion.Slerp(letter.transform.rotation, Quaternion.Euler(260, -132, 96),
                                    (1 / (letter_GoalPos[2].transform.position.y - letter.transform.position.y) * 0.1f * Time.deltaTime));
                letter.GetComponent<Rigidbody>().isKinematic = false;
                letter.GetComponent<BoxCollider>().enabled = true;
                letter.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                break;

        }
    }
}