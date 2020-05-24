using System.Collections;
using UnityEngine;

public class LookableItem : MonoBehaviour
{
    [SerializeField] private float distFromCamera;
    [SerializeField] private float animTime;
    private static LookableItem _currLookedItem;
    private bool _isInForeground;
    private Vector3 _startPos;
    private Quaternion _startRot;
    private static Camera _mainCam;

    private static Camera MainCam
    {
        get
        {
            if (_mainCam == null) _mainCam = Camera.main;
            return _mainCam;
        }
    }

    private Vector3 TargetPos => MainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distFromCamera));
    private Quaternion TargetRot => MainCam.transform.rotation * _startRot;

    private static LookableItem CurrLookedItem
    {
        set
        {
            if (_currLookedItem != null)
            {
                _currLookedItem.OnAnotherItemLookedAt();
            }
        }
    }

    private void Awake()
    {
        _startPos = transform.position;
        _startRot = transform.rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(TargetPos, 0.3f);
    }

    private void OnMouseUpAsButton()
    {
        ToggleBeingLookedAt();
    }

    private void ToggleBeingLookedAt()
    {
        if (_isInForeground)
        {
            StartCoroutine(LerpTo(_startPos, _startRot));
        }
        else
        {
            StartCoroutine(LerpTo(TargetPos, TargetRot));
        }

        _isInForeground = !_isInForeground;
    }

    private IEnumerator LerpTo(Vector3 pos, Quaternion rot)
    {
        var t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / animTime; 
            var deltaPos = pos - transform.position;
            var deltaRot = transform.rotation * Quaternion.Inverse(rot);

            var needMoreRot = deltaRot.eulerAngles.magnitude > 1f;
            var needMorePos = deltaPos.magnitude > 0.01f;
            if (!needMorePos && !needMoreRot) yield break;
            
            if (needMorePos)
            {
                transform.position = Vector3.Lerp(transform.position, pos, t);
            }

            if (needMoreRot)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, t);
            }

            yield return null;
        }
    }

    public void OnAnotherItemLookedAt()
    {
        if (!_isInForeground) return;
        ToggleBeingLookedAt();
    }
}
