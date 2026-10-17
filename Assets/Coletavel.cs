using UnityEngine;

public class Coletavel : MonoBehaviour
{
    // Nasce num lugar aleatório dentro da área de spawn da Arena
    public bool posicaoAleatoria = true;
    public float distanciaMinimaPlayer = 2f;

    // Efeito visual: gira e "respira" para chamar atenção
    public float velocidadeGiro = 90f;

    private Vector3 escalaOriginal;

    void Start()
    {
        escalaOriginal = transform.localScale;

        if (posicaoAleatoria)
            transform.position = Spawn.SorteiaPosicao(distanciaMinimaPlayer, 0.6f, gameObject);
    }

    void Update()
    {
        transform.Rotate(0f, 0f, velocidadeGiro * Time.deltaTime);
        transform.localScale = escalaOriginal * (1f + Mathf.Sin(Time.time * 4f) * 0.1f);
    }
}
