using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelGameMenu : MonoBehaviour {

	public BannerMenu m_bannerGamespeed;
	public BannerMenu m_bannerAging;
	public BannerMenu m_bannerRetire;

	public GameObject m_goRootMenuNum;
	public GameObject m_prefMenuNum;


	public GameObject m_goRetireAttention;

	public void Show()
	{
		bool bDevelopment = Application.identifier.Contains("development");
#if UNITY_EDITOR
		bDevelopment = true;
#endif
		m_bannerAging.gameObject.SetActive(bDevelopment);



		m_goRetireAttention.SetActive(false);


		m_prefMenuNum.SetActive(false);
		MenuNum[] arr = m_goRootMenuNum.GetComponentsInChildren<MenuNum>();
		foreach (MenuNum c in arr)
		{
			if (m_prefMenuNum != c.gameObject)
			{
				GameObject.Destroy(c.gameObject);
			}
		}
		AddMenuNum("Wave", string.Format("{0}/{1}", GameMain.Instance.now_wave, GameMain.Instance.total_wave));
		AddMenuNum("ターン数", GameMain.Instance.total_turn.ToString());
		AddMenuNum("カードプレイ数", GameMain.Instance.m_iCountCardPlay.ToString());
		AddMenuNum("デッキリロード数", GameMain.Instance.m_iCountDeck.ToString());

		_gamespeed_label();
	}

	private void AddMenuNum(string _strTitle , string _strParam)
	{
		MenuNum script = PrefabManager.Instance.MakeScript<MenuNum>(m_prefMenuNum, m_goRootMenuNum);
		script.Initialize(_strTitle, _strParam);

	}

	private void _gamespeed_label()
	{
		string star = "";
		for (int i = 0; i < 4; i++)
		{
			if (i <= DataManagerGame.Instance.GameSpeedIndex)
			{
				star += "★";
			}
			else
			{
				star += "☆";
			}
		}
		m_bannerGamespeed.m_txtLabel.text = string.Format("ゲームスピード変更：{0}", star);

	}

	void Start()
	{
		m_bannerAging.m_bToggle = DataManagerGame.Instance.IsAging;
		ToggleAgingMenu(m_bannerAging.m_bToggle);


		m_bannerAging.m_btn.onClick.AddListener(() =>
		{
			ToggleAgingMenu(!m_bannerAging.m_bToggle);
		});


		m_bannerGamespeed.m_btn.onClick.AddListener(() =>
		{
			DataManagerGame.Instance.ChangeGameSpeed();
			_gamespeed_label();
		});

	}

	private void ToggleAgingMenu(bool _bFlag)
	{
		m_bannerAging.m_bToggle = _bFlag;
		if (m_bannerAging.m_bToggle)
		{
			m_bannerAging.m_txtLabel.text = "エージングモード：ON";
			DataManagerGame.Instance.config.Write(Defines.KEY_AGING, "camp");
		}
		else
		{
			m_bannerAging.m_txtLabel.text = "エージングモード：OFF";
			DataManagerGame.Instance.config.Remove(Defines.KEY_AGING);
		}
		DataManagerGame.Instance.config.Save();
		DataManagerGame.Instance.GetAgingState();
	}


}
