using Obi;
using UnityEngine;

public class EnemyLatcher : MonoBehaviour
{
	private ObiRope rope;

	void Awake()
	{
		rope = GetComponent<ObiRope>();
	}

	void OnEnable()
	{
		rope.solver.OnCollision += Solver_OnCollision;
	}

	void OnDisable()
	{
		rope.solver.OnCollision -= Solver_OnCollision;
	}

	void Solver_OnCollision(ObiSolver solver, ObiSolver.ObiCollisionEventArgs e)
	{
		ObiColliderWorld worldObi = ObiColliderWorld.GetInstance();

		// just iterate over all contacts in the current frame:
		foreach (Oni.Contact contact in e.contacts)
		{
			// if this one is an actual collision:
			if (contact.distance < 0.02)
			{
				ObiColliderBase col = worldObi.colliderHandles[contact.bodyB].owner;
				if (col.gameObject.layer == 11)
				{
					EnemyInstance enemy = col.GetComponentInParent<EnemyInstance>();

					if(enemy != null)
					{

					}
				}
			}
		}
	}
}