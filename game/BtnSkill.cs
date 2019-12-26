using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BtnSkill : MonoBehaviour {

	public Button m_btn;
	public Image m_imgIcon;
	public TextMeshProUGUI m_txtMP;

	public class SkillButtonHandler : UnityEvent<BtnSkill>
	{

	}
	public SkillButtonHandler OnSkillButton = new SkillButtonHandler();

	public MasterSkillParam m_masterSkillParam;

	public void Initialize(MasterSkillParam _skill)
	{
		m_btn.onClick.RemoveAllListeners();

		m_masterSkillParam = _skill;

		m_imgIcon.sprite = SpriteManager.Instance.Get(m_masterSkillParam.sprite_name);
		m_txtMP.text = string.Format("MP:{0}", m_masterSkillParam.mp);

		m_btn.onClick.AddListener(() =>
		{
			OnSkillButton.Invoke(this);
		});

	}

}
