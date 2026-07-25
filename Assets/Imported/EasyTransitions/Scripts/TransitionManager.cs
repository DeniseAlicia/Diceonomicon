using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Linq;

namespace TransitionSystem
{

    public class TransitionManager : MonoBehaviour
    {
        [SerializeField] private GameObject transitionTemplate;

        public bool runningTransition;

        public UnityAction onTransitionBegin;
        public UnityAction onTransitionCutPointReached;
        public UnityAction onTransitionEnd;

        private static TransitionManager Instance;

        [SerializeField] private TransitionSettings transition;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static TransitionManager GetInstance()
        {
            if (Instance == null)
                Debug.LogError("You tried to access the instance before it exists.");

            return Instance;
        }

        /// <summary>
        /// Starts a transition without loading a new level.
        /// </summary>
        /// <param name="transition">The settings of the transition you want to use.</param>
        /// <param name="startDelay">The delay before the transition starts.</param>
        public void Transition(float startDelay)
        {
            if (transition == null || runningTransition)
            {
                Debug.LogError("You have to assing a transition.");
                return;
            }

            runningTransition = true;
            StartCoroutine(Timer(startDelay));
        }

        /// <summary>
        /// Loads the new Scene with a transition.
        /// </summary>
        /// <param name="sceneName">The name of the scene you want to load.</param>
        /// <param name="transition">The settings of the transition you want to use to load you new scene.</param>
        /// <param name="startDelay">The delay before the transition starts.</param>
        public void Transition(string sceneName, float startDelay)
        {
            if (transition == null || runningTransition)
            {
                transition = Resources.Load<TransitionSettings>($"Transitions/Noise");
            }

            runningTransition = true;
            StartCoroutine(Timer(sceneName, startDelay));
        }

        /// <summary>
        /// Loads the new Scene with a transition.
        /// </summary>
        /// <param name="sceneIndex">The index of the scene you want to load.</param>
        /// <param name="transition">The settings of the transition you want to use to load you new scene.</param>
        /// <param name="startDelay">The delay before the transition starts.</param>
        public void Transition(int sceneIndex, float startDelay)
        {
            if (transition == null || runningTransition)
            {
                transition = Resources.Load<TransitionSettings>($"Transitions/Noise");
            }

            runningTransition = true;
            StartCoroutine(Timer(sceneIndex, startDelay));
        }

        public void Transition(string sceneName, float startDelay, bool async)
        {
            if (transition == null || runningTransition)
            {
                transition = Resources.Load<TransitionSettings>($"Transitions/Noise");
            }

            runningTransition = true;
            StartCoroutine(Timer(sceneName, startDelay, async));
        }

        /// <summary>
        /// Gets the index of a scene from its name.
        /// </summary>
        /// <param name="sceneName">The name of the scene you want to get the index of.</param>
        int GetSceneIndex(string sceneName)
        {
            return SceneManager.GetSceneByName(sceneName).buildIndex;
        }

        IEnumerator Timer(string sceneName, float startDelay)
        {
            yield return new WaitForSecondsRealtime(startDelay);

            onTransitionBegin?.Invoke();

            GameObject template = Instantiate(transitionTemplate) as GameObject;
            template.GetComponent<Transition>().transitionSettings = transition;

            float transitionTime = transition.transitionTime;
            if (transition.autoAdjustTransitionTime)
                transitionTime = transitionTime / transition.transitionSpeed;

            yield return new WaitForSecondsRealtime(transitionTime);

            onTransitionCutPointReached?.Invoke();


            SceneManager.LoadScene(sceneName);

            yield return new WaitForSecondsRealtime(transition.destroyTime);

            onTransitionEnd?.Invoke();
        }

        IEnumerator Timer(string sceneName, float startDelay, bool async)
        {
            yield return new WaitForSecondsRealtime(startDelay);

            onTransitionBegin?.Invoke();

            GameObject template = Instantiate(transitionTemplate) as GameObject;
            template.GetComponent<Transition>().transitionSettings = transition;

            float transitionTime = transition.transitionTime;
            if (transition.autoAdjustTransitionTime)
                transitionTime = transitionTime / transition.transitionSpeed;

            yield return new WaitForSecondsRealtime(transitionTime);

            onTransitionCutPointReached?.Invoke();


            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

            yield return new WaitForSecondsRealtime(transition.destroyTime);

            onTransitionEnd?.Invoke();
        }

        IEnumerator Timer(int sceneIndex, float startDelay)
        {
            yield return new WaitForSecondsRealtime(startDelay);

            onTransitionBegin?.Invoke();

            GameObject template = Instantiate(transitionTemplate) as GameObject;
            template.GetComponent<Transition>().transitionSettings = transition;

            float transitionTime = transition.transitionTime;
            if (transition.autoAdjustTransitionTime)
                transitionTime = transitionTime / transition.transitionSpeed;

            yield return new WaitForSecondsRealtime(transitionTime);

            onTransitionCutPointReached?.Invoke();

            SceneManager.LoadScene(sceneIndex);

            yield return new WaitForSecondsRealtime(transition.destroyTime);

            onTransitionEnd?.Invoke();
        }

        IEnumerator Timer(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);

            onTransitionBegin?.Invoke();

            GameObject template = Instantiate(transitionTemplate) as GameObject;
            template.GetComponent<Transition>().transitionSettings = transition;

            float transitionTime = transition.transitionTime;
            if (transition.autoAdjustTransitionTime)
                transitionTime = transitionTime / transition.transitionSpeed;

            yield return new WaitForSecondsRealtime(transitionTime);

            onTransitionCutPointReached?.Invoke();

            template.GetComponent<Transition>().OnSceneLoad(SceneManager.GetActiveScene(), LoadSceneMode.Single);

            yield return new WaitForSecondsRealtime(transition.destroyTime);

            onTransitionEnd?.Invoke();

            runningTransition = false;
        }

        private IEnumerator Start()
        {
            while (this.gameObject.activeInHierarchy)
            {
                //Check for multiple instances of the Transition Manager component
                var managerCount = FindObjectsByType<TransitionManager>(FindObjectsSortMode.None).Count();
                if (managerCount > 1)
                    Debug.LogError($"There are {managerCount.ToString()} Transition Managers in your scene. Please ensure there is only one Transition Manager in your scene or overlapping transitions may occur.");

                yield return new WaitForSecondsRealtime(1f);
            }
        }
    }

}
