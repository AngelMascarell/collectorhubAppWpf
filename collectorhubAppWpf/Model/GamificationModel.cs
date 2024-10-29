using collectorhubAppWpf.Model;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace collectorhubAppWpf.Model
{
    public class GamificationModel : BaseModel
    {
        private long _id;
        private string _title;
        private string _description;
        private string _imageUrl;
        private List<GamificationConditionModel> _conditions;

        public BitmapImage ImageSource { get; set; }

        [JsonProperty("id")]
        public long Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        [JsonProperty("title")]
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        [JsonProperty("description")]
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; OnPropertyChanged(nameof(ImageUrl)); }
        }

        [JsonProperty("conditions")]
        public List<GamificationConditionModel> Conditions
        {
            get { return _conditions; }
            set { _conditions = value; OnPropertyChanged(nameof(Conditions)); }
        }
    }
}
