using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMain : Singleton<SkillMain> {

	public UnityEventInt SkillRequest = new UnityEventInt();
	public UnityEventBool SkillFinishHandler = new UnityEventBool();

	public MasterSkillParam master_skill_param;

	public List<MasterSkillEffectParam> master_skill_effect_param_list = new List<MasterSkillEffectParam>();

}
