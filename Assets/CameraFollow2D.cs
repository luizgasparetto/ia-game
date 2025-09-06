using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform alvo;
    public Vector3 offset = new Vector3(0, 0, -10f);
    public float suaviza = 10f;

    void LateUpdate()
    {
        if (!alvo) return;
        Vector3 alvoPos = alvo.position + offset;
        transform.position = Vector3.Lerp(transform.position, alvoPos, suaviza * Time.deltaTime);
        transform.rotation = Quaternion.identity; // nunca gira
    }
}
