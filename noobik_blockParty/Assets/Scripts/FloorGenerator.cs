using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public int gridSize = 5; // ������ ����� (��������, 5x5)
    public GameObject tilePrefab; // ������ ������
    public Color[] colors; // ������ ��������� ������

    public GameObject[,] tiles; // ������ ��� �������� ������

    void Start()
    {
        GenerateFloor();
    }

    public void GenerateFloor()
    {
        // ������� ���������� ������, ���� ��� ����
        ClearFloor();

        // �������� ������ ����� ������ �� � ���������� ��� �������
        Renderer tileRenderer = tilePrefab.GetComponent<Renderer>();
        if (tileRenderer == null)
        {
            Debug.LogError("� ������� ������ ����������� ��������� Renderer!");
            return;
        }

        // �������� ������ ������ � ������� �����������
        Bounds tileBounds = tileRenderer.bounds;
        float tileSizeX = tileBounds.size.x;
        float tileSizeZ = tileBounds.size.z;

        // ������� ������ ��� �������� ������
        tiles = new GameObject[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                // ������������ ������� ������
                Vector3 position = new Vector3(
                    x * tileSizeX - (gridSize * tileSizeX / 2f) + tileSizeX / 2f,
                    0,
                    z * tileSizeZ - (gridSize * tileSizeZ / 2f) + tileSizeZ / 2f
                );

                // ������� ������ �� �����
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                tile.transform.parent = transform;

                // ������������� ��������� ����
                Renderer renderer = tile.GetComponent<Renderer>();
                renderer.material.color = colors[Random.Range(0, colors.Length)];

                // ��������� ������ � �������
                tiles[x, z] = tile;
            }
        }
    }

    void ClearFloor()
    {
        // ������� ��� �������� ������� (������) � �������� �������
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}