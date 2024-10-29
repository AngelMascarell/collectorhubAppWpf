using collectorhubAppWpf.Model;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace collectorhubAppWpf.Model
{
    public class GamificationConditionModel : BaseModel
    {
        private Guid _id;
        private string _conditionType; // e.g., "task_completed"
        private int _threshold; // e.g., number of tasks to complete

        [JsonProperty("id")]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        [JsonProperty("conditionType")]
        public string ConditionType
        {
            get { return _conditionType; }
            set { _conditionType = value; OnPropertyChanged(nameof(ConditionType)); }
        }

        [JsonProperty("threshold")]
        public int Threshold
        {
            get { return _threshold; }
            set { _threshold = value; OnPropertyChanged(nameof(Threshold)); }
        }
    }
}
