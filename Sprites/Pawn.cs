using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    public class Pawn : PieceBase
    {
        protected bool isSecondMove = false;
        protected Rectangle lastPosition = new Rectangle();
        public bool IsSecondMove { get { return isSecondMove;} }
        public Pawn(ColorsEnum color, Texture2D chessPieces, Rectangle position) : base(color, PiecesEnum.Pawn, chessPieces, position)
        {

        }

        public Pawn(Pawn piece, Rectangle position) : base(piece, position)
        {

        }

        public override void Move(Rectangle targetRectangle)
        {
            this._didMove = !(targetRectangle == this._position);
            this.isSecondMove = this._didMove ? this.isFirstMove : this.isSecondMove;
            this.isFirstMove = this.isFirstMove && !this._didMove;
            this.lastPosition = this._didMove ? this._position : this.lastPosition;
            this._position = targetRectangle;
        }

        public override bool Collision(PieceBase targetPiece)
        {
            return this.EnPassant(targetPiece) || base.Collision(targetPiece);
        }

        public bool EnPassant(PieceBase targetPiece)
        {
            if (PieceManager.Instance.GetEnPassantReadyPawn() == null || 
                PieceManager.Instance.GetEnPassantReadyPawn().ID != this.ID ||
                targetPiece.GetType() != typeof(Pawn) || 
                ((Pawn)targetPiece).lastPosition.X - targetPiece.Position.X == 0)
            {
                return false;
            }

            return  this._position.X == targetPiece.Position.X && 
                    Math.Abs(this._position.Y - targetPiece.Position.Y) == UNIT;
        }

        public override IEnumerable<Rectangle> DisplayMoves(IEnumerable<PieceBase> teamPieces, IEnumerable<PieceBase> enemyPieces)
        {
            List<Rectangle> moves = [];

            int unit = UNIT * (this._color == ColorsEnum.White ? -1 : 1);
            Rectangle forward = new Rectangle(this._position.X, this._position.Y + unit, UNIT, UNIT);
            if (!enemyPieces.Where(w => w.Position == forward).Any())
                moves.Add(forward);
            if (this.isFirstMove) 
                moves.Add(new Rectangle(this._position.X, this._position.Y + 2*unit, UNIT, UNIT));
            
            Rectangle attackLeft = new Rectangle(this._position.X - UNIT, this._position.Y + unit, UNIT, UNIT);
            Rectangle attackRight = new Rectangle(this._position.X + UNIT, this._position.Y + unit, UNIT, UNIT);

            if (enemyPieces.Where(w => 
                w.Position == attackLeft ||
                (
                    w.Position.X == attackLeft.X &&
                    w.Position.Y == this._position.Y && 
                    w.GetType() == typeof(Pawn) &&
                    ((Pawn)w).IsSecondMove
                ))
            .Any())
            {
                moves.Add(attackLeft);
            }

            if (enemyPieces.Where(w => 
                w.Position == attackRight ||
                (
                    w.Position.X == attackRight.X &&
                    w.Position.Y == this._position.Y && 
                    w.GetType() == typeof(Pawn) &&
                    ((Pawn)w).IsSecondMove
                ))
            .Any())
            {
                moves.Add(attackRight);
            }

            return moves;

        }
    }
}