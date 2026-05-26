using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class LeaderboardMigrator : MonoBehaviour
{
    private DatabaseReference dbRef;

    void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        MigrateUserBestTimesToLeaderboard();
    }

    void MigrateUserBestTimesToLeaderboard()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Gagal mengambil data user.");
                return;
            }

            DataSnapshot usersSnapshot = task.Result;
            foreach (DataSnapshot userSnapshot in usersSnapshot.Children)
            {
                string userId = userSnapshot.Key;

                string name = userSnapshot.Child("name").Value?.ToString();
                if (string.IsNullOrEmpty(name)) continue;

                DataSnapshot bestTimesSnapshot = userSnapshot.Child("bestTimes");
                foreach (DataSnapshot levelSnapshot in bestTimesSnapshot.Children)
                {
                    string levelName = levelSnapshot.Key; // Contoh: level1, level2
                    string timeStr = levelSnapshot.Value?.ToString();
                    if (float.TryParse(timeStr, out float time))
                    {
                        // Simpan ke Leaderboards/Level 1/userId
                        string levelFormatted = levelName.Replace("level", "Level "); // level1 -> Level 1
                        string path = $"Leaderboards/{levelFormatted}/{userId}";
                        Dictionary<string, object> entry = new Dictionary<string, object>
                        {
                            { "name", name },
                            { "time", time }
                        };
                        dbRef.Child(path).SetValueAsync(entry);
                    }
                }
            }

            Debug.Log("✅ Migrasi selesai.");
        });
    }
}
