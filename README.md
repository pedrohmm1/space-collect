# Space Collect

Jogo 2D feito em Unity 6 (6000.3) em que você pilota uma nave pelo espaço coletando estrelas antes que o tempo acabe, desviando de OVNIs, minas espaciais e asteroides.

**Autor:** Pedro Mourão Machado

## Como jogar

Colete **todas as estrelas** da arena antes que o cronômetro chegue a zero.

| Ação | Teclado | Controle (Xbox) |
|---|---|---|
| Mover a nave | WASD ou setas | Analógico esquerdo ou D-pad |
| Navegar nos menus | Mouse | Analógico / D-pad |
| Confirmar | Clique | A |

- Você começa com **30 segundos** e cada estrela coletada dá **+1 segundo**.
- Você tem **3 vidas** (corações). Encostar num inimigo tira uma vida e deixa a nave invencível por um instante.
- O jogo acaba quando você coleta todas as estrelas (**vitória**), quando o tempo acaba ou quando as vidas acabam.
- **Pontuação:** 100 pontos por estrela + 10 pontos por segundo que sobrar ao vencer.

## Funcionalidades

- **Cronômetro** em contagem regressiva, com tempo final e pontuação mostrados no fim da partida
- **Inimigos** com comportamentos diferentes: parado (minas), patrulha, movimento aleatório e perseguição (persegue a nave quando ela chega perto)
- **Posições aleatórias**: estrelas, inimigos e asteroides nascem em lugares diferentes a cada partida, sem nascer em cima uns dos outros
- **Obstáculos** (asteroides) com tamanho e rotação sorteados
- **Arena grande** com câmera que segue a nave
- **Feedback de dano**: a nave pisca, a câmera treme, toca um som e o controle vibra
- **Interface**: corações de vida, cronômetro, pontuação e telas de vitória/derrota
- **Áudio**: música no menu, música na partida e efeitos sonoros
- **Suporte a controle** de Xbox/PlayStation (movimento, menus e vibração)

## Estrutura do código (`Assets/`)

| Script | Função |
|---|---|
| `GameController.cs` | Estado da partida: tempo, vidas, pontos, vitória/derrota |
| `PlayerMovement.cs` | Movimento da nave (teclado e controle), coleta e dano |
| `Inimigo.cs` | Comportamentos dos inimigos |
| `Coletavel.cs` / `Obstaculo.cs` | Estrelas e asteroides com posição aleatória |
| `Spawn.cs` / `Arena.cs` | Sorteio de posições livres e limites da arena |
| `CameraSegue.cs` | Câmera que segue a nave e efeito de tremor |
| `UIManager.cs` | HUD e tela de fim de jogo |
| `MenuActions.cs` | Botões do menu |

## Créditos e referências

- **Base do projeto:** [Tutorial – Introdução Rápida](https://slender-ceder-1da.notion.site/Tutorial-Introdu-o-R-pida-1a6d1ae1773780a7a59cf823a8b599ec)
- **Sprites do jogo** (nave, OVNI, mina, estrela, asteroide, corações e fundo estrelado): gerados por código (scripts Python) com auxílio do Claude (Anthropic)
- **Músicas e efeitos sonoros** (música do menu, música da partida, dano, vitória e derrota): gerados por código (síntese em Python) com auxílio do Claude (Anthropic)
- **Som de coleta:** "Coin Recieved" por ribhavagrawal — [Pixabay](https://pixabay.com/) (Licença Pixabay)
- **Capa e telas de vitória/derrota:** ChatGPT gerou as imagens
- **Fonte:** Liberation Sans (incluída no TextMesh Pro)
- **Engine:** [Unity](https://unity.com/)
