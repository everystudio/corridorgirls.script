using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class IconStageShopItem : MonoBehaviour {

	public int icon_index;
	public Button m_btn;
	public TextMeshProUGUI m_txtItemName;
	public Image m_imgItem;
	public TextMeshProUGUI m_txtGold;

	public Animator m_animator;
	public MasterItemParam m_masterItemParam;

	public class HanderClickedIcon : UnityEvent<IconStageShopItem>
	{
	}
	public HanderClickedIcon OnClickIcon = new HanderClickedIcon();

	public void Select( bool _bFlag)
	{
		m_animator.SetBool("select", _bFlag);
	}

	public void Initialize(int _iIndex , MasterItemParam _master)
	{
		icon_index = _iIndex;
		m_masterItemParam = _master;
		m_animator.Play("idle");
		Select(false);
		m_txtItemName.text = _master.name;
		m_imgItem.sprite = SpriteManager.Instance.Get(_master.sprite_name);
		m_txtGold.text = _master.gold.ToString();

		m_btn.onClick.RemoveAllListeners();
		m_btn.onClick.AddListener(() =>
		{
			OnClickIcon.Invoke(this);
		});
	}

}
