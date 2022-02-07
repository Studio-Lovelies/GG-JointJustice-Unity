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

    private void Awake()
    {
        var backButton = GetComponentInChildren<MenuItem>();
        backButton.OnItemSelect += () => _previewImage.color = Color.black;
        backButton.OnItemDeselect += () => _previewImage.color = Color.white;

        foreach (var @case in _cases)
        {
            var menuItem = Instantiate(_buttonPrefab, _buttonContainer);
            menuItem.Text = @case.Name;
            menuItem.OnItemSelect += () => _previewImage.sprite = @case.PreviewImage;
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
