using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

namespace QE
{
	namespace Common
	{
//**********************************************************
/// <summary>
///	便利関数系
/// </summary>
//**********************************************************
public class Utility
{
	//**********************************************************
	/// <summary>
	/// floatでのイコールチェック
	/// </summary>
	/// <param name="a">比較する値</param>
	/// <param name="b">比較される値</param>
	/// <returns>
	///	true=イコール
	/// <br>false=違う
	/// </returns>
	//**********************************************************
	static public bool IsFloatEqual(float a, float b)
	{
		if (a >= b-Mathf.Epsilon && a <= b + Mathf.Epsilon) {
			return true;
		} else {
			return false;
		}
	}

	//**********************************************************
	/// <summary>
	/// ドキュメントパスの取得
	/// </summary>
	/// <returns>
	///	ドキュメントパス
	/// </returns>
	//**********************************************************
	static public string GetDocumentPath()
	{
		if (Application.platform == RuntimePlatform.Android) {
			return Application.temporaryCachePath;
		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			string path = Application.temporaryCachePath;
			return path;
		}
		return Application.dataPath;
	}

	//**********************************************************
	/// <summary>
	/// スマートフォンなのか？(Android or iOS)
	/// </summary>
	/// <returns>
	/// true=Android か iOS
	/// <br>false=上記以外
	/// </returns>
	//**********************************************************
	static public bool IsSmartPhone()
	{
		if ((Application.platform != RuntimePlatform.Android) && (Application.platform != RuntimePlatform.IPhonePlayer)) {
			// スマホ以外
			return false;
		} else {
			// スマホ
			return true;
		}
	}

	//**********************************************************
	/// <summary>
	/// iPhone5ディスプレイなのか？(縦専用)
	/// </summary>
	/// <returns>
	/// true=iPhone5解像度のディスプレイ(640x1136)
	/// <br>false=それ以外
	/// </returns>
	//**********************************************************
	static public bool IsIPhone5PortraitDisplay()
	{
	#if UNITY_IPHONE
		if ((Screen.width == 640) && (Screen.height == 1136)) {
			return true;
		}
	#endif
	#if UNITY_EDITOR
		// ユーザー設定を解析（やってなければ）
		loadUserSetting();
		// 解析できてるなら、その値を使う
		if (doneUserSetting) {
			return _IsIPhone5PortraitDisplay;
		} else {
			if ((Screen.width == 640) && (Screen.height == 1136)) {
				return true;
			}
		}
	#endif
		return false;
	}

	//**********************************************************
	/// <summary>
	/// デバイス情報をDictionaryで返す
	/// <br>iPhoneのみdevie_genも追加します
	/// <br>device_model		モデル名
	/// <br>device_os		OS情報
	/// <br>device_width		画面横サイズ
	/// <br>device_height	画面縦サイズ
	/// <br>device_gen		世代名(iOSだけ追加されます、PC/Androidはキー自体設定しません)
	/// <br>Kindle Fire HD 8.9の場合
	///	<br> device_model	Amazon KFJWI
	///	<br> device_os		Android OS 4.0.4 / API-15 (IMM76D/8.3.1_user_3150820)
	///	<br> device_width	1200
	///	<br> device_height	1920
	/// <br>iPhone4S(井上私物)の場合
	///	<br> device_model	iPhone
	///	<br> device_os		iPhone OS 6.1.3
	///	<br> device_width	640
	///	<br> device_height	960
	///	<br> device_gen		iPhone4S	※iPhoneのみdevice_genも追加します
	/// <br>Windows
	///	<br> device_model	Intel(R) Core(TM)2 Quad CPU Q9550 @ 2.83GHz (3568 MB)
	///	<br> device_os		Windows XP Service Pack 3 (5.1.2600)
	///	<br> device_width	1260
	///	<br> device_height	1154
	/// <br>Mac
	///	<br> device_model	Macmini5,2
	///	<br> device_os		Mac OS X 10.7.5
	///	<br> device_width	495
	///	<br> device_height	881
	/// </summary>
	/// <returns>
	/// デバイス情報のDictionary
	/// </returns>
	//**********************************************************
	static public Dictionary<string, string> SetDeviceInfoToDictionary()
	{
		Dictionary<string, string> data = new Dictionary<string, string>();
		data["device_model"]	= SystemInfo.deviceModel.ToString();
		data["device_os"]		= SystemInfo.operatingSystem.ToString();
		data["device_width"]	= Screen.width.ToString();
		data["device_height"]	= Screen.height.ToString();
	#if UNITY_IPHONE
		if	(SystemInfo.operatingSystem.Contains("iPhone")) {
			data["device_gen"]	= iPhone.generation.ToString();
		}
	#endif
		return data;
	}

	//**********************************************************
	/// <summary>
	/// デバイス情報をstringで返す
	/// <br>iPhoneのみGenも追加します
	/// <br>Kindle Fire HD 8.9での結果は以下の通りになった
	/// <br> DevName=<unknown> DevModel=Amazon KFJWI OS=Android OS 4.0.4 / API-15 (IMM76D/8.3.1_user_3150820) Width=1200 Height=1920
	/// <br>iPhone4S
	/// <br> DevName=purge の iPhone DevModel=iPhone OS=iPhone OS 6.1.3 Width=640 Height=960 Gen=iPhone4S
	/// <br>Windows
	/// <br> DevModel=Intel(R) Core(TM)2 Quad CPU Q9550 @ 2.83GHz (3568 MB) OS=Windows XP Service Pack 3 (5.1.2600) Width=1260 Height=1154
	/// <br>Mac
	/// <br> DevModel=Macmini5,2 OS=Mac OS X 10.7.5 Width=495 Height=881
	/// </summary>
	/// <returns>
	/// デバイス情報string
	/// </returns>
	//**********************************************************
	static public string GetDeviceInformation()
	{
		string str = "";
		str = str + " DevModel="+SystemInfo.deviceModel;
		str = str + " OS="+SystemInfo.operatingSystem;
		str = str + " Width="+Screen.width;
		str = str + " Height="+Screen.height;
	#if UNITY_IPHONE
		if	(SystemInfo.operatingSystem.Contains("iPhone")) {
			str = str + " Gen="+iPhone.generation;
		}
	#endif
		return str;
	}

	static bool doneUserSetting = false;			//!< usersetting.txtを読み込んで解析したか？
	static bool _IsIPhone5PortraitDisplay = false;	//!< iPhone5縦画面なのか？(usersetting.txtで設定する内容)

	//**********************************************************
	/// <summary>
	/// usersetting.txtの読み込み解析
	/// <br>Assets/usersetting.txtを読み込みに行きます
	/// <br>UnityEditorの時のみ処理します
	/// <br>一度解析後はスキップします
	/// <br>・書式
	/// <br> [キーワード] 内容
	/// <br> キーワードと内容のセパレータは、半角スペースかタブのみです
	/// <br> コメントは行頭に#があるもののみとします
	/// <br> キーワードと内容は英数字記号のみとします
	/// <br>・現在使用しているキーワード
	/// <br>[DisplayMode]
	/// <br>    内容：「iPhone5」だと画面の縦解像度が1136じゃなくでもiPhone5解像度だと判断します
	/// <br>・記述例
	/// <br># 開発用のセッティングファイル
	/// <br># ディスプレイモードの設定 iPhone5かiPhone4、それ以外はiPhone4と判断
	/// <br>[DisplayMode] iPhone5
	/// </summary>
	//**********************************************************
	static void loadUserSetting()
	{
		if (doneUserSetting) return;
#if UNITY_EDITOR
		// 初期値としてusersetting.txtが読めなかった時の値を設定します
		if ((Screen.width == 640) && (Screen.height == 1136)) {
			_IsIPhone5PortraitDisplay = true;
		} else {
			_IsIPhone5PortraitDisplay = false;
		}

		string displaySettingFile = GetDocumentPath()+"/usersetting.txt";
		if (!File.Exists(displaySettingFile)) {
			Debug.Log("Not Found:"+displaySettingFile);
		} else {
			string dat = "";
		    using (StreamReader sr = new StreamReader(displaySettingFile, Encoding.GetEncoding("UTF-8")))
			{
				dat = sr.ReadToEnd();
		    }
		    StringReader strReader;
		    strReader = new StringReader(dat);

			int loopMax = 65536;
			char[] charSeparators = new char[] {' '};
			for (int loopIndex=0; loopIndex<loopMax; loopIndex++) {
				if (strReader.Peek() < 0) {
					break;
				}

				string str = strReader.ReadLine();
				if (str == null) {
					break;
				}

				// タブをスペースに置換した後に、スペースで分割する、余計なスペースは消えるようにしています
				string str2 = str.Replace("\t", " ");
				string[] strs = str2.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
				if (strs == null) {
					continue;
				}
				if (strs.Length <= 0) {
					continue;
				}
				if (strs[0] == null) {
    	            continue;
				}
    	        if (strs[0].Contains("#")) {
	                continue;
        	    }

				if (strs[0] == "[DisplayMode]") {
					if (strs[1] == "iPhone5") {
						Debug.Log("DISPLAY MODE iPhone5");
						_IsIPhone5PortraitDisplay = true;
					} else {
						_IsIPhone5PortraitDisplay = false;
					}
				}


			}

		}
#endif
		doneUserSetting = true;
	}

	//**********************************************************
	/// <summary>
	/// WebViewがサポートされているか？
	/// </summary>
	/// <returns>
	///	true=サポートされている
	/// <br>false=サポートされていない
	/// </returns>
	//**********************************************************
	static public bool IsSupportWebView()
	{
		if (SystemInfo.operatingSystem.Contains("iPhone")) {
			return true;
		}
		if (SystemInfo.operatingSystem.Contains("Android")) {
			return true;
		}
		return false;
	}

}
//---------------------------------------------------------------------------------
	}	// Common
		
}	// QE
