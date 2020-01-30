using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemMain : Singleton<ItemMain> {

	public UnityEventString RequestShow = new UnityEventString();

	public UnityEvent OnClose = new UnityEvent();

	public string situation;
	public int move;
	public int damage;


	public PanelItemList m_panelItemList;
	public PanelItemDetail m_panelItemDetail;

	public void Show()
	{

	}


}
