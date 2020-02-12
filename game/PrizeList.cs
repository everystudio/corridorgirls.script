using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PrizeList : Singleton<PrizeList> {

	public bool m_bShow;
	public bool m_bMenu;

	public TextMeshProUGUI m_txtFood;
	public TextMeshProUGUI m_txtMana;
	public TextMeshProUGUI m_txtGem;
	public TextMeshProUGUI m_txtGold;

	public int m_iFood;
	public int m_iFoodDisp;
	public int m_iMana;
	public int m_iManaDisp;
	public int m_iGem;
	public int m_iGemDisp;
	public int m_iGold;
	public int m_iGoldDisp;

	public void AddFood(int _iAdd)
	{
		m_iFood += _iAdd;
	}
	public void AddMana(int _iAdd)
	{
		m_iMana += _iAdd;
	}
	public void AddGem(int _iAdd)
	{
		m_iGem += _iAdd;
	}
	public void AddGold(int _iAdd)
	{
		m_iGold += _iAdd;
	}

	public UnityEvent OnCompleteShow = new UnityEvent();
	public UnityEvent OnCompleteDisable = new UnityEvent();

	public void MoveCompleteHandlerShow()
	{
		m_bShow = true;
		OnCompleteShow.Invoke();
	}

	public void MoveCompleteHandlerDisable()
	{
		m_bShow = false;
		OnCompleteDisable.Invoke();
	}

	public void Reset()
	{
		m_iFood = 0;
		m_iFoodDisp = 0;
		m_iMana = 0;
		m_iManaDisp = 0;
		m_iGem = 0;
		m_iGemDisp = 0;
		m_iGold = 0;
		m_iGoldDisp = 0;
	}

	public void NumUpdate()
	{
		m_txtFood.text = m_iFoodDisp.ToString();
		m_txtMana.text = m_iManaDisp.ToString();
		m_txtGem.text = m_iGemDisp.ToString();
		m_txtGold.text = m_iGoldDisp.ToString();
	}

	public void Show()
	{
		iTween.MoveTo(gameObject,
				iTween.Hash(
					"x", 0,
					"y", -70,
					"z", 0,
					"time", 0.5f,
					"oncomplete", "MoveCompleteHandlerShow",
					"oncompletetarget", gameObject,
					"isLocal", true)
					);

	}
	public void Disable()
	{
		iTween.MoveTo(gameObject,
				iTween.Hash(
					"x", 0,
					"y", -10,
					"z", 0,
					"time", 0.5f,
					"oncomplete", "MoveCompleteHandlerDisable",
					"oncompletetarget", gameObject,
					"isLocal", true)
					);

	}


}
