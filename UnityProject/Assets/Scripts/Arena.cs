using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Arena : MonoBehaviour 
{
    [SerializeField]
    private List<Color> tileColorKeys;

    [SerializeField]
    private List<GameObject> tiles;

    public void Start()
    {
        InitFromTexture(Resources.Load("Maps/TestMap1") as Texture2D);
    }
    
    public void InitFromTexture(Texture2D texture)
    {
        Vector2 cellDimensions = GetComponent<GridLayoutGroup>().cellSize;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellDimensions.x * texture.width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellDimensions.y * texture.height);

        foreach(Color color in texture.GetPixels())
        {
            int tileIndex = tileColorKeys.IndexOf(color);
            GameObject tile = Instantiate(tiles[tileIndex]) as GameObject;

            RectTransform tileRectTransform = tile.GetComponent<RectTransform>();
            tileRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellDimensions.x);
            tileRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellDimensions.y);
            tile.transform.SetParent(this.transform);

            BoxCollider2D boxCollider = tile.GetComponent<BoxCollider2D>();
            if(boxCollider != null)
            {
                boxCollider.size = cellDimensions;
            }
        }
    }
}
