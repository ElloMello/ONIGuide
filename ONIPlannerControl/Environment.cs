using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONIPlannerControl {
    public sealed class Environment {
        public static readonly uint DefaultGridWidth = 8;
        public static readonly uint DefaultGridHeight = 6;

        public static readonly uint DefaultCellSize = 103;

        public static readonly Color DefaultCellBorderColour = Color.Orange;
        public static readonly Color DefaultCellHoveredColour = Color.SeaGreen;
        public static readonly Color DefaultCellSelectedColour = Color.DarkSeaGreen;
    }
}
