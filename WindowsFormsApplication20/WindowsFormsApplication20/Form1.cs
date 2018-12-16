using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication20
{
    public partial class Form1 : Form
    {
        SqlConnection sql;
        public Form1()
        {
            
            InitializeComponent();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            label6.Visible = false;
            label8.Visible = false;
            label7.Visible = false;
            string ConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = e:\VisualStudio_Projects\WindowsFormsApplication20\WindowsFormsApplication20\Database1.mdf; Integrated Security = True";
            sql  = new SqlConnection(ConnectionString); //создаём соединение
            await sql.OpenAsync();// открываем запросы

            SqlDataReader sqlreader = null;// считывает с таблицы

            SqlCommand command = new SqlCommand("SELECT * FROM [PRODUCTS]", sql);
            try
            {
                sqlreader = await command.ExecuteReaderAsync(); //считает только столбцы await class
                while (await sqlreader.ReadAsync())
                {
                    listBox1.Items.Add(Convert.ToString(sqlreader["ID"]) + "     " + Convert.ToString(sqlreader["Name"]) + " " + Convert.ToString(sqlreader["Price"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); // ex.Source.ToString - вывод название ошибки
            }
            finally
            {
                if (sqlreader != null)
                {
                    sqlreader.Close();
                }
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sql != null && sql.State != ConnectionState.Closed)   // нужно зыкрвать соединение, чтобы не было утечки данных
            {
                sql.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sql != null && sql.State != ConnectionState.Closed)   // нужно зыкрвать соединение, чтобы не было утечки данных
            {
                sql.Close();
            }
        }

        private async void button1_Click(object sender, EventArgs e) //async - ассинхроность,  другой поток
        {
            if (label7.Visible) { label7.Visible = false; } //убирает надпись от лейбла 7   
            if ((!string.IsNullOrEmpty(textBox1.Text)) && (!string.IsNullOrWhiteSpace(textBox1.Text)) && 
               (!string.IsNullOrEmpty(textBox1.Text)) && (!string.IsNullOrWhiteSpace(textBox1.Text))) // проверка на не пустые поля 
            {
                SqlCommand command = new SqlCommand("INSERT INTO [PRODUCTS] (NAME, PRICE)VALUES (@NAME,@PRICE)", sql);
                command.Parameters.AddWithValue("NAME", textBox1.Text); // объяснение для прудыдущего шага, что такое NAME и PRICE
                command.Parameters.AddWithValue("PRICE", textBox2.Text);// в таблицу продукты вставляются Имена и Цены
                await command.ExecuteNonQueryAsync(); //await ассинхроность выполнения команд, не нужно ничего получать от метода, поэтому выполнение без запроса
            }
            else
            {
                label7.Visible = true;
                label7.Text = "Введите значения в таблицу";
            }
        }

        

        private async void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string ConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = e:\VisualStudio_Projects\WindowsFormsApplication20\WindowsFormsApplication20\Database1.mdf; Integrated Security = True";
            sql = new SqlConnection(ConnectionString);
            await sql.OpenAsync();
            SqlDataReader sqlreader = null; // зачем null? без нула не работает строка 124
            SqlCommand sqlcomnad = new SqlCommand("SELECT * FROM [PRODUCTS]", sql);

            try
            {
                sqlreader = await sqlcomnad.ExecuteReaderAsync();// Получает новые значения
                while (await sqlreader.ReadAsync())// пока есть записи
                {
                    listBox1.Items.Add(Convert.ToString(sqlreader["ID"]) + " " + Convert.ToString(sqlreader["NAME"]) + " " + Convert.ToString(sqlreader["PRICE"]) + " "); // элемент 'ID' у ридера
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
            finally
            {
                if(sqlreader != null)
                 sqlreader.Close();
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            label8.Visible = false;
            if (label8.Visible) { label8.Visible = false; }
            if ((!string.IsNullOrEmpty(textBox3.Text)) && (!string.IsNullOrWhiteSpace(textBox3.Text)) &&
              (!string.IsNullOrEmpty(textBox4.Text)) && (!string.IsNullOrWhiteSpace(textBox4.Text)) &&
              (!string.IsNullOrEmpty(textBox5.Text)) && (!string.IsNullOrWhiteSpace(textBox5.Text)))

            {
                
                SqlCommand command = new SqlCommand("UPDATE [PRODUCTS] SET [NAME] = @NAME, [PRICE]=@PRICE WHERE [ID]=@ID", sql); // обязательно передавать соединение, как параметр

                command.Parameters.AddWithValue("NAME", textBox4.Text);
                command.Parameters.AddWithValue("PRICE", textBox3.Text);
                command.Parameters.AddWithValue("ID", textBox5.Text);

                //не требуется возвращаемых значений

                await command.ExecuteNonQueryAsync();
                
            }
            else if ((!string.IsNullOrEmpty(textBox5.Text)) && (!string.IsNullOrWhiteSpace(textBox5.Text)))
            {
                label8.Visible = true;
                label8.Text = "Не введено значение";
            }
            
            else
            {
                label8.Visible = true;
                label8.Text = "Не введено значение";
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
           if (label6.Visible) { label6.Visible = false; }
            if ((!string.IsNullOrEmpty(textBox6.Text)) && (!string.IsNullOrWhiteSpace(textBox6.Text)))
            {
                SqlCommand command = new SqlCommand("DELETE FROM [PRODUCTS] WHERE [ID]=@id", sql);
                command.Parameters.AddWithValue("ID", Convert.ToInt16(textBox6.Text));//откуда получать ID
                await command.ExecuteNonQueryAsync(); // запус ассинхронной команды 
            }
            else
            {
                label6.Visible = true;
                label6.Text = "Нет ID";
            } 
        }        
    }
}
