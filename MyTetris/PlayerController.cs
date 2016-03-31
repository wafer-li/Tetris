///PLayer Controller
///By Ali Abdulhussein
///On 11 Sep. 2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyTetris
{
    /// <summary>
    /// This class represent the player controller by inheriting GameMgr Class.
    /// </summary>
    class PlayerController:GameMgr
    {
        private GameMgr m_playerGameMgr;

        public GameMgr PlayerGameMgr
        {
            set { m_playerGameMgr = value; }
        }
        
        public PlayerController(Grid objPlayerGamePanel):base(objPlayerGamePanel)
        {
        }
    }
}
