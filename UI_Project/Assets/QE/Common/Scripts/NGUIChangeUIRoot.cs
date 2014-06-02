using UnityEngine;
using System.Collections;

namespace QE
{
	namespace Common
	{

		[ExecuteInEditMode]
		//**********************************************************
		/// <summary>
		/// NGUIのUIRootの設定を先に行う
		/// <br>※ Edit->Project Settings->Script Execution OrderにてNGUI系コードより先に実行するように配置する事(特にUIRoot)
		/// <br>　 なので、QEDialogNGUIChangeUIRootをScript Execution Orderに登録してQEDialogNGUIChangeUIRootが先になるようにする事
		/// <br>現在は画面の縦サイズをチェックして縦1136とそれ以外でUIRootのmanualHeightを変更しています(iPhone5だけ縦長にする対応)
		/// <br>またscalingStyleもFixedSizeにしています
		/// <br>上記の処理はAwake時に行うようになっています
		/// </summary>
		//**********************************************************
		public class NGUIChangeUIRoot : MonoBehaviour {
		
			public UIRoot uiRootObj;					//<! 変更するUIRootを格納(Inspectorで設定)
			public QE.Common.camAspect camAspectObj;	//<! 変更するcamAspectを格納(Inspectorで設定)
			public bool bChange;						//<! Editor用に再設定を行うスイッチ(Inspectorで使用)
		
			void Awake () {
				changeUIRoot();
			}
			
			//**********************************************************
			/// <summary>
			/// UIRootへの設定を行う
			/// <br>画面の縦サイズをチェックして縦1136とそれ以外でUIRootのmanualHeightを変更しています(iPhone5だけ縦長にする対応)
			/// <br>またscalingStyleもFixedSizeにしています
			/// </summary>
			//**********************************************************
			void changeUIRoot()
			{
				if (uiRootObj != null) {
					uiRootObj.scalingStyle = UIRoot.Scaling.FixedSize;
					//if (Screen.height == 1136) {
					if (QE.Common.Utility.IsIPhone5PortraitDisplay()) {
						uiRootObj.manualHeight = 1136;
					} else {
						uiRootObj.manualHeight = 960;
					}
				}
				if (camAspectObj != null) {
					//if (Screen.height == 1136) {
					if (QE.Common.Utility.IsIPhone5PortraitDisplay()) {
						camAspectObj.displayHeight = 1136;
					} else {
						camAspectObj.displayHeight = 960;
					}
				}
			}
		
			void Update () {
		#if UNITY_EDITOR
				if (bChange) {
					changeUIRoot();
					bChange = false;
				}
		#endif
			}
			
		}
//---------------------------------------------------------------------------------
	}	// Common
		
}	// QE
