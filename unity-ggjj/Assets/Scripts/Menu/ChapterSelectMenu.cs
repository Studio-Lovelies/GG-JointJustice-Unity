using System.Collections.Generic;
using SceneLoading;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelectMenu : MonoBehaviour
{
    [field: SerializeField] public Menu Menu { get; private set; }
    [SerializeField] private Transform _buttonContainer;
    [SerializeField] private MenuItem _menuItemPrefab;
    [SerializeField] private Button _backButton;
    [SerializeField] private AudioController _audioController;
    [SerializeField] private AudioClip _buttonSelectAudioClip;
    [SerializeField] private GameLoader _gameLoader;
    [SerializeField] private AudioClip _startGameSound;

    private readonly List<MenuItem> _menuItems = new List<MenuItem>();
    
    /// <summary>
    /// Destroy existing buttons and instantiate buttons equal to number of chapters given.
    /// Subscribed back button to provided menu opener so this menu can be closed properly.
    /// </summary>
    /// <param name="chapters">Array of chapters to create buttons for</param>
    /// <param name="menuOpener">A menu opener to subscribe the back button to</param>
    public void Initialise(TextAsset[] chapters, MenuOpener menuOpener)
    {
        foreach (var menuItem in _menuItems)
        {
            Destroy(menuItem.gameObject);
        }
        _menuItems.Clear();
        
        for (var i = 1; i < chapters.Length + 1; i++)
        {
            var menuItem = Instantiate(_menuItemPrefab, _buttonContainer);
            if (i == 1)
            {
                menuItem.Selectable.Select();
            }
            menuItem.Text = $"Part {i}";
            menuItem.OnItemSelect.AddListener(() => _audioController.PlaySfx(_buttonSelectAudioClip));
            var chapterIndex = i - 1;
            ((Button)menuItem.Selectable).onClick.AddListener(() => StartGame(chapters[chapterIndex]));
            _menuItems.Add(menuItem);
        }

        _backButton.onClick.RemoveAllListeners();
        _backButton.onClick.AddListener(menuOpener.CloseMenu);
    }

    private void StartGame(TextAsset story)
    {
        _audioController.PlaySfx(_startGameSound);
        _gameLoader.NarrativeScriptTextAsset = story;
        _gameLoader.StartGame();
    }
}
