using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace StageSystem.Boss
{
public class ShotObject : MonoBehaviour
{
    bool _isMove = true; 
    [SerializeField] Vector3 moveDirection = Vector3.zero;
    
    void Start()
    {
        StartCoroutine(Timer());
    }
    
    public void MoveStop()
    {
        _isMove = false;
    }
    public bool IsMove()
    {
        return _isMove;
    }

    void FixedUpdate()
    {   
        if (_isMove)
        {
            transform.position += moveDirection * Time.deltaTime;
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
    }
}
}
