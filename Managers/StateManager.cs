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
        private bool _whiteInCheck = false;
        private bool _blackInCheck = false;
        public bool WhiteInCheck { get { return _whiteInCheck; } }
        public bool BlackInCheck { get { return _blackInCheck; } }
        public StateManager()
        {

        }

        public void CheckGameState(IEnumerable<PieceBase> checkingPieces, IEnumerable<PieceBase> toBeChecked)
        {
            if (IsInCheck(checkingPieces, toBeChecked))
            {
                _whiteInCheck = checkingPieces.FirstOrDefault().IsWhite;
                _blackInCheck = !checkingPieces.FirstOrDefault().IsWhite;
                return;
            }

            _whiteInCheck = false;
            _blackInCheck = false;
        }

        public bool IsInCheck(IEnumerable<PieceBase> checkingPieces, IEnumerable<PieceBase> toBeChecked)
        {
            King kingToVerify = (King)toBeChecked.Where(w => w.GetType() == typeof(King)).FirstOrDefault();

            foreach (PieceBase piece in checkingPieces)
            {
                // If there will be a collision with the available moves, then the other player is in check
                if (
                    piece.DisplayMoves(checkingPieces, toBeChecked)
                         .Where(w => kingToVerify.Collision(w))
                         .Any()
                )
                {
                    return true;
                }
            }

            return false;
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
                if (!IsInCheck(copyOfEnemyPieces, copyOfTeamPieces)) validMoves.Add(move);
                if (copyPieceToTake != null)
                {
                    copyOfEnemyPieces.Add(copyPieceToTake);
                }
            }

            return validMoves;
        }

    }
}