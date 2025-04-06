using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ResolutionDropdownV3 : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    void Start()
    {
        List<string> customResolutions = new List<string>()
        {
            "1280 x 720",
            "1920 x 1080",
 
        };

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(customResolutions);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        // Optional: Set current resolution index
        string current = Screen.currentResolution.width + " x " + Screen.currentResolution.height;
        int index = customResolutions.IndexOf(current);
        if (index != -1)
            resolutionDropdown.value = index;
    }

    void SetResolution(int index)
    {
        string[] dimensions = resolutionDropdown.options[index].text.Split('x');
        if (dimensions.Length == 2)
        {
            int width = int.Parse(dimensions[0].Trim());
            int height = int.Parse(dimensions[1].Trim());
            Screen.SetResolution(width, height, Screen.fullScreen);
            Debug.Log("Resolution set to: " + width + "x" + height);
        }
    }
}
