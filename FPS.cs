using UnityEngine;
using TMPro;

public class FPSSetter : MonoBehaviour
{
    public TMP_Dropdown fpsDropdown;

    void Start()
    {
        fpsDropdown.onValueChanged.AddListener(SetFPS);
    }

    void SetFPS(int index)
    {
        switch (index)
        {
            case 0: Application.targetFrameRate = 30; break;
            case 1: Application.targetFrameRate = 60; break;
            case 2: Application.targetFrameRate = 90; break;
            case 3: Application.targetFrameRate = -1; break; // Unlimited
        }
    }
}
