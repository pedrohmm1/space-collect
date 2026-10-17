using UnityEngine;

public static class GameController
{
    private static int collectableCount;

    // Mecânica de tempo
    public const float tempoInicial = 30f;   // segundos para começar (mapa maior)
    public const float bonusPorColetavel = 1f; // segundos ganhos a cada coletável

    // Vidas e pontuação
    public const int vidasIniciais = 3;
    public const int pontosPorColetavel = 100;
    public const int pontosPorSegundoRestante = 10; // bônus ao vencer

    public static float tempoDecorrido { get; private set; }
    public static float tempoRestante { get; private set; }
    public static int vidas { get; private set; }
    public static int pontos { get; private set; }

    public static bool venceu
    {
        get { return collectableCount <= 0; }
    }

    public static bool tempoEsgotado
    {
        get { return tempoRestante <= 0; }
    }

    public static bool semVidas
    {
        get { return vidas <= 0; }
    }

    public static bool gameOver
    {
        get { return venceu || tempoEsgotado || semVidas; }
    }

    // Bônus pelo tempo que sobrou (só se venceu)
    public static int bonusTempo
    {
        get { return venceu ? (int)(tempoRestante * pontosPorSegundoRestante) : 0; }
    }

    public static int pontuacaoFinal
    {
        get { return pontos + bonusTempo; }
    }


    public static void Init()
    {
        // Conta quantos coletáveis existem na cena (pode adicionar quantos quiser)
        collectableCount = GameObject.FindGameObjectsWithTag("Coletavel").Length;
        tempoDecorrido = 0f;
        tempoRestante = tempoInicial;
        vidas = vidasIniciais;
        pontos = 0;
    }

    public static void Collect()
    {
        collectableCount --;
        tempoRestante += bonusPorColetavel;
        pontos += pontosPorColetavel;
    }

    public static void PerdeVida()
    {
        vidas = Mathf.Max(0, vidas - 1);
    }

    public static void AtualizaTempo(float deltaTime)
    {
        if (gameOver) return;

        tempoDecorrido += deltaTime;
        tempoRestante = Mathf.Max(0f, tempoRestante - deltaTime);
    }

    public static string FormataTempo(float tempo)
    {
        int minutos = (int)(tempo / 60);
        float segundos = tempo % 60;
        return string.Format("{0:00}:{1:00.00}", minutos, segundos);
    }
}
