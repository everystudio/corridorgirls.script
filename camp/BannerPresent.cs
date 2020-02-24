using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BannerPresent : MonoBehaviour {

	public TextMeshProUGUI m_txtTitle;
	public TextMeshProUGUI m_txtOutline;
	public TextMeshProUGUI m_txtGetDate;
	public Image m_imgIcon;
	public Button m_btn;

	public DataPresentParam m_data;

	public class ClickBannerEvent : UnityEvent<DataPresentParam>
	{
	}
	public ClickBannerEvent OnClickBanner = new ClickBannerEvent();

	public void Initialize( DataPresentParam _data)
	{
		m_data = _data;
		MasterItemParam master = DMCamp.Instance.masterItem.list.Find(p => p.item_id == _data.item_id);

		m_txtTitle.text = string.Format("{0}x{1}", master.name, _data.num);
		m_txtOutline.text = _data.outline;
		m_txtGetDate.text = string.Format("獲得日時　{0}", _data.get_date);

		m_imgIcon.sprite = SpriteManager.Instance.Get(master.sprite_name);

		m_btn.onClick.RemoveAllListeners();
		m_btn.onClick.AddListener(() =>
		{
			OnClickBanner.Invoke(m_data);
		});
	}

}
