using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemMain : Singleton<ItemMain> {

	public UnityEvent RequestShow = new UnityEvent();

	public UnityEvent OnClose = new UnityEvent();


	public int move;


	public PanelItemList m_panelItemList;
	public PanelItemDetail m_panelItemDetail;

	public void Show()
	{

	}


}
