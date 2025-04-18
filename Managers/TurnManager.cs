using Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using Sprites;

namespace Managers
{
    public class TurnManager
    {
        private ColorsEnum _turn = ColorsEnum.White;
        public ColorsEnum Turn { get { return _turn; } }
        private PieceBase _lastPieceMoved;
        public PieceBase LastPieceMoved { get { return _lastPieceMoved; } }
        public TurnManager()
        {

        }

        public void Update(PieceBase movedPiece)
        {
            this._lastPieceMoved = movedPiece;
            PieceManager.Instance.SetEnPassantPiece(movedPiece);
            NextTurn();
        }

        public void NextTurn()
        {
            this._turn = this._turn == ColorsEnum.White ? ColorsEnum.Black : ColorsEnum.White;
        }
    }
}