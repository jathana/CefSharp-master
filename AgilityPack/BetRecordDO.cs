using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgilityPack
{
   public class BetRecordDO
   {
      public string League = "";
      public string Time = "";
      public string HomeTeam = "";
      public string AwayTeam = "";
      public double HomeOddFull;
      public double DrawOddFull;
      public double AwayOddFull;
      public int HomeGoalsFull = 0;
      public int AwayGoalsFull = 0;
      public double Over05Full;
      public double Under05Full;
      public double Over15Full;
      public double Under15Full;
      public double Over25Full;
      public double Under25Full;
      public double Over35Full;
      public double Under35Full;
      public double Over45Full;
      public double Under45Full;


      public override string ToString()
      {
         return $"{Time} {League} {HomeTeam} - {AwayTeam} {HomeGoalsFull}:{AwayGoalsFull}  (1){HomeOddFull} (X){DrawOddFull} (2){AwayOddFull}";
      }
   }
}
