using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyTetris.Model.Blocks;
using System.Windows.Threading;

namespace MyTetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variable
        private GameMgr m_objPlayerOneGameMgr;
        private GameMgr m_objPlayerTwoGameMgr;
        private DispatcherTimer disTimerPlayerOne;
        private DispatcherTimer disTimerPlayerTwo;
        #endregion
        #region Properties
        public GameMgr PlayerOneGameMgr
        {
            get { return m_objPlayerOneGameMgr; }
            set
            {
                if (value != null)
                    m_objPlayerOneGameMgr = value;
            }
        }
        public GameMgr PlayerTwoGameMgr
        {
            get { return m_objPlayerTwoGameMgr; }
            set
            {
                if (value != null)
                    m_objPlayerTwoGameMgr = value;
            }
        }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            InitializedGameControler();

            this.KeyDown += MainWindow_KeyDown;

        }
        private void InitializedGameControler()
        {
            disTimerPlayerOne = new DispatcherTimer();
            disTimerPlayerTwo = new DispatcherTimer();
            //Player One
            PlayerOneGameMgr = new GameMgr(gridPlayerOne);
            PlayerOneGameMgr.GameOver += PlayerOne_GameOver;
            PlayerOneGameMgr.GameStart += PlayerOne_GameStart;

            //Player Two
            PlayerTwoGameMgr = new GameMgr(gridPlayerTwo);
            PlayerTwoGameMgr.GameStart += PlayerTwoGame_GameStart;
            PlayerTwoGameMgr.GameOver += PlayerTwoGameMgr_GameOver;

        }

        #region Player One
        public void PlayerOne_GameStart()
        {
            disTimerPlayerOne.Start();
        }

        public void PlayerOne_GameOver()
        {
            disTimerPlayerOne.Stop();
            disTimerPlayerOne.Tick -= disOneTimer_Tick;

            MessageBox.Show("Player One Game Over");

        }
        private void disOneTimer_Tick(object sender, EventArgs e)
        {
            if (!m_objPlayerOneGameMgr.CurrentBlock.Locked)
            {
                m_objPlayerOneGameMgr.DoDownMoving();
            }
            else
            {
                m_objPlayerOneGameMgr.NewBlock();
            }
        }
        /// <summary>
        /// Player one start new game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemPlayerOneStartNewGame_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame(gridPlayerOne, PlayerOneGameMgr);

            disTimerPlayerOne.Tick += disOneTimer_Tick;
            disTimerPlayerOne.Interval = new TimeSpan(0, 0, 0, 0, 590);
        }

        #endregion

        #region PLayer Two
        void PlayerTwoGameMgr_GameOver()
        {
            MessageBox.Show("Player Two Game Over");
            disTimerPlayerTwo.Stop();
            disTimerPlayerTwo.Tick -= disTwoTimer_Tick;
        }
        private void PlayerTwoGame_GameStart()
        {
            //MessageBox.Show("hello player two");
            disTimerPlayerTwo.Start();
        }
        void disTwoTimer_Tick(object sender, EventArgs e)
        {
            if (!m_objPlayerTwoGameMgr.CurrentBlock.Locked)
            {
                //MessageBox.Show(objGameMgr.CurrentBlock.PosX.ToString());
                //DoDownMoving(objGameMgr);
                m_objPlayerTwoGameMgr.DoDownMoving();
            }
            else
            {
                // stop here to detect all lines
                m_objPlayerTwoGameMgr.TryToCleanRows();
                m_objPlayerTwoGameMgr.NewBlock();
                //dispatcherTimer.Stop();
            }
        }
        private void menuItemPlayerTwoStartNewGame_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame(gridPlayerTwo, PlayerTwoGameMgr);

            disTimerPlayerTwo.Tick += disTwoTimer_Tick;
            disTimerPlayerTwo.Interval = new TimeSpan(0, 0, 0, 0, 600);

            Task task = new Task(StartPlayerTwo_Task);
            task.Start();
            task.Wait();

        }

        #region Task and Async
        /// <summary>
        /// Start New game Player Two using async and task
        /// </summary>
        private async void StartPlayerTwo_Task()
        {
            await Task.Run(() =>
            {
                PlayerTwoGame_GameStart();
            });
        }
        #endregion

        #endregion



        #region shared Event
        private void StartNewGame(Grid gamePanel, GameMgr objPlayer)
        {
            try
            {
                //Start new game
                objPlayer.StartGame();

                ///Initialised Game Panel
                //objPlayer.InitializeGamePanel(gamePanel);


                //Paint Initial Block
                objPlayer.PaintCurrentBlock();

            }
            catch (Exception e)
            {
                
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// The main key event handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    m_objPlayerTwoGameMgr.Rotation();
                    break;
                case Key.Down:
                    ///Using this key for Right game panel
                    //DoDownMoving(objGameMgr);
                    m_objPlayerTwoGameMgr.DoDownMoving();
                    break;
                case Key.Left:
                    ///Using this key for Right game panel
                    //DoLeftMoving(objGameMgr);
                    m_objPlayerTwoGameMgr.DoLeftMoving();
                    break;
                case Key.Right:
                    ///Using this key for Right game panel
                    //DoRightMoving(objGameMgr);
                    m_objPlayerTwoGameMgr.DoRightMoving();
                    break;
                case Key.A:
                    m_objPlayerOneGameMgr.DoLeftMoving();
                    //DoLeftMoving(objGameMgr);
                    break;
                case Key.D:
                    m_objPlayerOneGameMgr.DoRightMoving();
                    //DoRightMoving(objGameMgr);
                    break;
                case Key.S:
                    m_objPlayerOneGameMgr.DoDownMoving();
                    //DoDownMoving(objGameMgr);
                    break;
                case Key.W:
                    m_objPlayerOneGameMgr.Rotation();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Close application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion


    }
}
