using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public static CameraManager Instance;
    public Camera cam;
    public GameObject mainCanvas, cinematicCanvas;
    GameObject[] bandits;

    bool playGuardScene = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start () {
        cinematicCanvas.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(playGuardScene)
        {
            foreach(GameObject bandit in bandits)
            {
                if (Vector3.Distance(cam.transform.position, bandit.transform.position) >= 15)
                {
                    Vector3 newPos = (cam.transform.position - bandit.transform.position) * .10f;

                    bandit.transform.position += newPos;
                }
            }
        }
	}

    public void SpaceStationGuardScene(GameObject station)
    {
        cam.gameObject.SetActive(true);
        mainCanvas.SetActive(false);
        cinematicCanvas.SetActive(true);

        PlayerMovement.player.enabled = false;
        GunController.Instance.enabled = false;

        cam.transform.position = (station.transform.position + PlayerMovement.player.transform.position) / 2;
        cam.transform.LookAt(station.transform.position);

        bandits = new GameObject[2];

        bandits[0] = Instantiate(Resources.Load("JuarezGangBandit") as GameObject, station.transform.parent);
        bandits[1] = Instantiate(Resources.Load("JuarezGangBandit") as GameObject, station.transform.parent);

        bandits[0].transform.localPosition = Vector3.zero + (bandits[0].transform.right * 200);
        bandits[1].transform.localPosition = Vector3.zero + (bandits[1].transform.right * -200);

        bandits[0].transform.LookAt(cam.transform.position);
        bandits[1].transform.LookAt(cam.transform.position);

        playGuardScene = true;
    }

    public void Reset()
    {
        cam.gameObject.SetActive(false);
        mainCanvas.SetActive(true);
        cinematicCanvas.SetActive(false);

        playGuardScene = false;

        for(int i = 0; i < bandits.Length; i++)
        {
            Destroy(bandits[i]);
        }

        StationController sc = GameManager.Instance.nearestStation.GetComponent<StationController>();
        sc.cutScene = false;
        sc.aiActive = false;
        PlayerMovement.player.enabled = true;
        PlayerMovement.player.acceleration = 0;
        GunController.Instance.enabled = true;
    }
}
