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
#region

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

#endregion

namespace MPTagThat.Core
{
  public class DataGridViewProgressColumn : DataGridViewImageColumn
  {
    public DataGridViewProgressColumn()
    {
      CellTemplate = new DataGridViewProgressCell();
    }
  }
}

namespace MPTagThat.Core
{
  internal class DataGridViewProgressCell : DataGridViewImageCell
  {
    // Used to make custom cell consistent with a DataGridViewImageCell
    private static readonly Image emptyImage;

    static DataGridViewProgressCell()
    {
      emptyImage = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
    }

    public DataGridViewProgressCell()
    {
      ValueType = typeof (int);
    }

    // Method required to make the Progress Cell consistent with the default Image Cell. 
    // The default Image Cell assumes an Image as a value, although the value of the Progress Cell is an int.
    protected override object GetFormattedValue(object value,
                                                int rowIndex, ref DataGridViewCellStyle cellStyle,
                                                TypeConverter valueTypeConverter,
                                                TypeConverter formattedValueTypeConverter,
                                                DataGridViewDataErrorContexts context)
    {
      return emptyImage;
    }

    protected override void Paint(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
                                  DataGridViewElementStates cellState, object value, object formattedValue,
                                  string errorText, DataGridViewCellStyle cellStyle,
                                  DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
    {
      int progressVal = 0;
      if (value != null)
        progressVal = (int)value;

      float percentage = (progressVal / 100.0f);
        // Need to convert to float before division; otherwise C# returns int which is 0 for anything but 100%.
      Brush backColorBrush = new SolidBrush(cellStyle.BackColor);
      Brush foreColorBrush = new SolidBrush(cellStyle.ForeColor);
      // Draws the cell grid
      base.Paint(g, clipBounds, cellBounds,
                 rowIndex, cellState, value, formattedValue, errorText,
                 cellStyle, advancedBorderStyle, (paintParts & ~DataGridViewPaintParts.ContentForeground));
      if (percentage > 0.0)
      {
        // Draw the progress bar and the text
        g.FillRectangle(new SolidBrush(Color.FromArgb(163, 189, 242)), cellBounds.X + 2, cellBounds.Y + 2,
                        Convert.ToInt32((percentage * cellBounds.Width - 4)), cellBounds.Height - 4);
        g.DrawString(progressVal + "%", cellStyle.Font, foreColorBrush, cellBounds.X + 6, cellBounds.Y + 2);
      }
      else
      {
        // draw the text
        if (DataGridView.CurrentRow.Index == rowIndex)
          g.DrawString(progressVal + "%", cellStyle.Font, new SolidBrush(cellStyle.SelectionForeColor), cellBounds.X + 6,
                       cellBounds.Y + 2);
        else
          g.DrawString(progressVal + "%", cellStyle.Font, foreColorBrush, cellBounds.X + 6, cellBounds.Y + 2);
      }
    }
  }
}