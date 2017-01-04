using MyTetris.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;

namespace MyTetris
{
    /// <summary>
    /// This class represent the Game panel
    /// Players panel.
    /// </summary>
    /// 
    public delegate void GameEvent();

    public class GameMgr : BindableBase
    {
        #region Event
        public event GameEvent GameOver;
        public event GameEvent GameStart;
        #endregion

        #region Instance variable
        /// <summary>
        /// Game panel Field represent Players Game Grid, in with hold the Blocks
        /// </summary>
        private Grid m_gamePanel;
        /// <summary>
        /// Parts container each block have 3x3 parts.
        /// </summary>
        private Block m_NextBlock;
        private List<Part> m_parentGrid;
        private int m_score = 0;
        #endregion

        #region Properties
        public Grid GamePanel
        {
            get { return m_gamePanel; }
            set
            {
                if (value != null)
                    m_gamePanel = value;
            }
        }

        public List<Part> ParentGrid
        {
            get { return m_parentGrid; }
            set { SetPropertyAndNotifyChanged(ref m_parentGrid, value); }
        }

        public Block NextBlock
        {
            get { return m_NextBlock; }
            set { SetPropertyAndNotifyChanged(ref m_NextBlock, value); }
        }

        public Block CurrentBlock { get; set; }

        #endregion

        #region Constractors
        public GameMgr()
        { }
        public GameMgr(Grid gamePanel):this()
        {
            GamePanel = gamePanel;
            ParentGrid = new List<Part>();
            CurrentBlock = null;
            NextBlock = null;
            //NewBlock();
            //StartGame();
        }
        #endregion

        ///Method done on block object
        #region Methods
        public void StartGame()
        {
            if(GameStart!=null)
            {
                InitializeGamePanel(m_gamePanel);
                GameStart();
                NewBlock();
            }
        }
        public void EndGame()
        {
            if (GameOver != null)
            {
                this.ClearGrid();
                GameOver();
            }
        }
        public void NewBlock()
        {
            NextBlock = Block.NewBlock(ParentGrid);
            //Check if next block confliect with current block
            if (NextBlock.MoveDown())
            {
                CurrentBlock = NextBlock;
            }
            else
            {
                //Fire event End Round
                if (GameOver != null)
                {
                    GameOver();
                }
            }
        }
        public void InitializeGamePanel(Grid gridPanel)
        {
            for (int i = 0; i < gridPanel.RowDefinitions.Count(); i++)
            {
                for (int j = 0; j < gridPanel.ColumnDefinitions.Count(); j++)
                {
                    Label gridCell = new Label();
                    gridCell.Background = Brushes.Transparent;
                    gridCell.BorderThickness = new Thickness(1);

                    gridPanel.Children.Add(gridCell);
                    Grid.SetRow(gridCell, i);
                    Grid.SetColumn(gridCell, j);
                }
            }
        }
        /// <summary>
        /// Paint Block
        /// Create block by change the UI element background color in Grid panel according to selected element from Bock collection.
        /// Where the UI element (Label) tack the color of Block Class.
        /// </summary>
        public void PaintCurrentBlock()
        {
            try
            {
                foreach (Part p in CurrentBlock.Parts)
                {
                    PaintPart(p);
                }
            }
            catch(Exception)
            {
            }
        }

        public void PaintPart(Part p)
        {
            var uiPart = GamePanel.Children.Cast<Control>().Where(e => Grid.GetRow(e) == p.PosY && Grid.GetColumn(e) == p.PosX).Single();
            uiPart.Background = new SolidColorBrush(p.ParentBlock.Color);
        }

        /// <summary>
        /// Remove Block from panel
        /// </summary>
        public void RemoveCurrentBlock()
        {
            try
            {
                foreach (Part p in CurrentBlock.Parts)
                {
                    RemovePart(p);
                }
            }
            catch(Exception)
            {
            }
        }

        /// <summary>
        /// Remove a Part from panel
        /// </summary>
        public void RemovePart(Part p)
        {
            var uiPart = GamePanel.Children.Cast<Control>().Where(e => Grid.GetRow(e) == p.PosY && Grid.GetColumn(e) == p.PosX).Single();
            uiPart.Background = new SolidColorBrush(Colors.Transparent);
        }

        public void MoveBlockDown()
        {
            if (!CurrentBlock.Locked)
                CurrentBlock.MoveDown();
        }
        public void MoveBlockLeft()
        {
            if (!CurrentBlock.Locked)
                CurrentBlock.MoveLeft();
        }
        public void MoveBlockRight()
        {
            if (!CurrentBlock.Locked)
                CurrentBlock.MoveRight();
        }
        public void DoDownMoving()
        {

            //Remove Current Block
            this.RemoveCurrentBlock();

            //Move Block Down
            this.MoveBlockDown();

            //Repaint the block
            this.PaintCurrentBlock();
        }
        public void DoLeftMoving()
        {
            //Remove Current Block
            this.RemoveCurrentBlock();

            //Move Block left
            this.MoveBlockLeft();

            //Repaint the block
            this.PaintCurrentBlock();
        }
        public void DoRightMoving()
        {
            //Remove Current Block
            this.RemoveCurrentBlock();

            //Move Block Right
            this.MoveBlockRight();

            //Repaint the block
            this.PaintCurrentBlock();
        }

        public void Rotation()
        {
            if (!CurrentBlock.Locked)
                DoRotation();
        }
        private void DoRotation()
        {
            //Remove Current Block
            this.RemoveCurrentBlock();

            //Move Block Right
            this.CurrentBlock.Rotate();

            //Repaint the block
            this.PaintCurrentBlock();
        }

        public void ClearGrid()
        {
            var cells = m_gamePanel.Children.Cast<Control>().ToList();
            foreach (var c in cells)
            {
                c.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
        #endregion

        public void TryToCleanRows()
        {
            // iterate over rows and calculate new grid positions
            // query each row from top to bottom
            m_parentGrid.ForEach(p => RemovePart(p));
            for (int i = 0; i < 20; i++)
            {
                var partsInARow = m_parentGrid.Where(p => p.PosY == i);
                // if all columns of a row are filled (12 parts in a row), this row should be cleared
                if (partsInARow.Count() == 12)
                {
                    var tmp_partsInARow = new List<Part>(partsInARow);
                    // remove part from the block, both from Grid, block and screen
                    foreach (var p in tmp_partsInARow)
                    {
                        var prevCount = p.ParentBlock.Parts.Count();
                        //RemovePart(p);
                        p.ParentBlock.Parts.Remove(p);
                        m_parentGrid.Remove(p);
                        
                        Debug.Assert(prevCount != p.ParentBlock.Parts.Count());
                    }
                    // move all block up down for 1 grid, both from Grid and screen
                    foreach (var p in m_parentGrid.Where(p => p.PosY < i))
                    {
                        var prevY = p.PosY;
                        //RemovePart(p);
                        p.MoveDown();
                        //PaintPart(p);
                        Debug.Assert(prevY + 1 == p.PosY);
                    }
                    m_score += 100; // one line generates 100 points.
                    Debug.WriteLine("The score is "+m_score+" now!");
                }
            }

            m_parentGrid.ForEach(p => PaintPart(p));
            foreach (var p in m_parentGrid)
            {
                Debug.Assert(m_parentGrid.Where(q => q.PosX == p.PosX && q.PosY == p.PosY).Count() == 1);
            }
        }
    }
}
