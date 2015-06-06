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
        InitFromTexture(Resources.Load("Maps/TestMap3") as Texture2D);
    }
    
    public void InitFromTexture(Texture2D texture)
    {
        Vector2 cellDimensions = new Vector2(35, 35);
        Vector2 mapDimensions = new Vector2(texture.width, texture.height);

        int currentIndex = 0;
        Color[] colors = texture.GetPixels();
        for (int i = 0; i < mapDimensions.x; i++)
        {
            for(int j = 0; j < mapDimensions.y; j++)
            {
                int tileIndex = tileColorKeys.IndexOf(colors[currentIndex]);
                GameObject tile = Instantiate(tiles[tileIndex]) as GameObject;

                tile.transform.SetParent(this.transform);
                tile.transform.localPosition = new Vector3(-5.25f + j * 0.35f, 5.25f - i * 0.35f, 0f);

                currentIndex++;
            }
        }

        /*

            foreach (Color color in texture.GetPixels())
            {
                int tileIndex = tileColorKeys.IndexOf(color);
                GameObject tile = Instantiate(tiles[tileIndex]) as GameObject;



                RectTransform tileRectTransform = tile.GetComponent<RectTransform>();
                tileRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellDimensions.x);
                tileRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellDimensions.y);
                tile.transform.SetParent(this.transform);

                BoxCollider2D boxCollider = tile.GetComponent<BoxCollider2D>();
                if (boxCollider != null)
                {
                    boxCollider.size = cellDimensions;
                }
            }*/
    }
}
