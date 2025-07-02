using UnityEngine;

public class PersistantDataManager : MonoBehaviour
{
    public Material defaultCarMaterial; 
    public GameObject carPrefab, gameController;
    public void SaveCarSettings()
    {
        PlayerPrefs.SetFloat("carSize", carPrefab.transform.localScale.x);
        Color color = defaultCarMaterial.color;
        PlayerPrefs.SetString("carColor", ColorUtility.ToHtmlStringRGBA(color));

        // Save position
        Vector3 pos = carPrefab.transform.position;
        PlayerPrefs.SetFloat("carPosX", pos.x);
        PlayerPrefs.SetFloat("carPosY", pos.y);
        PlayerPrefs.SetFloat("carPosZ", pos.z);

        // Save rotation (Euler)
        Vector3 rot = carPrefab.transform.eulerAngles;
        PlayerPrefs.SetFloat("carRotX", rot.x);
        PlayerPrefs.SetFloat("carRotY", rot.y);
        PlayerPrefs.SetFloat("carRotZ", rot.z);

        PlayerPrefs.Save(); // Always call this
    }

    public void LoadCarSettings()
    {
        // Size
        float size = PlayerPrefs.GetFloat("carSize", 1f); // Default scale 1
        carPrefab.transform.localScale = new Vector3(size, size, size);

        // Color
        string colorStr = PlayerPrefs.GetString("carColor", "FFFFFFFF"); // Default white
        Color color;
        if (ColorUtility.TryParseHtmlString("#" + colorStr, out color))
        {
            defaultCarMaterial.color = color;
        }

        // Position
        Vector3 pos = new Vector3(
            PlayerPrefs.GetFloat("carPosX", 0f),
            PlayerPrefs.GetFloat("carPosY", 0f),
            PlayerPrefs.GetFloat("carPosZ", 0f)
        );
        carPrefab.transform.position = pos;

        // Rotation
        Vector3 rot = new Vector3(
            PlayerPrefs.GetFloat("carRotX", 0f),
            PlayerPrefs.GetFloat("carRotY", 0f),
            PlayerPrefs.GetFloat("carRotZ", 0f)
        );
        carPrefab.transform.eulerAngles = rot;

        Debug.Log($"Car settings loaded: Size={size}, Color={color}, Position={pos}, Rotation={rot}");
    }

    private void OnApplicationQuit()
    {
        SaveCarSettings();
    }

    private void Start()
    {
        //if player prefs are available, load them
        if (PlayerPrefs.HasKey("carSize"))
        {
            carPrefab.SetActive(true);
            gameController.SetActive(true);
            LoadCarSettings();
        }
    }

}
