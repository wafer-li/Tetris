using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTetris.Model
{
    /// <summary>
    /// The Part class represent the smallest piece of the blocks
    /// And each cell in the Grid is a Part.
    /// </summary>
    public class Part
    {
        #region Fields
        private int m_posx;
        private int m_posy;
        private Block m_parentBlock;
        #endregion

        #region Properties
        public Block ParentBlock
        {
            get { return m_parentBlock; }
        }

        //The public properties for PosX/PosY will return the absolute position in the Grid (based on the values of the parent block).
        public int PosX
        {
            get { return ParentBlock.PosX + m_posx; }
        }

        public int PosY
        {
            get { return ParentBlock.PosY + m_posy; }
        }
        #endregion

        #region Contructors
        public Part(Block parentBlock, int posx, int posy)
        {
            m_parentBlock = parentBlock;
            m_posx = posx;
            m_posy = posy;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determine if the part can move to the new position x/y
        /// </summary>
        /// <param name="x">The new x value.</param>
        /// <param name="x">The new y value.</param>
        /// <returns>Returns if the move will be allowed.</returns>
        public bool CanMove(int x, int y)
        {

            //Check for the new position to be in the borders of the Grid
            if (x < 0 || x > 11 || y > 19)
                return false;

            //Get a part which is currently in the new location and not a part of the same block
            if (ParentBlock.Grid.Where(p => p.PosX == x && p.PosY == y && p.ParentBlock != ParentBlock).Count() == 0)
                return true;


            return false;
        }

        /// <summary>
        /// Rearrange the part in its block.
        /// </summary>
        /// <param name="x">The parts new x value inside its blocks subgrid (4x4).</param>
        /// <param name="y">The parts new x value inside its blocks subgrid (4x4).</param>
        public void RearrangePart(int x, int y)
        {
            //Check for the new values to be valid (0-3)
            if (!((x >= 0 && x < 4) && (y >= 0 && y < 4)))
                return;

            //Set the new part position
            m_posx = x;
            m_posy = y;
        }
        #endregion
    }
}
