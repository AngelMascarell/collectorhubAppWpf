using collectorhubAppWpf.Model;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace collectorhubAppWpf.Model
{
    public class RateModel : BaseModel
    {
        private Guid _id;
        private Guid _userId;
        private Guid _mangaId;
        private int _rate;
        private string _comment;
        private DateTime _date;

        [JsonProperty("id")]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        [JsonProperty("userId")]
        public Guid UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged(nameof(UserId)); }
        }

        [JsonProperty("mangaId")]
        public Guid MangaId
        {
            get { return _mangaId; }
            set { _mangaId = value; OnPropertyChanged(nameof(MangaId)); }
        }

        [JsonProperty("rate")]
        public int Rate
        {
            get { return _rate; }
            set { _rate = value; OnPropertyChanged(nameof(Rate)); }
        }

        [JsonProperty("comment")]
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; OnPropertyChanged(nameof(Comment)); }
        }

        [JsonProperty("date")]
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(nameof(Date)); }
        }
    }
}
