using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public static LoadingScreen instance;
    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);
        instance = this;

        EventHolder.OnLoadingScreenEnabled += EnableLoadingScreen;
    }

    private void OnDestroy()
    {
        EventHolder.OnLoadingScreenEnabled -= EnableLoadingScreen;
    }

    private void EnableLoadingScreen(bool val)
    {
        anim.SetBool("ShowClouds", val);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
