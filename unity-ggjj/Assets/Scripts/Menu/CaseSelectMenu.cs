using UnityEngine;
using SceneLoading;
using UnityEngine.UI;

public class CaseSelectMenu : MonoBehaviour
{
    [SerializeField] private MenuItem _buttonPrefab;
    [SerializeField] private Transform _buttonContainer;
    [SerializeField] private Image _previewImage;
    [SerializeField] private ChapterSelectMenu _chapterSelectMenu;
    [SerializeField] private AudioController _audioController;
    [SerializeField] private AudioClip _buttonSelectAudioClip;
    [SerializeField] private GameLoader _gameLoader;
    [SerializeField] private AudioClip _startGameSound;
    [SerializeField] private NarrativeCase[] _cases;

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

        foreach (var narrativeCase in _cases)
        {
            var menuItem = Instantiate(_buttonPrefab, _buttonContainer);
            menuItem.Text = narrativeCase.Name;
            menuItem.OnItemSelect.AddListener(() => _previewImage.sprite = narrativeCase.PreviewImage);
            var menuOpener = menuItem.GetComponent<MenuOpener>();
            menuOpener.MenuToOpen = _chapterSelectMenu.Menu;
            
            menuItem.OnItemSelect.AddListener(() => _audioController.PlaySfx(_buttonSelectAudioClip));
            ((Button)menuItem.Selectable).onClick.AddListener(() => StartGameOrShowChapterSelectionMenu(narrativeCase, menuOpener));
        }
    }

    /// <summary>
    /// Method assigned to buttons in the case select menu
    /// to make them either start the game, or open and initialised
    ///  the chapter select menu with the correct number of chapter buttons.
    /// </summary>
    /// <param name="narrativeCase">The NarrativeCase assign to this button</param>
    /// <param name="menuOpener">The menu opener attached to this button</param>
    private void StartGameOrShowChapterSelectionMenu(NarrativeCase narrativeCase, MenuOpener menuOpener)
    {
        if (narrativeCase.Chapters.Length == 1)
        {
            _audioController.PlaySfx(_startGameSound);
            _gameLoader.NarrativeScriptTextAsset = narrativeCase.Chapters[0];
            _gameLoader.StartGame();
            return;
        }
        
        menuOpener.OpenMenu();
        _chapterSelectMenu.Initialise(narrativeCase.Chapters, menuOpener);
    }
}
