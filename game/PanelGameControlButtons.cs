using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelGameControlButtons : MonoBehaviour {

	public List<Button> button_list;
	public List<TextMeshProUGUI> label_list;

	public UnityEventInt OnClickButton = new UnityEventInt();

	void Start()
	{
		/*
		int[] kasu = new int[button_list.Count];

		for( int i = 0; i < button_list.Count; i++)
		{
			kasu[i] = i;
		}
		int iIndex = 0;
		foreach( Button btn in button_list)
		{
			btn.onClick.AddListener(() =>
			{
				OnClickButton.Invoke(kasu[iIndex]);
			});

			iIndex += 1;
		}
		*/

		/*
		for( int i = 0; i < button_list.Count; i++)
		{
			button_list[i].onClick.AddListener(() => { OnClickButton.Invoke(i); });
		}
		*/
		// 小学生のような実装。なんかいい方法あるの？
		button_list[0].onClick.AddListener(() => { OnClickButton.Invoke(0); });
		button_list[1].onClick.AddListener(() => { OnClickButton.Invoke(1); });
		button_list[2].onClick.AddListener(() => { OnClickButton.Invoke(2); });
		/*
		OnClickButton.AddListener((int _i) =>
		{
			Debug.Log(_i);
		});
		*/
	}

	public void SetLabel( int _iIndex , string _strLabel)
	{
		label_list[_iIndex].text = _strLabel;
	}

	public void ShowButtonNum( int _iNum , string[] _label_arr )
	{
		// iNum == 0で非表示。後ろはnullでもいいよ
		bool bRootShow = 0 < _iNum;
		gameObject.SetActive(bRootShow);
		if(bRootShow == false)
		{
			return;
		}


		int iCount = 0;
		foreach( Button btn in button_list)
		{
			bool bShow = iCount < _iNum;
			btn.gameObject.SetActive(bShow);

			if( bShow)
			{
				SetLabel(iCount, _label_arr[iCount]);
			}
			iCount += 1;
		}
	}



}
