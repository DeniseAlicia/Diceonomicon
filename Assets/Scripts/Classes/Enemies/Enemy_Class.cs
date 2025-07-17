namespace Diceonomicon
{
    using UnityEngine;

    public abstract class Enemy : Entity
    {
        public string Description;
        public BattlesTablet BattleTablet;

        public abstract void Trait()
        {
            Debug.Log("Enemy.Trait");
        }

        public abstract void DicePlacement()
        {
            Debug.Log("Enemy.DicePlacement");
        }
    }
}