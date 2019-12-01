using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using System;

public class CardHolder : Singleton<CardHolder> {

	public UnityEvent OnRequestFill = new UnityEvent();
	public UnityEvent OnDecide = new UnityEvent();
	public UnityEvent OnOther = new UnityEvent();

	public class DataCardEvent : UnityEvent<DataCardParam>{
	}
	public DataCardEvent OnSelectCard = new DataCardEvent();

	public UnityEvent OnSelect = new UnityEvent();

	public int hand_num;

	public override void Initialize()
	{
		// シーン内だけでしんぐるとんであればいいです
		base.Initialize();
		hand_num = 3;
	}

	public void Clear(Action _onFinished)
	{
		DeleteObjects<Card>(gameObject);
		_onFinished.Invoke();
	}

	/// <summary>
	/// 手札カードを補充する
	/// </summary>
	/// <param name="_onFinished">_on finished.</param>
	/// _onFinishedは、不足している（補充すべき枚数を）
	public void Fill(Action<int> _onFinished)
	{

	}

	public bool IsNeedFill(){
		if (hand_num <= 1) {
			return true;
		}
		return false;
	}


}















