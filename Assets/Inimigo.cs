using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public enum Comportamento { Parado, Patrulha, Perseguicao, Aleatorio }

    public Comportamento comportamento = Comportamento.Aleatorio;
    public float velocidade = 2f;

    // Patrulha: anda entre a posição inicial e (posição inicial + deslocamento)
    public Vector2 deslocamentoPatrulha = new Vector2(3f, 0f);

    // Perseguição: se o player chegar mais perto que isso, o inimigo vai atrás dele
    public float raioDeteccao = 3f;

    // Aleatório: de quanto em quanto tempo (min/max) o inimigo troca de direção
    public float tempoMinTroca = 1f;
    public float tempoMaxTroca = 3f;

    // Spawn: nasce num lugar aleatório dentro da área de spawn da Arena
    public bool posicaoAleatoria = true;
    public float distanciaMinimaPlayer = 3f; // para não nascer em cima do player

    private Rigidbody2D rb;
    private Transform player;
    private Vector2 pontoA;
    private Vector2 pontoB;
    private Vector2 alvoPatrulha;
    private Vector2 direcaoAleatoria;
    private float proximaTroca;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").transform;

        // Configura a física por código: inimigo sólido, sem gravidade e sem girar.
        // O Parado fica Kinematic para o player não conseguir empurrá-lo.
        rb.bodyType = comportamento == Comportamento.Parado ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.useFullKinematicContacts = true; // Parado (Kinematic) também detecta contato com o player
        GetComponent<Collider2D>().isTrigger = false;

        if (posicaoAleatoria)
        {
            Vector2 posicao = Spawn.SorteiaPosicao(distanciaMinimaPlayer, 0.7f, gameObject);
            rb.position = posicao;
            transform.position = posicao;
        }

        pontoA = rb.position;
        pontoB = pontoA + deslocamentoPatrulha;
        alvoPatrulha = pontoB;
    }

    void FixedUpdate()
    {
        if (GameController.gameOver)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direcao = Vector2.zero;

        switch (comportamento)
        {
            case Comportamento.Parado:
                break;

            case Comportamento.Patrulha:
                direcao = DirecaoPatrulha();
                break;

            case Comportamento.Aleatorio:
                direcao = DirecaoAleatoria();
                break;

            case Comportamento.Perseguicao:
                // Persegue se o player estiver perto, senão anda aleatoriamente
                if (Vector2.Distance(rb.position, player.position) <= raioDeteccao)
                    direcao = ((Vector2)player.position - rb.position).normalized;
                else
                    direcao = DirecaoAleatoria();
                break;
        }

        rb.linearVelocity = direcao * velocidade;
    }

    Vector2 DirecaoPatrulha()
    {
        if (Vector2.Distance(rb.position, alvoPatrulha) < 0.1f)
            TrocaAlvoPatrulha();

        return (alvoPatrulha - rb.position).normalized;
    }

    void TrocaAlvoPatrulha()
    {
        alvoPatrulha = alvoPatrulha == pontoA ? pontoB : pontoA;
    }

    Vector2 DirecaoAleatoria()
    {
        if (Time.time >= proximaTroca)
        {
            direcaoAleatoria = Random.insideUnitCircle.normalized;
            proximaTroca = Time.time + Random.Range(tempoMinTroca, tempoMaxTroca);
        }
        return direcaoAleatoria;
    }

    // Quando bate em algo (parede, player...), muda de direção
    void OnCollisionEnter2D(Collision2D colisao)
    {
        if (comportamento == Comportamento.Patrulha)
            TrocaAlvoPatrulha();
        else
            direcaoAleatoria = Vector2.Reflect(direcaoAleatoria, colisao.GetContact(0).normal);
    }

    // Desenha o caminho da patrulha e o raio de detecção na aba Scene
    void OnDrawGizmosSelected()
    {
        Vector2 inicio = Application.isPlaying ? pontoA : (Vector2)transform.position;

        if (comportamento == Comportamento.Patrulha)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(inicio, inicio + deslocamentoPatrulha);
        }

        if (comportamento == Comportamento.Perseguicao)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, raioDeteccao);
        }
    }
}
