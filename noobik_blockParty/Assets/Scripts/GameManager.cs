using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public FloorGenerator floorGenerator; // ������ �� ��������� ����
    public Image colorIndicator; // UI Image ��� ����������� �����
    public float timerDuration = 3f; // ����� �� ����� �����

    private Color currentColor; // ������� ����
    private float timer; // ���������� �����

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
                Invoke("RegenerateFloor", 2f); // ����� 2 ������� ��������������� ���
            }
        }
    }

    void StartNewRound()
    {
        // �������� ��������� ����
        currentColor = floorGenerator.colors[Random.Range(0, floorGenerator.colors.Length)];

        // ������������� ���� ��� UI Image � ������������� �����-�������
        if (colorIndicator != null)
        {
            Color indicatorColor = currentColor;
            indicatorColor.a = 1f; // ������������� �����-����� � 1 (������������)
            colorIndicator.color = indicatorColor;
        }

        // ��������� ������
        timer = timerDuration;
    }

    void RemoveWrongTiles()
    {
        for (int x = 0; x < floorGenerator.gridSize; x++)
        {
            for (int z = 0; z < floorGenerator.gridSize; z++)
            {
                GameObject tile = floorGenerator.tiles[x, z];
                if (tile == null) continue; // ���������� ��������� ������

                Renderer renderer = tile.GetComponent<Renderer>();

                // ���� ���� ������ �� ��������� � ������� ������, ������� �
                if (!AreColorsEqual(renderer.material.color, currentColor))
                {
                    Destroy(tile);
                    floorGenerator.tiles[x, z] = null; // �������� ������ � �������
                }
            }
        }
    }

    void RegenerateFloor()
    {
        floorGenerator.GenerateFloor(); // ���������� ����� ���
        StartNewRound(); // �������� ����� �����
    }

    bool AreColorsEqual(Color color1, Color color2, float threshold = 0.01f)
    {
        // ���������� ����� � ��������
        return Mathf.Abs(color1.r - color2.r) < threshold &&
               Mathf.Abs(color1.g - color2.g) < threshold &&
               Mathf.Abs(color1.b - color2.b) < threshold;
    }
}