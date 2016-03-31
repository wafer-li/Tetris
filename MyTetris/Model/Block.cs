using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using UtilityLib;

namespace MyTetris.Model
{
    public abstract class Block : BindableBase
    {
        #region Fields
        private List<Part> m_parts;
        private int m_posx;
        private int m_posy;
        private Color m_color;
        private bool m_locked;
        private List<Part> m_grid;

        #endregion

        #region Properties
        public List<Part> Parts
        {
            get { return m_parts; }
            set { SetPropertyAndNotifyChanged(ref m_parts, value); }
        }
        public int PosX
        {
            get { return m_posx; }
            set { SetPropertyAndNotifyChanged(ref m_posx, value); }
        }
        public int PosY
        {
            get { return m_posy; }
            set { SetPropertyAndNotifyChanged(ref m_posy, value); }
        }
        public Color Color
        {
            get { return m_color; }
            set { SetPropertyAndNotifyChanged(ref m_color, value); }
        }
        /// <summary>
        /// Represent Grid for game panel
        /// This property return Grid with the new Block position update.
        /// </summary>
        public List<Part> Grid
        {
            get { return m_grid; }
            set { SetPropertyAndNotifyChanged(ref m_grid, value); }
        }
        public bool Locked
        {
            get { return m_locked; }
            set { SetPropertyAndNotifyChanged(ref m_locked, value); }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new random block
        /// </summary>
        /// <param name="grid">List of all Parts</param>
        public Block(List<Part> grid)
        {
            //Default start locations
            //The start location has to be x4 y1 for the center of every block (which is 1,1)
            PosX = 4;
            PosY = -1;

            Grid = grid;
        }
        #endregion

        #region Methods
        public static Block NewBlock(List<Part> grid)
        {
            //Return all classes inheriting from Block class.
            var blockTypes = typeof(Block).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Block))).ToList();

            //Get Rundom type from List
            Type randType = null;
            if (blockTypes.Count > 0)
            {
                var rnd = new Random();
                //Random block type according to random as collection index
                randType = blockTypes[rnd.Next(0, blockTypes.Count)];
            }
            //Return instance of block type
            return (Block)Activator.CreateInstance(randType, grid);
        }

        public bool MoveDown()
        {
            //Check if the block can move down and not on grid button
            if (Locked == false && Parts.Where(p => !p.CanMove(p.PosX + 0, p.PosY + 1)).Count() > 0)
            {
                Locked = true;
                return false;
            }
            //Remove Parts from Grid for this Block
            this.Parts.ForEach(p => Grid.Remove(p));
            this.PosY += 1;
            Grid.AddRange(this.Parts);
            return true;
        }

        public bool MoveLeft()
        {
            //Check if the block can move down and not on grid Left border
            if (Locked == false && Parts.Where(p => !p.CanMove(p.PosX + -1, p.PosY + 0)).Count() > 0)
            {
                //Locked = true;
                return false;
            }
            this.Parts.ForEach(p => Grid.Remove(p));
            this.PosX -= 1;
            Grid.AddRange(this.Parts);
            return true;
        }

        public bool MoveRight()
        {
            //Check if the block can move down and not on grid button
            if (Locked == false && Parts.Where(p => !p.CanMove(p.PosX + 1, p.PosY + 0)).Count() > 0)
            {
                //Locked = true;
                return false;
            }

            this.Parts.ForEach(p => Grid.Remove(p));
            this.PosX += 1;
            Grid.AddRange(this.Parts);
            return true;
        }

        public virtual bool Rotate()
        {
            return Rotation(90);
        }

        private bool Rotation(int degree)
        {
            //Temp array
            int[,] rotArray = new int[4, 2];

            //Set the center for the rotation
            int centerX = 1;
            int centerY = 1;

            #region Calculate the Rotation for every part
            foreach (Part p in Parts)
            {
                #region Test Method From Internet
                //#region Return the coordinates to a 0,0 center
                //int newX = p.PosX - p.ParentBlock.PosX - centerX;
                //int newY = p.PosY - p.ParentBlock.PosY - centerY;
                //#endregion

                ////Calculate new xy value according to degree
                //double degreeToRadians = (Math.PI * degree / 180) * -1;
                //int factor = newX;
                //newX = (int)-Math.Sin(degreeToRadians) * newY;
                //newY = (int)Math.Sin(degreeToRadians) * factor;

                //if (!p.CanMove(PosX + newX + centerX, PosY + newY + centerY))
                //    return false;

                //#region Add the center coordinates again and store the new values in the array to rotate later
                ////The index in the array is obtained through the parts index in its list
                //rotArray[Parts.IndexOf(p), 0] = newX + centerX;
                //rotArray[Parts.IndexOf(p), 1] = newY + centerY;
                //#endregion
                #endregion

                int tmpx = p.PosX - p.ParentBlock.PosX;
                int tmpy = p.PosY - p.ParentBlock.PosY;
                //Return Rotaited Point for each Part.
                Point tmpPoint = UtilityLib.HelpMethod.PartsRotaiter(new Point(tmpx, tmpy), new Point(centerX, centerY));

                if (!p.CanMove(PosX + (int)tmpPoint.X + centerX, PosY + (int)tmpPoint.Y + centerY))
                    return false;
                rotArray[Parts.IndexOf(p), 0] = (int)tmpPoint.X;
                rotArray[Parts.IndexOf(p), 1] = (int)tmpPoint.Y;

            }
            #endregion

            //Perform the rotations after every part was checked
            Parts.ForEach(p => p.RearrangePart(rotArray[Parts.IndexOf(p), 0], rotArray[Parts.IndexOf(p), 1]));
            return true;
        }

        public void DeleteRow(int row)
        {
        }

        #endregion
    }
}
