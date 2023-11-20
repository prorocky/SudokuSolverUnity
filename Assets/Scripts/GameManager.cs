using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // magic number for length of sudoku row/col
    const int N = 9;
    // magic number for number of regions on board
    const int R = 3;

    // GameObject board that is used in game
    [SerializeField]
    GameObject BOARD;

    // board for calculations
    int[,] intBoard;

    Color selectedTileColor = new Color(0.7f, 0.7f, 1f);
    Color defaultTileColor = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        intBoard = new int[9,9];
        // in order to assign to int array, need to keep track of rows and cols of regions AND tiles
        for (int regRow = 0; regRow < R; regRow++)
        {
            for (int regCol = 0; regCol < R; regCol++)
            {
                // current region
                // GameObject region = BOARD.transform.GetChild(regRow * R + regCol).gameObject;
                for (int tileRow = 0; tileRow < R; tileRow++)
                {
                    for (int tileCol = 0; tileCol < R; tileCol++)
                    {
                        // current tile
                        // GameObject tile = region.transform.GetChild(tileRow * R + tileCol).gameObject;
                        intBoard[regRow * R + tileRow, regCol * R + tileCol] = 0;
                    }
                }
            }
        }
        updateBoard();
    }

    void updateBoard() {
        for (int regRow = 0; regRow < R; regRow++)
        {
            for (int regCol = 0; regCol < R; regCol++)
            {
                // current region
                GameObject region = BOARD.transform.GetChild(regRow * R + regCol).gameObject;
                for (int tileRow = 0; tileRow < R; tileRow++)
                {
                    for (int tileCol = 0; tileCol < R; tileCol++)
                    {
                        // current tile
                        GameObject tile = region.transform.GetChild(tileRow * R + tileCol).gameObject;
                        // print(regRow * R + tileRow + ", " + (regCol * R + tileCol));
                        int val = intBoard[regRow * R + tileRow, regCol * R + tileCol];
                        // print(tile.GetComponentInChildren<TMPro.TextMeshProUGUI>());
                        tile.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = (val != 0) ? val.ToString() : "";
                    }
                }
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
