using collectorhubAppWpf.Model;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace collectorhubAppWpf.Model
{
    public class MangaModel : BaseModel
    {
        private long _id;
        private string _title;
        private string _author;
        private long _genreId;
        private int _chapters;
        private bool _completed;

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

        [JsonProperty("author")]
        public string Author
        {
            get { return _author; }
            set { _author = value; OnPropertyChanged(nameof(Author)); }
        }

        [JsonProperty("genreId")]
        public long GenreId
        {
            get { return _genreId; }
            set { _genreId = value; OnPropertyChanged(nameof(GenreId)); }
        }

        [JsonProperty("chapters")]
        public int Chapters
        {
            get { return _chapters; }
            set { _chapters = value; OnPropertyChanged(nameof(Chapters)); }
        }

        [JsonProperty("completed")]
        public bool Completed
        {
            get { return _completed; }
            set { _completed = value; OnPropertyChanged(nameof(Completed)); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
}
