﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

public class PanelMission : Singleton<PanelMission> {

	public TextMeshProUGUI m_txtMain;

	public Button m_btnYes;
	public Button m_btnNo;
	public TextMeshProUGUI m_txtBtnYes;
	public TextMeshProUGUI m_txtBtnNo;

	public Button m_btnContinue;

	public GameObject m_goRoot;
	public GameObject m_goRootContinue;
	public GameObject m_goRootYesNo;

	public UnityEvent OnFinished = new UnityEvent();
	public UnityEventInt RequestMissionHandler = new UnityEventInt();

	public IEnumerator MessageShow(string _strMessage)
	{
		//Debug.Log(_strMessage.Length);
		if(  _strMessage.Length <= 0)
		{
			m_txtMain.text = "";
			yield break;
		}
		int length = 0;
		while (length < _strMessage.Length)
		{
			length += 1;
			m_txtMain.text = _strMessage.Substring(0, length);
			yield return null;
			//yield return new WaitForSeconds(0.02f);
		}
	}

	public List<MasterMissionDetailParam> prize_list = new List<MasterMissionDetailParam>();

	public MasterMissionParam masterMissionParam;
	public List<MasterMissionDetailParam> masterMissionDetailParamList = new List<MasterMissionDetailParam>();

	public void RequestMission(int _iMissionId)
	{
		RequestMissionHandler.Invoke(_iMissionId);
	}

	public void set_mission( int _iMissionId)
	{
		masterMissionParam = DataManagerGame.Instance.masterMission.list.Find(p => p.mission_id == _iMissionId);
		masterMissionDetailParamList = DataManagerGame.Instance.masterMissionDetail.list.FindAll(p => p.mission_id == _iMissionId);
	}

	public void Close()
	{
		m_goRoot.SetActive(false);
		m_goRootContinue.SetActive(false);
		m_goRootYesNo.SetActive(false);
	}

	public void ShowTwoButton(string _strKeyMessage)
	{
		MasterMissionDetailParam detail = masterMissionDetailParamList.Find(p => p.type == _strKeyMessage);
		StartCoroutine(MessageShow(detail.message));
		m_goRoot.SetActive(true);
		m_goRootYesNo.SetActive(true);
		m_goRootContinue.SetActive(false);

		m_txtBtnYes.text = masterMissionParam.btnlabel_yes;
		m_txtBtnNo.text = masterMissionParam.btnlabel_no;
	}

	public void ShowMessageTwoButton(string _strMessage)
	{
		StartCoroutine(MessageShow(_strMessage));
		m_goRoot.SetActive(true);
		m_goRootYesNo.SetActive(true);
		m_goRootContinue.SetActive(false);

		m_txtBtnYes.text = masterMissionParam.btnlabel_yes;
		m_txtBtnNo.text = masterMissionParam.btnlabel_no;
	}


	/*
	public void ShowIntro()
	{
		MasterMissionDetailParam detail = masterMissionDetailParamList.Find(p => p.type == "intro");
		StartCoroutine(MessageShow(detail.param));
		m_goRoot.SetActive(true);
		m_goRootYesNo.SetActive(true);
		m_goRootContinue.SetActive(false);

		m_txtBtnYes.text = masterMissionParam.btnlabel_yes;
		m_txtBtnNo.text = masterMissionParam.btnlabel_no;

		Canvas.ForceUpdateCanvases();
	}
	*/

	public void ShowSuccess()
	{
		MasterMissionDetailParam detail = masterMissionDetailParamList.Find(p => p.type == "success");
		StartCoroutine(MessageShow(detail.message));
		m_goRoot.SetActive(true);
		m_goRootYesNo.SetActive(false);
		m_goRootContinue.SetActive(true);
	}

	public void ShowFail()
	{
		MasterMissionDetailParam detail = masterMissionDetailParamList.Find(p => p.type == "fail");
		StartCoroutine(MessageShow(detail.message));
		m_goRoot.SetActive(true);
		m_goRootYesNo.SetActive(false);
		m_goRootContinue.SetActive(true);
	}

	public void ShowNo()
	{
		MasterMissionDetailParam detail = masterMissionDetailParamList.Find(p => p.type == "no");
		StartCoroutine(MessageShow(detail.message));
		m_goRoot.SetActive(true);
		m_goRootYesNo.SetActive(false);
		m_goRootContinue.SetActive(true);
		Canvas.ForceUpdateCanvases();
	}

	public void ShowNoItem(string _strMessage )
	{
		StartCoroutine(MessageShow(_strMessage));
		m_goRoot.SetActive(true);
		m_goRootYesNo.SetActive(false);
		m_goRootContinue.SetActive(true);
	}

}
