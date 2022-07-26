using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamera : MonoBehaviour
{
    public RawImage RawImage;
    WebCamTexture webCam;

    public void OnClick()
    {
        WebCamDevice[] devices = WebCamTexture.devices; //接続されているウェブカメラを取得

        foreach (WebCamDevice device in devices)
        {
            Debug.Log("WebCamDevice " + device.name);
        }

        // WebCamTextureのインスタンスを生成
        webCam = new WebCamTexture();
        //RawImageのテクスチャにWebCamTextureのインスタンスを設定
        RawImage.texture = webCam;
        //カメラ表示開始
        webCam.Play();
    }
}
