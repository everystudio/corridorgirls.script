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

	public CharaIcon.OnCharaIcon OnClickIcon = new CharaIcon.OnCharaIcon();

	public void Reset()
	{

	}

	public void Initialize( MasterCharaParam _left , MasterCharaParam  _right , MasterCharaParam  _back)
	{
		left.Initialize(_left);
		right.Initialize(_right);
		back.Initialize(_back);

		left.OnClickIcon.RemoveAllListeners();
		right.OnClickIcon.RemoveAllListeners();
		back.OnClickIcon.RemoveAllListeners();

		left.OnClickIcon.AddListener((CharaIcon _icon) =>
		{
			OnClickIcon.Invoke(_icon);
		});
		right.OnClickIcon.AddListener((CharaIcon _icon) =>
		{
			OnClickIcon.Invoke(_icon);
		});
		back.OnClickIcon.AddListener((CharaIcon _icon) =>
		{
			OnClickIcon.Invoke(_icon);
		});

	}

	public void Cover( int _iCharaId)
	{
		left.Cover(_iCharaId);
		right.Cover(_iCharaId);
		back.Cover(_iCharaId);
	}

}
