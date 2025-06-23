using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // when play next button is clicked
    public void OnPlayNextClick()
    {
        SceneManager.LoadScene(1);
    }
    // when list of levels button is clicked
    public void OnLevelListClick()
    {
        Debug.Log("LevelListClick");
    }
    // when credits button is clicked
    public void OnCreditsClick()
    {
        //Debug.Log("OnCreditsClick");
        SceneManager.LoadScene(0);
    }
}
