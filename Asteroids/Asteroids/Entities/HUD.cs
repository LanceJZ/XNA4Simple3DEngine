#region Using
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Asteroids.Engine;
#endregion

namespace Asteroids.Entities
{
    public class HUD : DrawableGameComponent
    {
        #region Fields
        private SpriteFont spriteFont;
        private SpriteBatch spriteBatch;
        private Vector2 scorePosition;
        private Vector2 highScorePosition;
        private Vector2 hitsPosition;
        private Vector2 gameOverPosition;
        private Vector2 copyRightPosition;
        private int playerScore;
        private int playerHighScore;
        private int playerHitsLeft;
        private string playerScoreText;
        private string playerHighScoreText;
        private string playerHitsLeftText;
        private string gameOverText;
        private string copyRightText;
        private bool gameOver;
        #endregion
        #region Properties
        public int PlayerHitsLeft
        {
            set
            {
                playerHitsLeft = value;
                playerHitsLeftText = "Hits Available: " + playerHitsLeft;
            }

            get
            {
                return playerHitsLeft;
            }
        }

        public int PlayerScore
        {
            set
            {
                playerScore = value;
                playerScoreText = "Score: " + playerScore;
            }

            get
            {
                return playerScore;
            }
        }

        public bool GameOver
        {
            set
            {
                gameOver = value;
            }
        }

        public SpriteBatch SpriteBatchReferance
        {
            set
            {
                spriteBatch = value;
            }
        }
        #endregion
        #region Constructor
        public HUD(Game game)
            : base(game)
        {
            game.Components.Add(this);
        }
        #endregion
        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();
            gameOverText = "Game Over";
            playerHighScoreText = "High Score: 0";
            playerHitsLeftText = "Hits Available: 00 ";
            playerScoreText = "Score: 0";
            copyRightText = "Asteroids Copyright Atari inc 1979";
            scorePosition = new Vector2(20, 10);
            highScorePosition = new Vector2(Services.WindowWidth / 2 - playerHighScoreText.Length * 5, 10);
            hitsPosition = new Vector2(Services.WindowWidth - playerHitsLeftText.Length * 10, 10);
            gameOverPosition = new Vector2(Services.WindowWidth / 2 - gameOverText.Length * 5, Services.WindowHeight / 2); //60
            copyRightPosition = new Vector2(Services.WindowWidth / 2 - copyRightText.Length * 5, Services.WindowHeight - 24); //175
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, playerScoreText, scorePosition, Color.White);
            spriteBatch.DrawString(spriteFont, playerHighScoreText, highScorePosition, Color.White);
            spriteBatch.DrawString(spriteFont, playerHitsLeftText, hitsPosition, Color.White);
            spriteBatch.DrawString(spriteFont, copyRightText, copyRightPosition, Color.DarkSlateGray);

            if (gameOver)
            {
                spriteBatch.DrawString(spriteFont, gameOverText, gameOverPosition, Color.WhiteSmoke);
            }

            spriteBatch.End();
        }

        public void UpdateHighScore()
        {
            if (playerScore > playerHighScore)
            {
                playerHighScore = playerScore;
                playerHighScoreText = "High Score: " + playerHighScore;
            }
                
        }
        #endregion
        #region Protected Methods
        protected override void LoadContent()
        {
            base.LoadContent();
            spriteFont = Game.Content.Load<SpriteFont>(@"Fonts\Pericles");
        }
        #endregion
    }
}
