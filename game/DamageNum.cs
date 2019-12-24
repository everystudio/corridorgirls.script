using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;

public class DamageNum : MonoBehaviour {

	[SerializeField]
	private Animator m_animator;

	[SerializeField]
	private TextMeshPro m_txtNum;

	private UnityEvent FinishedHandler = new UnityEvent();

	public void anim_end()
	{
		FinishedHandler.Invoke();
	}

	public void Action(int _iDamage, UnityAction _action)
	{
		m_txtNum.text = _iDamage.ToString();
		m_animator.SetTrigger("appear");
		FinishedHandler.AddListener(_action);
	}

}
