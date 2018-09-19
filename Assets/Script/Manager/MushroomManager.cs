using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MushroomManager : MonoBehaviour {
    ///Private variable
    private static MushroomManager _MushroomManager = null;

    private GameManager _GameManager;

    private float _MushRoomCreateDelay = 3f;
    private float _MushRoomCheckTime = 0.0f;

    ///Public variable
    public GameObject m_MushRoom;
    public GameObject m_StartPos;

    ///Private method
    private void Start() {
        _GameManager = GameManager.GetGameManager();

        for (int i = 0; i < 20; i++) {
            CreateMushRoom();
        }
    }

    private void FixedUpdate() {
        if (_GameManager.GetGameStart()) {
            if (Time.time - _MushRoomCheckTime > _MushRoomCreateDelay) {
                _MushRoomCheckTime = Time.time;
                CreateMushRoom();
            }
        }
    }

    private void CreateMushRoom() {
        GameObject temp = Instantiate(m_MushRoom) as GameObject;

        temp.transform.position = m_StartPos.transform.position;

        int randScale = Random.Range(2, 5);
        temp.transform.localScale = new Vector3(randScale, randScale, randScale);
    }

    ///Public method
    public static MushroomManager GetMushroomManager() {
        if (_MushroomManager == null)
            _MushroomManager = FindObjectOfType(typeof(MushroomManager)) as MushroomManager;
        return _MushroomManager;
    }

    public GameObject GetStartPos() {
        return gameObject;
    }
}
