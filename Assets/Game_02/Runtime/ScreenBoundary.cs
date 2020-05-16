using System;
using UnityEditor;
using UnityEngine;

public class ScreenBoundary : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private bool _debug;

    [Space]
    [SerializeField] private Vector2 _minViewport;
    [SerializeField] private Vector2 _maxViewport;

    private float _depth;
    private Vector3 _screenDiag1;
    private Vector3 _screenDiag2;
    
    private Vector3 _worldPtCenter;

    private void OnDrawGizmos()
    {
        if (!_debug) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_screenDiag1, 0.5f);
        Gizmos.DrawSphere(_screenDiag2, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_worldPtCenter, 0.5f);
    }

    private void OnValidate() => Bind();

    // Update is called once per frame
    private void Update() => Bind();

    [ContextMenu("Bind")]
    public void Bind()
    {
        // Assumes camera is top down on 2D plane - (pos = (0, 0, -d), looking down
        // Assumes target is on z = 0
        // Assumes camera is orthographic

        _depth = Mathf.Abs(_camera.transform.position.z);
        _screenDiag1 = _camera.ViewportToWorldPoint(new Vector3(_minViewport.x, _minViewport.y, _depth));
        _screenDiag2 = _camera.ViewportToWorldPoint(new Vector3(_maxViewport.x, _maxViewport.y, _depth));
        
        var diag1 = (Vector2) _screenDiag1;
        var diag2 = (Vector2) _screenDiag2;
        //
        _worldPtCenter = 0.5f * (diag1 + diag2);
        var size = diag2 - diag1;
        size.x = Mathf.Abs(size.x);
        size.y = Mathf.Abs(size.y);

        _collider.offset = _worldPtCenter;
        _collider.size = size;
    }
}

