using UnityEngine;

public class GoToEnding : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EndingController endingController = animator.GetComponent<EndingController>();
        SceneTransitionManager sceneTransitionManager = endingController.sceneTransitionManager;
        sceneTransitionManager.GoToSceneAsync("ResultWin");
    }
}