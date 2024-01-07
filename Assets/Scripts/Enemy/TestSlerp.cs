using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class TestSlerp : MonoBehaviour
{
    [SerializeField] private float LerpSpeed,currentPoint,valueTarget;
    public AnimationCurve currentPointCurve;
    public GameObject target;
    public bool Lerp;
    public float offset = 5;
    private Vector3 basePosition,centre;
    [SerializeField]private CapsuleCollider capsuleCol;

    // Start is called before the first frame update
    void Awake()
    {
        capsuleCol =gameObject.transform.Find("capsule").GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        basePosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //currentPoint = Mathf.MoveTowards(currentPoint, target, LerpSpeed * Time.deltaTime);
        if (Lerp)
        {
            valueTarget = 1;
            if (currentPoint != 1)
            {
                
                
                var start = basePosition;
                centre = (start + target.transform.position) * 0.5f;
                var centreReal = centre - new Vector3(-offset, 0, 0);
                var RelativeSelfPos = start - centreReal;
                var RelativeTargetPos = target.transform.position- centreReal;
                //Debug.Log($"time= {Time.deltaTime} cal ={LerpSpeed*Time.deltaTime}");
                currentPoint = Mathf.MoveTowards(currentPoint, valueTarget,LerpSpeed*Time.deltaTime);
                //Debug.Log($" point ={currentPoint}");
                
               // DetectObstacles(centreReal,RelativeSelfPos,RelativeTargetPos,currentPoint, out var  varCanMove);
              
                    
                    transform.position= Vector3.Slerp(RelativeSelfPos, RelativeTargetPos,currentPointCurve.Evaluate(currentPoint)) + centreReal;
                    
                    
            }
        }
        else
        {
            currentPoint = 0;
            
            valueTarget = 0;
        }
    }

    private void DetectObstacles(Vector3 centre,Vector3 thisPos , Vector3 target, float currentePoint,out bool canMove)
    {
        RaycastHit hit ;
        var p1 = transform.position + capsuleCol.center + Vector3.up * -capsuleCol.height * 0.5f;
        var p2 = p1 + Vector3.up * capsuleCol.height;
        Physics.CapsuleCast(p1, p2, capsuleCol.radius, transform.forward, out hit, 10);
        Debug.Log(hit.collider);
        canMove = hit.collider ? canMove=false :canMove=true ;
       
    }

    
    
    private void OnDrawGizmosSelected()
    {
        RaycastHit hit;
            var p1  = transform.position + capsuleCol.center + Vector3.up * -capsuleCol.height * 0.5f;
            var p2 = p1 + Vector3.up * capsuleCol.height;
            var maxDistance = 0.1f;
            Gizmos.DrawLine(p1,p2);
            if (Physics.CapsuleCast(p1, p2, capsuleCol.radius, Vector3.forward, out hit, maxDistance))
            {
                Debug.Log(hit.collider);
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position,transform.forward*hit.distance);
                Gizmos.DrawWireSphere(transform.position + transform.forward * hit.distance,capsuleCol.radius);
            }
            else
            {
                Gizmos.color = Color.blue;
               // Gizmos.DrawRay(transform.position,transform.forward*hit.distance);
                Gizmos.DrawWireSphere(hit.point,0.1f);
                Gizmos.DrawLine(p1,p2);
               // Debug.Log($"p1 ={p1} & p2={p2}");
                Debug.Log("no");
            }
        
        
        
            
    }
    /*
    private IEnumerable<Vector3> DetectLoop(float OffSet,Vector3 thisPos , Vector3 target, bool canMove)
    {
        
        RaycastHit hit ;
        var p1 = transform.position + capsuleCol.center + Vector3.up * -capsuleCol.height * 0.5f;
        var p2 = p1 + Vector3.up * capsuleCol.height;
        Physics.CapsuleCast(p1, p2, capsuleCol.radius, transform.forward, out hit, 10);
        Debug.Log(hit.collider);
        canMove = hit.collider ? canMove=false :canMove=true ;
        Debug.Log(canMove);
        yield return canMove;
    }
    */

}
