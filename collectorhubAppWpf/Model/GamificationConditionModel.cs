using collectorhubAppWpf.Model;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace collectorhubAppWpf.Model
{
    public class GamificationConditionModel : BaseModel
    {
        private long _id;
        private string _type; // e.g., "task_completed"
        private int _threshold; // e.g., number of tasks to complete

        [JsonProperty("id")]
        public long Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        [JsonProperty("type")]
        public string Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged(nameof(Type)); }
        }

        [JsonProperty("threshold")]
        public int Threshold
        {
            get { return _threshold; }
            set { _threshold = value; OnPropertyChanged(nameof(Threshold)); }
        }
    }
}
