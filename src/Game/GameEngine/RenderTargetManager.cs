using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class RenderTargetManager
    {
        public static RenderTarget2D[] RenderTarget { get; private set; }

        public static void Initialize()
        {
            RenderTarget = new RenderTarget2D[2];
        }

        public static void InitRenderTargets(int width, int height)
        {
            RenderTarget[0] = new RenderTarget2D(
                GameServices.GraphicsDevice, width, height, false,
                GameServices.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.None);

            RenderTarget[1] = new RenderTarget2D(
                GameServices.GraphicsDevice, width, height, false,
                GameServices.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.None);
        }

        public static void Unload()
        {
            RenderTarget[0].Dispose();
            RenderTarget[0] = null;

            RenderTarget[1].Dispose();
            RenderTarget[1] = null;
        }

        public static void SetRenderTarget(uint? i)
        {
            if (i.HasValue && i < 2)
                GameServices.GraphicsDevice.SetRenderTarget(RenderTarget[i.Value]);
            else
                GameServices.GraphicsDevice.SetRenderTarget(null);
        }
    }
}
