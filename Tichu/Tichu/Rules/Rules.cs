using System;
using System.Collections.Generic;
using System.Text;

namespace Tichu
{
    interface Rules 
    {
        
        /// <summary>
        ///  Checks if Card is a valid combo
        /// </summary>
        /// <returns>true or false</returns>
        bool check(Card[] cards, PlayMode mode);
        

    }
}
