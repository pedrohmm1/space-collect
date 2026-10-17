using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuActions : MonoBehaviour
{
    // Seleciona o primeiro botão visível para dar pra navegar no menu com o controle
    void Start()
    {
        Button primeiro = GetComponentInChildren<Button>();
        if (primeiro != null)
            EventSystem.current.SetSelectedGameObject(primeiro.gameObject);
    }

    public void IniciaJogo()
    {
        GameController.Init();
        SceneManager.LoadScene(1);

    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
