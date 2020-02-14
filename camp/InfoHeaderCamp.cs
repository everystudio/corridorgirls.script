using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoHeaderCamp : MonoBehaviour {

	public TextMeshProUGUI m_txtFood;
	public TextMeshProUGUI m_txtMana;
	public TextMeshProUGUI m_txtGem;

	public int m_iFood;
	public int m_iFoodDisp;
	public int m_iMana;
	public int m_iManaDisp;
	public int m_iGem;
	public int m_iGemDisp;

	public void SetFood(int _iFood)
	{
		m_iFood = _iFood;
	}
	public void SetMana(int _iMana)
	{
		m_iMana = _iMana;
	}
	public void SetGem(int _iGem)
	{
		m_iGem = _iGem;
	}

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

	void Start()
	{
		Reset();
		NumUpdate();
	}

	public void Reset()
	{
		m_iFood = 0;
		m_iFoodDisp = 0;
		m_iMana = 0;
		m_iManaDisp = 0;
		m_iGem = 0;
		m_iGemDisp = 0;
	}

	public void NumUpdate()
	{
		m_txtFood.text = m_iFoodDisp.ToString();
		m_txtMana.text = m_iManaDisp.ToString();
		m_txtGem.text = m_iGemDisp.ToString();
	}
	private float timer;
	void Update()
	{
		timer += Time.deltaTime;
		if (0.05f < timer)
		{
			timer -= 0.05f;

			bool bChange = false;
			if (m_iFoodDisp < m_iFood)
			{
				m_iFoodDisp += 1;
				bChange = true;
			}
			else if( m_iFood < m_iFoodDisp)
			{
				m_iFoodDisp -= 1;
				bChange = true;
			}
			if (m_iGemDisp < m_iGem)
			{
				m_iGemDisp += 1;
				bChange = true;
			}
			else if( m_iGem < m_iGemDisp)
			{
				m_iGemDisp -= 1;
				bChange = true;
			}

			if (m_iManaDisp < m_iMana)
			{
				m_iManaDisp += 1;
				bChange = true;
			}
			else if( m_iMana < m_iManaDisp)
			{
				m_iManaDisp -= 1;
				bChange = true;
			}

			if (bChange)
			{
				NumUpdate();
			}
			else
			{
			}

		}

	}



}
