using Obi;
using UnityEngine;

public class LassoDetectTrigger : MonoBehaviour
{
	private ObiColliderBase colliderObi;
	private static ObiColliderWorld worldObi;

	private static ObiSolver solverObi;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void NullifySolver()
	{
		solverObi = null;
	}

	void Awake()
	{
		colliderObi = GetComponent<ObiCollider>();
		if (solverObi == null)
		{
			solverObi = FindObjectOfType<ObiSolver>();
		}
		worldObi = ObiColliderWorld.GetInstance();
	}

	void OnEnable()
	{
		solverObi.OnCollision += Solver_OnCollision;
	}

	void OnDisable()
	{
		solverObi.OnCollision -= Solver_OnCollision;
	}

	void Solver_OnCollision(object sender, Obi.ObiSolver.ObiCollisionEventArgs e)
	{
		// just iterate over all contacts in the current frame:
		foreach (Oni.Contact contact in e.contacts)
		{
			// if this one is an actual collision:
			if (contact.distance < 0.01)
			{
				ObiColliderBase col = worldObi.colliderHandles[contact.bodyB].owner;
				if (col == colliderObi)
				{

				}
			}
		}
	}
}