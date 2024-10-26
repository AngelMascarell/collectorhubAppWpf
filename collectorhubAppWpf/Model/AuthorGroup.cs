using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace collectorhubAppWpf.Model
{
    internal class AuthorGroup : BaseModel
    {
        public string Author { get; set; }
        public List<MangaModel> Mangas { get; set; }
    }
}
