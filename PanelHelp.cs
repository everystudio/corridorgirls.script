using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelHelp : Singleton<PanelHelp> {

	public GameObject m_goRoot;
	public Button btn;

	public Image m_img;
	public TMPro.TextMeshProUGUI m_txtTitle;

	public void Close()
	{
		m_goRoot.SetActive(false);
	}
	public void ShowCamp( int _iHelpId)
	{
		Show(DMCamp.Instance.masterHelp.list.Find(p => p.help_id == _iHelpId));
	}

	public void Show(MasterHelpParam _master)
	{
		m_goRoot.SetActive(true);

		m_txtTitle.text = _master.title;
		m_img.sprite = SpriteManager.Instance.Get(_master.image_name);
	}


}
