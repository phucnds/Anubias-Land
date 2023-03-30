using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolvingController : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer skinnedMesh;
    [SerializeField] float refreshRate = 0.025f;
    [SerializeField] float DissolveRate = 0.0125f;

    private Material[] skinnedMaterials;
   

    private void Start()
    {
        if (skinnedMesh != null)
        {
            skinnedMaterials = skinnedMesh.materials;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(DissolveCo());
        }
    }


    IEnumerator DissolveCo()
    {
        if (skinnedMaterials.Length > 0)
        {
            float counter = 0;
            while (skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += DissolveRate;
                for (int i = 0; i < skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
