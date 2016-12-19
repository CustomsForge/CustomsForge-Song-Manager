﻿/*
 *  Copyright (C) 2012 Atomic Wasteland
 *  http://www.atomicwasteland.co.uk/
 *
 *  This Program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 */

using System.Windows.Forms;

namespace CustomControls
{
    public partial class Seperator : UserControl
    {
        public Seperator()
        {
            InitializeComponent();
        }

        public string Label
        {
            // Returns the lavel text
            get
            {
                return textLabel.Text;
            }
            // Changes the label text to the text specified
            set
            {
                textLabel.Text = value;
            }
        }
    }
}
