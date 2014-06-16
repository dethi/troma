using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameEngine;

namespace Troma
{
    class UpdateAnimation : UpdateableEntityComponent
    {
        #region Fields

        AnimationPlayer animationPlayer;

        #endregion field

        public UpdateAnimation(Entity aParent)
            : base(aParent)
        {
            Name = "UpdateAnimation";
            _requiredComponents.Add("AnimatedModel3D");
        }

        public override void Start()
        {
            base.Start();
            animationPlayer = Entity.GetComponent<AnimatedModel3D>().animationPlayer;
        }

        public override void Update(GameTime gameTime)
        {
            if (animationPlayer.CurrentClip != null)
            {
                // Matrix.CreateScale(-1, 1, 1) permet de corriger l'effet miroir sorti de nul part...
                // Il faut cependant modifier dans les propriété de l'objet "Swap winding order" a true sinon la texture apparait a l'interieur
                // Commentaire a laisser au cas ou :p
                animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.CreateScale(-1, 1, 1) * Entity.GetComponent<Transform>().World);
            }
        }
    }
}
