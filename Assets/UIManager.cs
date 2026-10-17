using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public GameObject endGamePanel;
    public TMP_Text cronometroText;  // texto do cronômetro durante a partida
    public TMP_Text resultadoText;   // texto dentro do painel de fim de jogo
    public TMP_Text pontosText;      // pontuação durante a partida

    // Vidas em forma de corações
    public RectTransform coracoesContainer; // objeto vazio dentro do Canvas onde os corações aparecem
    public Sprite coracaoCheio;
    public Sprite coracaoVazio;
    public float tamanhoCoracao = 48f;
    public float espacoCoracao = 8f;

    // Imagens de fundo do painel de fim de jogo
    public Sprite imagemVitoria;
    public Sprite imagemDerrota;

    public AudioSource musica;      // música de fundo (para parar no fim do jogo)
    public AudioClip somVitoria;
    public AudioClip somDerrota;

    private Image[] coracoes;
    private int vidasMostradas;

    // Reinicia o jogo sempre que a cena carrega, assim funciona até dando Play direto nesta cena
    void Awake()
    {
        GameController.Init();
    }

    void Start()
    {
        CriaCoracoes();
        vidasMostradas = GameController.vidas;
    }

    // Cria uma Image para cada vida, lado a lado, dentro do container
    void CriaCoracoes()
    {
        coracoes = new Image[GameController.vidasIniciais];
        for (int i = 0; i < coracoes.Length; i++)
        {
            GameObject coracao = new GameObject("Coracao " + (i + 1), typeof(RectTransform), typeof(Image));
            coracao.transform.SetParent(coracoesContainer, false);

            RectTransform rt = coracao.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0f, 0.5f);
            rt.sizeDelta = new Vector2(tamanhoCoracao, tamanhoCoracao);
            rt.anchoredPosition = new Vector2(i * (tamanhoCoracao + espacoCoracao), 0f);

            coracoes[i] = coracao.GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameController.AtualizaTempo(Time.deltaTime);

        cronometroText.text = GameController.FormataTempo(GameController.tempoRestante);
        pontosText.text = "Pontos: " + GameController.pontos;

        // Deixa o cronômetro vermelho quando faltar pouco tempo
        cronometroText.color = GameController.tempoRestante <= 5f ? Color.red : Color.white;

        AtualizaCoracoes();

        if(GameController.gameOver && !endGamePanel.activeSelf)
        {
            endGamePanel.SetActive(true);

            // Seleciona o primeiro botão do painel (Reiniciar) para funcionar com o controle
            EventSystem.current.SetSelectedGameObject(endGamePanel.GetComponentInChildren<Button>().gameObject);

            if (musica != null)
                musica.Stop();

            MostraResultado();

            AudioClip som = GameController.venceu ? somVitoria : somDerrota;
            if (som != null)
                AudioSource.PlayClipAtPoint(som, Camera.main.transform.position);
        }
    }

    void AtualizaCoracoes()
    {
        for (int i = 0; i < coracoes.Length; i++)
            coracoes[i].sprite = i < GameController.vidas ? coracaoCheio : coracaoVazio;

        // O coração que acabou de ser perdido dá uma "pulsada"
        if (GameController.vidas < vidasMostradas)
            StartCoroutine(Pulsar(coracoes[GameController.vidas].rectTransform));
        vidasMostradas = GameController.vidas;
    }

    System.Collections.IEnumerator Pulsar(RectTransform alvo)
    {
        for (float t = 0; t < 0.3f; t += Time.deltaTime)
        {
            alvo.localScale = Vector3.one * (1f + Mathf.Sin(t / 0.3f * Mathf.PI) * 0.6f);
            yield return null;
        }
        alvo.localScale = Vector3.one;
    }

    void MostraResultado()
    {
        string titulo;
        if (GameController.venceu)
            titulo = "<color=#FFFFFF>VOCÊ VENCEU!</color>";
        else if (GameController.semVidas)
            titulo = "<color=#FF4D5E>GAME OVER</color>";
        else
            titulo = "<color=#FF9F43>TEMPO ESGOTADO</color>";

        // Texto compacto (título + 1 linha) para não tampar a imagem de fundo
        string texto = "<size=150%><b>" + titulo + "</b></size>\n"
            + "Tempo: <b>" + GameController.FormataTempo(GameController.tempoDecorrido) + "</b>"
            + "      Pontuação: <b><color=#FFD84D>" + GameController.pontuacaoFinal + "</color></b>";

        if (GameController.venceu)
            texto += "\n<size=75%>(" + GameController.pontos + " das estrelas + " + GameController.bonusTempo + " de bônus de tempo)</size>";

        resultadoText.text = texto;
        resultadoText.color = Color.white;
        resultadoText.alignment = TextAlignmentOptions.Center;

        // Contorno preto para o texto ficar legível em cima da imagem
        resultadoText.outlineWidth = 0.25f;
        resultadoText.outlineColor = Color.black;

        // Troca a imagem de fundo do painel conforme o resultado
        Image fundoPainel = endGamePanel.GetComponent<Image>();
        Sprite imagem = GameController.venceu ? imagemVitoria : imagemDerrota;
        if (imagem != null)
        {
            fundoPainel.sprite = imagem;
            fundoPainel.color = Color.white;
        }
    }
}
