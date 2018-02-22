
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Jurassic.Sooil.IServiceBase
{
    public class Spatial
    {
        public Spatial()
        {
            type = string.Empty;
            coordinates = new List<Polygon>();
        }
        public Spatial(string type, List<Polygon> coordinates)
        {
            this.type = type;
            this.coordinates =coordinates;
        }
        public string type { get; set; }
        public List<Polygon> coordinates { get; set; }
    }
    
}
