using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelResult : MonoBehaviour {

	public Button m_btn;

	public TextMeshProUGUI m_txtNumFood;
	public TextMeshProUGUI m_txtNumMana;
	public TextMeshProUGUI m_txtNumGem;

	private int m_iFood;
	private int m_iFoodDisp;
	private int m_iMana;
	private int m_iManaDisp;
	private int m_iGem;
	private int m_iGemDisp;

	public IEnumerator show( int _iFood , int _iMana , int _iGem)
	{
		m_btn.interactable = false;
		m_txtNumFood.text = "0";
		m_txtNumMana.text = "0";
		m_txtNumGem.text = "0";

		m_iFood = _iFood;
		m_iFoodDisp = 0;
		m_iMana = _iMana;
		m_iManaDisp = 0;
		m_iGem = _iGem;
		m_iGemDisp = 0;

		yield return new WaitForSeconds(1.0f);

		// 個別にカウントアップ


		while (m_iFoodDisp < m_iFood)
		{
			m_iFoodDisp += 1;
			m_txtNumFood.text = m_iFoodDisp.ToString();
			yield return new WaitForSeconds(0.01f);
		}

		if (0 < m_iFood)
		{
			yield return new WaitForSeconds(0.5f);
		}

		while (m_iManaDisp < m_iMana)
		{
			m_iManaDisp += 1;
			m_txtNumMana.text = m_iManaDisp.ToString();
			yield return new WaitForSeconds(0.01f);
		}

		if (0 < m_iMana)
		{
			yield return new WaitForSeconds(0.5f);
		}

		while (m_iGemDisp < m_iGem)
		{
			m_iGemDisp += 1;
			m_txtNumGem.text = m_iGemDisp.ToString();
			yield return new WaitForSeconds(0.01f);
		}

		if (0 < m_iGem)
		{
			yield return new WaitForSeconds(0.5f);
		}
		m_btn.interactable = true;

	}



}
