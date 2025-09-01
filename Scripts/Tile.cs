using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int coord;
    private SpriteRenderer sr;
    private Color defaultColor;  // store the original color

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color; // save the default color
    }

    // Set tile color manually
    public void SetColor(Color c)
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        sr.color = c;
    }

    // Highlight tile for legal move
    public void Highlight(bool on)
    {
        SetColor(on ? Color.green : defaultColor);
    }

    private void OnMouseDown()
    {
        if (BoardManager.Instance != null)
            BoardManager.Instance.OnTileClicked(this);
    }
}
