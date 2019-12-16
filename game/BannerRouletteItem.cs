using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BannerRouletteItem : MonoBehaviour {

	public Image m_imgBack;
	public Image m_imgIcon;
	public TextMeshProUGUI m_txtName;

	public Animator m_animator;
	public MasterItemParam masterItem;

	public void Initialize( MasterItemParam _master)
	{
		masterItem = _master;
		m_imgIcon.sprite = SpriteManager.Instance.Get(_master.sprite_name);
		m_txtName.text = _master.name;
	}

	public void Initialize( int _iItemId )
	{
		MasterItemParam master = DataManager.Instance.masterItem.list.Find(p => p.item_id == _iItemId);
		Initialize(master);
	}


}
