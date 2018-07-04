using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CharacterUI : MonoBehaviour {

    public static CharacterUI Instance;
    public Button exitToShip;

    public Text gelatoWarning;
    public GameObject gelatoGive;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gelatoWarning.gameObject.SetActive(false);
        exitToShip.gameObject.SetActive(false);
    }

    public void LeaveScene()
    {
        CharacterManager.Instance.character.enabled = false;

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "SpaceStation")
        {
            exitToShip.gameObject.SetActive(true);
        }
    }

    public void ChangeScene()
    {
        DataManager.Save(CharacterManager.Instance.pData);

        string _name = EventSystem.current.currentSelectedGameObject.name;
        SceneManager.LoadScene(_name);
    }

    public void GelatoRequestScreen()
    {
        CharacterManager.Instance.character.enabled = false;
        gelatoWarning.gameObject.SetActive(true);
        //gelatoGive.gameObject.SetActive(true);
    }
}
