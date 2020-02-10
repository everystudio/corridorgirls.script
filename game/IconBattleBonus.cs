using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class IconBattleBonus : MonoBehaviour {

	public Button m_btn;

	public Image m_imgFaceIcon;
	public TextMeshProUGUI m_txtBonus;

	public MasterBattleBonusParam m_masterBB;

	public class IconBattleBonusHandler : UnityEvent<MasterBattleBonusParam>
	{

	}
	public IconBattleBonusHandler OnIconClicked = new IconBattleBonusHandler();

	public void Initialize(MasterBattleBonusParam _master , int _iCharaId)
	{
		m_btn.onClick.RemoveAllListeners();
		m_imgFaceIcon.sprite = SpriteManager.Instance.Get(string.Format(Defines.STR_FORMAT_FACEICON, _iCharaId));

		//Debug.Log(_master);
		//Debug.Log(_master.field);

		string name = _master.field.ToUpper();
		if(_master.field == "hp_max")
		{
			name = "最大HP";
		}

		m_txtBonus.text = string.Format("{0}+{1}", name, _master.param);

		m_masterBB = _master;

		m_btn.onClick.AddListener(() =>
		{
			OnIconClicked.Invoke(m_masterBB);
		});
	}

}
