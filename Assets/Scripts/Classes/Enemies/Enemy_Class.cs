namespace Diceonomicon
{
    using UnityEngine;

    public abstract class Enemy : Entity
    {
        public string description;
        public BattleTablet battleTablet;

        public abstract void Trait();

        public abstract void DicePlacement();
    }
}