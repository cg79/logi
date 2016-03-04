using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.ENT;
using UI.Helpers;

namespace UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string filePath = @"file:\\index.html";
        string curDir = "";
        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
            //Uri uri = new Uri(filePath);
            //webBrowser1.Navigate(uri);
             curDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            this.webBrowser1.Url = new Uri(String.Format("file:///{0}/index.html", curDir));

            JsonPage jsPage = new JsonPage();
            jsPage.Items = new List<BaseObj>();
            jsPage.Items.Add(new GLabel() { Type="label",For="sd",LineNo=1,text="af"});
            jsPage.Items.Add(new SfpText() { Type = "text", LineNo = 1, model = "af" });
            jsPage.Items.Add(new SfpText() { Type = "text", LineNo = 1, model = "af" });

            richTextBox1.Text = jsPage.ToJSON();
            //string test = jsPage.
        }

       

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control && e.KeyCode == Keys.S)
            {
                RenderAgain();
            }
        }

        private void RenderAgain()
        {
            string rez = "";
            string val = richTextBox1.Text;
            JsonPage jsPage = new JsonPage();
            jsPage.Items = new List<BaseObj>();

            dynamic d = JObject.Parse(val);

            var items = d.Items;
            foreach (var item in items)
            {
                string type = item.Type.ToString();
                switch (type)
                {
                    case "label":
                        {
                            var xxx = ((object)item).ToJSON();
                            GLabel lbl = xxx.JsonDeserialize<GLabel>();
                            //rez = string.Concat(rez,lbl.ToString());
                            jsPage.Items.Add(lbl);
                            break;
                        }
                    case "text":
                        {
                            var xxx = ((object)item).ToJSON();
                            SfpText lbl = xxx.JsonDeserialize<SfpText>();
                            //rez = string.Concat(rez, lbl.ToString());
                            jsPage.Items.Add(lbl);
                            break;
                        }
                }
            }



            System.IO.File.WriteAllText(Path.Combine(curDir, "rendered.html"), rez);



        }
    }
}
