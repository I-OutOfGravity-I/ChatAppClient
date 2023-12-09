using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Objects
{
    public class ImageInfo
    {
        private int _height = 200;

        public int Width { get; set; }
        public int Height => _height;
        public double AspectRatio => Width > 0 ? (double)Width / _height : 0;

        public void SetHeight(int newHeight)
        {
            _height = newHeight;
        }
    }
}
