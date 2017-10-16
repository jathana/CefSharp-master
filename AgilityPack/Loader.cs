using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgilityPack
{
    public class Loader
    {
      #region fields
      private HtmlAgilityPack.HtmlDocument _Document;
      #endregion

      #region constructor
      public Loader()
      {
         _Document = new HtmlAgilityPack.HtmlDocument();
      }
      #endregion

      #region properties
      public HtmlAgilityPack.HtmlDocument Document
      {
         get { return _Document; }
      }

      #endregion

      #region public methods
      public void Load(string path)
      {
         _Document.Load(path,UTF8Encoding.UTF8);
      }

      public void GetData(string html)
      {
         if(_Document != null)
         {
            //File.WriteAllText(path, html, UTF8Encoding.UTF8);
            Parser p = new Parser();
            _Document.LoadHtml(html);
            var r=p.Parse(_Document.DocumentNode.InnerText);
            SaveToDB(r);
            foreach (var i in r)
            {
               Console.WriteLine(i);
            }
         }
      }

      public void SaveInnerText(string path)
      {
         File.WriteAllText(path, _Document.DocumentNode.InnerText);

      }

      #endregion

      #region private methods
      private void SaveToDB(List<BetRecordDO> items)
      {
         //DataSet1TableAdapters.OddsTableTableAdapter adapter = new DataSet1TableAdapters.OddsTableTableAdapter();
         //DataSet1.OddsTableDataTable table = new DataSet1.OddsTableDataTable();
         //foreach (var item in items)
         //{
         //   DataSet1.OddsTableRow row = table.NewOddsTableRow();
         //   row.CreationDate = DateTime.Now;
         //   row.Time = item.Time;
         //   row.HomeTeam = item.HomeTeam;
         //   row.AwayTeam = item.AwayTeam;
         //   row.HomeGoalsFull = item.HomeGoalsFull;
         //   row.AwayGoalsFull = item.AwayGoalsFull;
         //   row.HomeOddFull = Convert.ToDecimal(item.HomeOddFull);
         //   row.DrawOddFull = Convert.ToDecimal(item.DrawOddFull);
         //   row.AwayOddFull = Convert.ToDecimal(item.AwayOddFull);

         //   row.Over05Full = Convert.ToDecimal(item.Over05Full);
         //   row.Under05Full = Convert.ToDecimal(item.Under05Full);

         //   row.Over15Full = Convert.ToDecimal(item.Over15Full);
         //   row.Under15Full = Convert.ToDecimal(item.Under15Full);

         //   row.Over25Full = Convert.ToDecimal(item.Over25Full);
         //   row.Under25Full = Convert.ToDecimal(item.Under25Full);

         //   row.Over35Full = Convert.ToDecimal(item.Over35Full);
         //   row.Under35Full = Convert.ToDecimal(item.Under35Full);

         //   row.Over45Full = Convert.ToDecimal(item.Over45Full);
         //   row.Under45Full = Convert.ToDecimal(item.Under45Full);

         //   table.Rows.Add(row);
         //}
         //adapter.Update(table);
      }
      #endregion
   }
}
