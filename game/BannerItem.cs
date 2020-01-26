using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BannerItem : MonoBehaviour {

	public UnityEventInt OnClickBanner = new UnityEventInt();

	public Button m_btn;

	public Image m_imgIcon;
	public TextMeshProUGUI m_txtName;

	public DataItemParam dataItem;
	public MasterItemParam masterItem;

	public void Initialize(DataItemParam _data , MasterItemParam _master)
	{
		dataItem = _data;
		masterItem = _master;
		m_imgIcon.sprite = SpriteManager.Instance.Get(_master.sprite_name);
		m_txtName.text = _master.name;

		m_btn.onClick.RemoveAllListeners();
		m_btn.onClick.AddListener(() =>
		{
			OnClickBanner.Invoke(dataItem.serial);
		});
	}

}
