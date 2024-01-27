using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayingRandomPoint : MonoBehaviour
{
	private AudioSource source;

	void Start()
	{
		source = GetComponent<AudioSource>();

		source.time = Random.Range(0f, source.clip.length);
		source.Play();
	}
}
