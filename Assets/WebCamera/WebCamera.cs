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
}
