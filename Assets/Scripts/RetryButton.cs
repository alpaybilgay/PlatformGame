using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("GameplayScene"); // ger�ek oyun sahnenin ad�n� yaz
    }
}
