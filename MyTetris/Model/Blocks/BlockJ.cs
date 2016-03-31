using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyTetris.Model.Blocks
{
    public class BlockJ : Block
    {
        public BlockJ(List<Part> grid)
            : base(grid)
        {
            this.Color = Colors.Gray;
            Parts = new List<Part>() { new Part(this, 1, 0), new Part(this, 1, 1), new Part(this, 1, 2), new Part(this, 0, 2) };
        }
        public override bool Rotate()
        {
            return base.Rotate();
        }
    }
}
