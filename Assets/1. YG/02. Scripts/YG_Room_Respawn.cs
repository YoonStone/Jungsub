using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YG_Room_Respawn : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 바닥에 닿은 물건의 태그가 편지("GRABLETTER3")이라면
        if (collision.gameObject.tag == "GRABLETTER3")
        {
            // 물건의 위치와 그 물건의 리스폰 위치를 비교 (다르다면)
            if (collision.gameObject.transform.position != GameObject.Find(collision.gameObject.name + "_respwan").transform.position)
            {
                StartCoroutine("RespawnCtrl", collision);
            }
        }
    }

    public IEnumerator RespawnCtrl(Collision collision)
    {
        yield return new WaitForSeconds(2);

        // 아직 그 물건과 리스폰 포지션이 다르다면 리스폰
        if (collision.gameObject.transform.position != GameObject.Find(collision.gameObject.name + "_respwan").transform.position)
        {
            collision.gameObject.transform.position = GameObject.Find(collision.gameObject.name + "_respwan").transform.position;

            // 리스폰 시 원래 떠있던 LETTER라면 다시 떠있도록
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            // 태그 바꾸기
            collision.gameObject.tag = "LETTER3";

            // 로테이션 초기화 0 180 180
            collision.gameObject.transform.rotation = Quaternion.Euler(0, 180, 180);
        }

    }
}
