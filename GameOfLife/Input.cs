using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameOfLife
{
    /// <summary>
    /// The Input class.
    /// </summary>  
    public class Input : GameComponent
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public Input(Game game) : base(game)
        {

        }

        private KeyboardState _oldState;
         
        /// <summary>
        /// 
        /// </summary>
        public bool SpaceTrigger { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ResetTrigger { get; private set; }

        public bool FigureTrigger { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var currentState = Keyboard.GetState();

            SpaceTrigger = false;
            ResetTrigger = false;
            FigureTrigger = false;

            if (currentState.IsKeyDown(Keys.Space) && !_oldState.IsKeyDown(Keys.Space))
                SpaceTrigger = true;
             
            if (currentState.IsKeyDown(Keys.R) && !_oldState.IsKeyDown(Keys.R))
                ResetTrigger = true;

            if (currentState.IsKeyDown(Keys.F) && !_oldState.IsKeyDown(Keys.F))
                FigureTrigger = true;

            _oldState = currentState;

        }

    }
}