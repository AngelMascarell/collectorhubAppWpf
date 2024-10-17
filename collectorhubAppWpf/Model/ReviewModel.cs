using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace collectorhubAppWpf.Model
{
    public class ReviewModel : BaseModel
    {

        private long _userId;
        private int _rate;
        private string _comment;
        private DateTime _date;
        private string _username;

        [JsonProperty("userId")]
        public long UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged(nameof(UserId)); }
        }

        [JsonProperty("username")]
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(nameof(Username)); }
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
