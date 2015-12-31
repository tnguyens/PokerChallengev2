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
        //public int HighestCard { get; }
        public int HighestRuleCard { get; set; }

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

        //finding the highest card by rank in the player cards
        public int getHighCard()
        {
            sortByRank();
            return Cards[4].Rank;
        }

        //finding the highest card by index in the player cards
        public int getHighCardIndex()
        {
            sortByRank();
            return Cards[4].Index;
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
            if( Cards[0].Rank == Cards[1].Rank)
            {
                this.HighestRuleCard = Cards[0].Index;
                return true;
            }
            //AXXBC
            else if( Cards[1].Rank == Cards[2].Rank)
            {
                this.HighestRuleCard = Cards[1].Index;
                return true;
            }
            //ABXXC
            else if( Cards[2].Rank == Cards[3].Rank)
            {
                this.HighestRuleCard = Cards[2].Index;
                return true;
            }
            //ABCXX
            else if( Cards[3].Rank == Cards[4].Rank)
            {
                this.HighestRuleCard = Cards[3].Index;
                return true;
            }

            return false;
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
            //XXYYZ
            if( Cards[0].Rank == Cards[1].Rank
                    && Cards[2].Rank == Cards[3].Rank)
            {
                this.HighestRuleCard = Cards[3].Index;
                return true;
            }
            //XXYZZ
            else if( Cards[0].Rank == Cards[1].Rank
                    && Cards[3].Rank == Cards[4].Rank)
            {
                this.HighestRuleCard = Cards[4].Index;
                return true;
            }
            //XYYZZ
            else if( Cards[1].Rank == Cards[2].Rank
                    && Cards[3].Rank == Cards[4].Rank)
            {
                this.HighestRuleCard = Cards[4].Index;
                return true;
            }

            return false;
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
            if( Cards[0].Rank == Cards[1].Rank
                    && Cards[1].Rank == Cards[2].Rank)
            {
                this.HighestRuleCard = Cards[2].Index;
                return true;
            }
            //XYYYZ
            else if( Cards[1].Rank == Cards[2].Rank
                    && Cards[2].Rank == Cards[3].Rank)
            {
                this.HighestRuleCard = Cards[3].Index;
                return true;
            }
            //XYZZZ
            else if( Cards[2].Rank == Cards[3].Rank
                    && Cards[3].Rank == Cards[4].Rank)
            {
                this.HighestRuleCard = Cards[4].Index;
                return true;
            }

            return false;
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
                if( Cards[0].Rank == 12
                        && Cards[1].Rank == 11
                        && Cards[2].Rank == 10
                        && Cards[3].Rank == 9)
                {
                    this.HighestRuleCard = Cards[3].Index;
                    return true;
                }
                //case where TJQKA
                else if( Cards[0].Rank == 4
                        && Cards[1].Rank == 3
                        && Cards[2].Rank == 2
                        && Cards[3].Rank == 1)
                {
                    this.HighestRuleCard = Cards[4].Index;
                    return true;
                }
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
                this.HighestRuleCard = Cards[4].Index;
                return true;
            }
            return false;
        }

        //check for player hand is a flush, all cards has a same suit
        public bool isFlush()
        {
            sortBySuit();
            //AAAAA
            if(Cards[0].Suit == Cards[4].Suit)
            {
                this.HighestRuleCard = Cards[4].Index;
                return true;
            }
            return false;
        }

        //check for player hand is a fullhouse
        public bool isFullHouse()
        {
            sortByRank();
            //XXXYY
            if( (Cards[0].Rank == Cards[1].Rank)
                    && (Cards[1].Rank == Cards[2].Rank)
                    && (Cards[3].Rank == Cards[4].Rank))
            {
                this.HighestRuleCard = Cards[2].Index;
                return true;
            }
            //XXYYY
            else if((Cards[0].Rank == Cards[1].Rank)
                    && (Cards[2].Rank == Cards[3].Rank)
                    && (Cards[3].Rank == Cards[4].Rank))
            {
                this.HighestRuleCard = Cards[4].Index;
                return true;
            }

            return false;
        }

        //checking player hand has 4 of a kind
        public bool isFourOfaKind()
        {
            sortByRank();
            //XXXXY
            if( (Cards[0].Rank == Cards[1].Rank)
                    && (Cards[1].Rank == Cards[2].Rank)
                    && (Cards[2].Rank == Cards[3].Rank))
            {
                this.HighestRuleCard = Cards[3].Index;
                return true;
            }
            //XYYYY
            else if( (Cards[1].Rank == Cards[2].Rank)
                    && (Cards[2].Rank == Cards[3].Rank)
                    && (Cards[3].Rank == Cards[4].Rank))
            {
                this.HighestRuleCard = Cards[4].Index;
                return true;
            }

            return false;
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