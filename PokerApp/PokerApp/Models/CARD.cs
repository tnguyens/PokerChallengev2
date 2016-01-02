using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokerApp.Models
{
    public class Card
    {

        public int Rank { get; }
        public int Suit { get; }
        public int Index { get; set; }

        public Card(int index)
        {
            this.Index = index;
            this.Rank = this.SetRank();
            this.Suit = this.SetSuit();
        }

        //private function to set Rank. whereas Ace =0, king = 1... 2 =12.
        private int SetRank()
        {
            int result = (this.Index - 1) / 4; 

            return result;
        }

        //private function to set suit. whereas heart =0, diamond =1, club =2, and spade =3.
        private int SetSuit()
        {
            return (this.Index-1) % 4;
        }

    }
}