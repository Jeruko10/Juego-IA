using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Game;

[GlobalClass]
public partial class BoardState : Node
{
    [Export] Mana Player1StartingMana;
    [Export] Mana Player2StartingMana;

    public Dictionary<Vector2I, Tile> Tiles { get; private set; } = [];
    public List<Minion> Minions { get; private set; } = [];
    public List<Fort> Forts { get; private set; } = [];
    public Mana Player1Mana { get; set; }
    public Mana Player2Mana { get; set; }
    public Vector2I? SelectedCell { get; set; }
    public Minion SelectedMinion { get; private set; }
    public bool AttackMode { get; private set; }
    public event Action<Board.Players> TurnStarted;
    public event Action<Minion> MinionDeath;
    public event Action<Minion, int> MinionDamaged;
    public event Action<Minion, Vector2I> MinionAttack;
    public event Action<Minion> MinionRestored;
    public event Action<Minion, Vector2I[]> MinionMoved;
    public event Action<Minion> MinionAdded;
    public event Action<Minion> MinionUnselected;
    public event Action<Minion> MinionSelected;
    public event Action<bool> AttackModeToggled;
    public event Action<Fort, Minion> FortDominated;
    public event Action<Fort> FortHarvested;
    public event Action<Tile, Vector2I> TileAdded;
    public event Action<Fort> FortAdded;

    bool isPlayer1Turn = false;

    public struct CellData(Tile tile, Minion minion, Fort fort)
    {
        public Tile Tile { get; private set; } = tile;
        public Minion Minion { get; private set; } = minion;
        public Fort Fort { get; private set; } = fort;
    }

    public override void _Ready()
    {
        Player1Mana = Player1StartingMana;
        Player2Mana = Player2StartingMana;

        PassTurn();
        GodotExtensions.CallDeferred(CreateBoard);
    }

    public Board.Players GetActivePlayer() => isPlayer1Turn ? Board.Players.Player1 : Board.Players.Player2;

    public void SelectMinion(Minion minion)
    {
        SelectedMinion = minion;
        MinionSelected?.Invoke(minion);
    }

    public void UnselectMinion()
    {
        Minion oldSelection = SelectedMinion;
        SelectedMinion = null;
        AttackMode = false;
        MinionUnselected?.Invoke(oldSelection);
    }

    public void SetAttackMode(bool value)
    {
        AttackMode = value;
        AttackModeToggled?.Invoke(value);
    }

    public void PassTurn()
    {
        isPlayer1Turn = !isPlayer1Turn;
        Board.Players oldTurnOwner = Board.GetRival(GetActivePlayer());
        Board.Players newTurnOwner = GetActivePlayer();
        InputHandler.InteractionEnabled = false;
        UnselectMinion();
        SelectedCell = null;

        foreach (Minion minion in Minions)
            if (minion.Owner == oldTurnOwner)
                RestoreMinion(minion);
        
        foreach (Fort fort in Forts)
            if (fort.Owner == oldTurnOwner)
                HarvestMana(fort);

        TurnStarted?.Invoke(newTurnOwner);
    }

    public CellData GetCellData(Vector2I cell)
    {
        Tile tile = Tiles.GetValueOrDefault(cell);
        Minion minion = Minions.FirstOrDefault(m => m.Position == cell);
        Fort fort = Forts.FirstOrDefault(f => f.Position == cell);

        return new CellData(tile, minion, fort);
    }
}
