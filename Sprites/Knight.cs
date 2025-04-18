using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    public class Knight : PieceBase
    {
        public Knight(ColorsEnum color, Texture2D chessPieces, Rectangle position) : base(color, PiecesEnum.Knight, chessPieces, position)
        {

        }

        public Knight(Knight piece, Rectangle position) : base(piece, position)
        {

        }

        public override IEnumerable<Rectangle> DisplayMoves(IEnumerable<PieceBase> teamPieces, IEnumerable<PieceBase> enemyPieces)
        {
            List<Rectangle> moves = [];

            int newX = this._position.X - 2*UNIT;

            while (newX <= this._position.X + 2*UNIT)
            {
                if (newX == this._position.X || newX == 0|| newX == 9*UNIT)
                {
                    newX += UNIT;
                    continue;
                }
                int moveY = 3*UNIT - Math.Abs(newX - this._position.X);
                if (this._position.Y + moveY <= 8*UNIT)
                {
                    moves.Add(new Rectangle(newX, this._position.Y + moveY, UNIT, UNIT));
                }

                if (this._position.Y - moveY >= UNIT)
                {
                    moves.Add(new Rectangle(newX, this._position.Y - moveY, UNIT, UNIT));
                }
                newX += UNIT;
            }

            return moves;
        }
    }
}