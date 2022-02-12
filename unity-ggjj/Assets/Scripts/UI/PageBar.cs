using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageBar : MonoBehaviour
{
    [Tooltip("An object to represent a page dot")]
    [SerializeField] private Image _pageDot;
    
    private readonly List<Image> _pageDots = new List<Image>();

    /// <summary>
    /// Creates or deletes Image instances inside <see cref="_pageDots"/> until the length matches <see cref="pageCount"/>
    /// </summary>
    /// <param name="pageCount">The number of pages to create dots for</param>
    public void SetPageCount(int pageCount)
    {
        var pageDifference =  pageCount - _pageDots.Count;
        if (pageDifference > 0)
        {
            for (int i = 0; i < pageDifference; i++)
            {
                _pageDots.Add(Instantiate(_pageDot, transform));
            }
        }
        else
        {
            for (int i = pageCount; i < _pageDots.Count; i++)
            {
                var pageDot = _pageDots[i];
                _pageDots.Remove(_pageDots[i]);
                Destroy(pageDot.gameObject);
            }
        }
    }

    /// <summary>
    /// Sets the colour of a dot that corresponds to a specific page
    /// </summary>
    /// <param name="pageIndex">The index of the page to set</param>
    public void SetPage(int pageIndex)
    {
        for (int i = 0; i < _pageDots.Count; i++)
        {
            _pageDots[i].color = i == pageIndex ? Color.white : Color.black;
        }
    }
}
