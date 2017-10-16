using AgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
   class Program
   {
      static void Main(string[] args)
      {

         HtmlParser parser = new HtmlParser();
         parser.ParseFile(@"F:\My\Dev\OPAPStoixima\data\source_2017-10-15-11-54-10-829");
      }
   }
}
