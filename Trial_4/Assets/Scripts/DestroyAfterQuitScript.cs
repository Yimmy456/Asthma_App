using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyAfterQuitScript : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _otherObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        foreach (var obj in _otherObjects)
        {
            if(obj.GetComponent<ParticleSystem>() != null)
            {
                //ParticleSystem.Particle[] _p = obj.GetComponent<ParticleSystem>().GetParticles();
                obj.GetComponent<ParticleSystem>().Clear(true);

            }

            Destroy(obj);
        }
    }

    private void OnApplicationQuit()
    {
        foreach (var obj in _otherObjects)
        {
            if (obj.GetComponent<ParticleSystem>() != null)
            {
                obj.GetComponent<ParticleSystem>().Clear(true);
            }

            Destroy(obj);
        }

        Destroy(gameObject);
    }
}
