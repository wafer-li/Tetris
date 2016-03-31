/// By Ali Abdulhussein
/// 07 sep. 2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace UtilityLib
{
    public static class HelpMethod
    {
        /// <summary>
        /// Parts rotation, use base Titris block rotaion algorithem
        /// By multiplay original point by rotation matrix Vector
        /// [0  -1]
        /// [1   0]
        /// after subtract original point from basepoint (Center point).
        /// </summary>
        /// <param name="CurrientPoint">Original point OR Current Point</param>
        /// <param name="CenterPoint">Rotation Base point OR Center Point</param>
        /// <returns></returns>
        public static Point PartsRotaiter(Point CurrientPoint, Point CenterPoint)
        {
            //Rotation Matrix
            int[,] R = new int[,]
            {
                {0,-1},
                {1,0},
            };
            ///V:Currient Point
            int vx, vy;
            vx = (int)CurrientPoint.X;
            vy = (int)CurrientPoint.Y;

            ///P:Center Point
            int vrx, vry;
            vrx = vx - (int)CenterPoint.X;
            vry = vy - (int)CenterPoint.Y;

            //Multiplay R (Roation Vactor) By 0 base Point
            int vtx, vty;
            vtx = (R[0, 0] * vrx) + (R[0, 1] * vry);
            vty = (R[1, 0] * vrx) + (R[1, 1] * vry);

            //Return point to Original Center
            int vix, viy;
            vix = (int)CenterPoint.X + vtx;
            viy = (int)CenterPoint.Y + vty;

            return new Point(vix, viy);
        }
    }
}
