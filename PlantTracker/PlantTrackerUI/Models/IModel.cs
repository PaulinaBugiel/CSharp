using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantTrackerUI.Models
{
    public interface IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public static class ModelExtension
    {
        public static string ToString(this IEnumerable<IModel> models, char separator)
        {
            string ret = "";
            foreach (var cont in models)
            {
                ret += $"{cont.Name}{separator} ";
            }
            ret = ret.Trim(' ');
            ret = ret.Trim(separator);
            return ret;
        }
    }
}
