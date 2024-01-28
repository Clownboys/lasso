using UnityEngine;
using Obi;

public class Grabber : MonoBehaviour
{
	private ParticleFinder hook;
	private int iHeld;

	[SerializeField] private float switchHandRadius = 0.05f;

	private void Awake()
	{
		hook = GetComponent<ParticleFinder>();
	}

	private void OnEnable()
	{
		hook.rope.solver.OnSubstep += Substep;
		hook.rope.solver.OnEndStep += EndStep;

		prevPosition = transform.position;

		if(Vector3.Distance(transform.position, hook.transform.position) < switchHandRadius)
		{
			iHeld = hook.rope.elements[hook.iElementClosest + hook.limitSign].particle1;
		} else
		{
			iHeld = hook.iClosest;
		}
	}

	private void OnDisable()
	{
		hook.rope.solver.OnSubstep -= Substep;
		hook.rope.solver.OnEndStep -= EndStep;
	}

	private Vector3 prevPosition;

	private void Substep(ObiSolver solver, float stepTime)
	{
		solver.velocities[iHeld] = (transform.position - prevPosition) / stepTime;
		solver.positions[iHeld] = transform.position;
	}

	private void EndStep(ObiSolver solver)
	{
		prevPosition = transform.position;
	}
}