using System;
using Sprites;

namespace Managers
{
    public class PieceManager
    {
        private int _id;
        private static PieceManager _instance;
        private PieceBase _enPassant;

        public static PieceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PieceManager();
                }

                return _instance;
            }
        }
        private PieceManager()
        {
            _id = 0;
        }

        public int GenerateID()
        {
            _id++;
            return _id;
        }

        public PieceBase GenerateCopyPiece(PieceBase pieceBase)
        {
            if (pieceBase.GetType() == typeof(King)) return new King((King)pieceBase, pieceBase.Position);
            else if (pieceBase.GetType() == typeof(Queen)) return new Queen((Queen)pieceBase, pieceBase.Position);
            else if (pieceBase.GetType() == typeof(Bishop)) return new Bishop((Bishop)pieceBase, pieceBase.Position);
            else if (pieceBase.GetType() == typeof(Knight)) return new Knight((Knight)pieceBase, pieceBase.Position);
            else if (pieceBase.GetType() == typeof(Rook)) return new Rook((Rook)pieceBase, pieceBase.Position);
            else if (pieceBase.GetType() == typeof(Pawn)) return new Pawn((Pawn)pieceBase, pieceBase.Position);
            else throw new ArgumentException("Invaiid Piece");
        }

        /// <summary>
        /// Ok the Set as any piece, as the Getter will only return a value if this piece is a Pawn
        /// </summary>
        /// <param name="piece"></param>
        public void SetEnPassantPiece(PieceBase piece)
        {
            this._enPassant = piece;
        }

        /// <summary>
        /// Only Returns a value of Piece is a Pawn
        /// </summary>
        /// <returns></returns>
        public PieceBase GetEnPassantReadyPawn()
        {
            return _enPassant?.GetType() == typeof(Pawn) ? _enPassant : null;
        }

    }
}