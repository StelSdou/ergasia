using iText.Html2pdf;
using System;
using System.IO;
using System.Windows.Forms;

namespace FrontKEP
{
    internal class Item : TableLayoutPanel
    {
        private ContextMenuStrip m;
        private int id;
        private string fName;
        private string email;
        private string phone;
        private string address;
        private string dateOfBirth;
        private string dateTime;
        private string type;

        private MyLabel lId, lFN, lEm, lPh, lAd, lDoB, lDTN, lT;

        public Item(int id, string fName, string email, string phone, string address, string dateOfBirth, string dateTime, string type, int width)
        {
            m = new ContextMenuStrip();
            m.Items.Add("Print", null, print);
            m.Items.Add("Copy");
            m.Items.Add("Paste");

            ContextMenuStrip = m;

            this.id = id;
            this.fName = fName;
            this.email = email;
            this.phone = phone;
            this.address = address;
            this.dateOfBirth = dateOfBirth;
            this.dateTime = dateTime;
            this.type = type;

            Margin = new Padding(0);
            Width = width;
            Height = 20;

            ColumnCount = 8;
            RowCount = 1;

            ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.28571F));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.28571F));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.28571F));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.28571F));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.28571F));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.28571F));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.28571F));

            RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            lId = new MyLabel(id.ToString());
            lFN = new MyLabel(fName);
            lEm = new MyLabel(email);
            lPh = new MyLabel(phone);
            lAd = new MyLabel(address);
            lDoB = new MyLabel(dateOfBirth);
            lDTN = new MyLabel(dateTime);
            lT = new MyLabel(type);

            Controls.Add(lId, 0, 0);
            Controls.Add(lFN, 1, 0);
            Controls.Add(lEm, 2, 0);
            Controls.Add(lPh, 3, 0);
            Controls.Add(lAd, 4, 0);
            Controls.Add(lDoB, 5, 0);
            Controls.Add(lDTN, 6, 0);
            Controls.Add(lT, 7, 0);

            foreach (MyLabel c in Controls)
                c.MouseDown += Cl_MouseDown;
        }

        private void Cl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m.Show((MyLabel)sender, e.Location);
            }
        }

        private void print(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();

            var htmlContent = @"
            <html>
                <head>
	                 <style>
		                  * {
			                margin: 0;
		                  }
		                  body {
			                margin: 50px;
		                  }
		                  img {
			                width: 200px;
			                margin-bottom: 10px;
		                  }
                          .title {
			                background-color: rgb(0, 171, 0);
			                color: white;
			                padding: 10px;
		                  }
		                  h3 {
			                margin: 10px;
		                  }
                          .det {
			                display: grid;
			                grid-template-columns: 150px auto;
		                  }
		                  .c {
			                margin-right: 10px;
			                display: flex;
			                flex-direction: column;
			                justify-content: center;
		                  }
                          .box {
			                border: 1px solid;
			                padding: 0 40px 100px 40px;
		                  }
                          .foot {
			                margin-top: 200px;
			                display: flex;
			                align-items: center;
			                justify-content: right;
		                  }
		                </style>
	                 </head>
                    <body>
                        <header>
                            <img src=""logo/NewLogo.png"" />
                            <hr/>
		                    <h2 style=""margin: 20px 0 30px"">
                            Προσοχή: Το παρόν αρχείο περιέχει σημαντικά στοιχεία. Παρακαλείστε να
                            διαχειρίζεστε το περιεχόμενο με υπευθυνότητα και να τηρείτε τους κανόνες
                            εμπιστευτικότητας.
                            </h2>
                        </header>
                        <main>
		                    <div class=""title"">
			                <h4>Member Details</h4>
		                    </div>
		                    <div class=""det"">
			                <h3>Full Name</h3>
			                <div class=""c"">
			                    <p class=""name"">" + fName + @"</p>
			                    <hr />
			                </div>
			                <h3>E-mail</h3>
			                <div class=""c"">
			                    <p class=""mail"">" + email + @"</p>
			                    <hr />
			                </div>
			                <h3>Phone</h3>
			                <div class=""c"">
			                    <p class=""phone"">" + phone + @"</p>
			                    <hr />
			                </div>
			                <h3>Address</h3>
			                <div class=""c"">
			                    <p class=""address"">" + address + @"</p>
			                    <hr />
			                </div>
			                <h3>Date of Birth</h3>
			                <div class=""c"">
			                    <p class=""DoB"">" + dateOfBirth + @"</p>
			                    <hr />
			                </div>
			                <h3>Type</h3>
			                <div class=""c"">
			                    <p class=""type"">" + type + @"</p>
			                    <hr />
			                </div>
		                    </div>
		                </main>
                        <div class=""foot"">
                            <div class=""box"">
                            <h2>Υπογραφή</h2>
                            </div>
                        </div>
                    </body>
                </html>";

            if (path.ShowDialog() == DialogResult.OK)
                using (var stream = new FileStream(path.SelectedPath + "/" + fName + ".pdf", FileMode.Create))
                {
                    HtmlConverter.ConvertToPdf(htmlContent, stream);
                }

            Console.WriteLine("PDF created successfully!");
        }

        public void Edit(MyLabel act, bool enable, bool save)
        {
            act.Edit(enable);
            if (save)
            {
                act.setChange();
                fName = lFN.getText();
                email = lEm.getText();
                phone = lPh.getText();
                address = lAd.getText();
                dateOfBirth = lDoB.getText();
                type = lT.getText();
            }
            else
                act.restore();
        }

        public string[] GetAll()
        {
            return new string[] { fName, email, phone, address, dateOfBirth, dateTime, type };
        }

        public int getId()
        {
            return id;
        }
        public void setResize(int width)
        {
            Width = width;
        }
    }
}