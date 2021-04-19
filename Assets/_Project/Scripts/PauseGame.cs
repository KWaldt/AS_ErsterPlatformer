using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PauseGame : MonoBehaviour
{
    // "Fire1" = linke CTRL Taste oder Maus
    public string pauseButton = "Fire1";

    public VeryBasicPlayer player;
    public GameObject pauseMenu;
    public Camera cam; // camera ist ein interner Name, deshalb cam
    public float pausedCameraSize = 2f;
    public PostProcessVolume postProcessVolume;
    public float temperatureOnPause = 100f;
    
    private bool gameIsPaused;
    private float origCameraSize;
    private ColorGrading colorGrading;
    private float origTemperature;

    private void Awake()
    {
        origCameraSize = cam.orthographicSize;
        postProcessVolume.profile.TryGetSettings(out colorGrading);
        if (colorGrading != null)
        {
            colorGrading.temperature.overrideState = true;
            origTemperature = colorGrading.temperature.value;
        }
    }

    private void Update()
    {
        if (!Input.GetButtonDown(pauseButton))
            return;
        
        TogglePause();
    }

    private void TogglePause()
    {
        gameIsPaused = !gameIsPaused;

        player.AllowInput = !gameIsPaused;
        
        pauseMenu.SetActive(gameIsPaused);

        // CONDITION ? TRUE : FALSE
        // Shorthand für if (gameIsPaused) { cam.orthographicSize = pausedCameraSize; } else { cam.orthographicSize = origCameraSize;}
        cam.orthographicSize = gameIsPaused ? pausedCameraSize : origCameraSize;
        
        if (colorGrading != null)
        {
            colorGrading.temperature.value = gameIsPaused ? temperatureOnPause : origTemperature;
        }
    }
}
