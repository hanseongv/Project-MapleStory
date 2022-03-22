using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
   [SerializeField] Transform target;
   [SerializeField] float cameraZ=10f;
   private void FixedUpdate() {
       Vector3 targetPos=new Vector3(target.position.x,target.position.y,cameraZ);
       transform.position=Vector3.Lerp(transform.position,targetPos,Time.deltaTime*2f);
   }
}
