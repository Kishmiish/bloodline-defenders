using UnityEngine;

public class PreBuildProcessor : MonoBehaviour
{
    void Awake()
    {
        PlayerPrefs.DeleteAll();
    }
}
