using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Rigidbody2D rb;
    AudioSource audioSource;
    public float speed;

    public AudioClip somDano;            // som ao encostar num inimigo
    public float tempoInvencivel = 1.5f; // tempo sem levar dano depois de ser atingido
    private float fimInvencibilidade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameController.gameOver) return; // player para quando o jogo acaba

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal,moveVertical);

        // Controle (Xbox/PlayStation): analógico esquerdo ou setinhas (D-pad)
        Gamepad controle = Gamepad.current;
        if (controle != null)
        {
            Vector2 movimentoControle = controle.leftStick.ReadValue() + controle.dpad.ReadValue();
            if (movimentoControle.magnitude > movement.magnitude)
                movement = movimentoControle;
        }

        // ClampMagnitude (em vez de normalized) para o analógico andar devagar quando inclinado pouco
        rb.MovePosition(rb.position + Vector2.ClampMagnitude(movement, 1f) * speed * Time.fixedDeltaTime);

        // Vira a nave para a direção em que está andando (o sprite aponta para cima)
        if (movement.magnitude > 0.1f)
            rb.rotation = Mathf.LerpAngle(rb.rotation, Vector2.SignedAngle(Vector2.up, movement), 0.3f);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coletavel")
            {
                audioSource.Play();
                GameController.Collect();
                Destroy(other.gameObject);
            }


    }

    // Stay (e não Enter) para que um inimigo encostado no player continue causando dano
    private void OnCollisionStay2D(Collision2D colisao)
    {
        TentaLevarDano(colisao.gameObject);
    }

    // Caso o collider do player (ou do inimigo) esteja como trigger
    private void OnTriggerStay2D(Collider2D other)
    {
        TentaLevarDano(other.gameObject);
    }

    private void TentaLevarDano(GameObject outro)
    {
        // Reconhece o inimigo pelo script Inimigo (não depende da tag)
        bool ehInimigo = outro.GetComponent<Inimigo>() != null;

        if (ehInimigo && Time.time >= fimInvencibilidade && !GameController.gameOver)
        {
            GameController.PerdeVida();
            fimInvencibilidade = Time.time + tempoInvencivel;

            audioSource.PlayOneShot(somDano);
            StartCoroutine(Piscar());
            // Tremidinha na câmera para dar sensação de impacto
            CameraSegue cameraSegue = Camera.main.GetComponent<CameraSegue>();
            if (cameraSegue != null)
                cameraSegue.Tremer(0.2f, 0.15f);
            StartCoroutine(VibrarControle());
        }
    }

    // Vibra o controle rapidinho ao levar dano
    private System.Collections.IEnumerator VibrarControle()
    {
        Gamepad controle = Gamepad.current;
        if (controle == null) yield break;

        controle.SetMotorSpeeds(0.4f, 0.8f);
        yield return new WaitForSeconds(0.25f);
        controle.SetMotorSpeeds(0f, 0f);
    }

    // Garante que o controle não fica vibrando se o objeto sumir (ex.: trocar de cena)
    private void OnDisable()
    {
        if (Gamepad.current != null)
            Gamepad.current.SetMotorSpeeds(0f, 0f);
    }

    // Pisca entre vermelho e transparente enquanto estiver invencível
    private System.Collections.IEnumerator Piscar()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color corOriginal = sprite.color;
        Color transparente = new Color(corOriginal.r, corOriginal.g, corOriginal.b, 0.2f);

        while (Time.time < fimInvencibilidade)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = transparente;
            yield return new WaitForSeconds(0.1f);
        }
        sprite.color = corOriginal;
    }

}
