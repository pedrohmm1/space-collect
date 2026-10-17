using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    // Nasce num lugar aleatório dentro da área de spawn da Arena
    public bool posicaoAleatoria = true;
    public float distanciaMinimaPlayer = 2.5f;

    // Tamanho e rotação sorteados para os asteroides não ficarem todos iguais
    public float escalaMin = 0.5f;
    public float escalaMax = 1f;

    // Awake (e não Start) para os obstáculos se posicionarem ANTES dos inimigos e coletáveis,
    // assim eles já sabem onde os asteroides estão e não nascem em cima deles
    void Awake()
    {
        if (!posicaoAleatoria) return;

        float escala = Random.Range(escalaMin, escalaMax);
        transform.localScale = new Vector3(escala, escala, 1f);
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        // O sprite do asteroide tem 2 unidades de diâmetro, então o raio é ~1 × escala
        transform.position = Spawn.SorteiaPosicao(distanciaMinimaPlayer, escala, gameObject);
    }
}
