using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    public void LoadGameplayScene()
    {
        EventHolder.TriggerLoadingScreen(true);
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        yield return new WaitForSeconds(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            if(asyncLoad.progress >= 0.9f)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        asyncLoad.allowSceneActivation = true;


    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
