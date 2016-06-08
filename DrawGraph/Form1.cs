using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace DrawGraph
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        public Form1(List<double> IndividMax, List<double> IndividMiddle)
        {
            InitializeComponent();

            GraphPane pane = zedGraph.GraphPane; /// Получим панель для рисования.
            pane.CurveList.Clear(); /// Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы.
            PointPairList listmax = new PointPairList(); /// Создадим список точек из максимально приспособленных особей поколений.
            PointPairList listmiddle = new PointPairList(); /// Создадим список точек из особей с средней преспособленностью поколений.
            for (int i = 0; i < IndividMax.Count; i++)
            {
                listmax.Add(i, IndividMax[i]);
                listmiddle.Add(i, IndividMiddle[i]);
            }

            /// Создадим кривую с названием "Sinc", 
            /// которая будет рисоваться голубым цветом (Color.Blue),
            /// Опорные точки выделяться не будут (SymbolType.None)
            LineItem myCurveMax = pane.AddCurve("Функция лидера", listmax, Color.Red, SymbolType.None);
            LineItem myCurveMin = pane.AddCurve("Средняя фукнция", listmiddle, Color.Blue, SymbolType.None);

            /// Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            /// В противном случае на рисунке будет показана только часть графика, 
            /// которая умещается в интервалы по осям, установленные по умолчанию.
            zedGraph.AxisChange();

            /// Обновляем график.
            zedGraph.Invalidate();
            this.Show();
        }
    }
}
