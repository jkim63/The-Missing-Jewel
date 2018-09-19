using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCamera : MonoBehaviour {
    /// Private Variable
    private GameManager _GameManager;

    private Character _Character;

    private Camera _MainCam;

    private float _FollowSpeed = 30.0f;
    public float _ZoomValue = 5.0f;
    private float _ZoomTarget = 5.0f;
    private float _ZoomMax = 15.0f;
    private float _ZoomMin = 3.0f;
    private float _ZoomSpeed = 10.0f;
    private float _MouseSensitivity = 1.0f;

    private float _RotValX = 0.0f;
    private float _RotValY = 0.0f;

    private bool _MouseReverse = false;
    private bool _LockRot = false;

    private Quaternion _PlayerRot;

    /// Public Variable
    public Quaternion m_CamRot;
    public GameObject m_Player;
    public GameObject m_CamPos;

    /// Private Method
    private void Start() {
        _GameManager = GameManager.GetGameManager();
        _GameManager.SetCharacterCamera(this);
        _Character = Character.GetCharacter();
        _MainCam = Camera.main;

        m_CamRot = transform.rotation;
        transform.position = m_CamPos.transform.position;
    }

    private void FixedUpdate() {
        CameraMove();
        KeyInputCheck();
        ZoomUpdate();
        CameraRotate();
    }

    private void CameraMove() {
        Transform camTransform = _MainCam.transform;
        Transform targetTransform = _Character.m_CamTarget;

        float camPosX = camTransform.position.x;
        float camPosY = camTransform.position.y;
        float camPosZ = camTransform.position.z;

        Vector3 camMove = new Vector3(
            Mathf.Lerp(camPosX, targetTransform.position.x, Time.deltaTime * _FollowSpeed),
            Mathf.Lerp(camPosY, targetTransform.position.y, Time.deltaTime * _FollowSpeed),
            Mathf.Lerp(camPosZ, targetTransform.position.z, Time.deltaTime * _FollowSpeed));

        _MainCam.transform.position = camMove;
    }

    private void KeyInputCheck() {
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            _GameManager.SetCursorLockState(!_GameManager.GetCursorLockState());
        }

        if (Input.GetAxis("Mouse ScrollWheel") == 0.0f) return;

        // Wheel UP
        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            if (_ZoomTarget > _ZoomMin)
                _ZoomTarget -= 1.0f;
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0) {    // Wheel DOWN
            if (_ZoomTarget < _ZoomMax)
                _ZoomTarget += 1.0f;
        }
    }

    private void ZoomUpdate() {
        if (_ZoomValue != _ZoomTarget)
            _ZoomValue = Mathf.Lerp(_ZoomValue, _ZoomTarget, _ZoomSpeed);

        _Character.m_CamTarget.transform.localPosition = new Vector3(
            _Character.m_CamTarget.transform.localPosition.x,
            _Character.m_CamTarget.transform.localPosition.y,
             - (_ZoomValue / 2));
    }


    private void CameraRotate() {
        _RotValX += Input.GetAxis("Mouse X") * ((_MouseReverse) ? -_MouseSensitivity : _MouseSensitivity);
        _RotValY += Input.GetAxis("Mouse Y") * ((_MouseReverse) ? -_MouseSensitivity : _MouseSensitivity);

        _MainCam.transform.localRotation = Quaternion.Euler(-_RotValY, _RotValX, 0);

        _Character.SetCharRotateY(_MainCam.transform.eulerAngles.y);

        /*if (Input.GetMouseButton(2)) {
            float rotX = Input.GetAxis("Mouse X");
            float rotY = -Input.GetAxis("Mouse Y");

            float camRotX = transform.eulerAngles.y + rotX;
            float camRotY = transform.eulerAngles.x + rotY;

            transform.rotation = Quaternion.Euler(camRotY, camRotX, 0);
        }
        else {
            if (!_LockRot) _LockRot = true;
            m_CamRot = transform.rotation;
        }*/

        // 캐릭터의 보는 방향으로 고정한다면
        /*if (_LockRot) {

            // 카메라의 회전값이 캐릭터 회전값과 동일하지 않다면
            if (m_CamRot != _Character.m_CharRot)
                // 카메라의 방향을 캐릭터 방향으로 돌립니다.
                m_CamRot = Quaternion.Lerp(m_CamRot, _Character.m_CharRot, 10.0f * Time.deltaTime);

            // 카메라의 회전값을 설정합니다.
            transform.rotation = m_CamRot;

        }

        // 고정상태가 아니라면
        else {
            float rotX = Input.GetAxis("Mouse X");
            float rotY = -Input.GetAxis("Mouse Y");

            float camRotX = transform.eulerAngles.y + rotX;
            float camRotY = transform.eulerAngles.x + rotY;

            transform.rotation = Quaternion.Euler(camRotY, camRotX, 0);



        }*/
    }

    /// Public Method
}
