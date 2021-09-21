using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public SlingShooter SlingShooter;
    public List<Bird> Birds;
    public List<Enemy> Enemies;
    public TrailController trailController;
    private Bird _shotBird;
    public BoxCollider2D TapCollider;    

    public SceneManager SceneManager;

    [SerializeField] private GameObject _panel;
    [SerializeField] private Text _title;
    [SerializeField] private Text _text;

    private bool _isWin = false;
    private bool _isGameEnded = false;

    void Start()
    {
        for (int i = 0; i < Birds.Count; i++)
        {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrail;
        }

        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        TapCollider.enabled = false;
        SlingShooter.InitiateBird(Birds[0]);
        _shotBird = Birds[0];
    }

    private void Update()
    {
        if (_isWin)
        {
            winCondition();
        }

        string sceneName = SceneManager.GetActiveScene().name;
        if (Input.GetKeyDown(KeyCode.R) && _panel.activeSelf)
        {
            if(sceneName == "Level 1")
            {
                sceneName = "Level 2";
                SceneManager.LoadScene(sceneName);
            }

            else if(!_isWin && sceneName == "Level 2")
            {
                sceneName = "Level 2";
                SceneManager.LoadScene(sceneName);
            }

            else
            {
                sceneName = "Level 1";
                SceneManager.LoadScene(sceneName);
            }
        }        
    }

    public void ChangeBird()
    {
        TapCollider.enabled = false;

        Birds.RemoveAt(0);   

        if (Birds.Count > 0)
        {
            SlingShooter.InitiateBird(Birds[0]);
            _shotBird = Birds[0];
        }

        else if(Birds.Count <= 0 && Enemies.Count > 0)
        {
            _isWin = false;
            _isGameEnded = true;
            winCondition();
        }
    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].gameObject == destroyedEnemy)
            {
                Enemies.RemoveAt(i);
                break;
            }
        }

        if (Enemies.Count == 0)
        {
            _isWin = true;
            _isGameEnded = true;            
        }
    }

    public void AssignTrail(Bird bird)
    {
        trailController.SetBird(bird);
        StartCoroutine(trailController.SpawnTrail());
        TapCollider.enabled = true;
    }

    void OnMouseUp()
    {
        if (_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }

    public void winCondition()
    {
        print("Masuk win condition");
        print("win: ");
        print(_isWin);
        print("game end: ");
        print(_isGameEnded);
        string sceneName = SceneManager.GetActiveScene().name;

        if (_isWin && _isGameEnded)
        {
            _title.text = "You Win!";
            
            if(sceneName == "Level 1")
            {
                _text.text = "Tap to Level 2";
                _panel.gameObject.SetActive(true);                
            }
            else
            {
                _text.text = "R to Restart from Level 1!";
                _panel.gameObject.SetActive(true);
            }
        }

        else if(!_isWin && _isGameEnded)
        {
            _title.text = "You Lose!";
            _text.text = "R to Restart!";
            _panel.gameObject.SetActive(true);
        }
    }
}
