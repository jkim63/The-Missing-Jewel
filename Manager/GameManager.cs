using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    /// Private Variable
    private static GameManager _GameManager = null;

    private CharacterCamera _CharacterCam = null;

    private bool _GameStart = false;
    private bool _CursorLockState = true;

    private float _GravityY = -100.0f;

	/// Public Variable

	/// Private Method
    private void Awake() {
        DontDestroyOnLoad(gameObject);
        //SetGravity();
    }

    private void SetGravity() {
        Physics.gravity = new Vector3(0, _GravityY, 0);
    }

	/// Public Method
    public static GameManager GetGameManager() {
        if (_GameManager == null)
            _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

        return _GameManager;
    }

    public bool GetGameStart() { return _GameStart; }
    public void SetGameStart(bool start) { _GameStart = start; }

    public CharacterCamera GetCharacterCamera() { return _CharacterCam; }
    public void SetCharacterCamera(CharacterCamera characterCamera) { _CharacterCam = characterCamera; }

    public bool GetCursorLockState() { return _CursorLockState; }
    public void SetCursorLockState(bool state) {
        _CursorLockState = state;
        switch (_CursorLockState) {
            case true:
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case false:
                Cursor.lockState = CursorLockMode.None; break;
        }
    }
}
