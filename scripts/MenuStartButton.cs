using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStartButton : MonoBehaviour
{
    [SerializeField] private GameObject _animator;
    
    public void RestartGame()
    {
        FindObjectOfType<GameRuleController>().GetComponent<GameRuleController>().RestartGame();
        Destroy(_animator);
    }
}
