using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    [Header("Grid Settings")]
    public int boardSize = 5;
    public GameObject tilePrefab;
    public Transform gridRoot;
    public Tile[,] tiles;

    [Header("Pieces")]
    public GameObject piecePrefab;
    public List<ChessPiece> allPieces = new List<ChessPiece>();

    private ChessPiece selectedPiece;
    private List<Vector2Int> highlightedMoves = new List<Vector2Int>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BuildGrid();

        // Spawn test pieces
        SpawnPiece(PieceType.Pawn, true, new Vector2Int(0, 0));
        SpawnPiece(PieceType.Knight, true, new Vector2Int(1, 0));
        SpawnPiece(PieceType.Bishop, true, new Vector2Int(2, 0));

        SpawnPiece(PieceType.Pawn, false, new Vector2Int(0, 4));
        SpawnPiece(PieceType.Knight, false, new Vector2Int(1, 4));
        SpawnPiece(PieceType.Bishop, false, new Vector2Int(2, 4));
    }

    void BuildGrid()
    {
        tiles = new Tile[boardSize, boardSize];

        float startX = -(boardSize - 1) * 0.5f;
        float startY = -(boardSize - 1) * 0.5f;

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                GameObject go = Instantiate(tilePrefab, gridRoot);
                go.transform.position = new Vector3(startX + x, startY + y, 0);

                Tile t = go.GetComponent<Tile>();
                t.coord = new Vector2Int(x, y);

                Color col = ((x + y) % 2 == 0) ? new Color(0.9f, 0.9f, 0.9f) : new Color(0.7f, 0.7f, 0.7f);
                t.SetColor(col);

                tiles[x, y] = t;
            }
        }
    }

    public void SpawnPiece(PieceType type, bool isPlayer, Vector2Int pos)
    {
        GameObject go = Instantiate(piecePrefab, gridRoot);
        ChessPiece piece = go.GetComponent<ChessPiece>();
        piece.Init(type, isPlayer, pos, isPlayer ? Color.blue : Color.red);

        go.transform.position = new Vector3(
            -(boardSize - 1) * 0.5f + pos.x,
            -(boardSize - 1) * 0.5f + pos.y,
            0);

        allPieces.Add(piece);
    }

    // ------------------ Selection and Movement ------------------
    public void OnTileClicked(Tile tile)
    {
        // If a piece is selected and tile is a legal move, move the piece
        if (selectedPiece != null && highlightedMoves.Contains(tile.coord))
        {
            MovePiece(selectedPiece, tile.coord);
            ClearHighlights();
            selectedPiece = null;
            return;
        }

        // Clicked empty tile or invalid move: deselect
        ClearHighlights();
        selectedPiece = null;
    }

    public void OnPieceClicked(ChessPiece piece)
    {
        // Only allow selecting player pieces (blue)
        if (!piece.isPlayer) return;

        selectedPiece = piece;
        HighlightMoves(piece.GetAvailableMoves());
    }

    void HighlightMoves(List<Vector2Int> moves)
    {
        ClearHighlights();
        highlightedMoves = moves;
        foreach (var m in moves)
        {
            if (m.x >= 0 && m.x < boardSize && m.y >= 0 && m.y < boardSize)
                tiles[m.x, m.y].Highlight(true);
        }
    }

    void ClearHighlights()
    {
        foreach (var m in highlightedMoves)
            if (m.x >= 0 && m.x < boardSize && m.y >= 0 && m.y < boardSize)
                tiles[m.x, m.y].Highlight(false);

        highlightedMoves.Clear();
    }

    void MovePiece(ChessPiece piece, Vector2Int target)
    {
        // Move piece visually
        piece.boardPos = target;
        piece.transform.position = new Vector3(
            -(boardSize - 1) * 0.5f + target.x,
            -(boardSize - 1) * 0.5f + target.y,
            0);

        Debug.Log(piece.pieceType + " moved to " + target);
    }
}
