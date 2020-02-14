using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeScreen : MonoBehaviour {

	public bool is_open;
	public Animator m_animator;
	public Image m_img;
	public UnityEvent OnOpen = new UnityEvent();
	public UnityEvent OnClose = new UnityEvent();

	public void Open()
	{
		m_img.enabled = true;
		m_animator.SetBool("is_open", true);
	}
	public void Close()
	{
		m_img.enabled = true;
		m_animator.SetBool("is_open", false);
	}

	public void OnOpenFinished()
	{
		is_open = true;
		m_img.enabled = false;
		OnOpen.Invoke();
	}
	public void OnCloseFinished()
	{
		is_open = false;
		OnClose.Invoke();
	}

}
