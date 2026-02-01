using UnityEngine;
using System.Collections.Generic;

[System.Serializable] 
public class WallRange 
{
    public Vector2Int inicio;
    public Vector2Int fin;
}

[CreateAssetMenu(fileName = "NuevoNivel", menuName = "Encajonado/Nivel")]
public class LevelData : ScriptableObject
{
    public int width = 7;
    public int height = 10;
    public Vector2Int playerSpawn;
    public List<Vector2Int> winPositions = new List<Vector2Int>();
    public List<Vector2Int> boxPositions = new List<Vector2Int>();
    public List<Vector2Int> wallPositions = new List<Vector2Int>();
    public List<WallRange> wallRanges = new List<WallRange>(); 
}