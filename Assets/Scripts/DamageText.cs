using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    TextMesh textMesh;
    MeshRenderer mesh;
    Color alpha;
    public int damage;
    public string sortingLayerName;
    public int sortingOrder;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 2.0f;
        alphaSpeed = 2.0f;
        destroyTime = 2.0f;

        mesh = GetComponent<MeshRenderer>();
        textMesh = GetComponent<TextMesh>();

        mesh.sortingLayerName = sortingLayerName;
        mesh.sortingOrder = sortingOrder;

        alpha = textMesh.color;
        if(damage >= 0)
            textMesh.text = damage.ToString();
        Invoke("DestroyObject", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // 텍스트 위치
        
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // 텍스트 알파값
        textMesh.color = alpha;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
