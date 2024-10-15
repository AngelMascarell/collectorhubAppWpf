using collectorhubAppWpf.Model;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace collectorhubAppWpf.Model
{
    public class UserModel : BaseModel
    {
        private long _id;
        private string _username;
        private string _email;
        private string _password;
        private DateTime _birthdate;
        private DateTime _registerDate;
        private List<MangaModel> _mangas;

        [JsonProperty("id")]
        public long Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        [JsonProperty("username")]
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        [JsonProperty("email")]
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        [JsonProperty("birthdate")]
        public DateTime Birthdate
        {
            get { return _birthdate; }
            set { _birthdate = value; OnPropertyChanged(nameof(Birthdate)); }
        }

        [JsonProperty("registerDate")]
        public DateTime RegisterDate
        {
            get { return _registerDate; }
            set { _registerDate = value; OnPropertyChanged(nameof(RegisterDate)); }
        }

        [JsonProperty("password")]
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        [JsonProperty("mangas")]
        public List<MangaModel> Mangas
        {
            get { return _mangas; }
            set { _mangas = value; OnPropertyChanged(nameof(Mangas)); }
        }
    }
}
