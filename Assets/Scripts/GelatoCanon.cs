using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelatoCanon : MonoBehaviour {

    public static GelatoCanon Instance;

    public bool holding = false;
    GameObject currentCone;
    float holdTimer = 0;

    private void Start()
    {
        Instance = this;
    }

    void Update () {
		if(Input.GetButtonDown("Cannon") && GameManager.Instance.cones.Count > 0 && !holding)
        {
            PlaceCone();
            holdTimer = Time.timeSinceLevelLoad + 1;
        }

        if(Input.GetButtonUp("Cannon") && holding)
        {
            holding = false;
            Gelato g = currentCone.GetComponent<Gelato>();
            RaycastHit hit;

            if(Physics.SphereCast(transform.position, 50, transform.forward, out hit) && hit.transform.tag == "Bandito")
            {
                AudioManager.Instance.Whip();
                g.target = hit.transform;
            } else
            {
                g.dir = transform.forward;
            }

            g.launched = true;
        }

        if(holding && Input.GetButton("Cannon") && holdTimer > Time.timeSinceLevelLoad)
        {
            currentCone.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
            currentCone.transform.localPosition = new Vector3(0, 0, currentCone.transform.localScale.z);
        }
	}

    void PlaceCone()
    {
        currentCone = GameManager.Instance.cones.Dequeue();

        currentCone.transform.SetParent(transform);
        currentCone.transform.localPosition = Vector3.zero;

        Vector3 rot = new Vector3(Mathf.Clamp(currentCone.transform.localRotation.x * 100, -10, 10),
            Mathf.Clamp(currentCone.transform.localRotation.y * 100, -10, 10),
            Mathf.Clamp(currentCone.transform.localRotation.z * 100, -10, 10));

        currentCone.transform.localRotation = Quaternion.Euler(rot);

        holding = true;
    }
}
