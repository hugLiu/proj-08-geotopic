using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Jurassic.Sooil.IServiceBase
{
    public class FilterCollection : Collection<IList<Filter>>
    {
        public FilterCollection()
            : base()
        { }
    }

    public class Filter
    {
        public string PropertyName { get; set; }
        public Op Operation { get; set; }
        public object Value { get; set; }
    }

    public enum Op
    {
        Equals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        StartsWith,
        EndsWith
    }
}
