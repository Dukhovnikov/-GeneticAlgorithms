using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    class VisualizationGA
    {
        /// <summary>
        /// Вид кодирования.
        /// </summary>
        public string ViewsCoding { get; set; }
        /// <summary>
        /// Вид скрещивания
        /// </summary>
        public string ViewsСrossing { get; set; }

        public VisualizationGA(string ViewsCoding, string ViewsСrossing)
        {
            this.ViewsCoding = ViewsCoding;
            this.ViewsСrossing = ViewsСrossing;
        }

        /// <summary>
        /// Функция создает Методы и их классификацию.
        /// </summary>
        public static List<VisualizationGA> GetViews()
        {
            return new List<VisualizationGA>
            {
                new VisualizationGA("Вещественное", "Одноточечное скрещивание"),
                new VisualizationGA("Вещественное", "BLXa - скрещивание"),
                new VisualizationGA("Целочисленное", "Одноточечное скрещивание"),
                new VisualizationGA("Целочисленное", "Двухточечное скрещивание"),
                new VisualizationGA("Целочисленное", "Однородное скрещивание"),
            };
        }
    }
}
