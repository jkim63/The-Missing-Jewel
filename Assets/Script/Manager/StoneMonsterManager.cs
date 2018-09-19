using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoneMonsterManager : MonoBehaviour {
    ///Private variable
    private static StoneMonsterManager _StoneMonsterManager = null;

    private GameManager _GameManager;

    private float _MonsterCreateDelay = 10f;
    private float _MonsterCheckTime = 0.0f;

    ///Public variable
    public GameObject m_StoneMonster;
    public GameObject m_StartPos;

    public Text m_ScoreText;

    public int m_Score = 0;

    ///Private method
    private void Start() {
        _GameManager = GameManager.GetGameManager();

        for (int i = 0; i < 30;i++){
            CreateMonster();
        }
    }

    private void FixedUpdate() {
        if (_GameManager.GetGameStart()) {
            if (Time.time - _MonsterCheckTime > _MonsterCreateDelay) {
                _MonsterCheckTime = Time.time;
                CreateMonster();
            }
        }
    }

    private void CreateMonster() {
        GameObject temp = Instantiate(m_StoneMonster) as GameObject;

        temp.transform.position = m_StartPos.transform.position;

        int randScale = Random.Range(1, 4);
        temp.transform.localScale = new Vector3(randScale, randScale, randScale);
    } 

    ///Public method
    public static StoneMonsterManager GetStoneMonsterManager() {
        if (_StoneMonsterManager == null)
            _StoneMonsterManager = FindObjectOfType(typeof(StoneMonsterManager)) as StoneMonsterManager;
        return _StoneMonsterManager;
    }

    public GameObject GetStartPos() {
        return gameObject;
    }

    public void SetScore() {
        m_ScoreText.text = m_Score.ToString();
    }
}
