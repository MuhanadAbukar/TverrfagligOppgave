using BusinessClasses;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DatabaseLayer
{
    public class DBL
    {
        private static string connstr = "Data Source=itaserver;Initial Catalog=MuhanadOppgaveSkrive;Persist Security Info=True;User ID=muhanadharvester;Password=muhanad123";
        private static SqlConnection conn = new SqlConnection(connstr);
       
        public Text GetAllText(string id)
        {
            var cmd = new SqlCommand("SELECT Paragraph FROM Paragraphs WHERE TextId = @id", conn);
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
            cmd = new SqlCommand("SELECT Id, Header FROM Tekst WHERE Id = @id", conn);
            reader = cmd.ExecuteReader();
            var text = new Text();
            if (reader.Read())
            {
                text.Id = (int)reader["Id"];
                text.Header = (string)reader["Header"];
                text.Paragraph = paragraphs.ToArray();
            }

            reader.Close();
            conn.Close();
            return text;
        }
        public int CreateNewText(string Header, string Paragraph)
        {
            var id = 0;
            var cmd = new SqlCommand("insert into tekst (header, paragraph) output inserted.id values (@header, @paragraph)", conn);
            cmd.Parameters.AddWithValue("header", Header);
            cmd.Parameters.AddWithValue("paragraph", Paragraph);
            conn.Open();
            id = (int)cmd.ExecuteScalar();
            conn.Close();
            return id;
        }

        public void UpdateTextEntry(string textid, string header, string paragraph)
        {
            var cmd = new SqlCommand("update tekst set header=@header,paragraph=@paragraph where id=@id", conn);
            cmd.Parameters.AddWithValue("header", header);
            cmd.Parameters.AddWithValue("id", textid);
            cmd.Parameters.AddWithValue("paragraph", paragraph);
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
