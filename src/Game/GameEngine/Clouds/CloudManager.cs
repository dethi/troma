using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public enum SkyType
    {
        SpotClouds,
        CloudySky,
        CloudField,
        CloudSplatter
    }

    public class CloudManager
    {
        #region Fields

        private ICamera _camera;

        private ParticleInstancer _clouds;
        private List<ParticleInstance> _whisps;
        private Random _rand;

        private int[] _cloudSprites;

        private List<distData> bbDists;

        #endregion

        #region Initialization

        public CloudManager(SkyType skyType, TerrainInfo terrainInfo, ICamera camera)
        {
            Texture2D texture = FileManager.Load<Texture2D>("sprite_clouds");
            Effect effect = FileManager.Load<Effect>("Effects/Clouds");

            _clouds = new ParticleInstancer(texture, effect);
            _whisps = new List<ParticleInstance>();
            _rand = new Random(DateTime.Now.Millisecond);
            _camera = camera;

            _cloudSprites = new int[] 
            { 
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 
            };

            bbDists = new List<distData>();

            Initialize(skyType, terrainInfo);
        }

        private void Initialize(SkyType skyType, TerrainInfo terrainInfo)
        {
            float x, y, z;
            float d = 1;

            switch (skyType)
            {
                case SkyType.CloudSplatter:
                    float boxSize = terrainInfo.Size.Width;
                    Vector3 flatBase = new Vector3(10, 1, 5);

                    for (int c = 0; c < 170; c++)
                    {
                        d = 0.90f;

                        x = MathHelper.Lerp(-boxSize, 2 * boxSize, (float)_rand.NextDouble());
                        y = MathHelper.Lerp(terrainInfo.Depth + 75, terrainInfo.Depth + 300, 
                            (float)_rand.NextDouble());
                        z = MathHelper.Lerp(-boxSize, 2 * boxSize, (float)_rand.NextDouble());

                        if (y < terrainInfo.Depth + 100)
                            d = .85f;

                        AddCloud(25, new Vector3(x, y, z), 40, flatBase, flatBase * 5, d, 
                            0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
                    }

                    break;

                case SkyType.CloudField:
                    Vector3 cloudDim1 = new Vector3(
                        terrainInfo.Size.Width + 200, 
                        20, 
                        terrainInfo.Size.Height + 200);

                    AddCloud(2000, new Vector3(-100, terrainInfo.Depth + 90, -100), 
                        60, cloudDim1, cloudDim1, .25f, 0, 1, 2, 3, 4);
                    AddCloud(2000, new Vector3(-100, terrainInfo.Depth + 120, -100), 
                        60, cloudDim1, cloudDim1, .5f, 3, 4, 5, 6, 7, 8);
                    AddCloud(2000, new Vector3(-100, terrainInfo.Depth + 150, -100), 
                        60, cloudDim1, cloudDim1, .75f, 7, 8, 9, 10, 11);
                    AddCloud(2000, new Vector3(-100, terrainInfo.Depth + 180, -100), 
                        60, cloudDim1, cloudDim1, 1f, 0, 1, 2, 3, 4, 12, 13, 14, 15);

                    break;

                case SkyType.CloudySky:
                    Vector3 episode1PlayArea = new Vector3(1000, 1000, 1000);

                    // Outer large clouds                    
                    AddCloud(50, Vector3.Zero, 2500, new Vector3(4000, 4000, 4000), 
                        new Vector3(2000, 2000, 2000), .75f, _cloudSprites);

                    // clouds inplay
                    flatBase = new Vector3(50, 5, 25);

                    for (int c = 0; c < 50; c++)
                    {
                        d = 1;

                        x = MathHelper.Lerp(-episode1PlayArea.X, episode1PlayArea.X, 
                            (float)_rand.NextDouble());
                        y = MathHelper.Lerp(-episode1PlayArea.Y, episode1PlayArea.Y, 
                            (float)_rand.NextDouble());
                        z = MathHelper.Lerp(-episode1PlayArea.Z, episode1PlayArea.Z, 
                            (float)_rand.NextDouble());

                        if (y < 200)
                            d = .8f;
                        if (y < 0 && y > -500)
                            d = .75f;
                        if (y < -500)
                            d = .5f;

                        AddCloud(25, new Vector3(x, y, z), 300, flatBase, flatBase * 5, d, 
                            0, 1, 2, 3, 4, 5, 6, 7, 8, 9);

                    }

                    break;

                case SkyType.SpotClouds:
                    // Randomly place some clouds in the scene.
                    flatBase = new Vector3(20, 2, 20);

                    // Cloud are avolume
                    boxSize = 800;
                    for (int c = 0; c < 300; c++)
                    {
                        // Place the cloud randomly in the area.
                        x = MathHelper.Lerp(-boxSize, boxSize, (float)_rand.NextDouble());
                        y = MathHelper.Lerp(0, boxSize / 1, (float)_rand.NextDouble());
                        z = MathHelper.Lerp(-boxSize, boxSize, (float)_rand.NextDouble());

                        AddCloud(25, new Vector3(x, y, z), 64, flatBase, flatBase * 5, .75f, 
                            0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
                    }

                    break;
            }
        }

        #endregion

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            SortClouds();
            Rotate(Vector3.Up, .00015f);
        }

        public void Draw(GameTime gameTime, ICamera camera)
        {
            _clouds.Draw(gameTime, camera);
        }

        #endregion

        private void SortClouds()
        {
            bbDists.Clear();

            for (int p = 0; p < _whisps.Count; p++)
            {
                float dist = (new distData()).Distance(
                    _clouds.InstancesTransformMatrices[_whisps[p]].Translation,
                    _camera.Position);
                bbDists.Add(new distData(_whisps[p], dist));
            }

            bbDists.Sort(new distData());

            // Reorder the matrix list.
            _clouds.InstancesTransformMatrices.Clear();

            for (int p = 0; p < bbDists.Count; p++)
                _clouds.InstancesTransformMatrices.Add(bbDists[p].idx, bbDists[p].idx.World);

            _clouds.CalcVertexBuffer();
        }

        #region Public Methods

        public void AddCloud(int whispCount, Vector3 pos, float size,
            Vector3 min, Vector3 max, float colorMod, params int[] whispRange)
        {
            float scaleMod = Vector3.Distance(-min, max) / 4.5f;
            int i = 0;

            for (int w = 0; w < whispCount; w++)
            {
                float x = MathHelper.Lerp(-min.X, max.X, (float)_rand.NextDouble());
                float y = MathHelper.Lerp(-min.Y, max.Y, (float)_rand.NextDouble());
                float z = MathHelper.Lerp(-min.Z, max.Z, (float)_rand.NextDouble());

                if (i >= whispRange.Length)
                    i = 0;

                _whisps.Add(new ParticleInstance(
                    _clouds,
                    pos + new Vector3(x, y, z),
                    size * Vector3.One,
                    new Vector3(whispRange[i++] / 100f, 1, (_rand.Next(7, 10) / 10f) * colorMod)));
            }
        }

        public void AddCloud(int whispCount, Vector3 pos, float size,
            float radius, float colorMod, params int[] whispRange)
        {
            float scaleMod = Vector3.Distance(pos, pos * radius) / 4.5f;
            int i = 0;

            for (int w = 0; w < whispCount; w++)
            {
                float x = MathHelper.Lerp(-radius, radius, (float)_rand.NextDouble());
                float y = MathHelper.Lerp(-radius, radius, (float)_rand.NextDouble());
                float z = MathHelper.Lerp(-radius, radius, (float)_rand.NextDouble());

                if (i >= whispRange.Length)
                    i = 0;

                _whisps.Add(new ParticleInstance(
                    _clouds,
                    pos + new Vector3(x, y, z),
                    size * Vector3.One,
                    new Vector3(whispRange[i++] / 100f, 1, (_rand.Next(7, 10) / 10f) * colorMod)));
            }
        }

        /// <summary>
        /// Translate object
        /// </summary>
        public void TranslateOO(Vector3 distance)
        {
            _clouds.TranslateOO(distance);
        }

        public void TranslateAA(Vector3 distance)
        {
            _clouds.TranslateAA(distance);
        }

        /// <summary>
        /// Rotate object
        /// </summary>
        public void Rotate(Vector3 axis, float angle)
        {
            _clouds.Rotate(axis, angle);
        }

        #endregion

        #region distDate class

        public class distData : IComparer<distData>
        {
            public float dist;
            public ParticleInstance idx;

            public distData()
            { }

            public distData(ParticleInstance idx, float dist)
            {
                this.idx = idx;
                this.dist = dist;
            }

            public int Compare(distData x, distData y)
            {
                return (int)(y.dist - x.dist);
            }

            public float Distance(Vector3 v1, Vector3 v2)
            {
                Vector3 unit = v2 - v1;
                float distance = unit.Length();

                return distance *= distance;
            }
        }

        #endregion
    }
}
