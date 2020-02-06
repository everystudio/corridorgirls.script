using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaChara : MonoBehaviour {

	public Image m_imgChara;
	public EnergyBar hp;
	public EnergyBar tension;

	public DataUnitParam unit_param;
	public MasterCharaParam master_chara;

	public void Initialize( DataUnitParam _data , MasterCharaParam _masterCharaParam )
	{
		unit_param = _data;

		master_chara = _masterCharaParam;
		m_imgChara.sprite = SpriteManager.Instance.Get(string.Format("chara{0:000}01_00_faceicon", master_chara.chara_id));

		hp.SetValueMax(unit_param.hp_max);
		hp.SetValueCurrent(unit_param.hp);

		tension.SetValueCurrent(unit_param.tension);
	}

	public void Refresh()
	{
		hp.SetValueMax(unit_param.hp_max);
		hp.SetValueCurrent(unit_param.hp);
	}

}
