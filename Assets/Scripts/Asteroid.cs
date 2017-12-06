using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    MeshRenderer mr;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    private void OnBecameInvisible()
    {
        mr.enabled = false;
    }

    private void OnBecameVisible()
    {
        mr.enabled = true;
    }

    // Update is called once per frame
    void Update () {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    }
}
