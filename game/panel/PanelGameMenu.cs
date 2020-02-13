using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelGameMenu : MonoBehaviour {
	public BannerMenu m_bannerAging;

	public void Show()
	{
		bool bDevelopment = Application.identifier.Contains("development");

#if UNITY_EDITOR
		bDevelopment = true;
#endif
		m_bannerAging.gameObject.SetActive(bDevelopment);

	}


	void Start()
	{
		m_bannerAging.m_bToggle = DataManagerGame.Instance.IsAging;
		ToggleAgingMenu(m_bannerAging.m_bToggle);


		m_bannerAging.m_btn.onClick.AddListener(() =>
		{
			ToggleAgingMenu(!m_bannerAging.m_bToggle);
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
