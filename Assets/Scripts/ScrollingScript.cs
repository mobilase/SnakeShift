using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// Скрипт параллакс-скроллинга, который должен быть прописан для слоя
public class ScrollingScript : MonoBehaviour
{
    public Vector2 speed = new Vector2(10, 10);     // Скорость прокрутки
    public Vector2 direction = new Vector2(-1, 0);  // Направление движения
    public bool isLinkedToCamera = false;           // Движения должны быть применены к камере
    public bool isLooping = false;                  // Бесконечный фон
    private List<Transform> backgroundPart;         // Список детей с рендерером

    // Получаем всех детишек))
    void Start()
    {
        // Только для безконечного фона
        if (isLooping)
        {
            // Задействовать всех детей слоя с рендерером
            backgroundPart = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                // Добавить только видимых детей
                if (child.GetComponent<Renderer>() != null)
                {
                    backgroundPart.Add(child);
                }
            }

            // Сортировка по позиции.
            // Примечание: получаем детей слева направо.
            // Мы должны добавить несколько условий для обработки
            // разных направлений прокрутки.
            backgroundPart = backgroundPart.OrderBy(t => t.position.x).ToList();
        }
    }

    void Update()
    {
        // Перемещение
        Vector3 movement = new Vector3(speed.x * direction.x, speed.y * direction.y, 0);

        movement *= Time.deltaTime;
        transform.Translate(movement);

        // Перемещение камеры
        if (isLinkedToCamera)
        {
            Camera.main.transform.Translate(movement);
        }

        // 4 - Loop
        if (isLooping)
        {
            // Получение первого объекта.
            // Список упорядочен слева (позиция по оси X) направо.
            Transform firstChild = backgroundPart.FirstOrDefault();

            if (firstChild != null)
            {
                // Проверить, находится ли ребенок (частично) перед камерой.
                // Первым делом мы тестируем позицию, т.к. метод IsVisibleFrom
                // немного сложнее воплотить в жизнь
                if (firstChild.position.x < Camera.main.transform.position.x)
                {
                    // Если ребенок уже слева от камеры,
                    // мы проверяем, покинул ли он область кадра, чтобы использовать его
                    // повторно.
                    if (firstChild.GetComponent<Renderer>().isVisible == false)
                    {
                        // Получить последнюю позицию ребенка.
                        Transform lastChild = backgroundPart.LastOrDefault();
                        Vector3 lastPosition = lastChild.transform.position;
                        Vector3 lastSize = (lastChild.GetComponent<Renderer>().bounds.max - lastChild.GetComponent<Renderer>().bounds.min);

                        // Переместить повторно используемый объект так, чтобы он располагался ПОСЛЕ
                        // последнего ребенка
                        // Примечание: Пока работает только для горизонтального скроллинга.
                        firstChild.position = new Vector3(lastPosition.x + lastSize.x, firstChild.position.y, firstChild.position.z);

                        // Поставить повторно используемый объект
                        // в конец списка backgroundPart.
                        backgroundPart.Remove(firstChild);
                        backgroundPart.Add(firstChild);
                    }
                }
            }
        }
    }
}
