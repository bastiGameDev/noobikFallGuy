using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public FloorGenerator floorGenerator; // Ссылка на генератор пола
    public Image colorIndicator; // UI Image для отображения цвета
    public float timerDuration = 3f; // Время на выбор цвета

    private Color currentColor; // Текущий цвет
    private float timer; // Оставшееся время

    void Start()
    {
        StartNewRound();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                RemoveWrongTiles();
                Invoke("RegenerateFloor", 2f); // Через 2 секунды восстанавливаем пол
            }
        }
    }

    void StartNewRound()
    {
        // Выбираем случайный цвет
        currentColor = floorGenerator.colors[Random.Range(0, floorGenerator.colors.Length)];

        // Устанавливаем цвет для UI Image с фиксированным альфа-каналом
        if (colorIndicator != null)
        {
            Color indicatorColor = currentColor;
            indicatorColor.a = 1f; // Устанавливаем альфа-канал в 1 (непрозрачный)
            colorIndicator.color = indicatorColor;
        }

        // Запускаем таймер
        timer = timerDuration;
    }

    void RemoveWrongTiles()
    {
        for (int x = 0; x < floorGenerator.gridSize; x++)
        {
            for (int z = 0; z < floorGenerator.gridSize; z++)
            {
                GameObject tile = floorGenerator.tiles[x, z];
                if (tile == null) continue; // Пропускаем удаленные плитки

                Renderer renderer = tile.GetComponent<Renderer>();

                // Если цвет плитки не совпадает с текущим цветом, удаляем её
                if (!AreColorsEqual(renderer.material.color, currentColor))
                {
                    Destroy(tile);
                    floorGenerator.tiles[x, z] = null; // Обнуляем ссылку в массиве
                }
            }
        }
    }

    void RegenerateFloor()
    {
        floorGenerator.GenerateFloor(); // Генерируем новый пол
        StartNewRound(); // Начинаем новый раунд
    }

    bool AreColorsEqual(Color color1, Color color2, float threshold = 0.01f)
    {
        // Сравниваем цвета с допуском
        return Mathf.Abs(color1.r - color2.r) < threshold &&
               Mathf.Abs(color1.g - color2.g) < threshold &&
               Mathf.Abs(color1.b - color2.b) < threshold;
    }
}