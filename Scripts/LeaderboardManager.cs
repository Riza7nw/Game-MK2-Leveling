using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject leaderboardEntryPrefab; // Drag prefab LeaderboardEntry
    public Transform entryParent;             // Drag Content (LeaderboardPanel > Viewport > Content)

    [Header("Level Info")]
    public string levelName = "Level 1";      // Ubah di Inspector untuk tiap level

    private DatabaseReference dbRef;
    private string currentUserKey;
    private string currentUserName;

    void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        currentUserKey = PlayerPrefs.GetString("currentUserKey", "");
        currentUserName = PlayerPrefs.GetString("currentUserName", "");
        LoadLeaderboard();
    }

    public void LoadLeaderboard()
    {
        string levelPath = "leaderboard/" + levelName;
        dbRef.Child(levelPath).OrderByChild("time").LimitToFirst(10).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || !task.IsCompleted) return;

            DataSnapshot snapshot = task.Result;

            // Bersihkan entri lama
            foreach (Transform child in entryParent)
            {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot entry in snapshot.Children)
            {
                string uid = entry.Key;
                string name = entry.Child("name").Value.ToString();
                float time = float.Parse(entry.Child("time").Value.ToString());

                GameObject entryObj = Instantiate(leaderboardEntryPrefab, entryParent);
                TMP_Text[] texts = entryObj.GetComponentsInChildren<TMP_Text>();
                if (texts.Length >= 2)
                {
                    texts[0].text = name;
                    texts[1].text = FormatTime(time);
                }

                // Highlight pemain yang sedang login
                if (uid == currentUserKey)
                {
                    Image bg = entryObj.GetComponent<Image>();
                    if (bg != null)
                    {
                        bg.color = new Color(1f, 1f, 0.6f); // Kuning muda
                    }
                }
            }
        });
    }

    private string FormatTime(float time)
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(time);
        return t.ToString(@"mm\:ss\:fff");
    }
}
