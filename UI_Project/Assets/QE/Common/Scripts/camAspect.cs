using UnityEngine;
using System.Collections;

namespace QE
{
	namespace Common
	{
//**********************************************************
/// <summary>
/// Cameraアスペクトを調整します
/// <br>基準となる画面サイズ(displayWidth,displayHeight)と現在の画面サイズから比率を求めて、カメラのRectを調整して現在の画面に合うようにします
/// </summary>
//**********************************************************
public class camAspect : MonoBehaviour
{
	public Rect camRect;
	public Rect orgRect;
	public Rect tmpRect;
	public float baseAspect;
	public float nowAspect;
	public float changeAspect;
	public float height;
	public float width;

	public float displayWidth = 640.0f;		//!< 基準となる画面縦サイズ
	public float displayHeight = 960.0f;	//!< 基準となる画面横サイズ

	void Awake () {
		calcAspect();
	}

	//**********************************************************
	/// <summary>
	/// ゲームは横画面モードか？
	/// </summary>
	/// <returns>
	/// true=横モード<br>
	/// false=それ以外（恐らく縦）
	/// </returns>
	//**********************************************************
	bool isGameLandscape()
	{
        if ( Screen.orientation == ScreenOrientation.Landscape || Screen.orientation == ScreenOrientation.LandscapeRight || Screen.orientation == ScreenOrientation.LandscapeLeft ){
            return true;
        } else if ( Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown ){
            return false;
        }
        Debug.LogError("Unknown ScreenOrientation.");
        return false;
    }

	//**********************************************************
	/// <summary>
	/// 基準の画面サイズと現在の画面サイズから比率を計算してCameraのRectを設定します
	/// </summary>
	//**********************************************************
	void calcAspect () {
		changeAspect = 1.0f;
		Camera cam = gameObject.GetComponent<Camera>();
		if (isGameLandscape()) {
            cam.rect = new Rect(cam.rect.x, cam.rect.y, cam.rect.width, 0.5f);
		}

		orgRect = cam.rect;
		baseAspect = displayWidth / displayHeight;
		height = (float)Screen.height;
		width = (float)Screen.width;
		nowAspect = width/height;
  
  		if (!Mathf.Approximately(baseAspect, nowAspect)) {
			if (baseAspect>nowAspect) {   
				changeAspect = nowAspect/baseAspect;
				tmpRect = new Rect(0, (1-changeAspect)*0.5f, 1, changeAspect);
				camRect = new Rect(
								tmpRect.x, tmpRect.y, 
								orgRect.width * tmpRect.width, orgRect.height * tmpRect.height);
				cam.rect = camRect;
			} else {
				changeAspect = baseAspect/nowAspect;
				tmpRect = new Rect((1-changeAspect)*0.5f, 0, changeAspect, 1);
				camRect = new Rect(
								tmpRect.x, tmpRect.y, 
								orgRect.width * tmpRect.width, orgRect.height * tmpRect.height);
				cam.rect = camRect;
			}
		} else {
			camRect = orgRect;
			cam.rect = camRect;
		}

	}

}

//---------------------------------------------------------------------------------
	}	// Dialog
		
}	// QE
