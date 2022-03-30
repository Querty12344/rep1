using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPlayer : Player
{
    private Cart _choosenCart;
    
    [SerializeField] private float _botWaitTime;
    public override void StartAutoHode(int state)
    {
        if(state == 1)
        {
            if(FindDefuseCart() != null)
            {
                _choosenCart = FindDefuseCart();
                Invoke("MoveCart", _botWaitTime);
            }
            else
            {
                Invoke("EndHod", _botWaitTime);
            }
        }
        else
        {
            if(FindAttackCart() != null)
            {
                _choosenCart = FindAttackCart();
                Invoke("MoveCart", _botWaitTime);
            }
            else
            {
                
                Invoke("EndHod", _botWaitTime);
            }
            
        }   
        
    }
    public Cart GetChoosenCart()
    {
        return _choosenCart;
    }
    private void MoveCart()
    {
        if(_choosenCart.GetOwner() == this)
        {
            _choosenCart.gameObject.GetComponent<CartMovingScript>().ChangeTarget(GetFutureCartPos());
        }
        
    }   
}
