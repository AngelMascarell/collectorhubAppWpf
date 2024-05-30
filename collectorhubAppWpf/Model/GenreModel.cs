using collectorhubAppWpf.Model;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Intermodular2DAMGrupoCInterfaces.Models
{
    public class GenreModel : BaseModel
    {
        private Guid _id;
        private string _name;

        [JsonProperty("id")]
        public Guid Id
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
    }
}
