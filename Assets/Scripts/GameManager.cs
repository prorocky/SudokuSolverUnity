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
    static int[,] intBoard;

    Color selectedTileColor = new Color(0.7f, 0.7f, 1f);
    Color defaultTileColor = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        intBoard = new int[9,9];
        clearBoard();
        generateBoard();
        updateBoard();

    }

    void generateBoard() {
        int r = Random.Range(0, N-1);
        int c = Random.Range(0, N-1);
        int n = Random.Range(1, N);

        intBoard[r, c] = n;

        solve(intBoard);
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
                        tile.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = (val != 0) ? val.ToString() : "";
                    }
                }
            }
            
        }
    }

    void clearBoard() {
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
    }

    static bool isSafe(int row, int col, int num) {
        return isSafeRow(row, num) && isSafeCol(col, num) && isSafeRegion(row, col, num);
    }

    static bool isSafeRow(int row, int num) {
        for (int i = 0; i < N; i++)
        {
            if (intBoard[row, i] == num) return false;
        }
        return true;
    }

    static bool isSafeCol(int col, int num) {
        for (int i = 0; i < N; i++)
        {
            if (intBoard[i, col] == num) return false;
        }
        return true;
    }

    static bool isSafeRegion(int row, int col, int num) {
        int rowStart = row - row % R;
        int colStart = col - col % R;
        for (int i = rowStart; i < rowStart + R; i++)
        {
            for (int j = colStart; j < colStart + R; j++)
            {
                if (intBoard[i, j] == num) return false;
            }
        }
        return true;
    }

    static bool solve(int[,] board)
    {
        // set to negative to not cause errors with static calling
        int row = -1, col = -1;
        bool tilesFilled = true;

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (board[i, j] == 0)
                {
                    row = i;
                    col = j;

                    tilesFilled = false;
                    break;
                }
            }

            if (tilesFilled) {
                return true;
            }
        }

        // backtrack each row
        for (int num = 1; num <= N; num++)
        {
            if (isSafe(row, col, num)) {
                board[row, col] = num;
                if (solve(board)) {
                    return true;
                } 
                else {
                    board[row, col] = 0;
                }
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
