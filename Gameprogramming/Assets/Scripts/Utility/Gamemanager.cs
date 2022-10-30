using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utility;
using Input = Playercontroller.Input.Input;

/// <summary>
/// Class to general manage the game
/// </summary>
public class Gamemanager : MonoBehaviour
{
    [SerializeField] private int targetFPS = 60;
    [SerializeField] private List<int> dontHideCursorSceneIndices;
    [SerializeField] private GameObject UI;

    private Input _input;

    /// <summary>
    /// set framerate, and destroy multiple gamemanager
    /// </summary>
    private void Awake()
    {
        Application.targetFrameRate = targetFPS;

        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");

        if (objs.Length > 1) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        _input = new Input();
        _input.Enable();
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _input.Player.Menu.performed += MenuOnperformed;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        _input.Player.Menu.performed -= MenuOnperformed;
    }

    /// <summary>
    /// change cursor visibility on certain scenes
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        int buildIndex = scene.buildIndex;
        if (dontHideCursorSceneIndices.Contains(buildIndex)) {
            CursorManager.ExecuteCursorEvent(CursorManager.CursorEvent.Visible);
        }
        else {
            CursorManager.ExecuteCursorEvent(CursorManager.CursorEvent.Invisible);
        }

        Time.timeScale = 1;
    }

    private void MenuOnperformed(InputAction.CallbackContext obj)
    {
        UI.SetActive(!UI.activeSelf);
        CursorManager.ExecuteCursorEvent(CursorManager.CursorEvent.Invisible);
    }
}
