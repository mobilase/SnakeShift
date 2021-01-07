using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
	public int rows = Screen.width;//8;
	public int colums = Screen.width;//8;

    public Count foodCount = new Count(1, 5);
    public GameObject[] foodTiles;
    public GameObject[] floorTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List <Vector3> gridPositions = new List<Vector3>();

    void IniyializeList()
    {
		//Camera.main.pixelHeight = Screen.height;
		//Camera.main.pixelWidth = Screen.width;

		gridPositions.Clear();

        for (int x = 1; x < colums - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
				float x1 = -colums / 2 + 0.5f + x;
				float y1 = -rows / 2 + 0.5f + y;
                gridPositions.Add(new Vector3(x1, y1, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("sand").transform;

        for (int x = -1; x < colums + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInsatantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || y == -1 || x == colums || y == rows)
                    toInsatantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                float x1 = -colums/2 + 0.5f + x;
                float y1 = -rows/2 + 0.5f + y;
                GameObject instance = Instantiate(toInsatantiate, new Vector3(x1, y1, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition ()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        IniyializeList();
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
    }
}
