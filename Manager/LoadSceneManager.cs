using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour {
    /// Private variable
    private static string _SceneName;

    private bool _CheckFin = false;

    private float _LoadSpeed = 0.5f;
    private float _LoadDelay = 0.5f;
    private float _CheckTime = 0.0f;

    /// Public variable
    public Image m_ProgressBar;

    /// Private Method
    private void Start() {
        StartCoroutine(StartLoad());
    }

    private IEnumerator StartLoad() {
        AsyncOperation op = SceneManager.LoadSceneAsync(_SceneName);

        op.allowSceneActivation = false;

        while (!_CheckFin) {
            float fill = m_ProgressBar.fillAmount + (_LoadSpeed * Time.deltaTime);

            if (op.progress >= 0.9f) {
                if (m_ProgressBar.fillAmount != 1.0f) {
                    if (fill >= 1.0f)
                        fill = 1.0f;
                    m_ProgressBar.fillAmount = fill;
                    _CheckTime = Time.time;
                } else {
                    if (Time.time - _CheckTime > _LoadDelay) {
                        op.allowSceneActivation = true;
                        _CheckFin = true;
                    }
                }
            } else {            // still loading
                if (fill >= op.progress)
                    fill = op.progress;
                m_ProgressBar.fillAmount = fill;
            }
            yield return null;
        }
    }
    /// Public Method
    public static void LoadScene (string sceneName) {
        _SceneName = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
}
