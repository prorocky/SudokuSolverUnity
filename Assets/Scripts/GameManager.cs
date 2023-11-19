using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Board
{
    
}

struct Region
{
    Tile[,] tiles;
}

struct Tile
{
    Color currentColor;
    TextMesh text;
}


public class GameManager : MonoBehaviour
{
    // magic number for length of sudoku row/col
    const int N = 9;

    // GameObject board that is used in game
    

    // board for calculations
    int[,] intBoard;

    Color selectedTileColor = new Color(0.7f, 0.7f, 1f);
    Color defaultTileColor = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        // initialize board
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                intBoard[i,j] = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
