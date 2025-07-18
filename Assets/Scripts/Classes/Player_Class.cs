namespace Diceonomicon
{
    using UnityEngine;

    public class Player : Entity
    {
        public Impling[] ImplingRoster;
        public Impling[] ActiveImplings;
        public int MaxImplings;

        private void Awake()
        {
            Debug.Log("Player Start");
            health = 50;
        }
        public void Test()
        {
            Debug.Log("PlayerTest");
            Debug.Log(health);
        }
    }
}