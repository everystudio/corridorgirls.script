using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class CharaControl : MonoBehaviour {

	public enum STATUS
	{
		NONE		= 0,
		IDLE		,
		MOVE		,
		MAX			,
	}
	public STATUS m_eStatus;

	public UnityEventInt OnMoveRequest = new UnityEventInt();
	public UnityEvent OnMoveFinished = new UnityEvent();

	public OverrideSprite override_sprite;
	public Animator m_animator;

	public Button m_btnGo;
	public GameObject m_goArrowRoot;
	public GameObject m_prefArrowTargetCorridor;

	[HideInInspector]
	public DataCorridorParam target_corridor;

	void Start()
	{
		m_btnGo.gameObject.SetActive(false);
	}

	public void SetCorridor(DataCorridorParam _data)
	{
		target_corridor = _data;

		transform.localPosition = new Vector3(target_corridor.master.x, target_corridor.master.y + 0.85f, -1.0f);
	}

	public IEnumerator RequestMove( int _iPower , Action _onFinished )
	{
		while (m_eStatus != STATUS.IDLE)
		{
			yield return null;
		}

		OnMoveRequest.Invoke(_iPower);

		while(m_eStatus != STATUS.MOVE)
		{
			yield return null;
		}

		while (m_eStatus != STATUS.IDLE)
		{
			yield return null;
		}

		_onFinished.Invoke();
	}



	public void DebugMove3()
	{
		OnMoveRequest.Invoke(3);
	}

	public void ChangeCharaId(int _iCharaId)
	{
		override_sprite.overrideTexture = TextureManager.Instance.Get(string.Format(Defines.STR_FORMAT_CHARA_TEXTURE, _iCharaId));
	}



}
