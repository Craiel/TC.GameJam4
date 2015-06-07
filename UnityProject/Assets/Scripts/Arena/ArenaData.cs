namespace Assets.Scripts.Arena
{
    using System.Collections.Generic;
    using System.IO;

    using UnityEngine;

    public class ArenaData
    {
        private const string TagIndestructible = "x";
        private const string TagDestructible = "d";
        private const string TagHalf = "h";
        private const string TagSpawn = "s";

        private int currentLineToProcess;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public ArenaData()
        {
            this.SpawnPoints = new List<Vector3>();
            this.TileIndizes = new List<int>();
            this.Positions = new List<Vector3>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public IList<Vector3> SpawnPoints { get; private set; }
        public IList<int> TileIndizes { get; private set; } 
        public IList<Vector3> Positions { get; private set; }

        public Vector2 Dimensions { get; private set; }

        public bool IsValid { get; private set; }

        public string Name { get; set; }

        public void InitFromText(string fileName)
        {
            StreamReader streamReader = new StreamReader(Application.streamingAssetsPath + string.Format(StaticSettings.MapFileFilter, fileName));

            this.currentLineToProcess = 0;
            using (streamReader)
            {
                string line;
                do
                {
                    line = streamReader.ReadLine();
                    this.ProcessLine(line);
                }
                while (line != null);
            }

            this.Validate();
        }

        public void InitFromTexture(Texture2D texture, IList<Color> tileColorKeys)
        {
            int currentIndex = 0;
            Color[] colors = texture.GetPixels();
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    int tileIndex = tileColorKeys.IndexOf(colors[currentIndex]);
                    Vector3 position = this.GetTilePosition(x, y);

                    this.TileIndizes.Add(tileIndex);
                    this.Positions.Add(position);

                    currentIndex++;
                }
            }

            //TODO: Recognize spawns

            this.Validate();
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void Validate()
        {
            if (this.Positions.Count % 2 != 0)
            {
                Debug.LogWarning("Map has invalid Position data " + this.Name);
                return;
            }

            int dimension = this.Positions.Count / 2;
            this.Dimensions = new Vector2(dimension, dimension);

            if (this.SpawnPoints.Count <= 0)
            {
                Debug.LogWarning("Map has no Spawn points: " + this.Name);
                return;
            }

            if (this.Positions.Count != this.TileIndizes.Count)
            {
                Debug.LogWarning("Map has non-matching index data: " + this.Name);
                return;
            }

            this.IsValid = true;
        }

        private Vector3 GetTilePosition(int x, int y)
        {
            return new Vector3(-5.25f + y * 0.35f, 5.25f - x * 0.35f, 0f);
        }

        private void ProcessLine(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                for (int j = 0; j < line.Length; ++j)
                {
                    string character = line.Substring(j, 1);
                    int tileIndex = 0;
                    Vector3 position = this.GetTilePosition(j, this.currentLineToProcess);
                    if (character == TagIndestructible)
                    {
                        tileIndex = 1;
                    }
                    else if (character == TagDestructible)
                    {
                        tileIndex = 2;
                    }
                    else if (character == TagHalf)
                    {
                        tileIndex = 3;
                    }
                    else if (character == TagSpawn)
                    {
                        tileIndex = 4;
                        this.SpawnPoints.Add(position);
                    }

                    this.TileIndizes.Add(tileIndex);
                    this.Positions.Add(position);
                }

                this.currentLineToProcess++;
            }
        }
    }
}
