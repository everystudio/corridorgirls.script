using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimCharaReciver : MonoBehaviour {

	public UnityEvent OnDeadFinished = new UnityEvent();

	public void DeadFinishedHandler()
	{
		//Debug.Log("DeadFinishedHandler");
		OnDeadFinished.Invoke();
	}

}
