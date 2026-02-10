using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Bunu ekleyin

public class FrameByFramePlayer : MonoBehaviour
{
    public Sprite[] frames;
    public Image displayImage;
    public float frameRate = 12f;
    public float lastFramesDuration = 2f;
    public int lastFramesCount = 2;
    public bool loop = false; // Loop'u false yap�n
    public string nextSceneName = "MainHall"; // Gidilecek sahne

    private int currentFrame = 0;
    private float timer = 0f;
    private bool isPlaying = true;

    void Update()
    {
        if (!isPlaying || frames.Length == 0) return;

        timer += Time.deltaTime;

        bool isLastFrame = currentFrame >= frames.Length - lastFramesCount;
        float frameDuration = isLastFrame ? lastFramesDuration : (1f / frameRate);

        if (timer >= frameDuration)
        {
            timer = 0f;
            displayImage.sprite = frames[currentFrame];
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                if (loop)
                {
                    currentFrame = 0;
                }
                else
                {
                    isPlaying = false;
                    Debug.Log("Video bitti! MainHall'a gidiliyor...");

                    SceneManager.LoadScene(nextSceneName);
                }
            }
        }
    }
}