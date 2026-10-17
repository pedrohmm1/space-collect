using UnityEngine;

// Sorteia posições livres dentro da área de spawn da Arena (usado por inimigos, coletáveis e obstáculos)
public static class Spawn
{
    public static Vector2 SorteiaPosicao(float distanciaMinimaPlayer, float raio, GameObject quemPede)
    {
        // Atualiza a física com as posições dos objetos que já foram sorteados antes deste
        Physics2D.SyncTransforms();

        Arena arena = Arena.Instancia;
        Vector2 areaMin = arena != null ? arena.areaSpawnMin : new Vector2(-6.5f, -3.5f);
        Vector2 areaMax = arena != null ? arena.areaSpawnMax : new Vector2(6.5f, 3.5f);

        Transform player = GameObject.Find("Player").transform;
        Vector2 posicao = Vector2.zero;

        // Tenta algumas vezes achar um lugar longe do player e sem nada em cima (obstáculos, paredes...)
        for (int tentativa = 0; tentativa < 50; tentativa++)
        {
            posicao = new Vector2(
                Random.Range(areaMin.x, areaMax.x),
                Random.Range(areaMin.y, areaMax.y));

            if (Vector2.Distance(posicao, player.position) >= distanciaMinimaPlayer && EstaLivre(posicao, raio, quemPede))
                break;
        }
        return posicao;
    }

    static bool EstaLivre(Vector2 posicao, float raio, GameObject quemPede)
    {
        foreach (Collider2D outro in Physics2D.OverlapCircleAll(posicao, raio))
        {
            if (outro.gameObject != quemPede)
                return false;
        }
        return true;
    }
}
