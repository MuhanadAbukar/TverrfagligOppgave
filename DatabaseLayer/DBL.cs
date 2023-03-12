using BusinessClasses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DatabaseLayer
{
    public class DBL
    {
        private static string connstr = "Data Source=itaserver;Initial Catalog=MuhanadOppgaveSkrive;Persist Security Info=True;User ID=muhanadharvester;Password=muhanad123";
        private static SqlConnection conn = new SqlConnection(connstr);
        public List<Text> GetAllHeaders()
        {
            var cmd = new SqlCommand("SELECT * from Tekst", conn);
            conn.Open();
            var Text = new List<Text>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var txt = new Text();
                txt.Header = reader["Header"].ToString();
                txt.Id = reader["Id"].ToString();
                Text.Add(txt);
            }
            reader.Close();
            conn.Close();
            return Text;
        }
        public List<string> GetWebhooks()
        {
            var cmd = new SqlCommand("Select * from LogLinks", conn);
            conn.Open();
            var l = new List<string>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                l.Add(reader.GetString(0));
            }
            conn.Close();
            return l;
        }
        public List<Paragraphs> GetAllText(string id)
        {
            var cmd = new SqlCommand("SELECT Paragraph FROM Paragraphs WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            conn.Open();
            var reader = cmd.ExecuteReader();
            var paragraphs = new List<Paragraphs>();
            while (reader.Read())
            {
                var paragraph = new Paragraphs
                {
                    Text = (string)reader["Paragraph"]
                };
                paragraphs.Add(paragraph);
            }
            reader.Close();
            conn.Close();
            return paragraphs;
        }
        public string CreateNewText(Text text)
        {
            var cmd = new SqlCommand("insert into tekst  output inserted.id values (@id,@header)", conn);
            cmd.Parameters.AddWithValue("@header", text.Header);
            cmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());

            conn.Open();
            var tekstId = cmd.ExecuteScalar().ToString();
            conn.Close();
            cmd = new SqlCommand("insert into paragraphs (id, paragraph) values (@id, @paragraph)", conn);
            conn.Open();
            foreach (var paragraph in text.Paragraph)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id", tekstId);
                cmd.Parameters.AddWithValue("@paragraph", paragraph.Text);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
            return tekstId;
        }


        public void UpdateTextEntry(string textid, Text txt)
        {
            

            var cmd = new SqlCommand("delete from paragraphs where id=@id", conn);
            cmd.Parameters.AddWithValue("id", textid);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            cmd.Parameters.Clear();
            conn.Open();
            foreach (Paragraphs pgh in txt.Paragraph)
            {
                cmd = new SqlCommand("insert into Paragraphs values(@id,@text)", conn);
                cmd.Parameters.AddWithValue("id", textid);
                cmd.Parameters.AddWithValue("text", pgh.Text);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            conn.Close();
            cmd.Parameters.Clear();
            cmd = new SqlCommand("update tekst set header = @header where id=@id", conn);
            cmd.Parameters.AddWithValue("header", txt.Header);
            cmd.Parameters.AddWithValue("id", textid);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

        }
        public void DeleteTextEntry(string textid)
        {
            var cmd = new SqlCommand("delete from tekst where id = @id", conn);
            cmd.Parameters.AddWithValue("id", textid);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public int CheckIfPasswordAndUsernameMatch(string username, string password)
        {
            var command = new SqlCommand("SELECT Permissions AS cnt FROM admin WHERE username = @username and passwordadmin = @password", conn);
            command.Parameters.AddWithValue("username", username);
            command.Parameters.AddWithValue("password", password);
            conn.Open();
            var reader = command.ExecuteReader();
            var permissiontype = 0;
            if (reader.Read())
            {
                permissiontype = reader.GetInt32(0);
            }
            conn.Close();
            if (permissiontype == 0)
                return 0;
            return permissiontype;
        }
        public bool SavePasswordRecoveryLink(string code, string email)
        {
            var cmd = new SqlCommand("SELECT COUNT(*) FROM passwordreset WHERE email = @email", conn);
            cmd.Parameters.AddWithValue("email", email);
            conn.Open();
            int count = (int)cmd.ExecuteScalar();
            conn.Close();
            if (count > 0)
            {
                return true;
            }
            cmd = new SqlCommand("INSERT INTO passwordreset VALUES (@code, @email)", conn);
            cmd.Parameters.AddWithValue("code", code);
            cmd.Parameters.AddWithValue("email", email);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            return false;

        }
        public bool exists(string link)
        {
            var command = new SqlCommand("SELECT COUNT(*) AS cnt FROM passwordreset WHERE PasswordResetLink = @link", conn);
            command.Parameters.AddWithValue("link", link);
            conn.Open();
            var reader = command.ExecuteReader();
            int count = 0;
            if (reader.Read())
            {
                count = reader.GetInt32(0);
            }
            conn.Close();
            return count > 0;

        }
        public void UpdatePasswordById(string id, string newpass)
        {
            var cmd = new SqlCommand("UPDATE admin SET passwordadmin = @newpass WHERE admin.email IN ( SELECT admin.email FROM admin  JOIN passwordreset ON admin.email = passwordreset.email  WHERE passwordreset.PasswordResetLink = @id); Delete from passwordreset where passwordresetlink=@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("newpass", newpass);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
