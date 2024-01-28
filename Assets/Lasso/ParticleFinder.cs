using Obi;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ParticleFinder : MonoBehaviour
{
	public int iClosest = 1;
	public int iElementClosest = 1;
	public Vector3 closestTangent;

	public ObiRope rope;

	public int limitSign;
	public ParticleFinder other;

	private void Update()
	{
		float closestDist = float.MaxValue;
		iClosest = 0;
		iElementClosest = 0;
		closestTangent = Vector3.zero;

		ObiNativeVector4List points = rope.solver.positions;

		int start = 0;
		int end = rope.elements.Count - 1;

		if (limitSign == 1)
		{
			start = other.iElementClosest;
		}
		else if (limitSign == -1)
		{
			end = other.iElementClosest;
		}

		// Iterate over all particles in an actor:
		for (int i = start; i < end; i++)
		{
			// retrieve the particle index in the solver:
			int iSolvNext = rope.elements[i + 1].particle2;
			int iSolvCurr = rope.elements[i].particle2;
			int iSolvPrev = rope.elements[i].particle1;

			Vector3 particle = points[iSolvCurr];
			float dist = Vector3.Distance(particle, transform.position);

			if (dist < closestDist)
			{
				closestDist = dist;
				iClosest = iSolvCurr;
				iElementClosest = i;

				closestTangent = Vector3.Normalize(points[iSolvCurr] - points[iSolvPrev] + points[iSolvNext] - points[iSolvCurr]);
			}

		}
	}
}
