using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace CorridorGirls
{
	public class MakeMasterData
	{


		[MenuItem("Tools/Make MasterData")]
		public static void MakeData()
		{
			EveryStudioLibrary.Editor.EditorCoroutine.start(makeData());
		}

		[MenuItem("Tools/Make MasterData_test")]
		public static void MakeDataTest()
		{
			EveryStudioLibrary.Editor.EditorCoroutine.start(makeData());
		}

		private static IEnumerator makeDataTest()
		{
			MasterMissionDetail masterMissionDetail = new MasterMissionDetail();
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterMissionDetail.SpreadSheet(DataManagerGame.SS_ID, "mission_detail", () => {
				masterMissionDetail.SaveEditor("07data/master", "master_mission_detail");
				foreach (MasterMissionDetailParam p in masterMissionDetail.list)
				{
					//Debug.Log(p.message);
				}
			}));
		}

		static IEnumerator makeData()
		{
			#region ゲーム
			MasterStage masterStage = new MasterStage();
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterStage.SpreadSheet(DataManagerGame.SS_ID, "stage", () => {
				masterStage.SaveEditor("07data/master", "master_stage");
			}));

			MasterCorridor masterCorridor = new MasterCorridor();
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterCorridor.SpreadSheet(DataManagerGame.SS_ID, "corridor", () => {
				masterCorridor.SaveEditor("07data/master", "master_corridor");
			}));
			MasterCorridorEvent masterCorridorEvent = new MasterCorridorEvent();
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterCorridorEvent.SpreadSheet(DataManagerGame.SS_ID, "corridor_event", () => {
				masterCorridorEvent.SaveEditor("07data/master", "master_corridor_event");
			}));

			MasterCard masterCard = new MasterCard();
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterCard.SpreadSheet(DataManagerGame.SS_ID, "card", () => {
				masterCard.SaveEditor("07data/master", "master_card");
			}));

			MasterCardSymbol masterCardSymbol = new MasterCardSymbol();

			MasterChara masterChara = new MasterChara();
			MasterCharaCard masterCharaCard = new MasterCharaCard();

			MasterItem masterItem = new MasterItem();
			MasterStageWave masterStageWave = new MasterStageWave();
			MasterStageEvent masterStageEvent = new MasterStageEvent();
			MasterStageItem masterStageItem = new MasterStageItem();
			MasterStageCard masterStageCard = new MasterStageCard();
			MasterStageMission masterStageMission = new MasterStageMission();
			MasterStageEnemy masterStageEnemy = new MasterStageEnemy();
			MasterStageBattleBonus masterStageBattleBonus = new MasterStageBattleBonus();
			MasterStageShopItem masterStageShopItem = new MasterStageShopItem();

			MasterSkill masterSkill = new MasterSkill();
			MasterSkillEffect masterSkillEffect = new MasterSkillEffect();

			MasterBattleBonus masterBattleBonus = new MasterBattleBonus();

			MasterMission masterMission = new MasterMission();
			MasterMissionDetail masterMissionDetail = new MasterMissionDetail();


			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterCardSymbol.SpreadSheet(DataManagerGame.SS_ID, "card_symbol", () => {
				masterCardSymbol.SaveEditor("07data/master", "master_card_symbol");
			}));

			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterChara.SpreadSheet(DataManagerGame.SS_ID, "chara", () => {
				masterChara.SaveEditor("07data/master", "master_chara");
			}));
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterCharaCard.SpreadSheet(DataManagerGame.SS_ID, "chara_card", () => {
				masterCharaCard.SaveEditor("07data/master", "master_chara_card");
			}));

			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterItem.SpreadSheet(DataManagerGame.SS_ID, "item", () => {
				masterItem.SaveEditor("07data/master", "master_item");
			}));
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterStageWave.SpreadSheet(DataManagerGame.SS_ID, "stage_wave", () => {
				masterStageWave.SaveEditor("07data/master", "master_stage_wave");
			}));
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterStageEvent.SpreadSheet(DataManagerGame.SS_ID, "stage_event", () => {
				masterStageEvent.SaveEditor("07data/master", "master_stage_event");
			}));
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterStageItem.SpreadSheet(DataManagerGame.SS_ID, "stage_item", () => {
				masterStageItem.SaveEditor("07data/master", "master_stage_item");
			}));
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterStageCard.SpreadSheet(DataManagerGame.SS_ID, "stage_card", () => {
				masterStageCard.SaveEditor("07data/master", "master_stage_card");
			}));
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterStageMission.SpreadSheet(DataManagerGame.SS_ID, "stage_mission", () => {
				masterStageMission.SaveEditor("07data/master", "master_stage_mission");
			}));
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterStageEnemy.SpreadSheet(DataManagerGame.SS_ID, "stage_enemy", () => {
				masterStageEnemy.SaveEditor("07data/master", "master_stage_enemy");
			}));
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterStageBattleBonus.SpreadSheet(DataManagerGame.SS_ID, "stage_bb", () => {
				masterStageBattleBonus.SaveEditor("07data/master", "master_stage_bb");
			}));
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterStageShopItem.SpreadSheet(DataManagerGame.SS_ID, "stage_shopitem", () => {
				masterStageShopItem.SaveEditor("07data/master", "master_stage_shopitem");
			}));

			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterSkill.SpreadSheet(DataManagerGame.SS_ID, "skill", () => {
				masterSkill.SaveEditor("07data/master", "master_skill");
			}));
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterSkillEffect.SpreadSheet(DataManagerGame.SS_ID, "skill_effect", () => {
				masterSkillEffect.SaveEditor("07data/master", "master_skill_effect");
			}));

			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterBattleBonus.SpreadSheet(DataManagerGame.SS_ID, "bb", () => {
				masterBattleBonus.SaveEditor("07data/master", "master_bb");
			}));

			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterMission.SpreadSheet(DataManagerGame.SS_ID, "mission", () => {
				masterMission.SaveEditor("07data/master", "master_mission");
			}));
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterMissionDetail.SpreadSheet(DataManagerGame.SS_ID, "mission_detail", () => {
				masterMissionDetail.SaveEditor("07data/master", "master_mission_detail");
				foreach (MasterMissionDetailParam p in masterMissionDetail.list)
				{
					//Debug.Log(p.message);
				}
			}));
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterMission.SpreadSheet(DataManagerGame.SS_ID, "mission", () => {
				masterMission.SaveEditor("07data/master", "master_mission");
			}));


			#endregion

			#region キャンプ
			MasterCampItem masterCampItem = new MasterCampItem();
			MasterCampShop masterCampShop = new MasterCampShop();
			MasterLevelup masterLevelup = new MasterLevelup();
			MasterHelp masterHelp = new MasterHelp();

			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterCampItem.SpreadSheet(DMCamp.SS_ID, "campitem", () => {
				masterCampItem.SaveEditor("07data/master", "master_campitem");
			}));
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterCampShop.SpreadSheet(DMCamp.SS_ID, "campshop", () => {
				masterCampShop.SaveEditor("07data/master", "master_campshop");
			}));
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterLevelup.SpreadSheet(DMCamp.SS_ID, "levelup", () => {
				masterLevelup.SaveEditor("07data/master", "master_levelup");
			}));
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterHelp.SpreadSheet(DMCamp.SS_ID, "help", () => {
				masterHelp.SaveEditor("07data/master", "master_help");
			}));

			

			#endregion
			#region チュートリアル用
			DataCard tutorialCard = new DataCard();
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(tutorialCard.SpreadSheet(DMCamp.SS_ID, "tutorial_card", () => {
				tutorialCard.SaveEditor("07data/master", "data_tutorial_card");
			}));
			#endregion

		}

	}
}
