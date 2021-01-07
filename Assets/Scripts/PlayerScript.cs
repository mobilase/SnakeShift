using UnityEngine;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {

    public Vector2 speed = new Vector2(50, 50); // Скорость
    public int length = 5;                      // Длина змеи
    public GameObject headPrefab;               // Префаб головы змеи
    public GameObject bodyPrefab;               // Префаб туловища змеи
    public GameObject tailPrefab;               // Префаб хвоста змеи

    private Vector2 movement;                   // Направление движения
    private float angle;                        // Угол
    private GameObject head;                    // Объект головы змеи

    void Start()
    {
        float y0 = 0f;
        float x0 = 0f;
        GameObject instance;

        // Добавление головы
        head = Instantiate(headPrefab, new Vector3(x0, y0, 0f), Quaternion.identity) as GameObject;
        head.AddComponent<Rigidbody2D>();
        head.transform.parent = GetComponent<Transform>();

        // Добавление тела
        Transform current = head.transform;
        for (int y = 0; y < length; y++)
        {
            //if (y == 0)
            //{
            //    y0 = current.GetComponent<Rigidbody2D>().transform.position.y;
            //}
            //else
            //{
                y0 = current.GetComponent<Rigidbody2D>().transform.position.y + current.GetComponent<BoxCollider2D>().size.y / 2;
            //}
            x0 = current.GetComponent<Rigidbody2D>().transform.position.x;
            instance = Instantiate(bodyPrefab, new Vector3(x0, y0, 0f), Quaternion.identity) as GameObject;
            instance.AddComponent<Rigidbody2D>();
            instance.transform.parent = GetComponent<Transform>();
            instance.GetComponent<HingeJoint2D>().connectedBody = current.GetComponent<Rigidbody2D>();
            current = instance.transform;
        }

        // Добавление хвоста
        y0 = current.GetComponent<Rigidbody2D>().transform.position.y + current.GetComponent<BoxCollider2D>().size.y/2;
        x0 = current.GetComponent<Rigidbody2D>().transform.position.x;
        instance = Instantiate(tailPrefab, new Vector3(x0, y0, 0f), Quaternion.identity) as GameObject;
        instance.AddComponent<Rigidbody2D>();
        instance.transform.parent = GetComponent<Transform>();
        instance.transform.Rotate(0f, 0f, 90f);
        instance.GetComponent<HingeJoint2D>().connectedBody = current.GetComponent<Rigidbody2D>();

    }

    void Update ()
    {
        // Извлекаем координаты по изменению по осям из контроллера (клавиатуры) 
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // Вычисление вектора и угла движения
        movement = new Vector2(speed.x * inputX, speed.y * inputY);
        angle = 180 - Angle(new Vector2(0,1), new Vector2(inputX, inputY)) * Mathf.Sign(inputX);
    }

    void FixedUpdate()
    {
        // Перемещение по вектору движения
        head.GetComponent<Rigidbody2D>().velocity = movement;
        
        // Если перемещения не было - угол не меняется 
        if (movement.x == 0 && movement.y == 0)
        {
            head.GetComponent<Rigidbody2D>().rotation = head.GetComponent<Rigidbody2D>().rotation;
        }
        else
        {
            head.GetComponent<Rigidbody2D>().rotation = angle;
        }
    }

    // Рассчет угла между векторами
    public static float Angle(Vector2 from, Vector2 to)
    {
        return (Mathf.Acos(Mathf.Clamp(Vector2.Dot(from.normalized, to.normalized), -1f, 1f)) * Mathf.Rad2Deg);
    }
}