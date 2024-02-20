using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
       
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("It Hit");
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            if(LevelLoading.Instance == null) SceneManager.LoadScene(currentSceneIndex);
            else LevelLoading.Instance.LoadScene(currentSceneIndex);
        }
    }
}
