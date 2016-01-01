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
        public ActionResult Index()
        {
            Session.Clear();
            return View();
        }

        public ActionResult DetermineWinner(PokerHandFromView pokerHand)
        {
            //Player 1 setup
            var player1card1 = new CARD(Int32.Parse(pokerHand.Player1Card1));
            var player1card2 = new CARD(Int32.Parse(pokerHand.Player1Card2));
            var player1card3 = new CARD(Int32.Parse(pokerHand.Player1Card3));
            var player1card4 = new CARD(Int32.Parse(pokerHand.Player1Card4));
            var player1card5 = new CARD(Int32.Parse(pokerHand.Player1Card5));

            var player1 = new PLAYER
            {
                Cards = new List<CARD> { player1card1, player1card2, player1card3, player1card4, player1card5 }
            };
            
            //player 2 setup
            var player2card1 = new CARD(Int32.Parse(pokerHand.Player2Card1));
            var player2card2 = new CARD(Int32.Parse(pokerHand.Player2Card2));
            var player2card3 = new CARD(Int32.Parse(pokerHand.Player2Card3));
            var player2card4 = new CARD(Int32.Parse(pokerHand.Player2Card4));
            var player2card5 = new CARD(Int32.Parse(pokerHand.Player2Card5));
            var player2 = new PLAYER
            {
                Cards = new List<CARD> { player2card1, player2card2, player2card3, player2card4, player2card5 }
            };

            //return variable
            var winner = string.Empty;
            var errorMessage = string.Empty;

            if(Session["win1s"] == null)
            {
                Session["win1s"] = (int) 0;
            }
            if (Session["win2s"] == null)
            {
                Session["win2s"] = (int) 0;
            }
            var player1Wins = (int)Session["win1s"];
            var player2Wins = (int)Session["win2s"];
                        
            //validation of cards
            //each player
            bool testValue1 = arePlayersOk(player1, player2);
            bool testValue2 = player1.isValid();
            bool testValue3 = player2.isValid();

            if (player1.isValid() == false || player2.isValid() == false)
            {
                winner = "Undetermined";
                errorMessage = "Invalid hand(s)";
            }
            //both player
            else if (arePlayersOk(player1, player2) == false)
            {
                errorMessage = "Player 1 and Player 2 have same card(s).";
                winner = "Undetermined";
            }
            //no error
            else {
                var player1CardPoint = this.totalPoint(player1);
                var player2CardPoint = this.totalPoint(player2);

                int pointDiff = player1CardPoint - player2CardPoint;
                if (pointDiff > 0)
                {
                    winner = "Player 1";
                    errorMessage = "";
                    player1Wins++;
                }
                else if (pointDiff < 0)
                {
                    winner = "Player 2";
                    errorMessage = "";
                    player2Wins++;
                }
                else
                {
                    //for both hands do not have anything
                    if (player1CardPoint == 0)
                    {
                        int highCard1 = player1.getHighCardIndex();
                        int highCard2 = player2.getHighCardIndex();
                        if (highCard1 > highCard2)
                        {
                            winner = "Player 2";
                            errorMessage = "";
                            player2Wins++;
                        }
                        else
                        {
                            winner = "Player 1";
                            errorMessage = "";
                            player1Wins++;
                        }
                    }
                    //for both hands may have a same thing
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
                            }
                            else
                            {
                                winner = "Player 2";
                                errorMessage = "";
                                player2Wins++;
                            }
                        }
                        else
                        {
                            winner = "Tie";
                            errorMessage = "";
                        }
                    }
                }
            }

            Session["win1s"] = (int)player1Wins;
            Session["win2s"] = (int)player2Wins;

            var viewModel = new PokerHandFromController()
            {
                Winner = winner,
                Player1Wins = player1Wins,
                Player2Wins = player2Wins,
                Error = errorMessage

            };

            return PartialView("Index", viewModel);
        }

        //calculate points for player
        private int totalPoint(PLAYER playerHand)
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

        //validation of both hands
        private bool arePlayersOk(PLAYER a, PLAYER b)
        {
            foreach(CARD singleCardA in a.Cards)
            {
                foreach(CARD singleCardB in b.Cards)
                {
                    if (singleCardA.Index == singleCardB.Index)
                    {
                        return false;
                    }     
                }
            }
            return true;
        }
    }
}