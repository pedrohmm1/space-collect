using UnityEngine;

// Fica na Main Camera. Segue o player suavemente sem mostrar nada fora da arena.
public class CameraSegue : MonoBehaviour
{
    public float suavidade = 6f;

    private Transform player;
    private Camera cam;
    private Vector3 posicaoBase;   // posição sem o tremor
    private float fimTremor;
    private float forcaTremor;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        cam = GetComponent<Camera>();
        posicaoBase = Limita(new Vector3(player.position.x, player.position.y, transform.position.z));
    }

    // LateUpdate roda depois que o player já se moveu no frame
    void LateUpdate()
    {
        Vector3 destino = new Vector3(player.position.x, player.position.y, transform.position.z);
        posicaoBase = Limita(Vector3.Lerp(posicaoBase, destino, suavidade * Time.deltaTime));

        Vector3 tremor = Time.time < fimTremor ? (Vector3)Random.insideUnitCircle * forcaTremor : Vector3.zero;
        transform.position = posicaoBase + tremor;
    }

    // Impede a câmera de mostrar o que está fora do limite da Arena
    Vector3 Limita(Vector3 posicao)
    {
        Arena arena = Arena.Instancia;
        if (arena == null) return posicao;

        float meiaAltura = cam.orthographicSize;
        float meiaLargura = meiaAltura * cam.aspect;

        posicao.x = LimitaEixo(posicao.x, arena.limiteCameraMin.x + meiaLargura, arena.limiteCameraMax.x - meiaLargura);
        posicao.y = LimitaEixo(posicao.y, arena.limiteCameraMin.y + meiaAltura, arena.limiteCameraMax.y - meiaAltura);
        return posicao;
    }

    float LimitaEixo(float valor, float min, float max)
    {
        // Se a arena for menor que a tela nesse eixo, só centraliza
        return min > max ? (min + max) / 2 : Mathf.Clamp(valor, min, max);
    }

    public void Tremer(float duracao, float forca)
    {
        fimTremor = Time.time + duracao;
        forcaTremor = forca;
    }
}
