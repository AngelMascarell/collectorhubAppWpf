using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace collectorhubAppWpf.Model
{
    internal class MangaListModel : BaseModel
    {
        private long _id;

        private string _name;
        private string _description;
        private List<MangaModel> _mangas;

        [JsonProperty("id")]
        public long Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        [JsonProperty("description")]
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        [JsonProperty("mangas")]
        public List<MangaModel> Mangas
        {
            get { return _mangas; }
            set { _mangas = value; OnPropertyChanged(nameof(Mangas)); }
        }


    }
}
