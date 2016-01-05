using PokerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PokerApp.Controllers
{
    public class PokerController : Controller
    {
        // GET: Poker
        public ActionResult Index()//PokerHandFromController pokerHand
        {
            //reset score at initial point
            Session.Clear();
            return View();//"Index", pokerHand
        }

        public ActionResult DetermineWinner(PokerHandFromView pokerHand)
        {
            //Player 1 setup
            Card player1card1 = new Card(Int32.Parse(pokerHand.Player1Card1));
            Card player1card2 = new Card(Int32.Parse(pokerHand.Player1Card2));
            Card player1card3 = new Card(Int32.Parse(pokerHand.Player1Card3));
            Card player1card4 = new Card(Int32.Parse(pokerHand.Player1Card4));
            Card player1card5 = new Card(Int32.Parse(pokerHand.Player1Card5));
            Player player1 = new Player
            {
                Cards = new List<Card> { player1card1, player1card2, player1card3, player1card4, player1card5 }
            };
            
            //player 2 setup
            Card player2card1 = new Card(Int32.Parse(pokerHand.Player2Card1));
            Card player2card2 = new Card(Int32.Parse(pokerHand.Player2Card2));
            Card player2card3 = new Card(Int32.Parse(pokerHand.Player2Card3));
            Card player2card4 = new Card(Int32.Parse(pokerHand.Player2Card4));
            Card player2card5 = new Card(Int32.Parse(pokerHand.Player2Card5));
            Player player2 = new Player
            {
                Cards = new List<Card> { player2card1, player2card2, player2card3, player2card4, player2card5 }
            };

            //return variable
            string winner = string.Empty;
            string errorMessage = string.Empty;
            string winnerFactor = string.Empty;
            //setting session values of scores
            if(Session["win1s"] == null)
            {
                Session["win1s"] = (int) 0;
            }
            if (Session["win2s"] == null)
            {
                Session["win2s"] = (int) 0;
            }
            int player1Wins = (int)Session["win1s"];
            int player2Wins = (int)Session["win2s"];
                        
            //validation of cards
            //each player
           // testing purposes variables.
            //bool testValue1 = arePlayersOk(player1, player2);
            bool Player1Valid = player1.isValid();
            bool Player2Valid = player2.isValid();

            if (Player1Valid == false || Player2Valid == false)
            {
                winner = "Undetermined";
                if (!Player1Valid)
                {
                    errorMessage = "Invalid Player 1 Hand";
                }
                else
                {
                    errorMessage = "Invalid Player 2 Hand";
                }
            }
            //both player
            else if (arePlayersOk(player1, player2) == false)
            {
                errorMessage = "Player 1 and Player 2 have same card(s).";
                winner = "Undetermined";
            }
            //no error
            else {
                int player1CardPoint = this.totalPoint(player1);
                int player2CardPoint = this.totalPoint(player2);

                //compare points to determine winner
                int pointDiff = player1CardPoint - player2CardPoint;
                if (pointDiff > 0)
                {
                    winner = "Player 1";
                    errorMessage = "";
                    player1Wins++;
                    winnerFactor = WinningFactor(player1CardPoint);
                }
                else if (pointDiff < 0)
                {
                    winner = "Player 2";
                    errorMessage = "";
                    player2Wins++;
                    winnerFactor = WinningFactor(player2CardPoint);
                }
                else
                {
                    //for both hands do not have anything, points are the same, some pair the high card of each
                    if (player1CardPoint == 0)
                    {
                        int highCard1 = player1.getHighCardIndex();
                        int highCard2 = player2.getHighCardIndex();
                        if (highCard1 > highCard2)
                        {
                            winner = "Player 2";
                            errorMessage = "";
                            player2Wins++;
                            winnerFactor = WinningFactor(player2CardPoint);
                        }
                        else
                        {
                            winner = "Player 1";
                            errorMessage = "";
                            player1Wins++;
                            winnerFactor = WinningFactor(player1CardPoint);
                        }
                    }
                    //for both hands may have a same thing, compare that same hand using the high card of the hand.
                    else {
                        int player1CardSum = player1.HighestRuleCard;// player1.GetCardSum();
                        int player2CardSum = player2.HighestRuleCard;// player2.GetCardSum();
                        if (player1CardSum != player2CardSum)
                        {
                            if (player1CardSum < player2CardSum)
                            {
                                winner = "Player 1";
                                errorMessage = "";
                                player1Wins++;
                                winnerFactor = WinningFactor(player1CardPoint);
                            }
                            else
                            {
                                winner = "Player 2";
                                errorMessage = "";
                                player2Wins++;
                                winnerFactor = WinningFactor(player2CardPoint);
                            }
                        }
                        //only case where game is a tie that is when the players have the same thing, which nerver happens bc of duplication checking
                        else
                        {
                            winner = "Tie";
                            errorMessage = "";
                        }
                    }
                }
            }

            //score keeping setting to the session       
            Session["win1s"] = (int)player1Wins;
            Session["win2s"] = (int)player2Wins;

            //create the postback msg
            PokerHandFromController viewModel = new PokerHandFromController()
            {
                Winner = winner,
                Player1Wins = player1Wins,
                Player2Wins = player2Wins,
                Error = errorMessage,
                WinFactor = winnerFactor,
                Player1Card1 = pokerHand.Player1Card1,
                Player1Card2 = pokerHand.Player1Card2,
                Player1Card3 = pokerHand.Player1Card3,
                Player1Card4 = pokerHand.Player1Card4,
                Player1Card5 = pokerHand.Player1Card5,
                Player2Card1 = pokerHand.Player2Card1,
                Player2Card2 = pokerHand.Player2Card2,
                Player2Card3 = pokerHand.Player2Card3,
                Player2Card4 = pokerHand.Player2Card4,
                Player2Card5 = pokerHand.Player2Card5

            };

            return PartialView("Index", viewModel);
        }

        //calculate points for player
        private int totalPoint(Player playerHand)
        {
            //int sum = 0;
            if (playerHand.isRoyalFlush())
            {
                //sum += 256;
                return 256;
            }
            else if (playerHand.isStraightFlush())
            {
                //sum += 128;
                return 128;
            }
            else if (playerHand.isFourOfaKind())
            {
                //sum += 64;
                return 64;
            }
            else if (playerHand.isFullHouse())
            {
                //sum += 32;
                return 32;
            }
            else if (playerHand.isFlush())
            {
                //sum += 16;
                return 16;
            }
            else if (playerHand.isStraight())
            {
                //sum +=8;
                return 8;
            }
            else if (playerHand.isThreeOfaKind())
            {
                //sum += 4;
                return 4;
            }
            else if (playerHand.isTwoPair())
            {
                //sum +=2;
                return 2;
            }
            else if (playerHand.isOnePair())
            {
                //sum +=1;
                return 1;
            }
            //return sum;
            return 0;
        }

        //validation of both hands, no duplication in both hand
        private bool arePlayersOk(Player a, Player b)
        {
            foreach(Card singleCardA in a.Cards)
            {
                foreach(Card singleCardB in b.Cards)
                {
                    if (singleCardA.Index == singleCardB.Index)
                    {
                        return false;
                    }     
                }
            }
            return true;
        }

        //The function will return what winner has to be a winner.
        private string WinningFactor( int score)
        {
            if (score == 1)
            {
                return "because of One Pair";
            }
            else if (score == 2)
            {
                return "because of Two Pairs";
            }
            else if (score == 4)
            {
                return "because of Three of a Kind";
            }
            else if (score == 8)
            {
                return "because of a Straight";
            }
            else if (score == 16)
            {
                return "because of a Flush";
            }
            else if (score == 32)
            {
                return "because of a Full House";
            }
            else if (score == 64)
            {
                return "because of Four of a Kind";
            }
            else if (score == 128)
            {
                return "because of a Straight Flush";
            }
            else if (score == 256)
            {
                return "because of a Royal Flush";
            }

            return "by High Card";
        }
    }
}