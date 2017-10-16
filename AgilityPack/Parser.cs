using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AgilityPack
{
   public class Parser
   {
      //private string _Pattern = @"(?<League>.+)\n(?<Time>.+)\n(?<HomeTeam>.+)\n(?<HomeGoalsFull>[0-9]+)\n(?<AwayTeam>.+)\n(?<AwayGoalsFull>[0-9]+)\n.+\n1\nX\n2\n(?<HomeOddFull>.+)\n(?<DrawOddFull>.+)\n(?<AwayOddFull>.+)\n";
      //private string _Pattern = @"(?<League>.+)\n(?<Time>.+)\n(?<HomeTeam>.+)\n(?<HomeGoalsFull>[0-9]+)\n(?<AwayTeam>.+)\n(?<AwayGoalsFull>[0-9]+)\n.+\n1\nX\n2\n(?<HomeOddFull>.+)\n(?<DrawOddFull>.+)\n(?<AwayOddFull>.+)\n((Total Goals Over \/ Under)\n(U0[.]5)\nO\n(?<Under05Full>[0-9]+[,][0-9]+)\n(?<Over05Full>[0-9]+[,][0-9]+)\n)*((Total Goals Over \/ Under)\n(U1[.]5)\nO\n(?<Under15Full>[0-9]+[,][0-9]+)\n(?<Over15Full>[0-9]+[,][0-9]+)\n)*((Total Goals Over \/ Under)\n(U2[.]5)\nO\n(?<Under25Full>[0-9]+[,][0-9]+)\n(?<Over25Full>[0-9]+[,][0-9]+)\n)*((Total Goals Over \/ Under)\n(U3[.]5)\nO\n(?<Under35Full>[0-9]+[,][0-9]+)\n(?<Over35Full>[0-9]+[,][0-9]+)\n)*((Total Goals Over \/ Under)\n(U4[.]5)\nO\n(?<Under45Full>[0-9]+[,][0-9]+)\n(?<Over45Full>[0-9]+[,][0-9]+)\n)*";
      private string _Pattern = @"(?<League>.+)\n((?<Time>([0-9]+[:][0-9]+)|(HT))\n)*(?<HomeTeam>.+)\n(?<HomeGoalsFull>[0-9]+)\n(?<AwayTeam>.+)\n(?<AwayGoalsFull>[0-9]+)\n.+\n1\nX\n2\n(?<HomeOddFull>.+)\n(?<DrawOddFull>.+)\n(?<AwayOddFull>.+)\n";

      public List<BetRecordDO> Parse(string text)
      {
         text=CleanText(text);
         List<BetRecordDO> retval = new List<BetRecordDO>();
         Regex reg = new Regex(_Pattern);
         var matches = reg.Matches(text);
         foreach(Match m in matches)
         {
            BetRecordDO newRec = new BetRecordDO()
            {
               League = m.Groups["League"].Value,
               Time = m.Groups["Time"].Value,
               HomeTeam = m.Groups["HomeTeam"].Value,
               HomeGoalsFull = Convert.ToInt32(m.Groups["HomeGoalsFull"].Value),
               AwayTeam = m.Groups["AwayTeam"].Value,
               AwayGoalsFull = Convert.ToInt32(m.Groups["AwayGoalsFull"].Value),
            };


            if (!Double.TryParse(m.Groups["HomeOddFull"].Value, out newRec.HomeOddFull)) newRec.HomeOddFull = -1;
            if (!Double.TryParse(m.Groups["DrawOddFull"].Value, out newRec.DrawOddFull)) newRec.DrawOddFull = -1; 
            if (!Double.TryParse(m.Groups["AwayOddFull"].Value, out newRec.AwayOddFull)) newRec.AwayOddFull = -1;

            if (!Double.TryParse(m.Groups["Over05Full"].Value, out newRec.Over05Full)) newRec.Over05Full = -1;
            if (!Double.TryParse(m.Groups["Under05Full"].Value, out newRec.Under05Full)) newRec.Under05Full = -1;

            if (!Double.TryParse(m.Groups["Over15Full"].Value, out newRec.Over15Full)) newRec.Over15Full = -1;
            if (!Double.TryParse(m.Groups["Under15Full"].Value, out newRec.Under15Full)) newRec.Under15Full = -1;

            if (!Double.TryParse(m.Groups["Over25Full"].Value, out newRec.Over25Full)) newRec.Over25Full = -1;
            if (!Double.TryParse(m.Groups["Under25Full"].Value, out newRec.Under25Full)) newRec.Under25Full = -1;

            if (!Double.TryParse(m.Groups["Over35Full"].Value, out newRec.Over35Full)) newRec.Over35Full = -1;
            if (!Double.TryParse(m.Groups["Under35Full"].Value, out newRec.Under35Full)) newRec.Under35Full = -1;

            if (!Double.TryParse(m.Groups["Over45Full"].Value, out newRec.Over45Full)) newRec.Over45Full = -1;
            if (!Double.TryParse(m.Groups["Under45Full"].Value, out newRec.Under45Full)) newRec.Under45Full = -1;

            retval.Add(newRec);
         }
         return retval;
      }


      private string CleanText(string text)
      {
         string stext = text.Replace("&nbsp;", "");
         stext = Regex.Replace(stext, @"\t", "", RegexOptions.Multiline);
         stext = Regex.Replace(stext, @"^\s*$[\r\n]*", "", RegexOptions.Multiline);

         return stext;
      }
   }
}
