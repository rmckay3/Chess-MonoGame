using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace Managers
{
    public class StateManager
    {
        private GameStateEnum _gameState = GameStateEnum.Normal;
        public GameStateEnum GameState { get { return _gameState; } }
        private PieceBase _checkingPiece;
        public StateManager()
        {

        }

        public void CheckGameState(IEnumerable<PieceBase> checkingPieces, IEnumerable<PieceBase> toBeChecked)
        {
            this._gameState = GameStateEnum.Normal;
            this._checkingPiece = null;
            if (IsCheck(checkingPieces, toBeChecked))
            {
                this._gameState = checkingPieces.FirstOrDefault().IsWhite ? GameStateEnum.BlackInCheck : GameStateEnum.WhiteInCheck;

                if (IsCheckmate(checkingPieces, toBeChecked))
                {
                    this._gameState = this._gameState == GameStateEnum.BlackInCheck ? GameStateEnum.WhiteWin : GameStateEnum.BlackWin;
                }
            }
        }

        public bool IsCheck(IEnumerable<PieceBase> checkingPieces, IEnumerable<PieceBase> toBeChecked)
        {
            King kingToVerify = (King)toBeChecked.Where(w => w.GetType() == typeof(King)).FirstOrDefault();

            foreach (PieceBase piece in checkingPieces)
            {
                IEnumerable<Rectangle> checkingPieceMoves = piece.DisplayMoves(checkingPieces, toBeChecked);
                // If there will be a collision with the available moves, then the other player is in check
                if (
                    checkingPieceMoves.Where(kingToVerify.Collision)
                                      .Any()
                )
                {
                    this._checkingPiece = piece;
                    return true;
                }
            }

            return false;
        }

        public bool IsCheckmate(IEnumerable<PieceBase> checkingPieces, IEnumerable<PieceBase> toBeChecked)
        {
            IEnumerable<Rectangle> checkingPieceMoves = this._checkingPiece.DisplayMoves(checkingPieces, toBeChecked);

            List<PieceBase> copyOfTeamPieces = toBeChecked.Select(s => PieceManager.Instance.GenerateCopyPiece(s)).ToList();
            List<PieceBase> copyOfEnemyPieces = checkingPieces.Select(s => PieceManager.Instance.GenerateCopyPiece(s)).ToList();
            foreach (PieceBase teamPiece in toBeChecked.Where(w => w.GetType() != typeof(King)))
            {
                IEnumerable<Rectangle> teamPieceMoves = teamPiece.DisplayMoves(toBeChecked, checkingPieces);
                PieceBase copyTeamPiece = copyOfTeamPieces.Where(w => w.ID == teamPiece.ID).FirstOrDefault();
                foreach (var move in teamPieceMoves.Intersect(checkingPieceMoves))
                {
                    copyTeamPiece.Move(move);
                    var copyPieceToTake = copyOfEnemyPieces.Where(w => w.Collision(copyTeamPiece)).FirstOrDefault();
                    if (copyPieceToTake != null)
                    {
                        copyOfEnemyPieces.Remove(copyPieceToTake);
                    }

                    if (!IsCheck(copyOfEnemyPieces, copyOfTeamPieces)) return false; // Piece can move and King no long be in check. Not Checkmate

                    if (copyPieceToTake != null)
                    {
                        copyOfEnemyPieces.Add(copyPieceToTake);
                    }

                }
            }

            King checkedKing = (King)toBeChecked.Where(w=> w.GetType() == typeof(King))
                                                .FirstOrDefault();

            IEnumerable<Rectangle> checkedKingMoves = checkedKing.DisplayMoves(toBeChecked, checkingPieces);

            PieceBase copyPieceToMove = copyOfTeamPieces.Where(w => w.ID == checkedKing.ID).FirstOrDefault();
            foreach(var move in checkedKingMoves)
            {
                copyPieceToMove.Move(move);
                var copyPieceToTake = copyOfEnemyPieces.Where(w => w.Collision(copyPieceToMove)).FirstOrDefault();
                if (copyPieceToTake != null)
                {
                    copyOfEnemyPieces.Remove(copyPieceToTake);
                }

                if (!IsCheck(copyOfEnemyPieces, copyOfTeamPieces)) return false; // King can escape at least 1 way. Not Checkmate

                if (copyPieceToTake != null)
                {
                    copyOfEnemyPieces.Add(copyPieceToTake);
                }
            }

            return true; // King can not escape or be protected. Checkmate
        }

        public IEnumerable<Rectangle> CheckPlayerMoves(PieceBase pieceToMove, IEnumerable<PieceBase> teamPieces, IEnumerable<PieceBase> enemyPieces)
        {
            List<Rectangle> validMoves = new List<Rectangle>();
            var moves = pieceToMove.DisplayMoves(teamPieces, enemyPieces);
            var copyOfTeamPieces = teamPieces.Select(s => PieceManager.Instance.GenerateCopyPiece(s)).ToList();
            var copyOfEnemyPieces = enemyPieces.Select(s => PieceManager.Instance.GenerateCopyPiece(s)).ToList();
            var copyPieceToMove = copyOfTeamPieces.Where(w => w.ID == pieceToMove.ID).FirstOrDefault();
            foreach(var move in moves)
            {
                copyPieceToMove.Move(move);
                var copyPieceToTake = copyOfEnemyPieces.Where(w => w.Collision(copyPieceToMove)).FirstOrDefault();
                if (copyPieceToTake != null)
                {
                    copyOfEnemyPieces.Remove(copyPieceToTake);
                }

                CheckGameState(copyOfEnemyPieces, copyOfTeamPieces);
                if (this._gameState == GameStateEnum.Normal) validMoves.Add(move);
                if (copyPieceToTake != null)
                {
                    copyOfEnemyPieces.Add(copyPieceToTake);
                }
            }

            return validMoves;
        }

    }
}