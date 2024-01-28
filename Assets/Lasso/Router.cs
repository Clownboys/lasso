using UnityEngine;
using Obi;

public class Router : MonoBehaviour
{
	private ParticleFinder hook;

	private void Awake()
	{
		hook = GetComponent<ParticleFinder>();
	}

	private void OnEnable()
	{
		hook.rope.solver.OnSubstep += Substep;
		hook.rope.solver.OnEndStep += EndStep;

		prevPosition = transform.position;
	}

	private void OnDisable()
	{
		hook.rope.solver.OnSubstep -= Substep;
		hook.rope.solver.OnEndStep -= EndStep;
	}

	private Vector3 prevPosition;

	private void Substep(ObiSolver solver, float stepTime)
	{

		Vector3 diff = (Vector3)solver.positions[hook.iClosest] - transform.position;

		Vector3 targetVel = (Vector3)solver.velocities[hook.iClosest];
		targetVel = Vector3.Project(targetVel, hook.closestTangent);
		//targetVel += targetVel * Mathf.Abs(Vector3.Dot(targetVel.normalized, closestTangent));
		targetVel += Vector3.ProjectOnPlane((-diff + transform.position - prevPosition) / stepTime, hook.closestTangent);

		solver.velocities[hook.iClosest] = targetVel;
	}

	private void EndStep(ObiSolver solver)
	{
		prevPosition = transform.position;
	}
}