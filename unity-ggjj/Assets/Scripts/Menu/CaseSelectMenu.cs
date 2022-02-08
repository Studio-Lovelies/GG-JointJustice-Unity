using UnityEngine;
using System;
using UnityEngine.UI;

public class CaseSelectMenu : MonoBehaviour
{
    [SerializeField] private MenuItem _buttonPrefab;
    [SerializeField] private Transform _buttonContainer;
    [SerializeField] private Image _previewImage;
    [SerializeField] private ChapterSelectMenu _chapterSelectMenu;
    [SerializeField] private Case[] _cases;

    /// <summary>
    /// On Awake:
    /// Subscribe back button to show and hide preview
    /// Instantiate buttons equal to number of cases
    /// Set button text
    /// Subscribe instantiated buttons to update preview image
    /// Add onClick logic to each button: either start the game or open the chapter select menu
    /// </summary>
    private void Awake()
    {
        var backButton = GetComponentInChildren<MenuItem>();
        backButton.OnItemSelect.AddListener(() => _previewImage.color = Color.black);
        backButton.OnItemDeselect.AddListener(() => _previewImage.color = Color.white);

        foreach (var @case in _cases)
        {
            var menuItem = Instantiate(_buttonPrefab, _buttonContainer);
            menuItem.Text = @case.Name;
            menuItem.OnItemSelect.AddListener(() => _previewImage.sprite = @case.PreviewImage);
            var menuOpener = menuItem.GetComponent<MenuOpener>();
            menuOpener.MenuToOpen = _chapterSelectMenu.Menu;
            ((Button)menuItem.Selectable).onClick.AddListener(() =>
            {
                if (@case.Chapters.Length == 1)
                {
                    // No need to show chapters, put logic for starting game here
                }
                else
                {
                    menuOpener.OpenMenu();
                    _chapterSelectMenu.Initialise(@case.Chapters, menuOpener);
                }
            });
        }
    }
}

[Serializable]
public struct Case
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite PreviewImage { get; private set; }
    [field: SerializeField] public TextAsset[] Chapters { get; private set; }
}
