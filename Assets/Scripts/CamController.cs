using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {

    Quaternion og_Rot;

    private void Start()
    {
        og_Rot = transform.localRotation;
        Debug.Log(og_Rot);
    }

    private void LateUpdate()
    {        
        /*
        if(PlayerMovement.player.pitch == 0)
        {
            newRot.x = PlayerMovement.player.transform.rotation.x + og_Rot.x;
        }

        if(PlayerMovement.player.yaw == 0)
        {
            newRot.y = PlayerMovement.player.transform.rotation.y;
        }

        if(PlayerMovement.player.roll == 0)
        {
            newRot.z = PlayerMovement.player.transform.rotation.z;
        }*/
        transform.localRotation = Quaternion.Slerp(transform.localRotation, PlayerMovement.player.transform.rotation, 0.5f);
    }
}
