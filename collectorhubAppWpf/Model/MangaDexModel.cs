using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace collectorhubAppWpf.Model
{
    public class MangaDexModel : BaseModel
    {
        private long _id;
        private string _title;
        private string _author;
        private long _genreId;
        private int _chapters;
        private bool _completed;

        private string _imageUrl;

        private string _synopsis;
        private DateTime? _releaseDate;

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; OnPropertyChanged(nameof(ImageUrl)); }
        }

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

        [JsonProperty("synopsis")]
        public string Synopsis
        {
            get { return _synopsis; }
            set { _synopsis = value; OnPropertyChanged(nameof(Synopsis)); }
        }

        [JsonProperty("releasedate")]
        public DateTime? ReleaseDate
        {
            get { return _releaseDate; }
            set { _releaseDate = value; OnPropertyChanged(nameof(ReleaseDate)); }
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
