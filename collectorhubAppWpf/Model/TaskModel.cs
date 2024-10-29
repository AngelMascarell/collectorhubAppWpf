using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace collectorhubAppWpf.Model
{
    public class TaskModel : BaseModel
    {
        private Guid _id;
        private string _title;
        private string _description;
        private bool _isCompleted;
        private string _taskType;

        [JsonProperty("id")]
        public Guid Id
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

        [JsonProperty("isCompleted")]
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set { _isCompleted = value; OnPropertyChanged(nameof(IsCompleted)); }
        }

        [JsonProperty("taskType")]
        public string TaskType
        {
            get => _taskType;
            set { _taskType = value; OnPropertyChanged(nameof(TaskType)); }
        }
    }
}
