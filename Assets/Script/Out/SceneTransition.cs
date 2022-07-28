using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;
//using Assets.Scripts.Game.UI;
using UnityEngine.Assertions;


namespace Assets.Scripts._System
{
	public class SceneTransition : MonoBehaviour
	{
		/*
		/// ------------------------------------
		[Header("管理画面UI")]
		[SerializeField] GameObject ModeSelectCanvas;
		[SerializeField] CanvasGroup canvasGroupGames;
		[SerializeField] Button buttonStart;
		[SerializeField] Button buttonSetup;
		[SerializeField] GameObject Contents;
		[SerializeField] Text titleVer;
		[SerializeField] Toggle[] FOV0;
		[SerializeField] Toggle[] FOV1;
		[Header("壁面共通UI")]
		[SerializeField] DisplayUI_Wall displayUI_Wall;
		[Header("マスターデータ：Scriptable Object")]
		[SerializeField] SOGameLevel sOGameLevel;
		[SerializeField] CntrAdminScreen cntrAdminScreen;
		/// ------------------------------------

		_GameLogger.Logger logger;

		/// <summary>
		/// Kinectのアジャスト設定
		/// </summary>
		[Serializable]
		public struct Vector3InputField
		{
			public InputField x;
			public InputField y;
			public InputField z;
		}
		[Serializable]
		public class KinektDeviceSettiongs
		{
			public string Name;
			public Microsoft.Azure.Kinect.Sensor.DepthMode DepthMode;
			public Vector3InputField Rot;
			public Vector3InputField Trn;
			public InputField RoomScale;
		}

		[Header("インゲームかどうかの判定")]
		public bool IsInGame = false;

		GameObject[] kinectModeSelect = new GameObject[2];

		// Start is called before the first frame update
		void Awake()
		{
			LoadSetUp();

			kinectModeSelect[0] = GameObject.Find("Kinect0");
			kinectModeSelect[1] = GameObject.Find("Kinect1");
		}
		private void Start()
		{
			//ModeSelectCanvas.SetActive(true);
			//this.transform.GetComponent<KinectMain>().SeqIncrement();
		}

		// Update is called once per frame
		void Update()
		{
		}
#if false
		public void PresetKinectMode(int devId, int mode)
		{
			switch(devId)
			{
				case 0:
					FOV0[mode - 1].GetComponent<Toggle>().isOn = true;
					break;
				case 1:
					FOV1[mode - 1].GetComponent<Toggle>().isOn = true;
					break;
			}
		}
		public void KinectModeSet()
		{
			ModeSelectCanvas.SetActive(false);
			this.transform.GetComponent<KinectMain>().SeqIncrement();
		}
#endif

		public void StartGame()
		{
			IsInGame = true;
		}
		public void ActivateStartButton(bool _isOn)
		{
			buttonStart.interactable = _isOn;
		}

		/// <summary>
		/// CGameInfoで対応するゲームを特定する（ゲームセレクトのボタン順が変更される可能性が高い為）
		/// 管理画面UIのタイトル表示も確定する
		/// </summary>
		/// <param name="_eGameLists"></param>
		/// <returns></returns>
		AssetReference getAssetReference(SOGameLevel.EGameLists _eGameLists)
		{
			///初期化
			AssetReference aGameIn = sOGameLevel.cGameInfo[0].aGameIn;
			foreach (var game in sOGameLevel.cGameInfo)
			{
				if (game.eGameLists == _eGameLists)
				{
					displayUI_Wall.GameTitleString(game.name);
					Debug.Log(game.aGameIn);
					return game.aGameIn;
				}
			}
			return null;
		}
		public void LoadSetUp()
		{
			cntrAdminScreen.LoadSetUp();

			canvasGroupGames.interactable = true;
			displayUI_Wall.gameObject.SetActive(false);
			IsInGame = false;
			buttonSetup.interactable = false;
			foreach (var gameList in cntrAdminScreen.InstanceGameLists)
			{
				gameList.transform.Find("TitleButton").GetComponent<Button>().interactable = true;
			}
			buttonStart.interactable = false;
			titleVer.text = "『トレキング ver" + Application.version + "』";
		}

		public void ToggleKinect(Toggle toggle)
		{
			if (toggle.isOn)
			{
#if false
				int id = 0;
				switch(toggle.transform.parent.parent.parent.name)
				{
					case "C0":
						id = 0;
						break;
					case "D0":
						id = 1;
						break;
					case "E0":
						id = 2;
						break;
				}
#endif
				switch (toggle.transform.parent.gameObject.name)
				{
					case "K0":
						//effectiveDeviceId[id] = 0;
						break;
					case "K1":
						//effectiveDeviceId[id] = 1;
						break;
				}
			}
		}

		//public AssetReference objectToLoad;
		public AssetReference accessoryObjectToLoad;
		private GameObject instantiatedObject;
		private GameObject instantiatedAccessoryObject;

		private void ObjectLoadDone(AsyncOperationHandle<GameObject> obj)
		{
			if (obj.Status == AsyncOperationStatus.Succeeded)
			{
				GameObject loadedObject = obj.Result;
				Debug.Log("Successfully loaded object.");
				instantiatedObject = Instantiate(loadedObject);
				instantiatedObject.transform.GetComponent<SetParam>().MSceneTransition = this;
				instantiatedObject.transform.GetComponent<SetParam>().MKinectMain = this.GetComponent<KinectMain>();
				instantiatedObject.transform.GetComponent<SetParam>().MDisplayUI_Wall = displayUI_Wall;
				instantiatedObject.transform.GetComponent<SetParam>().MLogger = this.GetComponent<_GameLogger.Logger>();
				Debug.Log("Successfully instantiated object." + instantiatedObject.name);
			}
		}
		*/
	}
}
