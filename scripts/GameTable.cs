using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTable : MonoBehaviour
{
    private List<Cart> _allCartOnGame = new List<Cart>();
    private Cart _activeAttackCart;
    private Player _activePlayer;
    private Cart _nullCart;
    [SerializeField] private GameRuleController _gameRule;
    [SerializeField] private WinLoseAnimator _animator;
    [SerializeField] private Koloda _koloda;
    private int _specialMust;
    [SerializeField] private RectTransform[] _cartPlaces;
    private int _cartPlaceIndex;
    private Cart _currentGetCart;
    public void SetSpecialMust(int must)
    {
        _specialMust = must;
    }
    public int GetSpecialMust()
    {
        return _specialMust;
    }
    public bool CanEndAttackHode()
    {
        return _activeAttackCart != _nullCart;
    }
    public void SetActivePlayer(Player player)
    {
        _activePlayer = player;
    }
    public void TryGetCart(Player player,Cart cart) 
    {
        if (player!= null)
        {
            if (player.GetGameState() != 0)
            {
                if (_activePlayer.GetGameState() == -1)
                {
                    if (_allCartOnGame.Count == 0)
                    {
                        ChangeAttackCart(cart, player);
                    }
                    else
                    {

                        for (int i = 0; i < _allCartOnGame.Count; i++)
                        {
                            if (cart.GetNominal() == _allCartOnGame[i].GetNominal())
                            {
                                ChangeAttackCart(cart, player);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (DefuseCartCheck(_activeAttackCart, cart))
                    {
                        DefuseAttackCart(cart, player);
                    }
                }
            }
        }
        
    }
    
    public void ClearTable()
    {
        _cartPlaceIndex = 0;
        _koloda.DeliteCarts(_allCartOnGame);

        _activeAttackCart = _nullCart;

    }
    public bool AttackCartCheck(Cart cart)
    {
        if (_allCartOnGame.Count == 0)
        {
            return true;
        }
        for (int i = 0; i < _allCartOnGame.Count; i++)
        {
            if (cart.GetNominal() == _allCartOnGame[i].GetNominal())
            {
                return true;
            }
        }
        return false;
    }
    public bool DefuseCartCheck(Cart attackCart, Cart defuseCart)
    {
        if(attackCart.GetNominal() < defuseCart.GetNominal() && attackCart.GetMust() == defuseCart.GetMust())
        {
            return true;
        }
        if (attackCart.GetMust() != _specialMust && defuseCart.GetMust() == _specialMust)
        {
            return true;
        }
        return false;
    }
    public Cart GetAttackCart()
    {
        return _activeAttackCart;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Cart>(out var cart))
        {
            _currentGetCart = cart;
            _currentGetCart.gameObject.transform.SetSiblingIndex(99);
            if (cart.gameObject.GetComponent<CartMovingScript>().CheckIfPlayerMove() || cart.IfbotCart())
            {
                cart.gameObject.transform.SetSiblingIndex(99);
                TryGetCart(cart.GetOwner(), cart);
                _currentGetCart = null;
            }
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Cart>(out var cart))
        {
            if(cart == _currentGetCart)
            {
                _currentGetCart = null;
            }
        }
    }
    private void ChangeAttackCart(Cart cart, Player owner)
    {
        if (owner.NoneCart())
        {
            
            _animator.RunAnimation(owner);
            return;
        }
        
        _activeAttackCart = cart;
        _allCartOnGame.Add(cart);

        owner.GetCart(cart);
        cart.ChangePlace(GiveCartPlace());
        _gameRule.AutoEndAttackHod();


    }
    private void DefuseAttackCart(Cart cart,Player owner)
    {
        
      
        _allCartOnGame.Add(cart);
        owner.GetCart(cart);
        cart.ChangePlace(GiveCartPlace());
        _gameRule.AutoEndDefuseHod();
    }
    public Vector2 GiveFutureCartPlace()
    {
        Vector2 position;
        if (_cartPlaceIndex == _cartPlaces.Length)
        {
            position = _cartPlaces[_cartPlaceIndex - 1].position;
        }
        else
        {
            position = _cartPlaces[_cartPlaceIndex].position;
            
        }

        return position;
    }
    private Vector2 GiveCartPlace()
    {
        Vector2 position;
        if (_cartPlaceIndex == _cartPlaces.Length)
        {
            position = _cartPlaces[_cartPlaceIndex-1].position;
        }
        else
        {
            position = _cartPlaces[_cartPlaceIndex].position;
            _cartPlaceIndex++;
        }
        
        return position;
    }
    public void GrebiSuka(Player lox)
    {
        int count = _allCartOnGame.Count;
        for (int i  =0; i<count; i++)
        {
            lox.AddCart(_allCartOnGame[0]);
            _allCartOnGame.RemoveAt(0);
        }
        ClearTable();
    }
    private void Update()
    {
        if(_currentGetCart != null)
        {
            if (_currentGetCart.gameObject.GetComponent<CartMovingScript>().CheckIfPlayerMove() || _currentGetCart.IfbotCart())
            {
                _currentGetCart.gameObject.transform.SetSiblingIndex(99);
                TryGetCart(_currentGetCart.GetOwner(), _currentGetCart);
                _currentGetCart = null;
            }
        }
    }


}
