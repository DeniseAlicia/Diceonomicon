using UnityEngine;

namespace TransitionSystem
{

    public class DemoLoadScene : MonoBehaviour
    {
        public TransitionSettings transition;
        public float startDelay;

        
        public void LoadScene(string _sceneName)
        {
            TransitionManager.GetInstance().Transition(_sceneName, startDelay);
        }   
    }

}


