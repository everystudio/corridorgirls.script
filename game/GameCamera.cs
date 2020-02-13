using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HedgehogTeam;
using UnityEngine.Events;

public class GameCamera : Singleton<GameCamera> {

	public Vector3 offset;
	public float scale;
	public CharaControl m_charaControl;
	public Camera m_Camera;

	public UnityEvent RequestMoveStop = new UnityEvent();
	public UnityEvent RequestMoveStart = new UnityEvent();

	[SerializeField]
	private HedgehogTeam.EasyTouch.QuickSwipe quickSwipe;
	private bool is_swiping;
	public void SwipeStart()
	{
		if (!is_swiping)
		{
			is_swiping = true;
			quickSwipe.onSwipeAction.AddListener(OnSwipe);
		}
	}
	public void SwipeEnd()
	{
		if (is_swiping)
		{
			is_swiping = false;
			quickSwipe.onSwipeAction.RemoveListener(OnSwipe);
		}
	}

	public void MoveCompleteHandler()
	{
	}
	public void ResetPosition()
	{
		iTween.MoveTo(m_Camera.gameObject,
				iTween.Hash(
					"x", offset.x,
					"y", offset.y,
					"z", offset.z,
					"time", 0.5f,
					"oncomplete", "MoveCompleteHandler",
					"oncompletetarget", gameObject,
					"isLocal", true)
					);

	}

	public void OnSwipe(HedgehogTeam.EasyTouch.Gesture _a)
	{
		//Debug.Log(string.Format("x={0} y={1}", _a.deltaPosition.x, _a.deltaPosition.y));
		m_Camera.transform.localPosition += new Vector3(_a.deltaPosition.x * -1.0f, _a.deltaPosition.y * -0.0f, 0.0f) * scale;

	}

}
