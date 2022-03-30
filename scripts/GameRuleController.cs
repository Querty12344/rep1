using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRuleController : MonoBehaviour
{
    [SerializeField]private Player[] _players;
    private List<Player> _activePlayers = new List<Player>();
    [SerializeField] private int _playerCount;
    [SerializeField] private GameTable _gameTable;
    [SerializeField] private Koloda _koloda;
    [SerializeField] private Player _attackPlayer;
    [SerializeField] private Player _defusePlayer;
    private bool _attackCircleNotEnded;
    private int _currentCircleIndex;
    private bool _playerZagreb;
    
    private void Start()
    {
        StartGame();
        
    }
    private void StartGame()
    {
        InitPlayers();
        StartHode();
    }
    public void StartAttackCircle()
    {
        if (_attackCircleNotEnded)
        {
            _currentCircleIndex = 0;
        }
        if (_currentCircleIndex >= _activePlayers.Count -1)
        {
            
            _attackCircleNotEnded = false;
            _currentCircleIndex = 0;
            StartHode();
        }
        
        if(_currentCircleIndex < _activePlayers.Count)
        {
            if(_defusePlayer != _attackPlayer)
            {
                OnAttackPlayer();
            }
            else
            {
                
                _attackPlayer = _activePlayers[GiveNextPlayer(_activePlayers.IndexOf(_attackPlayer))];
                OnAttackPlayer();
            }
        }
        
        
    }
    public void EndHod(Player player)
    {
        if (player.GetGameState() == 1)
        {
            _playerZagreb = true;
            StartHode();
        }
        else
        {
            _attackCircleNotEnded = false;
            OffAllPlayers();
            _attackPlayer = _activePlayers[GiveNextPlayer(_activePlayers.IndexOf(_attackPlayer))]; 
            _currentCircleIndex++;
            StartAttackCircle();
        }
    }
    public void AutoEndAttackHod()
    {
        
        OffAllPlayers();
        OnDefusePlayer();
        _attackCircleNotEnded = true;
    }
    public void AutoEndDefuseHod()
    {
        OffAllPlayers();
        OnAttackPlayer();
    }
    private void StartHode()
    {
        
        OffAllPlayers();
        if (_playerZagreb)
        {
            _gameTable.GrebiSuka(_defusePlayer);
            _attackPlayer = _activePlayers[GiveNextPlayer(_activePlayers.IndexOf(_defusePlayer))];
            _defusePlayer = _activePlayers[GiveNextPlayer(_activePlayers.IndexOf(_attackPlayer))];
        }
        else
        {
            
            _gameTable.ClearTable();
            _attackPlayer = _defusePlayer;
            _defusePlayer = _activePlayers[GiveNextPlayer(_activePlayers.IndexOf(_attackPlayer))]; 
        }
        _currentCircleIndex = 0; 
        _attackCircleNotEnded = false;
        _playerZagreb = false;
        for(int i = 0; i< _activePlayers.Count; i++)
        {
            AddRandomCart(_activePlayers[i]);
        }
        OnAttackPlayer();
    }
    private void OnAttackPlayer()
    {
        _gameTable.SetActivePlayer(_attackPlayer);
        _defusePlayer.OnDefoultState();
        _attackPlayer.OnAttackState();
       
    }
    private void OnDefusePlayer()
    {
        _gameTable.SetActivePlayer(_defusePlayer);
        _attackPlayer.OnDefoultState();
        _defusePlayer.OnDefuseState();
    }
    private void OffAllPlayers()
    {
        _attackPlayer.OnDefoultState();
        _defusePlayer.OnDefoultState();
    }
    private int GiveNextPlayer(int currentPlayerIndex)
    {
        return currentPlayerIndex == _activePlayers.Count - 1 ? 0:currentPlayerIndex + 1;
    }
    private void InitPlayers()
    {
        for(int i = 0; i < _playerCount; i++)
        {
            _activePlayers.Add(_players[i]);
            
        }
        _attackPlayer = _activePlayers[0];
        _defusePlayer = _activePlayers[1];
    }
    private void AddRandomCart(Player player)
    {
        _koloda.AddCarts(player,player.NeedCartCount());
    }
    public void RestartGame()
    {
        OffAllPlayers();
        _gameTable.ClearTable();
        _koloda.RestartGame();
        for (int i = 0; i < _activePlayers.Count; i++)
        {
            _activePlayers[i].ClearHand();
            
        }
        _activePlayers= new List<Player>();
        
        
        

        
        
        StartGame();


    }
}
