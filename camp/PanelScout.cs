using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScout : MonoBehaviour {

	public PanelCharaDetail m_panelCharaDetail;

	public GameObject m_prefScountIcon;
	public GameObject m_goContentRoot;
	public List<CharaIcon> icon_list = new List<CharaIcon>();

	// リストで押されたキャラIDを返す
	public UnityEventInt OnListCharaId = new UnityEventInt();

	public BtnInvite m_btnInvite;

	public int Show()
	{
		int iRet = 0;
		m_prefScountIcon.SetActive(false);
		CharaIcon[] arr = m_goContentRoot.GetComponentsInChildren<CharaIcon>();
		foreach (CharaIcon c in arr)
		{
			if (m_prefScountIcon != c.gameObject)
			{
				GameObject.Destroy(c.gameObject);
			}
		}
		icon_list.Clear();


		foreach( MasterCharaParam master in DMCamp.Instance.masterChara.list.FindAll(p=>p.unit == "chara" && 0 < p.scout))
		{
			if( null == DMCamp.Instance.dataUnitCamp.list.Find(p=>p.chara_id == master.chara_id))
			{
				iRet += 1;
				CharaIcon icon = PrefabManager.Instance.MakeScript<CharaIcon>(m_prefScountIcon, m_goContentRoot);
				icon.Initialize(master);

				icon.OnClickIcon.AddListener((CharaIcon _icon) =>
				{
					OnListCharaId.Invoke(_icon.m_masterChara.chara_id);
				});
				icon_list.Add(icon);
			}
		}
		return iRet;
	}


}
