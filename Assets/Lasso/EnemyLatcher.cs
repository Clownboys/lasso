using Obi;
using UnityEngine;

public class EnemyLatcher : MonoBehaviour
{
	[SerializeField] public Transform head;

	private ObiRope rope;

	private bool isLatched;

	private ObiParticleAttachment attachment;

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
		if (isLatched)
			return;

		ObiColliderWorld worldObi = ObiColliderWorld.GetInstance();

		// just iterate over all contacts in the current frame:
		foreach (Oni.Contact contact in e.contacts)
		{
			if (isLatched)
				return;

			// if this one is an actual collision:
			if (contact.distance < 0.02)
			{
				ObiColliderBase col = worldObi.colliderHandles[contact.bodyB].owner;
				if (col.gameObject.layer == 10)
				{
					EnemyInstance enemy = col.GetComponentInParent<EnemyInstance>();

					if(enemy != null)
					{
						isLatched = true;

						enemy.Lasso();

						attachment = rope.gameObject.AddComponent<ObiParticleAttachment>();
						
						attachment.target = enemy.transform;

						ObiParticleGroup latcherGroup = new ObiParticleGroup();
						latcherGroup.SetSourceBlueprint(rope.blueprint);
						latcherGroup.particleIndices.Add(solver.simplices[contact.bodyA]);

						attachment.particleGroup = latcherGroup;

						attachment.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;

						return;
					}
				}
			}
		}
	}

	public void Unlatch()
	{
		isLatched = false;

		Destroy(attachment);
	}

	//private void Update()
	//{
	//	if(!isLatched) return;

 //       if (Vector3.Distance(latchedClown.position, head.position) < 2)
 //       {
 //           Destroy(latchedClown.gameObject);

	//		latchedClown = null;
	//		isLatched = false;

	//		var attachments = GetComponents<ObiParticleAttachment>();
	//		for (int i = 0; i < attachments.Length; i++)
	//		{
	//			Destroy(attachments[i]);
	//		}
 //       }
 //   }
}