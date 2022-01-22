using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScoreManager : MonoBehaviour {
    public static ScoreManager instance;

    public string currentName;
    public string bestName = "";
    public int bestScore = 0;

    public Text bestScoreText;
    public InputField input;

    string savePath;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Application.persistentDataPath + "savefile.json";
        LoadScore();
        bestScoreText.text = $"Best Score: {bestName} {bestScore}";
    }

    public void StartNewGame() {
        currentName = input.text;
        SceneManager.LoadScene(1);
    }

    public void Exit() {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [System.Serializable]
    class SaveData {
        public string bestName;
        public int bestScore;
    }

    public void SaveScore() {
        SaveData data = new SaveData();
        data.bestName = currentName;
        data.bestScore = bestScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(savePath, json);
    }

    public void LoadScore() {
        if (File.Exists(savePath)) {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestName = data.bestName;
            bestScore = data.bestScore;
        }
    }
}
