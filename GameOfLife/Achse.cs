using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameOfLife
{
    /// <summary>
    /// 
    /// </summary>
    public class Achse : DrawableGameComponent
    {
        private const int AchseWidth = 400;
        private const int AchseHeight = 400;
        private const int UpdateInterval = 25;
        private readonly bool[,,] _zellen;
        private readonly Rectangle[,] _rechts;
        private readonly SpriteBatch _spriteBatch;
        private Texture2D _lebendeZelleTexture;
        private readonly Input _input;
        private int _millisecondsSinceUpdated;
        private int _currentIndex;
        private int _futureIndex = 1;
        private int _oldWidth;
        private int _oldHeight;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="input"></param>
        public Achse(Game game, SpriteBatch spriteBatch, Input input) : base(game)
        {

            _zellen = new bool[2, AchseWidth + 2, AchseHeight + 2];
            _rechts = new Rectangle[AchseWidth, AchseHeight];

            this._spriteBatch = spriteBatch;
            this._input = input;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            _lebendeZelleTexture = new Texture2D(GraphicsDevice, 1, 1);
            var colors = new Color[1];
            colors[0] = Color.White;

            _lebendeZelleTexture.SetData(0, new Rectangle(0, 0, 1, 1), colors, 0, 1);

            base.LoadContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (_input.SpaceTrigger)
                CreateRandomCells(20);
            
            if (_input.ResetTrigger)
                ResetCells();

            if (_input.FigureTrigger)
                FigureCells();

            _millisecondsSinceUpdated += gameTime.ElapsedGameTime.Milliseconds;

            if (_millisecondsSinceUpdated >= UpdateInterval)
            {
                _millisecondsSinceUpdated = 0;
                for (var y = 1; y < AchseHeight + 1; y++)
                {
                    for (var x = 1; x < AchseWidth + 1; x++)
                    {
                        var neighboursCount = 0;

                        //link
                        if (_zellen[_currentIndex, x - 1, y])
                            neighboursCount++;

                        //rechts
                        if (_zellen[_currentIndex, x + 1, y])
                            neighboursCount++;

                        //oben
                        if (_zellen[_currentIndex, x, y - 1])
                            neighboursCount++;

                        //unten
                        if (_zellen[_currentIndex, x, y + 1])
                            neighboursCount++;

                        //links oben
                        if (_zellen[_currentIndex, x - 1, y - 1])
                            neighboursCount++;

                        //links unten
                        if (_zellen[_currentIndex, x - 1, y + 1])
                            neighboursCount++;

                        //rechts unten
                        if (_zellen[_currentIndex, x + 1, y + 1])
                            neighboursCount++;

                        //rechts oben
                        if (_zellen[_currentIndex, x + 1, y - 1])
                            neighboursCount++;

                        if (neighboursCount == 3)
                            _zellen[_futureIndex, x, y] = true;
                        
                        else if (neighboursCount < 2)
                            _zellen[_futureIndex, x, y] = false;
                        else if (neighboursCount > 3)
                            _zellen[_futureIndex, x, y] = false;
                        else if (neighboursCount == 2 && _zellen[_currentIndex, x, y])
                            _zellen[_futureIndex, x, y] = true;
                    }
                }

                if (_currentIndex == 0)
                {
                    _currentIndex = 1;
                    _futureIndex = 0;
                }
                else
                {
                    _currentIndex = 0;
                    _futureIndex = 1;
                }
            }

            var width = GraphicsDevice.Viewport.Width;
            var heigth = GraphicsDevice.Viewport.Height;

            if (_oldWidth != width || _oldHeight != heigth)
            {
                var zellenWidth = width / AchseWidth;
                var zellenHeigth = heigth / AchseHeight;

                var zellenSize = Math.Min(zellenWidth, zellenHeigth);

                var offSetX = (width - (zellenSize * AchseWidth)) / 2;
                var offSetY = (heigth - (zellenSize * AchseHeight)) / 2;

                for (var y = 0; y < AchseHeight; y++)
                {
                    for (var x = 0; x < AchseWidth; x++)
                    {
                        _rechts[x, y] = new Rectangle(offSetX + x * zellenSize, offSetY + y * zellenSize, zellenSize, zellenSize);
                    }
                }
                _oldHeight = heigth;
                _oldWidth = width;
            }

            base.Update(gameTime);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            for (var y = 1; y < AchseHeight + 1; y++)
            {
                for (var x = 1; x < AchseWidth + 1; x++) 
                {
                    if (_zellen[_currentIndex, x, y])
                        _spriteBatch.Draw(_lebendeZelleTexture, _rechts[x - 1, y - 1], Color.White);
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CreateRandomCells(int probability)
        {
            var r = new Random();

            for (var x = 1; x < AchseWidth + 1; x++)
            {
                for (var y = 1; y < AchseHeight + 1; y++)
                {
                    if (r.Next(0, probability) == 0)
                        _zellen[_currentIndex, x, y] = true;
                    else
                        _zellen[_currentIndex, x, y] = false;
                }
            }
        }

        private void ResetCells()
        {
            for (var y = 1; y < AchseHeight + 1; y++)
            {
                for (var x = 1; x < AchseWidth + 1; x++)
                {
                    _zellen[_currentIndex, x, y] = false;
                }
            }
        }

        private void FigureCells()
        {
            for (var y = 1; y < AchseHeight + 1; y++)
            {
                for (var x = 1; x < AchseWidth + 1; x++)
                {
                    if(x == y || AchseWidth - x == y )
                        _zellen[_currentIndex, x, y] = true;
                }
            }
        }
    }
}