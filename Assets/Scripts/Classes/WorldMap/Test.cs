using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Q))
        {
             SceneManager.LoadScene("Map", LoadSceneMode.Single);
        }
    }
}
