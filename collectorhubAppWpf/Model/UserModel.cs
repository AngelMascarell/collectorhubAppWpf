﻿using collectorhubAppWpf.Model;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Intermodular2DAMGrupoCInterfaces.Models
{
    public class UserModel : BaseModel
    {
        private Guid _id;
        private string _username;
        private string _email;
        private DateTime _birthdate;
        private DateTime _registerDate;
        private List<MangaModel> _mangas;

        [JsonProperty("id")]
        public Guid Id
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

        [JsonProperty("mangas")]
        public List<MangaModel> Mangas
        {
            get { return _mangas; }
            set { _mangas = value; OnPropertyChanged(nameof(Mangas)); }
        }
    }
}