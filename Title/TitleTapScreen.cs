using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TitleTapScreen : MonoBehaviour {
	public UnityEvent OnTapScreenFinish = new UnityEvent();
	public Animator m_animator;
	public void HandlerTapScreenFinish()
	{
		OnTapScreenFinish.Invoke();
	}
}
