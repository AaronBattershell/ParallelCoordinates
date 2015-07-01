﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using DataReader;

namespace ParallelCordinates
{
    public partial class ParallelCoordinates2D : Window
    {
        // Passed in arguments
        double VariableLineThickness;
        double MinXStep;
        int StartApproximation;
        int MaxUniqueEntries;
        int TotalLineThickness;

        bool HideFiltered;
        double CalculatedXStep;
        double settingsGearHeight = 10;

        const double BORDER_DISTANCE = 50;
        const double NUMERIC_POINTS = 5;
        const double Y_COLUMN_OFFSET = - BORDER_DISTANCE / 2;
        const double TEXT_OFFSET_X = 3;
        const double TEXT_OFFSET_Y = - 25;
        const string EMPTY_FIELD = "[Not Available]";

        int DownMouseColumnIndex = -1;
        int UpMouseColumnIndex = -1;

        List<bool> FilteredDataEntryList;

        Border SettingsGrid;
        Button SettingsBtn;
        DisplayData GraphData;

        // For the purpose of creating line density
        Dictionary<Tuple<Point, Point>, int> PlacementDencity;

        public ParallelCoordinates2D()
        {
            InitializeComponent();
        }

        public ParallelCoordinates2D(List<DataEntry> userData, int minColumnWidth, int beginNumericAprox, int maxUniqueEntries, int totalLineThickness, double variableLineThickness, bool hideFiltered) : this()
        {
            SettingsGrid = defaultSettingsGrid;
            SettingsBtn = defaultSettingsBtn;

            SettingsGrid.Visibility = Visibility.Hidden;

            MinXStep = minColumnWidth;
            StartApproximation = beginNumericAprox;
            MaxUniqueEntries = maxUniqueEntries;
            MaxUniqueEntries = maxUniqueEntries;
            TotalLineThickness = totalLineThickness;
            VariableLineThickness = variableLineThickness;
            HideFiltered = hideFiltered;

            GraphData = new DisplayData();
            GraphData.GridData = userData.Where(e => e.UniquEntries <= MaxUniqueEntries || e.AllNumbers == true).ToList();

            PlacementDencity = new Dictionary<Tuple<Point, Point>, int>();

            string[] unusedColumnNames = userData.Where(e => e.UniquEntries > MaxUniqueEntries && e.AllNumbers == false).Select(e => e.ColumnName).ToArray();

            // Alert user of unusable columns
            if (unusedColumnNames.Length > 0)
            {
                string errorMessage = "The following data column" + (unusedColumnNames.Length > 1 ? "s" : "") + " were removed for having too many unique non-numeric entries: ";
                string badColumns = string.Join(", ", unusedColumnNames);

                MessageBox.Show(errorMessage + badColumns, "Invalid Columns", System.Windows.MessageBoxButton.OK);
            }

            CalculateCanvasDisplayLocations();

            ClearFilters();

            DrawScreen();
        }

        private void SortList(ref List<string> values, bool isNumbers = false)
        {
            if (!isNumbers)
            {
                values.Sort();
            }
            else
            {
                List<double> newValues = new List<double>();
                foreach (var val in values)
                {
                    newValues.Add(double.Parse(val));
                }

                newValues.Sort();

                values = new List<string>();

                foreach (var val in newValues)
                {
                    values.Add(val.ToString());
                }
            }
        }

        private Line DrawLine(Pen p, Point p1, Point p2)
        {
            Line l = new Line()
            {
                X1 = p1.X,
                Y1 = p1.Y,
                X2 = p2.X,
                Y2 = p2.Y,
                StrokeThickness = p.Thickness,
                Stroke = p.Brush
            };

            return l;
        }

        private Rectangle DrawRectangle(Point topLeft, Point bottomRight, SolidColorBrush outline, SolidColorBrush fill)
        {
            Rectangle r = new Rectangle()
            {
                Stroke = outline,
                Fill = fill,
                Width = Convert.ToDouble(Math.Abs(bottomRight.X - topLeft.X)),
                Height = Convert.ToDouble(Math.Abs(bottomRight.Y - topLeft.Y)),
                Margin = new Thickness(topLeft.X, topLeft.Y, 0, 0)
            };

            return r;
        }

        private TextBlock DrawText(string text, Point location, Color color)
        {
            TextBlock t = new TextBlock()
            {
                Text = text,
                Foreground = new SolidColorBrush(color),
                Height = 30,
                Width = 200,
                Margin = new Thickness(location.X, location.Y, 0, 0)
            };

            return t;
        }

        private void mouseDown(object sender, RoutedEventArgs e)
        {
            if (SettingsGrid.IsVisible)
            {
                return;
            }
            
            Point downClick = Mouse.GetPosition(canvas);
            
            DownMouseColumnIndex = GetColumn(downClick);
            //MessageBox.Show("Mouse down on: " + DownMouseColumnIndex, "", System.Windows.MessageBoxButton.OK);

            // Ensure that the column is currently not already selected
            if (GraphData.GridData[DownMouseColumnIndex].FilteredColumn == false /*GraphData.GridData[DownMouseColumnIndex].DisplayFilter.First == -1*/)
            {
                GraphData.GridData[DownMouseColumnIndex].DisplayFilter.First = downClick.Y; 
            }
        }

        private void mouseUP(object sender, RoutedEventArgs e)
        {
            if (SettingsGrid.IsVisible)
            {
                return;
            }

            // Check for column click
            Point upClick = Mouse.GetPosition(canvas);

            int upMouseColumnClick = GetColumn(upClick);
            //MessageBox.Show("Mouse up on: " + upMouseColumnClick, "", System.Windows.MessageBoxButton.OK);

            if (upMouseColumnClick == DownMouseColumnIndex)
            {
                // Add filter for column
                if (/*GraphData.GridData[upMouseColumnClick].DisplayFilter.Second == -1*/ GraphData.GridData[upMouseColumnClick].FilteredColumn == false && Math.Abs(GraphData.GridData[DownMouseColumnIndex].DisplayFilter.First - upClick.Y) >= 5)
                {
                    //MessageBox.Show("Adding Column Filter", "", System.Windows.MessageBoxButton.OK);
                    // Assign lowest to first, heighest to second
                    GraphData.GridData[upMouseColumnClick].DisplayFilter.Second = Math.Max(upClick.Y, GraphData.GridData[upMouseColumnClick].DisplayFilter.First);
                    GraphData.GridData[upMouseColumnClick].DisplayFilter.First = Math.Min(upClick.Y, GraphData.GridData[upMouseColumnClick].DisplayFilter.First);

                    GraphData.GridData[upMouseColumnClick].FilteredColumn = true;
                }
                else // remove data filter for column
                {
                    //MessageBox.Show("Removing Column Filter", "", System.Windows.MessageBoxButton.OK);
                    GraphData.GridData[upMouseColumnClick].FilteredColumn = false;
                    GraphData.GridData[upMouseColumnClick].DisplayFilter.First = -1;
                    GraphData.GridData[upMouseColumnClick].DisplayFilter.Second = -1;
                }

                // Filter or free constraints based upon restrictions
                SetFilters();
                DrawScreen();

                return;
            }

            // Check for column drag
            UpMouseColumnIndex = GetBetweenColumns(upClick);

            if (UpMouseColumnIndex != -1 && DownMouseColumnIndex != -1 && !(UpMouseColumnIndex - 2 < DownMouseColumnIndex && UpMouseColumnIndex + 1 > DownMouseColumnIndex))
            {
                //MessageBox.Show("Column Drag Detected", "", System.Windows.MessageBoxButton.OK);

                var dataHolder = GraphData.GridData[DownMouseColumnIndex];

                GraphData.GridData.RemoveAt(DownMouseColumnIndex);

                if (DownMouseColumnIndex < UpMouseColumnIndex)
                {
                    --UpMouseColumnIndex;
                    GraphData.GridData.Insert(UpMouseColumnIndex, dataHolder);
                }
                else
                {
                    GraphData.GridData.Insert(UpMouseColumnIndex, dataHolder);
                }

                DownMouseColumnIndex = -1;
                DrawScreen();
            }

            UpMouseColumnIndex = -1;
        }

        private void ClearFilters()
        {
            FilteredDataEntryList.ForEach(e => e = false);
            GraphData.GridData.ForEach(e => { e.DisplayFilter.First = -1; e.DisplayFilter.Second = -1; });
        }

        private void SetFilters()
        {
            for (int i = 0; i < FilteredDataEntryList.Count; ++i)
            {
                FilteredDataEntryList[i] = false;
            }

            for (int i = 0; i < GraphData.GridData.Count; ++i)
            {
                if (GraphData.GridData[i].DisplayFilter.First == -1 && GraphData.GridData[i].DisplayFilter.Second == -1)
                {
                    continue;
                }

                for (int j = 0; j < GraphData.GridData[i].Data.Count; ++j)
                {
                    double comparableValueHeight;

                    // Estimate
                    if (GraphData.GridData[i].AllNumbers && GraphData.GridData[i].UniquEntries > StartApproximation)
                    {
                        comparableValueHeight = GetDataPointCoordinates(j, i).Y;
                    }
                    else // Non-estimate
                    {
                        comparableValueHeight = GraphData.GridData[i].YPlacements[GraphData.GridData[i].Data[j]];
                    }

                    // Decide if filter should be placed on data entry based on the existing 'click' variables first and second
                    if (GraphData.GridData[i].DisplayFilter.First != -1 && GraphData.GridData[i].DisplayFilter.Second != -1)
                    {
                        if (GraphData.GridData[i].DisplayFilter.First > comparableValueHeight || comparableValueHeight > GraphData.GridData[i].DisplayFilter.Second)
                        {
                            FilteredDataEntryList[j] = true;
                            break;    
                        }
                    }
                }
            }
        }

        private int GetColumn(Point p)
        {
            for (int i = 0; i < GraphData.GridData.Count; ++i)
            {
                double halfWidth = CalculatedXStep / 2;

                if (GraphData.ColumnPositions[i].Top.X - halfWidth < p.X && p.X < GraphData.ColumnPositions[i].Top.X + halfWidth)
                {
                    return i;
                }
            }

            return 0;
        }

        // Returns the column index to the right of your cursor
        private int GetBetweenColumns(Point p)
        {
            //Special case: first column
            if (p.X < GraphData.ColumnPositions[0].Top.X)
            {
                return 0;
            }

            for (int i = 0; i < GraphData.GridData.Count - 1; ++i)
            {
                if (GraphData.ColumnPositions[i].Top.X < p.X && p.X < GraphData.ColumnPositions[i + 1].Top.X)
                {
                    return i + 1;
                }
            }

            return GraphData.GridData.Count;
        }

        private void DrawScreen()
        {
            canvas.Children.Clear();

            DrawColumns();
            DrawDatasetLines();
            DrawColumnDataPoints();
            DrawSettingsButton();
        }

        private void DrawColumns()
        {
            for (int i = 0; i < GraphData.GridData.Count; ++i)
            {
                canvas.Children.Add(DrawLine(new Pen(Brushes.DarkGray, 1), GraphData.ColumnPositions[i].Top, GraphData.ColumnPositions[i].Botumn));
                canvas.Children.Add(DrawText(GraphData.GridData[i].ColumnName, new Point(GraphData.ColumnPositions[i].Top.X - GraphData.GridData[i].ColumnName.Length * TEXT_OFFSET_X, -Y_COLUMN_OFFSET / 2 + (CalculatedXStep < 150 ? (i % 3) * 20 : 0)), Colors.Black));

                if (GraphData.GridData[i].FilteredColumn)
                {
                    Point topLeft = new Point(GraphData.ColumnPositions[i].Top.X - Math.Min(30, CalculatedXStep / 3), GraphData.GridData[i].DisplayFilter.First);
                    Point bottomRight = new Point(GraphData.ColumnPositions[i].Botumn.X + Math.Min(30, CalculatedXStep/3), GraphData.GridData[i].DisplayFilter.Second);
                    canvas.Children.Add(DrawRectangle(topLeft, bottomRight, Brushes.Black, Brushes.Transparent));
                }
            }
        }

        private void DrawColumnDataPoints()
        {
            for (int i = 0; i < GraphData.GridData.Count; ++i)
            {
                foreach (var field in GraphData.GridData[i].YPlacements)
                {
                    canvas.Children.Add(DrawLine(new Pen(Brushes.Black, 2), new Point(GraphData.ColumnPositions[i].Top.X - 10, field.Value), new Point(GraphData.ColumnPositions[i].Top.X + 10, field.Value)));
                    canvas.Children.Add(DrawText(field.Key, new Point(GraphData.ColumnPositions[i].Top.X - field.Key.Length * TEXT_OFFSET_X, field.Value + TEXT_OFFSET_Y), Colors.Black));
                }
            }
        }

        private void DrawDatasetLines()
        {
            PlacementDencity.Clear();

            var pointPositions = new List<List<Tuple<Point, Point>>>();

            for (int i = 0; i < GraphData.GridData[0].Data.Count; ++i)
            {
                pointPositions.Add(new List<Tuple<Point, Point>>());
                
                Point left = GetDataPointCoordinates(i, 0);
                Point right;

                for (int j = 1; j < GraphData.GridData.Count; ++j)
                {
                    right = GetDataPointCoordinates(i, j);

                    pointPositions[i].Add(Tuple.Create(left, right));

                    if (PlacementDencity.ContainsKey(pointPositions[i][j-1]))
                    {
                        ++PlacementDencity[pointPositions[i][j - 1]];
                    }
                    else
                    {
                        PlacementDencity[pointPositions[i][j - 1]] = 1;
                    }

                    left = right;
                }
            }

            for (int i = 0; i < pointPositions.Count; ++i)
            {
                if (!FilteredDataEntryList[i] || !HideFiltered)
                {
                    for (int j = 0; j < pointPositions[i].Count; ++j)
                    {
                        if (PlacementDencity[pointPositions[i][j]] == -1)
                        {
                            continue;
                        }

                        canvas.Children.Add(DrawLine(new Pen((FilteredDataEntryList[i] ? Brushes.LightGray : Brushes.DarkBlue), Math.Pow(PlacementDencity[pointPositions[i][j]], VariableLineThickness) / GraphData.GridData.Count * TotalLineThickness), pointPositions[i][j].Item1, pointPositions[i][j].Item2));

                        PlacementDencity[pointPositions[i][j]] = -1;
                    }
                }
            }
        }

        private void DrawSettingsButton()
        {
            canvas.Children.Add(SettingsBtn);
            canvas.Children.Add(SettingsGrid);

            SetGearHeight();
        }

        Point GetDataPointCoordinates(int i, int j)
        {
            // Case: Too many numeric points, so approximation must take place
            if (GraphData.GridData[j].AllNumbers && GraphData.GridData[j].UniquEntries > StartApproximation)
            {
                //  Case: No avalid entry provided
                if (GraphData.GridData[j].Data[i][0] == '[')
                {
                    return new Point(GraphData.ColumnPositions[j].Top.X, GraphData.GridData[j].YPlacements[EMPTY_FIELD]);
                }
                else // Case: Approximate location of numeric point
                {
                    // Point location based on range extreem values
                    double range = (GraphData.GridData[j].NumberRange.Second - GraphData.GridData[j].NumberRange.First);
                    double currentValue = Double.Parse(GraphData.GridData[j].Data[i]);
                    double heightPercentage = (currentValue - GraphData.GridData[j].NumberRange.First) / range;
                    double heightDistance = GraphData.GridData[j].YPlacements[GraphData.GridData[j].NumberRange.Second.ToString()] - GraphData.GridData[j].YPlacements[GraphData.GridData[j].NumberRange.First.ToString()];
                    double YVal = heightPercentage * heightDistance + GraphData.GridData[j].YPlacements[GraphData.GridData[j].NumberRange.First.ToString()];
                    return new Point(GraphData.ColumnPositions[j].Top.X, YVal);
                }
            }
            else // Case: No approximations needed 
            {
                string currentField = (GraphData.GridData[j].Data[i][0] == '[' ? EMPTY_FIELD : GraphData.GridData[j].Data[i]);
                return new Point(GraphData.ColumnPositions[j].Top.X, GraphData.GridData[j].YPlacements[currentField]);
            }
        }

        static void swap<T>(ref T left, ref T right)
        {
            T temp = left;
            left = right;
            right = temp;
        }

        private void CloseSettingsWindow(object sender, RoutedEventArgs e)
        {
            canvas.Background = Brushes.White;
            SettingsGrid.Visibility = Visibility.Hidden;
        }

        private void SetGearHeight()
        {
            // The setting gear's heigh fluctuates based on if the scrollbar is visible 
            // Here the gears upper margin is being changed depending on scrollbar visibility
            if ((int)SystemParameters.FullPrimaryScreenWidth < canvas.Width)
            {
                settingsGearHeight = 11.4;
            }
            else
            {
                settingsGearHeight = 3;
            }

            SettingsGrid.Margin = new Thickness(SettingsGrid.Margin.Left, settingsGearHeight, 0, 0);
            SettingsBtn.Margin = new Thickness(SettingsBtn.Margin.Left, settingsGearHeight, 0, 0);
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            if ((int)(MinXStep) != Int32.Parse(MinColumnWidthTxtBx.Text) || StartApproximation != Int32.Parse(BeginNumericAproxTxtBx.Text) || HideFiltered != (bool)FilterTxtBx.IsChecked || TotalLineThickness != Int32.Parse(TotalLineThicknessTxtBx.Text) || Math.Abs(VariableLineThickness - LineWidthVarianceSlider.Value) > 0.01)
            {
                MinXStep = Int32.Parse(MinColumnWidthTxtBx.Text);
                StartApproximation = Int32.Parse(BeginNumericAproxTxtBx.Text);
                TotalLineThickness = Int32.Parse(TotalLineThicknessTxtBx.Text);
                VariableLineThickness = LineWidthVarianceSlider.Value;

                CalculateCanvasDisplayLocations();

                HideFiltered = (bool)FilterTxtBx.IsChecked;

                SetFilters();
                DrawScreen();
            }
        }

        private void CalculateCanvasDisplayLocations()
        {
            FilteredDataEntryList = new List<bool>(new bool[GraphData.GridData[0].Data.Count]);

            canvas.Height = (int)SystemParameters.FullPrimaryScreenHeight - 17;
            canvas.Width = (int)SystemParameters.FullPrimaryScreenWidth;

            double normalWidth = (canvas.Width - BORDER_DISTANCE * 2) / GraphData.GridData.Count;
            CalculatedXStep = Math.Max(normalWidth, MinXStep);
            canvas.Width = CalculatedXStep * GraphData.GridData.Count + BORDER_DISTANCE * 2;

            // Calculate vertical Lines and text
            GraphData.ColumnPositions.Clear();
            for (int i = 0; i < GraphData.GridData.Count; ++i)
            {
                GraphData.ColumnPositions.Add(new ColumnData(new Point(CalculatedXStep * i + CalculatedXStep / 2 + BORDER_DISTANCE, BORDER_DISTANCE - Y_COLUMN_OFFSET), new Point(CalculatedXStep * i + CalculatedXStep / 2 + BORDER_DISTANCE, canvas.Height - BORDER_DISTANCE - Y_COLUMN_OFFSET)));
            }

            // Calculate horizontal Lines
            for (int i = 0; i < GraphData.GridData.Count; ++i)
            {
                if (GraphData.GridData[i].AllNumbers && GraphData.GridData[i].UniquEntries > StartApproximation)
                {
                    GraphData.GridData[i].YPlacements.Clear();

                    for (int j = 0; j < NUMERIC_POINTS; ++j)
                    {
                        double labelNumber = (j == 0 ? GraphData.GridData[i].NumberRange.First : (j == NUMERIC_POINTS - 1 ? GraphData.GridData[i].NumberRange.Second : (GraphData.GridData[i].NumberRange.Second - GraphData.GridData[i].NumberRange.First) * (j / (double)NUMERIC_POINTS) + GraphData.GridData[i].NumberRange.First));

                        GraphData.GridData[i].YPlacements[labelNumber.ToString()] = (int)(j / (NUMERIC_POINTS + (GraphData.GridData[i].ContainsEmptyEntry ? 1 : 0)) * (canvas.Height - 2 * BORDER_DISTANCE) + (0.5 / (NUMERIC_POINTS + (GraphData.GridData[i].ContainsEmptyEntry ? 1 : 0)) * (canvas.Height - 2 * BORDER_DISTANCE)) + BORDER_DISTANCE - Y_COLUMN_OFFSET);
                    }

                    if (GraphData.GridData[i].ContainsEmptyEntry)
                    {
                        GraphData.GridData[i].YPlacements[EMPTY_FIELD] = (int)((NUMERIC_POINTS) / (NUMERIC_POINTS + (GraphData.GridData[i].ContainsEmptyEntry ? 1 : 0)) * (canvas.Height - 2 * BORDER_DISTANCE) + (0.5 / (NUMERIC_POINTS + (GraphData.GridData[i].ContainsEmptyEntry ? 1 : 0)) * (canvas.Height - 2 * BORDER_DISTANCE)) + BORDER_DISTANCE - Y_COLUMN_OFFSET);
                    }
                }
                else
                {
                    var uniquValues = GraphData.GridData[i].Data.Distinct().Where(ee => ee[0] != '[').ToList();

                    SortList(ref uniquValues, GraphData.GridData[i].AllNumbers);

                    if (GraphData.GridData[i].ContainsEmptyEntry)
                    {
                        uniquValues.Add(EMPTY_FIELD);
                    }

                    for (int j = 0; j < uniquValues.Count; ++j)
                    {
                        GraphData.GridData[i].YPlacements[uniquValues[j]] = ((int)(j / (double)GraphData.GridData[i].UniquEntries * (canvas.Height - 2 * BORDER_DISTANCE) + (0.5 / (double)GraphData.GridData[i].UniquEntries * (canvas.Height - 2 * BORDER_DISTANCE)) + BORDER_DISTANCE - Y_COLUMN_OFFSET));
                    }
                }
            }
        }

        private void ShowGridSettings(object sender, RoutedEventArgs e)
        {
            SettingsGrid.Visibility = Visibility.Visible;
            canvas.Background = Brushes.Snow;

            MinColumnWidthTxtBx.Text = MinXStep.ToString();
            BeginNumericAproxTxtBx.Text = StartApproximation.ToString();
            TotalLineThicknessTxtBx.Text = TotalLineThickness.ToString();
            FilterTxtBx.IsChecked = HideFiltered;
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int val;
            e.Handled = !Int32.TryParse(e.Text, out val);
        }

        private void ValidateCount(object sender, RoutedEventArgs e)
        {
            // Fill textboxes is empty
            if (BeginNumericAproxTxtBx.Text == "")
            {
                BeginNumericAproxTxtBx.Text = StartApproximation.ToString();
            }

            if (TotalLineThicknessTxtBx.Text == "")
            {
                BeginNumericAproxTxtBx.Text = TotalLineThickness.ToString();
            }

            if (MinColumnWidthTxtBx.Text == "")
            {
                MinColumnWidthTxtBx.Text = ((int)MinXStep).ToString();
            }
        }

        public void OnScrollMove(object sender, RoutedEventArgs e)
        {
            UIElement canvasContainer = VisualTreeHelper.GetParent(canvas) as UIElement;
            Point relativeLocation = canvas.TranslatePoint(new Point(0, 0), canvasContainer);

            SettingsGrid.Margin = new Thickness(-relativeLocation.X + 10, settingsGearHeight, 0, 0);
            SettingsBtn.Margin = new Thickness(-relativeLocation.X + 10, settingsGearHeight, 0, 0);
        }
    }

    public class DisplayData
    {
        public DisplayData()
        {
            GridData = new List<DataEntry>();
            ColumnPositions = new List<ColumnData>();
        }

        public List<DataEntry> GridData { get; set; }
        public List<ColumnData> ColumnPositions { get; set; }
    }

    public class ColumnData
    {
        public ColumnData(Point top, Point botumn)
        {
            Top = top;
            Botumn = botumn;
        }

        public Point Top { get; set; }
        public Point Botumn { get; set; }
    }
}
