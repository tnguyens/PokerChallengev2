using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokerApp.Models
{
    public class PLAYER
    {
        public string Player1Card1 { get; set; }
        public List<CARD> Cards { get; set; }
        public int HighestCard { get; }
        public int HighestRuleCard { get; }

        //Player hand validation
        public bool isValid()
        {
            bool result = true;
            //check for ... card, user has not pick a card
            if (Cards.Contains(new CARD(0)))
            {
                result = false;
            }
            else
            {
                //Check for repeating card on the player
                for (int i = 0; i < Cards.Count - 1; i++)
                {
                    for (int j = i + 1; j < Cards.Count; j++)
                    {
                        if (Cards[i].Index == Cards[j].Index)
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result; 
        }

        //get sum of all cards
        public int GetCardSum (){
            //return this.Cards.Select(x => x.Rank).ToList().Sum();
            int sum = 0;
            foreach (var card in Cards)
            {
                sum += (card.Rank * card.Rank);
            }
            return sum;
        }

        //sorting the player card bases on the suit, heart to spade
        private void sortBySuit()
        {
            int i, j, min;
            //selection sorting in descending order
            for (i = 0; i < Cards.Count; i++)
            {
                min = i;

                for (j = i + 1; j < Cards.Count; j++)
                {
                    if (Cards[j].Suit < Cards[min].Suit)
                    {
                        min = j;
                    }
                }
                CARD temp = Cards[i];
                Cards[i] = Cards[min];
                Cards[min] = temp;
            }
        }

        //sort player cards by rank/face. 2 to Ace
        private void sortByRank()
        {
            int i, j, min;
            //selection sorting in ascending order
            for (i = 0; i < Cards.Count; i++)
            {
                min = i;

                for (j = i + 1; j < Cards.Count; j++)
                {
                    if (Cards[j].Rank > Cards[min].Rank)
                    {
                        min = j;
                    }
                }
                CARD temp = Cards[i];
                Cards[i] = Cards[min];
                Cards[min] = temp;
            }
        }

        //finding the highest car in the player cards
        public int getHighCard()
        {
            sortByRank();
            return Cards[4].Rank;
        }

        //Check to see if player has 1 pair
        public bool isOnePair()
        {            
            //making sure player does not have 4ofakind, fullhouse, three of a kind or 2pairs
            if (isFourOfaKind() || isFullHouse() || isThreeOfaKind() || isTwoPair())
            {
                return false;
            }

            sortByRank();
            //XXABC
            bool case1 = Cards[0].Rank == Cards[1].Rank;
            //AXXBC
            bool case2 = Cards[1].Rank == Cards[2].Rank;
            //ABXXC
            bool case3 = Cards[2].Rank == Cards[3].Rank;
            //ABCXX
            bool case4 = Cards[3].Rank == Cards[4].Rank;

            return (case1 || case2 || case3 || case4);
        }

        //checking player hands has 2 pairs
        public bool isTwoPair()
        {            
            //making sure player doesnot have 4ofakind, fullhouse or 3ofakind
            if (isFourOfaKind() || isFullHouse() || isThreeOfaKind())
            {
                return false;
            }

            sortByRank();
            //XXYYX
            bool case1 = Cards[0].Rank == Cards[1].Rank
                    && Cards[2].Rank == Cards[3].Rank;
            //XXYZZ
            bool case2 = Cards[0].Rank == Cards[1].Rank
                    && Cards[3].Rank == Cards[4].Rank;
            //XYYZZ
            bool case3 = Cards[1].Rank == Cards[2].Rank
                    && Cards[3].Rank == Cards[4].Rank;

            return (case1 || case2 || case3);
        }

        //checking player hand for 3 of a kind
        public bool isThreeOfaKind()
        {
            //maing sure the player doesnot have 4ofakind or fullhouse
            if (isFourOfaKind() || isFullHouse())
            {
                return false;
            }

            sortByRank();
            //XXXYZ
            bool case1 = Cards[0].Rank == Cards[1].Rank
                    && Cards[1].Rank == Cards[2].Rank;
            //XYYYZ
            bool case2 = Cards[1].Rank == Cards[2].Rank
                    && Cards[2].Rank == Cards[3].Rank;
            //XYZZZ
            bool case3 = Cards[2].Rank == Cards[3].Rank
                    && Cards[3].Rank == Cards[4].Rank;

            return (case1 || case2 || case3);
        }

        public bool isStraight()
        {
            sortByRank();
            int ranking = 0;
            int i;
            //2345A or TJQKA
            if (Cards[4].Rank == 0)
            {   
                //case where 2345A
                bool a = Cards[0].Rank == 12
                        && Cards[1].Rank == 11
                        && Cards[2].Rank == 10
                        && Cards[3].Rank == 9;
                //case where TJQKA
                bool b = Cards[0].Rank == 4
                        && Cards[1].Rank == 3
                        && Cards[2].Rank == 2
                        && Cards[3].Rank == 1;
                return (a || b);
            }
            //other cases cards are continuos of each other
            else
            {
                ranking = Cards[0].Rank - 1;
                for (i = 1; i < 5; i++)
                {
                    if (Cards[i].Rank != ranking)
                        return false;
                    ranking--;
                }

                return true;
            }
        }

        //check for player hand is a flush, all cards has a same suit
        public bool isFlush()
        {
            sortBySuit();
            //AAAAA
            return (Cards[0].Suit == Cards[4].Suit);
        }

        //check for player hand is a fullhouse
        public bool isFullHouse()
        {
            sortByRank();
            //XXXYY
            bool case1 = (Cards[0].Rank == Cards[1].Rank)
                    && (Cards[1].Rank == Cards[2].Rank)
                    && (Cards[3].Rank == Cards[4].Rank);
            //XXYYY
            bool case2 = (Cards[0].Rank == Cards[1].Rank)
                    && (Cards[2].Rank == Cards[3].Rank)
                    && (Cards[3].Rank == Cards[4].Rank);

            return (case1 || case2);
        }

        //checking player hand has 4 of a kind
        public bool isFourOfaKind()
        {
            sortByRank();
            //XXXXY
            bool case1 = (Cards[0].Rank == Cards[1].Rank)
                    && (Cards[1].Rank == Cards[2].Rank)
                    && (Cards[2].Rank == Cards[3].Rank);
            //XYYYY
            bool case2 = (Cards[1].Rank == Cards[2].Rank)
                    && (Cards[2].Rank == Cards[3].Rank)
                    && (Cards[3].Rank == Cards[4].Rank);

            return (case1 || case2);
        }

        //checking player hand is a straight flush
        public bool isStraightFlush()
        {
            //happens only when player has a straight and that straight is a flush
            return isStraight() && isFlush();
        }

        //checking the player hand is a royalflush
        public bool isRoyalFlush()
        {   
            //happens only when the player is a straight flush and that straightflush has the high card that is an Ace
            return isStraightFlush() && getHighCard() == 0;
        }

    }
}