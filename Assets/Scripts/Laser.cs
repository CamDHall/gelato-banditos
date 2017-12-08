using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    private void Start()
    {
        Destroy(gameObject, Time.deltaTime * 5);
    }
}
