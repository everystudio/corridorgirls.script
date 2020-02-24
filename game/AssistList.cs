using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistList : MonoBehaviour {

	public GameObject m_goContents;
	public GameObject m_prefAssistIcon;

	public void Show(int _iCharaId)
	{
		m_prefAssistIcon.SetActive(false);
		AssistIcon[] arr = m_goContents.GetComponentsInChildren<AssistIcon>();
		foreach (AssistIcon c in arr)
		{
			if (m_prefAssistIcon != c.gameObject)
			{
				GameObject.Destroy(c.gameObject);
			}
		}

		List<DataUnitParam> assist_list = DataManagerGame.Instance.dataUnit.list.FindAll(p =>
		p.chara_id == _iCharaId &&
		p.unit == "assist" &&
		p.turn != 0);

		foreach( DataUnitParam assist in assist_list)
		{
			AssistIcon script = PrefabManager.Instance.MakeScript<AssistIcon>(m_prefAssistIcon, m_goContents);
			script.Show(assist);
		}
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}


}
