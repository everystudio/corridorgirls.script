using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyHolder : MonoBehaviour {

	[SerializeField]
	private CharaIcon left;
	[SerializeField]
	private CharaIcon right;
	[SerializeField]
	private CharaIcon back;

	public void Reset()
	{

	}

	public void Initialize( MasterCharaParam _left , MasterCharaParam  _right , MasterCharaParam  _back)
	{
		left.Initialize(_left);
		right.Initialize(_right);
		back.Initialize(_back);
	}

	public void Cover( int _iCharaId)
	{
		left.Cover(_iCharaId);
		right.Cover(_iCharaId);
		back.Cover(_iCharaId);
	}

}
