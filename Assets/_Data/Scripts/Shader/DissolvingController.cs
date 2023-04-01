using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolvingController : MonoBehaviour
{
    [SerializeField] GameObject root;
    [SerializeField] SkinnedMeshRenderer skinnedMesh;
    [SerializeField] float refreshRate = 0.025f;
    [SerializeField] float DissolveRate = 0.0125f;
    [SerializeField] ParticleSystem particleSystems;

    private Material[] skinnedMaterials;




    private void Start()
    {
        if (skinnedMesh != null)
        {
            skinnedMaterials = skinnedMesh.materials;
        }

        skinnedMesh.gameObject.SetActive(false);
        root.SetActive(true);
    }

    public IEnumerator DissolveCo()
    {

        skinnedMesh.gameObject.SetActive(true);
        root.SetActive(false);
        if (particleSystems != null)
        {
            particleSystems.Play();
        }

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
