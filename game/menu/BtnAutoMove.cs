using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BtnAutoMove : MonoBehaviour {

	public GameObject m_goCheckWindow;
	public Button m_btnUse;
	public Button m_btnCancel;

	public TextMeshProUGUI m_txtButtonMessage;

	public void ShowUpdate()
	{
		if (DataManagerGame.Instance.IsAutoMove())
		{
			DataManagerGame.Instance.gameData.WriteInt(Defines.KeyMoviePlay_GameEnd, 1);
			m_txtButtonMessage.text = "オートモード\nON";
		}
		else
		{
			m_txtButtonMessage.text = "オートモード\nOFF";
		}
	}

	public void OnClickButton()
	{
		if(DataManagerGame.Instance.user_data.HasKey(Defines.KeyAutoModeMove))
		{
			int set_param = 0;
			if( 0 < DataManagerGame.Instance.user_data.ReadInt(Defines.KeyAutoModeMove))
			{
				set_param = 0;
			}
			else
			{
				set_param = 1;
			}
			DataManagerGame.Instance.user_data.WriteInt(Defines.KeyAutoModeMove, set_param);
			DataManagerGame.Instance.user_data.Save();
		}
		else
		{
			m_goCheckWindow.SetActive(true);
			m_btnUse.onClick.RemoveAllListeners();
			m_btnCancel.onClick.RemoveAllListeners();
			m_btnUse.onClick.AddListener(() =>
			{
				DataManagerGame.Instance.user_data.WriteInt(Defines.KeyAutoModeMove, 1);
				DataManagerGame.Instance.user_data.Save();
				m_goCheckWindow.SetActive(false);
				ShowUpdate();
			});
			m_btnCancel.onClick.AddListener(() =>
			{
				m_goCheckWindow.SetActive(false);
			});
		}
		ShowUpdate();
	}




}
