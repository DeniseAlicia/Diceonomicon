// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Networking;
// using TMPro;

// [System.Serializable]
// public class HighscoreEntry
// {
//     public string player_name;
//     public int score;
//     public List<string> impling_roster;
// }

// // [System.Serializable]
// // public class HighscoreList
// // {
// //     public List<HighscoreEntry> highscores;
// // }

// public class HighscoreManager : MonoBehaviour
// {
//     private const string baseUrl = "http://localhost:8000/highscores";
//     public BattleSceneManager sceneManager;
//     public GameObject enterNameField;
//     public TMP_Text enteredName;

//     public void Start()
//     {
//         //BattleSceneManager.OnLoss.AddListener(() => StartCoroutine(SaveAndLoadHighscores()));
//     }

//     public void AddHighscore()
//     {
//         StartCoroutine(SaveAndLoadHighscores());
//     }

//     public string GetPlayerName()
//     {
//         enterNameField.SetActive(false);
//         return enteredName.text;
//     }

//     private IEnumerator SaveAndLoadHighscores()
//     {
//         yield return StartCoroutine(PostHighscore());
//         yield return StartCoroutine(GetHighscores());
//     }

//     public void SaveScore()
//     {
//         StartCoroutine(PostHighscore());
//     }

//     public void LoadHighscores()
//     {
//         StartCoroutine(GetHighscores());
//     }

//     private IEnumerator PostHighscore()
//     {
//         int charLimit = 5;
//         int minChar = 1;
//         int newScore = sceneManager.Score;
//         string playerName = GetPlayerName();

//         if (playerName.Length > charLimit)
//         {
//             playerName = playerName.Substring(0, charLimit);
//         }
        
//         if (playerName.Length <= minChar)
//         {
//             playerName = "Empty";
//         }

//         List<string> roster = sceneManager.player.ImplingRoster;

//         HighscoreEntry entry = new()
//         {
//             player_name = playerName,
//             score = newScore,
//             impling_roster = roster
//         };

//         string jsonData = JsonUtility.ToJson(entry);

//         using UnityWebRequest request = new UnityWebRequest(baseUrl, "POST");
//         byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
//         request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//         request.downloadHandler = new DownloadHandlerBuffer();
//         request.SetRequestHeader("Content-Type", "application/json");

//         yield return request.SendWebRequest();

//         if (request.result != UnityWebRequest.Result.Success)
//             Debug.LogError($"Error: {request.error}");
//         else
//             Debug.Log("Score saved successfully!");
//     }

//     private IEnumerator GetHighscores()
//     {
//         using UnityWebRequest req = UnityWebRequest.Get(baseUrl);
//         yield return req.SendWebRequest();

//         if (req.result == UnityWebRequest.Result.Success)
//         {
//             string json = req.downloadHandler.text;
//             Debug.Log("Highscores JSON: " + json);

//             HighscoreEntry[] highscores = JsonHelper.FromJson<HighscoreEntry>(json);

//             foreach (var hs in highscores)
//             {
//                 string roster = hs.impling_roster != null ? string.Join(", ", hs.impling_roster) : "(empty)";
//                 Debug.Log($"{hs.player_name}: {hs.score} | Roster: {roster}");
//             }

//             sceneManager.UpdateScoreDisplay(highscores);
//         }
//         else
//         {
//             Debug.LogError($"Error loading highscores: {req.error}");
//         }
//     }
// }

// // Helper to parse JSON arrays with JsonUtility
// public static class JsonHelper
// {
//     public static T[] FromJson<T>(string json)
//     {
//         string newJson = "{\"Items\":" + json + "}";
//         Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
//         return wrapper.Items;
//     }

//     [System.Serializable]
//     private class Wrapper<T>
//     {
//         public T[] Items;
//     }
// }
