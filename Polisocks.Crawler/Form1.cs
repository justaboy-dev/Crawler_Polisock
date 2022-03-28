using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Polisocks.Crawler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        HttpClient httpClient = new HttpClient();
        static String baseUrl = "https://poolisocks.com/";
        static ExcelPackage excelPackage;
        static int maxpage = 17;
        List<Sock> socks = new List<Sock>();
        async Task<String> getHtml(String html)
        {
            var result = await httpClient.GetStringAsync(new Uri(html));
            return result;
        }
        async void onCrawl()
        {
            for(int i = 1; i <= maxpage; i ++)
            {
                String url = baseUrl + "collections/all?page=" + i.ToString();
                string html = await getHtml(url);
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                HtmlAgilityPack.HtmlNodeCollection listitem = doc.DocumentNode.SelectNodes("/html/body/div[1]/section/div/section/div/div/div[3]/div/div/div[2]/div");

                foreach (var item in listitem)
                {
                    var price = item.Descendants(0).Where(n => n.HasClass("pro-price")).ToList()[0].InnerText;
                    var name = item.Descendants(0).Where(n => n.HasClass("pro-name")).ToList()[0].ChildNodes[0].InnerText;
                    var Listimage = item.Descendants(0).Where(n => n.HasClass("product-img")).ToList()[0].ChildNodes.Where(n=>n.Name=="a").ToList()[0];
                    foreach(var image in Listimage.ChildNodes.Where(n=>n.Name=="img"))
                    {
                        var imageurl = image.Attributes["src"];
                        MessageBox.Show(imageurl.Value);
                    }    
                    
                }
            }    
        }

        private void btnCrawl_Click(object sender, EventArgs e)
        {
            onCrawl();
        }
    }
}
