using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CharaIcon : MonoBehaviour {

	public Button m_btn;
	public Image m_imgIcon;
	public TextMeshProUGUI m_txtName;

	public Image m_imgCover;

	public MasterCharaParam m_masterChara;

	void Start()
	{
		Cover(false);
	}

	public class OnCharaIcon : UnityEvent<CharaIcon>
	{

	}
	public OnCharaIcon OnClickIcon = new OnCharaIcon();

	public void Initialize( MasterCharaParam _master)
	{
		m_txtName.text = _master.name;
		m_masterChara = _master;


		m_imgIcon.sprite = SpriteManager.Instance.Get(string.Format("chara{0:000}01_00_faceicon", _master.chara_id));

		m_btn.onClick.RemoveAllListeners();
		m_btn.onClick.AddListener(() =>
		{
			OnClickIcon.Invoke(this);
		});
	}

	public void Cover( int _iCharaId)
	{
		if (m_masterChara != null)
		{
			Debug.Log(string.Format("chara_id={0} _iCharaId={1}", m_masterChara.chara_id, _iCharaId));
			Cover(m_masterChara.chara_id == _iCharaId);
		}
		else
		{
			Cover(false);
		}
	}

	public void Cover( bool _bShow)
	{
		Debug.Log(_bShow);
		m_imgCover.gameObject.SetActive(_bShow);
	}
	

}
