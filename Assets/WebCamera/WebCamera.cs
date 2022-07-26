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

    // �J�����̑I��
    int selectCamera = 0;

    private void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices; //�ڑ�����Ă���E�F�u�J�������擾

        foreach (WebCamDevice device in devices)
        {
            Debug.Log("WebCamDevice " + device.name);
        }

        // WebCamTexture�̃C���X�^���X�𐶐�
        webCam = new WebCamTexture();
        //RawImage�̃e�N�X�`����WebCamTexture�̃C���X�^���X��ݒ�
        RawImage.texture = webCam;
        //�J�����\���J�n
        webCam.Play();
    }

    public void OnClick()
    {
        // �J�����̎擾
        WebCamDevice[] webCamDevice = WebCamTexture.devices;

        // �J������1�̎��͖�����
        if (webCamDevice.Length <= 1) return;

        // �J�����̐؂�ւ�
        selectCamera++;
        if (selectCamera >= webCamDevice.Length) selectCamera = 0;
        this.webCam.Stop();
        this.webCam = new WebCamTexture(webCamDevice[selectCamera].name,INPUT_SIZE, INPUT_SIZE, FPS);
        this.RawImage.texture = this.webCam;
        this.webCam.Play();
    }
}
