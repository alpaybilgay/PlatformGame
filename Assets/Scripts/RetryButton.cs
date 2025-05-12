using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("GameplayScene"); // gerçek oyun sahnenin adýný yaz
    }
}
