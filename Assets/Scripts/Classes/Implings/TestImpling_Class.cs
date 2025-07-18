namespace Diceonomicon
{
    using UnityEngine;

    public class TestImpling : RoundStartImpling
    {
        public override void OnRoundStart()
        {
            Debug.Log("New Round begins");
        }
    }
}

