using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Arrow
{
    public class MouseManager
    {
        private MouseState currentState;
        private MouseState lastState;

        public float deltaX { get; private set; }
        public float deltaY { get; private set; }

        public bool isMove { get; set; }

        public MouseManager()
        {
            currentState = Mouse.GetState();
            lastState = currentState;
        }

        public void Update(GameTime gameTime)
        {
            currentState = Mouse.GetState();

            deltaX = lastState.X - currentState.X;
            deltaY = lastState.Y - currentState.Y;

            isMove = (deltaX == 0 && deltaY == 0) ? false : true;

            lastState = currentState;
        }
    }
}