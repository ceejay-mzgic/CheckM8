using System.Collections.Generic;
using UnityEngine;

public enum PieceType { Pawn, Knight, Bishop, Rook, Queen, King }

public class ChessPiece : MonoBehaviour
{
    public PieceType pieceType;
    public bool isPlayer;                // true = player, false = enemy
    public Vector2Int boardPos;          // position on the 5x5 board

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Initialize the piece with type, side, position, and color
    public void Init(PieceType type, bool isPlayer, Vector2Int pos, Color c)
    {
        pieceType = type;
        this.isPlayer = isPlayer;
        boardPos = pos;
        sr = GetComponent<SpriteRenderer>();
        sr.color = c;
    }

    // TEMP: return basic available moves (up, down, left, right) for testing
    public virtual List<Vector2Int> GetAvailableMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        // just for testing — up, down, left, right
        moves.Add(boardPos + Vector2Int.up);
        moves.Add(boardPos + Vector2Int.down);
        moves.Add(boardPos + Vector2Int.left);
        moves.Add(boardPos + Vector2Int.right);

        return moves;
    }

    // Respond to clicks
    private void OnMouseDown()
    {
        Debug.Log("Clicked piece: " + pieceType + " at " + boardPos);

        // tell BoardManager this piece was clicked
        if (BoardManager.Instance != null)
            BoardManager.Instance.OnPieceClicked(this);
    }
}
