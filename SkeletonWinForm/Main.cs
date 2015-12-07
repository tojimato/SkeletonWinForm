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

            var x = new TSQL.TSQL<AppData.SkeletonLINQDataContext>();

            
            //Select Data Source From Linq To SQL
            gvTestGrid.DataSource = x.fetch<AppData.MUH_FATURA>()
                .Select(y => new { y.MUH_FATURAID })
                .Where((refx => refx.MUH_FATURAID == 59 || refx.MUH_FATURAID == 58 || refx.MUH_FATURAID == 77));

            // /* ##################################### Run Custom Query.  ################################# */  
            var result = x.runQuery<AppData.MUH_FATURA>(@"Select * FROM PRJTHOR.dbo.MUH_FATURA WHERE MUH_FATURAID={0}",58);
            Console.WriteLine("Run Query : "+result.Count().ToString());

           
            /* ##################################### Insert ################################# */ 
            AppData.IL Il = new AppData.IL(){
                IL_ADI = "Torzincan",
                IL_ADI_BUYUK = "TORZINCAN",
                IL_ADI_KUCUK = "torzincan",
                IL_KODU = "99",
                PLAKA = 101,
                IL_ID = 1011
            };
             var insert_result = x.insert<AppData.IL>(Il);
           
            //Sorgu suresi, Hata Mesaji vs vs..

            /* ####################################### Update ##############################################*/

            AppData.IL IlUpdate = new AppData.IL()
            {
                IL_ADI = "Torzinc33an",
                IL_ADI_BUYUK = "TORZINCAN333333",
                IL_ADI_KUCUK = "torzin33can",
                IL_KODU = "99",
                PLAKA = 101,
                IL_ID = 1011
            };

            var update_result = x.update<AppData.IL>(IlUpdate, (il => il.IL_ID == IlUpdate.IL_ID));
            /* ###################################### Delete ##############################################*/

            var delete_result = x.delete<AppData.IL>(d=> d.IL_ID == 70);
           
           
        }
    }
}
