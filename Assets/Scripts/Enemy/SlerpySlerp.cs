using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[ExecuteInEditMode]
public class SlerpySlerp : MonoBehaviour {
    [SerializeField] private Transform _start, _end;
    [SerializeField] private int _count = 15;
    [SerializeField] private float offset;
    private void OnDrawGizmos()
    {
        var centre = (_start.position + _end.position) * 0.5F;
        centre -= new Vector3(-offset, 0, 0);
        foreach (var point in EvaluateSlerpPoints(_start.position, _end.position,_count)) {
            Gizmos.DrawSphere(point, 0.1f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(centre, 0.2f);
    }

    IEnumerable<Vector3> EvaluateSlerpPoints(Vector3 start, Vector3 end,int count = 10) {
        var centre = (_start.position + _end.position) * 0.5F;
        centre -= new Vector3(-offset, 0, 0); // new Vector3(0, 0, -offset); 
        var startRelativeCenter = start - centre;
        var endRelativeCenter = end - centre;

        var f = 1f / count;

        for (var i = 0f; i < 1 + f; i += f) {
            yield return Vector3.Slerp(startRelativeCenter, endRelativeCenter, i) + centre;
        }
    }
}