using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelChara : MonoBehaviour {

	public Button m_btnClose;
	public Button m_btnEdit;
	public Button m_btnList;

	public GameObject m_goCharaListRoot;
	public GameObject m_prefCharaIcon;
	public GameObject m_goCharaListContents;

	public Button m_btnListClose;

	// リストで押されたキャラIDを返す
	public UnityEventInt OnListCharaId = new UnityEventInt();

	public List<CharaIcon> icon_list = new List<CharaIcon>();

	public GameObject m_goCharaButtons;

	public PanelCharaDetail m_panelCharaDetail;

	void Start()
	{
		m_prefCharaIcon.SetActive(false);
		m_goCharaListRoot.SetActive(false);
	}

	public void ShowList()
	{
		m_goCharaListRoot.SetActive(true);

		CharaIcon[] arr = m_goCharaListContents.GetComponentsInChildren<CharaIcon>();
		foreach (CharaIcon c in arr)
		{
			if (m_prefCharaIcon != c.gameObject)
			{
				GameObject.Destroy(c.gameObject);
			}
		}
		icon_list.Clear();

		//Debug.Log( DMCamp.Instance.masterChara.list.Count );
		foreach (MasterCharaParam p in DMCamp.Instance.masterChara.list)
		{
			//Debug.Log(string.Format("id:{0} name:{1} unit:{2}", p.chara_id, p.name, p.unit));
		}

		foreach (MasterCharaParam p in DMCamp.Instance.masterChara.list.FindAll(p=>p.unit == "chara"))
		{
			CharaIcon icon = PrefabManager.Instance.MakeScript<CharaIcon>(m_prefCharaIcon, m_goCharaListContents);
			icon.Initialize(p);

			icon.OnClickIcon.AddListener((CharaIcon _icon) =>
			{
				OnListCharaId.Invoke(_icon.m_masterChara.chara_id);
			});
			icon_list.Add(icon);
		}
	}

	public void CloseList()
	{
		m_goCharaListRoot.SetActive(false);
	}
}
