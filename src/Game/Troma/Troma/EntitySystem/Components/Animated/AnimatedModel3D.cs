using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine;
using Microsoft.Xna.Framework.Graphics;

namespace Troma
{
    public class AnimatedModel3D : EntityComponent
    {
        public Model Model;
        public string ModelName;
        public SkinningData skinningData;
        public AnimationPlayer animationPlayer;
        public AnimationClip clip;

        public AnimatedModel3D(Entity aParent, string model)
            : base(aParent)
        {
            Name = "AnimatedModel3D";
            _requiredComponents.Add("Transform");

            Model = FileManager.Load<Model>("Models/" + model);
            ModelName = model;
            skinningData = Model.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException ("This model does not contain a SkinningData tag.");
            
            animationPlayer = new AnimationPlayer(skinningData);

            clip = skinningData.AnimationClips[model +"Action"];
        }

        public void PlayClip(AnimInfo animInfo, int nb_bone)
        {
            animationPlayer.StartClip(clip, animInfo, nb_bone);
        }
    }
}
