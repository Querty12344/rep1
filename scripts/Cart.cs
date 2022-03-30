using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cart : MonoBehaviour
{
    [SerializeField] private int _nominal;
    [SerializeField] private int _must;
    [SerializeField] private Player _owner;
    [SerializeField] private Sprite _backSide;
    private Sprite _basicSprite;
    private CartMovingScript _movingScript;
    private void Awake()
    {
        _basicSprite = GetComponent<Image>().sprite;
        _movingScript = GetComponent<CartMovingScript>();
    }
    public int GetNominal()
    {
        return _nominal;
    }
    public int GetMust()
    {
        return _must;
    }
    public void ChangePlace(Vector2 newPlace)
    {
        _movingScript = GetComponent<CartMovingScript>();
        _movingScript.ChangeTarget(newPlace);
    }
    public void MovePlace(Vector2 newPlace)
    {
        _movingScript.MoveTarget(newPlace);
    }
    public void SetOwner(Player player)
    {
        if(player != null)
        {
            if (player.gameObject.TryGetComponent<BotPlayer>(out var x))
            {
                GetComponent<Image>().sprite = _backSide;
            }
            else
            {
                GetComponent<Image>().sprite = _basicSprite;
            }
        }
        
        else
        {
            GetComponent<Image>().sprite = _basicSprite;
        }
        
        _owner = player;
        
    }
    public Player GetOwner()
    {
        return _owner;
    }
    public bool IfbotCart()
    {
        if(_owner != null)
        {
            if(_owner.TryGetComponent<BotPlayer>(out var botPlayer))
            {
                if(this == botPlayer.GetChoosenCart())
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void Clear(Vector2 kolodaPos)
    {
        _owner = null;
        ChangePlace(kolodaPos);
    }
    
}
