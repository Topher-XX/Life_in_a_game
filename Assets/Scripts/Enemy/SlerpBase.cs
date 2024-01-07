using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


[ExecuteInEditMode]
public class SlerpBase : MonoBehaviour
{
    [SerializeField] private float LerpSpeed, currentPoint, valueTarget;
    public AnimationCurve currentPointCurve;
    public GameObject target, go1;
    public GameObject testcuber;
    public GameObject debugcuber;
    public bool Lerp, startLerping, canMove, blockMove = false;
    public float offset = 1;
    private Vector3 basePosition, centre;
    [SerializeField] private CapsuleCollider capsuleCol;
    [SerializeField] private float Calc;

    // Start is called before the first frame update
    private void Awake()
    {
        capsuleCol = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    private void Update()
    {
        //currentPoint = Mathf.MoveTowards(currentPoint, target, LerpSpeed * Time.deltaTime);
        if (Lerp)
        {
            canMove = false;
            blockMove = canMove;
            if (currentPoint == 0 & !startLerping)
            {
                basePosition = transform.position;
                valueTarget = 1;
                startLerping = true;
            }
            else // currentpoint = 0
            {
                currentPoint = Mathf.MoveTowards(currentPoint, valueTarget, LerpSpeed * Time.deltaTime);
                foreach (var point in DetectLoop(offset, basePosition, target.transform.position))
                {
                    Instantiate(testcuber, point, new Quaternion(0f, 0f, 0f, 0f));
                }

                Lerp = false;
                // StartCoroutine(DetectLoop(offset, basePosition, target.transform.position, canMove));

                if (canMove)
                {

                    Lerp = false;
                    /*  Debug.Log("Can Slerp");
                      var neededPosition = Vector3.Slerp(RelativeSelfPos, RelativeTargetPos,
                          currentPointCurve.Evaluate(currentPoint)) + centreReal;
                      neededPosition.y = transform.position.y;
                      transform.position = neededPosition;   */
                }
                else // canMove
                {
                    Debug.Log("Don't Slerp");
                }
            }
        }
        else // lerp = true
        {
            currentPoint = 0;
            startLerping = false;
            valueTarget = 0;
        }
    }

    /*
    private void DetectObstacles(Vector3 centre,Vector3 thisPos , Vector3 target, float currentePoint,out bool canMove)
    {
        RaycastHit hit ;
        var p1 = transform.position + capsuleCol.center + Vector3.up * -capsuleCol.height * 0.5f;
        var p2 = p1 + Vector3.up * capsuleCol.height;
        Physics.CapsuleCast(p1, p2, capsuleCol.radius, transform.forward, out hit, 10);
        Debug.Log(hit.collider);
        canMove = hit.collider ? canMove=false :canMove=true ;

    }
    */
    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerable<Vector3> DetectLoop(float OffSet, Vector3 thisPos, Vector3 target)
    {
        var centre = (thisPos + target) * 0.5F;
        centre -= new Vector3(-OffSet, 0, 0);
        var startRelativeCenter = thisPos - centre;
        var endRelativeCenter = target - centre;
        //phase 1 calcule des points
        // phase 2 check au collider
        var circleRange = (Mathf.Round(startRelativeCenter.magnitude + endRelativeCenter.magnitude)) * Mathf.PI / 2;
        //Debug.Log($"{thisPos.magnitude} + {target.magnitude} - {centre.magnitude}   cal = {Mathf.Round(circleRange)}");

        var f = 1f / Mathf.Round(circleRange)*Calc; //- (Mathf.Round(circleRange)*.1f) ; // -1 car flemme de faire un magnitude vector check du joueur
        var f2 = f;
        if (Mathf.Round(circleRange)  < 20)
        { 
            Debug.Log($"{Mathf.Round(circleRange)}");
            f2 += f+f ;  
            Debug.Log($"f2 = {f2}");
           
        }
        Debug.Log($"{Mathf.Round(circleRange)}");
        for (var i = 0f + f; i <= 1 - f2; i += f2)
        {
           // var scaleCol = new Vector3(capsuleCol.tr, 1,0);
            
            var basePos = Vector3.Slerp(startRelativeCenter, endRelativeCenter, i) + centre;
            var endPos = Vector3.Slerp(startRelativeCenter, endRelativeCenter, i) + centre;
            //var p3 = capsuleCol.center
            //RaycastHit hit;

            var p1 = basePos + capsuleCol.center + Vector3.up * 0.5f * -capsuleCol.height * 0.5f;
            var p2 = p1 + Vector3.up * 0.5f * capsuleCol.height;
            var isHit = Physics.CapsuleCast(p1 - Vector3.forward, p2 - Vector3.forward, capsuleCol.radius,
                transform.forward, 1);
            Debug.Log($" {isHit}");
            
            
            var hit = new Vector3(0, 0, 0); // var hit = isHit[isHit.Length - L];
            if (isHit)
            {
                
                Debug.Log($"debug all = true = {isHit}"); 
                yield break;
            }
            else
            {
                Debug.Log($"debug all = false = {isHit}");
            }

            
            yield return Vector3.Slerp(startRelativeCenter, endRelativeCenter, i) + centre;
        }
    }
    

#if UNITY_EDITOR
    
    private void OnDrawGizmosSelected()
    {
        
            var p1 = transform.position + capsuleCol.center + Vector3.up * -capsuleCol.height * 0.5f;
            var p2 = p1 + Vector3.up * capsuleCol.height;
            Gizmos.color = Color.blue;
            //Gizmos.DrawLine(p1,p2);

            foreach (var point in DetectLoop(offset, transform.position, target.transform.position))
            {
              
                Gizmos.DrawWireSphere(point, capsuleCol.radius);

            }



        
    }
#endif

    
}
