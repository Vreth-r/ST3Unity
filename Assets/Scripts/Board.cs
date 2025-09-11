using System;
using UnityEngine;

public enum Marker
{
    B, // Blank
    X,
    O,
    T, // Tie
    N  // Neither
}

public class Board
{
    private Marker[,] board = new Marker[9, 10]; // 9 inner boards, each with 9 positions + 1 win state
    private int gameToPlay = -1;
    private int lastPosPlayed = -1;
    public int[] winningGames = new int[3];

    public Board()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
                board[i, j] = Marker.B;

            board[i, 9] = Marker.N; // status marker
        }
    }

    public int GetGameToPlay() => gameToPlay;

    public bool IsPositionPlayable(int posOuter, int posInner)
    {
        bool isSpaceEmpty = board[posOuter, posInner] == Marker.B && board[posOuter, 9] == Marker.N;
        bool validGame = gameToPlay == -1 || gameToPlay == posOuter;
        return isSpaceEmpty && validGame;
    }

    public bool SetPosition(int posOuter, int posInner, Marker marker)
    {
        if (!IsPositionPlayable(posOuter, posInner)) return false;

        board[posOuter, posInner] = marker;

        if (board[posInner, 9] != Marker.N)
            gameToPlay = -1;
        else
            gameToPlay = posInner;

        lastPosPlayed = posInner;
        return true;
    }

    public int IsGameDone(int posOuter)
    {
        int returnCode = 0;

        Marker result = CheckWinner(GetRow(board, posOuter), Marker.B);

        if (result != Marker.N)
        {
            returnCode = result == Marker.T ? 3 : 1;

            if (lastPosPlayed == posOuter)
                gameToPlay = -1;

            board[posOuter, 9] = result;

            Marker[] outerResults = new Marker[9];
            for (int i = 0; i < 9; i++)
                outerResults[i] = board[i, 9];

            Marker outerResult = CheckWinner(outerResults, Marker.N);

            if (outerResult == Marker.T)
                returnCode = 4;
            else if (outerResult != Marker.N)
                returnCode = 2;
        }

        return returnCode;
    }

    public Marker[] GetRow(Marker[,] fullBoard, int outerIndex)
    {
        Marker[] row = new Marker[9];
        for (int i = 0; i < 9; i++)
            row[i] = fullBoard[outerIndex, i];
        return row;
    }

    public Marker CheckWinner(Marker[] line, Marker tieMarker)
    {
        Marker winner = CheckRows(line);
        if (winner != Marker.N && winner != Marker.T) return winner;

        winner = CheckColumns(line);
        if (winner != Marker.N && winner != Marker.T) return winner;

        winner = CheckDiagonals(line);
        if (winner != Marker.N && winner != Marker.T) return winner;

        if (IsTie(line, tieMarker))
            return Marker.T;

        return Marker.N;
    }

    private Marker CheckRows(Marker[] line)
    {
        for (int i = 0; i < 3; i++)
        {
            int idx = i * 3;
            if (line[idx] != Marker.B && line[idx] != Marker.N && line[idx] != Marker.T &&
                line[idx] == line[idx + 1] && line[idx] == line[idx + 2])
            {
                winningGames[0] = idx;
                winningGames[1] = idx + 1;
                winningGames[2] = idx + 2;
                return line[idx];
            }
        }
        return Marker.N;
    }

    private Marker CheckColumns(Marker[] line)
    {
        for (int i = 0; i < 3; i++)
        {
            if (line[i] != Marker.B && line[i] != Marker.N && line[i] != Marker.T &&
                line[i] == line[i + 3] && line[i] == line[i + 6])
            {
                winningGames[0] = i;
                winningGames[1] = i + 3;
                winningGames[2] = i + 6;
                return line[i];
            }
        }
        return Marker.N;
    }

    private Marker CheckDiagonals(Marker[] line)
    {
        if (line[0] != Marker.B && line[0] != Marker.N && line[0] != Marker.T &&
        line[0] == line[4] && line[0] == line[8])
        {
            winningGames[0] = 0;
            winningGames[1] = 4;
            winningGames[2] = 8;
            return line[0];
        }
        if (line[2] != Marker.B && line[2] != Marker.N && line[2] != Marker.T &&
            line[2] == line[4] && line[2] == line[6])
        {
            winningGames[0] = 2;
            winningGames[1] = 4;
            winningGames[2] = 6;
            return line[2];
        }
        return Marker.N;
    }

    private bool IsTie(Marker[] line, Marker tieMarker)
    {
        foreach (var marker in line)
        {
            if (marker == tieMarker)
                return false;
        }
        return true;
    }
}
