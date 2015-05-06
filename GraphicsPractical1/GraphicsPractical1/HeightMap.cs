using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace GraphicsPractical1
{
    class HeightMap
    {
        private int width;
        private int height;
        private byte[,] heightData;

        public int Width
        {
            get { return this.width; }
        }

        public int Height
        {
            get { return this.height; }
        }

        public HeightMap(Texture2D heightMap)
        {
            this.width = heightMap.Width;
            this.height = heightMap.Height;
            this.loadHeightData(heightMap);
        }

        //Set the height data according to the provided heigthMap texture
        private void loadHeightData(Texture2D heightMap)
        {
            this.heightData = new byte[this.width, this.height];
            Color[] colorData = new Color[this.width * this.height];
            heightMap.GetData(colorData);
            for (int x = 0; x < this.width; x++)
                for (int y = 0; y < this.height; y++)
                    this.heightData[x, y] = colorData[x + y * this.width].R;
        }

        public byte this[int x, int y]
        {
            get { return this.heightData[x, y]; }
            set { this.heightData[x, y] = value; }
        }
    }
}
