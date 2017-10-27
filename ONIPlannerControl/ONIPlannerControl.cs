using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ONIPlannerControl {
    #region class ONIPlannerControl 

    public class ONIPlannerControl : UserControl {
        #region InternalVariables

        private ONIPlannerSubControl[,] cellGrid;
        private Point activeCellCoordinates;
    
        #endregion


        #region Accessors

        public ONIPlannerSubControl[,] CellGrid { get { return cellGrid; } protected set { cellGrid = value; ReformatCellGrid((UInt16)value.GetLength(0), (UInt16)value.GetLength(1), true); } }
        public Point ActiveCellCoordinates { get { return activeCellCoordinates; } protected set { if (value.X < CellGrid.GetLength(0) && value.Y < CellGrid.GetLength(1)) { activeCellCoordinates = value; } } }

        #endregion


        #region Interpreters 

        public UInt16 CellGridWidth { get { return (UInt16)CellGrid.GetLength(0); } set { ReformatCellGrid(value, (UInt16)CellGrid.GetLength(1), false); } }
        public UInt16 CellGridHeight { get { return (UInt16)CellGrid.GetLength(1); } set { ReformatCellGrid((UInt16)CellGrid.GetLength(0), value, false); } }

        public ONIPlannerSubControl GetCellAt(UInt16 x, UInt16 y) {
            if(x > CellGrid.GetLength(0)) {
                throw new ArgumentOutOfRangeException("X", x, "X value given is higher than the number of columns!");
            }

            if (y > CellGrid.GetLength(1)) {
                throw new ArgumentOutOfRangeException("Y", y, "Y value given is higher than the number of rows!");
            }

            return CellGrid[x, y];
        }

        #endregion


        #region Constructors

        #endregion


        #region Methods

        public void ReformatCellGrid(UInt16 newWidth, UInt16 newHeight, bool forceRedraw) {
            ONIPlannerSubControl[,] newGrid = new ONIPlannerSubControl[newWidth, newHeight];

            if (newWidth == CellGrid.GetLength(0) && newHeight == CellGrid.GetLength(1)) {
                return;
            }

            if (CellGrid == null) {
                newGrid.Initialize();
            }

            else {
                for (int x = 0; x < newWidth; x++) {
                    for (int y = 0; y < newHeight; y++) {
                        if (CellGrid.GetLength(0) > newWidth || CellGrid.GetLength(1) > newHeight) {
                             newGrid[x, y] = new ONIPlannerSubControl();
                        }

                        else {
                            newGrid[x, y] = CellGrid[x, y];
                        }
                    }
                }
            }

            CellGrid = newGrid;

            if (forceRedraw) {
                for (int x = 0; x < newWidth; x++) {
                    for (int y = 0; y < newHeight; y++) {
                        CellGrid[x, y].NeedsRedraw = true;
                        CellGrid[x, y].Refresh();
                    }
                }
            }
        }

        #endregion


        #region OverrideMethods

        protected override void OnPaint(PaintEventArgs paintEventArgs) {
        }

        protected override void OnLoad(EventArgs eventArgs) {
        }

        #endregion
    }

    #endregion


    #region class ONIPlannerSubControl

    public class ONIPlannerSubControl : UserControl {
        #region InternalVariables

        private uint cellSize;
        private Image originalImage;
        private Image rotatedImage;
        private RotateFlipType originalImageRotation;

        private bool draws;
        private bool drawsImage;
        private bool needsRedraw;
        private bool forceScaling;
        private bool isHoveredOver;
        private bool isSelected;

        private Color defaultBorderColour;
        private Color hoveredBorderColour;
        private Color selectedBorderColour;

        private bool drawsTop;
        private bool drawsRight;
        private bool drawsBottom;
        private bool drawsLeft;

        #endregion


        #region Accessors

        [Browsable(true)]
        [Category("Planner Cell")]
        [Description("The size (width and height) of a cell.")]
        public uint CellSize { get { return cellSize; } set { cellSize = value; base.Width = (int) value; base.Height = (int) value; } }
        [Browsable(true)]
        [Category("Planner Cell")]
        [Description("The image that is rotated and then drawn onto the cell.")]
        public Image OriginalImage { get { return originalImage; } set { originalImage = value; } }
        [Browsable(false)]
        [DefaultValue(null)]
        public Image RotatedImage { get { return rotatedImage; } protected set { rotatedImage = value; } }
        [Browsable(true)]
        [Category("Planner Cell")]
        [Description("The rotation transformation for the original image.")]
        public RotateFlipType OriginalImageRotation { get { return originalImageRotation; } set { if (rotatedImage != null) { rotatedImage = (Image)originalImage.Clone(); rotatedImage.RotateFlip(value); } else { rotatedImage = null; } originalImageRotation = value; } }

        [Browsable(true)]
        [Category("Planner Cell")]
        [Description("Whether the cell draws its contents.")]
        public bool Draws { get { return draws; } set { draws = value; } }
        [Browsable(true)]
        [Category("Planner Cell")]
        [Description("Whether the cell draws its image.")]
        public bool DrawsImage { get { return drawsImage; } set { drawsImage = value; } }
        [Browsable(false)]
        [DefaultValue(true)]
        public bool NeedsRedraw { get { return needsRedraw; } set { needsRedraw = value; } }
        [Browsable(true)]
        [Category("Planner Cell")]
        [Description("Whether the cell forcibly scales its image before drawing.")]
        public bool ForceScaling { get { return forceScaling; } set { forceScaling = value; } }
        [Browsable(false)]
        [DefaultValue(false)]
        public bool IsHoveredOver { get { return isHoveredOver; } set { isHoveredOver = value; isSelected = false; } }
        [Browsable(false)]
        [DefaultValue(false)]
        public bool IsSelected { get { return isSelected; } set { isSelected = value; isHoveredOver = false; } }

        [Browsable(true)]
        [Category("Planner Cell")]
        [Description("The cell's border's colour when not hovered over or selected.")]
        public Color DefaultBorderColour { get { return defaultBorderColour; } set { defaultBorderColour = value; } }
        [Browsable(true)]
        [Category("Planner Cell")]
        [Description("The cell's border's colour when hovered over.")]
        public Color HoveredBorderColour { get { return hoveredBorderColour; } set { hoveredBorderColour = value; } }
        [Browsable(true)]
        [Category("Planner Cell")]
        [Description("The cell's border's colour when the cell is selected.")]
        public Color SelectedBorderColour { get { return selectedBorderColour; } set { selectedBorderColour = value; } }

        [Browsable(true)]
        [Category("Planner Cell")]
        [Description("Whether the cell draws the top of its border.")]
        public bool DrawsTop { get { return drawsTop; } set { drawsTop = value; } }
        [Browsable(true)]
        [Category("Planner Cell")]
        [Description("Whether the cell draws the right of its border.")]
        public bool DrawsRight { get { return drawsRight; } set { drawsRight = value; } }
        [Browsable(true)]
        [Category("Planner Cell")]
        [Description("Whether the cell draws the bottom of its border.")]
        public bool DrawsBottom { get { return drawsBottom; } set { drawsBottom = value; } }
        [Browsable(true)]
        [Category("Planner Cell")]
        [Description("Whether the cell draws the left of its border.")]
        public bool DrawsLeft { get { return drawsLeft; } set { drawsLeft = value; } }

        #endregion


        #region Constructors

        public ONIPlannerSubControl() {
            CellSize = Environment.DefaultCellSize;
            OriginalImage = null;
            RotatedImage = null;
            OriginalImageRotation = RotateFlipType.RotateNoneFlipNone;

            Draws = true;
            DrawsImage = true;
            NeedsRedraw = true;
            ForceScaling = true;
            IsHoveredOver = false;
            IsSelected = false;

            DefaultBorderColour = Environment.DefaultCellBorderColour;
            HoveredBorderColour = Environment.DefaultCellHoveredColour;
            SelectedBorderColour = Environment.DefaultCellSelectedColour;

            DrawsTop = true;
            DrawsRight = true;
            DrawsBottom = true;
            DrawsLeft = true;
        }

        public ONIPlannerSubControl(uint cellSize) {
            if(cellSize == 0) {
                throw new ArgumentOutOfRangeException("cellSize", cellSize, "Cell Size cannot be 0!");
            }

            CellSize = cellSize;
            OriginalImage = null;
            RotatedImage = null;
            OriginalImageRotation = RotateFlipType.RotateNoneFlipNone;

            Draws = true;
            DrawsImage = true;
            NeedsRedraw = true;
            ForceScaling = true;
            IsHoveredOver = false;
            IsSelected = false;

            DefaultBorderColour = Environment.DefaultCellBorderColour;
            HoveredBorderColour = Environment.DefaultCellHoveredColour;
            SelectedBorderColour = Environment.DefaultCellSelectedColour;

            DrawsTop = true;
            DrawsRight = true;
            DrawsBottom = true;
            DrawsLeft = true;
        }

        public ONIPlannerSubControl(uint cellSize, Image originalImage, RotateFlipType imageRotation) {
            if (cellSize == 0) {
                throw new ArgumentOutOfRangeException("cellSize", cellSize, "Cell Size cannot be 0!");
            }

            CellSize = cellSize;
            OriginalImage = originalImage;
            RotatedImage = originalImage;
            OriginalImageRotation = imageRotation;

            Draws = true;
            DrawsImage = true;
            NeedsRedraw = true;
            ForceScaling = true;
            IsHoveredOver = false;
            IsSelected = false;

            if (originalImage != null) {
                RotatedImage.RotateFlip(imageRotation);
            }

            DefaultBorderColour = Environment.DefaultCellBorderColour;
            HoveredBorderColour = Environment.DefaultCellHoveredColour;
            SelectedBorderColour = Environment.DefaultCellSelectedColour;

            DrawsTop = true;
            DrawsRight = true;
            DrawsBottom = true;
            DrawsLeft = true;
        }

        public ONIPlannerSubControl(uint cellSize, Image originalImage, RotateFlipType imageRotation, bool draws, bool drawsImage, bool needsRedraw, bool forceScaling) {
            if (cellSize == 0)  {
                throw new ArgumentOutOfRangeException("cellSize", cellSize, "Cell Size cannot be 0!");
            }

            CellSize = cellSize;
            OriginalImage = originalImage;
            RotatedImage = originalImage;
            OriginalImageRotation = imageRotation;

            Draws = draws;
            DrawsImage = drawsImage;
            NeedsRedraw = needsRedraw;
            ForceScaling = forceScaling;
            IsHoveredOver = false;
            IsSelected = false;

            if (originalImage != null) {
                RotatedImage.RotateFlip(imageRotation);
            }

            DefaultBorderColour = Environment.DefaultCellBorderColour;
            HoveredBorderColour = Environment.DefaultCellHoveredColour;
            SelectedBorderColour = Environment.DefaultCellSelectedColour;

            DrawsTop = true;
            DrawsRight = true;
            DrawsBottom = true;
            DrawsLeft = true;
        }

        public ONIPlannerSubControl(uint cellSize, Image originalImage, RotateFlipType imageRotation, bool draws, bool drawsImage, bool needsRedraw, bool forceScaling, bool isHoveredOver, bool isSelected) {
            if (cellSize == 0)  {
                throw new ArgumentOutOfRangeException("cellSize", cellSize, "Cell Size cannot be 0!");
            }

            CellSize = cellSize;
            OriginalImage = originalImage;
            RotatedImage = originalImage;
            OriginalImageRotation = imageRotation;

            Draws = draws;
            DrawsImage = drawsImage;
            NeedsRedraw = needsRedraw;
            ForceScaling = forceScaling;
            IsHoveredOver = isHoveredOver;
            IsSelected = isSelected;

            if (originalImage != null) {
                RotatedImage.RotateFlip(imageRotation);
            }

            DefaultBorderColour = Environment.DefaultCellBorderColour;
            HoveredBorderColour = Environment.DefaultCellHoveredColour;
            SelectedBorderColour = Environment.DefaultCellSelectedColour;

            DrawsTop = true;
            DrawsRight = true;
            DrawsBottom = true;
            DrawsLeft = true;
        }

        public ONIPlannerSubControl(uint cellSize, Image originalImage, RotateFlipType imageRotation, bool draws, bool drawsImage, bool needsRedraw, bool forceScaling, bool isHoveredOver, bool isSelected, Color defaultBorderColour, Color hoveredBorderColour, Color selectedBorderColour) {
            if (cellSize == 0) {
                throw new ArgumentOutOfRangeException("cellSize", cellSize, "Cell Size cannot be 0!");
            }

            CellSize = cellSize;
            OriginalImage = originalImage;
            RotatedImage = originalImage;
            OriginalImageRotation = imageRotation;

            Draws = draws;
            DrawsImage = drawsImage;
            NeedsRedraw = needsRedraw;
            ForceScaling = forceScaling;
            IsHoveredOver = isHoveredOver;
            IsSelected = isSelected;

            if (originalImage != null) {
                RotatedImage.RotateFlip(imageRotation);
            }

            DefaultBorderColour = defaultBorderColour;
            HoveredBorderColour = hoveredBorderColour;
            SelectedBorderColour = selectedBorderColour;

            DrawsTop = true;
            DrawsRight = true;
            DrawsBottom = true;
            DrawsLeft = true;
        }

        public ONIPlannerSubControl(uint cellSize, Image originalImage, RotateFlipType imageRotation, bool draws, bool drawsImage, bool needsRedraw, bool forceScaling, bool isHoveredOver, bool isSelected, Color defaultBorderColour, Color hoveredBorderColour, Color selectedBorderColour, bool drawsTop, bool drawsRight, bool drawsBottom, bool drawsLeft) {
            if (cellSize == 0) {
                throw new ArgumentOutOfRangeException("cellSize", cellSize, "Cell Size cannot be 0!");
            }

            CellSize = cellSize;
            OriginalImage = originalImage;
            RotatedImage = originalImage;
            OriginalImageRotation = imageRotation;

            Draws = draws;
            DrawsImage = drawsImage;
            NeedsRedraw = needsRedraw;
            ForceScaling = forceScaling;
            IsHoveredOver = isHoveredOver;
            IsSelected = isSelected;

            if (originalImage != null) {
                RotatedImage.RotateFlip(imageRotation);
            }

            DefaultBorderColour = defaultBorderColour;
            HoveredBorderColour = hoveredBorderColour;
            SelectedBorderColour = selectedBorderColour;

            DrawsTop = drawsTop;
            DrawsRight = drawsRight;
            DrawsBottom = drawsBottom;
            DrawsLeft = drawsLeft;
        }

        #endregion


        #region OverrideMethods

        protected override void OnPaint(PaintEventArgs paintEventArgs) {
            Graphics graphics = paintEventArgs.Graphics;

            if (NeedsRedraw)  {
                Pen outlinePen;

                if (IsSelected) {
                    outlinePen = new Pen(SelectedBorderColour);
                }

                else if (IsHoveredOver) {
                    outlinePen = new Pen(HoveredBorderColour);
                }

                else  {
                    outlinePen = new Pen(DefaultBorderColour);
                }

                outlinePen.DashStyle = DashStyle.Dash;
                outlinePen.DashPattern = new float[] { (CellSize * 1f) / 5f, (CellSize * 1f) / 5f };

                if (DrawsTop) {
                    graphics.DrawLine(outlinePen, 0, 0, base.Width, 0);
                }

                if (DrawsRight) {
                     graphics.DrawLine(outlinePen, base.Width - 1, 0, base.Width - 1, base.Height);
                }
                
                if (DrawsBottom) {
                     graphics.DrawLine(outlinePen, base.Width, base.Height - 1, 0, base.Height - 1);
                }
                
                if (DrawsLeft) {
                     graphics.DrawLine(outlinePen, 0, base.Height, 0, 0);
                }

                if (DrawsImage && OriginalImage != null) {
                    if (RotatedImage == null) {
                        RotatedImage = OriginalImage;
                        RotatedImage.RotateFlip(OriginalImageRotation);
                    }

                    if (ForceScaling) {
                        graphics.DrawImage(RotatedImage, 0, 0, Width, Height);
                    }

                    else {
                        graphics.DrawImageUnscaled(RotatedImage, 0, 0);
                    }
                }

                NeedsRedraw = false;
            }
        }

        protected override void OnLoad(EventArgs eventArgs) {
            NeedsRedraw = true;
        }

        protected override void OnResize(EventArgs eventArgs) {
            CellSize = (uint) Math.Min(base.Width, base.Height);
        }

        #endregion
    }

    #endregion
}
