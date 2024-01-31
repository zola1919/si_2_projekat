using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntityLib
{
    public class Predmet
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<Pitanje> PredmetnaPitanja { get; set; } = new List<Pitanje>();
        public ICollection<Oblast> OblastPoPredmetima { get; set; } = new List<Oblast>();

    }
}
