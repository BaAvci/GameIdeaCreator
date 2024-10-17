using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameIdeaCreator
{

    public class GameInfo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<string>? Genre { get; set; }
        public bool? Free { get; set; }
        public string? Type { get; set; }
        public int? ParentID { get; set; }
    }
}
