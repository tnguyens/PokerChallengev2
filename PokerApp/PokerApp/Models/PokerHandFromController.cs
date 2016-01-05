using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokerApp.Models
{
    public class PokerHandFromController
    {
        public string Winner { get; set; }
        public int Player1Wins { get; set; }
        public int Player2Wins { get; set; }
        public string Error { get; set; }
        public string WinFactor { get; set; }

        public string Player1Card1 { get; set; }
        public string Player1Card2 { get; set; }
        public string Player1Card3 { get; set; }
        public string Player1Card4 { get; set; }
        public string Player1Card5 { get; set; }
        public string Player2Card1 { get; set; }
        public string Player2Card2 { get; set; }
        public string Player2Card3 { get; set; }
        public string Player2Card4 { get; set; }
        public string Player2Card5 { get; set; }
    }
}