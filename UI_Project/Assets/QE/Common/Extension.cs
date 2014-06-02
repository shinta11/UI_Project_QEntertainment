using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//***************************************************
/// <summary>
/// 拡張クラス
/// </summary>
//***************************************************
public static class Extension 
{

  //***************************************************
  /// <summary>
  /// ローカルのX回転のみさせる
  /// </summary>
  //***************************************************
  public static void SetLocalRotationX( this Transform transform, float x )
  {
    Vector3 rot = transform.localRotation.eulerAngles;
    rot.x = x;
    transform.localRotation = Quaternion.Euler( rot );
  }

  //***************************************************
  /// <summary>
  /// ローカルのY回転のみさせる
  /// </summary>
  //***************************************************
  public static void SetLocalRotationY( this Transform transform, float y )
  {
    Vector3 rot = transform.localRotation.eulerAngles;
    rot.y = y;
    transform.localRotation = Quaternion.Euler( rot );
  }


  //***************************************************
  /// <summary>
  /// ローカルのY回転のみさせる
  /// </summary>
  //***************************************************
  public static void AddLocalRotationY( this Transform transform, float y )
  {
    Vector3 rot = transform.localRotation.eulerAngles;
    rot.y += y;
    transform.localRotation = Quaternion.Euler( rot );
  }

  //***************************************************
  /// <summary>
  /// ローカルのz回転のみさせる
  /// </summary>
  //***************************************************
  public static void SetLocalRotationZ( this Transform transform, float z )
  {
    Vector3 rot = transform.localRotation.eulerAngles;
    rot.z = z;
    transform.localRotation = Quaternion.Euler( rot );
  }


  //***************************************************
  /// <summary>
  /// ローカルのz回転のみさせる
  /// </summary>
  //***************************************************
  public static void AddLocalRotationZ( this Transform transform, float z )
  {
    Vector3 rot = transform.localRotation.eulerAngles;
    rot.z += z;
    transform.localRotation = Quaternion.Euler( rot );
  }

  //***************************************************
  /// <summary>
  /// ローカルのx座標のみ移動させる
  /// </summary>
  //***************************************************
  public static void SetLocalPositionX( this Transform transform, float x )
  {
    Vector3 pos = transform.localPosition;
    pos.x = x;
    transform.localPosition = pos;
  }

  //***************************************************
  /// <summary>
  /// ローカルのY座標のみ移動させる
  /// </summary>
  //***************************************************
  public static void SetLocalPositionY( this Transform transform, float y )
  {
    Vector3 pos = transform.localPosition;
    pos.y = y;
    transform.localPosition = pos;
  }

  //***************************************************
  /// <summary>
  /// グローバルのY座標のみ移動させる
  /// </summary>
  //***************************************************
  public static void SetPositionY( this Transform transform, float y )
  {
    Vector3 pos = transform.position;
    pos.y = y;
    transform.position = pos;
  }


  //***************************************************
  /// <summary>
  /// ローカルのz座標のみ移動させる
  /// </summary>
  //***************************************************
  public static void SetLocalPositionZ( this Transform transform, float z )
  {
    Vector3 pos = transform.localPosition;
    pos.z = z;
    transform.localPosition = pos;
  }

  //*******************************************************
  /// <summary>
  /// 移動アニメーション
  /// </summary>
  /// <param name="endPos"> 終了位置 </param>
  /// <param name="moveTime"> 移動時間 </param>
  //*******************************************************
  public static IEnumerator MovePosition( this Transform transform, Vector3 endPos, float moveTime )
  {
    float time = 0.0f;
    float curRate = 0.0f;
    Vector3 startPos = transform.position;
    while( true )
    {
      time += Time.deltaTime;
      if( time >= moveTime ) time = moveTime;
      curRate = time / moveTime;
      transform.position = Vector3.Lerp( startPos, endPos, curRate );
      yield return 0;
      if( time >= moveTime )
      {
        break;
      }
    }
  }

  //*******************************************************
  /// <summary>
  /// 移動アニメーション(なめらか)
  /// </summary>
  /// <param name="endPos"> 終了位置 </param>
  /// <param name="moveTime"> 移動時間 </param>
  //*******************************************************
  public static IEnumerator SMovePosition( this Transform transform, Vector3 endPos, float moveTime )
  {
    float time = 0.0f;
    float curRate = 0.0f;
    Vector3 startPos = transform.position;
    while( true )
    {
      time += Time.deltaTime;
      if( time >= moveTime ) time = moveTime;
      curRate = time / moveTime;
      transform.position = Vector3.Slerp( startPos, endPos, curRate );
      yield return 0;
      if( time >= moveTime )
      {
        break;
      }
    }
  }


  /// <summary>
  /// Dictionary<>をカンマ区切りの文字列に変換
  /// </summary>
  /// <returns> 変換後の文字列 </returns>
  /// <param name="dict"> 変換元のDictionary(this) </param>
  public static string ConvertToString( this IDictionary<string,object> dict )
  {
    string result = "";
    if( dict != null )
    {
      foreach( var pair in dict )
      {
        if( pair.Value is Dictionary<string,object> )
        {
          result += ( string.Format("{0}={{ {1} }}", (pair.Value as Dictionary<string,object>).ConvertToString() ) );
        }
        else if( pair.Value.GetType().IsGenericType )
        {
          result += ( string.Format("{0}={1}", pair.Key, (pair.Value as IList).ConvertToString() ) );
        }
        else
        {
          result += ( string.Format("{0}={1}", pair.Key, pair.Value.ToString() ) );
        }
        result += "\n";
      }
    }
    return result;
  }

  /// <summary>
  /// リストをカンマ区切りの文字列に変換
  /// </summary>
  /// <returns> 変換後の文字列 </returns>
  /// <param name="list"> 変換元のリスト(this) </param>
  private static string ConvertToString( this IList list )
  {
    string result = "[";
    bool isNewLine = false;
    if( list != null )
    {
      for( int i = 0; i < list.Count; i++ )
      {
        if( list[i] is Dictionary<string,object> )
        {
          isNewLine = true;
          result += "\n{\n" + (list[i] as Dictionary<string,object>).ConvertToString() + "},";
        }
        else
        {
          isNewLine = false;
          result += (list[i].ToString() + ",");
        }
      }
      result = result.Substring(0, result.Length-1);
      if( isNewLine ) result += "\n";
      result += "]";
    }
    return result;
  }
}
