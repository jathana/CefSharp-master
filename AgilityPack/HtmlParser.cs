using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgilityPack
{
   public class HtmlParser
   {
      HtmlAgilityPack.HtmlDocument _Doc;
      string _DecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

      public HtmlParser()
      {
         _Doc = new HtmlAgilityPack.HtmlDocument();
      }

      public List<BetRecordDOExtended> Parse(string html)
      {
         List<BetRecordDOExtended> retval = new List<BetRecordDOExtended>();
         _Doc.LoadHtml(html);

         retval = ParseInternal();

         return retval;
      }

      public List<BetRecordDOExtended> ParseFile(string path)
      {
         List<BetRecordDOExtended> retval = new List<BetRecordDOExtended>();

         _Doc.Load(path);

         retval = ParseInternal();

         return retval;
      }

      /// <summary>
      /// Assumes doc is loaded.
      /// </summary>
      /// <returns></returns>
      private List<BetRecordDOExtended> ParseInternal()
      {
         List<BetRecordDOExtended> retval = new List<BetRecordDOExtended>();

         //tmlNodeCollection nodes = doc.DocumentNode
         //                     .SelectNodes("//table[@id='MyTable']//tr");

         var events = _Doc.DocumentNode.SelectNodes("//div[@class='event']");

         foreach (var ev in events)
         {
            BetRecordDOExtended rec = new BetRecordDOExtended();

            // load event info
            ParseEventInfo(ev, rec);
            // Game , Bet Start
            ParseGameAndBetStart(ev, rec);
            // period
            ParsePeriod(ev, rec);
            // competition
            ParseCompetition(ev, rec);

            // home
            ParseHomeTeam(ev, rec);

            // away
            ParseAwayTeam(ev, rec);

            // score home
            ParseScoreHome(ev, rec);

            // score away
            ParseScoreAway(ev, rec);

            // score match result odds
            ParseMatchResultOdds(ev, rec);

            // over under
            ParseOverUnder(ev, rec, "Over", "0.5", "FT");
            ParseOverUnder(ev, rec, "Under", "0.5", "FT");

            ParseOverUnder(ev, rec, "Over", "1.5", "FT");
            ParseOverUnder(ev, rec, "Under", "1.5", "FT");

            ParseOverUnder(ev, rec, "Over", "2.5", "FT");
            ParseOverUnder(ev, rec, "Under", "2.5", "FT");

            ParseOverUnder(ev, rec, "Over", "3.5", "FT");
            ParseOverUnder(ev, rec, "Under", "3.5", "FT");

            ParseOverUnder(ev, rec, "Over", "4.5", "FT");
            ParseOverUnder(ev, rec, "Under", "4.5", "FT");

            retval.Add(rec);
         }
         return retval;
      }

      private void ParseEventInfo(HtmlNode eventNode, BetRecordDOExtended rec)
      {
         bool parsed = false;
         // extract event information
         var eventinfo = eventNode.SelectSingleNode(".//div[@class='event-info']");
         if (eventinfo != null)
         {
            rec.EventId = eventinfo.Attributes["behavior.showscoreboard.event"].Value.Trim();
            rec.BetRadarId = eventinfo.Attributes["behavior.showscoreboard.idfosbevent"].Value.Trim();
            rec.IsAwaitingStart = Boolean.Parse(eventinfo.Attributes["behavior.showscoreboard.isawaitingstart"].Value.Trim());
            rec.Sport = eventinfo.Attributes["behavior.showscoreboard.sport"].Value.Trim();
            parsed = true;
         }
         
         if(!parsed)
         {
            rec.ParseInfo.Add(new ParseInfo()
            {
               ErrorType=ParseErrorType.Error,
               Field="EventInfo",
               Message= "element div[@class='event-info'] not found."
            });
         }
      }



      private void ParsePeriod(HtmlNode eventNode, BetRecordDOExtended rec)
      {
         bool parsed = false;
         // period
         var period = eventNode.SelectSingleNode(".//div[@class='period']//span[@class='live-counter']");
         if (period != null)
         {
            // get sub node 
            rec.Time = period.InnerText;
            parsed = true;
         }
         else
         {
            period = eventNode.SelectSingleNode(".//div[@class='period']");
            if (period != null)
            {
               // get sub node HT
               rec.Time = period.InnerText.Trim();
               parsed = true;
            }
         }

         if(!parsed)
         {
            rec.ParseInfo.Add(new ParseInfo()
            {
               ErrorType = ParseErrorType.Error,
               Field = "Period",
               Message = "Period not found."
            });
         }

      }

      private void ParseCompetition(HtmlNode eventNode, BetRecordDOExtended rec)
      {
         bool parsed = false;
         // comp
         var comp = eventNode.SelectSingleNode(".//td[@class='event-competition']/span");
         if (comp != null)
         {
            // get sub node 
            rec.Competition = comp.InnerText.Trim();
            parsed = true;
         }

         if(!parsed)
         {
            rec.ParseInfo.Add(new ParseInfo()
            {
               ErrorType = ParseErrorType.Error,
               Field = "Competition",
               Message = "Competition not found."
            });
         }
      }

      private void ParseHomeTeam(HtmlNode eventNode, BetRecordDOExtended rec)
      {
         // team
         var team = eventNode.SelectSingleNode(".//td[@class='name home']/span");
         if (team != null)
         {
            // get sub node 
            rec.HomeTeam = team.InnerText.Trim();
         }
         else
         {
            rec.ParseInfo.Add(new ParseInfo()
            {
               ErrorType = ParseErrorType.Error,
               Field = "HomeTeam",
               Message = "HomeTeam not found."
            });
         }
      }

      private void ParseAwayTeam(HtmlNode eventNode, BetRecordDOExtended rec)
      {
         // team
         var team = eventNode.SelectSingleNode(".//td[@class='name away']/span");
         if (team != null)
         {
            // get sub node 
            rec.AwayTeam = team.InnerText.Trim();
         }
         else
         {
            rec.ParseInfo.Add(new ParseInfo()
            {
               ErrorType = ParseErrorType.Error,
               Field = "AwayTeam",
               Message = "AwayTeam not found."
            });
         }
      }

      private void ParseScoreHome(HtmlNode eventNode, BetRecordDOExtended rec)
      {
         bool parsed = false;
         // team
         var score = eventNode.SelectSingleNode(".//td[@class='score home']");
         if (score != null)
         {
            // get sub node 
            int goals = 0;
            if (int.TryParse(score.InnerText, out goals))
            {
               rec.HomeGoalsFT = goals;
               parsed = true;
            }
         }
         if(!parsed)
         {
            rec.HomeGoalsFT = -1;
            rec.ParseInfo.Add(new ParseInfo()
            {
               ErrorType = ParseErrorType.Error,
               Field = "ScoreHome",
               Message = "ScoreHome not found."
            });
         }
      }

      private void ParseScoreAway(HtmlNode eventNode, BetRecordDOExtended rec)
      {
         bool parsed = false;
         // team
         var score = eventNode.SelectSingleNode(".//td[@class='score away']");
         if (score != null)
         {
            int goals = 0;
            if (int.TryParse(score.InnerText, out goals))
            {
               rec.AwayGoalsFT = goals;
               parsed = true;
            }
         }
         if(!parsed)
         {
            rec.AwayGoalsFT = -1;
            rec.ParseInfo.Add(new ParseInfo()
            {
               ErrorType = ParseErrorType.Error,
               Field = "AwayHome",
               Message = "AwayHome not found."
            });
         }
      }

      private void ParseGameAndBetStart(HtmlNode eventNode, BetRecordDOExtended rec)
      {

         bool parsed = false;
         // home
         var node = eventNode.SelectSingleNode(".//input[@type='checkbox' and @behavior.selectionwithbetdatesclick.hadvalue='H' and @behavior.selectionwithbetdatesclick.marketname='Match Result']");
         if (node==null)
            node = eventNode.SelectSingleNode(".//input[@type='checkbox' and @behavior.selectionwithbetdatesclick.hadvalue='H' and @behavior.selectionwithbetdatesclick.marketname='Match winner']");
         if (node != null)
         {
            DateTime startDT = DateTime.Parse("1753-01-01");
            //behavior.selectionwithbetdatesclick.tsstart="2017-10-15T11:15:00" 
            if (!DateTime.TryParse(node.Attributes["behavior.selectionwithbetdatesclick.tsstart"].Value.Trim(), out rec.GameStart)) rec.GameStart = startDT;
            //behavior.selectionwithbetdatesclick.tsbetstart="2017-10-15T11:15:00" 
            if (!DateTime.TryParse(node.Attributes["behavior.selectionwithbetdatesclick.tsbetstart"].Value.Trim(), out rec.BetStart)) rec.BetStart = startDT;

            if (rec.GameStart < startDT) rec.GameStart = startDT;
            if (rec.BetStart < startDT) rec.BetStart = startDT;
            parsed = true;
         }

         if (!parsed)
         {
            rec.ParseInfo.Add(new ParseInfo()
            {
               ErrorType = ParseErrorType.Error,
               Field = "Game Or Bet Start",
               Message = "Game Or Bet Start not parsed."
            });
         }
      }


      private void ParseMatchResultOdds(HtmlNode eventNode, BetRecordDOExtended rec)
      {

         bool parsed = false;
         // home
         var homeOdd = eventNode.SelectSingleNode(".//input[@type='checkbox' and @behavior.selectionwithbetdatesclick.hadvalue='H' and @behavior.selectionwithbetdatesclick.marketname='Match Result' ]");
         if(homeOdd==null)
            homeOdd = eventNode.SelectSingleNode(".//input[@type='checkbox' and @behavior.selectionwithbetdatesclick.hadvalue='H' and @behavior.selectionwithbetdatesclick.marketname='Match winner' ]");
         rec.HomeOddFT = -1;
         if (homeOdd != null)
         {
            //get next sibling <span>
            var span = homeOdd.NextSibling;
            if (span != null)
            {
               // get sub node 
               double odd = -1;
               if (double.TryParse(span.InnerText.Replace(",", _DecimalSeparator), out odd))
               {
                  rec.HomeOddFT = odd;
               }
               parsed = true;
            }
         }
         if(!parsed)
         {
            rec.ParseInfo.Add(new ParseInfo()
            {
               ErrorType = ParseErrorType.Error,
               Field = "HomeOdd",
               Message = "HomeOdd not parsed."
            });
         }
         
         // draw
         var drawOdd = eventNode.SelectSingleNode(".//input[@type='checkbox' and @behavior.selectionwithbetdatesclick.hadvalue='D' and @behavior.selectionwithbetdatesclick.marketname='Match Result' ]");
         if(drawOdd==null)
            drawOdd = eventNode.SelectSingleNode(".//input[@type='checkbox' and @behavior.selectionwithbetdatesclick.hadvalue='D' and @behavior.selectionwithbetdatesclick.marketname='Match winner' ]");

         rec.DrawOddFT = -1;
         if (drawOdd != null)
         {
            //get next sibling <span>
            var span = drawOdd.NextSibling;
            if (span != null)
            {
               // get sub node 
               double odd = 0;
               if (double.TryParse(span.InnerText.Replace(",", _DecimalSeparator), out odd))
               {
                  rec.DrawOddFT = odd;
               }
               parsed = true;
            }
         }
         if (!parsed)
         {
            rec.ParseInfo.Add(new ParseInfo()
            {
               ErrorType = ParseErrorType.Error,
               Field = "DrawOdd",
               Message = "DrawOdd not parsed."
            });
         }
         // away
         var awayOdd = eventNode.SelectSingleNode(".//input[@type='checkbox' and @behavior.selectionwithbetdatesclick.hadvalue='A' and @behavior.selectionwithbetdatesclick.marketname='Match Result' ]");
         if(awayOdd==null)
            awayOdd = eventNode.SelectSingleNode(".//input[@type='checkbox' and @behavior.selectionwithbetdatesclick.hadvalue='A' and @behavior.selectionwithbetdatesclick.marketname='Match winner' ]");
         rec.AwayOddFT = -1;
         if (awayOdd != null)
         {
            //get next sibling <span>
            var span = awayOdd.NextSibling;
            if (span != null)
            {
               // get sub node 
               double odd = 0;
               if (double.TryParse(span.InnerText.Replace(",", _DecimalSeparator), out odd))
               {
                  rec.AwayOddFT = odd;
               }
               parsed = true;
            }
         }
         if (!parsed)
         {
            rec.AwayOddFT = -1;
            rec.ParseInfo.Add(new ParseInfo()
            {
               ErrorType = ParseErrorType.Error,
               Field = "AwayOdd",
               Message = "AwayOdd not parsed."
            });
         }

      }

      // <td class="price"><input type="checkbox" ... behavior.selectionwithbetdatesclick.marketname="Total Goals Over / Under" ... behavior.selectionwithbetdatesclick.selectionname="Over" ... behavior.selectionwithbetdatesclick.handicap="0.5"><span>2,15</span>
      /// <summary>
      /// 
      /// </summary>
      /// <param name="eventNode"></param>
      /// <param name="rec"></param>
      /// <param name="betType">Over or Under</param>
      /// <param name="value">0.5, 1.5 etc</param>
      /// <param name="period">FT|HT</param>
      private void ParseOverUnder(HtmlNode eventNode, BetRecordDOExtended rec, string betType, string overUnderValue, string period)
      {
         bool parsed = false;
         // home
         var oddNode = eventNode.SelectSingleNode($".//input[@type='checkbox' and @behavior.selectionwithbetdatesclick.marketname='Total Goals Over / Under' and @behavior.selectionwithbetdatesclick.selectionname='{betType}' and @behavior.selectionwithbetdatesclick.handicap='{overUnderValue}']");
         if(oddNode==null)
            oddNode = eventNode.SelectSingleNode($".//input[@type='checkbox' and @behavior.selectionwithbetdatesclick.marketname='Total Goals Under/Over' and @behavior.selectionwithbetdatesclick.selectionname='{betType}' and @behavior.selectionwithbetdatesclick.handicap='{overUnderValue}']");
         if (oddNode != null)
         {
            //get next sibling <span>
            var span = oddNode.NextSibling;
            if (span != null)
            {
               parsed = UpdateOverUnder(rec, betType, overUnderValue, period, span.InnerText);
            }

            
         }

         if (!parsed)
         {

            rec.ParseInfo.Add(new ParseInfo()
            {
               ErrorType = ParseErrorType.Warning,
               Field = $"{betType} {overUnderValue}",
               Message = $"{betType} {overUnderValue} not parsed."
            });
         }
      }


      private bool UpdateOverUnder(BetRecordDOExtended rec, string betType, string overUnderValue, string period, string parsedOdd)
      {
         double odd = -1;
         bool parsed = double.TryParse(parsedOdd.Trim().Replace(",", _DecimalSeparator), out odd);
       
         if (betType.Equals("Over") && overUnderValue.Equals("0.5") && period.Equals("FT")) rec.Over05FT = odd;
         if (betType.Equals("Under") && overUnderValue.Equals("0.5") && period.Equals("FT")) rec.Under05FT = odd;

         if (betType.Equals("Over") && overUnderValue.Equals("1.5") && period.Equals("FT")) rec.Over15FT = odd;
         if (betType.Equals("Under") && overUnderValue.Equals("1.5") && period.Equals("FT")) rec.Under15FT = odd;

         if (betType.Equals("Over") && overUnderValue.Equals("2.5") && period.Equals("FT")) rec.Over25FT = odd;
         if (betType.Equals("Under") && overUnderValue.Equals("2.5") && period.Equals("FT")) rec.Under25FT = odd;

         if (betType.Equals("Over") && overUnderValue.Equals("3.5") && period.Equals("FT")) rec.Over35FT = odd;
         if (betType.Equals("Under") && overUnderValue.Equals("3.5") && period.Equals("FT")) rec.Under35FT = odd;

         if (betType.Equals("Over") && overUnderValue.Equals("4.5") && period.Equals("FT")) rec.Over45FT = odd;
         if (betType.Equals("Under") && overUnderValue.Equals("4.5") && period.Equals("FT")) rec.Under45FT = odd;

         return parsed;
      }

   }


}
