using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ChangeColor : MonoBehaviour {
    private float _Speed;
    private float _StartTime;
    private float _MaxInt = 1.1f;

    private Renderer rend;

    private bool m_MouseOver = false;

    public Material m_StartMat;
    public Material m_MouseOverMat;

    public Light m_PointLight;

    void Start() {
        rend = GetComponent<Renderer>();
        rend.material = m_StartMat;

        _StartTime = Time.time;

        m_PointLight.intensity = 0;
    }

    private void Update() {
        if (!m_MouseOver) {
            _Speed = 0.6f;

            float t = (Time.time - _StartTime) * _Speed;
            rend.material.Lerp(m_MouseOverMat, m_StartMat, t);

            if (m_PointLight.intensity >= 0) {
                m_PointLight.intensity -= 1 * Time.deltaTime;
            }
        }
    }

    private void OnMouseOver() {
        m_MouseOver = true;
        _Speed = 0.1f;

        //float lerp = Mathf.PingPong(Time.time, duration) / duration;
        float t = (Time.time - _StartTime) * _Speed;
        rend.material.Lerp(m_StartMat, m_MouseOverMat, t);

        if (m_PointLight.intensity <= _MaxInt) {
            m_PointLight.intensity += 1 * Time.deltaTime;
        }
    }

    private void OnMouseExit() {
        m_MouseOver = false;
    }

    private void OnMouseDown() {
        if (gameObject.name == "OrangeLight")
            LoadSceneManager.LoadScene("GameScene");
        else if (gameObject.name == "BlueLight")
            //Application.Quit();
            EditorApplication.isPlaying = false;
    }
}
