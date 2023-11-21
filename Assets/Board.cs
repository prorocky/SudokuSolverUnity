using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    class instruction
    {
        public int row;
        public int col;
        public int num;

        public instruction(int row, int col, int num)
        {
            this.row = row;
            this.col = col;
            this.num = num;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}): {2}", row, col, num);
        }
    }


    // magic number for length of sudoku row/col
    const int N = 9;
    // magic number for sqrt(N)
    const int R = 3;

    // GameObject board that is used in game
    [SerializeField]
    GameObject BOARD;

    // board for calculations
    int[,] intBoard;

    // this list will hold the list of instructions to then display onto the board in game scene
    List<instruction> list;

    Color selectedTileColor = new Color(0.7f, 0.7f, 1f);
    Color defaultTileColor = Color.white;
    // delay for coroutines, will be mutable via scroller in game scene
    [SerializeField]
    float delay = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        intBoard = new int[9,9];
        list = new List<instruction>();
        // clearBoard();
        // generateBoard();

        // updateBoard();

        // solve(intBoard, true);
        // StartCoroutine("showAlgorithm");

    }

    void generateSolvedBoard() {
        int r = Random.Range(0, N-1);
        int c = Random.Range(0, N-1);
        int n = Random.Range(1, N);

        intBoard[r, c] = n;

        solve(intBoard, false);
    }

    void generateBoard() {
        generateSolvedBoard();

        // delete 56 at random
        int numToDelete = 56;
        // instead of 1 random between 0 and N*N, 2 random for row and col to add more randomness
        int rng1, rng2;
        while (numToDelete > 0)
        {
            rng1 = Random.Range(0, N);
            rng2 = Random.Range(0, N);

            if (intBoard[rng1, rng2] != 0) {
                intBoard[rng1, rng2] = 0;
                numToDelete--;
            }
            // StartCoroutine("deleteNumbersSlowly");
        }
    }

    // creating method AND coroutine doing the same thing except the coroutine has a delay
    // the delay will be able to be selected as a choice from the "gameplay" scene to watch the numbers get deleted slowly
    void deleteNumbers() {
        // delete 56 at random
        int numToDelete = 56;
        // instead of 1 random between 0 and N*N, 2 random for row and col to add more randomness
        int rng1, rng2;
        while (numToDelete > 0)
        {
            rng1 = Random.Range(0, N);
            rng2 = Random.Range(0, N);

            if (intBoard[rng1, rng2] != 0) {
                intBoard[rng1, rng2] = 0;
                numToDelete--;
            }
        }
    }

    IEnumerator deleteNumbersSlowly() {
        // delete 56 at random
        int numToDelete = 56;
        // instead of 1 random between 0 and N*N, 2 random for row and col to add more randomness
        int rng1, rng2;
        while (numToDelete > 0)
        {
            rng1 = Random.Range(0, N);
            rng2 = Random.Range(0, N);

            if (intBoard[rng1, rng2] != 0) {
                intBoard[rng1, rng2] = 0;
                numToDelete--;
                yield return new WaitForSeconds(delay);
                updateBoard();
            }
        }
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

    bool isSafe(int row, int col, int num) {
        return intBoard[row, col] == 0 && isSafeRow(row, num) && isSafeCol(col, num) && isSafeRegion(row, col, num);
    }

    bool isSafeRow(int row, int num) {
        for (int i = 0; i < N; i++)
        {
            if (intBoard[row, i] == num) return false;
        }
        return true;
    }

    bool isSafeCol(int col, int num) {
        for (int i = 0; i < N; i++)
        {
            if (intBoard[i, col] == num) return false;
        }
        return true;
    }

    bool isSafeRegion(int row, int col, int num) {
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

    bool solve(int[,] board, bool addToList)
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
            if (addToList) list.Add(new instruction(row, col, num));
            if (isSafe(row, col, num)) {
                board[row, col] = num;

                if (solve(board, addToList)) {
                    return true;
                } 
                else {
                    board[row, col] = 0;
                    if (addToList) list.Add(new instruction(row, col, 0));
                }
            }
            if (num == N) {
                board[row, col] = 0;
                if (!isSafe(row, col, N)) {
                    board[row, col] = 0;
                    if (addToList) list.Add(new instruction(row, col, 0));
                }
            }
        }
        return false;
    }

    // coroutine to show step by step changes of solver algorithm
    // coroutine instead of function to add delays freely whenever I need
    IEnumerator showAlgorithm()
    {
        // list is empty, immediately leave coroutine
        if (list.Count != 0) {
            instruction current = list[0];
            instruction next;

            // check next instruction to see if it's on a different tile, if yes, then we will want to change the color of the tile back to default
            bool changeColorBack = true;
            if (list.Count > 1)
            {
                next = list[1];
                if (current.row == next.row && current.col == next.col)
                {
                    changeColorBack = false;
                }
            }

            int row = current.row;
            int col = current.col;
            int num = current.num;

            int regRow = row / R;
            int tileRow = row % R;

            int regCol = col / R;
            int tileCol = col % R;


            GameObject region = BOARD.transform.GetChild(regRow * R + regCol).gameObject;
            GameObject tile = region.transform.GetChild(tileRow * R + tileCol).gameObject;

            tile.GetComponent<Image>().color = selectedTileColor;
            tile.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = (num != 0) ? num.ToString() : "";

            yield return new WaitForSeconds(delay);
            if (list.Count > 1) {
                next = list[1];
                if (current.row * N + current.col - 1 == next.row * N + next.col) {
                    tile.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
                    yield return new WaitForSeconds(delay * 2);
                }
            }
            
            list.RemoveAt(0);
            if (changeColorBack) tile.GetComponent<Image>().color = defaultTileColor;
            
            StartCoroutine("showAlgorithm");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
