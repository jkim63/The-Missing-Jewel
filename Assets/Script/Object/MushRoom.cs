using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushRoom : MonoBehaviour {
    ///Private variable

    ///Public variable
    public Transform m_StartPos;

    public GameObject m_MushroomObj;

    ///Private method
    private void Start() {
        m_StartPos = MushroomManager.GetMushroomManager().GetStartPos().transform;

        transform.position = new Vector3(
            m_StartPos.position.x + Random.Range(50, 250),
            m_StartPos.position.y + Random.Range(100, 150),
            m_StartPos.position.z + Random.Range(50, 250));
    }
    ///Public method
}
