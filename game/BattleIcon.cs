using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleIcon : MonoBehaviour {

	public SpriteRenderer sprIcon;

	public int index;
	public bool is_left;

	public MasterCardSymbolParam master_symbol;

	public void Initialize(MasterCardSymbolParam _symbol , int _iIndex)
	{
		index = _iIndex;
		sprIcon.sprite = SpriteManager.Instance.Get(_symbol.sprite_name);

		float pos_x = 0.3f * (is_left ? -1 : 1) - (0.35f * index * (is_left ? -1 : 1));
		float pos_y = -0.5f - (0.15f * _symbol.line);

		transform.localPosition = new Vector3(pos_x, pos_y, -1.0f);
	}

}
