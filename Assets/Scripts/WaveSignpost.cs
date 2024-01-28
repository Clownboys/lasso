using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveSignpost : MonoBehaviour
{
    public TextMeshProUGUI text;
    Animator anim;
    BillboardToCenter billboard;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void NextWave(int wave)
    {
        text.text = "Wave " + (wave+1);
        anim.SetBool("isWaving", true);
    }

    public void Lower()
    {
        anim.SetBool("isWaving", false);
    }

    private void Update()
    {
        if (!anim.GetBool("isWaving"))
        {
            Vector3 position = GameWrangler.Instance.transform.position + Camera.main.transform.forward * 10;
            position.y = 0;
            transform.position = position;
        }
    }
}
