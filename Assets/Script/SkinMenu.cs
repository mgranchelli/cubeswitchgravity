using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinMenu : MonoBehaviour
{
    public GameObject cube;
    public GameObject ColorSection;
    public GameObject SkinSection;
    public Sprite[] skins;
    
    public static readonly Color32 RedColor = new Color32(226, 0, 0, 255);
    public static readonly Color32 GreenColor = new Color32(1, 171, 0, 255);
    public static readonly Color32 YellowColor = new Color32(252, 228, 90, 255);
    public static readonly Color32 LightBlueColor = new Color32(0, 156, 253, 255);
    public static readonly Color32 OrangeColor = new Color32(250, 142, 0, 255);
    public static readonly Color32 PurpleColor = new Color32(192, 52, 217, 255);

    // Start is called before the first frame update
    void Start()
    {
        SetSkinsAndColors(PlayerPrefs.GetString("skin"), PlayerPrefs.GetString("color_skin"));
    }

    public void SetSkin1()
    {
        SetSkinColors(skins[0]);
    }

    public void SetSkin2()
    {
        SetSkinColors(skins[1]);
    }

    public void SetSkin3()
    {
        SetSkinColors(skins[2]);
    }

    public void SetSkin4()
    {
        SetSkinColors(skins[3]);
    }

    public void SetSkin5()
    {
        SetSkinColors(skins[4]);
    }

    public void SetSkin6()
    {
     
        SetSkinColors(skins[5]);
    }

    public void SetRedColor()
    {
        SetColorSkins(RedColor, "Red");
    }

    public void SetGreenColor()
    {
        SetColorSkins(GreenColor, "Green");
    }

    public void SetYellowColor()
    {
        SetColorSkins(YellowColor, "Yellow");
    }

    public void SetLightBlueColor()
    {
        SetColorSkins(LightBlueColor, "Lightblue");
    }

    public void SetOrangeColor()
    {
        SetColorSkins(OrangeColor, "Orange");
    }

    public void SetPurpleColor()
    {
        SetColorSkins(PurpleColor, "Purple");
    }

    private void SetColorSkins(Color32 color, string color_name)
    {
        PlayerPrefs.SetString("color_skin", color_name);
        cube.GetComponent<Image>().color = color;
        Image[] Skins = SkinSection.GetComponentsInChildren<Image>();
        for(int i = 1; i < Skins.Length; i++)
        {
            Skins[i].color = color;
        }
        
    }

    private void SetSkinColors(Sprite skin)
    {
        PlayerPrefs.SetString("skin", skin.name);
        cube.GetComponent<Image>().sprite = skin;
        Image[] Colors = ColorSection.GetComponentsInChildren<Image>();
        for (int i = 1; i < Colors.Length; i++)
        {
            Colors[i].sprite = skin;
        }

    }

    private void SetSkinsAndColors(string skin, string color_skin)
    {
        switch (skin)
        {
            case "Skin1":
                SetSkinColors(skins[0]);
                break;
            case "Skin2":
                SetSkinColors(skins[1]);
                break;
            case "Skin3":
                SetSkinColors(skins[2]);
                break;
            case "Skin4":
                SetSkinColors(skins[3]);
                break;
            case "Skin5":
                SetSkinColors(skins[4]);
                break;
            case "Skin6":
                SetSkinColors(skins[5]);
                break;

            default:
                SetSkinColors(skins[0]);
                break;

        }

        switch (color_skin)
        {
            case "Red":
                SetColorSkins(RedColor, "Red");
                break;
            case "Green":
                SetColorSkins(GreenColor, "Green");
                break;
            case "Yellow":
                SetColorSkins(YellowColor, "Yellow");
                break;
            case "Lightblue":
                SetColorSkins(LightBlueColor, "Lightblue");
                break;
            case "Orange":
                SetColorSkins(OrangeColor, "Orange");
                break;
            case "Purple":
                SetColorSkins(PurpleColor, "Purple");
                break;

            default:
                SetColorSkins(RedColor, "Red");
                break;

        }

    }
}
