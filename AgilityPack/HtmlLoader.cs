using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgilityPack
{
    public class HtmlLoader
    {
      #region fields
      
      #endregion

      #region constructor
      public HtmlLoader()
      {
         
      }
      #endregion

      #region properties

      #endregion

      #region public methods
      
      public void GetData(string html)
      {
         if(!string.IsNullOrEmpty(html))
         {
            //File.WriteAllText(path, html, UTF8Encoding.UTF8);
            HtmlParser p = new HtmlParser();            
            var recs=p.Parse(html);
            SaveToDB(recs);
            foreach (var i in recs)
            {
               Console.WriteLine(i);
            }
         }
      }

      #endregion

      #region private methods
      private void SaveToDB(List<BetRecordDOExtended> items)
      {
         DataSet1TableAdapters.OddsTableTableAdapter adapter = new DataSet1TableAdapters.OddsTableTableAdapter();
         DataSet1.OddsTableDataTable table = new DataSet1.OddsTableDataTable();
         foreach (var item in items)
         {
            
            DataSet1.OddsTableRow row = table.NewOddsTableRow();
            try
            {
               row.EventId = item.EventId;
               row.BetRadarId = item.BetRadarId;
               row.IsAwaitingStart = item.IsAwaitingStart;
               row.Sport = item.Sport;
               row.BetStart = item.BetStart;
               row.GameStart = item.GameStart;
               row.CreationDate = DateTime.Now;
               row.Time = item.Time;
               row.Competition = item.Competition;
               row.HomeTeam = item.HomeTeam;
               row.AwayTeam = item.AwayTeam;
               row.HomeGoalsFT = item.HomeGoalsFT;
               row.AwayGoalsFT = item.AwayGoalsFT;
               row.HomeOddFT = Convert.ToDecimal(item.HomeOddFT);
               row.DrawOddFT = Convert.ToDecimal(item.DrawOddFT);
               row.AwayOddFT = Convert.ToDecimal(item.AwayOddFT);

               row.Over05FT = Convert.ToDecimal(item.Over05FT);
               row.Under05FT = Convert.ToDecimal(item.Under05FT);

               row.Over15FT = Convert.ToDecimal(item.Over15FT);
               row.Under15FT = Convert.ToDecimal(item.Under15FT);

               row.Over25FT = Convert.ToDecimal(item.Over25FT);
               row.Under25FT = Convert.ToDecimal(item.Under25FT);

               row.Over35FT = Convert.ToDecimal(item.Over35FT);
               row.Under35FT = Convert.ToDecimal(item.Under35FT);

               row.Over45FT = Convert.ToDecimal(item.Over45FT);
               row.Under45FT = Convert.ToDecimal(item.Under45FT);

               table.Rows.Add(row);
            }
            catch (Exception ex)
            {
            }

         }
         adapter.Update(table);
      }
      #endregion
   }
}
