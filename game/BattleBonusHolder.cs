using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBonusHolder : MonoBehaviour {

	public Button m_btn;
	public Image m_imgBack;
	public int holder_id;

	public UnityEventInt OnClick = new UnityEventInt();

	void Start()
	{
		m_btn.onClick.AddListener(() =>
		{
			OnClick.Invoke(holder_id);
		});
	}
	public void Select( int _iIndex )
	{
		Select(holder_id == _iIndex);
	}

	public void Select( bool _bFlag)
	{
		m_imgBack.color = _bFlag ? new Color(1.0f, 0.5f, 0.5f):Color.white;
	}

	public List<MasterBattleBonusParam> battle_bonus_list = new List<MasterBattleBonusParam>();
	public List<int> chara_id_list = new List<int>();

	public List<IconBattleBonus> icon_battle_bonus_list = new List<IconBattleBonus>();

	public GameObject m_goRoot;
	public GameObject m_prefIcon;

	public void Reset( int _iId )
	{
		holder_id = _iId;

		m_prefIcon.SetActive(false);

		IconBattleBonus[] arr = m_goRoot.GetComponentsInChildren<IconBattleBonus>();
		foreach (IconBattleBonus c in arr)
		{
			if (m_prefIcon != c.gameObject)
			{
				GameObject.Destroy(c.gameObject);
			}
		}
		battle_bonus_list.Clear();
		chara_id_list.Clear();
		icon_battle_bonus_list.Clear();
		Select(false);
	}

	public void Add(MasterBattleBonusParam _master , int _iCharaId)
	{
		battle_bonus_list.Add(_master);
		chara_id_list.Add(_iCharaId);

		IconBattleBonus icon = PrefabManager.Instance.MakeScript<IconBattleBonus>(m_prefIcon, m_goRoot);
		icon.Initialize(_master, _iCharaId);
		icon_battle_bonus_list.Add(icon);
	}


	
}
