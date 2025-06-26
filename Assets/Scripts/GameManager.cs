using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public InnerBoard[] innerBoards; // 9 InnerBoard GameObjects (in UI GridLayout)
    public TextMeshProUGUI statusMessage;

    public GameObject innerBoardPrefab;
    public Transform ibParent;

    private Board board;
    private ToggleSwitch<Marker> turn;
    private ToggleSwitch<Color> turnColor;
    private ToggleSwitch<Color> winColor;

    private int status;
    public string smString;

    public static GameManager Instance;

    void Awake() => Instance = this;
    void Start()
    {
        innerBoards = new InnerBoard[9];
        board = new Board();
        turn = new ToggleSwitch<Marker>(Marker.X, Marker.O);
        turnColor = new ToggleSwitch<Color>(Color.red, Color.blue);
        winColor = new ToggleSwitch<Color>(Color.magenta, Color.cyan); // Unity doesn't have pink

        statusMessage.text = "SUPER TIC TAC TOE";

        for (int i = 0; i < 9; i++)
        {
            GameObject ib = Instantiate(innerBoardPrefab, ibParent);
            innerBoards[i] = ib.GetComponent<InnerBoard>();
            innerBoards[i].Init(i);
        }
    }

    public void OnCellPressed(int outer, int inner, CellButton cell)
    {
        if (!board.SetPosition(outer, inner, turn.GetValue()))
        {
            statusMessage.text = "Invalid move, try again";
            return;
        }

        status = board.IsGameDone(outer);
        smString = $"{turn.GetInverseValue()}'s Turn";
        cell.SetText(turn.GetValue().ToString());
        cell.SetColor(turnColor.GetValue());
        cell.SetInteractable(false);

        if (status == 1)
        {
            innerBoards[outer].SetAllCells(winColor.GetValue(), false);
            smString = $"{turn.GetValue()} wins a game!\n{turn.GetInverseValue()}'s Turn";
        }
        else if (status == 2)
        {
            innerBoards[outer].SetAllCells(winColor.GetValue(), false);
            DisableNonWinningBoards();
            statusMessage.text = $"{turn.GetValue()} wins the board!";
            return;
        }
        else if (status == 3)
        {
            innerBoards[outer].SetAllCells(Color.gray, false);
            smString = "Tie game!";
        }
        else if (status == 4)
        {
            foreach (var board in innerBoards)
            {
                board.SetAllCells(Color.gray, false);
            }
            statusMessage.text = "Tie Board!";
            return;
        }

        int gameToPlay = board.GetGameToPlay();
        SetNonPlayable(true);
        SetPlayable(gameToPlay);

        toggleSwitches();
        statusMessage.text = smString;
    }

    private void SetPlayable(int outerIndex)
    {
        if (outerIndex == -1)
        {
            foreach (var ib in innerBoards)
                ib.HighlightPlayable(true);
        }
        else
        {
            for (int i = 0; i < innerBoards.Length; i++)
            {
                if (i == outerIndex)
                    innerBoards[i].HighlightPlayable(true);
            }
        }
    }

    private void SetNonPlayable(bool interactable)
    {
        foreach (var ib in innerBoards)
        {
            ib.HighlightPlayable(false);
            ib.SetInactiveVisual(Color.gray);
        }
    }

    private void DisableNonWinningBoards()
    {
        foreach (var ib in innerBoards)
        {
            ib.SetAllCells(Color.gray, false);
        }

        foreach (int idx in board.winningGames)
        {
            innerBoards[idx].SetAllCells(winColor.GetValue(), false);
        }
    }

    private void toggleSwitches()
    {
        turn.Toggle();
        turnColor.Toggle();
        winColor.Toggle();
    }

    public Marker GetCurrentTurnMarker() => turn.GetValue();
    public Color GetCurrentColor() => turnColor.GetValue();
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Restart()
    {
        for (int i = 0; i < 9; i++)
        {
            Destroy(innerBoards[i].gameObject);
        }
        Start();
    }

}
