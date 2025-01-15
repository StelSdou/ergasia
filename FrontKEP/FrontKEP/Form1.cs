using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data.SQLite;

namespace FrontKEP
{
    public partial class Form1 : Form
    {
        bool editAndDelete = true;
        Control activeCtr;
        string connectionString = "Data Source=kepdb.db;Version = 3;";
        SQLiteConnection connection;
        string fullname, email, phone, birthdate, request, address, date;
        int id, sum;

        public Form1()
        {
            InitializeComponent();
            TLPAdd.Visible = false;
            connection = new SQLiteConnection(connectionString);

            //fLPList.WrapContents = false;
            //fLPList.HorizontalScroll.Enabled = false;
            //fLPList.HorizontalScroll.Visible = false;

            //Alpha = 255×(1−0.5)   50%
            editEl(panel1, true,153, 50);
            editEl(PSearch, true, 127, 30);
            editEl(PDel, true, 100, 30);
            editEl(PEdit, true, 100, 30);
            editEl(PView, true, 90, 50);
            editEl(PTaleColor, false, 100, 50);
            line.Width = flowLayoutPanel1.ClientSize.Width - 30;

            tSearch.BorderStyle = BorderStyle.None;
            textBox2.BorderStyle = BorderStyle.None;
            textBox3.BorderStyle = BorderStyle.None;
            textBox4.BorderStyle = BorderStyle.None;
            textBox5.BorderStyle = BorderStyle.None;
            comboBox1.FlatStyle = FlatStyle.Flat;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connection.Open();

            // ----------------- Check the existance of the table -----------------
            string checkTableSQL = "SELECT name FROM sqlite_master WHERE type='table' AND name='Info';";
            SQLiteCommand checkTableCommand = new SQLiteCommand(checkTableSQL, connection);
            var result = checkTableCommand.ExecuteScalar();
            //---------------------------------------------------------------------
            if (result == null)
            {
                // ----------------- Creation of data base -----------------             
                string createTableSQL = @"CREATE TABLE Info(
                    Ιd INTEGER PRIMARY KEY AUTOINCREMENT,
                    Fullname TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    Phone_number TEXT NOT NULL,
                    Birth_date TEXT NOT NULL,
                    Request_type TEXT NOT NULL,
                    Home_address TEXT NOT NULL,
                    Date_and_time TEXT NOT NULL)";
                SQLiteCommand createCommand = new SQLiteCommand(createTableSQL, connection);
                createCommand.ExecuteNonQuery();
            }
            connection.Close();

            toolTip1.SetToolTip(this.tSearch, "Αναζήτηση με βάση το όνομα");

            viewAll();
            tableLayoutPanel1.Width = flowLayoutPanel1.Width - 50;
            LV.BackColor = Color.FromArgb(100, 56, 145, 70);
            PV.BackColor = Color.FromArgb(100, 56, 145, 70);
        }

        private void editEl(Panel el, bool t, int trans, int radius)
        {
            el.BackColor = Color.Transparent;

            el.Paint += (s, args) =>
            {
                Graphics g = args.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Δημιουργία στρογγυλεμένου πλαισίου
                using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(el.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(el.Width - radius, el.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, el.Height - radius, radius, radius, 90, 90);
                    path.CloseFigure();

                    if (t)
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(trans, 255, 255, 255)))
                           g.FillPath(brush, path);
                    else
                        using (LinearGradientBrush brush = new LinearGradientBrush(
                            PTaleColor.ClientRectangle,
                            Color.FromArgb(118, 151, 121),
                            Color.FromArgb(126, 133, 153),
                            LinearGradientMode.Vertical))
                        {
                            g.FillPath(brush, path);
                        }
                }
            };
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            line.Width = flowLayoutPanel1.ClientSize.Width - 30;
            tableLayoutPanel1.Width = flowLayoutPanel1.Width - 600;
            foreach (Item item in fLPList.Controls)
            {
                item.setResize(fLPList.Width);
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            goTo(0);
        }

        private void View_Click(object sender, EventArgs e)
        {
            goTo(1);
        }

        private async void viewAll()
        {
            fLPList.Controls.Clear();
            // ----------------- read from data base -----------------
            connection.Open();
            String selectSQL = "SELECT * FROM Info";
            SQLiteCommand selectCommand = new SQLiteCommand(selectSQL, connection);
            sum = 0;
            using (SQLiteDataReader reader = selectCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Ιd"]);
                    fullname = reader["Fullname"].ToString();
                    email = reader["Email"].ToString();
                    phone = reader["Phone_number"].ToString();
                    birthdate = reader["Birth_date"].ToString();
                    request = reader["Request_type"].ToString();
                    address = reader["Home_address"].ToString();
                    date = reader["Date_and_time"].ToString();
                    
                    fLPList.Controls.Add(new Item(id, fullname, email, phone, address, birthdate, date, request, fLPList.Width));
                    sum++;
                }
            }
            label7.Text = sum.ToString();
            connection.Close();
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            goTo(2);
        }

        private void Trash_Click(object sender, EventArgs e)
        {
            goTo(3);
        }

        private void PEd_Click(dynamic sender, EventArgs e)
        {
            if (TLPAdd.Visible)
            {
                //Save New
                fullname = textBox2.Text.ToUpper();
                email = textBox3.Text;
                phone = textBox4.Text;
                address = textBox5.Text.ToUpper();
                birthdate = monthCalendar1.SelectionStart.Date.ToShortDateString();
                request = comboBox1.Text.ToUpper();
                date = DateTime.Now.ToString();
                clearInp();

                //Insert Data to DataBase \/
                connection.Open();
                string insertSQl = "INSERT INTO Info(Fullname, Email, Phone_number, Birth_date, Request_type, Home_address, Date_and_time)" +
                "VALUES(@fullname, @email, @phone, @birthdate, @request, @address, @date)";
                SQLiteCommand insertCommand = new SQLiteCommand(insertSQl, connection);
                insertCommand.Parameters.AddWithValue("@fullname", fullname);
                insertCommand.Parameters.AddWithValue("@email", email);
                insertCommand.Parameters.AddWithValue("@phone", phone);
                insertCommand.Parameters.AddWithValue("@birthdate", birthdate);
                insertCommand.Parameters.AddWithValue("@request", request);
                insertCommand.Parameters.AddWithValue("@address", address);
                insertCommand.Parameters.AddWithValue("@date", date);
                insertCommand.ExecuteNonQuery();
                connection.Close();

                viewAll();
            }
            else
            {
                if (editAndDelete)
                {
                    //Edit
                    if (ActiveControl.Name != "tSearch")
                    {
                        activeCtr = ActiveControl;
                        ((Item)activeCtr.Parent).Edit((MyLabel)activeCtr, true, true);
                        
                        showButtonsAsEdit(false);
                    }
                }
                else
                {
                    Item act = (Item)activeCtr.Parent;
                    //Save
                    act.Edit((MyLabel)activeCtr, false, true);
                    showButtonsAsEdit(true);
                    checkEdit();
                    //set Edit to DataBase \/
                    try
                    {
                        MessageBox.Show(act.getId().ToString());
                        connection.Open();

                        // Ενημέρωση εγγραφής
                        string updateSQL = "UPDATE Info SET Fullname = @fullname, Email = @email, Phone_number = @phone, Birth_date = @birthdate, Request_type = @request, Home_address = @address, Date_and_time = @date WHERE Ιd = @id";
                        SQLiteCommand updateCommand = new SQLiteCommand(updateSQL, connection);
                        updateCommand.Parameters.AddWithValue("@fullname", act.GetAll()[0]);
                        updateCommand.Parameters.AddWithValue("@email", act.GetAll()[1]);
                        updateCommand.Parameters.AddWithValue("@phone", act.GetAll()[2]);
                        updateCommand.Parameters.AddWithValue("@birthdate", act.GetAll()[4]);
                        updateCommand.Parameters.AddWithValue("@request", act.GetAll()[6]);
                        updateCommand.Parameters.AddWithValue("@address", act.GetAll()[3]);
                        updateCommand.Parameters.AddWithValue("@date", act.GetAll()[5]);
                        updateCommand.Parameters.AddWithValue("@id", act.getId());

                        int updatedRows = updateCommand.ExecuteNonQuery();
                        Console.WriteLine($"Updated {updatedRows} rows.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }


                }
            }
        }

        private void PTr_Click(object sender, EventArgs e)
        {
            if (TLPAdd.Visible)
            {
                clearInp();
            }
            else
            {
                if (editAndDelete)
                {
                    //Delete
                    if (ActiveControl.Name != "tSearch")
                    {
                        Item del = (Item)ActiveControl.Parent;
                        DialogResult mnm = MessageBox.Show($"You wont DELETE: \n {del.GetAll()[0]}, With type {del.GetAll()[6]}", "Delete", MessageBoxButtons.YesNo);
                        if (mnm == DialogResult.Yes)
                        {
                            DialogResult sure = MessageBox.Show("Are you sure?", "Verify", MessageBoxButtons.YesNo);
                            if (sure == DialogResult.Yes)
                            {
                                int id = del.getId();
                                fLPList.Controls.Remove(del);
                                //Delete From DataBase using id\/
                                try
                                {
                                    connection.Open();
                                    String deleteSQL = "DELETE FROM Info WHERE Ιd = @id";
                                    SQLiteCommand deleteCommand = new SQLiteCommand(deleteSQL, connection);
                                    deleteCommand.Parameters.AddWithValue("@id", id);
                                    deleteCommand.ExecuteNonQuery();
                                    connection.Close();
                                }
                                catch (SystemException ex)
                                {
                                    MessageBox.Show(string.Format("An error occurred: {0}", ex.Message));
                                }
                                //refresh data from DataBase
                                viewAll();
                            }
                        }
                    }
                }
                else
                {
                    ((Item)activeCtr.Parent).Edit((MyLabel)activeCtr, false, false);
                    activeCtr = null;
                    editOrAdd(true);
                    checkEdit();
                }
            }
        }        

        private void editOrAdd(bool edit)
        {
            if (edit)
            {
                PTr.Image = Properties.Resources.Btrash;
                PEd.Image = Properties.Resources.Bedit;
            }
            else
            {
                PTr.Image = Properties.Resources.Cansel;
                PEd.Image = Properties.Resources.Save;
            }
            editAndDelete = edit;
        }

        private void showButtonsAsEdit(bool edit)
        {
            editOrAdd(edit);
            ((TableLayoutPanel)PDel.Parent).SetColumnSpan(PDel, 1);
            ((TableLayoutPanel)PEdit.Parent).SetColumnSpan(PEdit, 1);
            ((TableLayoutPanel)PEdit.Parent).SetColumn(PEdit, 1);
            PEdit.Visible = true;
            PDel.Visible = true;
        }

        private void checkEdit()
        {
            if (Title.Text == "Edit")
            {
                editOrAdd(true);
                ((TableLayoutPanel)PDel.Parent).SetColumnSpan(PDel, 1);
                PEdit.Visible = true;
                PDel.Visible = false;
                ((TableLayoutPanel)PEdit.Parent).SetColumn(PEdit, 0);
                ((TableLayoutPanel)PEdit.Parent).SetColumnSpan(PEdit, 2);
            }
        }

        private void clearInp()
        {
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
            comboBox1.Text = "Select";
        }

        private void goTo(int to)
        {
            viewAll();
            bool set = true;

            LA.BackColor = Color.Transparent;
            PA.BackColor = Color.Transparent;

            LV.BackColor = Color.Transparent;
            PV.BackColor = Color.Transparent;

            LE.BackColor = Color.Transparent;
            PE.BackColor = Color.Transparent;

            LT.BackColor = Color.Transparent;
            PT.BackColor = Color.Transparent;
            switch (to)
            {
                case 0:
                    set = false;
                    LA.BackColor = Color.FromArgb(100, 56, 145, 70);
                    PA.BackColor = Color.FromArgb(100, 56, 145, 70);
                    
                    TLPView.Visible = false;
                    if (!TLPAdd.Visible)
                        TLPAdd.Visible = true;

                    showButtonsAsEdit(false);
                    break;
                case 1:
                    LV.BackColor = Color.FromArgb(100, 56, 145, 70);
                    PV.BackColor = Color.FromArgb(100, 56, 145, 70);

                    TLPAdd.Visible = false;
                    if (!TLPView.Visible)
                        TLPView.Visible = true;
                    Title.Text = "View";

                    showButtonsAsEdit(true);
                    break;
                case 2:
                    LE.BackColor = Color.FromArgb(100, 56, 145, 70);
                    PE.BackColor = Color.FromArgb(100, 56, 145, 70);

                    TLPAdd.Visible = false;
                    if (!TLPView.Visible)
                        TLPView.Visible = true;
                    Title.Text = "Edit";

                    checkEdit();
                    break;
                case 3:
                    LT.BackColor = Color.FromArgb(100, 56, 145, 70);
                    PT.BackColor = Color.FromArgb(100, 56, 145, 70);

                    TLPAdd.Visible = false;
                    if (!TLPView.Visible)
                        TLPView.Visible = true;
                    Title.Text = "Trash";


                    ((TableLayoutPanel)PEdit.Parent).SetColumnSpan(PEdit, 1);
                    ((TableLayoutPanel)PEdit.Parent).SetColumn(PEdit, 1);
                    PEdit.Visible = false;
                    PDel.Visible = true;
                    ((TableLayoutPanel)PDel.Parent).SetColumnSpan(PDel, 2);
                    break;
            }
            editOrAdd(set);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            goTo(1);
            string text = tSearch.Text.ToUpper();            
            readFromDb(text);             
        }

        private void readFromDb(string f)
        {
            sum = 0;
            // ----------------- read from data base -----------------
            connection.Open();
            String selectSQL = "SELECT * FROM Info WHERE Fullname = @fullname";
            SQLiteCommand selectCommand = new SQLiteCommand(selectSQL, connection);
            selectCommand.Parameters.AddWithValue("@fullname", f);
            
            fLPList.Controls.Clear();
            using (SQLiteDataReader reader = selectCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["Ιd"]);
                    fullname = reader["Fullname"].ToString();
                    email = reader["Email"].ToString();
                    phone = reader["Phone_number"].ToString();
                    birthdate = reader["Birth_date"].ToString();
                    request = reader["Request_type"].ToString();
                    address = reader["Home_address"].ToString();
                    date = reader["Date_and_time"].ToString();

                    fLPList.Controls.Add(new Item(id, fullname, email, phone, address, birthdate, date, request, fLPList.Width));
                    sum++;
                    label7.Text = sum.ToString();
                }
                if (sum == 0) MessageBox.Show("Δεν βρέθηκε καμία εγγραφή με το όνομα αυτό.");
                
            }
            selectCommand.ExecuteNonQuery();
            connection.Close();
        }
    }
}
