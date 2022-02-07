using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelectMenu : MonoBehaviour
{
    [field: SerializeField] public Menu Menu { get; private set; }
    [SerializeField] private Transform _buttonContainer;
    [SerializeField] private MenuItem _menuItemPrefab;
    [SerializeField] private Button _backButton;

    private readonly List<MenuItem> _menuItems = new List<MenuItem>();
    
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
            _menuItems.Add(menuItem);
        }
        
        _backButton.onClick.RemoveAllListeners();
        _backButton.onClick.AddListener(menuOpener.CloseMenu);
    }
}
