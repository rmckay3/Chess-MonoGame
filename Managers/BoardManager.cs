using System.Collections.Generic;
using System.Linq;
using Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using Sprites;

namespace Managers
{
    public class BoardManager
    {
        public const int UNIT = 45;
        private Rectangle _backgroundRectangle;
        private Texture2D _selectedTexture;
        private Texture2D _lastMovedTexture;
        private Texture2D _dangerTexture;
        private Texture2D _background;
        private Texture2D _chessPieces;
        private List<PieceBase> _whitePieces;
        private List<PieceBase> _whitePiecesTaken;
        private List<PieceBase> _blackPieces;
        private List<PieceBase> _blackPiecesTaken;
        private PieceBase _selectedPiece;
        private PieceBase _pieceToHighlight;
        private IEnumerable<Rectangle> _selectedPieceMoves;

        public BoardManager()
        {

        }

        public void Load(GraphicsDevice graphicsDevice, Texture2D[] textureArray)
        {
            this._backgroundRectangle = new Rectangle(UNIT, UNIT, 8*UNIT, 8*UNIT);
            this._background = textureArray[0];
            this._chessPieces = textureArray[1];
            this._selectedTexture = new Texture2D(graphicsDevice, UNIT, UNIT);
            Color color = new(Color.Tan, 0.85f);
            Color[] colors = Enumerable.Repeat(color, UNIT*UNIT).ToArray();
            this._selectedTexture.SetData(colors);
            this._lastMovedTexture = new Texture2D(graphicsDevice, UNIT, UNIT);
            color = new(Color.SpringGreen, 0.6f);
            colors = Enumerable.Repeat(color, UNIT*UNIT).ToArray();
            this._lastMovedTexture.SetData(colors);
            this._dangerTexture = new Texture2D(graphicsDevice, UNIT, UNIT);
            color = new(Color.OrangeRed, 0.85f);
            colors = Enumerable.Repeat(color, UNIT*UNIT).ToArray();
            this._dangerTexture.SetData(colors);


            this._blackPieces = new List<PieceBase>
            {
                new Rook(ColorsEnum.Black,this._chessPieces, new Rectangle(UNIT, UNIT, UNIT, UNIT)),
                new Knight(ColorsEnum.Black, this._chessPieces, new Rectangle(2*UNIT, UNIT, UNIT, UNIT)),
                new Bishop(ColorsEnum.Black, this._chessPieces, new Rectangle(3*UNIT, UNIT, UNIT, UNIT)),
                new Queen(ColorsEnum.Black, this._chessPieces, new Rectangle(4*UNIT, UNIT, UNIT, UNIT)),
                new King(ColorsEnum.Black, this._chessPieces, new Rectangle(5*UNIT, UNIT, UNIT, UNIT)),
                new Bishop(ColorsEnum.Black, this._chessPieces, new Rectangle(6*UNIT, UNIT, UNIT, UNIT)),
                new Knight(ColorsEnum.Black, this._chessPieces, new Rectangle(7*UNIT, UNIT, UNIT, UNIT)),
                new Rook(ColorsEnum.Black, this._chessPieces, new Rectangle(8*UNIT, UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.Black, this._chessPieces, new Rectangle(1*UNIT, 2*UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.Black, this._chessPieces, new Rectangle(2*UNIT, 2*UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.Black, this._chessPieces, new Rectangle(3*UNIT, 2*UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.Black, this._chessPieces, new Rectangle(4*UNIT, 2*UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.Black, this._chessPieces, new Rectangle(5*UNIT, 2*UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.Black, this._chessPieces, new Rectangle(6*UNIT, 2*UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.Black, this._chessPieces, new Rectangle(7*UNIT, 2*UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.Black, this._chessPieces, new Rectangle(8*UNIT, 2*UNIT, UNIT, UNIT)),
            };

            this._whitePieces = new List<PieceBase>
            {
                new Rook(ColorsEnum.White, this._chessPieces, new Rectangle(1*UNIT, 8*UNIT, UNIT, UNIT)),
                new Knight(ColorsEnum.White, this._chessPieces, new Rectangle(2*UNIT, 8*UNIT, UNIT, UNIT)),
                new Bishop(ColorsEnum.White, this._chessPieces, new Rectangle(3*UNIT, 8*UNIT, UNIT, UNIT)),
                new Queen(ColorsEnum.White, this._chessPieces, new Rectangle(4*UNIT, 8*UNIT, UNIT, UNIT)),
                new King(ColorsEnum.White, this._chessPieces, new Rectangle(5*UNIT, 8*UNIT, UNIT, UNIT)),
                new Bishop(ColorsEnum.White, this._chessPieces, new Rectangle(6*UNIT, 8*UNIT, UNIT, UNIT)),
                new Knight(ColorsEnum.White, this._chessPieces, new Rectangle(7*UNIT, 8*UNIT, UNIT, UNIT)),
                new Rook(ColorsEnum.White, this._chessPieces, new Rectangle(8*UNIT, 8*UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.White, this._chessPieces, new Rectangle(1*UNIT, 7*UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.White, this._chessPieces, new Rectangle(2*UNIT, 7*UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.White, this._chessPieces, new Rectangle(3*UNIT, 7*UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.White, this._chessPieces, new Rectangle(4*UNIT, 7*UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.White, this._chessPieces, new Rectangle(5*UNIT, 7*UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.White, this._chessPieces, new Rectangle(6*UNIT, 7*UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.White, this._chessPieces, new Rectangle(7*UNIT, 7*UNIT, UNIT, UNIT)),
                new Pawn(ColorsEnum.White, this._chessPieces, new Rectangle(8*UNIT, 7*UNIT, UNIT, UNIT))
            };

            this._blackPiecesTaken = [];
            this._whitePiecesTaken = [];
        }

        public void Update(InputStateManager inputStateManager, TurnManager turnManager, StateManager stateManager)
        {
            if (inputStateManager.LeftClickEvent)
            {
                if (this._selectedPiece == null) 
                {
                    SelectPiece(inputStateManager, turnManager, stateManager);
                }
                else 
                {
                    MovePiece(inputStateManager, turnManager, stateManager);
                }
            }

            this._pieceToHighlight = null;

            if (stateManager.GameState == GameStateEnum.WhiteInCheck)
            {
                this._pieceToHighlight = this._whitePieces.Where(w => w.GetType() == typeof(King)).FirstOrDefault();
            }
            else if (stateManager.GameState == GameStateEnum.BlackInCheck)
            {
                this._pieceToHighlight = this._blackPieces.Where(w => w.GetType() == typeof(King)).FirstOrDefault();

            }
            
            if (stateManager.GameState == GameStateEnum.WhiteWin || stateManager.GameState == GameStateEnum.BlackWin)
            {

            }

        }

        private void SelectPiece(InputStateManager inputStateManager, TurnManager turnManager, StateManager stateManager)
        {
            List<PieceBase> teamPieces = turnManager.Turn == ColorsEnum.White ? this._whitePieces : this._blackPieces;
            List<PieceBase> enemyPieces = turnManager.Turn == ColorsEnum.White ? this._blackPieces : this._whitePieces;

            foreach (var piece in teamPieces)
            {
                if (piece.IsClicked(inputStateManager))
                {
                    this._selectedPiece = piece;
                    this._selectedPieceMoves = stateManager.CheckPlayerMoves(piece, teamPieces, enemyPieces);
                    this._selectedPieceMoves = this._selectedPieceMoves.Where(w => !teamPieces.Where(x => x.Collision(w)).Any());

                    return;
                }
            }

            this._selectedPiece = null;

        }

        public void MovePiece(InputStateManager inputStateManager, TurnManager turnManager, StateManager stateManager)
        {
            var oldPosition = this._selectedPiece.Position;

            foreach(Rectangle rectangle in this._selectedPieceMoves)
            {
                if (
                    inputStateManager.CurrentMouseState.X >= rectangle.X && 
                    inputStateManager.CurrentMouseState.X <= rectangle.X + UNIT &&
                    inputStateManager.CurrentMouseState.Y >= rectangle.Y &&
                    inputStateManager.CurrentMouseState.Y <= rectangle.Y + UNIT
                )
                {
                    this._selectedPiece.Move(rectangle);
                    var pieceToTake = this._selectedPiece.IsWhite ? 
                                        this._blackPieces.Where(w => w.Collision(this._selectedPiece)) : 
                                        this._whitePieces.Where(w => w.Collision(this._selectedPiece));

                    TakePiece(pieceToTake.FirstOrDefault());
                    if (this._selectedPiece.GetType() == typeof(King) && ((King)this._selectedPiece).Castle) 
                        CastleKing();
                    break;
                }
            }

            if (this._selectedPiece.DidMove) 
            {
                stateManager.CheckGameState(
                    turnManager.Turn == ColorsEnum.White ? this._whitePieces : this._blackPieces,
                    turnManager.Turn == ColorsEnum.White ? this._blackPieces : this._whitePieces
                );
                turnManager.Update(_selectedPiece);
            }
            this._selectedPiece.Deselect();
            this._selectedPiece = null;

        }

        public void TakePiece(PieceBase pieceToTake)
        {
            if (pieceToTake == null) return;

            if (pieceToTake.IsWhite)
            {
                this._whitePieces.Remove(pieceToTake);
                pieceToTake.Move( 
                    new Rectangle(
                        0,
                        this._whitePiecesTaken.Select(s => s.Position.Y).DefaultIfEmpty(-15).Max() + 15,
                        UNIT,
                        UNIT
                    )
                );
                this._whitePiecesTaken.Add(pieceToTake);
            }
            else 
            {
                this._blackPieces.Remove(pieceToTake);
                pieceToTake.Move(
                    new Rectangle(
                        9*UNIT,
                        this._blackPiecesTaken.Select(s => s.Position.Y).DefaultIfEmpty(9*UNIT + 15).Max() - 15,
                        UNIT,
                        UNIT
                    )
                );
                this._blackPiecesTaken.Add(pieceToTake);
            }
        }

        public void CastleKing()
        {
            King king = (King)this._selectedPiece;
            IEnumerable<PieceBase> kingsPieces = king.IsWhite ? this._whitePieces : this._blackPieces;
            if (!king.DidMove) return; // If the King didn't move. Do nothing

            // Find the Rook the King moved next to
            PieceBase rook = null;
            if (king.Position.X == 2*UNIT) rook = kingsPieces.Where(w => w.Position.X == UNIT).FirstOrDefault();
            if (king.Position.X == 7*UNIT) rook = kingsPieces.Where(w => w.Position.X == 8*UNIT).FirstOrDefault();

            if (rook == null) return;

            rook.Move(new Rectangle((2 * king.Position.X) - rook.Position.X, rook.Position.Y, UNIT, UNIT));
        }

        public void Draw(SpriteBatch spriteBatch, TurnManager turnManager)
        {

            spriteBatch.Draw(this._background, this._backgroundRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);

            if (this._selectedPiece != null)
            {
                spriteBatch.Draw(this._selectedTexture, this._selectedPiece.Position, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.8f);
                foreach (Rectangle move in this._selectedPieceMoves)
                {
                    spriteBatch.Draw(this._selectedTexture, move, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.8f);
                }
            }

            if (turnManager.LastPieceMoved != null)
            {
                spriteBatch.Draw(this._lastMovedTexture, turnManager.LastPieceMoved.Position, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.9f);
            }
            if (this._pieceToHighlight != null)
            {
                spriteBatch.Draw(this._dangerTexture, this._pieceToHighlight.Position, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.9f);
            }

            foreach (PieceBase piece in this._blackPieces)
            {
                piece.Draw(spriteBatch);
            }

            foreach (PieceBase piece in this._whitePieces)
            {
                piece.Draw(spriteBatch);
            }

            float order = -0.01f;
            foreach (PieceBase piece in this._blackPiecesTaken)
            {
                piece.Draw(spriteBatch, order);
                order -= 0.01f;
            }

            order = -0.01f;
            foreach (PieceBase piece in this._whitePiecesTaken)
            {
                piece.Draw(spriteBatch, order);
                order -= 0.01f;
            }

        }
    }
}