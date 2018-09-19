using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    /// Private Variable
    private static Character _Character = null;

    private GameObject _ParentObj;

    private Animator _Animator;
    //private Animator _DoorAnim;

    private Soldier _Soldier;

    private Actions _Actions;

    private CharacterCamera _CharacterCam;

    private float _Speed = 2.5f;
    private float _InputVertical = 0.0f;
    private float _RotateY = 0.0f;
    private float _HitForce = 80f;
    private float _HitDelay = 1.0f;
    private float _HitCheckTime = 0.0f;

    private bool _IsRunning = false;
    private bool _IsAiming = false;
    private bool _CanHit = true;

    private LayerMask _ShootLayer;

    /// Public Variable
    public GameObject m_CharacterObj;
    public GameObject m_HitEffect;
    public GameObject m_HPObj;
    public GameObject[] m_HPBar;

    public Transform m_CamTarget;

    public Quaternion m_CharRot;

    public Camera m_Camera;

    public ParticleSystem m_MuzzleFlash;

    public float m_damage = 1.0f;
    public float m_range = 100.0f;

    public int m_HPValue;

    /// Private Method
    private void Start() {
        _Animator = m_CharacterObj.GetComponent<Animator>();
        _Actions = m_CharacterObj.GetComponent<Actions>();

        _ParentObj = gameObject;

        m_CharRot = transform.rotation;

        //_DoorAnim = m_Door.GetComponent<Animator>();

        _ShootLayer = LayerMask.NameToLayer("Everything");

        GameManager.GetGameManager().SetGameStart(true);

        m_HPValue = m_HPBar.Length - 1;
    }

    private void FixedUpdate() {
        KeyInputCheck();
        CharacterAction();
        RotateCharacter();

        if (_IsAiming) {
            _Character.m_CamTarget.transform.localPosition = new Vector3(
            _Character.m_CamTarget.transform.localPosition.x,
            _Character.m_CamTarget.transform.localPosition.y,
             -(3 / 2));
        }

        if (_CanHit)
            _HitCheckTime = Time.time;
        else {
            if (Time.time - _HitCheckTime > _HitDelay) {
                _HitCheckTime = Time.time;
                _CanHit = true;
            }
        }

        if (m_HPValue == 0) {
            
        }
            
    }
    
    private void SetHP() {
        for (int i = m_HPBar.Length - 1; i >= 0; i--) {
            if (i < m_HPValue) break;
            if (m_HPBar[i].active)
                m_HPBar[i].SetActive(false);
        }
    }

    private void AddHP() {
        for (int i = 0; i <= m_HPBar.Length - 1; i++) {
            if (i >= m_HPValue) break;
            if (!m_HPBar[i].active)
                m_HPBar[i].SetActive(true);
        }
    }

    private void KeyInputCheck() {
        _InputVertical = Input.GetAxis("Vertical");         //Forward & Backward

        if (Input.GetKey(KeyCode.LeftShift)) {
            if (!_IsRunning) _IsRunning = true;
            _Actions.Run();
        }
        // 눌리지 않은 상태라면
        else {
            if (_IsRunning) _IsRunning = false;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space)) {
            //_Actions.Jump();
            if (_IsRunning) {
                _Animator.SetBool("RunJump", true);
                _Animator.SetTrigger("Jump");
            } else {
                _Actions.Jump();
            }

        } else {
            _Animator.SetBool("RunJump", false);
        }

        // Aim

        if (Input.GetMouseButtonDown(0)) {

            if (!_IsAiming)  {
                Debug.Log("Aiming");
                _IsAiming = true;
                _Animator.SetBool("Aim", true);
            }
            else if (_IsAiming) {
                _Animator.SetTrigger("Attack");
                Shoot();
            }
        }  

        if (Input.GetKeyDown(KeyCode.E)) {
            _Animator.SetBool("Aim", false);
            _IsAiming = false;
        }


        // Crouch
        if (Input.GetKeyDown(KeyCode.C)) {
            _Actions.Sitting();
        }

        // Cursor Lock
        if (Input.GetMouseButton(1)) {
            GameManager.GetGameManager().SetCursorLockState(!GameManager.GetGameManager().GetCursorLockState());
        }
    }
    private void Shoot() {
        m_MuzzleFlash.Play();

        RaycastHit Hit;
        if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out Hit, m_range, _ShootLayer)) {
            Debug.Log(Hit.transform.name);
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white

            Monster monster = Hit.transform.GetComponent<Monster>();
            StoneMonster stoneMonster = Hit.transform.GetComponent<StoneMonster>();

            if (monster != null) {
                monster.Damage();
            }

            if (stoneMonster != null) {
                stoneMonster.Damage();
            }

            if (Hit.transform.tag == "Door") {
                //m_ShootLayer = "Door";

                Animator animator = Hit.transform.GetComponent<Animator>();

                animator.SetBool("character_nearby", true);
            }

            if (Hit.transform.tag == "Gem") {
                LoadSceneManager.LoadScene("MainScene");
            }

            if (Hit.rigidbody != null) {
                Hit.rigidbody.AddForce(-Hit.normal * _HitForce);
            }

            GameObject HitGO = Instantiate(m_HitEffect, Hit.point, Quaternion.LookRotation(Hit.normal));
            Destroy(HitGO, 2f);
        }
    }

    private void CharacterAction()
    {            // Walk
        if (_InputVertical > 0)
        {
            if (_IsAiming) return;

            if (!_IsRunning)
                _Actions.Walk();

            // if RUNNING
            if (_IsRunning)
            {
                _ParentObj.transform.Translate(Vector3.forward * Time.deltaTime * _InputVertical * (_Speed * 4));
            }
            else
            {
                _ParentObj.transform.Translate(Vector3.forward * Time.deltaTime * _InputVertical * _Speed);
            }
            //_Actions.Walk();
        }
        else if (_InputVertical < 0)
        {
            if (_IsAiming) return;

            if (_Animator.GetFloat("Speed") != 0.5f)
                _Actions.WalkBack();

            _ParentObj.transform.Translate(Vector3.back * Time.deltaTime * _InputVertical * -_Speed);
        }
        else
        {
            _Actions.Stay();
        }
    }

    private void RotateCharacter() {
        transform.rotation = Quaternion.Euler(0, _RotateY, 0);
    }

    /// Public Method
    public static Character GetCharacter() {
        if (_Character == null) {
            _Character = FindObjectOfType(typeof(Character)) as Character;
        }
        return _Character;
    }

    public float GetCharRotateY() { return _RotateY; }
    public void SetCharRotateY(float rotateY) { _RotateY = rotateY; }

    public Soldier GetSoldier() { return _Soldier; }
    public void SetSoldier(Soldier soldier) { _Soldier = soldier; }

    public GameObject GetParentObj() { return _ParentObj; }

    public void OnCollisionEnter(Collision col)
    {
        Debug.Log("collision!!");
        if (col.transform.tag.Equals("Monster") && _CanHit) {
            if (m_HPValue != 0)
                m_HPValue--;

            SetHP();

            _CanHit = false;
        } else if (col.transform.tag.Equals("House")) {
            LoadSceneManager.LoadScene("FinalScene");
        } else if (col.transform.tag.Equals("Mushroom")) {
            if (m_HPValue < m_HPBar.Length - 1)
                m_HPValue++;

            AddHP();
            Destroy(col.gameObject);
        }
    }
}
