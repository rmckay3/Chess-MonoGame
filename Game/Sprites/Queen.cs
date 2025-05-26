using System.Collections.Generic;
using Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    public class Queen : PieceBase
    {
        public Queen(ColorsEnum color, Rectangle position) : base(color, PiecesEnum.Queen, position)
        {

        }

        public Queen(Queen piece, Rectangle position) : base(piece, position)
        {

        }

        public override IEnumerable<Rectangle> DisplayMoves(IEnumerable<PieceBase> teamPieces, IEnumerable<PieceBase> enemyPieces)
        {
            List<Rectangle> moves = [];

            moves.AddRange(DiagonalMoves(teamPieces, enemyPieces));
            moves.AddRange(StraightLineMoves(teamPieces, enemyPieces));

            return moves;
        }
    }
}