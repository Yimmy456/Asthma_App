using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSpawnerScript : MonoBehaviour
{
    [SerializeField]
    GameObject _prefab;

    [SerializeField]
    float _numberOfSeconds;

    [SerializeField]
    bool _spawnBool = true;

    [SerializeField]
    float _destroyAfter = 10.0f;

    [SerializeField]
    Vector3 _minPos;

    [SerializeField]
    Vector3 _maxPos;

    float _count = 0.0f;

    [SerializeField]
    Vector3 _initialRotation;

    [SerializeField]
    GameObject _parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnFunction();
    }

    void SpawnFunction()
    {
        if(_prefab == null || !_spawnBool)
        {
            return;
        }

        _count -= Time.deltaTime;

        if(_count <= 0.0f)
        {
            float RandX = Random.Range(_minPos.x, _maxPos.x);

            float RandY = Random.Range(_minPos.y, _maxPos.y);

            float RandZ = Random.Range(_minPos.z, _maxPos.z);

            Vector3 _pos = new Vector3(RandX, RandY, RandZ);

            Quaternion _q = Quaternion.Euler(_initialRotation);

            GameObject _obj = Instantiate(_prefab, _pos, _q);

            Destroy(_obj, _destroyAfter);

            if(_parent != null)
            {
                _obj.transform.parent = _parent.transform;
            }

            _count = _numberOfSeconds;

            //OnApplicationQuit () => { delegate { Destroy(_obj); }; };
        }
    }

    private void OnApplicationQuit()
    {
        Destroy(gameObject);
    }
}
