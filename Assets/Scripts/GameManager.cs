using UnityEngine;
using UnityEngine.Events;

public enum GameState { Paused, Running, Intro }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState state {  get; private set; }
    public static bool isPaused { get { return instance.state == GameState.Paused || instance.state == GameState.Intro; } }
    GameState previousState;

    public UnityAction eventsAtPause;
    public UnityAction eventsAtResume;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        state = GameState.Intro;
        OnIntro();
        eventsAtPause.Invoke();

        previousState = GameState.Paused;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.Intro)
        {
            return;
        }

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

    void ChangeCursorState(bool active)
    {
        Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = active;
    }

    void OnIntro()
    {
        Time.timeScale = 0.0001f;
        ChangeCursorState(false);
    }

    void OnPause()
    { 
        Time.timeScale = 0.0001f;
        ChangeCursorState(true);
    }

    void OnResume()
    {
        Time.timeScale = 1f;
        ChangeCursorState(false);
    }
}