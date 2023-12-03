using UnityEngine;
using UnityEngine.Events;

public enum GameState { Paused, Running }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState state {  get; private set; }
    GameState previousState;

    public UnityAction eventsAtPause;
    public UnityAction eventsAtResume;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        state = GameState.Running;
        previousState = GameState.Paused;
    }

    // Update is called once per frame
    void Update()
    {
        PauseCheck();
        SwitchMachine();
    }

    void PauseCheck()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == GameState.Paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void SwitchMachine()
    {
        if (previousState != state)
        {
            previousState = state;

            switch (state)
            {
                case GameState.Paused:
                    OnPause();
                    eventsAtPause.Invoke();
                    break;
                case GameState.Running:
                    OnResume();
                    eventsAtResume.Invoke();
                    break;
            }
        }
    }

    public void Pause()
    {
        state = GameState.Paused;
    }

    public void Resume()
    {
        state = GameState.Running;
    }

    public void Exit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void OnPause()
    { 
        Time.timeScale = 0.0001f;
    }

    void OnResume()
    {
        Time.timeScale = 1f;
    }
}