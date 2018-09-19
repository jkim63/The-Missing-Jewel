using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour {
    /// Private variable
    private float _HP = 30f;
    private float _Speed = 5f;
    private float _TimeLeft = 2.0f;
    private float _ReadyDist = 40.0f;
    private float _AttackDist = 30.0f;

    private bool _HPActivate = false;
    private bool battle_state;

    private Image _HPBarImg;

    private Animator _Anim;

    private Character _Character;

    /// Public variable
    public GameObject m_HPBar;
    public GameObject m_MonstObj;

    public float runSpeed = 1.7f;
    public float turnSpeed = 60.0f;

    /// Private method
    private void Start()
    {
        _Character = Character.GetCharacter();
        _HPBarImg = m_HPBar.GetComponent<Image>();
        _Anim = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);

        float Distance = Vector3.Distance(
            _Character.transform.position,
            m_MonstObj.transform.position);

        if (Distance > _AttackDist && Distance <= _ReadyDist) {
            // Battle Idle
            _Anim.SetInteger("battle", 1);
            battle_state = true;

        } else if (Distance <= _AttackDist) {
            int[] randomLevels = new int[] { 3, 4, 5, 8 };
            _Anim.SetInteger("moving", randomLevels[Random.Range(0, randomLevels.Length)]);
            Attack();
        }
    }

    private void Attack() {
        transform.Translate(Vector3.forward * _Speed * Time.deltaTime);
    }

    private void ShowHPBar()
    {
        if (!_HPActivate) return;
        m_HPBar.GetComponent<Image>().color += new Color(0, 0, 0, 1.0f);
    }

    /// Public method
    public void Damage()
    {
        _HPActivate = true;
        ShowHPBar();

        _HP -= 1;
        _HPBarImg.fillAmount = (1.0f / 30) * _HP;

        if (_HP == 0)
        {
            StoneMonsterManager.GetStoneMonsterManager().m_Score += 30;
            StoneMonsterManager.GetStoneMonsterManager().SetScore();
            Destroy(gameObject);
        }
    }
}
