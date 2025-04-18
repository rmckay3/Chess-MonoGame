using System.Collections.Generic;
using System.Linq;
using Enums;
using Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    public abstract class PieceBase
    {
        protected Texture2D _chessPieces;
        protected Rectangle _position;
        protected Rectangle _pieceSprite;
        protected ColorsEnum _color;
        protected PiecesEnum _pieceType;
        protected const int UNIT = 45;
        protected bool isFirstMove = true;
        protected bool _didMove = false;
        protected int _id;

        public Rectangle Position { get { return _position; } }
        public bool OnLeft { get { return _position.Left == UNIT; }}
        public bool OnRight { get { return _position.Right == 9*UNIT; }}
        public bool OnBottom { get { return _position.Bottom == 9*UNIT; }}
        public bool OnTop { get { return _position.Top == UNIT; }}
        public bool IsWhite { get { return _color == ColorsEnum.White; }}
        public bool DidMove { get { return _didMove; } }
        public int ID { get { return _id; } }
        public bool FirstMove { get { return isFirstMove; } }

        public PieceBase(ColorsEnum color, PiecesEnum piece, Texture2D chessPieces, Rectangle position)
        {
            this.Load(color, piece, chessPieces, position);
        }

        public PieceBase(PieceBase pieceToCopy, Rectangle newPosition)
        {
            this.Load(pieceToCopy._color, pieceToCopy._pieceType, pieceToCopy._chessPieces, newPosition, pieceToCopy.ID);
            this.isFirstMove = pieceToCopy.isFirstMove;
            this._didMove = pieceToCopy._didMove;
        }

        public void Load(ColorsEnum color, PiecesEnum piece, Texture2D chessPieces, Rectangle position, int id = -1)
        {
            this._id = id == -1 ? PieceManager.Instance.GenerateID() : id;
            this._color = color;
            this._pieceType = piece;
            this._chessPieces = chessPieces;
            this._position = position;
            this._pieceSprite = new Rectangle((int)_pieceType * UNIT, (int)_color * UNIT, UNIT, UNIT);
        }

        public void Update(InputStateManager inputStateManager)
        {

        }

        public virtual void Move(Rectangle targetRectangle)
        {
            this._didMove = !(targetRectangle == this._position);
            this.isFirstMove = this.isFirstMove && !this._didMove;
            this._position = targetRectangle;
        }

        public void Deselect()
        {
            this._didMove = false;
        }

        public bool IsClicked(InputStateManager inputStateManager)
        {
            return  inputStateManager.CurrentMouseState.X >= this._position.X && 
                    inputStateManager.CurrentMouseState.X <= this._position.X + UNIT &&
                    inputStateManager.CurrentMouseState.Y >= this._position.Y &&
                    inputStateManager.CurrentMouseState.Y <= this._position.Y + UNIT;
        }

        public virtual bool Collision(PieceBase targetPiece)
        {
            return  this._position.X == targetPiece.Position.X &&
                    this._position.Y == targetPiece.Position.Y;
        }

        public bool Collision(Rectangle targetRectangle)
        {
            return  this._position.X == targetRectangle.X &&
                    this._position.Y == targetRectangle.Y;
        }

        public void Draw(SpriteBatch spriteBatch, float order = 0)
        {
            spriteBatch.Draw(_chessPieces, _position, _pieceSprite, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.8f + order);
        }

        protected IEnumerable<Rectangle> DiagonalMoves(IEnumerable<PieceBase> teamPieces, IEnumerable<PieceBase> enemyPieces)
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

        protected IEnumerable<Rectangle> StraightLineMoves(IEnumerable<PieceBase> teamPieces, IEnumerable<PieceBase> enemyPieces)
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

        public abstract IEnumerable<Rectangle> DisplayMoves(IEnumerable<PieceBase> teamPieces, IEnumerable<PieceBase> enemyPieces);
    }
}