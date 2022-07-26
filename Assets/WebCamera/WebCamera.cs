using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamera : MonoBehaviour
{
    public RawImage RawImage;
    WebCamTexture webCam;

    private static int INPUT_SIZE = 256;
    private static int FPS = 30;

    // カメラの選択
    int selectCamera = 0;

    private void Start()
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

    public void OnClick()
    {
        // カメラの取得
        WebCamDevice[] webCamDevice = WebCamTexture.devices;

        // カメラが1個の時は無処理
        if (webCamDevice.Length <= 1) return;

        // カメラの切り替え
        selectCamera++;
        if (selectCamera >= webCamDevice.Length) selectCamera = 0;
        this.webCam.Stop();
        this.webCam = new WebCamTexture(webCamDevice[selectCamera].name,INPUT_SIZE, INPUT_SIZE, FPS);
        this.RawImage.texture = this.webCam;
        this.webCam.Play();
    }
}
