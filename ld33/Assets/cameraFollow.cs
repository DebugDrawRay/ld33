using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour
{
    private player _player;
    public Vector3 anchorOffset;
    public float followLag;
    void Start()
    {
        _player = player.Instance;
    }
    void Update()
    {
        Vector3 targetPos = _player.transform.position + anchorOffset;
        Vector3 anchorPos = Vector3.Lerp(transform.position, targetPos, followLag);
        transform.position = anchorPos;
        transform.rotation = _player.transform.rotation;
    } 
}
