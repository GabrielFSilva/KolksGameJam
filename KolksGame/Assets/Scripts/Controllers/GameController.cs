using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneController))]
public class GameController : MonoBehaviour
{
    #region Fields

    private GameController s_Instance = null;
    private SceneController m_SceneController = null;

    #endregion

    #region Properties

    public GameController Instance
    {
        get { return s_Instance; }
    }

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        SingletonSetup();
        GetComponentReferences();
    }

    private void Start()
    {
        if (m_SceneController != null)
            m_SceneController.Initialize();
    }

    #endregion

    void SingletonSetup()
    {
        if(s_Instance == null && s_Instance != this)
        {
            s_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void GetComponentReferences()
    {
        m_SceneController = GetComponent<SceneController>();
    }
}
