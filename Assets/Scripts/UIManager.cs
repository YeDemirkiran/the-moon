using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject inGameUI, pauseUI;

    GameManager gameManager;

    private IEnumerator Start()
    {
        while (GameManager.instance == null)
        {
            yield return null;
        }
        
        gameManager = GameManager.instance;

        gameManager.eventsAtPause += () => ActivatePauseMenu();
        gameManager.eventsAtResume += () => DeactivatePauseMenu();
    }

    public void ActivatePauseMenu()
    {
        inGameUI.SetActive(false);
        pauseUI.SetActive(true);  
    }

    public void DeactivatePauseMenu()
    {
        inGameUI.SetActive(true);
        pauseUI.SetActive(false);
    }
}