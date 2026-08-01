using UnityEngine;
using TransitionSystem;

public static class SceneTransition
{
    public static void Load(string sceneName, int startDelay = 0)
    {
        TransitionManager.GetInstance().Transition(sceneName, startDelay);
    }

    public static void LoadAsync(string sceneName, int startDelay = 0, bool async = true)
    {
        TransitionManager.GetInstance().Transition(sceneName, startDelay, async);
    }
}
