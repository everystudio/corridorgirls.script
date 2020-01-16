using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BannerSkill : MonoBehaviour
{
	public Button m_btn;
	public Image m_imgIcon;

	public TextMeshProUGUI m_txtName;
	public TextMeshProUGUI m_txtOutline;
	public TextMeshProUGUI m_txtMP;

	public Image m_imgSelectCover;

	public MasterSkillParam m_master;

	public class SkillBannerHandler : UnityEvent<BannerSkill>
	{

	}

	public SkillBannerHandler OnSkillBanner = new SkillBannerHandler();

	public void Initialize(MasterSkillParam _master)
	{
		m_master = _master;

		m_imgIcon.sprite = SpriteManager.Instance.Get(m_master.sprite_name);
		m_txtMP.text = string.Format("MP:{0}", m_master.mp);

		m_txtName.text = m_master.name;
		m_txtOutline.text = m_master.outline;

		m_btn.onClick.RemoveAllListeners();

		m_btn.onClick.AddListener(() =>
		{
			OnSkillBanner.Invoke(this);
		});
	}

}