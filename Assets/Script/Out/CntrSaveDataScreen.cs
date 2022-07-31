using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts._System;

public class CntrSaveDataScreen : MonoBehaviour
{
    
    [SerializeField] public Dropdown[] dropdowns;
    [SerializeField] public Toggle fullScreen;
    public enum EDisplays
    {
        Display1,
        Display2,
        Display3,
        Display4,
    };
    public enum EDropDown
    {
        Wall,
        Left,
        Right,
    };

    Dictionary<EDropDown, EDisplays> displayNum = new Dictionary<EDropDown, EDisplays>()
        {
            {EDropDown.Wall, EDisplays.Display2 },
            {EDropDown.Left, EDisplays.Display3 },
            {EDropDown.Right, EDisplays.Display4 },
        };

    
    void Awake()
    {
        dropdowns[(int)EDropDown.Wall].value = (int)EDisplays.Display2;
        dropdowns[(int)EDropDown.Left].value = (int)EDisplays.Display3;
        dropdowns[(int)EDropDown.Right].value = (int)EDisplays.Display4;
    }
    private void Start()
    {
        Debug.Log("ディスプレイの数:" + Display.displays.Length);
        if (Display.displays.Length >= 2)
        {
            foreach (var monitor in Display.displays)
            {
                monitor.Activate();
            }
        }
    }

    public EDisplays GetDisplayNumber(EDropDown _prj)
    {
        return (EDisplays)displayNum[_prj];
    }
    public void ChangedWall(int _display)
    {
        /*
        displayNum[EDropDown.Wall] = (EDisplays)(_display);
        var savedataTotal = this.GetComponent<SaveDataAdmin>().SaveDataTotal;
        savedataTotal.cSettingScreen.displayLocation[(int)EDropDown.Wall] = displayNum[EDropDown.Wall];
        */
    }
    public void ChangedLeft(int _display)
    {
        /*
        displayNum[EDropDown.Left] = (EDisplays)(_display);
        var savedataTotal = this.GetComponent<SaveDataAdmin>().SaveDataTotal;
        savedataTotal.cSettingScreen.displayLocation[(int)EDropDown.Left] = displayNum[EDropDown.Left];
        */
    }
    public void ChangedRight(int _display)
    {
        /*
        displayNum[EDropDown.Right] = (EDisplays)(_display);
        var savedataTotal = this.GetComponent<SaveDataAdmin>().SaveDataTotal;
        savedataTotal.cSettingScreen.displayLocation[(int)EDropDown.Right] = displayNum[EDropDown.Right];
        */
    }
    public void ChangeIsFullScreen()
    {
        /*
        var savedataTotal = this.GetComponent<SaveDataAdmin>().SaveDataTotal;
        savedataTotal.cSettingScreen.IsFullScreen = fullScreen.isOn;*/
    }

    [System.Serializable]
    public class CSettingScreen
    {
        EDropDown[] displayLocation = new EDropDown[3];
    }

    public CSettingScreen cSettingScreen;
    public void UpdateFromLoadedData()
    {
        /*
        var savedataTotal = this.GetComponent<SaveDataAdmin>().SaveDataTotal;
        //SaveDataAdmin.CSaveDataTotal.CSettingScreen savedata = new SaveDataAdmin.CSaveDataTotal.CSettingScreen();
        var savedata = savedataTotal.cSettingScreen;
        for (int d = 0; d < 3; d++)
        {
            if (savedata.displayLocation[d] == EDisplays.Display1)
            {
                savedata.displayLocation[d] = EDisplays.Display2;
            }
        }

        ChangedWall((int)savedata.displayLocation[(int)EDropDown.Wall]);
        ChangedLeft((int)savedata.displayLocation[(int)EDropDown.Left]);
        ChangedRight((int)savedata.displayLocation[(int)EDropDown.Right]);
        dropdowns[(int)EDropDown.Wall].value = (int)savedata.displayLocation[(int)EDropDown.Wall];
        dropdowns[(int)EDropDown.Left].value = (int)savedata.displayLocation[(int)EDropDown.Left];
        dropdowns[(int)EDropDown.Right].value = (int)savedata.displayLocation[(int)EDropDown.Right];

        fullScreen.isOn = savedata.IsFullScreen;
        */
    }
    
}
