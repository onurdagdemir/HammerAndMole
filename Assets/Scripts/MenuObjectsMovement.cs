using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuObjectsMovement : MonoBehaviour
{

    public Transform object1;
    public Transform object2;

    private Vector3 targetPosition1;
    private Vector3 targetPosition2;
    private Vector3 startPosition1;
    private Vector3 startPosition2;

    private bool reachedTarget = false;
    private bool isRotated = false;

    // Start is called before the first frame update
    void Start()
    {
        startPosition1 = object1.position;
        startPosition2 = object2.position;
        targetPosition1 = object1.position + new Vector3(14f, 0f, 0f);
        targetPosition2 = object2.position + new Vector3(10f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!reachedTarget)
        {
            object1.position = Vector3.MoveTowards(object1.position, targetPosition1, Time.deltaTime * 3f);
            object2.position = Vector3.MoveTowards(object2.position, targetPosition2, Time.deltaTime * 3f);
            isRotated = false;

            if (object1.position == targetPosition1 && object2.position == targetPosition2)
            {
                reachedTarget = true;
            }

        }
        else
        {
            // Objeleri Y ekseninde 180 derece döndürüyoruz.
            if(isRotated == false)
            { 
            object1.Rotate(0f, 180f, 0f);
            object2.Rotate(0f, 180f, 0f);
                isRotated = true;
            }

            // Objeleri başlangıç noktasına doğru hareket ettiriyoruz.
            object1.position = Vector3.MoveTowards(object1.position, startPosition1, Time.deltaTime * 3f);
            object2.position = Vector3.MoveTowards(object2.position, startPosition2, Time.deltaTime * 3f);

            // Başlangıç noktasına ulaşıldığında, hareketi sıfırlıyoruz ve hedefe doğru harekete devam etmelerini sağlamak için değişkeni false yapıyoruz.
            if (object1.position == startPosition1 && object2.position == startPosition2)
            {
                object1.Rotate(0f, 180f, 0f);
                object2.Rotate(0f, 180f, 0f);
                reachedTarget = false;
            }
        }

    }
}
