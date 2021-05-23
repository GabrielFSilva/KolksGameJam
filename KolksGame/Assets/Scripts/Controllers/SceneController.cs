using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region Fields

    private Scene m_CurrentScene;
    private Scene m_PreviousScene;
    private Scene m_PersistentScene;

    private AsyncOperation asyncOperation = null;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        m_PersistentScene = SceneManager.GetActiveScene();
    }

    #endregion

    public void Initialize()
    {
        LoadScene(1);
    }

    public void LoadScene(int sceneBuildIndex)
    {
        StartCoroutine(LoadAndSetActive(sceneBuildIndex));
    }

    public void ChangeScene(int sceneBuildIndex)
    {
        if(m_CurrentScene.IsValid())
        {
            StartCoroutine(UnloadAndLoadNewScene(sceneBuildIndex));
        }
        else
        {
            StartCoroutine(LoadAndSetActive(sceneBuildIndex));
        }
    }

    private IEnumerator LoadAndSetActive(int sceneIndex)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        Scene loadedScene = SceneManager.GetSceneByBuildIndex(sceneIndex);

        if(loadedScene.IsValid())
        {
            m_CurrentScene = loadedScene;
            SceneManager.SetActiveScene(loadedScene);
        }
        else
        {
            Debug.LogError($"Failed to load specified scene: {loadedScene.name}");
        }
    }

    private IEnumerator UnloadAndLoadNewScene(int sceneIndex)
    {
        m_PreviousScene = m_CurrentScene;
        //Tornar a cena persistente como ativa
        SceneManager.SetActiveScene(m_PersistentScene);
        //Iniciar operação assíncrona para descarregar cena atual
        asyncOperation = SceneManager.UnloadSceneAsync(m_CurrentScene);
        
        //Fazer fade out (ou transição) aqui
        
        //Retornar a esse ponto enquanto a cena não é descarregada
        while(!asyncOperation.isDone)
        {
            yield return null;
        }

        yield return LoadAndSetActive(sceneIndex);
    }
}
