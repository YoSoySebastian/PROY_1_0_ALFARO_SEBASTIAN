using UnityEngine;

public class BoxController : MonoBehaviour
{
    public Vector2Int gridPos;
    private GridManager gridManager;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        gridPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }

    public bool TryMove(Vector2Int direction)
    {
        Vector2Int targetPos = gridPos + direction;
        LevelData data = gridManager.ObtenerNivelActual();

        if (targetPos.x < 0 || targetPos.x >= data.width || targetPos.y < 0 || targetPos.y >= data.height)
            return false;

        Collider2D hit = Physics2D.OverlapPoint((Vector2)targetPos);
        
        if (hit == null || hit.CompareTag("WinTarget"))
        {
            gridPos = targetPos;
            transform.position = new Vector3(gridPos.x, gridPos.y, 0);
            gridManager.ComprobarVictoria();
            return true;
        }
        return false;
    }
}