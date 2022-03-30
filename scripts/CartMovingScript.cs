using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMovingScript : MonoBehaviour
{
    [SerializeField] private Player _realPlayer;
    [SerializeField] private float _canGiveTime;
    [SerializeField] private float _speed;
    private Cart _cart;
    private RectTransform _transform;
    private bool _onPlayerMoving;
    private Vector2 _targetPosition;
    private float _timer = 9999f;
    private void Awake()
    {
        _timer = 9999f;
        _cart = GetComponent<Cart>();
        _transform = GetComponent<RectTransform>();

    }
    
    public bool CheckIfPlayerMove()
    {
        if(!_onPlayerMoving && _timer <= _canGiveTime)
        {
            return true;
        }
        return false;
        
    }
    public void OnClickOnCard()
    {
        
        if (_cart.GetOwner() == _realPlayer)
        {
            _onPlayerMoving = true;
        }
    }
    public void MoveTarget(Vector2 target)
    {
        _targetPosition = _targetPosition + target;
    }
    public void ChangeTarget(Vector2 target)
    {
        _targetPosition = target;
    }
    public void OffClickOnCard()
    {
        if (_cart.GetOwner() == _realPlayer)
        {
            _onPlayerMoving = false;
            _timer = 0f;
        }
        
    }
    private void FixedUpdate()
    {
        if (!_onPlayerMoving  && _targetPosition != Vector2.zero)
        {
            
            _transform.position =  (Vector2)_transform.position + ((_targetPosition -  (Vector2)transform.position) * _speed);
        }
        

        if (_onPlayerMoving)
        {
            
            _transform.position = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
            _transform.position = new Vector3(_transform.position.x,_transform.position.y,0);
        }
        if(_timer <= _canGiveTime)
        {
            _timer += Time.deltaTime;
        }
        
    }
    
}
