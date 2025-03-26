using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Infrastructure
{
    public class AppStartup: MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
}