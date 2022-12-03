using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaShot : MonoBehaviour
{

    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    private Transform Target;
    private Rigidbody2D rig;

    void Start()
    {
        Target = enemyManager.Instance.playerPos;
        rig = GetComponent<Rigidbody2D>();
        StartCoroutine(SimulateProjectile());
    }

    IEnumerator SimulateProjectile()
    {

        //타겟과 거리를 계산
        float target_Distance = Vector3.Distance(transform.position, Target.position);

        // 각도와 중력을 대입하여 포물선 속도르 구함
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        //x,y 값을 구함
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        transform.rotation = Quaternion.LookRotation(Target.position - transform.position);
        transform.Rotate(new Vector3(0, -90, 0));
        float elapse_time = 0;

        while (true)
        {
            transform.Translate(Vx * Time.deltaTime, (Vy - (gravity * elapse_time)) * Time.deltaTime, 0.0f);

            elapse_time += Time.deltaTime;

            yield return null;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
