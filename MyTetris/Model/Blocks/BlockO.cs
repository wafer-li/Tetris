using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyTetris.Model.Blocks
{
    public class BlockO : Block
    {
        public BlockO(List<Part> grid)
            : base(grid)
        {
            Color = Colors.MediumOrchid;
            Parts = new List<Part>() { new Part(this, 0,0), new Part(this, 1, 0), new Part(this, 0, 1), new Part(this, 1, 1) };
        }

        /// <summary>
        /// The BlockO cannot rotate at all.
        /// </summary>
        /// <returns></returns>
        public override bool Rotate()
        {
            return false;
        }
    }
}
