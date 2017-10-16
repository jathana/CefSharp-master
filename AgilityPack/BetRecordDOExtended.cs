using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgilityPack
{
   public class BetRecordDOExtended
   {
      // START OF <div class="event-info"
      public string EventId = ""; //behavior.showscoreboard.event="690176.1"
      public string BetRadarId = ""; //behavior.showscoreboard.idfosbevent="4770272"
      public bool IsAwaitingStart = false; // behavior.showscoreboard.isawaitingstart="false"
      public string Sport = ""; // behavior.showscoreboard.sport="FOOTBALL"
      // END OF <div class="event-info"

      // date and bet date
      //behavior.selectionwithbetdatesclick.tsstart="2017-10-15T11:15:00" behavior.selectionwithbetdatesclick.tsbetstart="2017-10-15T11:15:00" 
      public DateTime GameStart=DateTime.Parse("1753-01-01");
      public DateTime BetStart = DateTime.Parse("1753-01-01");

      // <span data-hide-tooltip-if-no-overflow="true" class="competition text-overflow  pointer" title="Czech Republic CFL - IR">Czech Republic CFL - IR</span>
      public string Competition = "";

      // event period
      // <td class="event-period"><div class="period"><span class="live-counter" name="live-counter-687009.1">20:40</span></div></td>
      // <td class="event-period"><div class="period">HT</div></td>
      public string Time = "";

      //<tr>
      //   <td class="name home" colspan="3"><span class="text-overflow" data-hide-tooltip-if-no-overflow="true" title="Davao Aguilas FC">Davao Aguilas FC</span></td>
      //   <td class="score home">1</td>
      //</tr>
      public string HomeTeam = "";
      public int HomeGoalsFT = -1;

      //<tr>
      //   <td class="name away" colspan="3"><span class="text-overflow" data-hide-tooltip-if-no-overflow="true" title="Ilocos United FC">Ilocos United FC</span></td>
      //   <td class="score away">0</td>
      //</tr>
      public string AwayTeam = "";
      public int AwayGoalsFT = -1;


      // <td class="price"><input type="checkbox" ... behavior.selectionwithbetdatesclick.marketname="Match Result" ... behavior.selectionwithbetdatesclick.hadvalue="H"><span>2,15</span>
      public double HomeOddFT = -1;
      // <td class="price"><input type="checkbox" ... behavior.selectionwithbetdatesclick.marketname="Match Result" ... behavior.selectionwithbetdatesclick.hadvalue="D"><span>2,15</span>
      public double DrawOddFT = -1;
      // <td class="price"><input type="checkbox" ... behavior.selectionwithbetdatesclick.marketname="Match Result" ... behavior.selectionwithbetdatesclick.hadvalue="A"><span>2,15</span>
      public double AwayOddFT = -1;

      // <td class="price"><input type="checkbox" ... behavior.selectionwithbetdatesclick.marketname="Total Goals Over / Under" ... behavior.selectionwithbetdatesclick.selectionname="Over" ... behavior.selectionwithbetdatesclick.handicap="0.5"><span>2,15</span>
      public double Over05FT = -1;
      // <td class="price"><input type="checkbox" ... behavior.selectionwithbetdatesclick.marketname="Total Goals Over / Under" ... behavior.selectionwithbetdatesclick.selectionname="Under" ... behavior.selectionwithbetdatesclick.handicap="0.5"><span>2,15</span>
      public double Under05FT = -1;
      public double Over15FT = -1;
      public double Under15FT = -1;
      public double Over25FT = -1;
      public double Under25FT = -1;
      public double Over35FT = -1;
      public double Under35FT = -1;
      public double Over45FT = -1;
      public double Under45FT = -1;


      public override string ToString()
      {
         return $"{Time} {Competition} {HomeTeam} - {AwayTeam} {HomeGoalsFT}:{AwayGoalsFT}  MatchResult: (1){HomeOddFT} (X){DrawOddFT} (2){AwayOddFT}  FT Over/Under(0.5):{Over05FT}/{Under05FT}  FT Over/Under(1.5):{Over15FT}/{Under15FT}  FT Over/Under(2.5):{Over25FT}/{Under25FT}  FT Over/Under(3.5):{Over35FT}/{Under35FT}  FT Over/Under(4.5):{Over45FT}/{Under45FT}";
      }

      public List<ParseInfo> ParseInfo=new List<ParseInfo>();


      public void CheckForEmptyValues()
      {

      }
   }
}
