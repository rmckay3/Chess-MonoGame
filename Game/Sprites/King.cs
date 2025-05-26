using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    public class King : PieceBase
    {
        private bool _castle = true;
        public bool Castle { get { return _castle; } }
        public King(ColorsEnum color, Rectangle position) : base(color, PiecesEnum.King, position)
        {

        }

        public King(King piece, Rectangle position) : base(piece, position)
        {

        }

        public override void Move(Rectangle targetRectangle)
        {
            this._castle = this._castle && this.FirstMove;
            base.Move(targetRectangle);
        }

        public override IEnumerable<Rectangle> DisplayMoves(IEnumerable<PieceBase> teamPieces, IEnumerable<PieceBase> enemyPieces)
        {
            List<Rectangle> moves = [];

            if (!OnTop) moves.Add(new(this._position.X, this._position.Y - UNIT, UNIT, UNIT));
            if (!OnTop && !OnLeft) moves.Add(new(this._position.X - UNIT, this._position.Y - UNIT, UNIT, UNIT));
            if (!OnTop && !OnRight) moves.Add(new(this._position.X + UNIT, this._position.Y - UNIT, UNIT, UNIT));
            if (!OnLeft) moves.Add(new(this._position.X - UNIT, this._position.Y, UNIT, UNIT));
            if (!OnRight) moves.Add(new(this._position.X + UNIT, this._position.Y, UNIT, UNIT));
            if (!OnBottom) moves.Add(new(this._position.X, this._position.Y + UNIT, UNIT, UNIT));
            if (!OnBottom && !OnLeft) moves.Add(new(this._position.X - UNIT, this._position.Y + UNIT, UNIT, UNIT));
            if (!OnBottom && !OnRight) moves.Add(new(this._position.X + UNIT, this._position.Y + UNIT, UNIT, UNIT));

            if (this.isFirstMove)
            {
                var rooks = teamPieces.Where(w => w.GetType() == typeof(Rook) && w.FirstMove).ToList();
                if (rooks.Count() == 0) return moves;

                // Left Rook
                if (rooks.Where(w => w.Position.X == UNIT).Any())
                    moves.Add(new(2*UNIT, this._position.Y, UNIT, UNIT));

                if (rooks.Where(w => w.Position.X == 8*UNIT).Any())
                    moves.Add(new(7*UNIT, this._position.Y, UNIT, UNIT));
            }

            return moves;
        }
    }   
}