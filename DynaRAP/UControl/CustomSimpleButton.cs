using DevExpress.Skins;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.UControl
{
    public class CustomSimpleButton : SimpleButton
    {
        protected override BaseStyleControlViewInfo CreateViewInfo()
        {
            var sd = base.CreateViewInfo();
            return new CustomSimpleButtonViewInfo(this);//sd;
        }
    }

    public class CustomSimpleButtonViewInfo : SimpleButtonViewInfo
    {
        public CustomSimpleButtonViewInfo(SimpleButton owner) : base(owner)
        {

        }

        protected override EditorButtonPainter GetButtonPainter()
        {
            return new CustomSkinEditorButtonPainter(this.LookAndFeel);
        }
    }

    public class CustomSkinEditorButtonPainter : SkinEditorButtonPainter
    {
        public CustomSkinEditorButtonPainter(ISkinProvider provider) : base(provider)
        {

        }

        public override void DrawObject(ObjectInfoArgs e)
        {
            base.DrawObject(e);
        }

        GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.AddArc(arc, 180, 90);
            
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        int CalcRadius(Rectangle rect)
        {
            if (rect.Width > rect.Height)
                return rect.Height / 2;
            else
                return rect.Width / 2; 
        }

        protected override void DrawFocusRect(EditorButtonObjectInfoArgs e, Rectangle rect)
        {
           // base.DrawFocusRect(e, rect);
        }

        Color GetStateColor(ObjectState state)          
        {
            switch (state)
            {
                case ObjectState.Normal:
                    return Color.FromArgb(28, 171, 238);
                case ObjectState.Pressed:
                    return Color.FromArgb(68, 171, 238);
                case ObjectState.Selected:
                    return Color.FromArgb(48, 200, 238);
                case ObjectState.Hot:
                    return Color.FromArgb(48, 200, 238);
                case ObjectState.Disabled:
                    return Color.FromArgb(28, 171, 238);

                case ObjectState.Hot | ObjectState.Selected:
                     return Color.FromArgb(48, 200, 238);
                case ObjectState.Hot | ObjectState.Pressed | ObjectState.Selected:
                    return Color.FromArgb(148, 231, 150);
            }
            return Color.Empty;
        }

        protected override void DrawButton(ObjectInfoArgs e)
        {
            var roundedRect = GetRoundedRect(e.Bounds, CalcRadius(e.Bounds));
            Console.WriteLine(e.State);
            SolidBrush brush = new SolidBrush(GetStateColor(e.State));
            e.Cache.FillPath(brush, roundedRect);
            brush.Dispose();
        }
    }
}
