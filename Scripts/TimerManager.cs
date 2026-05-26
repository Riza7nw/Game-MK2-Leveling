using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Firebase.Database;
using Firebase.Extensions;

public class TimerManager : MonoBehaviour
{
    public TMP_Text currentTimeText;
    public TMP_Text bestTimeText;
    public GameObject finishUI;
    public string levelName = "Level 1"; // Set ini di Inspector

    private float timer;
    private bool isTiming = true;
    private bool levelCompleted = false;
    private bool isValidRun = true;

    private DatabaseReference dbReference;

    void Start()
    {
        timer = 0f;
        isTiming = true;
        levelCompleted = false;
        isValidRun = true;

        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        ShowBestTime();
    }

    void Update()
    {
        if (isTiming)
        {
            timer += Time.deltaTime;
            currentTimeText.text = FormatTime(timer);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                ResetBestTime();
            }
            else
            {
                RestartLevel();
            }
        }
    }

    public void OnLevelComplete()
    {
        if (!isTiming || levelCompleted) return;

        isTiming = false;
        levelCompleted = true;

        if (isValidRun)
        {
            SaveBestTime();
            SaveBestTimeToFirebase();
            SaveToLeaderboard();
        }

        ShowBestTime();

        if (finishUI != null)
            finishUI.SetActive(true);
    }

    public void InvalidateTimer()
    {
        isValidRun = false;
    }

    public void StopTimer()
    {
        isTiming = false;
    }

    void SaveBestTime()
    {
        string key = "BestTime_" + levelName;
        float bestTime = PlayerPrefs.GetFloat(key, float.MaxValue);
        if (timer < bestTime)
        {
            PlayerPrefs.SetFloat(key, timer);
            PlayerPrefs.Save();
        }
    }

    void SaveBestTimeToFirebase()
    {
        string userKey = PlayerPrefs.GetString("currentUserKey", "");
        if (string.IsNullOrEmpty(userKey)) return;

        string levelKey = levelName.Replace(" ", "").ToLower(); // e.g. level1
        float bestTime = PlayerPrefs.GetFloat("BestTime_" + levelName, float.MaxValue);

        dbReference.Child($"users/{userKey}/bestTimes/{levelKey}").SetValueAsync(bestTime).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                Debug.Log($"Best time {bestTime} untuk {levelName} disimpan ke users/");
            else
                Debug.LogError("Gagal simpan ke users/: " + task.Exception);
        });
    }

    void SaveToLeaderboard()
    {
        string userKey = PlayerPrefs.GetString("currentUserKey", "");
        string userName = PlayerPrefs.GetString("currentUserName", "");
        if (string.IsNullOrEmpty(userKey) || string.IsNullOrEmpty(userName)) return;

        float bestTime = PlayerPrefs.GetFloat("BestTime_" + levelName, float.MaxValue);
        string levelKey = levelName; // Tetap pakai "Level 1", "Level 2", dll

        dbReference.Child($"Leaderboards/{levelKey}/{userKey}/name").SetValueAsync(userName);
        dbReference.Child($"Leaderboards/{levelKey}/{userKey}/time").SetValueAsync(bestTime);
    }

    void ShowBestTime()
    {
        float bestTime = PlayerPrefs.GetFloat("BestTime_" + levelName, float.MaxValue);
        bestTimeText.text = (bestTime != float.MaxValue)
            ? "Best: " + FormatTime(bestTime)
            : "Best: --:--:---";
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetBestTime()
    {
        PlayerPrefs.DeleteKey("BestTime_" + levelName);
        ShowBestTime();
        Debug.Log("Best time telah direset!");
    }

    string FormatTime(float time)
    {
        TimeSpan t = TimeSpan.FromSeconds(time);
        return t.ToString(@"mm\:ss\:fff");
    }
}
