using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleIcon : MonoBehaviour {

	public SpriteRenderer sprIcon;
	public Animator m_animator;
	public int index;
	public bool is_left;

	public int m_iDamageNum;

	public const float POS_Z = -1.0f;

	public class EventBattleIcon : UnityEvent<BattleIcon> {
	}
	public EventBattleIcon HitHandler = new EventBattleIcon();
	public EventBattleIcon AttackHandler = new EventBattleIcon();

	public MasterCardSymbolParam master_symbol;

	public void Initialize(MasterCardSymbolParam _symbol , int _iIndex , bool _bIsLeft)
	{
		index = _iIndex;
		sprIcon.sprite = SpriteManager.Instance.Get(_symbol.sprite_name);
		is_left = _bIsLeft;
		master_symbol = _symbol;

		transform.localScale = _bIsLeft ? Vector3.one : new Vector3(-1.0f, 1.0f, 1.0f);

		set_pos(index+10, master_symbol.line, is_left);
		move(0.5f ,index, master_symbol.line, is_left);
	}

	public void set_pos(int _iIndex ,int _iLine , bool _bIsLeft)
	{
		float pos_x = 0.3f * (_bIsLeft? -1 : 1) + (0.35f * _iIndex * (_bIsLeft ? -1 : 1));
		float pos_y = -0.5f - (0.15f * _iLine);

		transform.localPosition = new Vector3(pos_x, pos_y, -1.0f);
	}

	public void move(float _fTime , int _iIndex, int _iLine, bool _bIsLeft)
	{
		float pos_x = 0.3f * (_bIsLeft ? -1 : 1) + (0.35f * _iIndex * (_bIsLeft ? -1 : 1));
		float pos_y = -0.5f - (0.15f * _iLine);

		//Debug.Log(string.Format("pos_x={0} pos_y={1}", pos_x, pos_y));

		iTween.MoveTo(gameObject,
				iTween.Hash(
					"x", pos_x,
					"y", pos_y,
					"z", POS_Z,
					"time", _fTime,
					"oncomplete", "MoveCompleteHandler",
					"oncompletetarget", gameObject,
					"isLocal", true)
					);


	}

	public void MoveCompleteHandler()
	{
		//Debug.Log("MoveCompleteHandler");
	}

	public void Hit()
	{
		//Debug.Log("hit");

		GameObject root = null;

		if( is_left)
		{
			root = BattleMain.Instance.m_goBattleEnemy;
		}
		else
		{
			root = BattleMain.Instance.m_goBattleChara;
		}

		DamageNum script = PrefabManager.Instance.MakeScript<DamageNum>(BattleMain.Instance.m_prefDamageNum, root);
		//Debug.Log(script.gameObject.transform.localPosition);
		script.gameObject.transform.localPosition = new Vector3(0.0f, -1.5f, 0.0f);

		m_iDamageNum = 12;

		HitHandler.Invoke(this);

		script.Action(m_iDamageNum, () =>
		{
			AttackHandler.Invoke(this);
		});

		//AttackHandler.Invoke(this);
	}

}
