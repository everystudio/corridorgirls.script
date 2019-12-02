using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : DataManagerBase<DataManager> {

	public static readonly string CONFIG_SS = "18bT3K48n7wOjPwQP-zn1QdUs_JoNKVKZpE6pb-I6PUQ";
	public static readonly string SS_QUEST = "1ls0u6TFbOisVM5nOw6cIP1Y4nK0mDtdgRPaOKmlzGvU";

	public int m_iWaitLoadNum;

	public MasterGimic masterGimic = new MasterGimic();
	public DataDungeonStep dataDungeonStep = new DataDungeonStep();

	public MasterCard masterCard = new MasterCard();

	public DataCard dataHand = new DataCard();
	public DataCard dataDeck = new DataCard ();

	public override void Initialize()
	{
		base.Initialize();
		m_iWaitLoadNum = 0;
		SetDontDestroy(true);
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;

		// しかるべきURLが決まったら設定する必要あり
		AssetBundleManager.Instance.Initialize("https://s3-ap-northeast-1.amazonaws.com/every-studio/app/dungeongirls/AssetBundles", 0);

		m_iWaitLoadNum += 1;
		masterGimic.OnRecieveData.AddListener(OnRecievedMasterDungeonStep);
		masterGimic.SpreadSheet(CONFIG_SS, "gimic");

		masterCard.OnRecieveData.AddListener(OnRecievedMasterCard);
		masterCard.SpreadSheet(CONFIG_SS, "card");
	}

	private void OnRecievedMasterDungeonStep(List<MasterGimicParam> arg0)
	{
		m_iWaitLoadNum -= 1;
		foreach( MasterGimicParam param in arg0)
		{
			//Debug.Log(param.name);
		}

		m_iWaitLoadNum += 1;
		dataDungeonStep.OnRecieveData.AddListener(OnRecievedDataDungeonStep);
		dataDungeonStep.SpreadSheet(SS_QUEST, "sample");
	}

	private void OnRecievedDataDungeonStep(List<DataDungeonStepParam> arg0)
	{
		m_iWaitLoadNum -= 1;
		foreach( DataDungeonStepParam param in arg0)
		{
			//MasterDungeonStepParam masterParam = masterDungeonStep.Get(param.dungeon_step_id);
			//Debug.Log(string.Format("pos:{0} name:{1}", param.pos, masterParam.name));
		}
	}
	private void OnRecievedMasterCard(List<MasterCardParam> arg0)
	{
		foreach(MasterCardParam param in arg0)
		{
			Debug.Log(param.card_id);
		}
	}


}
