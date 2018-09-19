using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoneMonster : MonoBehaviour {
    /// Private variable
    private float _HP = 6f;
    private float _Speed = 10.0f;
    private float _TimeLeft = 1.0f;

    private bool _HPActivate = false;
    private bool _DistCheck = false;

    private Image _HPBarImg;

    private Animation_Test _Anim;

    private Character _Character;

    /// Public variable
    public Transform _StartPos;

    public GameObject m_HPBar;
    public GameObject m_MonstObj;

    /// Private method
    private void Start() {
        _HPBarImg = m_HPBar.GetComponent<Image>();

        _Character = Character.GetCharacter();

        _StartPos = StoneMonsterManager.GetStoneMonsterManager().GetStartPos().transform;

        _Speed += Random.Range(3, 5);

        transform.position = new Vector3(
            _StartPos.position.x + Random.Range(10, 290),
            _StartPos.position.y + Random.Range(100, 150),
            _StartPos.position.z + Random.Range(10, 290));
    }

    private void Update()
    {
        transform.LookAt(_Character.transform);

        float Distance = Vector3.Distance(
            _Character.transform.position,
            m_MonstObj.transform.position);

        if (Distance < 15.0f)
        {
            //_Anim.AttackAni();
            Move();
        }
    }

    private void Move() {
        transform.Translate(Vector3.forward * _Speed * Time.deltaTime);
    }

    private void ShowHPBar() {
        if (!_HPActivate) return;
        m_HPBar.GetComponent<Image>().color += new Color(0, 0, 0, 1.0f);
    }

    /// Public method
    public void Damage() {
        _HPActivate = true;
        ShowHPBar();

        _HP -= 1;
        _HPBarImg.fillAmount = (1.0f / 6) * _HP;

        if (_HP == 0) {
            StoneMonsterManager.GetStoneMonsterManager().m_Score += 10;
            StoneMonsterManager.GetStoneMonsterManager().SetScore();
            Destroy(gameObject);
        }
    }
}