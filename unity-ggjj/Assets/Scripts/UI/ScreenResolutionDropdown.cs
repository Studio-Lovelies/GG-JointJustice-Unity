using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class ScreenResolutionDropdown : MonoBehaviour
{
    private TMP_Dropdown _dropdown;

    private void Awake()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
    }

    private void OnEnable()
    {
        var options = Screen.resolutions.Select(res => $"{res.width}x{res.height}").ToList();
        _dropdown.ClearOptions();
        _dropdown.AddOptions(options);

        _dropdown.value = Screen.resolutions.Select((res, index) => new {res, index})
            .OrderBy(x => Mathf.Abs(x.res.width - Screen.currentResolution.width) + Mathf.Abs(x.res.height - Screen.currentResolution.height))
            .First().index;
        
        _dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        
        _dropdown.RefreshShownValue();
    }

    private static void OnDropdownValueChanged(int currentlySelectedIndex)
    {   
        var currentlySelectedResolution = Screen.resolutions[currentlySelectedIndex];
        Screen.SetResolution(currentlySelectedResolution.width, currentlySelectedResolution.height, Screen.fullScreen);
    }
}
