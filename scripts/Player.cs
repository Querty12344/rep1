using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private List<Cart> _hand = new List<Cart>();
    [SerializeField] private float _cartPositionOffset;
    [SerializeField] private int _state;

    [SerializeField] private GameTable _gameTable;
    [SerializeField] private GameRuleController ruleController;
    [SerializeField] private float _cartOffsetCof;
    [SerializeField] private Koloda _koloda;
    public void EndHod()
    {

        if(_state == 1 || _state == -1 && _gameTable.CanEndAttackHode())
        {
            ruleController.EndHod(this);
        }
        
    }
    private void OnTriggerEnter2D()
    {
        UpdateLayers();
    }
    
    public void AddCart(Cart cart)
    {
        _hand.Add(cart);
        cart.SetOwner(this);
        for(int i = 0; i< _hand.Count; i++)
        {
            _hand[i].SetOwner(this);
        }
        TryChangeCartsOffset();
        UpdateLayers();


    }
    public void GetCart(Cart cart)
    {
        
        _hand.Remove(cart);
        cart.SetOwner(null);
        TryChangeCartsOffset();
        UpdateLayers();
        

    }
    private void UpdateLayers()
    {
        for(int i = 0;i< _hand.Count; i++)
        {
            _hand[i].gameObject.transform.SetSiblingIndex(99);
        }
        
        _hand[0].gameObject.transform.SetSiblingIndex(99);
        
        
    }
    public int NeedCartCount()
    {
        return 6 - _hand.Count;
    }
    private Vector2 GiveCartPlace(float offset)
    {
        return new Vector2(GetComponent<RectTransform>().position.x + _hand.Count * offset / 2 - offset / 2, GetComponent<RectTransform>().position.y);
    }
    
    private void ReplaceCarts(int removedCartIndex,float cof,float offset)
    {
        for(int i =0;i < _hand.Count; i++)
        {
            if(i > removedCartIndex)
            {
                _hand[i].MovePlace(cof*new Vector2(offset, 0));
            }
        }
        
    }
    
    private void TryChangeCartsOffset()
    {
        for (int i = 0; i < _hand.Count && _cartOffsetCof!=0; i++)
        {
                _hand[i].ChangePlace(GiveCartPlace(_cartOffsetCof/_hand.Count));
                ReplaceCarts(0,-1f,_cartOffsetCof / _hand.Count);
        }
        
            
        



    }
    private void RebuildHand(int cartIndex)
    {
        
    }
    public int GetGameState()
    {
        return _state;
    }
    public void OnAttackState()
    {
        _state = -1;
        StartAutoHode(_state);
    }

    public void OnDefuseState()
    {
        _state = 1;
        StartAutoHode(_state);
    }

    public void OnDefoultState() => _state = 0;
    public virtual void StartAutoHode(int state) { }
    public Cart FindDefuseCart()
    {
        
        Cart smallestCart = null;
        int currentNominal;
        int smallestNominal = 999999;


        for (int i = 0; i < _hand.Count; i++)
        {
            currentNominal = _hand[i].GetMust() == _gameTable.GetSpecialMust() ? _hand[i].GetNominal() + 1000 : _hand[i].GetNominal();
            if (currentNominal < smallestNominal && _gameTable.DefuseCartCheck(_gameTable.GetAttackCart(), _hand[i]))
            {
                smallestCart = _hand[i];
                smallestNominal = currentNominal;
            }
        }
        return smallestCart;
    }
    
    public Vector2 GetFutureCartPos()
    {
        return _gameTable.GiveFutureCartPlace();
    }
    public Cart FindAttackCart()
    {
        Cart smallestCart = null;
        int currentNominal;
        int smallestNominal = 999999;
        
        
        for(int i =0;i< _hand.Count; i++)
        {
            currentNominal = _hand[i].GetMust() == _gameTable.GetSpecialMust() ? _hand[i].GetNominal() +1000 : _hand[i].GetNominal();
            if (currentNominal < smallestNominal && _gameTable.AttackCartCheck(_hand[i]))
            {
                smallestCart = _hand[i];
                smallestNominal = currentNominal;
            }
        }
        return smallestCart;
    }
    public void ClearHand()
    {
        _state = 0;
        var count = _hand.Count;
        for (int i = 0; i < count; i++)
        {
            
            _hand.Remove(_hand[0]);

        }
    }
    public bool NoneCart()
    {
       
        if(_koloda.AvailibleCartCount() == 0 && _hand.Count == 1)
        {
            return true;
        }
        return false;
    }
    

}
