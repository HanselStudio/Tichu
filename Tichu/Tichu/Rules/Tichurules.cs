using System;
using System.Collections.Generic;
using System.Text;

namespace Tichu
{
    class Tichurules : Rules
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool check(Card[] cards, PlayMode mode)
        {
            //so bekommt mann denn Playmode
            PlayMode modeOfCards = PlayMode.getPlayMode(cards);

            return false;
        }

    }
}
