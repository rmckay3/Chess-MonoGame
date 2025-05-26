using System.Collections.Generic;
using Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    public class Rook : PieceBase
    {
        public Rook(ColorsEnum color, Rectangle position) : base(color, PiecesEnum.Rook, position)
        {

        }

        public Rook(Rook piece, Rectangle position) : base(piece, position)
        {

        }

        public override IEnumerable<Rectangle> DisplayMoves(IEnumerable<PieceBase> teamPieces, IEnumerable<PieceBase> enemyPieces)
        {
            return StraightLineMoves(teamPieces, enemyPieces);
        }
    }
}