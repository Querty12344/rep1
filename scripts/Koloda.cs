using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koloda : MonoBehaviour
{
    private List<Cart> _allAvailibleCarts = new List<Cart>();
    [SerializeField] private RectTransform _deletePlace;
    [SerializeField] private RectTransform _kolodaPlace;
    private List<Cart> _allCarts = new List<Cart>();
    [SerializeField] private RectTransform _kozirPlace;
    private Cart _cart;
    public int AvailibleCartCount()
    {
        return _allAvailibleCarts.Count;
    }
    private void Awake()
    {
        
        var carts = GameObject.FindObjectsOfType<Cart>();
        for(int i = 0; i < carts.Length; i++)
        {
            _allAvailibleCarts.Add(carts[i]);
            _allCarts.Add(carts[i]);
        }
        


        

    }
    private void Start()
    {
        int cartIndex = Random.Range(0, _allAvailibleCarts.Count);
        _cart = _allAvailibleCarts[cartIndex];
        _allAvailibleCarts.Remove(_cart);
        FindObjectOfType<GameTable>().SetSpecialMust(_cart.GetMust());


        _cart.ChangePlace(_kozirPlace.position);



        _cart.SetOwner(null);
    }
    public void AddCarts(Player player,int count)
    {
        for(int i = 0;i< count; i++)
        {
            if(_allAvailibleCarts.Count!=0)
            {
                int cartIndex = Random.Range(0, _allAvailibleCarts.Count);
                player.AddCart(_allAvailibleCarts[cartIndex]);
                _allAvailibleCarts.RemoveAt(cartIndex);
            }
            
        }
    }
    public void DeliteCarts(List<Cart> carts)
    {
        for(int i =0;i<carts.Count; i++)
        {
            carts[i].ChangePlace(_deletePlace.position);
        }
        carts.Clear();
    }
    public void RestartGame()
    {

        _cart.ChangePlace(_kolodaPlace.position);
        _allAvailibleCarts = new List<Cart>();
        _allCarts = new List<Cart>();

        var carts = GameObject.FindObjectsOfType<Cart>();
        for (int i = 0; i < carts.Length; i++)
        {
            _allAvailibleCarts.Add(carts[i]);
            _allCarts.Add(carts[i]);
        }
        for (int i = 0; i < _allCarts.Count; i++)
        {
            _allCarts[i].Clear(_kolodaPlace.position);

        }
        int cartIndex = Random.Range(0, _allAvailibleCarts.Count);
        _cart = _allAvailibleCarts[cartIndex];
        _allAvailibleCarts.Remove(_cart);
        FindObjectOfType<GameTable>().SetSpecialMust(_cart.GetMust());


        _cart.ChangePlace(_kozirPlace.position);



        _cart.SetOwner(null);

    }
}
