using System.Collections.Generic;
using Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    public class Bishop: PieceBase
    {
        public Bishop(ColorsEnum color, Rectangle position) : base(color, PiecesEnum.Bishop, position)
        {

        }

        public Bishop(Bishop piece, Rectangle position) : base(piece, position)
        {

        }

        public override IEnumerable<Rectangle> DisplayMoves(IEnumerable<PieceBase> teamPieces, IEnumerable<PieceBase> enemyPieces)
        {
            return this.DiagonalMoves(teamPieces,enemyPieces);
        }
    }
}