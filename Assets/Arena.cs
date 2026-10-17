using UnityEngine;

// Fica no objeto "Mundo". Define onde as coisas podem nascer e até onde a câmera pode ir.
// Os retângulos aparecem na aba Scene: amarelo = spawn, ciano = limite da câmera.
public class Arena : MonoBehaviour
{
    public Vector2 areaSpawnMin = new Vector2(-13f, -7f);
    public Vector2 areaSpawnMax = new Vector2(13f, 7f);

    public Vector2 limiteCameraMin = new Vector2(-16f, -10f);
    public Vector2 limiteCameraMax = new Vector2(16f, 10f);

    private static Arena instancia;

    // Procura a Arena da cena (funciona mesmo se for chamado no Awake de outro objeto)
    public static Arena Instancia
    {
        get
        {
            if (instancia == null)
                instancia = FindFirstObjectByType<Arena>();
            return instancia;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((areaSpawnMin + areaSpawnMax) / 2, areaSpawnMax - areaSpawnMin);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube((limiteCameraMin + limiteCameraMax) / 2, limiteCameraMax - limiteCameraMin);
    }
}
