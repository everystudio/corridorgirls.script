using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundControl : MonoBehaviour {

	public GameObject m_goStageRoot;
	public GameObject m_goCharaControl;
	public GameObject m_goCamera;
	public Vector3 offset;

	public Material mat;

	void Update()
	{
		//Debug.Log(m_goCharaControl.transform.localPosition - m_goCamera.transform.localPosition + offset);
		Vector3 move = ( m_goCharaControl.transform.localPosition + m_goCamera.transform.localPosition + offset);
		float scroll = Mathf.Repeat(1.0f *( move.x / 10.0f), 1.0f);
		mat.SetTextureOffset("_MainTex", new Vector2(scroll, 0.0f));
	}

}
