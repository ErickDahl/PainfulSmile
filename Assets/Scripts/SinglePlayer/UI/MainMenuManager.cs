using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _options;

    [SerializeField]
    private GameObject _mainMenu;

    void Start()
    {
        _options.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void ActiveOptions()
    {
        _options.SetActive(true);
        _mainMenu.SetActive(false);
    }

    public void ActiveMainMenu()
    {
        _options.SetActive(false);
        _mainMenu.SetActive(true);
    }
}
