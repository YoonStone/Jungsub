using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YG_Dowan_Sand : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 모래사장에 물건이 닿으면
        if (collision.gameObject.tag == "GRABOBJECT" || collision.gameObject.tag == "GRABPEACH")
        {
            collision.gameObject.GetComponent<YG_Dowan_Object>().SandSound();
        }
    }
}