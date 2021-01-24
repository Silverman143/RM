using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{

    private float touchStartPos, touchPos, targetPosZ;
    private GameObject target;

    void Start()
    {
        target = this.gameObject;
    }

    private void Update()
    {



        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position.x;

            }
            if (touch.phase == TouchPhase.Moved | touch.phase == TouchPhase.Stationary)
            {
                touchPos = touch.position.x;

                if (touchStartPos - touchPos >= 5)
                {
                    targetPosZ += 0.75f;
                    if (targetPosZ > 10) targetPosZ = 10;
                    touchStartPos = touch.position.x;

                }
                if (touchStartPos - touchPos <= -5)
                {
                    targetPosZ -= 0.75f;
                    if (targetPosZ < -10) targetPosZ = -10;
                    touchStartPos = touch.position.x;

                }

                Vector3 targetNewPos = target.transform.localPosition;
                targetNewPos.z = targetPosZ;


                target.transform.localPosition = Vector3.MoveTowards(target.transform.localPosition, targetNewPos, 50f * Time.deltaTime);






                

            }
            
        }
    }

}
