using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Linq.Mapping;


namespace SkeletonWinForm
{
    /*
     Basirisiz insert sonrasi delete islemi yapilamiyor.
     * 
     */
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            var appContext = new AppData.SkeletonLINQDataContext();

            var myObj = new TSQL.TSQL<AppData.SkeletonLINQDataContext>();
            var muh = new AppData.MUH_FATURA();


            /** INSERT  START **/

            var result = myObj.insert<AppData.IL>(new AppData.IL()
            {
                IL_ID = 1000,
                IL_ADI = "TulayBakir",
                PLAKA = 255,
                IL_ADI_BUYUK = "TULAYBAKIR",
                IL_ADI_KUCUK = "tulaybakir",
                IL_KODU = "99"
            });

            Console.WriteLine("Insert : " + result.IsSucceeded.ToString());
            /** INSERT  END **/

            /** FECTH  START **/

            var result2 = myObj.fetch<AppData.MUH_FATURA>(
                _where: w => w.MUH_FATURAID == 59 || w.MUH_FATURAID == 58 || w.MUH_FATURAID == 7).Select(x => new { x.MUH_FATURAID});

            gvT.DataSource = result2;

            if (gvT.RowCount > 0)
            {
                Console.WriteLine("Fecth : True "); 
            }
            /** FECTH  END **/

            
            /* RUN CUSTOM QUERY  START*/

            var result3 = myObj.runQuery<AppData.MUH_FATURA>(@"SELECT * FROM dbo.MUH_FATURA WHERE MUH_FATURAID={0}", 58);
            Console.WriteLine("Run Query : " + result3.IsSucceeded.ToString());

            /* RUN CUSTOM QUERY  END */

            /* RUN CUSTOM QUERY  START*/

            var result4 = myObj.runQuery<String>(@"SELECT dbo.FN_BUGUNUN_TARIHI({0})", DateTime.Now);
            Console.WriteLine("Run Query : " + result3.IsSucceeded.ToString());

            /* RUN CUSTOM QUERY  END */



            /* UPDATE START */

            AppData.IL IlUpdate = new AppData.IL()
            {
                IL_ADI = "IKILI UPDATE",
                IL_KODU = "99",
                PLAKA = 101,
                IL_ID = 1011
            };

            var update_result = myObj.update<AppData.IL>(IlUpdate, (il => il.IL_ID == 1011 || il.IL_ID == 1000));
            Console.WriteLine("Update :" + update_result.IsSucceeded.ToString());

            /* UPDATE END*/ 
       
            
           
           
        }
    }
}
