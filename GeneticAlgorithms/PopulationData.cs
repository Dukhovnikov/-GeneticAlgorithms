using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    /// <summary>
    /// Класс, который хранит данные о поведении генетического алгоритма.
    /// </summary>
    class PopulationData
    {
        public List<double> MinValues { get; set; } = new List<double>();
        public List<double> MiddleValues { get; set; } = new List<double>();

    }
}
