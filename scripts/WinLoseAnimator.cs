using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseAnimator : MonoBehaviour
{
    [SerializeField] private GameObject _WinAnimationObject;
    [SerializeField] private GameObject _LoseAnimationObject;
    [SerializeField] private RectTransform _transform;
    public void RunAnimation(Player player)
    {
        if(player.TryGetComponent<BotPlayer>(out var botPlayer))
        {
            Instantiate(_LoseAnimationObject, _transform);
        }
        else
        {
            Instantiate(_WinAnimationObject, _transform);
        }
    }
    
}
