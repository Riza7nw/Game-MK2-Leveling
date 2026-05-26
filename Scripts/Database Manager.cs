using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using System;
using System.Globalization;

public class DatabaseManager : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField idInput;
    public TMP_Text warningText; // Tambahan: teks peringatan di UI

    public GameObject profilePanel;
    public TMP_Text profileNameText;
    public TMP_Text profileIdText;

    public TMP_Text bestTimeLevel1Text;
    public TMP_Text bestTimeLevel2Text;
    public TMP_Text bestTimeLevel3Text;
    public TMP_Text bestTimeLevel4Text;
    public TMP_Text bestTimeLevel5Text;

    private DatabaseReference dbReference;
    private string currentUserKey = "";

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                app.Options.DatabaseUrl = new System.Uri("https://lvllng-a04ca-default-rtdb.firebaseio.com/");
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase siap dan Database URL diset!");
            }
            else
            {
                Debug.LogError("Firebase belum siap: " + dependencyStatus);
            }
        });

        if (profilePanel != null)
            profilePanel.SetActive(false);

        if (warningText != null)
            warningText.gameObject.SetActive(false);
    }

    public void GenerateRandomID()
    {
        int randomID = UnityEngine.Random.Range(10000, 99999);
        idInput.text = randomID.ToString();
    }

    public void CreateUser()
    {
        string inputName = nameInput.text.Trim();
        string inputId = idInput.text.Trim();

        // Validasi input kosong
        bool nameEmpty = string.IsNullOrEmpty(inputName);
        bool idEmpty = string.IsNullOrEmpty(inputId);

        // Tampilkan warning dan highlight merah jika kosong
        if (nameEmpty || idEmpty)
        {
            if (warningText != null)
            {
                warningText.text = "Nama dan ID harus diisi!";
                warningText.gameObject.SetActive(true);
            }

            if (nameInput != null)
                nameInput.image.color = nameEmpty ? Color.red : Color.white;

            if (idInput != null)
                idInput.image.color = idEmpty ? Color.red : Color.white;

            return;
        }

        // Validasi ID berupa angka
        if (!int.TryParse(inputId, out int parsedId))
        {
            if (warningText != null)
            {
                warningText.text = "ID dan Nama harus diisi!";
                warningText.gameObject.SetActive(true);
            }
            return;
        }

        // Reset warna dan warning
        if (nameInput != null) nameInput.image.color = Color.white;
        if (idInput != null) idInput.image.color = Color.white;
        if (warningText != null) warningText.gameObject.SetActive(false);

        // Cek apakah user sudah terdaftar
        dbReference.Child("users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Gagal mengambil data dari Firebase: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;
            foreach (DataSnapshot userSnapshot in snapshot.Children)
            {
                string name = userSnapshot.Child("name").Value.ToString();
                string id = userSnapshot.Child("id").Value.ToString();

                if (name == inputName && id == inputId)
                {
                    Debug.LogWarning("User sudah terdaftar dengan ID dan nama ini.");
                    if (warningText != null)
                    {
                        warningText.text = "User sudah terdaftar.";
                        warningText.gameObject.SetActive(true);
                    }
                    return;
                }
            }

            // Jika belum ada, simpan user baru
            User newUser = new User(inputName, parsedId);
            string json = JsonUtility.ToJson(newUser);

            dbReference.Child("users").Push().SetRawJsonValueAsync(json).ContinueWithOnMainThread(createTask =>
            {
                if (createTask.IsCompleted)
                {
                    Debug.Log("User berhasil didaftarkan.");
                    SceneManager.LoadScene("Login");
                }
                else
                {
                    Debug.LogError("Error saat mendaftar user: " + createTask.Exception);
                }
            });
        });
    }

    public void LoginUser()
    {
        string inputName = nameInput.text.Trim();
        string inputId = idInput.text.Trim();

        if (string.IsNullOrEmpty(inputName) || string.IsNullOrEmpty(inputId))
        {
            Debug.LogWarning("Name or ID is empty.");
            if (warningText != null)
            {
                warningText.text = "Nama dan ID harus diisi!";
                warningText.gameObject.SetActive(true);
            }
            return;
        }

        dbReference.Child("users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Gagal mengambil data dari Firebase: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;
            foreach (DataSnapshot userSnapshot in snapshot.Children)
            {
                string name = userSnapshot.Child("name").Value.ToString();
                string id = userSnapshot.Child("id").Value.ToString();

                if (name == inputName && id == inputId)
                {
                    currentUserKey = userSnapshot.Key;
                    PlayerPrefs.SetString("currentUserKey", currentUserKey);
                    PlayerPrefs.SetString("currentUserName", name);
                    PlayerPrefs.SetString("currentUserId", id);

                    Debug.Log("Login berhasil!");
                    SceneManager.LoadScene("Main Menu 1");
                    return;
                }
            }

            if (warningText != null)
            {
                warningText.text = "Login gagal: nama atau ID salah.";
                warningText.gameObject.SetActive(true);
            }
            Debug.LogWarning("Login gagal: nama atau ID salah.");
        });
    }

    public void ShowProfile()
    {
        if (profilePanel == null)
        {
            Debug.LogWarning("Profile panel tidak diset.");
            return;
        }

        string name = PlayerPrefs.GetString("currentUserName", "-");
        string id = PlayerPrefs.GetString("currentUserId", "-");
        string userKey = PlayerPrefs.GetString("currentUserKey", "");

        profileNameText.text = "Name: " + name;
        profileIdText.text = "ID: " + id;
        profilePanel.SetActive(true);

        if (string.IsNullOrEmpty(userKey))
        {
            Debug.LogWarning("User key kosong.");
            return;
        }

        FirebaseDatabase.DefaultInstance.GetReference("users").Child(userKey).Child("bestTimes")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Gagal mengambil best times dari Firebase.");
                    return;
                }

                DataSnapshot snapshot = task.Result;
                SetBestTimeText(bestTimeLevel1Text, snapshot, "level1");
                SetBestTimeText(bestTimeLevel2Text, snapshot, "level2");
                SetBestTimeText(bestTimeLevel3Text, snapshot, "level3");
                SetBestTimeText(bestTimeLevel4Text, snapshot, "level4");
                SetBestTimeText(bestTimeLevel5Text, snapshot, "level5");
            });
    }

    private void SetBestTimeText(TMP_Text textComponent, DataSnapshot snapshot, string levelKey)
    {
        if (snapshot.HasChild(levelKey))
        {
            double time = Convert.ToDouble(snapshot.Child(levelKey).Value);
            TimeSpan t = TimeSpan.FromSeconds(time);
            textComponent.text = $"{CultureInfo.InvariantCulture.TextInfo.ToTitleCase(levelKey)}: {t:mm\\:ss\\:fff}";
        }
        else
        {
            textComponent.text = $"{CultureInfo.InvariantCulture.TextInfo.ToTitleCase(levelKey)}: --:--:---";
        }
    }

    public void CloseProfile()
    {
        if (profilePanel != null)
        {
            profilePanel.SetActive(false);
        }
    }
}

[System.Serializable]
public class User
{
    public string name;
    public int id;

    public User(string name, int id)
    {
        this.name = name;
        this.id = id;
    }
}
