using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelPartyEdit : MonoBehaviour {


	public PartyHolder m_partyHolder;

	public Button m_btnDeck;
	public Button m_btnStatus;

	public UnityEvent OnFinished = new UnityEvent();

}
