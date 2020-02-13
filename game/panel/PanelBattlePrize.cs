using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBattlePrize : MonoBehaviour {

	public GameObject m_goRootBBHolder;
	public GameObject m_prefBattleBonusHolder;

	public List<BattleBonusHolder> battle_bonus_holder_list = new List<BattleBonusHolder>();
	public UnityEventInt OnClickHolder = new UnityEventInt();

	public void Initialize( int _iNum , int _iStage , int _iWave , int[] _iCharaIdArr)
	{
		m_prefBattleBonusHolder.SetActive(false);
		BattleBonusHolder[] arr = m_goRootBBHolder.GetComponentsInChildren<BattleBonusHolder>();
		foreach (BattleBonusHolder c in arr)
		{
			if (m_prefBattleBonusHolder != c.gameObject)
			{
				GameObject.Destroy(c.gameObject);
			}
		}
		battle_bonus_holder_list.Clear();

		List<MasterStageBattleBonusParam> stage_bb_list = DataManagerGame.Instance.masterStageBattleBonus.list.FindAll(p => p.stage_id == _iStage && p.wave == _iWave);
		if(stage_bb_list .Count == 0)
		{
			stage_bb_list = DataManagerGame.Instance.masterStageBattleBonus.list.FindAll(p => p.stage_id == _iStage && p.wave == 0);
		}

		int[] prob_arr = new int[stage_bb_list.Count];
		for( int i = 0; i < stage_bb_list.Count; i++)
		{
			prob_arr[i] = stage_bb_list[i].prob;

			//Debug.Log(string.Format("index={0} prob={1}", i, prob_arr[i]));
		}

		for( int set_index = 0; set_index < _iNum; set_index++)
		{
			BattleBonusHolder holder = PrefabManager.Instance.MakeScript<BattleBonusHolder>(m_prefBattleBonusHolder, m_goRootBBHolder);
			holder.Reset(set_index+1);
			for ( int party_index = 0; party_index < _iCharaIdArr.Length; party_index++)
			{
				int index = UtilRand.GetIndex(prob_arr);
				//Debug.Log(index);
				MasterBattleBonusParam master_bb = DataManagerGame.Instance.masterBattleBonus.list.Find(p => p.battle_bonus_id == stage_bb_list[index].battle_bonus_id);

				holder.Add(master_bb, _iCharaIdArr[party_index]);
			}
			battle_bonus_holder_list.Add(holder);
			holder.OnClick.AddListener((int _iIndex) =>
			{
				OnClickHolder.Invoke(_iIndex);
			});
		}
	}
	public void Select(int _iIndex)
	{
		foreach( BattleBonusHolder holder in battle_bonus_holder_list)
		{
			holder.Select(_iIndex);
		}
	}

	public BattleBonusHolder GetBBHolder(int _iIndex)
	{
		foreach (BattleBonusHolder holder in battle_bonus_holder_list)
		{
			if( holder.holder_id == _iIndex)
			{
				return holder;
			}
		}
		return null;
	}
}
