#region Copyright (C) 2009-2011 Team MediaPortal
// Copyright (C) 2009-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MPTagThat is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MPTagThat is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MPTagThat. If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DataGridViewTools.Properties;


namespace DataGridViewTools
{
  public class DataGridViewRatingColumn : DataGridViewImageColumn
  {
    public DataGridViewRatingColumn()
    {
      CellTemplate = new RatingCell();
      DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      ValueType = typeof (int);
    }
  }

  public class RatingCell : DataGridViewImageCell
  {
    #region Variables

    private const int IMAGEWIDTH = 58;
    private static readonly Image[] starImages;
    private static readonly Image[] starHotImages;

    #endregion

    #region ctor

    static RatingCell()
    {
      starImages = new Image[6];
      starHotImages = new Image[6];
      // load normal stars
      for (int i = 0; i <= 5; i++)
        starImages[i] = (Image)Resources.ResourceManager.GetObject("star" + i);

      // load hot normal stars
      for (int i = 0; i <= 5; i++)
        starHotImages[i] = (Image)Resources.ResourceManager.GetObject("starhot" + i);
    }

    public RatingCell()
    {
      // Value type is an integer.
      // Formatted value type is an image since we derive from the ImageCell
      ValueType = typeof (int);
    }

    #endregion

    // setup star images

    public override object DefaultNewRowValue
    {
      // default new row to 0 stars
      get { return 0; }
    }

    protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle,
                                                TypeConverter valueTypeConverter,
                                                TypeConverter formattedValueTypeConverter,
                                                DataGridViewDataErrorContexts context)
    {
      if (value == null)
      {
        return starImages[0];
      }

      // Check for a valid range
      if ((int)value < 0 || (int)value > 5)
        return starImages[0];

      // Convert integer to star images
      return starImages[(int)value];
    }

    protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
                                  DataGridViewElementStates elementState, object value, object formattedValue,
                                  string errorText, DataGridViewCellStyle cellStyle,
                                  DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
    {
      Image cellImage = (Image)formattedValue;

      if (value == null)
        return;

      int starNumber = (int) value;

      if (starNumber > -1 && starNumber < 6)
        cellImage = starHotImages[starNumber];

      //if (DataGridView.Rows[rowIndex].Selected)
      //{
      //  cellStyle.BackColor =
      //    ServiceScope.Get<IThemeManager>().CurrentTheme.SelectionBackColor;

      //}
      //else
      //{
      //  if ((string)this.DataGridView.Rows[rowIndex].Tag == "Changed")
      //  {
      //    cellStyle.BackColor =
      //      ServiceScope.Get<IThemeManager>().CurrentTheme.ChangedBackColor;
      //  }
      //  else
      //  {
      //    if (rowIndex % 2 == 0)
      //      cellStyle.BackColor =
      //        ServiceScope.Get<IThemeManager>().CurrentTheme.DefaultBackColor;
      //    else
      //      cellStyle.BackColor =
      //        ServiceScope.Get<IThemeManager>().CurrentTheme.AlternatingRowBackColor;
      //  }
      //}

      // surpress painting of selection
      base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, cellImage, errorText, cellStyle,
                 advancedBorderStyle, (paintParts & ~DataGridViewPaintParts.SelectionBackground));
    }

    // Update cell's value when the user clicks on a star
    protected override void OnContentClick(DataGridViewCellEventArgs e)
    {
      base.OnContentClick(e);
      int starNumber =
        GetStarFromMouse(
          DataGridView.GetCellDisplayRectangle(DataGridView.CurrentCellAddress.X, DataGridView.CurrentCellAddress.Y,
                                               false), DataGridView.PointToClient(Control.MousePosition));

      if (starNumber != -1)
        Value = starNumber;
    }

    #region Invalidate cells when mouse moves or leaves the cell

    protected override void OnMouseLeave(int rowIndex)
    {
      base.OnMouseLeave(rowIndex);
      DataGridView.InvalidateCell(this);
    }

    protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
    {
      base.OnMouseMove(e);
      DataGridView.InvalidateCell(this);
    }

    #endregion

    #region Private Implementation

    private int GetStarFromMouse(Rectangle cellBounds, Point mouseLocation)
    {
      if (cellBounds.Contains(mouseLocation))
      {
        int mouseXRelativeToCell = (mouseLocation.X - cellBounds.X);
        int imageXArea = (cellBounds.Width / 2) - (IMAGEWIDTH / 2);
        if (((mouseXRelativeToCell + 4) < imageXArea) || (mouseXRelativeToCell >= (imageXArea + IMAGEWIDTH)))
          return -1;
        else
        {
          int oo =
            (int)
            Math.Round((((mouseXRelativeToCell - imageXArea + 5) / (float)IMAGEWIDTH) * 5f),
                       MidpointRounding.AwayFromZero);
          if (oo > 5 || oo < 0) Debugger.Break();
          return oo;
        }
      }
      else
        return -1;
    }

    #endregion
  }
}
