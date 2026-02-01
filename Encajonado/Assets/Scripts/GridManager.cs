using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GridManager : MonoBehaviour
{
    [Header("Configuración")]
    public List<LevelData> niveles;
    private int indiceNivelActual = 0;

    [Header("Prefabs")]
    public GameObject cellPrefab;
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject wallPrefab;
    public GameObject winPrefab;

    [Header("UI")]
    public TextMeshProUGUI textoPasosGlobal;
    public TextMeshProUGUI textoRecordUI;

    private List<GameObject> instanciasActuales = new List<GameObject>();
    private string recordParaMostrar = "";

    void Start()
    {
        if (niveles.Count > 0) CargarNivel(indiceNivelActual);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) CargarNivel(indiceNivelActual);

        if (!string.IsNullOrEmpty(recordParaMostrar))
        {
            if (textoRecordUI != null) 
            {
                textoRecordUI.text = recordParaMostrar;
                recordParaMostrar = ""; 
            }
        }
    }

    public void CargarNivel(int indice)
    {
        LimpiarNivel();
        LevelData data = niveles[indice];

        for (int x = 0; x < data.width; x++)
            for (int y = 0; y < data.height; y++)
                CrearInstancia(cellPrefab, x, y, "Cell");

        GameObject playerObj = Instantiate(playerPrefab, new Vector3(data.playerSpawn.x, data.playerSpawn.y, 0), Quaternion.identity);
        playerObj.transform.SetParent(transform);
        instanciasActuales.Add(playerObj);

        ContarPasos cp = playerObj.GetComponent<ContarPasos>();
        if (cp != null) {
            cp.textoPasosUI = textoPasosGlobal;
            cp.ResetearPasos();
        }

        foreach (var pos in data.wallPositions) CrearInstancia(wallPrefab, pos.x, pos.y, "Wall");
        
        foreach (var range in data.wallRanges)
            for (int x = Mathf.Min(range.inicio.x, range.fin.x); x <= Mathf.Max(range.inicio.x, range.fin.x); x++)
                for (int y = Mathf.Min(range.inicio.y, range.fin.y); y <= Mathf.Max(range.inicio.y, range.fin.y); y++)
                    CrearInstancia(wallPrefab, x, y, "Wall");

        foreach (var pos in data.boxPositions) CrearInstancia(boxPrefab, pos.x, pos.y, "Box");
        foreach (var pos in data.winPositions) CrearInstancia(winPrefab, pos.x, pos.y, "WinTarget");

        AuthManager auth = FindObjectOfType<AuthManager>();
        if (auth != null)
        {
            recordParaMostrar = "Récord: ...";
            auth.ObtenerRecordFirebase(indice + 1, (record) => {
                recordParaMostrar = record > 0 ? "Récord: " + record : "Récord: --";
            });
        }
    }

    private void CrearInstancia(GameObject prefab, float x, float y, string nombre)
    {
        if (prefab == null) return;
        GameObject obj = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
        obj.name = nombre;
        obj.transform.SetParent(transform);
        instanciasActuales.Add(obj);
    }

    private void LimpiarNivel()
    {
        foreach (GameObject obj in instanciasActuales) Destroy(obj);
        instanciasActuales.Clear();
    }

    public void ComprobarVictoria()
    {
        BoxController[] todasLasCajas = FindObjectsOfType<BoxController>();
        LevelData data = ObtenerNivelActual();
        int metasCubiertas = 0;

        foreach (Vector2Int posMeta in data.winPositions)
            foreach (var caja in todasLasCajas)
                if (caja.gridPos == posMeta) { metasCubiertas++; break; }

        if (metasCubiertas >= data.winPositions.Count && data.winPositions.Count > 0)
            FinalizarNivelYGuardar();
    }

    public void SiguienteNivel()
    {
        if (indiceNivelActual + 1 < niveles.Count) {
            indiceNivelActual++;
            CargarNivel(indiceNivelActual);
        } else Debug.Log("Fin del juego");
    }

    public void FinalizarNivelYGuardar()
    {
        ContarPasos contador = FindObjectOfType<ContarPasos>();
        AuthManager auth = FindObjectOfType<AuthManager>();
        if (auth != null && contador != null)
        {
            auth.GuardarPasosFirebase(indiceNivelActual + 1, contador.GetPasos());
        }
        SiguienteNivel();
    }

    public LevelData ObtenerNivelActual()
    {
        return niveles[Mathf.Clamp(indiceNivelActual, 0, niveles.Count - 1)];
    }
}