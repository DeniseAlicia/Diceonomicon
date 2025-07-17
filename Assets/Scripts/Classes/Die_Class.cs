namespace Diceonomicon
{
    using UnityEngine;

    public class Die : MonoBehaviour
    {
        public int[] Range; //which values the die can have
        public int Value; //which value the die rolled this round
        public string Tag;
        public Vector3 LastPosition; 
}
}