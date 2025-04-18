using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Enums;
using Managers;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;

namespace Sprites
{
    public class Piece 
    {
        private Texture2D _chessPieces;
        private Rectangle _position;
        private Rectangle _pieceSprite;
        private ColorsEnum _color;
        private PiecesEnum _pieceType;
        private const int UNIT = 45;
        private bool isFirstMove = true;
        private bool isSecondMove = false;
        private bool EnPassant = false;
        private bool _didMove = false;

        public Rectangle Position { get { return _position; } }
        public bool OnLeft { get { return _position.Left == UNIT; }}
        public bool OnRight { get { return _position.Right == 9*UNIT; }}
        public bool OnBottom { get { return _position.Bottom == 9*UNIT; }}
        public bool OnTop { get { return _position.Top == UNIT; }}
        public bool IsWhite { get { return _color == ColorsEnum.White; }}
        public bool DidMove { get { return _didMove; } }

        public Piece()
        {
            this._position = new Rectangle(0, 0, UNIT, UNIT);
        }

        public Piece(ColorsEnum color, PiecesEnum piece, Texture2D chessPieces, Rectangle position)
        {
            this.Load(color, piece, chessPieces, position);
        }

        public Piece(Piece pieceToCopy, Rectangle newPosition)
        {
            this.Load(pieceToCopy._color, pieceToCopy._pieceType, pieceToCopy._chessPieces, newPosition);
        }

        public void Load(ColorsEnum color, PiecesEnum piece, Texture2D chessPieces, Rectangle position)
        {
            this._color = color;
            this._pieceType = piece;
            this._chessPieces = chessPieces;
            this._position = position;
            this._pieceSprite = new Rectangle((int)_pieceType * UNIT, (int)_color * UNIT, UNIT, UNIT);
        }

        public void Update(InputStateManager inputStateManager)
        {

        }

        public void Move(Rectangle targetRectangle)
        {
            this._didMove = !(targetRectangle == this._position);
            this.isSecondMove = this._didMove ? this.isFirstMove : this.isSecondMove;
            this.isFirstMove = this.isFirstMove && !this._didMove;
            this._position = targetRectangle;
        }

        public bool IsClicked(InputStateManager inputStateManager)
        {
            return  inputStateManager.CurrentMouseState.X >= this._position.X && 
                    inputStateManager.CurrentMouseState.X <= this._position.X + UNIT &&
                    inputStateManager.CurrentMouseState.Y >= this._position.Y &&
                    inputStateManager.CurrentMouseState.Y <= this._position.Y + UNIT;
        }

        public bool Collision(Rectangle targetPieceRectangle)
        {
            return  this.EnPassant ||
                    (
                        this._position.X == targetPieceRectangle.X &&
                        this._position.Y == targetPieceRectangle.Y
                    );
        }

        public IEnumerable<Rectangle> DisplayMoves(IEnumerable<Piece> teamPieces, IEnumerable<Piece> enemyPieces)
        {
            switch (this._pieceType)
            {
                case PiecesEnum.King:
                    return KingMoves();
                case PiecesEnum.Queen:
                    return QueenMoves(teamPieces, enemyPieces);
                case PiecesEnum.Bishop:
                    return BishopMoves(teamPieces, enemyPieces);
                case PiecesEnum.Knight:
                    return KnightMoves();
                case PiecesEnum.Rook:
                    return RookMoves(teamPieces, enemyPieces);
                case PiecesEnum.Pawn:
                    return PawnMoves(enemyPieces);
                default:
                    return new List<Rectangle>();
            }
        }

        private IEnumerable<Rectangle> KingMoves()
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

            return moves;
        }

        private IEnumerable<Rectangle> QueenMoves(IEnumerable<Piece> teamPieces, IEnumerable<Piece> enemyPieces)
        {
            List<Rectangle> moves = [];

            moves.AddRange(BishopMoves(teamPieces, enemyPieces));
            moves.AddRange(RookMoves(teamPieces, enemyPieces));

            return moves;
        }

        private IEnumerable<Rectangle> BishopMoves(IEnumerable<Piece> teamPieces, IEnumerable<Piece> enemyPieces)
        {
            List<Rectangle> moves = [];

            int currX = this._position.X;
            int currY = this._position.Y;

            while (true) // Move Up-Left
            {
                Rectangle move = new Rectangle(currX - UNIT, currY - UNIT, UNIT, UNIT);

                if (move.X == 0 || move.Y == 0) break;

                moves.Add(move);

                if (teamPieces.Where(w => w.Collision(move)).Any() || enemyPieces.Where(w => w.Collision(move)).Any()) break;

                currX = move.X;
                currY = move.Y;
            }
            
            currX = this._position.X;
            currY = this._position.Y;

            while (true) // Move Up-Right
            {
                Rectangle move = new Rectangle(currX + UNIT, currY - UNIT, UNIT, UNIT);

                if (move.X == 9*UNIT || move.Y == 0) break;

                moves.Add(move);

                if (teamPieces.Where(w => w.Collision(move)).Any() || enemyPieces.Where(w => w.Collision(move)).Any()) break;

                currX = move.X;
                currY = move.Y;
            }

            currX = this._position.X;
            currY = this._position.Y;

            while (true) // Move Down-Right
            {
                Rectangle move = new Rectangle(currX + UNIT, currY + UNIT, UNIT, UNIT);

                if (move.X == 9*UNIT || move.Y == 9*UNIT) break;

                moves.Add(move);

                if (teamPieces.Where(w => w.Collision(move)).Any() || enemyPieces.Where(w => w.Collision(move)).Any()) break;

                currX = move.X;
                currY = move.Y;
            }

            currX = this._position.X;
            currY = this._position.Y;

            while (true) // Move Down-Left
            {
                Rectangle move = new Rectangle(currX - UNIT, currY + UNIT, UNIT, UNIT);

                if (move.X == 0 || move.Y == 9*UNIT) break;

                moves.Add(move);

                if (teamPieces.Where(w => w.Collision(move)).Any() || enemyPieces.Where(w => w.Collision(move)).Any()) break;

                currX = move.X;
                currY = move.Y;
            }

            return moves;
        }

        private IEnumerable<Rectangle> KnightMoves()
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

        private IEnumerable<Rectangle> RookMoves(IEnumerable<Piece> teamPieces, IEnumerable<Piece> enemyPieces)
        {
            List<Rectangle> moves = [];

            int currX = this._position.X;
            int currY = this._position.Y;

            while (true) // Move Up
            {
                Rectangle move = new Rectangle(currX, currY - UNIT, UNIT, UNIT);

                if (move.Y == 0) break;

                moves.Add(move);

                if (teamPieces.Where(w => w.Collision(move)).Any() || enemyPieces.Where(w => w.Collision(move)).Any()) break;

                currX = move.X;
                currY = move.Y;
            }
            
            currX = this._position.X;
            currY = this._position.Y;

            while (true) // Move Right
            {
                Rectangle move = new Rectangle(currX + UNIT, currY, UNIT, UNIT);

                if (move.X == 9*UNIT) break;

                moves.Add(move);

                if (teamPieces.Where(w => w.Collision(move)).Any() || enemyPieces.Where(w => w.Collision(move)).Any()) break;

                currX = move.X;
                currY = move.Y;
            }

            currX = this._position.X;
            currY = this._position.Y;

            while (true) // Move Down
            {
                Rectangle move = new Rectangle(currX, currY + UNIT, UNIT, UNIT);

                if (move.Y == 9*UNIT) break;

                moves.Add(move);

                if (teamPieces.Where(w => w.Collision(move)).Any() || enemyPieces.Where(w => w.Collision(move)).Any()) break;

                currX = move.X;
                currY = move.Y;
            }

            currX = this._position.X;
            currY = this._position.Y;

            while (true) // Move Left
            {
                Rectangle move = new Rectangle(currX - UNIT, currY, UNIT, UNIT);

                if (move.X == 0) break;

                moves.Add(move);

                if (teamPieces.Where(w => w.Collision(move)).Any() || enemyPieces.Where(w => w.Collision(move)).Any()) break;

                currX = move.X;
                currY = move.Y;
            }


            return moves;
        }

        private IEnumerable<Rectangle> PawnMoves(IEnumerable<Piece> enemyPieces)
        {
            List<Rectangle> moves = [];

            int unit = UNIT * (this._color == ColorsEnum.White ? -1 : 1);
            Rectangle forward = new Rectangle(this._position.X, this._position.Y + unit, UNIT, UNIT);
            if (!enemyPieces.Where(w => w._position == forward).Any())
                moves.Add(forward);
            if (this.isFirstMove) 
                moves.Add(new Rectangle(this._position.X, this._position.Y + 2*unit, UNIT, UNIT));
            
            Rectangle attackLeft = new Rectangle(this._position.X - UNIT, this._position.Y + unit, UNIT, UNIT);
            Rectangle attackRight = new Rectangle(this._position.X + UNIT, this._position.Y + unit, UNIT, UNIT);

            if (enemyPieces.Where(w => 
                w._position == attackLeft ||
                (
                    w._position.X == attackLeft.X &&
                    Math.Abs(w._position.Y - attackLeft.Y) == UNIT && 
                    w._pieceType == PiecesEnum.Pawn && 
                    w.isSecondMove
                ))
            .Any())
            {
                moves.Add(attackLeft);
            }

            if (enemyPieces.Where(w => 
                w._position == attackRight ||
                (
                    w._position.X == attackRight.X &&
                    Math.Abs(w._position.Y - attackRight.Y) == UNIT && 
                    w._pieceType == PiecesEnum.Pawn && 
                    w.isSecondMove
                ))
            .Any())
            {
                moves.Add(attackRight);
            }

            return moves;
        }

        public void Draw(SpriteBatch spriteBatch, float order = 0)
        {
            spriteBatch.Draw(_chessPieces, _position, _pieceSprite, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.8f + order);
        }
    }
}