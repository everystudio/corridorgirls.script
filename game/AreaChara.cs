using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaChara : MonoBehaviour {

	public Image m_imgChara;
	public EnergyBar hp;

	public DataUnitParam unit_param;

	public void Initialize( DataUnitParam _data)
	{
		unit_param = _data;

		hp.SetValueMax(unit_param.hp_max);
		hp.SetValueCurrent(unit_param.hp);
	}

	public void Refresh()
	{
		hp.SetValueMax(unit_param.hp_max);
		hp.SetValueCurrent(unit_param.hp);
	}

}
