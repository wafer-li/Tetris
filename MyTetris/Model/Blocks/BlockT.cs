using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyTetris.Model.Blocks
{
    public class BlockT : Block
    {
        public BlockT(List<Part> grid)
            : base(grid)
        {
            this.Color = Colors.CadetBlue;
            Parts = new List<Part>() { new Part(this, 0, 0), new Part(this, 1, 0), new Part(this, 2, 0), new Part(this, 1, 1) };
        }

        public override bool Rotate()
        {
            return base.Rotate();
        }
    }
}
