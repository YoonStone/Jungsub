using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YG_Dowan_Sea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 바다에 물건이 빠지면
        if(other.gameObject.tag == "GRABOBJECT" || other.gameObject.tag == "GRABPEACH")
        {
            other.gameObject.GetComponent<YG_Dowan_Object>().SeaSound();
        }
    }
}
