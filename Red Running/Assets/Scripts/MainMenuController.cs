using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject levels;

    [SerializeField]
    GameObject level1, level2, level3;

    [SerializeField]
    GameObject locks;

    [SerializeField]
    GameObject lock1, lock2, lock3;

    void Start()
    {
        for(int i = 0; i < levels.transform.childCount; i++)
        {
            levels.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < locks.transform.childCount; i++)
        {
            locks.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < PlayerPrefs.GetInt("levelId"); i++)
        {
            levels.transform.GetChild(i).GetComponent<Button>().interactable = true;
        }

        //PlayerPrefs.DeleteAll();
    }

    public void ChoiceButton(int buttonId)
    {
        if (buttonId == 1)
        {
            if (PlayerPrefs.GetInt("levelId") == 0)
            {
                SceneManager.LoadScene(1);
            }

            else
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("levelId"));
            }
        }

        else if (buttonId == 2)
        {
            for (int i = 0; i < locks.transform.childCount; i++)
            {
                locks.transform.GetChild(i).gameObject.SetActive(true);
            }

            for (int i = 0; i < levels.transform.childCount; i++)
            {
                levels.transform.GetChild(i).gameObject.SetActive(true);
            }

            for (int i = 0; i < PlayerPrefs.GetInt("levelId"); i++)
            {
                locks.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        else if (buttonId == 3)
        {
            Application.Quit();
        }
    }

    public void LevelsButton(int levelId)
    {
        SceneManager.LoadScene(levelId);
    }
}
