using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingParent : MonoBehaviour {

    public GameObject bandit;
    public int amount = 100;
    public List<GameObject> flock = new List<GameObject>();
    public GameObject leader;
    public Material mat;

    bool active = true;

    void Start () {
        leader = Instantiate(bandit, transform);

        leader.transform.localPosition = Vector3.zero;

        leader.transform.localScale = new Vector3(10, 10, 10);
        leader.name = "Leader";
        leader.GetComponent<MeshRenderer>().material = mat;

        for (int i = 0; i < amount; i++)
        {
            Vector3 Pos = new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), Random.Range(25, 28));
            GameObject temp = Instantiate(bandit, transform);

            temp.transform.localPosition = Pos;
            flock.Add(temp);
        }
	}
	
	// Update is called once per frame
	public void RemoveLeader () {
        if (flock.Count > 0)
        {
            foreach (GameObject bandit in flock)
            {
                bandit.GetComponent<Flocking>().leaderDead = true;
            }
        }
    }
}
