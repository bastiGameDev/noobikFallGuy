using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public int gridSize = 5; // Размер сетки (например, 5x5)
    public GameObject tilePrefab; // Префаб плитки
    public Color[] colors; // Массив доступных цветов

    public GameObject[,] tiles; // Массив для хранения плиток

    void Start()
    {
        GenerateFloor();
    }

    public void GenerateFloor()
    {
        // Очищаем предыдущие плитки, если они есть
        ClearFloor();

        // Получаем размер одной плитки из её коллайдера или рендера
        Renderer tileRenderer = tilePrefab.GetComponent<Renderer>();
        if (tileRenderer == null)
        {
            Debug.LogError("У префаба плитки отсутствует компонент Renderer!");
            return;
        }

        // Получаем размер плитки в мировых координатах
        Bounds tileBounds = tileRenderer.bounds;
        float tileSizeX = tileBounds.size.x;
        float tileSizeZ = tileBounds.size.z;

        // Создаем массив для хранения плиток
        tiles = new GameObject[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                // Рассчитываем позицию плитки
                Vector3 position = new Vector3(
                    x * tileSizeX - (gridSize * tileSizeX / 2f) + tileSizeX / 2f,
                    0,
                    z * tileSizeZ - (gridSize * tileSizeZ / 2f) + tileSizeZ / 2f
                );

                // Создаем плитку на сцене
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                tile.transform.parent = transform;

                // Устанавливаем случайный цвет
                Renderer renderer = tile.GetComponent<Renderer>();
                renderer.material.color = colors[Random.Range(0, colors.Length)];

                // Сохраняем плитку в массиве
                tiles[x, z] = tile;
            }
        }
    }

    void ClearFloor()
    {
        // Удаляем все дочерние объекты (плитки) у текущего объекта
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}