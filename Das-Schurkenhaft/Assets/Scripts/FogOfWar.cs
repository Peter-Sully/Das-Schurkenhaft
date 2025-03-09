using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour
{
    public Tilemap fogTilemap;
    public Tilemap wallsTilemap;
    public TileBase fogTile;
    public float revealRadius = 5f;
    public Transform player;

    private BoundsInt mapBounds;

    void Start()
    {
        if (wallsTilemap == null)
        {
            Debug.LogError("Walls tilemap not assigned!");
            return;
        }
        // Use the walls tilemap bounds to determine the fog area.
        mapBounds = wallsTilemap.cellBounds;
        Debug.Log("Using walls tilemap bounds: " + mapBounds);

        // Fill the fogTilemap with fog tiles based on the walls tilemap bounds.
        foreach (Vector3Int pos in mapBounds.allPositionsWithin)
        {
            fogTilemap.SetTile(pos, fogTile);
        }
    }

    public void RevealFog()
    {
        if (player == null)
        {
            Debug.LogWarning("Player transform not assigned!");
            return;
        }
        // Convert the player's world position to a cell position on the fog tilemap.
        Vector3Int center = fogTilemap.WorldToCell(player.position);
        int radius = Mathf.CeilToInt(revealRadius);

        // Iterate over a square area centered on the player.
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                // Only clear fog within a circular radius.
                if (Vector2.Distance(new Vector2(x, y), Vector2.zero) <= revealRadius)
                {
                    Vector3Int tilePos = new Vector3Int(center.x + x, center.y + y, center.z);
                    fogTilemap.SetTile(tilePos, null);
                }
            }
        }
    }

    void Update()
    {
        // Reveal fog continuously as the player moves.
        RevealFog();
    }
}
