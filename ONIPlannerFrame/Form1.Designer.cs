namespace ONIPlannerFrame
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.oniPlannerSubControl1 = new ONIPlannerControl.ONIPlannerSubControl();
            this.SuspendLayout();
            // 
            // oniPlannerSubControl1
            // 
            this.oniPlannerSubControl1.CellSize = ((uint)(103u));
            this.oniPlannerSubControl1.DefaultBorderColour = System.Drawing.Color.Orange;
            this.oniPlannerSubControl1.Draws = true;
            this.oniPlannerSubControl1.DrawsBottom = true;
            this.oniPlannerSubControl1.DrawsImage = true;
            this.oniPlannerSubControl1.DrawsLeft = true;
            this.oniPlannerSubControl1.DrawsRight = true;
            this.oniPlannerSubControl1.DrawsTop = true;
            this.oniPlannerSubControl1.ForceScaling = true;
            this.oniPlannerSubControl1.HoveredBorderColour = System.Drawing.Color.SeaGreen;
            this.oniPlannerSubControl1.Location = new System.Drawing.Point(85, 80);
            this.oniPlannerSubControl1.Name = "oniPlannerSubControl1";
            this.oniPlannerSubControl1.NeedsRedraw = false;
            this.oniPlannerSubControl1.OriginalImage = global::ONIPlannerFrame.Properties.Resources.ljunction;
            this.oniPlannerSubControl1.OriginalImageRotation = System.Drawing.RotateFlipType.Rotate90FlipXY;
            this.oniPlannerSubControl1.SelectedBorderColour = System.Drawing.Color.DarkSeaGreen;
            this.oniPlannerSubControl1.Size = new System.Drawing.Size(103, 103);
            this.oniPlannerSubControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 261);
            this.Controls.Add(this.oniPlannerSubControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private ONIPlannerControl.ONIPlannerSubControl oniPlannerSubControl1;
    }
}

