using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObiRopePoint : MonoBehaviour
{
    [SerializeField] private ObiRope rope;
    [SerializeField] private int indexToFollow;
    [SerializeField] private bool fromEnd;

    private ObiSolver solver;

    private void OnEnable()
    {
        StartCoroutine(WaitEnable());
    }

    private void OnDisable()
    {
        if (!ReferenceEquals(solver, null))
            solver.OnEndStep -= UpdatePosition;
    }

    private IEnumerator WaitEnable()
    {
        yield return new WaitForEndOfFrame();
        solver = rope.solver;
        if (!ReferenceEquals(solver, null))
            solver.OnEndStep += UpdatePosition;
    }

    private void UpdatePosition(ObiSolver solver)
    {
        int targetParticle = fromEnd ? rope.activeParticleCount - indexToFollow - 1: indexToFollow;
        int targetElement, actualParticle;
        if (targetParticle >= rope.elements.Count)
        {
            targetElement = targetParticle - 1;
            actualParticle = rope.elements[targetElement].particle2;
        }
        else
        {
            targetElement = targetParticle;
            actualParticle = rope.elements[targetElement].particle1;
        }
        transform.position = rope.GetParticlePosition(actualParticle);
    }
}